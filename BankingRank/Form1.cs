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

            // Đọc public key từ file <SEAL>
            string publicKeyHex = File.ReadAllText(textBox3.Text);
            byte[] publicKeyByte = Enumerable.Range(0, publicKeyHex.Length)
                                        .Where(x => x % 2 == 0)
                                        .Select(x => Convert.ToByte(publicKeyHex.Substring(x, 2), 16))
                                        .ToArray();

            // Đọc public key từ file <SEAL>
            string publicKeyHexRSA = File.ReadAllText("public_keyRSA.txt");
            byte[] publicKeyRSA = Enumerable.Range(0, publicKeyHexRSA.Length)
                                        .Where(x => x % 2 == 0)
                                        .Select(x => Convert.ToByte(publicKeyHexRSA.Substring(x, 2), 16))
                                        .ToArray();

            // Tạo RSACryptoServiceProvider với public key
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.ImportRSAPublicKey(publicKeyRSA, out _);

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
            List<MyData> encryptMyData = new List<MyData>();  
            foreach (var data in creditData)
            {
                MyData myItem = new MyData();
                List<string> attributes = new List<string>
                {
                    data.Name.ToString(),
                    data.ID.ToString()
                };

                for (int i = 0; i < attributes.Count; i++)
                {
                    // Mã hóa dữ liệu sử dụng RSA với public key
                    byte[] encryptedBytes = rsa.Encrypt(Encoding.UTF8.GetBytes(attributes[i]), false);
                    
                }
                myItem.ID = attributes[0];
                myItem.Name = attributes[1];




                // Mã hóa số lượng khoản vay sử dụng bộ mã hóa đã cung cấp
                Plaintext plainData = new Plaintext();
                encoder.Encode(data.LoanCount, plainData);
                Ciphertext encryptedDataItem = new Ciphertext();
                encryptor.Encrypt(plainData, encryptedDataItem);
                myItem.LoanCount = encryptedDataItem;



                encryptMyData.Add(myItem);
            }

            // Lưu dữ liệu mã hóa vào file JSON
            string jsonDataRe = Newtonsoft.Json.JsonConvert.SerializeObject(encryptedData, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText("encrypted_data.json", jsonDataRe);








            MessageBox.Show("Encrypt successfully!");



            /*
            // Đẩy lên cloud
            IMongoDatabase database = client.GetDatabase("Crypto");
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("CustomerTest");

            // Đọc dữ liệu từ file JSON
            string jsonData = File.ReadAllText("test.json");
            JArray jsonArray = JArray.Parse(jsonData);

            // Duyệt từng đối tượng JSON và đẩy lên MongoDB
            foreach (JObject obj in jsonArray)
            {
                BsonDocument document = BsonDocument.Parse(obj.ToString());
                collection.InsertOne(document);
            }

            MessageBox.Show("Dữ liệu đã được đẩy lên MongoDB.");
            */
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
