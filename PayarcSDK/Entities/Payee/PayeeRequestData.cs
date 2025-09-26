using Newtonsoft.Json;
using PayarcSDK.Entities;

namespace PayarcSDK.Entities
{
    public class PayeeRequestData
    {
        [JsonProperty("type")]
        public string type { get; set; }

        [JsonProperty("personal_info")]
        public PersonalInfo personal_info { get; set; }

        [JsonProperty("business_info")]
        public BusinessInfo business_info { get; set; }

        [JsonProperty("contact_info")]
        public ContactInfo contact_info { get; set; }

        [JsonProperty("address_info")]
        public AddressInfo address_info { get; set; }

        [JsonProperty("banking_info")]
        public BankingInfo banking_info { get; set; }

        [JsonProperty("foundation_date")]
        public string foundation_date { get; set; }

        [JsonProperty("date_incorporated")]
        public string date_incorporated { get; set; }

        public string ToJson() {
			var settings = new JsonSerializerSettings {
				NullValueHandling = NullValueHandling.Ignore,
				Formatting = Formatting.Indented
			};

			return JsonConvert.SerializeObject(this, settings);
		}
	}
    public class AddressInfo
    {
        [JsonProperty("street")]
        public string street { get; set; }

        [JsonProperty("city")]
        public string city { get; set; }

        [JsonProperty("zip_code")]
        public string zip_code { get; set; }

        [JsonProperty("county_code")]
        public string county_code { get; set; }
    }

    public class BankingInfo
    {
        [JsonProperty("dda")]
        public string dda { get; set; }

        [JsonProperty("routing")]
        public string routing { get; set; }
    }

    public class BusinessInfo
    {
        [JsonProperty("legal_name")]
        public string legal_name { get; set; }

        [JsonProperty("ein")]
        public string ein { get; set; }

        [JsonProperty("irs_filing_type")]
        public string irs_filing_type { get; set; }
    }

    public class ContactInfo
    {
        [JsonProperty("email")]
        public string email { get; set; }

        [JsonProperty("phone_number")]
        public string phone_number { get; set; }
    }

    public class PersonalInfo
    {
        [JsonProperty("first_name")]
        public string first_name { get; set; }

        [JsonProperty("last_name")]
        public string last_name { get; set; }

        [JsonProperty("ssn")]
        public string ssn { get; set; }

        [JsonProperty("dob")]
        public string dob { get; set; }
    }
}
