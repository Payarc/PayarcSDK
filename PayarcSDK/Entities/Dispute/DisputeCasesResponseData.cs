using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayarcSDK.Entities.Dispute
{
    public class DisputeCasesResponseData : BaseResponse {
		[JsonProperty("object")]
		public Object Object { get; set; }

		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("full_reason_code")]
		public string FullReasonCode { get; set; }

		[JsonProperty("item_type")]
		public string ItemType { get; set; }

		[JsonProperty("merchant_number")]
		public string MerchantNumber { get; set; }

		[JsonProperty("resolution_to")]
		public string? ResolutionTo { get; set; }

		[JsonProperty("debit_credit")]
		public string DebitCredit { get; set; }

		[JsonProperty("trans_code")]
		public string TransCode { get; set; }

		[JsonProperty("reason_code")]
		public string ReasonCode { get; set; }

		[JsonProperty("bin_ica")]
		public string BinIca { get; set; }

		[JsonProperty("record_type")]
		public string RecordType { get; set; }

		[JsonProperty("card_brand")]
		public string CardBrand { get; set; }

		[JsonProperty("date_resolved")]
		public DateTimeOffset? DateResolved { get; set; }

		[JsonProperty("acquirer_reference_number")]
		public string AcquirerReferenceNumber { get; set; }

		[JsonProperty("original_reference_number")]
		public string? OriginalReferenceNumber { get; set; }

		[JsonProperty("foreign_domestic")]
		public string ForeignDomestic { get; set; }

		[JsonProperty("mcc")]
		public string? Mcc { get; set; }

		[JsonProperty("auth_code")]
		public string AuthCode { get; set; }

		[JsonProperty("date_posted")]
		public DateTimeOffset DatePosted { get; set; }

		[JsonProperty("date_loaded")]
		public DateTimeOffset DateLoaded { get; set; }

		[JsonProperty("date_second_request")]
		public object DateSecondRequest { get; set; }

		[JsonProperty("card_number_prefix")]
		public string CardNumberPrefix { get; set; }

		[JsonProperty("card_last4")]
		public string CardLast4 { get; set; }

		[JsonProperty("merchant_dba_name")]
		public string MerchantDbaName { get; set; }

		[JsonProperty("merchant_address1")]
		public object MerchantAddress1 { get; set; }

		[JsonProperty("merchant_address2")]
		public object MerchantAddress2 { get; set; }

		[JsonProperty("merchant_city")]
		public string MerchantCity { get; set; }

		[JsonProperty("merchant_state")]
		public string MerchantState { get; set; }

		[JsonProperty("merchant_zip")]
		public string MerchantZip { get; set; }

		[JsonProperty("group")]
		public string Group { get; set; }

		[JsonProperty("tsys_bank_number")]
		public string TsysBankNumber { get; set; }

		[JsonProperty("tsys_association")]
		public string TsysAssociation { get; set; }

		[JsonProperty("tsys_space")]
		public string TsysSpace { get; set; }

		[JsonProperty("date_warehouse")]
		public DateTimeOffset DateWarehouse { get; set; }

		[JsonProperty("transaction_id")]
		public string TransactionId { get; set; }

		[JsonProperty("merchant_amount")]
		public double MerchantAmount { get; set; }

		[JsonProperty("family_id")]
		public string FamilyId { get; set; }

		[JsonProperty("created_at")]
		public DateTimeOffset CreatedAt { get; set; }

		[JsonProperty("updated_at")]
		public DateTimeOffset UpdatedAt { get; set; }

		[JsonProperty("chargeback_reference_number")]
		public string ChargebackReferenceNumber { get; set; }

		[JsonProperty("mcomm_claim_id")]
		public string McommClaimId { get; set; }

		[JsonProperty("vrol_case_number")]
		public string VrolCaseNumber { get; set; }

		[JsonProperty("fingerprint")]
		public string Fingerprint { get; set; }

		[JsonProperty("log_id")]
		public long LogId { get; set; }

		[JsonProperty("bank_number")]
		public long BankNumber { get; set; }

		[JsonProperty("case_submitted")]
		public bool CaseSubmitted { get; set; }

		[JsonProperty("tsys_fiserv_indicator")]
		public string TsysFiservIndicator { get; set; }

		[JsonProperty("latest_secondary_case_id")]
		public long? LatestSecondaryCaseId { get; set; }

		[JsonProperty("rdr_indicator")]
		public long RdrIndicator { get; set; }

		[JsonProperty("report_date")]
		public DateTimeOffset ReportDate { get; set; }

		[JsonProperty("case_number")]
		public string CaseNumber { get; set; }

		[JsonProperty("is_primary_case")]
		public bool IsPrimaryCase { get; set; }

		[JsonProperty("transaction_date")]
		public DateTimeOffset TransactionDate { get; set; }

		[JsonProperty("case_amount")]
		public double CaseAmount { get; set; }

		[JsonProperty("reason_desc")]
		public string ReasonDesc { get; set; }

		[JsonProperty("case_type")]
		public string CaseType { get; set; }

		[JsonProperty("status")]
		public string Status { get; set; }

		[JsonProperty("card_type")]
		public string CardType { get; set; }

		[JsonProperty("card_number")]
		public string CardNumber { get; set; }

		[JsonProperty("response")]
		public string Response { get; set; }

		[JsonProperty("last_updated_date")]
		public DateTimeOffset? LastUpdatedDate { get; set; }

		[JsonProperty("responded_date")]
		public DateTimeOffset? RespondedDate { get; set; }
	}
}
