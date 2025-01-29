using Newtonsoft.Json;

namespace PayarcSDK.Entities.CustomerService
{
    public class CustomerResponseData : BaseResponse {
		[JsonProperty("object")]
		public override string Object { get; set; }

		[JsonProperty("object_id")]
		public override string? ObjectId { get; set; }

		[JsonIgnore]
		public Func<CustomerInfoData?, Task<BaseResponse?>> Update { get; set; }

		[JsonProperty("customer_id")]
		public string CustomerId { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("email")]
		public string? Email { get; set; }

		[JsonProperty("description")]
		public string? Description { get; set; }

		[JsonProperty("payment_overdue")]
		public int? PaymentOverdue { get; set; }

		[JsonProperty("send_email_address")]
		public string? SendEmailAddress { get; set; }

		[JsonProperty("cc_email_address")]
		public string? CcEmailAddress { get; set; }

		[JsonProperty("source_id")]
		public string? SourceId { get; set; }

		[JsonProperty("address_1")]
		public string? Address1 { get; set; }

		[JsonProperty("address_2")]
		public string? Address2 { get; set; }

		[JsonProperty("city")]
		public string? City { get; set; }

		[JsonProperty("state")]
		public string? State { get; set; }

		[JsonProperty("zip")]
		public string? Zip { get; set; }

		[JsonProperty("phone")]
		public string? Phone { get; set; }

		[JsonProperty("country")]
		public string? Country { get; set; }

		[JsonProperty("created_at")]
		public long CreatedAt { get; set; }

		[JsonProperty("updated_at")]
		public long UpdatedAt { get; set; }

		[JsonProperty("readable_created_at")]
		public string ReadableCreatedAt { get; set; }

		[JsonProperty("readable_updated_at")]
		public string ReadableUpdatedAt { get; set; }

		[JsonProperty("invoice_prefix")]
		public string InvoicePrefix { get; set; }

		[JsonProperty("card")]
		public CardContainer? Card { get; set; }

		[JsonProperty("bank_account")]
		public BankAccountContainer? BankAccount { get; set; }

		[JsonProperty("charge")]
		public ChargeContainer? Charge { get; set; }

		[JsonIgnore]
		public CardsContainer? Cards { get; set; }

		[JsonIgnore]
		public BankAccountsContainer? Bank_Accounts { get; set; }

		[JsonIgnore]
		public ChargesContainer? Charges { get; set; }
	}

	public class CardContainer {
		[JsonProperty("data")]
		public List<Card> Data { get; set; }
	}

	public class BankAccountContainer {
		[JsonProperty("data")]
		public List<BankAccount> Data { get; set; }
	}

	public class ChargeContainer {
		[JsonProperty("data")]
		public List<object> Data { get; set; }
	}

	public class CardsContainer {

		[JsonIgnore]
		public Func<Dictionary<string, object>?, Task<BaseResponse?>> Create { get; set; }
	}

	public class BankAccountsContainer {

		[JsonIgnore]
		public Func<Dictionary<string, object>?, Task<BaseResponse?>> Create { get; set; }
	}

	public class ChargesContainer {

		[JsonIgnore]
		public Func<Dictionary<string, object>?, Task<BaseResponse?>> Create { get; set; }
	}
}
