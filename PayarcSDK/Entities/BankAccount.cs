using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayarcSDK.Entities
{
    public class BankAccount {
		[JsonProperty("object")]
		public string Object { get; set; }

		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("first_name")]
		public string FirstName { get; set; }

		[JsonProperty("last_name")]
		public string LastName { get; set; }

		[JsonProperty("company_name")]
		public string CompanyName { get; set; }

		[JsonProperty("account_type")]
		public string AccountType { get; set; }

		[JsonProperty("sec_code")]
		public string SecCode { get; set; }

		[JsonProperty("routing_number")]
		public string RoutingNumber { get; set; }

		[JsonProperty("account_number")]
		public string AccountNumber { get; set; }

		[JsonProperty("is_default")]
		public int IsDefault { get; set; }
	}
}
