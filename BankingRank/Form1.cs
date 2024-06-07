using System;
using System.Collections.Generic;
using System.IO;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Research.SEAL;
using System.Text;
using BankingRank.DAO;
using System.Drawing.Imaging;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using ThirdParty.Json.LitJson;
using System.Buffers;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.Json;
using System.Xml.Linq;

namespace BankingRank
{
    public partial class Form1 : Form
    {

        MongoClient client = new MongoClient("mongodb+srv://22521303:NDTan1303uit%3E%3E@cluster0.hhv63yx.mongodb.net/");

        static void SaveKeyToFile(string filePath, string key)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(key);
            File.WriteAllText(filePath, BitConverter.ToString(bytes).Replace("-", ""));
        }

        public Form1()
        {
            InitializeComponent();


        }

        private void label10_Click(object sender, EventArgs e)
        {

        }



     

      

  
        private async void button2_Click(object sender, EventArgs e)
        {
            
            // Đọc dữ liệu JSON từ file
            string jsonData = File.ReadAllText(textBox1.Text);
            List<MyData> creditData = Newtonsoft.Json.JsonConvert.DeserializeObject<List<MyData>>(jsonData);

            // Đọc public key từ file "pub.txt"
            string publicKeyHex = File.ReadAllText(textBox3.Text);
            byte[] publicKeyByte = Enumerable.Range(0, publicKeyHex.Length)
                                        .Where(x => x % 2 == 0)
                                        .Select(x => Convert.ToByte(publicKeyHex.Substring(x, 2), 16))
                                        .ToArray();

            // Đọc public key từ file "public_keyRSA.txt"
            string publicKeyHexRSA = File.ReadAllText("public_keyRSA.txt");
            // Chuyển đổi chuỗi hex thành mảng byte
            byte[] publicKeyBytesRSA = new byte[publicKeyHexRSA.Length / 2];
            for (int i = 0; i < publicKeyHexRSA.Length; i += 2)
            {
                publicKeyBytesRSA[i / 2] = Convert.ToByte(publicKeyHexRSA.Substring(i, 2), 16);
            }

            // Khởi tạo RSA từ modulus và exponent
            RSAParameters rsaParameters = new RSAParameters();
            rsaParameters.Modulus = publicKeyBytesRSA; // Gán modulus từ chuỗi hex
            rsaParameters.Exponent = new byte[] { 0x01, 0x00, 0x01 }; // Giả sử một số exponent cụ thể, bạn cần thay đổi nếu cần
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(rsaParameters);

            // Thiết lập tham số mã hóa
            EncryptionParameters parms = new EncryptionParameters(SchemeType.CKKS);
            ulong polyModulusDegree = 8192;
            parms.PolyModulusDegree = polyModulusDegree;
            parms.CoeffModulus = CoeffModulus.Create(polyModulusDegree, new int[] { 60, 40, 40, 60 });

            // Tạo ngữ cảnh mã hóa
            SEALContext context = new SEALContext(parms);
            // Tạo CKKSEncoder thay vì BatchEncoder
            CKKSEncoder encoder = new CKKSEncoder(context);

            // Tạo PublicKey mới và đọc khóa công khai từ byte array
            Microsoft.Research.SEAL.PublicKey publicKeySEAL = new Microsoft.Research.SEAL.PublicKey();
            using (var ms = new MemoryStream(publicKeyByte))
            {
                publicKeySEAL.Load(context, ms);
            }


            Encryptor encryptor = new Encryptor(context, publicKeySEAL);

            // Mã hóa dữ liệu
            List<Ciphertext> encryptedData = new List<Ciphertext>();
            List<mydata_encrypt> encryptMyData = new List<mydata_encrypt>();
            foreach (var data in creditData)
            {
                mydata_encrypt myItem = new mydata_encrypt();
                List<string> attributes = new List<string>
                {
                    data.Name.ToString(),
                    data.CCCD.ToString()

                };
                List<byte[]> encryptedAttributes = new List<byte[]>();
                for (int i = 0; i < attributes.Count; i++)
                {

                    // Mã hóa dữ liệu sử dụng RSA với public key
                    byte[] encryptedBytes = rsa.Encrypt(Encoding.UTF8.GetBytes(attributes[i]), false);
                    encryptedAttributes.Add(encryptedBytes);
                }
                myItem.CCCD = encryptedAttributes[0];
                myItem.Name = encryptedAttributes[1];


                // Mã hóa số lượng khoản vay sử dụng bộ mã hóa đã cung cấp
                Plaintext plainData = new Plaintext();
                encoder.Encode(data.LoanCount, plainData);
                Ciphertext encryptedDataItem = new Ciphertext();
                encryptor.Encrypt(plainData, encryptedDataItem);
                myItem.LoanCount = encryptedDataItem;

                /*
                // Mã hóa số lượng khoản trả chậm
                Plaintext plainLatePaymentCount = new Plaintext();
                encoder.Encode(data.LatePaymentCount, plainLatePaymentCount);
                Ciphertext encryptedLatePaymentCount = new Ciphertext();
                encryptor.Encrypt(plainLatePaymentCount, encryptedLatePaymentCount);
                myItem.LatePaymentCount = encryptedLatePaymentCount;

                // Mã hóa số tiền nợ
                Plaintext plainDebtAmount = new Plaintext();
                encoder.Encode(data.DebtAmount, plainDebtAmount);
                Ciphertext encryptedDebtAmount = new Ciphertext();
                encryptor.Encrypt(plainDebtAmount, encryptedDebtAmount);
                myItem.DebtAmount = encryptedDebtAmount;

                // Mã hóa giá trị tài sản
                Plaintext plainAssetValue = new Plaintext();
                encoder.Encode(data.AssetValue, plainAssetValue);
                Ciphertext encryptedAssetValue = new Ciphertext();
                encryptor.Encrypt(plainAssetValue, encryptedAssetValue);
                myItem.AssetValue = encryptedAssetValue;

                // Mã hóa thời gian sử dụng dịch vụ
                Plaintext plainServiceUsageTime = new Plaintext();
                encoder.Encode(data.ServiceUsageTime, plainServiceUsageTime);
                Ciphertext encryptedServiceUsageTime = new Ciphertext();
                encryptor.Encrypt(plainServiceUsageTime, encryptedServiceUsageTime);
                myItem.ServiceUsageTime = encryptedServiceUsageTime;

                // Mã hóa tổng thời gian kể từ khi mở thẻ
                Plaintext plainTotalTimeSinceCardOpened = new Plaintext();
                encoder.Encode(data.TotalTimeSinceCardOpened, plainTotalTimeSinceCardOpened);
                Ciphertext encryptedTotalTimeSinceCardOpened = new Ciphertext();
                encryptor.Encrypt(plainTotalTimeSinceCardOpened, encryptedTotalTimeSinceCardOpened);
                myItem.TotalTimeSinceCardOpened = encryptedTotalTimeSinceCardOpened;

                // Mã hóa số loại tín dụng
                Plaintext plainCreditTypeCount = new Plaintext();
                encoder.Encode(data.CreditTypeCount, plainCreditTypeCount);
                Ciphertext encryptedCreditTypeCount = new Ciphertext();
                encryptor.Encrypt(plainCreditTypeCount, encryptedCreditTypeCount);
                myItem.CreditTypeCount = encryptedCreditTypeCount;

                // Mã hóa tổng số loại tín dụng
                Plaintext plainTotalCreditTypeCount = new Plaintext();
                encoder.Encode(data.TotalCreditTypeCount, plainTotalCreditTypeCount);
                Ciphertext encryptedTotalCreditTypeCount = new Ciphertext();
                encryptor.Encrypt(plainTotalCreditTypeCount, encryptedTotalCreditTypeCount);
                myItem.TotalCreditTypeCount = encryptedTotalCreditTypeCount;

                // Mã hóa số tài khoản mới trong tháng
                Plaintext plainNewAccountsInMonth = new Plaintext();
                encoder.Encode(data.NewAccountsInMonth, plainNewAccountsInMonth);
                Ciphertext encryptedNewAccountsInMonth = new Ciphertext();
                encryptor.Encrypt(plainNewAccountsInMonth, encryptedNewAccountsInMonth);
                myItem.NewAccountsInMonth = encryptedNewAccountsInMonth;

                // Mã hóa tổng số tài khoản người dùng
                Plaintext plainTotalUserAccounts = new Plaintext();
                encoder.Encode(data.TotalUserAccounts, plainTotalUserAccounts);
                Ciphertext encryptedTotalUserAccounts = new Ciphertext();
                encryptor.Encrypt(plainTotalUserAccounts, encryptedTotalUserAccounts);
                myItem.TotalUserAccounts = encryptedTotalUserAccounts;*/

                // Thêm myItem vào danh sách mã hóa
                encryptMyData.Add(myItem);
            }

            // Lưu dữ liệu mã hóa vào file JSON
            string jsonDataRe = Newtonsoft.Json.JsonConvert.SerializeObject(encryptMyData, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText("encrypted_data.json", jsonDataRe);



            


            

            MessageBox.Show("Encrypt successfully!");


            /*
            
            // Đẩy lên cloud
            IMongoDatabase database = client.GetDatabase("Crypto");
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("Test");

            // Đọc dữ liệu từ file JSON
            string jsonDataUp = File.ReadAllText("encrypted_data.json");
            JArray jsonArray = JArray.Parse(jsonDataUp);

            foreach (JObject obj in jsonArray)
            {
                BsonDocument document = new BsonDocument();
                foreach (JProperty prop in obj.Properties())
                {
                    // Convert the value to the correct data type before saving to MongoDB
                    if (prop.Value.Type == JTokenType.Integer && prop.Value.ToObject<long>() > int.MaxValue)
                    {
                        document[prop.Name] = new BsonDecimal128(prop.Value.ToObject<decimal>());
                    }
                    else if (prop.Value.Type == JTokenType.Boolean)
                    {
                        document[prop.Name] = prop.Value.ToObject<bool>();
                    }
                    else if (prop.Value.Type == JTokenType.Date)
                    {
                        document[prop.Name] = prop.Value.ToObject<DateTime>();
                    }
                    else if (prop.Value.Type == JTokenType.Object)
                    {
                        JObject childObject = (JObject)prop.Value;
                        BsonDocument childDocument = new BsonDocument();
                        foreach (JProperty childProp in childObject.Properties())
                        {
                            if (childProp.Value.Type == JTokenType.Object)
                            {
                                // Handle the "Pool" object
                                BsonDocument poolDocument = new BsonDocument();
                                JObject poolObject = (JObject)childProp.Value;
                                foreach (JProperty poolProp in poolObject.Properties())
                                {
                                    if (poolProp.Value.Type == JTokenType.Array)
                                    {
                                        // Handle array properties within the "Pool" object
                                        BsonArray arrayValue = new BsonArray();
                                        JArray arrayToken = (JArray)poolProp.Value;
                                        foreach (JToken item in arrayToken)
                                        {
                                            if (item.Type == JTokenType.Integer)
                                            {
                                                // Convert System.Numerics.BigInteger to BsonDecimal
                                                arrayValue.Add(BsonValue.Create(item.ToObject<decimal>()));
                                            }
                                            else
                                            {
                                                arrayValue.Add(BsonValue.Create(item.ToObject<object>()));
                                            }
                                        }
                                        poolDocument[poolProp.Name] = arrayValue;
                                    }
                                    else
                                    {
                                        if (poolProp.Value.Type == JTokenType.Integer)
                                        {
                                            // Convert System.Numerics.BigInteger to BsonDecimal
                                            poolDocument[poolProp.Name] = BsonValue.Create(poolProp.Value.ToObject<decimal>());
                                        }
                                        else
                                        {
                                            poolDocument[poolProp.Name] = BsonValue.Create(poolProp.Value.ToObject<object>());
                                        }
                                    }
                                }
                                childDocument[childProp.Name] = poolDocument;
                            }
                            else
                            {
                                if (childProp.Value.Type == JTokenType.Integer)
                                {
                                    // Convert System.Numerics.BigInteger to BsonDecimal
                                    childDocument[childProp.Name] = BsonValue.Create(childProp.Value.ToObject<decimal>());
                                }
                                else
                                {
                                    childDocument[childProp.Name] = BsonValue.Create(childProp.Value.ToObject<object>());
                                }
                            }
                        }
                        document[prop.Name] = childDocument;
                    }
                    else
                    {
                        document[prop.Name] = BsonValue.Create(prop.Value.ToObject<object>());
                    }
                }

                // Use BulkWrite to push data to MongoDB
                var result = await collection.BulkWriteAsync(new List<WriteModel<BsonDocument>>
                {
                    new InsertOneModel<BsonDocument>(document)
                });
            }
            MessageBox.Show("Dữ liệu đã được đẩy lên MongoDB.");
            */
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {/*
            IMongoDatabase database = client.GetDatabase("Crypto");
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("Test");

            // Truy vấn dữ liệu từ MongoDB, không lấy trường "_id"
            var documents = collection.Find(FilterDefinition<BsonDocument>.Empty)
                                     .Project(Builders<BsonDocument>.Projection.Exclude("_id"))
                                     .ToList();

            // Định dạng lại JSON output
            var jsonWriterSettings = new JsonWriterSettings
            {
                Indent = true,
                OutputMode = JsonOutputMode.Strict
            };
            static string FormatBsonDocument(BsonDocument doc, JsonWriterSettings settings)
            {
                var json = doc.ToJson(settings);

                // Định dạng lại các trường có dạng "$numberDecimal"
                json = Regex.Replace(json, @"\{\s*""?\$numberDecimal""?\s*:\s*""?([-+]?\d+(?:\.\d+)?)""?\s*\}", m => $"{m.Groups[1].Value}");

                return json;
            }

            var jsonData = "[\n" + string.Join(",\n", documents.Select(doc => FormatBsonDocument(doc, jsonWriterSettings))) + "\n]";

           
            // Lưu dữ liệu JSON vào file
            File.WriteAllText("down.json", jsonData);



            MessageBox.Show("Dữ liệu đã được tải về file 'customer_data.json'.");
            */

            // Thiết lập tham số mã hóa
            EncryptionParameters parms = new EncryptionParameters(SchemeType.CKKS);
            ulong polyModulusDegree = 8192;
            parms.PolyModulusDegree = polyModulusDegree;
            parms.CoeffModulus = CoeffModulus.Create(polyModulusDegree, new int[] { 60, 40, 40, 60 });

            // Tạo ngữ cảnh mã hóa
            SEALContext context = new SEALContext(parms);
            // Tạo CKKSEncoder thay vì BatchEncoder
            CKKSEncoder encoder = new CKKSEncoder(context);

            // Đọc private key từ file "pri.txt"
            string prikeyHex = File.ReadAllText("pri.txt");
            byte[] prikeyByteSEAL = Enumerable.Range(0, prikeyHex.Length)
                                        .Where(x => x % 2 == 0)
                                        .Select(x => Convert.ToByte(prikeyHex.Substring(x, 2), 16))
                                        .ToArray();

            // Đọc private key từ file "private_keyRSA.txt"
            string privateKeyHexRSA = File.ReadAllText("private_keyRSA.txt");
            // Chuyển đổi chuỗi hex thành mảng byte
            byte[] privateKeyBytesRSA = new byte[privateKeyHexRSA.Length / 2];
            for (int i = 0; i < privateKeyHexRSA.Length; i += 2)
            {
                privateKeyBytesRSA[i / 2] = Convert.ToByte(privateKeyHexRSA.Substring(i, 2), 16);
            }

            // Khởi tạo RSA từ modulus và exponent
            RSAParameters rsaParameters = new RSAParameters();
            rsaParameters.Modulus = privateKeyBytesRSA; // Gán modulus từ chuỗi hex
            rsaParameters.Exponent = new byte[] { 0x01, 0x00, 0x01 }; // Giả sử exponent cụ thể, bạn cần thay đổi nếu cần

            // Khởi tạo RSACryptoServiceProvider và nhập private key
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(rsaParameters);



            // Tạo PublicKey mới và đọc khóa công khai từ byte array
            Microsoft.Research.SEAL.SecretKey privateSEAL = new Microsoft.Research.SEAL.SecretKey();
            using (var ms = new MemoryStream(prikeyByteSEAL))
            {
                privateSEAL.Load(context, ms);
            }

            Decryptor decryptor = new Decryptor(context, privateSEAL);


            // Decrypt
            string jsonData = File.ReadAllText("down.json");
          
            // Deserialize dữ liệu từ JSON trở về Ciphertext
            List<mydata_encrypt> decryptedCiphertext = Newtonsoft.Json.JsonConvert.DeserializeObject<List<mydata_encrypt>>(jsonData);
            List<MyData> resultDecrypt = new List<MyData>();
            foreach (mydata_encrypt itemEncrypt in decryptedCiphertext)
            {
                MyData resultItem = new MyData();
                byte[] nametoEn = itemEncrypt.Name;
                byte[] CCCDtoEn = itemEncrypt.CCCD;

                byte[] decryptedBytesName = rsa.Decrypt(nametoEn, true);
                string nameDe = System.Text.Encoding.UTF8.GetString(decryptedBytesName);

                byte[] decryptedBytesCCCD = rsa.Decrypt(CCCDtoEn, false);
                string CCCDDe = System.Text.Encoding.UTF8.GetString(decryptedBytesCCCD);
               
                // Giải mã LoanCount
                Ciphertext ciphertextLoanCount = new Ciphertext(itemEncrypt.LoanCount);
                Plaintext plaintextLoanCount = new Plaintext();
                decryptor.Decrypt(ciphertextLoanCount, plaintextLoanCount);


                // Trích xuất giá trị của LoanCount
                List<double> loanCountList = new List<double>();
                encoder.Decode(plaintextLoanCount, loanCountList, null);
                int loanCount = (int)loanCountList[0];

                resultItem.Name = "Decrypt";
                resultItem.LoanCount = loanCount;
                resultItem.CCCD = "Decrypt";

                resultDecrypt.Add(resultItem);



            }


            // Cấu hình JsonSerializerOptions
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            // Serialize đối tượng MyData thành chuỗi JSON
            string jsonDataOut = System.Text.Json.JsonSerializer.Serialize(resultDecrypt, options);

            // Ghi chuỗi JSON vào file
            File.WriteAllText("output.json", jsonData);




            MessageBox.Show("Success");

            
            
           
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SaveFileDialog ofd = new SaveFileDialog();
            ofd.ShowDialog(this);

            string fileName1 = System.IO.Path.GetFileName(ofd.FileName);
            textBox2.Text = fileName1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog(this);

            string fileName1 = System.IO.Path.GetFileName(ofd.FileName);
            textBox1.Text = fileName1;


        }

        private void button7_Click(object sender, EventArgs e)
        {
            // RSA
            // Generate RSA key pair
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048);
            string publicKeyRSA = rsa.ToXmlString(false);
            string privateKeyRSA = rsa.ToXmlString(true);

            // Save public and private keys to files in Hex format
            SaveKeyToFile("public_keyRSA.txt", publicKeyRSA);
            SaveKeyToFile("private_keyRSA.txt", privateKeyRSA);



            // Thiết lập tham số mã hóa
            EncryptionParameters parms = new EncryptionParameters(SchemeType.CKKS);
            ulong polyModulusDegree = 8192;
            parms.PolyModulusDegree = polyModulusDegree;
            parms.CoeffModulus = CoeffModulus.Create(polyModulusDegree, new int[] { 60, 40, 40, 60 });

            // Tạo ngữ cảnh mã hóa
            SEALContext context = new SEALContext(parms);

            // Tạo khóa công khai, khóa bí mật và mã hóa
            KeyGenerator keygen = new KeyGenerator(context);
            Microsoft.Research.SEAL.PublicKey publicKey;
            keygen.CreatePublicKey(out publicKey);
            SecretKey secretKey = keygen.SecretKey;

            // Lưu khóa công khai và bí mật vào file dưới dạng Hex
            using (var ms = new MemoryStream())
            {
                publicKey.Save(ms);
                string publicKeyHex = BitConverter.ToString(ms.ToArray()).Replace("-", "");

                ms.Position = 0;
                secretKey.Save(ms);
                string secretKeyHex = BitConverter.ToString(ms.ToArray()).Replace("-", "");

                File.WriteAllText(priName.Text, secretKeyHex);
                File.WriteAllText(pubName.Text, publicKeyHex);
            }

            // Đọc khóa công khai và bí mật từ file
            string secret = File.ReadAllText(priName.Text);
            string publicK = File.ReadAllText(pubName.Text);

            // Hiển thị khóa công khai và bí mật lên RichTextBox
            priText.Text = secret;
            pubText.Text = publicK;

            MessageBox.Show("Gen key successfully (RSA Default - HE)!");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog(this);

            string fileName1 = System.IO.Path.GetFileName(ofd.FileName);
            textBox3.Text = fileName1;
        }
    }
}
