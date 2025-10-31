using Newtonsoft.Json;

namespace PayarcSDK.Entities.Batch {
	internal class BatchReportResponseData : BaseResponse {

		[JsonProperty("object")]
		public override string? Object { get; set; }

        [JsonProperty("object_id")]
        public override string? ObjectId
        {
            get
            {
                if (string.IsNullOrEmpty(Object) || string.IsNullOrEmpty(Id))
                    return null;
                return $"brn_{Id}";
            }
        }

        [JsonProperty("Merchant_Account_Number")]
		public string MerchantAccountNumber { get; set; }

		[JsonProperty("Settlement_Date")]
		public DateOnly SettlementDate { get; set; }

		[JsonProperty("ad_total_sale")]
		public decimal AdTotalSale { get; set; }

		[JsonProperty("ad_total_refunds")]
		public decimal AdTotalRefunds { get; set; }

		[JsonProperty("total_sale")]
		public decimal TotalSale { get; set; }

		[JsonProperty("Amounts")]
		public decimal Amounts { get; set; }

		[JsonProperty("total_refunds")]
		public decimal TotalRefunds { get; set; }

		[JsonProperty("rj_total_sale")]
		public decimal RjTotalSale { get; set; }

		[JsonProperty("rj_total_refunds")]
		public decimal RjTotalRefunds { get; set; }

		[JsonProperty("ad_net_amt")]
		public decimal AdNetAmt { get; set; }

		[JsonProperty("total_net_amt")]
		public decimal TotalNetAmt { get; set; }

		[JsonProperty("rj_net_amt")]
		public decimal RjNetAmt { get; set; }

		[JsonProperty("Transactions")]
		public int Transactions { get; set; }

		[JsonProperty("Batch_Reference_Number")]
		public string BatchReferenceNumber { get; set; }

		[JsonProperty("reject_record")]
		public string RejectRecord { get; set; }

		[JsonProperty("dba_name")]
		public string DbaName { get; set; }

		[JsonProperty("pfac_account_type")]
		public string PfacAccountType { get; set; }
	}
}
