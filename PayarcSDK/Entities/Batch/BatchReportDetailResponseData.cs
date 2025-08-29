using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PayarcSDK.Entities.Batch {
	internal class BatchReportDetailResponseData : BaseResponse {
		[JsonPropertyName("data")]
		public Dictionary<string, JsonElement> Data { get; set; }
	}

	// Represents a single transaction record within a batch.
	public class BatchData : BaseResponse {
		// Use JsonPropertyName to map snake_case JSON keys to PascalCase C# properties.
		[JsonProperty("Merchant_Account_Number")]
		public string MerchantAccountNumber { get; set; }

		[JsonProperty("account_type")]
		public string AccountType { get; set; }

		[JsonProperty("amount")]
		public int Amount { get; set; }

		[JsonProperty("card_number")]
		public string CardNumber { get; set; }

		[JsonProperty("trans_type")]
		public string TransType { get; set; }

		[JsonProperty("transaction_date")]
		public string TransactionDate { get; set; }

		[JsonProperty("pos_entry_mode")]
		public string PosEntryMode { get; set; }

		[JsonProperty("auth_code")]
		public string AuthCode { get; set; }

		[JsonProperty("charge_id")]
		public string ChargeId { get; set; }

		[JsonProperty("card_type")]
		public string CardType { get; set; }

		[JsonProperty("batch_ref_num")]
		public string BatchRefNum { get; set; }

		[JsonProperty("reject_record")]
		public object RejectRecord { get; set; }
	}

	// Represents the total amounts for a single batch.
	public class BatchTotal {
		[JsonPropertyName("Amounts")]
		public int Amounts { get; set; }
	}

	// Represents the total amounts for the entire response.
	public class GrandTotal {
		[JsonPropertyName("total_amount")]
		public int TotalAmount { get; set; }
	}

	// Represents the data structure for a single batch, identified by a reference number.
	public class BatchDetails {
		[JsonPropertyName("batch_data")]
		public List<BatchData> BatchData { get; set; }

		[JsonPropertyName("batch_total")]
		public BatchTotal BatchTotal { get; set; }
	}
}
