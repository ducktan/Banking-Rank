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
using System.Security.Cryptography.Xml;
using Newtonsoft.Json;
namespace BankingRank
{
    public partial class Form1 : Form
    {
        private static string ConvertCiphertextsToBase64(Ciphertext ciphertext)
        {
            StringBuilder base64StringBuilder = new StringBuilder();
            using (MemoryStream ms = new MemoryStream())
            {
                ciphertext.Save(ms);
                base64StringBuilder.Append(Convert.ToBase64String(ms.ToArray()));
            }

            return base64StringBuilder.ToString();
        }



        private static List<Ciphertext> ConvertBase64ToCiphertexts(List<string> base64List, SEALContext context)
        {
            List<Ciphertext> ciphertexts = new List<Ciphertext>(base64List.Count);
            foreach (var base64 in base64List)
            {
                Ciphertext ciphertext = new Ciphertext();
                byte[] bytes = Convert.FromBase64String(base64);
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    ciphertext.Load(context, ms);
                }
                ciphertexts.Add(ciphertext);
            }
            return ciphertexts;
        }



        

        private static void SaveKey(string fileName, Microsoft.Research.SEAL.PublicKey key)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                key.Save(fs);
            }
        }

        private static void SaveKey(string fileName, SecretKey key)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                key.Save(fs);
            }
        }

        private static Microsoft.Research.SEAL.PublicKey LoadPublicKey(string fileName, SEALContext context)
        {
            Microsoft.Research.SEAL.PublicKey publicKey = new Microsoft.Research.SEAL.PublicKey();
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                publicKey.Load(context, fs);
            }
            return publicKey;
        }

        private static SecretKey LoadSecretKey(string fileName, SEALContext context)
        {
            SecretKey secretKey = new SecretKey();
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                secretKey.Load(context, fs);
            }
            return secretKey;
        }

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





        private static byte[] EncryptString(string input)
        {
            // Implement the encryption logic for strings
            return System.Text.Encoding.UTF8.GetBytes(input); // This is just a placeholder
        }

        private static string EncryptDouble(CKKSEncoder encoder, Encryptor encryptor, double value, double scale)
        {
            Plaintext plainData = new Plaintext();
            encoder.Encode(value, scale, plainData);
            Ciphertext encryptedData = new Ciphertext();
            encryptor.Encrypt(plainData, encryptedData);

            return ConvertCiphertextToBase64(encryptedData);
        }
        private static string ConvertCiphertextToBase64(Ciphertext ciphertext)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                ciphertext.Save(ms);
                return Convert.ToBase64String(ms.ToArray());
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {

            // Đọc dữ liệu JSON từ file
            string jsonData = File.ReadAllText(textBox1.Text.ToString());
            List<MyData> creditData = Newtonsoft.Json.JsonConvert.DeserializeObject<List<MyData>>(jsonData);

           

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
            Microsoft.Research.SEAL.PublicKey publicKeySEAL = LoadPublicKey(textBox3.Text.ToString(), context);

            Encryptor encryptor = new Encryptor(context, publicKeySEAL);
            double scale = Math.Pow(2.0, 40);

            // Mã hóa dữ liệu
            List<Ciphertext> encryptedData = new List<Ciphertext>();
            List<mydata_encrypt> encryptMyData = new List<mydata_encrypt>();

            // Encrypting data for multiple individuals
            List<mydata_encrypt> encryptedIndividualsData = new List<mydata_encrypt>();
            foreach (var data in creditData)
            {
                List<string> base64Ciphertexts = new List<string>();
                mydata_encrypt myItem = new mydata_encrypt();
                List<string> attributes = new List<string>
                {
                    data.Name.ToString(),
                    data.CCCD.ToString()

                };
               
                
               
                Plaintext plain = new Plaintext();
                Ciphertext encrypted;
                double doubleValue;
                encoder.Encode(data.LatePaymentCount, scale, plain);
                encrypted = new Ciphertext();
                encryptor.Encrypt(plain, encrypted);
                base64Ciphertexts.Add(ConvertCiphertextToBase64(encrypted));

                doubleValue = data.LoanCount;
                encoder.Encode(1 / doubleValue, scale, plain);
                encrypted = new Ciphertext();
                encryptor.Encrypt(plain, encrypted);
                base64Ciphertexts.Add(ConvertCiphertextToBase64(encrypted));

                encoder.Encode(data.DebtAmount, scale, plain);
                encrypted = new Ciphertext();
                encryptor.Encrypt(plain, encrypted);
                base64Ciphertexts.Add(ConvertCiphertextToBase64(encrypted));

                doubleValue = data.AssetValue;
                encoder.Encode(1 / doubleValue, scale, plain);
                encrypted = new Ciphertext();
                encryptor.Encrypt(plain, encrypted);
                base64Ciphertexts.Add(ConvertCiphertextToBase64(encrypted));

                encoder.Encode(data.ServiceUsageTime, scale, plain);
                encrypted = new Ciphertext();
                encryptor.Encrypt(plain, encrypted);
                base64Ciphertexts.Add(ConvertCiphertextToBase64(encrypted));

                doubleValue = data.TotalTimeSinceCardOpened;
                encoder.Encode(1 / doubleValue, scale, plain);
                encrypted = new Ciphertext();
                encryptor.Encrypt(plain, encrypted);
                base64Ciphertexts.Add(ConvertCiphertextToBase64(encrypted));

                encoder.Encode(data.CreditTypeCount, scale, plain);
                encrypted = new Ciphertext();
                encryptor.Encrypt(plain, encrypted);
                base64Ciphertexts.Add(ConvertCiphertextToBase64(encrypted));

                doubleValue = data.TotalCreditTypeCount;
                encoder.Encode(1 / doubleValue, scale, plain);
                encrypted = new Ciphertext();
                encryptor.Encrypt(plain, encrypted);
                base64Ciphertexts.Add(ConvertCiphertextToBase64(encrypted));

                encoder.Encode(data.NewAccountsInMonth, scale, plain);
                encrypted = new Ciphertext();
                encryptor.Encrypt(plain, encrypted);
                base64Ciphertexts.Add(ConvertCiphertextToBase64(encrypted));

                doubleValue = data.TotalUserAccounts;
                encoder.Encode(1 / doubleValue, scale, plain);
                encrypted = new Ciphertext();
                encryptor.Encrypt(plain, encrypted);
                base64Ciphertexts.Add(ConvertCiphertextToBase64(encrypted));

                List<double> values_CCCD = Encoding.UTF8.GetBytes(data.CCCD).Select(b => (double)b).ToList();
                encoder.Encode(values_CCCD, scale, plain);
                encrypted = new Ciphertext();
                encryptor.Encrypt(plain, encrypted);
                base64Ciphertexts.Add(ConvertCiphertextToBase64(encrypted));

                List<double> values_name = Encoding.UTF8.GetBytes(data.Name).Select(b => (double)b).ToList();
                encoder.Encode(values_name, scale, plain);
                encrypted = new Ciphertext();
                encryptor.Encrypt(plain, encrypted);
                base64Ciphertexts.Add(ConvertCiphertextToBase64(encrypted));

                var encryptedDatabasetotext = new mydata_encrypt
                {
                    ID = data.ID,
                    CCCD = base64Ciphertexts[10],
                    Name = base64Ciphertexts[11],
                    LatePaymentCount = base64Ciphertexts[0],
                    LoanCount = base64Ciphertexts[1],
                    DebtAmount = base64Ciphertexts[2],
                    AssetValue = base64Ciphertexts[3],
                    ServiceUsageTime = base64Ciphertexts[4],
                    TotalTimeSinceCardOpened = base64Ciphertexts[5],
                    CreditTypeCount = base64Ciphertexts[6],
                    TotalCreditTypeCount = base64Ciphertexts[7],
                    NewAccountsInMonth = base64Ciphertexts[8],
                    TotalUserAccounts = base64Ciphertexts[9]
                };
                encryptedIndividualsData.Add(encryptedDatabasetotext);

            }

            // Serialize list of encrypted data objects to JSON
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(encryptedIndividualsData, Formatting.Indented);
            File.WriteAllText("data.json", json);
            MessageBox.Show("Encrypt successfully!");


            // Đẩy lên cloud
            IMongoDatabase database = client.GetDatabase("Crypto");
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("BankingInfo");

            // Đọc dữ liệu từ file JSON
            string jsonDataUp = File.ReadAllText("data.json");
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

        }

        private void button3_Click(object sender, EventArgs e)
        {

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

            // Save the keys to files
            SaveKey(pubName.Text.ToString(), publicKey);
            SaveKey(priName.Text.ToString(), secretKey);

            // Đọc khóa công khai và bí mật từ file
            string secret = File.ReadAllText(priName.Text.ToString());
            string publicK = File.ReadAllText(pubName.Text.ToString());

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

        private static Ciphertext ConvertBase64ToCiphertext(string base64, SEALContext context)
        {
            Ciphertext ciphertext = new Ciphertext();
            byte[] bytes = Convert.FromBase64String(base64);
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                ciphertext.Load(context, ms);
            }
            return ciphertext;
        }
        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                // Kết nối MongoDB
                var client = new MongoClient("mongodb+srv://thailam:uNj5rP3APTnXmDfX@cluster0.hhv63yx.mongodb.net/");
                var database = client.GetDatabase("Crypto");
                var collection = database.GetCollection<BsonDocument>("BankingInfo");

                var filter = Builders<BsonDocument>.Filter.Eq("ID", textBox5.Text.ToString());
                var results = collection.Find(filter).ToList();

                if (results.Count > 0)
                {
                    MessageBox.Show("ID found in database.");

                    var people = new List<mydata_encrypt>();

                    foreach (var result in results)
                    {
                        var person = new mydata_encrypt
                        {
                            ID = result["ID"].AsString,
                            CCCD = result["CCCD"].AsString,
                            Name = result["Name"].AsString,
                            LatePaymentCount = result["LatePaymentCount"].AsString,
                            LoanCount = result["LoanCount"].AsString,
                            DebtAmount = result["DebtAmount"].AsString,
                            AssetValue = result["AssetValue"].AsString,
                            ServiceUsageTime = result["ServiceUsageTime"].AsString,
                            TotalTimeSinceCardOpened = result["TotalTimeSinceCardOpened"].AsString,
                            CreditTypeCount = result["CreditTypeCount"].AsString,
                            TotalCreditTypeCount = result["TotalCreditTypeCount"].AsString,
                            NewAccountsInMonth = result["NewAccountsInMonth"].AsString,
                            TotalUserAccounts = result["TotalUserAccounts"].AsString,
                        };

                        people.Add(person);
                    }

                    // Chuyển đổi danh sách các đối tượng Person thành JSON
                    string json = System.Text.Json.JsonSerializer.Serialize(people, new JsonSerializerOptions { WriteIndented = true });

                    // Lưu JSON vào file
                    File.WriteAllText("data.json", json);
                }
            }
            catch { 
            }
            

               

            // Setting up encryption parameters
            EncryptionParameters parms = new EncryptionParameters(SchemeType.CKKS);
            ulong polyModulusDegree = 8192;
            parms.PolyModulusDegree = polyModulusDegree;
            parms.CoeffModulus = CoeffModulus.Create(polyModulusDegree, new int[] { 60, 40, 40, 60 });

            double scale = Math.Pow(2.0, 40);

            // Creating context
            SEALContext context = new SEALContext(parms);

            // Load the keys from files*/
            Microsoft.Research.SEAL.PublicKey loadedPublicKey = LoadPublicKey("public.key", context);
            SecretKey loadedSecretKey = LoadSecretKey("secret.key", context);

            Encryptor encryptor = new Encryptor(context, loadedPublicKey);
            Decryptor decryptor = new Decryptor(context, loadedSecretKey);
            Evaluator evaluator = new Evaluator(context);
            CKKSEncoder encoder = new CKKSEncoder(context);

            // Read the JSON file and deserialize
            string jsonFromFile = File.ReadAllText("data.json");
            var encryptedDataFromFile = Newtonsoft.Json.JsonConvert.DeserializeObject<List<mydata_encrypt>>(jsonFromFile);

            double[] rank = new double[] { 0.35, 0.3, 0.15, 0.1, 0.1 };
            // Encrypt rankPoint
            List<Ciphertext> cipherRank = new List<Ciphertext>(rank.Length);
            for (int i = 0; i < rank.Length; i++)
            {
                Plaintext plain1 = new Plaintext();
                encoder.Encode(rank[i], scale, plain1);
                Ciphertext encrypted = new Ciphertext();
                encryptor.Encrypt(plain1, encrypted);
                cipherRank.Add(encrypted);
            }

            Plaintext plain = new Plaintext();
            Ciphertext CCCD_search;
            double doubleValue;
            
            
            
            foreach (var encryptedData in encryptedDataFromFile)
            {
                

                
                    // Convert base64 strings back to Ciphertext objects
                    List<Ciphertext> ciphertextsFromJson = new List<Ciphertext>
                    {
                        ConvertBase64ToCiphertext(encryptedData.LatePaymentCount, context),
                        ConvertBase64ToCiphertext(encryptedData.LoanCount, context),
                        ConvertBase64ToCiphertext(encryptedData.DebtAmount, context),
                        ConvertBase64ToCiphertext(encryptedData.AssetValue, context),
                        ConvertBase64ToCiphertext(encryptedData.ServiceUsageTime, context),
                        ConvertBase64ToCiphertext(encryptedData.TotalTimeSinceCardOpened, context),
                        ConvertBase64ToCiphertext(encryptedData.CreditTypeCount, context),
                        ConvertBase64ToCiphertext(encryptedData.TotalCreditTypeCount, context),
                        ConvertBase64ToCiphertext(encryptedData.NewAccountsInMonth, context),
                        ConvertBase64ToCiphertext(encryptedData.TotalUserAccounts, context),
                    };

                    // Perform operations as an example (similar to before)
                    Ciphertext result1 = new Ciphertext();
                    Ciphertext result2 = new Ciphertext();
                    Ciphertext finalResult = new Ciphertext();

                    evaluator.Multiply(ciphertextsFromJson[0], ciphertextsFromJson[1], result1);
                    evaluator.Multiply(cipherRank[0], result1, result1);

                    evaluator.Multiply(ciphertextsFromJson[2], ciphertextsFromJson[3], result2);
                    evaluator.Multiply(cipherRank[1], result2, result2);

                    // Check scales before addition
                    if (result1.Scale != result2.Scale)
                    {
                        double newScale = Math.Pow(2.0, 40);
                        result1.Scale = newScale;
                        result2.Scale = newScale;
                    }

                    // Perform addition
                    evaluator.Add(result1, result2, finalResult);

                    evaluator.Multiply(ciphertextsFromJson[4], ciphertextsFromJson[5], result2);
                    evaluator.Multiply(cipherRank[2], result2, result2);

                    if (result2.Scale != finalResult.Scale)
                    {
                        double newScale = Math.Pow(2.0, 40);
                        result2.Scale = newScale;
                        finalResult.Scale = newScale;
                    }

                    evaluator.Add(finalResult, result2, finalResult);

                    evaluator.Multiply(ciphertextsFromJson[6], ciphertextsFromJson[7], result2);
                    evaluator.Multiply(cipherRank[3], result2, result2);

                    if (result2.Scale != finalResult.Scale)
                    {
                        double newScale = Math.Pow(2.0, 40);
                        result2.Scale = newScale;
                        finalResult.Scale = newScale;
                    }

                    evaluator.Add(finalResult, result2, finalResult);

                    evaluator.Multiply(ciphertextsFromJson[8], ciphertextsFromJson[9], result2);
                    evaluator.Multiply(cipherRank[4], result2, result2);

                    if (result2.Scale != finalResult.Scale)
                    {
                        double newScale = Math.Pow(2.0, 40);
                        result2.Scale = newScale;
                        finalResult.Scale = newScale;
                    }

                    evaluator.Add(finalResult, result2, finalResult);

                    // Decrypt and print result
                    Plaintext decrypted_result = new Plaintext();
                    decryptor.Decrypt(finalResult, decrypted_result);
                    List<double> decoded1 = new List<double>();
                    encoder.Decode(decrypted_result, decoded1);
                    double res = decoded1[0] * 1000; 
                    textBox8.Text = res.ToString();

                string mucDoRuiRo = "";

                if (res >= 710 && res <= 999)
                {
                    mucDoRuiRo = "Mức độ rủi ro hạng 1";
                }
                else if (res >= 680 && res <= 709)
                {
                    mucDoRuiRo = "Mức độ rủi ro hạng 2";
                }
                else if (res >= 635 && res <= 679)
                {
                    mucDoRuiRo = "Mức độ rủi ro hạng 3";
                }
                else if (res >= 610 && res <= 634)
                {
                    mucDoRuiRo = "Mức độ rủi ro hạng 4";
                }
                else if (res >= 560 && res <= 609)
                {
                    mucDoRuiRo = "Mức độ rủi ro hạng 5";
                }
                else if (res >= 520 && res <= 559)
                {
                    mucDoRuiRo = "Mức độ rủi ro hạng 6";
                }
                else if (res >= 420 && res <= 519)
                {
                    mucDoRuiRo = "Mức độ rủi ro hạng 7";
                }
                else if (res >= 1 && res <= 419)
                {
                    mucDoRuiRo = "Mức độ rủi ro hạng 8";
                }
                else
                {
                    mucDoRuiRo = "Không xác định";
                }

                richTextBox1.Text = mucDoRuiRo;

                // Decrypt and print CCCD
                Ciphertext cipher_CCCD = new Ciphertext();
                    cipher_CCCD = ConvertBase64ToCiphertext(encryptedData.CCCD, context);
                    // Giải mã Ciphertext trở lại Plaintext
                    Plaintext decryptedPlainText = new Plaintext();
                    decryptor.Decrypt(cipher_CCCD, decryptedPlainText);
                    // Giải mã mảng số thực thành chuỗi
                    List<double> decryptedValues = new List<double>();
                    encoder.Decode(decryptedPlainText, decryptedValues);
                    byte[] decodedBytes = decryptedValues.Select(v => (byte)Math.Round(v)).ToArray();
                    string decryptedText = Encoding.UTF8.GetString(decodedBytes);
                    textBox7.Text = decryptedText;

                    // Decrypt and print Name
                    Ciphertext cipher_Name = new Ciphertext();
                    cipher_Name = ConvertBase64ToCiphertext(encryptedData.Name, context);
                    // Giải mã Ciphertext trở lại Plaintext
                    decryptedPlainText = new Plaintext();
                    decryptor.Decrypt(cipher_Name, decryptedPlainText);
                    // Giải mã mảng số thực thành chuỗi
                    decryptedValues = new List<double>();
                    encoder.Decode(decryptedPlainText, decryptedValues);
                    byte[] decodedBytes_Name = decryptedValues.Select(v => (byte)Math.Round(v)).ToArray();
                    string decryptedText_name = Encoding.UTF8.GetString(decodedBytes_Name);
                    textBox6.Text = decryptedText_name;

                    
                    break;
                
                
            }
        }
    }
}
