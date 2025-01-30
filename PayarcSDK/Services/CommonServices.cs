using Newtonsoft.Json;
using PayarcSDK.Entities;
using PayarcSDK.Entities.CustomerService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JsonException = System.Text.Json.JsonException;
using JsonSerializer = System.Text.Json.JsonSerializer;
using PayarcSDK.Entities.Billing.Subscriptions;

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
			if (rawObj != null)
			{
				if (type == "Plan")
				{
					var planResponse = JsonConvert.DeserializeObject<PlanResponseData>(rawObj) ?? new PlanResponseData();
					planResponse.RawData = rawObj;
					if (obj.ContainsKey("plan_id") && obj["plan_id"]?.ToString() != null)
					{
						var planService = new PlanService(_httpClient);
						planResponse.Object = "Plan";
						planResponse.ObjectId ??= $"{obj["plan_id"]}";
						planResponse.Retrieve = async() => await planService.Retrieve(planResponse);
						planResponse.Update = async (newData) => await planService.Update(planResponse, newData);
						planResponse.Delete = async() => await planService.Delete(planResponse);
						planResponse.CreateSubscription = async (newData) => await planService.CreateSubscription(planResponse, newData);
					}
					response = planResponse;
				}
				else if (type == "Subscription")
				{
					var subService = new SubscriptionService(_httpClient);
					var subResponse = JsonConvert.DeserializeObject<SubscriptionResponseData>(rawObj) ?? new SubscriptionResponseData();
					subResponse.RawData = rawObj;
					subResponse.ObjectId ??= $"sub_{obj["id"]}";
					subResponse.Update = async (newData) => await subService.Update(subResponse, newData);
					subResponse.Cancel = async () => await subService.Cancel(subResponse);
					response = subResponse;
				}
				else if (type == "Charge")
				{
					ChargeService chargeService = new ChargeService(_httpClient);
					var chargeResponse = JsonConvert.DeserializeObject<ChargeResponseData>(rawObj) ?? new ChargeResponseData();
					chargeResponse.RawData = rawObj;
					chargeResponse.ObjectId ??= $"ch_{obj["id"]}";
					chargeResponse.CreateRefund = async (chargeData) => await chargeService.CreateRefund(chargeResponse, chargeData);
					response = chargeResponse;
				}
				else if (type == "ACHCharge")
				{
					ChargeService chargeService = new ChargeService(_httpClient);
					var achChargeResponse = JsonConvert.DeserializeObject<AchChargeResponseData>(rawObj) ?? new AchChargeResponseData();
					achChargeResponse.RawData = rawObj;
					achChargeResponse.ObjectId ??= $"ach_{obj["id"]}";
					achChargeResponse.CreateRefund = async (chargeData) => await chargeService.CreateRefund(achChargeResponse, chargeData);
					if (achChargeResponse.BankAccount?.Data != null) achChargeResponse.BankAccount.Data.ObjectId = $"bnk_{obj["id"]}";
					response = achChargeResponse;
				}
				else if (obj["object"]?.ToString() == "customer") {
					CustomerService customerService = new CustomerService(_httpClient);
					var customerResponse = JsonConvert.DeserializeObject<CustomerResponseData>(rawObj) ?? new CustomerResponseData();
					customerResponse.RawData = rawObj;
					customerResponse.ObjectId ??= $"cus_{obj["customer_id"]}";
					customerResponse.Update = async (customerData) => {
						var result = await customerService.Update(customerResponse, customerData);
						return JsonConvert.DeserializeObject<CustomerResponseData>(result.ToString());
					};
					response = customerResponse;
				} else if (obj["object"]?.ToString() == "Token") {
					var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(rawObj) ?? new TokenResponse();
					tokenResponse.RawData = rawObj;
					tokenResponse.ObjectId ??= $"tok_{obj["id"]}";
					response = tokenResponse;
				}
			}
       
			return response;
		}
	}
}
