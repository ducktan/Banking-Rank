using Microsoft.Research.SEAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BankingRank.DAO
{
    public class MyData
    {
        [JsonPropertyName("Name")]
        public string Name { get; set; }

        [JsonPropertyName("CCCD")]
        public string CCCD { get; set; }

        [JsonPropertyName("LoanNum")]
        public int LoanCount { get; set; }
        
        [JsonPropertyName("LatePaymentCount")]
        public int LatePaymentCount { get; set; }

        [JsonPropertyName("DebtAmount")]
        public int DebtAmount { get; set; }

        [JsonPropertyName("AssetValue")]
        public int AssetValue { get; set; }

        [JsonPropertyName("ServiceUsageTime")]
        public int ServiceUsageTime { get; set; }

        [JsonPropertyName("TotalTimeSinceCardOpened ")]
        public int TotalTimeSinceCardOpened { get; set; }

        [JsonPropertyName("CreditTypeCount")]
        public int CreditTypeCount { get; set; }

        [JsonPropertyName("TotalCreditTypeCount")]
        public int TotalCreditTypeCount { get; set; }

        [JsonPropertyName("NewAccountsInMonth")]
        public int NewAccountsInMonth { get; set; }

        [JsonPropertyName("TotalUserAccounts")]
        public int TotalUserAccounts { get; set; } 
    }


    public class mydata_encrypt
    {
        public byte[] ID { get; set; }
        public byte[] Name { get; set; }
        public Ciphertext LoanCount { get; set; }
        public Ciphertext LatePaymentCount { get; set; }    
        public Ciphertext DebtAmount { get; set; }
        public Ciphertext AssetValue { get; set;}
        public Ciphertext ServiceUsageTime { get; set; }
        public Ciphertext TotalTimeSinceCardOpened { get;set; }
        public Ciphertext CreditTypeCount { get; set;}
        public Ciphertext TotalCreditTypeCount { get; set; }
        public Ciphertext NewAccountsInMonth { get;set; }
        public Ciphertext TotalUserAccounts { get; set; }

    }
}
