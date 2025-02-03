using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayarcSDK.Entities.ApplicationService
{
	public partial class ApplicationInfoData {
		[JsonProperty("agentId")]
		public string? agentId { get; set; }

		[JsonProperty("MerchantCode")]
		public string? MerchantCode { get; set; }

		[JsonProperty("Lead")]
		public Lead Lead { get; set; }

		[JsonProperty("BlendedRatePricing")]
		public BlendedRatePricing BlendedRatePricing { get; set; }

		[JsonProperty("Owners")]
		public List<Owner> Owners { get; set; }

		[JsonProperty("BeneficialOwners")]
		public List<BeneficialOwner> BeneficialOwners { get; set; }

		[JsonProperty("Questionnaire")]
		public Questionnaire Questionnaire { get; set; }

		[JsonProperty("AssignedAgentEmail")]
		public string AssignedAgentEmail { get; set; }

		[JsonProperty("TSQuestionery")]
		public TsQuestionery TsQuestionery { get; set; }

		public string ToJson() {
			var settings = new JsonSerializerSettings {
				NullValueHandling = NullValueHandling.Ignore,
				Formatting = Formatting.Indented
			};

			return JsonConvert.SerializeObject(this, settings);
		}
	}

	public partial class BeneficialOwner {
		[JsonProperty("FirstName")]
		public string FirstName { get; set; }

		[JsonProperty("LastName")]
		public string LastName { get; set; }

		[JsonProperty("Title")]
		public string Title { get; set; }

		[JsonProperty("OwnershipPct")]
		public string OwnershipPct { get; set; }

		[JsonProperty("Address")]
		public string Address { get; set; }

		[JsonProperty("City")]
		public string City { get; set; }

		[JsonProperty("State")]
		public string State { get; set; }

		[JsonProperty("Country")]
		public string Country { get; set; }

		[JsonProperty("ZipCode")]
		public string ZipCode { get; set; }

		[JsonProperty("BirthDate")]
		public string BirthDate { get; set; }

		[JsonProperty("SSN")]
		public string SSN { get; set; }

		[JsonProperty("OtherIdNo")]
		public string OtherIdNo { get; set; }

		[JsonProperty("OtherIdCountry")]
		public string OtherIdCountry { get; set; }

		[JsonProperty("OtherIdType")]
		public string OtherIdType { get; set; }

		[JsonProperty("")]
		public string Empty { get; set; }
	}

	public partial class BlendedRatePricing {
		[JsonProperty("BlendedBaseCharge")]
		public string BlendedBaseCharge { get; set; }

		[JsonProperty("BlendedChargePct")]
		public string BlendedChargePct { get; set; }

		[JsonProperty("AmexOptBlue")]
		//[JsonConverter(typeof(ParseStringConverter))]
		public long AmexOptBlue { get; set; }

		[JsonProperty("AmexBaseCharge")]
		public string AmexBaseCharge { get; set; }

		[JsonProperty("AmexChargePct")]
		public string AmexChargePct { get; set; }

		[JsonProperty("DailyFee")]
		public string DailyFee { get; set; }

		[JsonProperty("WeeklyFee")]
		public string WeeklyFee { get; set; }

		[JsonProperty("MonthlyFee")]
		public string MonthlyFee { get; set; }

		[JsonProperty("ChargebackFee")]
		public string ChargebackFee { get; set; }

		[JsonProperty("ChargebackPercFee")]
		public string ChargebackPercFee { get; set; }

		[JsonProperty("FiservDiscountRateFeePct")]
		public string FiservDiscountRateFeePct { get; set; }

		[JsonProperty("FiservTransactionFee")]
		public string FiservTransactionFee { get; set; }

		[JsonProperty("FiservMonthlySplitFundingFee")]
		public string FiservMonthlySplitFundingFee { get; set; }

		[JsonProperty("FiservGatewayMonthlyFee")]
		public string FiservGatewayMonthlyFee { get; set; }

		[JsonProperty("FiservGatewayTransactionFee")]
		public string FiservGatewayTransactionFee { get; set; }

		[JsonProperty("FiservGatewayTransactionFeePct")]
		public string FiservGatewayTransactionFeePct { get; set; }

		[JsonProperty("FiservAmexSponsorFeePct")]
		public string FiservAmexSponsorFeePct { get; set; }

		[JsonProperty("FiservBatchFee")]
		public string FiservBatchFee { get; set; }

		[JsonProperty("FiservSplitFundingFee")]
		public string FiservSplitFundingFee { get; set; }

		[JsonProperty("FiservTokenizationFee")]
		public string FiservTokenizationFee { get; set; }

		[JsonProperty("FiservMonthlyMinimumFee")]
		public string FiservMonthlyMinimumFee { get; set; }
	}

	public partial class Lead {
		[JsonProperty("ProcessingType")]
		public string ProcessingType { get; set; }

		[JsonProperty("Industry")]
		//[JsonConverter(typeof(ParseStringConverter))]
		public string Industry { get; set; }

		[JsonProperty("MerchantName")]
		public string MerchantName { get; set; }

		[JsonProperty("LegalName")]
		public string LegalName { get; set; }

		[JsonProperty("ContactFirstName")]
		public string ContactFirstName { get; set; }

		[JsonProperty("ContactLastName")]
		public string ContactLastName { get; set; }

		[JsonProperty("ContactEmail")]
		public string ContactEmail { get; set; }

		[JsonProperty("DiscountRateProgram")]
		public string DiscountRateProgram { get; set; }

		[JsonProperty("Website")]
		public string? Website { get; set; }

		[JsonProperty("IdentificationNumberType")]
		//[JsonConverter(typeof(ParseStringConverter))]
		public long? IdentificationNumberType { get; set; }

		[JsonProperty("MerchantSSN")]
		public string MerchantSsn { get; set; }

		[JsonProperty("TaxId")]
		public string TaxId { get; set; }

		[JsonProperty("MerchantOtherTaxId")]
		public string MerchantOtherTaxId { get; set; }

		[JsonProperty("IrsFilingType")]
		public string IrsFilingType { get; set; }

		[JsonProperty("FoundationDate")]
		public DateTimeOffset? FoundationDate { get; set; }

		[JsonProperty("SalesDeliveryOffer")]
		public string SalesDeliveryOffer { get; set; }

		[JsonProperty("SalesAnnualMastercard")]
		public string SalesAnnualMastercard { get; set; }

		[JsonProperty("SalesDeliveryDays0Pct")]
		public long? SalesDeliveryDays0Pct { get; set; }

		[JsonProperty("SalesDeliveryDays1Pct")]
		public long? SalesDeliveryDays1Pct { get; set; }

		[JsonProperty("SalesDeliveryDays8Pct")]
		public long? SalesDeliveryDays8Pct { get; set; }

		[JsonProperty("SalesDeliveryDays15Pct")]
		public long? SalesDeliveryDays15Pct { get; set; }

		[JsonProperty("SalesDeliveryDays30Pct")]
		public long? SalesDeliveryDays30Pct { get; set; }

		[JsonProperty("CardPresentPct")]
		public long? CardPresentPct { get; set; }

		[JsonProperty("MotoPct")]
		public long? MotoPct { get; set; }

		[JsonProperty("SalesKeyedInternetPct")]
		public long? SalesKeyedInternetPct { get; set; }

		[JsonProperty("SalesKeyedMailPct")]
		public long? SalesKeyedMailPct { get; set; }

		[JsonProperty("SalesKeyedPhonePct")]
		public long? SalesKeyedPhonePct { get; set; }

		[JsonProperty("SalesTradeshowPct")]
		public long? SalesTradeshowPct { get; set; }

		[JsonProperty("SalesTotalAnnual")]
		public long? SalesTotalAnnual { get; set; }

		[JsonProperty("SalesB2BPct")]
		public long? SalesB2BPct { get; set; }

		[JsonProperty("SalesB2CPct")]
		public long? SalesB2CPct { get; set; }

		[JsonProperty("SalesCardB2BPct")]
		public long? SalesCardB2BPct { get; set; }

		[JsonProperty("SalesCardB2CPct")]
		public long? SalesCardB2CPct { get; set; }

		[JsonProperty("SalesProductServices")]
		public string SalesProductServices { get; set; }

		[JsonProperty("MerchantBankName")]
		public string MerchantBankName { get; set; }

		[JsonProperty("MerchantAccountAddress")]
		public string MerchantAccountAddress { get; set; }

		[JsonProperty("MerchantAccountCity")]
		public string MerchantAccountCity { get; set; }

		[JsonProperty("MerchantAccountState")]
		public string MerchantAccountState { get; set; }

		[JsonProperty("MerchantZipCode")]
		public string MerchantZipCode { get; set; }

		[JsonProperty("DateIncorporated")]
		public DateTimeOffset? DateIncorporated { get; set; }

		[JsonProperty("OfficeCity")]
		public string OfficeCity { get; set; }

		[JsonProperty("OfficeZipCode")]
		public string OfficeZipCode { get; set; }

		[JsonProperty("OfficeState")]
		public string OfficeState { get; set; }

		[JsonProperty("CustomerSupportNumber")]
		public string CustomerSupportNumber { get; set; }

		[JsonProperty("RegisteredZipCode")]
		public string RegisteredZipCode { get; set; }

		[JsonProperty("RegisteredCity")]
		public string RegisteredCity { get; set; }

		[JsonProperty("Country")]
		public string Country { get; set; }

		[JsonProperty("RegisteredAddress")]
		public string RegisteredAddress { get; set; }

		[JsonProperty("RegisteredState")]
		public string RegisteredState { get; set; }

		[JsonProperty("BankAccountNo")]
		public string BankAccountNo { get; set; }

		[JsonProperty("BankRoutingNo")]
		public string BankRoutingNo { get; set; }

		[JsonProperty("BankAccountType")]
		public string BankAccountType { get; set; }

		[JsonProperty("SlugId")]
		public string? SlugId { get; set; }

		[JsonProperty("SkipGiact")]
		public bool? SkipGiact { get; set; }

		[JsonProperty("OwnershipType")]
		public string OwnershipType { get; set; }

		[JsonProperty("TotalMonthlyProcessing")]
		public string TotalMonthlyProcessing { get; set; }

		[JsonProperty("AvgTicketValue")]
		public string AvgTicketValue { get; set; }

		[JsonProperty("HighTicketValue")]
		public string HighTicketValue { get; set; }

		[JsonProperty("SalesDeliveryDays")]
		public string SalesDeliveryDays { get; set; }

		[JsonProperty("OfficeAddress")]
		public string OfficeAddress { get; set; }

		[JsonProperty("ContactPhoneNo")]
		public string ContactPhoneNo { get; set; }

		[JsonProperty("StateOfIncorporation")]
		public string StateOfIncorporation { get; set; }

		[JsonProperty("BusinessSummary")]
		public string BusinessSummary { get; set; }

		[JsonProperty("CurrentyAcceptCreditCards")]
		public string CurrentyAcceptCreditCards { get; set; }

		[JsonProperty("ProcessingTerminated")]
		public string ProcessingTerminated { get; set; }

		[JsonProperty("Bankruptcy")]
		public string Bankruptcy { get; set; }

		[JsonProperty("CommentsOther")]
		public string CommentsOther { get; set; }

		[JsonProperty("DriverLicenseOrId")]
		public string DriverLicenseOrId { get; set; }

		[JsonProperty("MerchantAmexProcessingPerYear")]
		public long? MerchantAmexProcessingPerYear { get; set; }
	}

	public partial class Owner {
		[JsonProperty("Title")]
		public string Title { get; set; }

		[JsonProperty("FirstName")]
		public string FirstName { get; set; }

		[JsonProperty("LastName")]
		public string LastName { get; set; }

		[JsonProperty("Address")]
		public string Address { get; set; }

		[JsonProperty("City")]
		public string City { get; set; }

		[JsonProperty("State")]
		public string State { get; set; }

		[JsonProperty("ZipCode")]
		public string ZipCode { get; set; }

		[JsonProperty("PhoneNo")]
		public string PhoneNo { get; set; }

		[JsonProperty("BirthDate")]
		public string BirthDate { get; set; }

		[JsonProperty("SSN")]
		public string SSN { get; set; }

		[JsonProperty("Email")]
		public string Email { get; set; }

		[JsonProperty("OwnershipPct")]
		public long? OwnershipPct { get; set; }

		[JsonProperty("DriverLicenseNo")]
		public string DriverLicenseNo { get; set; }

		[JsonProperty("DriverLicenseState")]
		public string DriverLicenseState { get; set; }
	}

	public partial class Questionnaire {
		[JsonProperty("WebsitePayments")]
		public string WebsitePayments { get; set; }

		[JsonProperty("SSLProvider")]
		public string SslProvider { get; set; }

		[JsonProperty("ProdSoldNorthAmericaPct")]
		public long? ProdSoldNorthAmericaPct { get; set; }

		[JsonProperty("ProdSoldEuropePct")]
		public long? ProdSoldEuropePct { get; set; }

		[JsonProperty("ProdSoldOtherMarketsPct")]
		public long? ProdSoldOtherMarketsPct { get; set; }

		[JsonProperty("SellBusinessPct")]
		public long? SellBusinessPct { get; set; }

		[JsonProperty("SellConsumerPct")]
		public long? SellConsumerPct { get; set; }

		[JsonProperty("ReturnPolicy")]
		public string ReturnPolicy { get; set; }

		[JsonProperty("NoRefundsInfo")]
		public string NoRefundsInfo { get; set; }

		[JsonProperty("ReturnPolicyOther")]
		public string ReturnPolicyOther { get; set; }

		[JsonProperty("MonthlyRefundsPct")]
		public long? MonthlyRefundsPct { get; set; }

		[JsonProperty("RefundDays")]
		public long? RefundDays { get; set; }

		[JsonProperty("Refund100Pct")]
		public bool? Refund100Pct { get; set; }

		[JsonProperty("RefundLess100PctInfo")]
		public string RefundLess100PctInfo { get; set; }

		[JsonProperty("WhenIsCustomerCharged")]
		public string WhenIsCustomerCharged { get; set; }

		[JsonProperty("IsShipmentTraceable")]
		public bool? IsShipmentTraceable { get; set; }

		[JsonProperty("IsDeliveryReceiptRequested")]
		public bool? IsDeliveryReceiptRequested { get; set; }

		[JsonProperty("OrderTurnaroundTime")]
		public long? OrderTurnaroundTime { get; set; }

		[JsonProperty("AdvancedDeposits")]
		public bool? AdvancedDeposits { get; set; }

		[JsonProperty("DepositPct")]
		public long? DepositPct { get; set; }

		[JsonProperty("DepositAmt")]
		public long? DepositAmt { get; set; }

		[JsonProperty("WarehouseAddress")]
		public string WarehouseAddress { get; set; }

		[JsonProperty("WarehouseCity")]
		public string WarehouseCity { get; set; }

		[JsonProperty("WarehouseState")]
		public string WarehouseState { get; set; }

		[JsonProperty("WarehouseCountry")]
		public string WarehouseCountry { get; set; }

		[JsonProperty("WarehouseZipCode")]
		public string WarehouseZipCode { get; set; }

		[JsonProperty("OwnProductAtTimeOfSale")]
		public bool? OwnProductAtTimeOfSale { get; set; }

		[JsonProperty("OtherCompaniesInvolved")]
		public bool? OtherCompaniesInvolved { get; set; }

		[JsonProperty("OtherCompaniesInvolvedInfo")]
		public string OtherCompaniesInvolvedInfo { get; set; }

		[JsonProperty("AdvertiseInfo")]
		public string AdvertiseInfo { get; set; }

		[JsonProperty("WhoEntersCardInfo")]
		public string WhoEntersCardInfo { get; set; }

		[JsonProperty("IsProcessingSeasonal")]
		public bool? IsProcessingSeasonal { get; set; }

		[JsonProperty("ProcessingSeasonalMonth1")]
		public bool? ProcessingSeasonalMonth1 { get; set; }

		[JsonProperty("ProcessingSeasonalMonth2")]
		public bool? ProcessingSeasonalMonth2 { get; set; }

		[JsonProperty("ProcessingSeasonalMonth3")]
		public bool? ProcessingSeasonalMonth3 { get; set; }

		[JsonProperty("ProcessingSeasonalMonth4")]
		public bool? ProcessingSeasonalMonth4 { get; set; }

		[JsonProperty("ProcessingSeasonalMonth5")]
		public bool? ProcessingSeasonalMonth5 { get; set; }

		[JsonProperty("ProcessingSeasonalMonth6")]
		public bool? ProcessingSeasonalMonth6 { get; set; }

		[JsonProperty("ProcessingSeasonalMonth7")]
		public bool? ProcessingSeasonalMonth7 { get; set; }

		[JsonProperty("ProcessingSeasonalMonth8")]
		public bool? ProcessingSeasonalMonth8 { get; set; }

		[JsonProperty("ProcessingSeasonalMonth9")]
		public bool? ProcessingSeasonalMonth9 { get; set; }

		[JsonProperty("ProcessingSeasonalMonth10")]
		public bool? ProcessingSeasonalMonth10 { get; set; }

		[JsonProperty("ProcessingSeasonalMonth11")]
		public bool? ProcessingSeasonalMonth11 { get; set; }

		[JsonProperty("ProcessingSeasonalMonth12")]
		public bool? ProcessingSeasonalMonth12 { get; set; }

		[JsonProperty("MembershipPayments")]
		public bool? MembershipPayments { get; set; }

		[JsonProperty("MembershipPaymentsPct")]
		public long? MembershipPaymentsPct { get; set; }

		[JsonProperty("PackageUsageTime")]
		public long? PackageUsageTime { get; set; }

		[JsonProperty("WhoManagesRecurringPayments")]
		public string WhoManagesRecurringPayments { get; set; }

		[JsonProperty("RecurringPaymentProvider")]
		public string RecurringPaymentProvider { get; set; }

		[JsonProperty("CaptureAVS")]
		public bool? CaptureAvs { get; set; }

		[JsonProperty("CaptureAVSAction")]
		public string CaptureAvsAction { get; set; }

		[JsonProperty("CaptureCVV")]
		public bool? CaptureCvv { get; set; }

		[JsonProperty("PerformVerifiedByVisa")]
		public bool? PerformVerifiedByVisa { get; set; }

		[JsonProperty("DoPayArcFraudCheck")]
		public bool? DoPayArcFraudCheck { get; set; }

		[JsonProperty("")]
		public string Empty { get; set; }
	}

	public partial class TsQuestionery {
		[JsonProperty("MerchantCurvPos")]
		public long? MerchantCurvPos { get; set; }

		[JsonProperty("TerminalType")]
		public List<string> TerminalType { get; set; }

		[JsonProperty("TerminalGatewaySetup")]
		public string TerminalGatewaySetup { get; set; }

		[JsonProperty("TerminalGateway")]
		public string TerminalGateway { get; set; }

		[JsonProperty("TerminalPinDebit")]
		public string TerminalPinDebit { get; set; }

		[JsonProperty("TerminalEbtFns")]
		public string TerminalEbtFns { get; set; }

		[JsonProperty("TerminalPrimaryMethod")]
		public string TerminalPrimaryMethod { get; set; }

		[JsonProperty("TerminalEmvQuantity")]
		public TerminalEmvQuantity TerminalEmvQuantity { get; set; }

		[JsonProperty("TerminalPaxPos")]
		public string TerminalPaxPos { get; set; }

		[JsonProperty("TerminalDejavooPosDebit")]
		public string TerminalDejavooPosDebit { get; set; }

		[JsonProperty("TerminalDejavooPosCash")]
		public string TerminalDejavooPosCash { get; set; }

		[JsonProperty("TerminalDejavooPosGift")]
		public string TerminalDejavooPosGift { get; set; }

		[JsonProperty("TerminalBatchAutoClose")]
		public string TerminalBatchAutoClose { get; set; }

		[JsonProperty("TerminalOptionTip")]
		public string TerminalOptionTip { get; set; }

		[JsonProperty("TerminalTrackClerks")]
		public string TerminalTrackClerks { get; set; }

		[JsonProperty("TerminalOptionInvoice")]
		public string TerminalOptionInvoice { get; set; }

		[JsonProperty("TerminalOptionTax")]
		public string TerminalOptionTax { get; set; }

		[JsonProperty("TerminalCityTaxPct")]
		public long? TerminalCityTaxPct { get; set; }

		[JsonProperty("TerminalStateTaxPct")]
		public long? TerminalStateTaxPct { get; set; }

		[JsonProperty("TerminalOptionSignature")]
		public string TerminalOptionSignature { get; set; }

		[JsonProperty("TerminalHeader")]
		public string TerminalHeader { get; set; }

		[JsonProperty("TerminalFooter")]
		public string TerminalFooter { get; set; }

		[JsonProperty("TerminalDeployment")]
		public string TerminalDeployment { get; set; }

		[JsonProperty("TerminalShippingName")]
		public string TerminalShippingName { get; set; }

		[JsonProperty("TerminalShippingContactName")]
		public string TerminalShippingContactName { get; set; }

		[JsonProperty("TerminalShippingAddress")]
		public string TerminalShippingAddress { get; set; }

		[JsonProperty("TerminalShippingCity")]
		public string TerminalShippingCity { get; set; }

		[JsonProperty("TerminalShippingState")]
		public string TerminalShippingState { get; set; }

		[JsonProperty("TerminalShippingZipCode")]
		public string TerminalShippingZipCode { get; set; }

		[JsonProperty("TerminalShippingPhoneNo")]
		public string TerminalShippingPhoneNo { get; set; }

		[JsonProperty("TerminalShippingPreferred")]
		public string TerminalShippingPreferred { get; set; }

		[JsonProperty("TerminalComments")]
		public string TerminalComments { get; set; }
	}

	public partial class TerminalEmvQuantity {
		[JsonProperty("dejavoo")]
		public System.Collections.Generic.Dictionary<string, long> Dejavoo { get; set; }

		[JsonProperty("mobileReaders")]
		public MobileReaders MobileReaders { get; set; }

		[JsonProperty("pax")]
		public Pax Pax { get; set; }
	}

	public partial class MobileReaders {
		[JsonProperty("b250")]
		public long? B250 { get; set; }

		[JsonProperty("bbpos")]
		public long? Bbpos { get; set; }

		[JsonProperty("idynamo5")]
		public long? Idynamo5 { get; set; }
	}

	public partial class Pax {
		[JsonProperty("a80")]
		public long? A80 { get; set; }

		[JsonProperty("a920")]
		public long? A920 { get; set; }

		[JsonProperty("a920pro")]
		public long? A920Pro { get; set; }

		[JsonProperty("sp30")]
		public long? Sp30 { get; set; }

		[JsonProperty("sp300")]
		public long? Sp300 { get; set; }
	}

	//public partial class ApplicationInfoData {
	//	public static ApplicationInfoData FromJson(string json) => JsonConvert.DeserializeObject<ApplicationInfoData>(json, ApplicationService.Converter.Settings);
	//}

	//public static class Serialize {
	//	public static string ToJson(this ApplicationInfoData self) => JsonConvert.SerializeObject(self, ApplicationService.Converter.Settings);
	//}

	//internal static class Converter {
	//	public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings {
	//		MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
	//		DateParseHandling = DateParseHandling.None,
	//		Converters =
	//		{
	//			new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
	//		},
	//	};
	//}

	//internal class ParseStringConverter : JsonConverter {
	//	public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

	//	public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer) {
	//		if (reader.TokenType == JsonToken.Null) return null;
	//		var value = serializer.Deserialize<string>(reader);
	//		long l;
	//		if (Int64.TryParse(value, out l)) {
	//			return l;
	//		}
	//		throw new Exception("Cannot unmarshal type long");
	//	}

	//	public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer) {
	//		if (untypedValue == null) {
	//			serializer.Serialize(writer, null);
	//			return;
	//		}
	//		var value = (long)untypedValue;
	//		serializer.Serialize(writer, value.ToString());
	//		return;
	//	}

	//	public static readonly ParseStringConverter Singleton = new ParseStringConverter();
	//}
}
