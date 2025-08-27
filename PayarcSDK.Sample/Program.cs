using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PayarcSDK.Entities.PayarcConnectService;
using PayarcSDK.Entities;
using System.Text.Json;
using System.Text;
using PayarcSDK.Entities;
using JsonDocument = System.Text.Json.JsonDocument;
using JsonSerializer = System.Text.Json.JsonSerializer;
namespace PayarcSDK.Sample {
	internal class Program {
		static async Task Main(string[] args) {
			var settings = new JsonSerializerSettings {
				NullValueHandling = NullValueHandling.Ignore,
				Formatting = Formatting.Indented
			};

			Payarc payarc = null;
			Payarc payarcAgent = null;
			Payarc payarcAgentWithSubAgent = null;
			Payarc payarcConnect = null;
			Payarc payarcDisputeCases = null;
			Payarc payarcAccountListExisting = null;

			// Get Tokens from appsettings.secrets.json
			var tokens = SecretManager.LoadTokens("appsettings.secrets.json");
			var serials = SecretManager.LoadSerials("appsettings.secrets.json");

			// Retrieve the device serial number
			var deviceSerialNumber = serials["deviceSerialNo"]; // "";

			// Retrieve the access token
			var accessToken = tokens["MerchantToken"]; // "";

			if (string.IsNullOrEmpty(accessToken)) {
				throw new InvalidOperationException("Access token is missing in the response.");
			}

			//Temprorary Merchant Access Token taken from portal
			try {
				payarc = new SdkBuilder()
					.Configure(config => {
						config.Environment = "sandbox";     // Use sandbox environment
						config.ApiVersion = "v1";           // Use version 2 of the API
						config.BearerToken = accessToken;   // Set the Bearer Token
					})
					.Build();

				// Use Payarc services
				Console.WriteLine("SDK initialized successfully.");
			} catch (Exception ex) {
				Console.WriteLine($"Error: {ex.Message}");
			}

			//Temprorary Agent Access Token taken from portal
			var agentAccessToken = tokens["AgentToken"]; // "";

			if (string.IsNullOrEmpty(agentAccessToken)) {
				throw new InvalidOperationException("Agent access token is missing in the response.");
			}

			try {
				payarcAgent = new SdkBuilder()
					.Configure(config => {
						//config.Environment = "sandbox";         // Use sandbox environment
						config.ApiVersion = "v1";               // Use version 2 of the API
						config.BearerToken = agentAccessToken;  // Set the Bearer Token
					})
					.Build();

				// Use Payarc services
				Console.WriteLine("SDK initialized successfully.");
			} catch (Exception ex) {
				Console.WriteLine($"Error: {ex.Message}");
			}

			//Temprorary Agent with subAgent Access Token taken from portal
			var agentWithSubAgentAccessToken = tokens["AgentWithSubAgentToken"]; // "";

			if (string.IsNullOrEmpty(agentWithSubAgentAccessToken)) {
				throw new InvalidOperationException("Agent access token is missing in the response.");
			}

			try {
				payarcAgentWithSubAgent = new SdkBuilder()
					.Configure(config => {
						config.Environment = "sandbox";         // Use sandbox environment
						config.ApiVersion = "v1";               // Use version 2 of the API
						config.BearerToken = agentWithSubAgentAccessToken;  // Set the Bearer Token
					})
					.Build();

				// Use Payarc services
				Console.WriteLine("SDK initialized successfully.");
			} catch (Exception ex) {
				Console.WriteLine($"Error: {ex.Message}");
			}

			var dicputeCaseAccessToken = tokens["DisputeCaseToken"]; // "";

			if (string.IsNullOrEmpty(dicputeCaseAccessToken)) {
				throw new InvalidOperationException("Agent access token is missing in the response.");
			}

			try {
				payarcDisputeCases = new SdkBuilder()
					.Configure(config => {
						config.Environment = "sandbox";         // Use sandbox environment
						config.ApiVersion = "v1";               // Use version 2 of the API
						config.BearerToken = dicputeCaseAccessToken;  // Set the Bearer Token
					})
					.Build();

				// Use Payarc services
				Console.WriteLine("SDK initialized successfully.");
			} catch (Exception ex) {
				Console.WriteLine($"Error: {ex.Message}");
			}

			// Payarc Connect 
			string payarcConnectAccessToken = tokens["PpayarcConnectToken"]; // "";

			if (string.IsNullOrEmpty(payarcConnectAccessToken)) {
				throw new InvalidOperationException("Payarc Connect access token is missing in the response.");
			}

			try {
				payarcConnect = new SdkBuilder()
					.Configure(config => {
						config.Environment = "payarcConnect";                 // Use sandbox environment
						config.ApiVersion = "v1";                             // Use version 2 of the API
						config.BearerToken = payarcConnectAccessToken;        // Set the Bearer Token
					})
					.Build();

				// Use Payarc services
				Console.WriteLine("SDK initialized successfully.");
			} catch (Exception ex) {
				Console.WriteLine($"Error: {ex.Message}");
			}

			// Retrieve the access token
			var AccountListExistingAccessToken = tokens["AccountListExistingToken"]; // "";

			if (string.IsNullOrEmpty(accessToken)) {
				throw new InvalidOperationException("Access token is missing in the response.");
			}

			//Temprorary Merchant Access Token taken from portal
			try {
				payarcAccountListExisting = new SdkBuilder()
					.Configure(config => {
						config.Environment = "sandbox";     // Use sandbox environment
						config.ApiVersion = "v1";           // Use version 2 of the API
						config.BearerToken = AccountListExistingAccessToken;   // Set the Bearer Token
					})
					.Build();

				// Use Payarc services
				Console.WriteLine("SDK initialized successfully.");
			} catch (Exception ex) {
				Console.WriteLine($"Error: {ex.Message}");
			}

			//var testService = "billingService";
			//var testService = "customerService";
			//var testService = "applicationService";
			//var testService = "disputeService";
			//var testService = "splitCampaignService";
			var testService = "chargeService";
			//var testService = "payarcConnect";
			var apiRequester = new ApiRequester(payarc);
			var apiAgentRequester = new ApiRequester(payarcAgent);
			if (payarc != null && payarcAgent != null) {
				switch (testService) {
					case "customerService":
						var cardData = new List<CardData>();
						var bankData = new List<BankData>();
						var customerId = "";

						cardData.Add(new CardData {
							CardSource = "INTERNET",
							CardNumber = "4012000098765439",
							ExpMonth = "12",
							ExpYear = "2025",
							Cvv = "999",
							CardHolderName = "John Doe",
							AddressLine1 = "411 West Putnam Avenue",
							City = "Greenwich",
							State = "CT",
							Zip = "06840",
							Country = "US"
						});

						cardData.Add(new CardData {
							CardSource = "INTERNET",
							CardNumber = "4111111111111111",
							ExpMonth = "02",
							ExpYear = "2027",
							Cvv = "999",
							CardHolderName = "John Doe",
							AddressLine1 = "411 West Putnam Avenue",
							City = "New York",
							State = "NY",
							Zip = "06830",
							Country = "US"
						});

						bankData.Add(new BankData {
							AccountNumber = "123432876",
							RoutingNumber = "021000021",
							FirstName = "Test",
							LastName = "Account",
							AccountType = "Personal Checking",
							SecCode = "TEL"
						});

						bankData.Add(new BankData {
							AccountNumber = "1234567890",
							RoutingNumber = "021000021",
							FirstName = "Test2",
							LastName = "Account2",
							AccountType = "Personal Checking",
							SecCode = "TEL"
						});

						// List customers

						var listOptions = new BaseListOptions() {
							Limit = 3,
							//Page = 1,
						};

						var customers = await payarc.Customers.List(listOptions);
						//Console.WriteLine($"List of Customers: {customers}");
						//Console.WriteLine("Customers Data");
						//for (int i = 0; i < customers?.Data?.Count; i++) {
						//	var t = customers.Data[i];
						//	Console.WriteLine(t);
						//	if (i == customers?.Data?.Count - 1) {
						//		customerId = t.ObjectId;
						//	}
						//}
						//Console.WriteLine("Pagination Data");
						//Console.WriteLine(customers?.Pagination["total"]);
						//Console.WriteLine(customers?.Pagination["count"]);
						//Console.WriteLine(customers?.Pagination["per_page"]);
						//Console.WriteLine(customers?.Pagination["current_page"]);
						//Console.WriteLine(customers?.Pagination["total_pages"]);
						customerId = customers.Data[0].ObjectId;
						Console.WriteLine(JsonConvert.SerializeObject(customers, settings));
						var testCustomerAction = "createCustomer";
						//var testCustomerAction = "updateCustomer";
						//var testCustomerAction = "deleteCustomer";

						switch (testCustomerAction) {
							case "createCustomer":
								// Create a new customer
								//CustomerRequestData newCustomerData = new CustomerRequestData {
								//	Name = "Shah Test12",
								//	Email = "shah@test12.com",
								//	Phone = 1234567890
								//};

								//newCustomerData.Cards = cardData;
								//newCustomerData.BankAccounts = bankData;
								//BaseResponse createdCustomer = await payarc.Customers.Create(newCustomerData);
								//Console.WriteLine($"Created Customer: {createdCustomer}");
								//// Retrieve a customer
								//customerId = createdCustomer.ObjectId;
								customerId = "cus_jDPAnVxAPMKKVpKM";
								CustomerResponseData customer = await payarc.Customers.Retrieve(customerId) as CustomerResponseData;
								Console.WriteLine($"Retrieved Customer: {customer}");


								//CustomerRequestData updateCustomerData = new CustomerRequestData {
								//	Name = "Shah Test4444",
								//	Email = "shahupdate4444@sdk.com",
								//	Phone = 4444444444
								//};
								//var updatedCustomerRes = await customer.Update(updateCustomerData);
								//Console.WriteLine($"Updated Customer: {updatedCustomerRes}");


								//var updatedCustomerRes = await customer.Cards.Create(new CardData {
								//	CardSource = "INTERNET",
								//	CardNumber = "4111111111111111",
								//	ExpMonth = "02",
								//	ExpYear = "2027",
								//	Cvv = "999",
								//	CardHolderName = "John Doe",
								//	AddressLine1 = "411 West Putnam Avenue",
								//	City = "New York",
								//	State = "NY",
								//	Zip = "06830",
								//	Country = "US"
								//});
								//Console.WriteLine($"Updated Customer: {JsonConvert.SerializeObject(updatedCustomerRes, settings)}");



								var updatedCustomerRes = await customer.Bank_Accounts.Create(new BankData {
									AccountNumber = "1234567890",
									RoutingNumber = "021000021",
									FirstName = "Test2",
									LastName = "Account2",
									AccountType = "Personal Checking",
									SecCode = "TEL"
								});
								Console.WriteLine($"Updated Customer: {JsonConvert.SerializeObject(updatedCustomerRes, settings)}");
								break;
							case "updateCustomer":
								// Update a customer
								CustomerRequestData customerData = new CustomerRequestData {
									Name = "Shah Test4",
									Email = "shahupdate22@sdk.com",
									Phone = 2222222222
								};
								customerData.Cards = cardData;
								customerData.BankAccounts = bankData;
								var updatedCustomer = await payarc.Customers.Update(customerId, customerData);
								Console.WriteLine($"Updated Customer: {updatedCustomer}");
								break;
							case "deleteCustomer":
								// Delete a customer
								var deleted = await payarc.Customers.Delete(customerId);
								Console.WriteLine($"Deleted Customer: {deleted}");
								break;
							default:
								Console.WriteLine("Nothing to test.");
								break;
						}
						break;
					case "applicationService":
						var applicationId = "";

						//OptionsData queryParamsApplications = new OptionsData {
						//	Limit = 10,
						//	Page = 1
						//};

						var applications = await payarcAgent.Applications.List();
						Console.WriteLine("Customers Data");
						Console.WriteLine(JsonConvert.SerializeObject(applications, settings));

						applicationId = applications.Data[0].ObjectId;
						//var application = applications.Data[0] as ApplicationResponseData;
						//ApplicationResponseData applicant = await application.Retrieve() as ApplicationResponseData;
						//Console.WriteLine($"Documents: {JsonConvert.SerializeObject(applicant, settings)}");

						//applicationId = "appl_wzbrmqg53k7pk469";
						//var testApplicationAction = "createApplication";
						//var testApplicationAction = "retrieveApplication";
						//var testApplicationAction = "updateApplication";
						//var testApplicationAction = "deleteApplication";
						//var testApplicationAction = "addDocument";
						var testApplicationAction = "submitDocument";
						//var testApplicationAction = "deleteDocument";
						//var testApplicationAction = "listSubAgent";

						switch (testApplicationAction) {
							case "createApplication":
								ApplicationInfoData merccandidate = new ApplicationInfoData {
									Lead = new Lead {
										Industry = "cbd",
										MerchantName = "My applications company 2",
										LegalName = "Best Co in w",
										ContactFirstName = "Joan",
										ContactLastName = "Dhow",
										ContactEmail = "contact+23@mail.com",
										DiscountRateProgram = "interchange"
									},
									Owners = new List<Owner> {
										new Owner {
											FirstName = "First2",
											LastName = "Last2",
											Title = "President",
											OwnershipPct = 100,
											Address = "Somewhere",
											City = "City Of Test",
											SSN = "4546-0034",
											State = "WY",
											ZipCode = "10102",
											BirthDate = "1993-06-24",
											Email = "nikoj@negointeresuva.com",
											PhoneNo = "2346456784"
										}
									}
								};
								var listSubAgents = await payarcAgentWithSubAgent.Applications.ListSubAgents();
								merccandidate.AgentId = listSubAgents.Data[0].ObjectId;
								BaseResponse createdApplication = await payarcAgent.Applications.Create(merccandidate);
								Console.WriteLine($"Created Application: {JsonConvert.SerializeObject(createdApplication, settings)}");
								break;
							case "retrieveApplication":
								//applicationId = "appl_dpjlrewlnrgvz583";
								BaseResponse retrievedApplication = await payarcAgent.Applications.Retrieve(applicationId);
								Console.WriteLine($"Retrieved Application: {retrievedApplication}");


								break;
							case "updateApplication":
								// Update a Application
								ApplicationInfoData newApplicationData = new ApplicationInfoData {
									Lead = new Lead {
										MerchantName = "Updated"										
									}
								};
								var updatedApplication = await payarcAgent.Applications.Update(applicationId, newApplicationData);
								Console.WriteLine($"Updated Application: {updatedApplication}");
								break;
							case "deleteApplication":
								// Delete a Application
								var deleted = await payarcAgent.Applications.Delete(applicationId);
								Console.WriteLine($"Deleted Application: {deleted}");
								break;
							case "addDocument":
								// Add a document to an application
								//var merchantDocumentsData = new List<MerchantDocument> {
								//	new MerchantDocument
								//	{
								//		DocumentType = "SSN",
								//		DocumentName = "Sample Document 2",
								//		//DocumentIndex = 12243,
								//		DocumentDataBase64 = "data:image/jpeg;base64,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"
								//	}
								//};
								//var documentAdded = await payarcAgent.Applications.AddDocument(applicationId, merchantDocumentsData);
								//var documentAdded = await applicant.AddDocument(new MerchantDocument {
								//	DocumentType = "EIN",
								//	DocumentName = "Sample Document 2",
								//	//DocumentIndex = 12243,
								//	DocumentDataBase64 = "data:image/jpeg;base64,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"
								//});
								//Console.WriteLine($"Document Added Application: {documentAdded}");
								break;
							case "submitDocument":
								// Submit for signature
								var submitted = await payarcAgent.Applications.Submit(applicationId);
								Console.WriteLine($"Submitted Applicant: {submitted}");
								break;
							case "deleteDocument":
								// Delete a document
								//BaseResponse testApplication = await payarcAgent.Applications.Retrieve(applicationId);
								//Console.WriteLine($"Retrieved Application: {testApplication}");
								//ApplicationResponseData applicationData = testApplication as ApplicationResponseData;
								//var applicationData = await applicant.Retrieve() as ApplicationResponseData;
								//var documentId = applicationData.Documents?.FirstOrDefault().ObjectId;
								//var deletedDocument = await applicationData.Documents.FirstOrDefault().Delete();
								var documentId = "doc_63alndgynn7p49y8";
								var deletedDocument = await payarcAgent.Applications.DeleteDocument(documentId);
								Console.WriteLine($"Deleted Document: {deletedDocument}");
								break;
							case "listSubAgent":
								BaseListOptions queryParamsSubAgents = new BaseListOptions {
									Limit = 10,
									Page = 1
								};
								var subAgents = await payarcAgentWithSubAgent.Applications.ListSubAgents(queryParamsSubAgents);
								Console.WriteLine("Sub Agent Data");
								for (int i = 0; i < subAgents?.Data?.Count; i++) {
									var t = subAgents.Data[i];
									Console.WriteLine(t);
								}
								break;
							default:
								Console.WriteLine("Nothing to test.");
								break;
						}
						break;
					case "disputeService":
						// List cases
						var caseId = "";

						var currentDate = DateTime.UtcNow;
						var tomorrowDate = currentDate.AddDays(1).ToString("yyyy-MM-dd");
						var lastMonthDate = currentDate.AddMonths(-60).ToString("yyyy-MM-dd");

						BaseListOptions queryParamsDisputeCases = new BaseListOptions {
							Report_DateGTE = lastMonthDate,
							Report_DateLTE = tomorrowDate,
						};

						var cases = await payarcDisputeCases.Disputes.List(queryParamsDisputeCases);
						Console.WriteLine($"List Cases:");
						//for (int i = 0; i < cases?.Data?.Count; i++) {
						//	var t = cases.Data[i];
						//	Console.WriteLine(t);
						//	if (i == cases?.Data?.Count - 1) {
						//		caseId = t.ObjectId;
						//	}
						//}
						caseId = cases.Data[0].ObjectId;
						Console.WriteLine($"List Cases:: {JsonConvert.SerializeObject(cases, settings)}");


						// Get a specific case
						var specificCase = await payarcDisputeCases.Disputes.Retrieve(caseId);
						Console.WriteLine($"Case Id with {caseId}: {specificCase}");

						// Add a document to a case
						var documentParams = new DocumentParameters {
							DocumentDataBase64 = "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",
							MimeType = "application/pdf",
							Text = "Additional evidence for the dispute.",
							Message = "Submitting dispute case with evidence."
						};
						var result = await payarcDisputeCases.Disputes.AddDocument(caseId, documentParams);
						var jsonResult = JObject.Parse(result);
						Console.WriteLine($"Add Document Result: {JsonConvert.SerializeObject(jsonResult, settings)}");
						var evidenceAddedCase = await payarcDisputeCases.Disputes.Retrieve(caseId);
						Console.WriteLine($"Evidence Added Case: {evidenceAddedCase}");
						break;
					case "splitCampaignService":
						var campaignId = "";
						// List campaign

						//var listCampaignOptions = new BaseListOptions() {
						//	Limit = 10,
						//	Page = 1,
						//};

						//var campaigns = await payarcAgent.SplitCampaigns.List();
						//Console.WriteLine($"List of Customers: {customers}");
						//Console.WriteLine("Campaigns List");
						//for (int i = 0; i < campaigns?.Data?.Count; i++) {
						//	var t = campaigns.Data[i];
						//	Console.WriteLine(t);
						//	if (i == campaigns?.Data?.Count - 1) {
						//		campaignId = t.ObjectId;
						//	}
						//}
						//campaignId = campaigns.Data[0].ObjectId;
						//Console.WriteLine($"List Cases:: {JsonConvert.SerializeObject(campaigns, settings)}");

						//var testCampaignAction = "createCampaign";
						var testCampaignAction = "updateCampaign";
						//var testCampaignAction = "getListAccounts";
						campaignId = "cmp_6dl8k079079ymxw4";
						switch (testCampaignAction) {
							case "createCampaign":
								// Example: Create a new campaign
								//SplitCampaignRequestData newCampaign = new SplitCampaignRequestData {
								//	Name = "Mega bonus Shah2",
								//	Description = "Compliment for my favorite customers",
								//	Notes = "Only for VIPs",
								//	BaseCharge = 33.33,
								//	PercCharge = "7.77",
								//	IsDefault = "0",
								//	Accounts = new string[] { }
								//};

								//var createdCampaign = await payarcAgent.SplitCampaigns.Create(newCampaign);
								//Console.WriteLine($"Campaign Created: {createdCampaign}");


								// Retrieve a campaign
								//campaignId = createdCampaign.ObjectId;
								BaseResponse campaign = await payarcAgent.SplitCampaigns.Retrieve(campaignId);
								Console.WriteLine($"Retrieved Campaign: {campaign}");
								break;
							case "updateCampaign":
								// Update a campaign
								//SplitCampaignRequestData updatedData = new SplitCampaignRequestData {
								//	Notes = "New version of notes"
								//};
								//var updatedCampaign = await payarcAgent.SplitCampaigns.Update(campaignId, updatedData);


								CampaignResponseData campaignResponse = await payarcAgent.SplitCampaigns.Retrieve(campaignId) as CampaignResponseData;
								var updatedCampaign = await campaignResponse.Update(new SplitCampaignRequestData {
									Notes = "Internal modifications"
								});
								Console.WriteLine($"Updated Campaign: {updatedCampaign}");
								break;
							case "getListAccounts":
								//Example: Get all accounts
								var allAccounts = await payarcAccountListExisting.SplitCampaigns.ListAccounts();
								//Console.WriteLine($"All Accounts:");
								//for (int i = 0; i < allAccounts?.Data?.Count; i++) {
								//	var t = allAccounts.Data[i];
								//	Console.WriteLine(t);
								//}
								Console.WriteLine($"All Accounts: {JsonConvert.SerializeObject(allAccounts, settings)}");
								break;
							default:
								Console.WriteLine("Nothing to test.");
								break;
						}
						break;
					case "chargeService":
						// await apiRequester.CreateChargeExample();
						// await apiRequester.CreateChargeExample();
						// await apiRequester.GetChargeById();
						// await apiRequester.CreateChargeByCardIdExample();
						// await apiRequester.CreateChargeByCustomerIdExample();
						// await apiRequester.CreateChargeByToken();
						// await apiRequester.CreateACHChargeByBankAccount();
						// await apiRequester.CreateACHChargeByBankAccountDetails();
						// await apiRequester.ListCharges();
						// await apiRequester.RefundChargeById();
						// await apiRequester.RefundChargeByObject();
						// await apiRequester.RefundACHChargeByObject();
						// await apiAgentRequester.ListByAgentPayfac();

						var currentDateForCharges = DateTime.UtcNow;
						var tomorrowDateForCharges = currentDateForCharges.AddDays(1).ToString("yyyy-MM-dd");
						//var lastMonthDateForCharges = currentDateForCharges.AddMonths(-1).ToString("yyyy-MM-dd");
						var lastMonthDateForCharges = currentDateForCharges.AddDays(-1).ToString("yyyy-MM-dd");

						BaseListOptions queryParamsAgentTraditionalCharges = new BaseListOptions {
							From_Date = lastMonthDateForCharges,
							To_Date = tomorrowDateForCharges,
						};
						await apiAgentRequester.ListByAgentTraditional(queryParamsAgentTraditionalCharges);
						await apiAgentRequester.ListByAgentTraditional();
						break;
					case "billingService":
						// await apiRequester.CreatePlan();
						// await apiRequester.getPlanById();
						// await apiRequester.ListPlans();
						// await apiRequester.UpdatePlanByIdFromList();
						// await apiRequester.UpdatePlanById();
						// await apiRequester.DeletePlanById();
						// await apiRequester.CreateSubscription();
						// await apiRequester.CreateSubscriptionById();
						// await apiRequester.ListSubscriptions();
						// await apiRequester.UpdateSubscription();
						// await apiRequester.UpdateSubscriptionByObject();
						// await apiRequester.CancelSubscription();
						break;
					case "payarcConnect":
						string deviceSerialNo = deviceSerialNumber; // Serial number of the device you are testing with

						/// Test with the following methods, replace input parameters as needed. More information can be found in the Payarc Connect API documentation.

						//LoginResponse loginResponse = await payarcConnect.PayarcConnect.Login();
						//SaleResponse saleResponse = await payarcConnect.PayarcConnect.Sale("CREDIT", "REF61", "61", deviceSerialNo);
						//SaleResponse voidResponse = await payarcConnect.PayarcConnect.Void("nbDBOMBWRoyRoORX", deviceSerialNo);
						//SaleResponse refundResponse = await payarcConnect.PayarcConnect.Refund("32", "DoBnOnDMWLXWbOyW", deviceSerialNo);
						//SaleResponse blindCreditResponse = await payarcConnect.PayarcConnect.BlindCredit("BLINDCREDIT69", "69", "2m1v0L595vN9L0MP", "0227", deviceSerialNo);
						//SaleResponse authResponse = await payarcConnect.PayarcConnect.Auth("AUTH64", "64", deviceSerialNo);
						//SaleResponse postAuthReponse = await payarcConnect.PayarcConnect.PostAuth("POSTAUTH64", "12", "64", deviceSerialNo);
						//SaleResponse lastTransaction = await payarcConnect.PayarcConnect.LastTransaction(deviceSerialNo);
						//ServerInfoResponse serverInfo = await payarcConnect.PayarcConnect.ServerInfo();
						//TerminalResponse terminalResponse = await payarcConnect.PayarcConnect.Terminals();

						break;
					default:
						Console.WriteLine("Nothing to test.");
						break;
				}
				Console.WriteLine("Done.");
			}
		}
	}
}