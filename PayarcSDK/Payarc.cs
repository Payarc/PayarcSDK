using PayarcSDK.Configuration;
using PayarcSDK.Services;

namespace PayarcSDK
{
    public class Payarc
    {
        private readonly HttpClient _httpClient;
        private readonly CommonServices _commonServices;

        public ApplicationService ApplicationService { get; }
        public DisputeService DisputeService { get; }
        public SplitCampaignService SplitCampaignService { get; }
        public CustomerService CustomerService { get; }
        public ChargeService Charges { get; }
        public BillingService Billing { get; }
        public PayarcConnectService PayarcConnect { get; }

        /// <summary>
        /// Initializes the Payarc client with the given base URL and API key.
        /// </summary>
        /// <param name="httpClient">An instance of HttpClient to be used for API requests.</param>
        public Payarc(HttpClient httpClient, SdkConfiguration config)
        {
            // Initialize the ApiClient
            _apiClient = new ApiClient(httpClient);

            // Instantiate the services
            ApplicationService = new ApplicationService(httpClient);
            DisputeService = new DisputeService(httpClient);
            SplitCampaignService = new SplitCampaignService(httpClient);
            CustomerService = new CustomerService(httpClient);
            Charges = new ChargeService(httpClient);
            Billing = new BillingService(httpClient);
            PayarcConnect = new PayarcConnectService(httpClient, config);
        }

        /// <summary>
        /// A method to ensure the Payarc client is working correctly (e.g., for health checks).
        /// </summary>
        /// <returns>A success message to indicate connectivity.</returns>
        public string TestConnection()
        {
            return "Payarc client initialized successfully.";
        }
    }
}