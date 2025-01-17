using PayarcSDK.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace PayarcSDK.Services {
	public class ChargeService {
		private readonly ApiClient _apiClient;

		public ChargeService(ApiClient apiClient) {
			_apiClient = apiClient;
		}

		//public async Task<ChargeResponse> CreateChargeAsync(object obj, ChargeRequest chargeRequest) {
		//	if (chargeRequest == null) {
		//		chargeRequest = obj as ChargeRequest ?? throw new ArgumentNullException(nameof(chargeRequest));
		//	}

		//	ChargeDataRequest chargeData = new ChargeDataRequest();
		//	chargeData.amount = chargeRequest.amount;
		//	chargeData.currency = chargeRequest.currency;
		//	// Process source and customer data
		//	if (!String.IsNullOrEmpty(chargeRequest.source.token_id)) {
		//		chargeData.token_id = chargeRequest.source.token_id;
		//	} else if (!String.IsNullOrEmpty(chargeRequest.source.customer_id)) {
		//		chargeData.customer_id = chargeRequest.source.customer_id;
		//	} else if (!String.IsNullOrEmpty(chargeRequest.source.card_id)) {
		//		chargeData.card_id = chargeRequest.source.card_id;
		//	}// else if (chargeData.Source.StartsWith("bnk_") || chargeData.SecCode != null) {
		//	 //	chargeData.BankAccountId = chargeData.Source.StartsWith("bnk_") ? chargeData.Source[4..] : chargeData.BankAccountId;
		//	 //	chargeData.Type = "debit";
		//	 //	return await _apiClient.PostAsync<ChargeRequest, ChargeResponse>("achcharges", chargeData);
		//	 //}

		//	// Remove source to avoid duplication
		//	chargeRequest.source = null;

		//	// Call API
		//	return await _apiClient.PostAsync<ChargeDataRequest, ChargeResponse>("charges", chargeData);
		//}

		//public async Task<ChargeResponse> RetrieveChargeAsync(string chargeId) {
		//	if (chargeId.StartsWith("ch_")) {
		//		chargeId = chargeId[3..];
		//		return await _apiClient.GetAsync<ChargeResponse>($"charges/{chargeId}?include=transaction_metadata,extra_metadata");
		//	} else if (chargeId.StartsWith("ach_")) {
		//		chargeId = chargeId[4..];
		//		return await _apiClient.GetAsync<ChargeResponse>($"achcharges/{chargeId}?include=review");
		//	}

		//	throw new ArgumentException("Invalid charge ID format.", nameof(chargeId));
		//}

		//public async Task<ChargeListResponse> ListChargesAsync(ChargeListRequest request) {
		//	var queryParams = new Dictionary<string, string>
		//	{
		//	{ "limit", request.Limit.ToString() },
		//	{ "page", request.Page.ToString() }
		//};

		//	if (request.Search != null) {
		//		foreach (var kvp in request.Search) {
		//			queryParams[kvp.Key] = kvp.Value;
		//		}
		//	}

		//	return await _apiClient.GetAsync<ChargeListResponse>("charges", queryParams);
		//}

		//public async Task<ChargeResponse> RefundChargeAsync(string chargeId, RefundRequest refundData) {
		//	if (chargeId.StartsWith("ch_")) {
		//		chargeId = chargeId[3..];
		//		return await _apiClient.PostAsync<RefundRequest, ChargeResponse>($"charges/{chargeId}/refunds", refundData);
		//	} else if (chargeId.StartsWith("ach_")) {
		//		return await RefundAchChargeAsync(chargeId, refundData);
		//	}

		//	throw new ArgumentException("Invalid charge ID format.", nameof(chargeId));
		//}

		//private async Task<ChargeResponse> RefundAchChargeAsync(string chargeId, RefundRequest refundData) {
		//	chargeId = chargeId[4..];
		//	refundData.Type = "credit";
		//	return await _apiClient.PostAsync<RefundRequest, ChargeResponse>("achcharges", refundData);
		//}

		public async Task<JObject> GetAllAccountsAsync() {
			return await _apiClient.GetAsync("accounts");
		}
	}
}
