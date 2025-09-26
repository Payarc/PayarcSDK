using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayarcSDK.Entities.Payee
{
    public class PayeeListData : BaseResponse {
        [JsonProperty("object")]
        public override string Object { get; set; }

        [JsonProperty("object_id")]
        public override string? ObjectId {
            get {
                if (string.IsNullOrEmpty(Object) || string.IsNullOrEmpty(MerchantCode))
                    return null;
                return $"appy_{MerchantCode}";
            }
        }

        [JsonProperty("MerchantCode")]
        public string MerchantCode { get; set; }

        [JsonProperty("Status")]
        public int Status { get; set; }

        [JsonProperty("StatusDescription")]
        public string StatusDescription { get; set; }

        [JsonProperty("LeadCode")]
        public string LeadCode { get; set; }

        [JsonProperty("LeadStatus")]
        public string LeadStatus { get; set; }

        [JsonProperty("LeadModule")]
        public string LeadModule { get; set; }

        [JsonProperty("AccountCode")]
        public string AccountCode { get; set; }

        [JsonProperty("AccountMID")]
        public string AccountMID { get; set; }

        [JsonProperty("AccountStatus")]
        public string AccountStatus { get; set; }

        [JsonProperty("AchStatus")]
        public string AchStatus { get; set; }

        [JsonProperty("AppName")]
        public string AppName { get; set; }

        [JsonProperty("AppData")]
        public AppData AppData { get; set; }
    }
    public class AppData
    {
        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("amount_paid_ytd")]
        public string AmountPaidYtd { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("entity_type")]
        public string EntityType { get; set; }

        [JsonProperty("boarded_date")]
        public DateTime BoardedDate { get; set; }

        [JsonProperty("boarded_date_readable")]
        public string BoardedDateReadable { get; set; }
    }
}
