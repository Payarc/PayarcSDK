using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayarcSDK.Entities.SplitCampaign
{
    public class MyAccountResponseData : BaseResponse
    {
        [JsonProperty("object")]
        public override string? Object { get; set; }

        [JsonProperty("object_id")]
        public override string? ObjectId
        {
            get
            {
                if (string.IsNullOrEmpty(Object) || string.IsNullOrEmpty(Id))
                    return null;
                return $"macc_{Id}";
            }
        }

        [JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("email")]
		public string Email { get; set; }

		[JsonProperty("mid")]
		public string Mid { get; set; }

		[JsonProperty("legal_name")]
		public string LegalName { get; set; }

		[JsonProperty("dba_name")]
		public string DbaName { get; set; }

		[JsonProperty("merchant_name")]
		public string MerchantName { get; set; }

		[JsonProperty("status")]
		public string Status { get; set; }

		[JsonProperty("pci_compliance_grade")]
		public object PciComplianceGrade { get; set; }

		[JsonProperty("gateway_type")]
		public string GatewayType { get; set; }

		[JsonProperty("trans_type")]
		public string? TransType { get; set; }

		[JsonProperty("sic_code")]
		public long SicCode { get; set; }

		[JsonProperty("approved_volume")]
		public long? ApprovedVolume { get; set; }

		[JsonProperty("last_process_date")]
		public DateTimeOffset? LastProcessDate { get; set; }

		[JsonProperty("first_batch_date")]
		public DateTimeOffset? FirstBatchDate { get; set; }

		[JsonProperty("agent_referral")]
		public string AgentReferral { get; set; }

		[JsonProperty("website")]
		public Uri Website { get; set; }

		[JsonProperty("phone_number")]
		public long? PhoneNumber { get; set; }

		[JsonProperty("owner_first_name")]
		public string OwnerFirstName { get; set; }

		[JsonProperty("owner_last_name")]
		public string OwnerLastName { get; set; }

		[JsonProperty("street_address")]
		public string StreetAddress { get; set; }

		[JsonProperty("city")]
		public string City { get; set; }

		[JsonProperty("state")]
		public string State { get; set; }

		[JsonProperty("zipcode")]
		public string Zipcode { get; set; }

		[JsonProperty("business_phone")]
		public string BusinessPhone { get; set; }

		[JsonProperty("cell_phone")]
		public string CellPhone { get; set; }

		[JsonProperty("owner_address")]
		public string OwnerAddress { get; set; }

		[JsonProperty("owner_city")]
		public string OwnerCity { get; set; }

		[JsonProperty("owner_state")]
		public string OwnerState { get; set; }

		[JsonProperty("owner_zipcode")]
		public string OwnerZipcode { get; set; }

		[JsonProperty("owner_email")]
		public string OwnerEmail { get; set; }

		[JsonProperty("start_processing")]
		public DateTimeOffset? StartProcessing { get; set; }

		[JsonProperty("approved_average")]
		public long? ApprovedAverage { get; set; }

		[JsonProperty("approved_high_ticket")]
		public long? ApprovedHighTicket { get; set; }

		[JsonProperty("settlement")]
		public string Settlement { get; set; }

		[JsonProperty("isActive")]
		public bool IsActive { get; set; }

		[JsonProperty("isDormant")]
		public bool IsDormant { get; set; }

		[JsonProperty("isInactive")]
		public bool IsInactive { get; set; }

		[JsonProperty("residualMonthPrevious")]
		public double ResidualMonthPrevious { get; set; }

		[JsonProperty("residualYearToMonth")]
		public long ResidualYearToMonth { get; set; }
	}
}
