using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayarcSDK.Entities.Payee
{
    public class PayeeResponseData : BaseResponse {
        [JsonProperty("object")]
        public override string Object { get; set; }

        [JsonProperty("object_id")]
        public override string? ObjectId
        {
            get
            {
                if (string.IsNullOrEmpty(Object) || string.IsNullOrEmpty(Id))
                    return null;
                return $"appy_{Id}";
            }
        }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("industry")]
        public object Industry { get; set; }

        [JsonProperty("processing_type")]
        public string ProcessingType { get; set; }

        [JsonProperty("isv_merchant_type")]
        public string IsvMerchantType { get; set; }

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
        public object IsCopied { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }

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
        public object SignatureOverride { get; set; }

        [JsonProperty("giact_failed_checks")]
        public object GiactFailedChecks { get; set; }

        [JsonProperty("electronic_signature_full_name")]
        public object ElectronicSignatureFullName { get; set; }

        [JsonProperty("possible_duplicates")]
        public List<object> PossibleDuplicates { get; set; }
    }
}
