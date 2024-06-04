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
        [JsonPropertyName("Tên")]
        public string Name { get; set; }

        [JsonPropertyName("Số CMND/CCCD")]
        public string ID { get; set; }

        [JsonPropertyName("Số khoản vay")]
        public int LoanCount { get; set; }

        [JsonPropertyName("Số lần trễ hạn")]
        public int LatePaymentCount { get; set; }

        [JsonPropertyName("Số tiền nợ")]
        public int DebtAmount { get; set; }

        [JsonPropertyName("Giá trị tài sản")]
        public int AssetValue { get; set; }

        [JsonPropertyName("Thời gian sử dụng dịch vụ")]
        public int ServiceUsageTime { get; set; }

        [JsonPropertyName("Tổng thời gian từ lúc mở thẻ")]
        public int TotalTimeSinceCardOpened { get; set; }

        [JsonPropertyName("Số loại hình tín dụng")]
        public int CreditTypeCount { get; set; }

        [JsonPropertyName("Tổng loại hình tín dụng")]
        public int TotalCreditTypeCount { get; set; }

        [JsonPropertyName("Số tài khoản mới trong 1 tháng")]
        public int NewAccountsInMonth { get; set; }

        [JsonPropertyName("Tổng số tài khoản người dùng")]
        public int TotalUserAccounts { get; set; }
    }
}
