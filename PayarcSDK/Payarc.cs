using PayarcSDK.Services;

namespace PayarcSDK
{
    public class Payarc {
		private readonly HttpClient _httpClient;
		private readonly CommonServices _commonServices;

		public ApplicationService ApplicationService { get; }
		public DisputeService DisputeService { get; }
		public SplitCampaignService SplitCampaignService { get; }
		public CustomerService CustomerService { get; }
		public ChargeService Charges { get; }

		public Payarc(HttpClient httpClient) {
			// Instantiate the services
			ApplicationService = new ApplicationService(httpClient);
			DisputeService = new DisputeService(httpClient);
			SplitCampaignService = new SplitCampaignService(httpClient);
			CustomerService = new CustomerService(httpClient);
			Charges = new ChargeService(httpClient);
		}

		public string TestConnection() {
			return "Payarc client initialized successfully.";
		}
	}
}
