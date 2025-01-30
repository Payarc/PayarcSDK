using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using AnyOfTypes;
using PayarcSDK.Entities;

namespace PayarcSDK.Entities
{
    public class ChargeResponseData : BaseResponse
    {
        
        [JsonProperty("object")]
        public override string? Object { get; set; }

        [JsonProperty("object_id")]
        public override string? ObjectId { get; set; }

        [JsonIgnore]
        public Func< Dictionary<string, object>?, Task<BaseResponse?>> CreateRefund { get; set; }
        
        [JsonProperty("id")]
        public new string? Id { get; set; }

        [JsonProperty("amount")]
        public decimal? Amount { get; set; }

        [JsonProperty("amount_approved")]
        public decimal? AmountApproved { get; set; }

        [JsonProperty("amount_refunded")]
        public decimal? AmountRefunded { get; set; }

        [JsonProperty("amount_captured")]
        public decimal? AmountCaptured { get; set; }

        [JsonProperty("amount_voided")]
        public decimal? AmountVoided { get; set; }

        [JsonProperty("application_fee_amount")]
        public decimal? ApplicationFeeAmount { get; set; }

        [JsonProperty("tip_amount")]
        public decimal? TipAmount { get; set; }

        [JsonProperty("payarc_fees")]
        public decimal? PayarcFees { get; set; }

        [JsonProperty("type")]
        public string? Type { get; set; }

        [JsonProperty("customer_email")]
        public string? CustomerEmail { get; set; }

        [JsonProperty("net_amount")]
        public decimal? NetAmount { get; set; }

        [JsonProperty("captured")]
        public bool? Captured { get; set; }

        [JsonProperty("is_refunded")]
        public bool? IsRefunded { get; set; }

        [JsonProperty("status")]
        public string? Status { get; set; }

        [JsonProperty("auth_code")]
        public string? AuthCode { get; set; }

        [JsonProperty("failure_code")]
        public string? FailureCode { get; set; }

        [JsonProperty("failure_message")]
        public string? FailureMessage { get; set; }

        [JsonProperty("charge_description")]
        public string? ChargeDescription { get; set; }

        [JsonProperty("kount_details")]
        public string? KountDetails { get; set; }

        [JsonProperty("kount_status")]
        public string? KountStatus { get; set; }

        [JsonProperty("statement_description")]
        public string? StatementDescription { get; set; }

        [JsonProperty("invoice")]
        public string? Invoice { get; set; }

        [JsonProperty("under_review")]
        public bool? UnderReview { get; set; }

        [JsonProperty("created_at")]
        public string? CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public string? UpdatedAt { get; set; }

        [JsonProperty("email")]
        public string? Email { get; set; }

        [JsonProperty("phone_number")]
        public string? PhoneNumber { get; set; }

        // Card Level
        [JsonProperty("card_level")]
        public string? CardLevel { get; set; }

        // Level 2
        [JsonProperty("sales_tax")]
        public decimal? SalesTax { get; set; }

        [JsonProperty("purchase_order")]
        public string? PurchaseOrder { get; set; }

        [JsonProperty("supplier_reference_number")]
        public string? SupplierReferenceNumber { get; set; }

        [JsonProperty("customer_ref_id")]
        public string? CustomerRefId { get; set; }

        [JsonProperty("ship_to_zip")]
        public string? ShipToZip { get; set; }

        [JsonProperty("amex_descriptor")]
        public string? AmexDescriptor { get; set; }

        // Level 3
        [JsonProperty("customer_vat_number")]
        public string? CustomerVatNumber { get; set; }

        [JsonProperty("summary_commodity_code")]
        public string? SummaryCommodityCode { get; set; }

        [JsonProperty("shipping_charges")]
        public decimal? ShippingCharges { get; set; }

        [JsonProperty("duty_charges")]
        public decimal? DutyCharges { get; set; }

        [JsonProperty("ship_from_zip")]
        public string? ShipFromZip { get; set; }

        [JsonProperty("destination_country_code")]
        public string? DestinationCountryCode { get; set; }

        [JsonProperty("vat_invoice")]
        public string? VatInvoice { get; set; }

        [JsonProperty("order_date")]
        public string? OrderDate { get; set; }

        [JsonProperty("tax_category")]
        public string? TaxCategory { get; set; }

        [JsonProperty("tax_type")]
        public string? TaxType { get; set; }

