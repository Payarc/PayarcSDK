using PayarcSDK.Configuration;
using PayarcSDK.Services;

namespace PayarcSDK {
    public class Payarc {
        public ApplicationService Applications { get; }
        public PayeeService Payees { get; }
        public DisputeService Disputes { get; }
        public SplitCampaignService SplitCampaigns { get; }
        public CustomerService Customers { get; }
        public ChargeService Charges { get; }
        public BillingService Billing { get; }
        public BatchService Batches { get; }
		public DepositService Deposits { get; }
        public InstructionalFundingService InstructionalFunding { get; }
        public PayarcConnectService PayarcConnect { get; }

        /// <summary>
        /// Initializes the Payarc client with the given base URL and API key.
        /// </summary>
        /// <param name="httpClient">An instance of HttpClient to be used for API requests.</param>
        public Payarc(HttpClient httpClient, SdkConfiguration config) {
            // Instantiate the services
            Applications = new ApplicationService(httpClient);
            Payees = new PayeeService(httpClient);
            Deposits = new DepositService(httpClient);
			Disputes = new DisputeService(httpClient);
            SplitCampaigns = new SplitCampaignService(httpClient);
            Customers = new CustomerService(httpClient);
            Charges = new ChargeService(httpClient);
            Billing = new BillingService(httpClient);
            Batches = new BatchService(httpClient);
            InstructionalFunding = new InstructionalFundingService(httpClient);
            PayarcConnect = new PayarcConnectService(httpClient, config);
        }

        /// <summary>
        /// A method to ensure the Payarc client is working correctly (e.g., for health checks).
        /// </summary>
        /// <returns>A success message to indicate connectivity.</returns>
        public string TestConnection() {
            return "Payarc client initialized successfully.";
        }
    }
}