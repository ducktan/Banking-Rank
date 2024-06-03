using System;
using System.Collections.Generic;
using System.IO;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Research.SEAL;

namespace BankingRank
{
    public partial class Form1 : Form
    {
        MongoClient client = new MongoClient("mongodb+srv://22521303:NDTan1303uit%3E%3E@cluster0.hhv63yx.mongodb.net/");

        public Form1()
        {
            InitializeComponent();


        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
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
            // Khởi tạo các tham số mã hóa
            var parms = new EncryptionParameters(SchemeType.CKKS);
            parms.PolyModulusDegree = 8192;
            parms.CoeffModulus = CoeffModulus.Create(8192, new int[] { 60, 40, 40, 60 });

            // Tạo ngữ cảnh mã hóa
            var context = new SEALContext(parms);

            // Khởi tạo key generator
            var keyGenerator = new KeyGenerator(context);
            var publicKey = keyGenerator.CreatePublicKey();
            var secretKey = keyGenerator.SecretKey;

            priText.Text = secretKey.ToString();
            pubText.Text = publicKey.ToString();
            MessageBox.Show("Gen key successfully!");
        }
    }
}
