using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayarcSDK.Entities
{
    public class ApplicationResponseData : BaseResponse {
		[JsonProperty("object")]
		public string Object { get; set; }

		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("industry")]
		public string Industry { get; set; }

		[JsonProperty("processing_type")]
		public string ProcessingType { get; set; }

		[JsonProperty("isv_merchant_type")]
		public object IsvMerchantType { get; set; }

		[JsonProperty("isv_process_own_transactions")]
		public object IsvProcessOwnTransactions { get; set; }

		[JsonProperty("merchant_category")]
		public object MerchantCategory { get; set; }

		[JsonProperty("hubspot_record_id")]
		public object HubspotRecordId { get; set; }

		[JsonProperty("bank_account_type")]
		public object BankAccountType { get; set; }

		[JsonProperty("agent_name")]
		public string AgentName { get; set; }

		[JsonProperty("agent_tag_values")]
		public AgentTagValues AgentTagValues { get; set; }

		[JsonProperty("agent_id")]
		public int AgentId { get; set; }

		[JsonProperty("agent_parent_id")]
		public object AgentParentId { get; set; }

		[JsonProperty("agent_parent_name")]
		public object AgentParentName { get; set; }

		[JsonProperty("apply_pricing_template_id")]
		public object ApplyPricingTemplateId { get; set; }

		[JsonProperty("apply_pricing_template_name")]
		public string ApplyPricingTemplateName { get; set; }

		[JsonProperty("step")]
		public object Step { get; set; }

		[JsonProperty("is_copied")]
		public int IsCopied { get; set; }

		[JsonProperty("created_at")]
		public string CreatedAt { get; set; }

		[JsonProperty("updated_at")]
		public string UpdatedAt { get; set; }

		[JsonProperty("status")]
		public string Status { get; set; }

		[JsonProperty("completed")]
		public int Completed { get; set; }

		[JsonProperty("lead_status")]
		public object LeadStatus { get; set; }

		[JsonProperty("hardware_shippings")]
		public object HardwareShippings { get; set; }

		[JsonProperty("status_id")]
		public int StatusId { get; set; }

		[JsonProperty("signature_override")]
		public int SignatureOverride { get; set; }

		[JsonProperty("giact_failed_checks")]
		public int GiactFailedChecks { get; set; }

		[JsonProperty("electronic_signature_full_name")]
		public object ElectronicSignatureFullName { get; set; }

		[JsonProperty("possible_duplicates")]
		public List<string> PossibleDuplicates { get; set; }

		[JsonProperty("Documents")]
		public List<DocumentResponseData>? Documents { get; set; }
	}

	public class AgentTagValues {
		[JsonProperty("id")]
		public int Id { get; set; }

		[JsonProperty("category")]
		public string Category { get; set; }

		[JsonProperty("item")]
		public string Item { get; set; }

		[JsonProperty("color")]
		public string Color { get; set; }

		[JsonProperty("agent_id")]
		public int AgentId { get; set; }

		[JsonProperty("user_id")]
		public object UserId { get; set; }

		[JsonProperty("created_at")]
		public object CreatedAt { get; set; }

		[JsonProperty("updated_at")]
		public object UpdatedAt { get; set; }
	}
}
