using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PayarcSDK.Entities.Batch {
	internal class BatchReportResponseData : BaseResponse {

		[JsonProperty("object")]
		public override string? Object { get; set; }

		[JsonProperty("object_id")]
		public override string? ObjectId { get; set; }

		[JsonPropertyName("Merchant_Account_Number")]
		public string MerchantAccountNumber { get; set; }

		[JsonPropertyName("Settlement_Date")]
		public DateOnly SettlementDate { get; set; }

		[JsonPropertyName("ad_total_sale")]
		public decimal AdTotalSale { get; set; }

		[JsonPropertyName("ad_total_refunds")]
		public decimal AdTotalRefunds { get; set; }

		[JsonPropertyName("total_sale")]
		public decimal TotalSale { get; set; }

		[JsonPropertyName("Amounts")]
		public decimal Amounts { get; set; }

		[JsonPropertyName("total_refunds")]
		public decimal TotalRefunds { get; set; }

		[JsonPropertyName("rj_total_sale")]
		public decimal RjTotalSale { get; set; }

		[JsonPropertyName("rj_total_refunds")]
		public decimal RjTotalRefunds { get; set; }

		[JsonPropertyName("ad_net_amt")]
		public decimal AdNetAmt { get; set; }

		[JsonPropertyName("total_net_amt")]
		public decimal TotalNetAmt { get; set; }

		[JsonPropertyName("rj_net_amt")]
		public decimal RjNetAmt { get; set; }

		[JsonPropertyName("Transactions")]
		public int Transactions { get; set; }

		[JsonPropertyName("Batch_Reference_Number")]
		public string BatchReferenceNumber { get; set; }

		[JsonPropertyName("reject_record")]
		public string RejectRecord { get; set; }

		[JsonPropertyName("dba_name")]
		public string DbaName { get; set; }

		[JsonPropertyName("pfac_account_type")]
		public string PfacAccountType { get; set; }
	}
}
