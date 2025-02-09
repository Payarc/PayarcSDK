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

		[JsonIgnore]
		public Func<Task<BaseResponse?>> Retrieve { get; set; }

		[JsonIgnore]
		public Func<Task<BaseResponse?>> Delete { get; set; }

		[JsonIgnore]
		public Func<MerchantDocument?, Task<BaseResponse?>> AddDocument { get; set; }

		[JsonIgnore]
		public Func<Task<BaseResponse?>> Submit { get; set; }

		[JsonIgnore]
		public Func<ApplicationInfoData?, Task<BaseResponse?>> Update { get; set; }

		[JsonIgnore]
		public Func<BaseListOptions?, Task<ListBaseResponse?>> ListSubAgents { get; set; } // Fix for CS7003 and CS8618

		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("industry")]
		public string Industry { get; set; }

		[JsonProperty("processing_type")]
		public string ProcessingType { get; set; }

		[JsonProperty("isv_merchant_type")]
		public string? IsvMerchantType { get; set; }

		[JsonProperty("isv_process_own_transactions")]
		public string? IsvProcessOwnTransactions { get; set; }

		[JsonProperty("merchant_category")]
		public string? MerchantCategory { get; set; }

		[JsonProperty("hubspot_record_id")]
		public string? HubspotRecordId { get; set; }

		[JsonProperty("bank_account_type")]
		public string? BankAccountType { get; set; }

		[JsonProperty("agent_name")]
		public string AgentName { get; set; }

		[JsonProperty("agent_tag_values")]
		public AgentTagValues AgentTagValues { get; set; }

		[JsonProperty("agent_id")]
		public int AgentId { get; set; }

		[JsonProperty("agent_parent_id")]
		public string? AgentParentId { get; set; }

		[JsonProperty("agent_parent_name")]
		public string? AgentParentName { get; set; }

		[JsonProperty("apply_pricing_template_id")]
		public string? ApplyPricingTemplateId { get; set; }

		[JsonProperty("apply_pricing_template_name")]
		public string ApplyPricingTemplateName { get; set; }

		[JsonProperty("step")]
		public string? Step { get; set; }

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
		public string? LeadStatus { get; set; }

		[JsonProperty("hardware_shippings")]
		public string? HardwareShippings { get; set; }

		[JsonProperty("status_id")]
		public int StatusId { get; set; }

		[JsonProperty("signature_override")]
		public int SignatureOverride { get; set; }

		[JsonProperty("giact_failed_checks")]
		public int GiactFailedChecks { get; set; }

		[JsonProperty("electronic_signature_full_name")]
		public string? ElectronicSignatureFullName { get; set; }

		[JsonProperty("possible_duplicates")]
		public List<string> PossibleDuplicates { get; set; }

		[JsonProperty("Documents")]
		public List<DocumentResponseData>? Documents { get; set; }

		[JsonProperty("bank_number")]
		public string? BankNumber { get; set; }

		[JsonProperty("lead_id")]
		public string? LeadId { get; set; }

		[JsonProperty("signed_upload_url")]
		public string? SignedUploadUrl { get; set; }

		[JsonProperty("adobe_agreement_id")]
		public string? AdobeAgreementId { get; set; }

		[JsonProperty("docusign_agreement_envelope_id")]
		public string? DocusignAgreementEnvelopeId { get; set; }

		[JsonProperty("agreement_status")]
		public string? AgreementStatus { get; set; }

		[JsonProperty("date_leaded")]
		public string? DateLeaded { get; set; }

		[JsonProperty("ach_transactions_yn")]
		public long? AchTransactionsYn { get; set; }

		[JsonProperty("storagePit")]
		public string? StoragePit { get; set; }

		[JsonProperty("discount_rate_program")]
		public string? DiscountRateProgram { get; set; }

		[JsonProperty("vartsq")]
		public string? Vartsq { get; set; }

		[JsonProperty("MerchantContactFirstName")]
		public string? MerchantContactFirstName { get; set; }

		[JsonProperty("MerchantContactLastName")]
		public string? MerchantContactLastName { get; set; }

		[JsonProperty("MerchantContactEmail")]
		public string? MerchantContactEmail { get; set; }

		[JsonProperty("MerchantLegalName")]
		public string? MerchantLegalName { get; set; }

		[JsonProperty("MerchantOfficeAddress")]
		public string? MerchantOfficeAddress { get; set; }

		[JsonProperty("MerchantOfficeCity")]
		public string? MerchantOfficeCity { get; set; }

		[JsonProperty("MerchantOfficeState")]
		public string? MerchantOfficeState { get; set; }

		[JsonProperty("MerchantOfficeZipCode")]
		public string? MerchantOfficeZipCode { get; set; }

		[JsonProperty("MerchantRegisteredAddress")]
		public string? MerchantRegisteredAddress { get; set; }

		[JsonProperty("MerchantRegisteredCity")]
		public string? MerchantRegisteredCity { get; set; }

		[JsonProperty("MerchantRegisteredState")]
		public string? MerchantRegisteredState { get; set; }

		[JsonProperty("MerchantRegisteredZipCode")]
		public string? MerchantRegisteredZipCode { get; set; }

		[JsonProperty("merchant_ssn")]
		public string? MerchantSsn { get; set; }

		[JsonProperty("merchant_other_tax_id")]
		public string? MerchantOtherTaxId { get; set; }

		[JsonProperty("MerchantContactPhoneNo")]
		public string? MerchantContactPhoneNo { get; set; }

		[JsonProperty("merchant_identification_number_type")]
		public string? MerchantIdentificationNumberType { get; set; }

		[JsonProperty("merchant_foundation_date")]
		public DateTimeOffset? MerchantFoundationDate { get; set; }

		[JsonProperty("MerchantTaxId")]
		public string? MerchantTaxId { get; set; }

		[JsonProperty("MerchantDateIncorporated")]
		public DateTimeOffset? MerchantDateIncorporated { get; set; }

		[JsonProperty("MerchantStateOfIncorporation")]
		public string? MerchantStateOfIncorporation { get; set; }

		[JsonProperty("MerchantOwnershipType")]
		public string? MerchantOwnershipType { get; set; }

		[JsonProperty("MerchantWebsite")]
		public string? MerchantWebsite { get; set; }

		[JsonProperty("MerchantCustomerSupportNumber")]
		public string? MerchantCustomerSupportNumber { get; set; }

		[JsonProperty("merchant_irs_filing_type")]
		public string? MerchantIrsFilingType { get; set; }

		[JsonProperty("MerchantBusinessSummary")]
		public string? MerchantBusinessSummary { get; set; }

		[JsonProperty("MerchantAccountUpdater")]
		public string? MerchantAccountUpdater { get; set; }

		[JsonProperty("MerchantMinMonthlyFee")]
		public string? MerchantMinMonthlyFee { get; set; }

		[JsonProperty("MerchantMonthlyFee")]
		public string? MerchantMonthlyFee { get; set; }

		[JsonProperty("MerchantPciFee")]
		public string? MerchantPciFee { get; set; }

		[JsonProperty("MerchantTerminalFee")]
		public string? MerchantTerminalFee { get; set; }

		[JsonProperty("MerchantGatewayFee")]
		public string? MerchantGatewayFee { get; set; }

		[JsonProperty("MerchantStatementFee")]
		public string? MerchantStatementFee { get; set; }

		[JsonProperty("MerchantRiskAnalysisFee")]
		public string? MerchantRiskAnalysisFee { get; set; }

		[JsonProperty("MerchantWebMonitoringFee")]
		public string? MerchantWebMonitoringFee { get; set; }

		[JsonProperty("MerchantBankSponsorFee")]
		public string? MerchantBankSponsorFee { get; set; }

		[JsonProperty("MerchantAmexSponsorFee")]
		public string? MerchantAmexSponsorFee { get; set; }

		[JsonProperty("MerchantBatchFee")]
		public string? MerchantBatchFee { get; set; }

		[JsonProperty("MerchantChargebackFee")]
		public string? MerchantChargebackFee { get; set; }

		[JsonProperty("MerchantChargebackReversalFee")]
		public string? MerchantChargebackReversalFee { get; set; }

		[JsonProperty("MerchantRetrievalFee")]
		public string? MerchantRetrievalFee { get; set; }

		[JsonProperty("MerchantArbitrationFee")]
		public string? MerchantArbitrationFee { get; set; }

		[JsonProperty("MerchantVoiceAuthFee")]
		public string? MerchantVoiceAuthFee { get; set; }

		[JsonProperty("MerchantDebitCardAuthsFee")]
		public string? MerchantDebitCardAuthsFee { get; set; }

		[JsonProperty("MerchantEbtAuthsFee")]
		public string? MerchantEbtAuthsFee { get; set; }

		[JsonProperty("MerchantAccountUpdateOtherFee")]
		public string? MerchantAccountUpdateOtherFee { get; set; }

		[JsonProperty("MerchantGatewayTransactionsFee")]
		public string? MerchantGatewayTransactionsFee { get; set; }

		[JsonProperty("MerchantPerAchRejectFee")]
		public string? MerchantPerAchRejectFee { get; set; }

		[JsonProperty("MerchantDeclinesFee")]
		public string? MerchantDeclinesFee { get; set; }

		[JsonProperty("MerchantRefundsFee")]
		public string? MerchantRefundsFee { get; set; }

		[JsonProperty("MerchantAvsFee")]
		public string? MerchantAvsFee { get; set; }

		[JsonProperty("MerchantEthocaVerifiAlertsFee")]
		public string? MerchantEthocaVerifiAlertsFee { get; set; }

		[JsonProperty("MerchantRapidDisputeResolutionFee")]
		public string? MerchantRapidDisputeResolutionFee { get; set; }

		[JsonProperty("MerchantCurrentlyAcceptCreditCards")]
		public string? MerchantCurrentlyAcceptCreditCards { get; set; }

		[JsonProperty("MerchantCardPresentPct")]
		public string? MerchantCardPresentPct { get; set; }

		[JsonProperty("MerchantMotoPct")]
		public string? MerchantMotoPct { get; set; }

		[JsonProperty("MerchantBankAccountNo")]
		public string? MerchantBankAccountNo { get; set; }

		[JsonProperty("MerchantBankRoutingNo")]
		public string? MerchantBankRoutingNo { get; set; }

		[JsonProperty("merchant_bank_name")]
		public string? MerchantBankName { get; set; }

		[JsonProperty("merchant_account_address")]
		public string? MerchantAccountAddress { get; set; }

		[JsonProperty("merchant_account_city")]
		public string? MerchantAccountCity { get; set; }

		[JsonProperty("merchant_account_state")]
		public string? MerchantAccountState { get; set; }

		[JsonProperty("merchant_account_zip_code")]
		public string? MerchantAccountZipCode { get; set; }

		[JsonProperty("MerchantTotalMonthlyProcessing")]
		public string? MerchantTotalMonthlyProcessing { get; set; }

		[JsonProperty("SalesAnnualMastercard")]
		public string? SalesAnnualMastercard { get; set; }

		[JsonProperty("SalesDeliveryOffer")]
		public string SalesDeliveryOffer { get; set; }

		[JsonProperty("SalesDeliveryDays")]
		public string? SalesDeliveryDays { get; set; }

		[JsonProperty("SalesDeliveryDays0Pct")]
		public string? SalesDeliveryDays0Pct { get; set; }

		[JsonProperty("SalesDeliveryDays1Pct")]
		public string? SalesDeliveryDays1Pct { get; set; }

		[JsonProperty("SalesDeliveryDays8Pct")]
		public string? SalesDeliveryDays8Pct { get; set; }

		[JsonProperty("SalesDeliveryDays15Pct")]
		public string? SalesDeliveryDays15Pct { get; set; }

		[JsonProperty("SalesDeliveryDays30Pct")]
		public string? SalesDeliveryDays30Pct { get; set; }

		[JsonProperty("SalesKeyedInternetPct")]
		public string? SalesKeyedInternetPct { get; set; }

		[JsonProperty("SalesKeyedMailPct")]
		public string? SalesKeyedMailPct { get; set; }

		[JsonProperty("SalesKeyedPhonePct")]
		public string? SalesKeyedPhonePct { get; set; }

		[JsonProperty("SalesTradeshowPct")]
		public string? SalesTradeshowPct { get; set; }

		[JsonProperty("SalesTotalAnnual")]
		public string? SalesTotalAnnual { get; set; }

		[JsonProperty("SalesB2BPct")]
		public string? SalesB2BPct { get; set; }

		[JsonProperty("SalesB2CPct")]
		public string? SalesB2CPct { get; set; }

		[JsonProperty("SalesCardB2BPct")]
		public string? SalesCardB2BPct { get; set; }

		[JsonProperty("SalesCardB2CPct")]
		public string? SalesCardB2CPct { get; set; }

		[JsonProperty("SalesProductServices")]
		public string SalesProductServices { get; set; }

		[JsonProperty("MerchantAvgTicketValue")]
		public string? MerchantAvgTicketValue { get; set; }

		[JsonProperty("MerchantHighTicketValue")]
		public string? MerchantHighTicketValue { get; set; }

		[JsonProperty("MerchantDesiredDescriptor")]
		public string? MerchantDesiredDescriptor { get; set; }

		[JsonProperty("MerchantEbtFnsNumber")]
		public string? MerchantEbtFnsNumber { get; set; }

		[JsonProperty("MerchantProcessingTerminated")]
		public string? MerchantProcessingTerminated { get; set; }

		[JsonProperty("MerchantBankruptcy")]
		public string? MerchantBankruptcy { get; set; }

		[JsonProperty("MerchantAmexProcessingPerYear")]
		public string? MerchantAmexProcessingPerYear { get; set; }

		[JsonProperty("MerchantOtherFee")]
		public string? MerchantOtherFee { get; set; }

		[JsonProperty("MerchantOtherFeeName")]
		public string? MerchantOtherFeeName { get; set; }

		[JsonProperty("MerchantCommentsOther")]
		public string? MerchantCommentsOther { get; set; }

		[JsonProperty("MerchantMonthlyOtherFee")]
		public string? MerchantMonthlyOtherFee { get; set; }

		[JsonProperty("MerchantMonthlyOtherFeeName")]
		public string? MerchantMonthlyOtherFeeName { get; set; }

		[JsonProperty("MerchantAmexEsaNumber")]
		public string? MerchantAmexEsaNumber { get; set; }

		[JsonProperty("TieredPricingQualifiedPctVisaMC")]
		public string? TieredPricingQualifiedPctVisaMc { get; set; }

		[JsonProperty("TieredPricingQualifiedPctAmex")]
		public string? TieredPricingQualifiedPctAmex { get; set; }

		[JsonProperty("TieredPricingQualifiedPctDiscover")]
		public string? TieredPricingQualifiedPctDiscover { get; set; }

		[JsonProperty("TieredPricingQualifiedPctDebit")]
		public string? TieredPricingQualifiedPctDebit { get; set; }

		[JsonProperty("TieredPricingMidQualifiedPctVisaMC")]
		public string? TieredPricingMidQualifiedPctVisaMc { get; set; }

		[JsonProperty("TieredPricingMidQualifiedPctAmex")]
		public string? TieredPricingMidQualifiedPctAmex { get; set; }

		[JsonProperty("TieredPricingMidQualifiedPctDiscover")]
		public string? TieredPricingMidQualifiedPctDiscover { get; set; }

		[JsonProperty("TieredPricingMidQualifiedPctDebit")]
		public string? TieredPricingMidQualifiedPctDebit { get; set; }

		[JsonProperty("TieredPricingNonQualifiedPctVisaMC")]
		public string? TieredPricingNonQualifiedPctVisaMc { get; set; }

		[JsonProperty("TieredPricingNonQualifiedPctAmex")]
		public string? TieredPricingNonQualifiedPctAmex { get; set; }

		[JsonProperty("TieredPricingNonQualifiedPctDiscover")]
		public string? TieredPricingNonQualifiedPctDiscover { get; set; }

		[JsonProperty("TieredPricingNonQualifiedPctDebit")]
		public string? TieredPricingNonQualifiedPctDebit { get; set; }

		[JsonProperty("TieredPricingQualifiedAuthVisaMC")]
		public string? TieredPricingQualifiedAuthVisaMc { get; set; }

		[JsonProperty("TieredPricingQualifiedAuthAmex")]
		public string? TieredPricingQualifiedAuthAmex { get; set; }

		[JsonProperty("TieredPricingQualifiedAuthDiscover")]
		public string? TieredPricingQualifiedAuthDiscover { get; set; }

		[JsonProperty("TieredPricingQualifiedAuthDebit")]
		public string? TieredPricingQualifiedAuthDebit { get; set; }

		[JsonProperty("TieredPricingMidQualifiedAuthVisaMC")]
		public string? TieredPricingMidQualifiedAuthVisaMc { get; set; }

		[JsonProperty("TieredPricingMidQualifiedAuthAmex")]
		public string? TieredPricingMidQualifiedAuthAmex { get; set; }

		[JsonProperty("TieredPricingMidQualifiedAuthDiscover")]
		public string? TieredPricingMidQualifiedAuthDiscover { get; set; }

		[JsonProperty("TieredPricingMidQualifiedAuthDebit")]
		public string? TieredPricingMidQualifiedAuthDebit { get; set; }

		[JsonProperty("TieredPricingNonQualifiedAuthVisaMC")]
		public string? TieredPricingNonQualifiedAuthVisaMc { get; set; }

		[JsonProperty("TieredPricingNonQualifiedAuthAmex")]
		public string? TieredPricingNonQualifiedAuthAmex { get; set; }

		[JsonProperty("TieredPricingNonQualifiedAuthDiscover")]
		public string? TieredPricingNonQualifiedAuthDiscover { get; set; }

		[JsonProperty("TieredPricingNonQualifiedAuthDebit")]
		public string? TieredPricingNonQualifiedAuthDebit { get; set; }

		[JsonProperty("InterchangePlusPricingPctVisaMC")]
		public string? InterchangePlusPricingPctVisaMc { get; set; }

		[JsonProperty("InterchangePlusPricingPctAmex")]
		public string? InterchangePlusPricingPctAmex { get; set; }

		[JsonProperty("InterchangePlusPricingPctDiscover")]
		public string? InterchangePlusPricingPctDiscover { get; set; }

		[JsonProperty("InterchangePlusAuthorizationsVisaMC")]
		public string? InterchangePlusAuthorizationsVisaMc { get; set; }

		[JsonProperty("InterchangePlusAuthorizationsAmex")]
		public string? InterchangePlusAuthorizationsAmex { get; set; }

		[JsonProperty("InterchangePlusAuthorizationsDiscover")]
		public string? InterchangePlusAuthorizationsDiscover { get; set; }

		[JsonProperty("BlendedRatePricingFlatRatePct")]
		public string? BlendedRatePricingFlatRatePct { get; set; }

		[JsonProperty("BlendedRatePricingAuthorizations")]
		public string? BlendedRatePricingAuthorizations { get; set; }

		[JsonProperty("BlendedRatePricingCashDiscountRatePct")]
		public string? BlendedRatePricingCashDiscountRatePct { get; set; }

		[JsonProperty("BlendedRatePricingCashDiscountFee")]
		public string? BlendedRatePricingCashDiscountFee { get; set; }

		[JsonProperty("BlendedBaseCharge")]
		public string? BlendedBaseCharge { get; set; }

		[JsonProperty("BlendedPercentageCharge")]
		public string? BlendedPercentageCharge { get; set; }

		[JsonProperty("AmexOptBlue")]
		public string? AmexOptBlue { get; set; }

		[JsonProperty("AmexBaseCharge")]
		public string? AmexBaseCharge { get; set; }

		[JsonProperty("AmexPercentageCharge")]
		public string? AmexPercentageCharge { get; set; }

		[JsonProperty("WeeklyFee")]
		public string? WeeklyFee { get; set; }

		[JsonProperty("MonthlyFee")]
		public string? MonthlyFee { get; set; }

		[JsonProperty("DailyFee")]
		public string? DailyFee { get; set; }

		[JsonProperty("ChargebackFee")]
		public string? ChargebackFee { get; set; }

		[JsonProperty("ChargebackPercFee")]
		public string? ChargebackPercFee { get; set; }

		[JsonProperty("FiservDiscountRateFeePct")]
		public string? FiservDiscountRateFeePct { get; set; }

		[JsonProperty("FiservTransactionFee")]
		public string? FiservTransactionFee { get; set; }

		[JsonProperty("FiservMonthlySplitFundingFee")]
		public string? FiservMonthlySplitFundingFee { get; set; }

		[JsonProperty("FiservGatewayMonthlyFee")]
		public string? FiservGatewayMonthlyFee { get; set; }

		[JsonProperty("FiservGatewayTransactionFee")]
		public string? FiservGatewayTransactionFee { get; set; }

		[JsonProperty("FiservGatewayTransactionFeePct")]
		public string? FiservGatewayTransactionFeePct { get; set; }

		[JsonProperty("FiservAmexSponsorFeePct")]
		public string? FiservAmexSponsorFeePct { get; set; }

		[JsonProperty("FiservBatchFee")]
		public string? FiservBatchFee { get; set; }

		[JsonProperty("FiservSplitFundingFee")]
		public string? FiservSplitFundingFee { get; set; }

		[JsonProperty("FiservTokenizationFee")]
		public string? FiservTokenizationFee { get; set; }

		[JsonProperty("FiservMonthlyMinimumFee")]
		public string? FiservMonthlyMinimumFee { get; set; }

		[JsonProperty("QuestionnaireAdvancedDeposits")]
		public string? QuestionnaireAdvancedDeposits { get; set; }

		[JsonProperty("QuestionnaireCaptureCVV")]
		public string? QuestionnaireCaptureCvv { get; set; }

		[JsonProperty("QuestionnaireDepositPct")]
		public string? QuestionnaireDepositPct { get; set; }

		[JsonProperty("QuestionnaireIsDeliveryReceiptRequested")]
		public string? QuestionnaireIsDeliveryReceiptRequested { get; set; }

		[JsonProperty("QuestionnaireIsProcessingSeasonal")]
		public string? QuestionnaireIsProcessingSeasonal { get; set; }

		[JsonProperty("QuestionnaireMembershipPayments")]
		public string? QuestionnaireMembershipPayments { get; set; }

		[JsonProperty("QuestionnaireMembershipPaymentsPct")]
		public string? QuestionnaireMembershipPaymentsPct { get; set; }

		[JsonProperty("QuestionnaireNoRefundsInfo")]
		public string? QuestionnaireNoRefundsInfo { get; set; }

		[JsonProperty("QuestionnaireOrderTurnaroundTime")]
		public string? QuestionnaireOrderTurnaroundTime { get; set; }

		[JsonProperty("QuestionnairePackageUsageTime")]
		public string? QuestionnairePackageUsageTime { get; set; }

		[JsonProperty("QuestionnaireProcessingSeasonalMonths")]
		public string[] QuestionnaireProcessingSeasonalMonths { get; set; }

		[JsonProperty("QuestionnaireProdSoldEuropePct")]
		public string? QuestionnaireProdSoldEuropePct { get; set; }

		[JsonProperty("QuestionnaireProdSoldNorthAmericaPct")]
		public string? QuestionnaireProdSoldNorthAmericaPct { get; set; }

		[JsonProperty("QuestionnaireProdSoldOtherMarketsPct")]
		public string? QuestionnaireProdSoldOtherMarketsPct { get; set; }

		[JsonProperty("QuestionnaireRecurringPaymentProvider")]
		public string? QuestionnaireRecurringPaymentProvider { get; set; }

		[JsonProperty("QuestionnaireReturnPolicy")]
		public string? QuestionnaireReturnPolicy { get; set; }

		[JsonProperty("QuestionnaireReturnPolicyOther")]
		public string? QuestionnaireReturnPolicyOther { get; set; }

		[JsonProperty("QuestionnaireSellBusinessPct")]
		public string? QuestionnaireSellBusinessPct { get; set; }

		[JsonProperty("QuestionnaireSellConsumerPct")]
		public string? QuestionnaireSellConsumerPct { get; set; }

		[JsonProperty("QuestionnaireWhenIsCustomerCharged")]
		public string? QuestionnaireWhenIsCustomerCharged { get; set; }

		[JsonProperty("QuestionnaireWhoManagesRecurringPayments")]
		public string? QuestionnaireWhoManagesRecurringPayments { get; set; }
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
		public string? AgentId { get; set; }

		[JsonProperty("user_id")]
		public string? UserId { get; set; }

		[JsonProperty("created_at")]
		public string? CreatedAt { get; set; }

		[JsonProperty("updated_at")]
		public string? UpdatedAt { get; set; }
	}
}
