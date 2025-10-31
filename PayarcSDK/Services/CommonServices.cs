using Newtonsoft.Json;
using PayarcSDK.Entities;
using PayarcSDK.Entities.Batch;
using PayarcSDK.Entities.Billing.Subscriptions;
using PayarcSDK.Entities.Deposit;
using PayarcSDK.Entities.Dispute;
using PayarcSDK.Entities.InstructionalFunding;
using PayarcSDK.Entities.Payee;
using PayarcSDK.Entities.SplitCampaign;
using PayarcSDK.Entities.UserSetting;
using System.Text.Json;
using System.Text.Json.Nodes;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace PayarcSDK.Services {
    public class CommonServices {

        private readonly HttpClient _httpClient;

        public CommonServices(HttpClient httpClient) {
            _httpClient = httpClient;
        }
        public string BuildQueryString(Dictionary<string, object?> parameters) {
            var queryString = string.Join("&",
                parameters.Select(p =>
                    $"{Uri.EscapeDataString(p.Key)}={Uri.EscapeDataString(p.Value?.ToString() ?? string.Empty)}"));
            return queryString;
        }
        public BaseResponse? TransformJsonRawObject(Dictionary<string, object> obj, string? rawObj, string type = "object") {
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
                    chargeResponse.TipAdjust = async (tipData) => await chargeService.TipAdjust(chargeResponse, tipData);
                    response = chargeResponse;
                } else if (type == "ACHCharge") {
                    ChargeService chargeService = new ChargeService(_httpClient);
                    var achChargeResponse = JsonConvert.DeserializeObject<AchChargeResponseData>(rawObj) ?? new AchChargeResponseData();
                    achChargeResponse.RawData = rawObj;
                    achChargeResponse.ObjectId ??= $"ach_{obj["id"]}";
                    achChargeResponse.CreateRefund = async (chargeData) => await chargeService.CreateRefund(achChargeResponse, chargeData);
                    if (achChargeResponse.BankAccount?.Data != null) achChargeResponse.BankAccount.Data.ObjectId = achChargeResponse.BankAccount.Data.Id;
                    response = achChargeResponse;
                } else if (type == "Customer") {
                    var customerService = new CustomerService(_httpClient);
                    var customerResponse = JsonConvert.DeserializeObject<CustomerResponseData>(rawObj) ?? new CustomerResponseData();
                    customerResponse.RawData = rawObj;
                    customerResponse.ObjectId ??= $"cus_{obj["customer_id"]}";
                    if (customerResponse.Card.Data.Count() != 0) {
                        List<BaseResponse?>? cards = new List<BaseResponse?>();
                        customerResponse.Card.Data.ForEach(doc => {
                            var dataCard = JsonSerializer.Deserialize<Dictionary<string, object>>(doc.ToString());
                            var card = TransformJsonRawObject(dataCard, JsonSerializer.Serialize(doc), "Card");
                            cards.Add(card);
                        });
                        var json = JsonConvert.SerializeObject(customerResponse.Card);
                        var jsonObject = JsonNode.Parse(json);
                        var responseCaseFiles = jsonObject?.ToJsonString() ?? "[]";
                        var responseCaseFilesData = JsonSerializer.Deserialize<Dictionary<string, object>>(responseCaseFiles);
                        if (responseCaseFilesData == null || !responseCaseFilesData.TryGetValue("data", out var caseFilesDataValue) ||
                            !(caseFilesDataValue is JsonElement caseFilesDataElement)) {
                            throw new InvalidOperationException("Response data is invalid or missing.");
                        }
                        var caseFilesRawData = caseFilesDataElement.GetRawText();
                        CustomerCardListResponse listResponseCustomerCards = new CustomerCardListResponse {
                            Data = cards,
                            RawData = caseFilesRawData
                        };
                        if (customerResponse.Cards == null) {
                            customerResponse.Cards = new CardsContainer();
                        }
                        customerResponse.Cards.Cards = listResponseCustomerCards;
                    }
                    customerResponse.Card = null;
                    if (customerResponse.BankAccount.Data.Count() != 0) {
                        List<BaseResponse?>? bankAccoounts = new List<BaseResponse?>();
                        customerResponse.BankAccount.Data.ForEach(doc => {
                            var dataBankAcc = JsonSerializer.Deserialize<Dictionary<string, object>>(doc.ToString());
                            var bankAcc = TransformJsonRawObject(dataBankAcc, JsonSerializer.Serialize(doc), "BankAccount");
                            bankAccoounts.Add(bankAcc);
                        });
                        var jsonBankAccounts = JsonConvert.SerializeObject(customerResponse.BankAccount);
                        var jsonObjectBankAccounts = JsonNode.Parse(jsonBankAccounts);
                        var responseBankAccounts = jsonObjectBankAccounts?.ToJsonString() ?? "[]";
                        var responseBankAccountsData = JsonSerializer.Deserialize<Dictionary<string, object>>(responseBankAccounts);
                        if (responseBankAccountsData == null || !responseBankAccountsData.TryGetValue("data", out var caseFilesDataValue) ||
                            !(caseFilesDataValue is JsonElement bankAccountsDataElement)) {
                            throw new InvalidOperationException("Response data is invalid or missing.");
                        }

                        var BankAccountsRawData = bankAccountsDataElement.GetRawText();
                        CustomerBankListResponse listResponseCustomerBankAccounts = new CustomerBankListResponse {
                            Data = bankAccoounts,
                            RawData = BankAccountsRawData
                        };
                        if (customerResponse.Bank_Accounts == null) {
                            customerResponse.Bank_Accounts = new BankAccountsContainer();
                        }
                        customerResponse.Bank_Accounts.Bank_Accounts = listResponseCustomerBankAccounts;
                    }
                    customerResponse.BankAccount = null;
                    customerResponse.Update = async (customerData) => await customerService.Update(customerResponse, customerData);
                    if (customerResponse.Cards != null) {
                        customerResponse.Cards.Create = async (cardData) => await customerService.AddCardToCustomerAsync(customerResponse, cardData);
                    }
                    if (customerResponse.Bank_Accounts != null) {
                        customerResponse.Bank_Accounts.Create = async (bankData) => await customerService.AddBankAccountToCustomerAsync(customerResponse, bankData);
                    }
                    response = customerResponse;
                } else if (type == "Card") {
                    var tokenResponse = JsonConvert.DeserializeObject<Card>(rawObj) ?? new Card();
                    tokenResponse.RawData = rawObj;
                    tokenResponse.ObjectId ??= $"card_{obj["id"]}";
                    response = tokenResponse;
                } else if (type == "BankAccount") {
                    var bankAccountResponse = JsonConvert.DeserializeObject<BankAccount>(rawObj) ?? new BankAccount();
                    bankAccountResponse.RawData = rawObj;
                    bankAccountResponse.ObjectId ??= $"bnk_{obj["id"]}";
                    response = bankAccountResponse;
                } else if (type == "Token") {
                    var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(rawObj) ?? new TokenResponse();
                    tokenResponse.RawData = rawObj;
                    tokenResponse.ObjectId ??= $"tok_{obj["id"]}";
                    response = tokenResponse;
                } else if (type == "Payee") {
                    var payeeResponse = JsonConvert.DeserializeObject<PayeeResponseData>(rawObj) ?? new PayeeResponseData();
                    payeeResponse.Object = "Payee";
                    payeeResponse.ObjectId ??= $"appy_{obj["id"]}";
                    response = payeeResponse;
                } else if (type == "PayeeList") {
                    var payeeResponse = JsonConvert.DeserializeObject<PayeeListData>(rawObj) ?? new PayeeListData();
                    payeeResponse.Object = "Payee";
                    payeeResponse.ObjectId ??= $"appy_{obj["MerchantCode"]}";
                    response = payeeResponse;
                } else if (type == "ApplyApp") {
                    var applicationService = new ApplicationService(_httpClient);
                    var applicationResponse = JsonConvert.DeserializeObject<ApplicationResponseData>(rawObj) ?? new ApplicationResponseData();
                    applicationResponse.RawData = rawObj;
                    applicationResponse.ObjectId ??= $"appl_{obj["id"]}";
                    applicationResponse.Documents?.ForEach(doc => {
                        doc.ObjectId = $"doc_{doc.Id}";
                        doc.Delete = async () => await applicationService.DeleteDocumentLink(applicationResponse, doc.Id);
                    });
                    applicationResponse.Retrieve = async () => await applicationService.Retrieve(applicationResponse);
                    applicationResponse.Delete = async () => await applicationService.Delete(applicationResponse);
                    applicationResponse.AddDocument = async (merchantDocuments) => await applicationService.AddDocument(applicationResponse, merchantDocuments);
                    applicationResponse.Submit = async () => await applicationService.Submit(applicationResponse);
                    applicationResponse.Update = async (applicationData) => await applicationService.Update(applicationResponse, applicationData);
                    applicationResponse.ListSubAgents = async (optionsData) => await applicationService.ListSubAgents(optionsData);
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
                } else if (type == "AppStatus") {
                    var applicationStatusResponse = JsonConvert.DeserializeObject<ApplicationStatusResponse>(rawObj) ?? new ApplicationStatusResponse();
                    applicationStatusResponse.RawData = rawObj;
                    applicationStatusResponse.ObjectId ??= $"appl_{obj["MerchantCode"]}";
                    applicationStatusResponse.Object = "AppStatus";
                    obj.Remove("MerchantCode");
                    response = applicationStatusResponse;
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
                    var applicationService = new ApplicationService(_httpClient);
                    var documentResponse = JsonConvert.DeserializeObject<DocumentResponseData>(rawObj) ?? new DocumentResponseData();
                    documentResponse.RawData = rawObj;
                    documentResponse.ObjectId ??= $"doc_{obj["id"]}";
                    //documentResponse.Delete = async (applicantId) => await applicationService.DeleteDocument(applicantId, documentResponse);
                    response = documentResponse;
                } else if (type == "User") {
                    var documentResponse = JsonConvert.DeserializeObject<SubAgentResponseData>(rawObj) ?? new SubAgentResponseData();
                    documentResponse.RawData = rawObj;
                    documentResponse.ObjectId ??= $"usr_{obj["id"]}";
                    response = documentResponse;
                } else if (type == "Cases") {
                    var disputeCaseResponse = JsonConvert.DeserializeObject<DisputeCasesResponseData>(rawObj) ?? new DisputeCasesResponseData();
                    disputeCaseResponse.RawData = rawObj;
                    disputeCaseResponse.Object = "Dispute";
                    disputeCaseResponse.ObjectId ??= $"dis_{obj["id"]}";
                    response = disputeCaseResponse;
                } else if (type == "CaseFile") {
                    var disputeCaseFileResponse = JsonConvert.DeserializeObject<DisputeCaseFileData>(rawObj) ?? new DisputeCaseFileData();
                    disputeCaseFileResponse.RawData = rawObj;
                    disputeCaseFileResponse.ObjectId ??= $"cfl_{obj["id"]}";
                    response = disputeCaseFileResponse;
                } else if (type == "Evidence") {
                    var disputeEvidenceResponse = JsonConvert.DeserializeObject<DisputeEvidenceData>(rawObj) ?? new DisputeEvidenceData();
                    disputeEvidenceResponse.RawData = rawObj;
                    disputeEvidenceResponse.ObjectId ??= $"evd_{obj["id"]}";
                    response = disputeEvidenceResponse;
                } else if (type == "CaseSubmission") {
                    var disputeCaseSubmissionResponse = JsonConvert.DeserializeObject<DisputeCaseSubmissionData>(rawObj) ?? new DisputeCaseSubmissionData();
                    disputeCaseSubmissionResponse.RawData = rawObj;
                    disputeCaseSubmissionResponse.ObjectId ??= $"sbm_{obj["id"]}";
                    response = disputeCaseSubmissionResponse;
                } else if (type == "Campaign") {
                    var campaignService = new SplitCampaignService(_httpClient);
                    var campaignResponse = JsonConvert.DeserializeObject<CampaignResponseData>(rawObj) ?? new CampaignResponseData();
                    campaignResponse.RawData = rawObj;
                    campaignResponse.ObjectId ??= $"cmp_{obj["id"]}";
                    campaignResponse.Update = async (newData) => await campaignService.Update(campaignResponse, newData);
                    campaignResponse.Retrieve = async () => await campaignService.Retrieve(campaignResponse);
                    response = campaignResponse;
                } else if (type == "MyAccount") {
                    var myAccountResponse = JsonConvert.DeserializeObject<MyAccountResponseData>(rawObj) ?? new MyAccountResponseData();
                    myAccountResponse.RawData = rawObj;
                    myAccountResponse.Object = "MyAccount";
                    myAccountResponse.ObjectId ??= $"macc_{obj["id"]}";
                    response = myAccountResponse;
                } else if (type == "Batch") {
                    var myAccountResponse = JsonConvert.DeserializeObject<BatchReportResponseData>(rawObj) ?? new BatchReportResponseData();
                    myAccountResponse.RawData = rawObj;
                    myAccountResponse.Object = "Batch";
                    myAccountResponse.ObjectId ??= $"brn_{obj["Batch_Reference_Number"]}";
                    response = myAccountResponse;
                } else if (type == "BatchDetail") {
                    var myAccountResponse = JsonConvert.DeserializeObject<BatchData>(rawObj) ?? new BatchData();
                    myAccountResponse.RawData = rawObj;
                    myAccountResponse.Object = "Batch";
                    myAccountResponse.ObjectId ??= $"brn_{obj["batch_ref_num"]}";
                    response = myAccountResponse;
                } else if (type == "Account") {
                    var depositResponse = JsonConvert.DeserializeObject<DepositAccount>(rawObj) ?? new DepositAccount();
                    depositResponse.RawData = rawObj;
                    depositResponse.Object = "Merchant";
                    depositResponse.ObjectId ??= $"acc_{obj["id"]}";
                    response = depositResponse;
                } else if (type == "ChargeSplit") {
                    var depositResponse = JsonConvert.DeserializeObject<InstructionalFundingResponseData>(rawObj) ?? new InstructionalFundingResponseData();
                    depositResponse.RawData = rawObj;
                    depositResponse.ObjectId ??= $"cspl_{obj["id"]}";
                    response = depositResponse;
                } else if (type == "UserSetting") {
                    var userSettingResponse = JsonConvert.DeserializeObject<UserSettingResponseData>(rawObj) ?? new UserSettingResponseData();
                    userSettingResponse.RawData = rawObj;
                    userSettingResponse.Object = "UserSetting";
                    userSettingResponse.ObjectId ??= $"{obj["id"]}";
                    response = userSettingResponse;
                }
            }
            return response;
        }
    }
}
