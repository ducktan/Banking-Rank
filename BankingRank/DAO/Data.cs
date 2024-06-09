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
        public string ID { get; set; }
        public string Name { get; set; }
        public string CCCD { get; set; }

        public double LatePaymentCount { get; set; }
        public double LoanCount { get; set; }
        public double DebtAmount { get; set; }
        public double AssetValue { get; set; }
        public double ServiceUsageTime { get; set; }
        public double TotalTimeSinceCardOpened { get; set; }
        public double CreditTypeCount { get; set; }
        public double TotalCreditTypeCount { get; set; }
        public double NewAccountsInMonth { get; set; }
        public double TotalUserAccounts { get; set; }

    }


    public class mydata_encrypt
    {
        public string ID { get; set; }
        public string CCCD { get; set; }
        public string Name { get; set; }

        public string LatePaymentCount { get; set; }
        public string LoanCount { get; set; }
        public string DebtAmount { get; set; }
        public string AssetValue { get; set; }
        public string ServiceUsageTime { get; set; }
        public string TotalTimeSinceCardOpened { get; set; }
        public string CreditTypeCount { get; set; }
        public string TotalCreditTypeCount { get; set; }
        public string NewAccountsInMonth { get; set; }
        public string TotalUserAccounts { get; set; }

    }



    


}