        [JsonProperty("tax_rate")]
        public decimal? TaxRate { get; set; }

        [JsonProperty("tax_amount")]
        public decimal? TaxAmount { get; set; }

        [JsonProperty("created_by")]
        public string? CreatedBy { get; set; }

        [JsonProperty("terminal_register")]
        public TerminalRegister? TerminalRegister { get; set; }

        [JsonProperty("amex_level3")]
        public List<string>? AmexLevel3 { get; set; }

        // Refunded
        [JsonProperty("tip_amount_refunded")]
        public decimal? TipAmountRefunded { get; set; }

        [JsonProperty("sales_tax_refunded")]
        public decimal? SalesTaxRefunded { get; set; }

        [JsonProperty("shipping_charges_refunded")]
        public decimal? ShippingChargesRefunded { get; set; }

        [JsonProperty("duty_charges_refunded")]
        public decimal? DutyChargesRefunded { get; set; }

        [JsonProperty("pax_reference_number")]
        public string? PaxReferenceNumber { get; set; }

        [JsonProperty("refund_reason")]
        public string? RefundReason { get; set; }

        [JsonProperty("refund_description")]
        public string? RefundDescription { get; set; }

        [JsonProperty("surcharge")]
        public decimal? Surcharge { get; set; }

        [JsonProperty("toll_amount")]
        public decimal? TollAmount { get; set; }

        [JsonProperty("airport_fee")]
        public decimal? AirportFee { get; set; }

        [JsonProperty("health_care")]
        public bool? HealthCare { get; set; }

        [JsonProperty("health_care_type")]
        public string? HealthCareType { get; set; }

        [JsonProperty("prescription_amount")]
        public decimal? PrescriptionAmount { get; set; }

        [JsonProperty("vision_amount")]
        public decimal? VisionAmount { get; set; }

        [JsonProperty("clinic_amount")]
        public decimal? ClinicAmount { get; set; }

        [JsonProperty("dental_amount")]
        public decimal? DentalAmount { get; set; }

        [JsonProperty("industry_type")]
        public string? IndustryType { get; set; }

        [JsonProperty("void_reason")]
        public string? VoidReason { get; set; }

        [JsonProperty("void_description")]
        public string? VoidDescription { get; set; }

        [JsonProperty("server_id")]
        public string? ServerId { get; set; }

        [JsonProperty("external_invoice_id")]
        public string? ExternalInvoiceId { get; set; }

        [JsonProperty("external_order_id")]
        public string? ExternalOrderId { get; set; }

        // EMV Sale Data
        [JsonProperty("tsys_response_code")]
        public string? TsysResponseCode { get; set; }

        [JsonProperty("host_response_code")]
        public string? HostResponseCode { get; set; }

        [JsonProperty("host_response_message")]
        public string? HostResponseMessage { get; set; }

        [JsonProperty("emv_issuer_scripts")]
        public string? EmvIssuerScripts { get; set; }

        [JsonProperty("emv_issuer_authentication_data")]
        public string? EmvIssuerAuthenticationData { get; set; }

        [JsonProperty("host_reference_number")]
        public string? HostReferenceNumber { get; set; }

        [JsonProperty("sale_terminal_id")]
        public string? SaleTerminalId { get; set; }

        [JsonProperty("sale_mid")]
        public string? SaleMid { get; set; }

        [JsonProperty("edc_type")]
        public string? EdcType { get; set; }

        [JsonProperty("ecr_reference_number")]
        public string? EcrReferenceNumber { get; set; }
        [JsonProperty("refund")]
        public RefundsWrapper? RefundsWrapper { get; set; }
        [JsonProperty("card")]
        public CardWrapper? CardWrapper { get; set; }

        // Optional: to handle null values like `do_not_send_email_to_customer` dynamically
        [JsonProperty("do_not_send_email_to_customer")]
        public bool? DoNotSendEmailToCustomer { get; set; }

        [JsonProperty("do_not_send_sms_to_customer")]
        public bool? DoNotSendSmsToCustomer { get; set; }
        
    }
}

public class CardWrapper
{
    [JsonProperty("data")]
    public Card? Card { get; set; }
}

public class RefundsWrapper
{
    [JsonProperty("data")]
    public List<Refund>? Refunds { get; set; }
}
// public class RefundWrapper
// {
//     // [JsonProperty("data")]
//     public Refund? Refund { get; set; }
// }
