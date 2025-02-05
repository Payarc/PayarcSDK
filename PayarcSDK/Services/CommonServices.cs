using Newtonsoft.Json;
using PayarcSDK.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JsonException = System.Text.Json.JsonException;
using JsonSerializer = System.Text.Json.JsonSerializer;
using PayarcSDK.Entities.Billing.Subscriptions;
using PayarcSDK.Entities.Dispute;

namespace PayarcSDK.Services {
	public class CommonServices {
		
		private readonly HttpClient _httpClient;
		
		public CommonServices(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}
		public string BuildQueryString(Dictionary<string, object?> parameters)
		{
			var queryString = string.Join("&",
				parameters.Select(p =>
					$"{Uri.EscapeDataString(p.Key)}={Uri.EscapeDataString(p.Value?.ToString() ?? string.Empty)}"));
			return queryString;
		}
		public BaseResponse? TransformJsonRawObject(Dictionary<string, object> obj, string? rawObj, string type = "object")
		{
			BaseResponse? response = null;
			if (rawObj != null) {
				if (type == "Plan") {
					var planResponse = JsonConvert.DeserializeObject<PlanResponseData>(rawObj) ?? new PlanResponseData();
					planResponse.RawData = rawObj;
					if (obj.ContainsKey("plan_id") && obj["plan_id"]?.ToString() != null) {
						var planService = new PlanService(_httpClient);
						planResponse.Object = "Plan";
						planResponse.ObjectId ??= $"{obj["plan_id"]}";
						planResponse.Retrieve = async () => await planService.Retrieve(planResponse);
						planResponse.Update = async (newData) => await planService.Update(planResponse, newData);
						planResponse.Delete = async () => await planService.Delete(planResponse);
						planResponse.CreateSubscription = async (newData) => await planService.CreateSubscription(planResponse, newData);
					}
					response = planResponse;
				} else if (type == "Subscription") {
					var subService = new SubscriptionService(_httpClient);
					var subResponse = JsonConvert.DeserializeObject<SubscriptionResponseData>(rawObj) ?? new SubscriptionResponseData();
					subResponse.RawData = rawObj;
					subResponse.ObjectId ??= $"sub_{obj["id"]}";
					subResponse.Update = async (newData) => await subService.Update(subResponse, newData);
					subResponse.Cancel = async () => await subService.Cancel(subResponse);
					response = subResponse;
				} else if (type == "Charge") {
					ChargeService chargeService = new ChargeService(_httpClient);
					var chargeResponse = JsonConvert.DeserializeObject<ChargeResponseData>(rawObj) ?? new ChargeResponseData();
					chargeResponse.RawData = rawObj;
					chargeResponse.ObjectId ??= $"ch_{obj["id"]}";
					chargeResponse.CreateRefund = async (chargeData) => await chargeService.CreateRefund(chargeResponse, chargeData);
					response = chargeResponse;
				} else if (type == "ACHCharge") {
					ChargeService chargeService = new ChargeService(_httpClient);
					var achChargeResponse = JsonConvert.DeserializeObject<AchChargeResponseData>(rawObj) ?? new AchChargeResponseData();
					achChargeResponse.RawData = rawObj;
					achChargeResponse.ObjectId ??= $"ach_{obj["id"]}";
					achChargeResponse.CreateRefund = async (chargeData) => await chargeService.CreateRefund(achChargeResponse, chargeData);
					if (achChargeResponse.BankAccount?.Data != null) achChargeResponse.BankAccount.Data.ObjectId = $"bnk_{obj["id"]}";
					response = achChargeResponse;
				} else if (type == "Customer") {
					CustomerService customerService = new CustomerService(_httpClient);
					var customerResponse = JsonConvert.DeserializeObject<CustomerResponseData>(rawObj) ?? new CustomerResponseData();
					customerResponse.RawData = rawObj;
					customerResponse.ObjectId ??= $"cus_{obj["customer_id"]}";
					customerResponse.Update = async (customerData) => {
						var result = await customerService.Update(customerResponse, customerData);
						return JsonConvert.DeserializeObject<CustomerResponseData>(result.ToString());
					};
					response = customerResponse;
				} else if (type == "Token") {
					var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(rawObj) ?? new TokenResponse();
					tokenResponse.RawData = rawObj;
					tokenResponse.ObjectId ??= $"tok_{obj["id"]}";
					response = tokenResponse;
				} else if (type == "ApplyApp") {
					var applicationResponse = JsonConvert.DeserializeObject<ApplicationResponseData>(rawObj) ?? new ApplicationResponseData();
					applicationResponse.RawData = rawObj;
					applicationResponse.ObjectId ??= $"appl_{obj["id"]}";
					applicationResponse.Documents?.ForEach(doc => {
						doc.ObjectId = $"doc_{doc.Id}";
					});
					response = applicationResponse;
				} else if (type == "MerchantCode") {
                    var documentChangeResponse = JsonConvert.DeserializeObject<DocumentChangeResponse>(rawObj) ?? new DocumentChangeResponse();
                    documentChangeResponse.RawData = rawObj;
                    documentChangeResponse.ObjectId ??= $"appl_{obj["MerchantCode"]}";
                    documentChangeResponse.Object = "ApplyApp";
                    documentChangeResponse.MerchantDocuments?.ForEach(doc => {
						doc.Object = "ApplyDocuments";
                        doc.ObjectId = $"doc_{doc.DocumentCode}";
                    });
                    obj.Remove("MerchantCode");
					response = documentChangeResponse;
				} else if (type == "DocumentCode") {

                    var documentChangeResponse = JsonConvert.DeserializeObject<DocumentChangeResponse>(rawObj) ?? new DocumentChangeResponse();
                    documentChangeResponse.RawData = rawObj;
                    documentChangeResponse.Object = "ApplyApp";
                    documentChangeResponse.MerchantDocuments?.ForEach(doc => {
                        doc.Object = "ApplyDocuments";
                        doc.ObjectId = $"doc_{doc.DocumentCode}";
                    });
                    obj.Remove("MerchantDocuments");
					response = documentChangeResponse;
				} else if (type == "ApplyDocuments") {
					//CustomerService customerService = new CustomerService(_httpClient);
					var documentResponse = JsonConvert.DeserializeObject<DocumentResponseData>(rawObj) ?? new DocumentResponseData();
					documentResponse.RawData = rawObj;
					documentResponse.ObjectId ??= $"doc_{obj["id"]}";
					//customerResponse.Update = async (customerData) => {
					//	var result = await customerService.Update(customerResponse, customerData);
					//	return JsonConvert.DeserializeObject<CustomerResponseData>(result.ToString());
					//};
					response = documentResponse;
				} else if (type == "User") {
                    //CustomerService customerService = new CustomerService(_httpClient);
                    var documentResponse = JsonConvert.DeserializeObject<SubAgentResponseData>(rawObj) ?? new SubAgentResponseData();
                    documentResponse.RawData = rawObj;
                    documentResponse.ObjectId ??= $"usr_{obj["id"]}";
                    //customerResponse.Update = async (customerData) => {
                    //	var result = await customerService.Update(customerResponse, customerData);
                    //	return JsonConvert.DeserializeObject<CustomerResponseData>(result.ToString());
                    //};
                    response = documentResponse;
                } else if (type == "Cases") {
					//CustomerService customerService = new CustomerService(_httpClient);
					var disputeCaseResponse = JsonConvert.DeserializeObject<DisputeCasesResponseData>(rawObj) ?? new DisputeCasesResponseData();
					disputeCaseResponse.RawData = rawObj;
					disputeCaseResponse.Object = "Dispute";
					disputeCaseResponse.ObjectId ??= $"dis_{obj["id"]}";
					//customerResponse.Update = async (customerData) => {
					//	var result = await customerService.Update(customerResponse, customerData);
					//	return JsonConvert.DeserializeObject<CustomerResponseData>(result.ToString());
					//};
					response = disputeCaseResponse;
				} else if (type == "CaseFile") {
					//CustomerService customerService = new CustomerService(_httpClient);
					var disputeCaseResponse = JsonConvert.DeserializeObject<DisputeCaseFileData>(rawObj) ?? new DisputeCaseFileData();
					disputeCaseResponse.RawData = rawObj;
					disputeCaseResponse.ObjectId ??= $"cfl_{obj["id"]}";
					//customerResponse.Update = async (customerData) => {
					//	var result = await customerService.Update(customerResponse, customerData);
					//	return JsonConvert.DeserializeObject<CustomerResponseData>(result.ToString());
					//};
					response = disputeCaseResponse;
				} else if (type == "Evidence") {
					//CustomerService customerService = new CustomerService(_httpClient);
					var disputeCaseResponse = JsonConvert.DeserializeObject<DisputeEvidenceData>(rawObj) ?? new DisputeEvidenceData();
					disputeCaseResponse.RawData = rawObj;
					disputeCaseResponse.ObjectId ??= $"evd_{obj["id"]}";
					//customerResponse.Update = async (customerData) => {
					//	var result = await customerService.Update(customerResponse, customerData);
					//	return JsonConvert.DeserializeObject<CustomerResponseData>(result.ToString());
					//};
					response = disputeCaseResponse;
				} else if (type == "CaseSubmission") {
					//CustomerService customerService = new CustomerService(_httpClient);
					var disputeCaseResponse = JsonConvert.DeserializeObject<DisputeCaseSubmissionData>(rawObj) ?? new DisputeCaseSubmissionData();
					disputeCaseResponse.RawData = rawObj;
					disputeCaseResponse.ObjectId ??= $"sbm_{obj["id"]}";
					//customerResponse.Update = async (customerData) => {
					//	var result = await customerService.Update(customerResponse, customerData);
					//	return JsonConvert.DeserializeObject<CustomerResponseData>(result.ToString());
					//};
					response = disputeCaseResponse;
				} else if (type == "Campaign") {
					//CustomerService customerService = new CustomerService(_httpClient);
					var disputeCaseResponse = JsonConvert.DeserializeObject<CampaignResponseData>(rawObj) ?? new CampaignResponseData();
					disputeCaseResponse.RawData = rawObj;
					disputeCaseResponse.ObjectId ??= $"cmp_{obj["id"]}";
					//customerResponse.Update = async (customerData) => {
					//	var result = await customerService.Update(customerResponse, customerData);
					//	return JsonConvert.DeserializeObject<CustomerResponseData>(result.ToString());
					//};
					response = disputeCaseResponse;
				}
			}
			return response;
		}
	}
}
