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



     

      

  
        private void button2_Click(object sender, EventArgs e)
        {
            
            // Đọc dữ liệu JSON từ file
            string jsonData = File.ReadAllText("test.json");
            List<MyData> creditData = Newtonsoft.Json.JsonConvert.DeserializeObject<List<MyData>>(jsonData);

            // Đọc public key từ file "pub.txt"
            string publicKeyHex = File.ReadAllText("pub.txt");
            byte[] publicKeyByte = Enumerable.Range(0, publicKeyHex.Length)
                                        .Where(x => x % 2 == 0)
                                        .Select(x => Convert.ToByte(publicKeyHex.Substring(x, 2), 16))
                                        .ToArray();

            // Đọc public key từ file "public_keyRSA.txt"
            string publicKeyHexRSA = File.ReadAllText("public_keyRSA.txt");
            // Chuyển đổi chuỗi hex thành mảng byte
            byte[] publicKeyBytes = new byte[publicKeyHexRSA.Length / 2];
            for (int i = 0; i < publicKeyHexRSA.Length; i += 2)
            {
                publicKeyBytes[i / 2] = Convert.ToByte(publicKeyHexRSA.Substring(i, 2), 16);
            }

            // Khởi tạo RSA từ modulus và exponent
            RSAParameters rsaParameters = new RSAParameters();
            rsaParameters.Modulus = publicKeyBytes; // Gán modulus từ chuỗi hex
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
                myItem.ID = encryptedAttributes[0];
                myItem.Name = encryptedAttributes[1];


                // Mã hóa số lượng khoản vay sử dụng bộ mã hóa đã cung cấp
                Plaintext plainData = new Plaintext();
                encoder.Encode(data.LoanCount, plainData);
                Ciphertext encryptedDataItem = new Ciphertext();
                encryptor.Encrypt(plainData, encryptedDataItem);
                myItem.LoanCount = encryptedDataItem;


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
                myItem.TotalUserAccounts = encryptedTotalUserAccounts;

                // Thêm myItem vào danh sách mã hóa
                encryptMyData.Add(myItem);
            }

            // Lưu dữ liệu mã hóa vào file JSON
            string jsonDataRe = Newtonsoft.Json.JsonConvert.SerializeObject(encryptMyData, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText("encrypted_data.json", jsonDataRe);






            

            MessageBox.Show("Encrypt successfully!");



            
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
                    if (prop.Value.Type == JTokenType.Integer)
                    {
                        long propValue = prop.Value.ToObject<long>();
                        if (propValue > int.MaxValue)
                        {
                            document[prop.Name] = new BsonDecimal128((decimal)propValue);
                        }
                        else
                        {
                            document[prop.Name] = (int)propValue;
                        }
                    }
                    else if (prop.Value.Type == JTokenType.Float)
                    {
                        document[prop.Name] = prop.Value.ToObject<double>();
                    }
                    else if (prop.Value.Type == JTokenType.Object)
                    {
                        // Xử lý trường lưu trữ object
                        if (prop.Name == "ParmsId")
                        {
                            // Xử lý trường ParmsId
                            BsonDocument parmsIdDoc = new BsonDocument();
                            JObject parmsIdObj = (JObject)prop.Value;
                            foreach (JProperty parmsIdProp in parmsIdObj.Properties())
                            {
                                if (parmsIdProp.Value.Type == JTokenType.Array)
                                {
                                    BsonArray parmsIdArray = new BsonArray();
                                    foreach (JToken item in parmsIdProp.Value)
                                    {
                                        parmsIdArray.Add(new BsonInt64(item.ToObject<long>()));
                                    }
                                    parmsIdDoc[parmsIdProp.Name] = parmsIdArray;
                                }
                                else
                                {
                                    parmsIdDoc[parmsIdProp.Name] = BsonValue.Create(parmsIdProp.Value);
                                }
                            }
                            document[prop.Name] = parmsIdDoc;
                        }
                        else
                        {
                            BsonDocument subdocument = new BsonDocument();
                            foreach (JProperty subProp in ((JObject)prop.Value).Properties())
                            {
                                if (subProp.Value.Type == JTokenType.Integer && subProp.Value.ToObject<long>() > int.MaxValue)
                                {
                                    subdocument[subProp.Name] = new BsonDecimal128(subProp.Value.ToObject<decimal>());
                                }
                                else
                                {
                                    subdocument[subProp.Name] = BsonValue.Create(subProp.Value);
                                }
                            }
                            document[prop.Name] = subdocument;
                        }
                    }
                    else if (prop.Value.Type == JTokenType.Array)
                    {
                        // Xử lý trường lưu trữ array
                        BsonArray bsonArray = new BsonArray();
                        foreach (JToken item in prop.Value)
                        {
                            if (item.Type == JTokenType.Integer && item.ToObject<long>() > int.MaxValue)
                            {
                                bsonArray.Add(new BsonDecimal128(item.ToObject<decimal>()));
                            }
                            else
                            {
                                bsonArray.Add(BsonValue.Create(item));
                            }
                        }
                        document[prop.Name] = bsonArray;
                    }
                    else
                    {
                        document[prop.Name] = BsonValue.Create(prop.Value);
                    }
                }
                collection.InsertOne(document);
            }
            MessageBox.Show("Dữ liệu đã được đẩy lên MongoDB.");
            
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            IMongoDatabase database = client.GetDatabase("Crypto");
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("CustomerTest");

            // Truy vấn dữ liệu từ MongoDB, không lấy trường "_id"
            var documents = collection.Find(FilterDefinition<BsonDocument>.Empty)
                                     .Project(Builders<BsonDocument>.Projection.Exclude("_id"))
                                     .ToList();

            // Chuyển đổi dữ liệu thành JSON array
            var jsonWriterSettings = new JsonWriterSettings
            {
                Indent = true,
                OutputMode = JsonOutputMode.Strict
            };

            var jsonData = "[\n" + string.Join(",\n", documents.Select(doc => doc.ToJson(jsonWriterSettings))) + "\n]";

            // Lưu dữ liệu JSON vào file
            File.WriteAllText("down.json", jsonData);

            MessageBox.Show("Dữ liệu đã được tải về file 'customer_data.json'.");
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
