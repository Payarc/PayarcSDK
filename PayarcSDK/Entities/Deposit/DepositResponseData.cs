using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PayarcSDK.Entities.Deposit {
	public class RowDataItem {
		[JsonProperty("processing_date")]
		public string ProcessingDate { get; set; }

		[JsonProperty("effective_date")]
		public string EffectiveDate { get; set; }

		[JsonProperty("transType")]
		public string TransType { get; set; }

		[JsonProperty("amount")]
		public double? Amount { get; set; }

		[JsonProperty("release_amount")]
		public double? ReleaseAmount { get; set; }

		[JsonProperty("batch_reference_number")]
		public string BatchReferenceNumber { get; set; }

		[JsonProperty("transaction_count")]
		public int? TransactionCount { get; set; }

		[JsonProperty("merchant_statement_id")]
		public string MerchantStatementId { get; set; }

		[JsonProperty("release_date")]
		public string ReleaseDate { get; set; }

		[JsonProperty("cb_reference_number")]
		public string CbReferenceNumber { get; set; }

		[JsonProperty("reserve_funding_department")]
		public string ReserveFundingDepartment { get; set; }

		[JsonProperty("sorting_reference_number")]
		public string SortingReferenceNumber { get; set; }

		[JsonProperty("line_item")]
		public string LineItem { get; set; }

		[JsonProperty("description")]
		public string Description { get; set; }
	}

	public class RowTotals {
		[JsonProperty("Amounts")]
		public double Amounts { get; set; }

		[JsonProperty("Transactions")]
		public int Transactions { get; set; }

		[JsonProperty("Settlement_Date")]
		public string SettlementDate { get; set; }
	}

	public class DepositDetail {
		[JsonProperty("row_data")]
		public List<RowDataItem> RowData { get; set; }

		[JsonProperty("row_totals")]
		public RowTotals RowTotals { get; set; }
	}

	public class DepositAccount : BaseResponse {
		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("object")]
		public string Object { get; set; }

		[JsonProperty("legal_name")]
		public string LegalName { get; set; }

		[JsonProperty("dba_name")]
		public string DbaName { get; set; }

		[JsonProperty("mid")]
		public string Mid { get; set; }

		[JsonProperty("deposits")]
		public Dictionary<string, DepositDetail> Deposits { get; set; }
	}

	public class Meta {
		[JsonProperty("include")]
		public List<object> Include { get; set; }

		[JsonProperty("custom")]
		public List<object> Custom { get; set; }
	}

	public class ResponseBody {
		[JsonProperty("data")]
		public List<DepositAccount> Data { get; set; }

		[JsonProperty("meta")]
		public Meta Meta { get; set; }
	}
}
