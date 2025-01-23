using PayarcSDK.Http;

namespace PayarcSDK.Services {
	public class ChargeService {
		private readonly ApiClient _apiClient;

		public ChargeService(ApiClient apiClient) {
			_apiClient = apiClient;
		}
	}
}
