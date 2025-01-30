using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using PayarcSDK.Entities;
using JsonDocument = System.Text.Json.JsonDocument;
using JsonSerializer = System.Text.Json.JsonSerializer;
namespace PayarcSDK.Sample
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // var requestBody = new {
            // 	username = "shahsuvar@payarc.com",
            // 	password = "6qCUnwTD.4K_CVz"
            // };
            //
            // var requestContent = new StringContent(
            // JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            // var httpClient = new HttpClient();
            // var response = await httpClient.PostAsync("https://testapi.payarc.net/v1/login", requestContent);
            //
            // if (!response.IsSuccessStatusCode) {
            // 	var errorContent = await response.Content.ReadAsStringAsync();
            // 	throw new InvalidOperationException($"Failed to retrieve access token. StatusCode: {response.StatusCode}, Content: {errorContent}");
            // }
            //
            // // Deserialize the success response
            // var responseContent = await response.Content.ReadAsStringAsync();
            // var successResponse = JsonConvert.DeserializeObject<AddSuccessResponse>(responseContent);
            //
            // // Retrieve the access token
            // var accessToken = successResponse?.response_content?.access_token;
            //
            // if (string.IsNullOrEmpty(accessToken)) {
            // 	throw new InvalidOperationException("Access token is missing in the response.");
            // }

            Payarc payarc = null;
            Payarc payarcAgent = null;

            try
            {
                payarc = new SdkBuilder()
                    .Configure(config =>
                    {
                        config.Environment = "dev"; // Use sandbox environment
                        config.BaseUrl = "http://localapi6.payarc.net";
                        config.ApiVersion = "v1"; // Use version 2 of the API
                        config.BearerToken =
                            "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJhdWQiOiIyNiIsImp0aSI6IjZkNTA1YTA2YjdlNWMwYTk5OGU4MWQ2YjJjYmJmMWFjNzFkMGFlNWU0MmE2MjIxMzk5NGZhYzE4MzI1ZWVlNDhhOGQ0MDRkZTVjNDc5NTYyIiwiaWF0IjoxNjU5MDIwNjUyLCJuYmYiOjE2NTkwMjA2NTIsImV4cCI6MTgxNjY4OTg1Miwic3ViIjoiMTU0MTQiLCJzY29wZXMiOiIqIn0.KDaegyhna7eFAQRQfIuYv-nVFPI2G7iMyCR4fyEFwtirXOxAky7Sie0Oc8HJXiuQ9k7UaNhGF3akFo_8vipTBfQD6L-_wbr8Nj3vf_EuX2Yjz35e5HBBwaJ3b5vGXJGsu_UlpgIntLYPW6DRFSWAdzTtv0t6uyW_98oVvhm-Yxf6stj-UR3mHA18tjP1ISti53Oc2BrIKH_s58eFFdyzj6q1Q63r05uAQ9XC96Gl35ZCnaGEzcWgviTxCrKVfMuRKCzFKB-rJPlRM1lfzGcz-5wvWuqp0jWgsS383I7Pn1uXRWTxasHM93-ioa7TCWlOtyvmrs1_HIG8x9c2QgUoPXGhKzCA8pYiYyeWfguPX250P03B3hks3hZSMu6L9f1xbCBBFH484oMdxYn-CUAOQhysFN7b9-O6PW0Ge2XhsCA3rs3c9vEewTlPggoa8WHr4tTOS1GdCFxneFPzHjsbB3C-ig7r7Qq6594nw-Bb_l2ONsVmsQ19GTU08zwq4hKtdKyPI879pvh9f-IDqJliHAc1qXga5pqX_Cj4pkcTihltdQ3Q0KyKp-Wi3O0Xi6afw8EXcrVLlDnVtyic8sbBpl8Gh8EVOL1zq8D4qwrWH-oZSITmZaBqRiTz-UaKmRyMVg0yVxAfGfqjXtNYkINJEQxwS2X-Id_Mr0sUb72LmyE";
                    })
                    .Build();

                // Use Payarc services
                Console.WriteLine("SDK initialized successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            //Temprorary Agent Access Token taken from portal
            var agentAccessToken =
                "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJhdWQiOiIzIiwianRpIjoiMjg3Y2QxMDc3NmY2NTgxNDJiOWExMjQ1NzRlZGFmYjAxZmMyYWFhZDY1YjM1YTExMDMyMWIyYzE0YmFmZTc0MTY5MmNlNTIyNTc3MGM0NTQiLCJpYXQiOjE3MzI1NTk0MjEsIm5iZiI6MTczMjU1OTQyMSwiZXhwIjoxODkwMjM5NDIxLCJzdWIiOiI4NTI2MzA3Iiwic2NvcGVzIjpbIioiXX0.y6cBWKIHzx7ElgBxMlLFaE0lyE3pgoluQkRIUJ2XWh5A5qiII3SJt7Spu5_tcXX36zpsiCozXjbrsQ3LN1VXrbeITKUxFVewrbmG6CqawqnVgsxHaUpwVT_0rYQz4b1_N-Enf4cr5I1DgcxUWH9U9Z9koTiurrGTV0TrX1kt-8WcrKzD1RTTZokjSLSfPL3zz4za2f1fVmyWaa8-HAEAmXZz2p0p8cs_FjTkuMdwTD-cf4jJ8_RI7CXSoZhvAktrHJOXnebfKq_PFfbOmSqLhQvzGO9KKAcFn8BsDE9JQ4FW70lsxghVIpeqqDOfrCi5wGy7aVicBvssFx5gnyEBE_UHZ78kUcotQQGS7p5-_VhRdE9UYWOO0hbWrrYeGwBHzNAGomyrhwtFtsaWHWG7KNhhSBWeoalsdavh6BA9gWWbP8y-IGxbTG_XWM8bnwxYh-P1x6vOs_dF4X27_p4hP6iSO_uTFLnNR7AthVRINk96fnV_KafjPYT7_yjVwARKm9zzL79RZB5t9SoHO1NEpupaXgwGH0ROF4_zi_YwwTiTXWCIidHzV7BnlZ3_uT0eBFA0gbSySunBYCYGyLZJ-qCUsptH0O4e7WDsBYfltvu-3SAcfIo7Z-uuD1JjZeLhOoEFVff7aAZ4YpvmhazRB2hCgCzLSGt_HmYbdHQgnzY";

            try
            {
                payarcAgent = new SdkBuilder()
                    .Configure(config =>
                    {
                        config.Environment = "dev"; // Use sandbox environment
                        config.BaseUrl = "http://localapi6.payarc.net";
                        config.ApiVersion = "v1"; // Use version 2 of the API
                        config.BearerToken = agentAccessToken; // Set the Bearer Token
                    })
                    .Build();

                // Use Payarc services
                Console.WriteLine("SDK initialized successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            var testService = "billingService";
            // var testService = "chargeService";
            var apiRequester = new ApiRequester(payarc);
            var apiAgentRequester = new ApiRequester(payarcAgent);
            if (payarc != null && payarcAgent != null)
            {
                switch (testService)
                {
                    case "customerService":
                        var cardData = new List<JObject>();
                        var bankData = new List<JObject>();
                        var customerId = "";

                        // Add a card to a customer
                        cardData.Add(new JObject
                        {
                            ["card_source"] = "INTERNET",
                            ["card_number"] = "4012000098765439",
                            ["exp_month"] = "12",
                            ["exp_year"] = "2025",
                            ["cvv"] = "999",
                            ["card_holder_name"] = "John Doe",
                            ["address_line1"] = "411 West Putnam Avenue",
                            ["city"] = "Greenwich",
                            ["state"] = "CT",
                            ["zip"] = "06840",
                            ["country"] = "US"
                        });

                        // Add a card to a customer
                        cardData.Add(new JObject
                        {
                            ["card_source"] = "INTERNET",
                            ["card_number"] = "4111111111111111",
                            ["exp_month"] = "02",
                            ["exp_year"] = "2027",
                            ["cvv"] = "999",
                            ["card_holder_name"] = "John Doe",
                            ["address_line1"] = "411 West Putnam Avenue",
                            ["city"] = "New York",
                            ["state"] = "NY",
                            ["zip"] = "06830",
                            ["country"] = "US"
                        });

                        // Add a bank info to a customer
                        bankData.Add(new JObject
                        {
                            ["account_number"] = 1234567890,
                            ["routing_number"] = 123456789,
                            ["first_name"] = "Test",
                            ["last_name"] = "Account",
                            ["account_type"] = "Personal Checking",
                            ["sec_code"] = "TEL"
                        });

                        // Add a bank info to a customer
                        bankData.Add(new JObject
                        {
                            ["account_number"] = 1234567890,
                            ["routing_number"] = 123456789,
                            ["first_name"] = "Test2",
                            ["last_name"] = "Account2",
                            ["account_type"] = "Personal Checking",
                            ["sec_code"] = "TEL"
                        });

                        // List customers
                        var queryParams = new Dictionary<string, string>
                        {
                            ["limit"] = "10",
                            ["page"] = "1"
                        };

                        var customers = await payarc.CustomerService.list(queryParams);
                        Console.WriteLine($"List of Customers: {customers["Data"]}");

                        customerId = customers["Data"]["data"]
                            .Children<JObject>()
                            .Where(p => (string)p.SelectToken("name") == "Shah Test7")
                            .Select(p => (string)p.SelectToken("customer_id")).FirstOrDefault();

                        var testAction = "deleteCustomer";

                        switch (testAction)
                        {
                            case "createCustomer":
                                // Create a new customer
                                var newCustomerData = new JObject
                                {
                                    ["name"] = "Shah Test8",
                                    ["email"] = "shah@test8.com",
                                    ["phone"] = "1234567890"
                                };
                                newCustomerData.Add("cards", JToken.FromObject(cardData));
                                newCustomerData.Add("bank_accounts", JToken.FromObject(bankData));
                                var createdCustomer = await payarc.CustomerService.create(newCustomerData);
                                Console.WriteLine($"Created Customer: {createdCustomer}");
                                // Retrieve a customer
                                customerId = (string)createdCustomer.SelectToken("customer_id");
                                var customer = await payarc.CustomerService.retrieve(customerId);
                                Console.WriteLine($"Retrieved Customer: {customer}");
                                break;
                            case "updateCustomer":
                                // Update a customer
                                var customerData = new JObject
                                {
                                    ["description"] = "Example customer add card",
                                    ["email"] = "shahupdate2@sdk.com",
                                    ["phone"] = "2222222222"
                                };
                                customerData.Add("cards", JToken.FromObject(cardData));
                                customerData.Add("bank_accounts", JToken.FromObject(bankData));
                                var updatedCustomer = await payarc.CustomerService.update(customerId, customerData);
                                Console.WriteLine($"Updated Customer: {updatedCustomer}");
                                break;
                            case "deleteCustomer":
                                // Delete a customer
                                var deleted = await payarc.CustomerService.delete(customerId);
                                Console.WriteLine($"Deleted Customer: {deleted}");
                                break;
                            default:
                                Console.WriteLine("Nothing to test.");
                                break;
                        }

                        break;
                    case "applicationService":
                        // Add a lead
                        var merccandidate = new JObject
                        {
                            ["Lead"] = new JObject
                            {
                                ["Industry"] = "cbd",
                                ["MerchantName"] = "My applications company",
                                ["LegalName"] = "Best Co in w",
                                ["ContactFirstName"] = "Joan",
                                ["ContactLastName"] = "Dhow",
                                ["ContactEmail"] = "contact+23@mail.com",
                                ["DiscountRateProgram"] = "interchange"
                            },
                            ["Owners"] = new JArray
                            {
                                new JObject
                                {
                                    ["FirstName"] = "First",
                                    ["LastName"] = "Last",
                                    ["Title"] = "President",
                                    ["OwnershipPct"] = 100,
                                    ["Address"] = "Somewhere",
                                    ["City"] = "City Of Test",
                                    ["SSN"] = "4546-0034",
                                    ["State"] = "WY",
                                    ["ZipCode"] = "10102",
                                    ["BirthDate"] = "1993-06-24",
                                    ["Email"] = "nikoj@negointeresuva.com",
                                    ["PhoneNo"] = "2346456784"
                                }
                            }
                        };

                        try
                        {
                            var addedLead = await payarcAgent.ApplicationService.create(merccandidate);
                            Console.WriteLine($"Added Lead: {addedLead}");
                        }
                        catch (HttpRequestException ex)
                        {
                            Console.WriteLine($"Request failed: {ex.Message}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Unexpected error: {ex.Message}");
                        }

                        // List Applications
                        var queryParamsApplications = new Dictionary<string, string>
                        {
                            ["limit"] = "10",
                            ["page"] = "1"
                        };

                        // Retrieve applications
                        var applications = await payarcAgent.ApplicationService.list(queryParamsApplications);
                        Console.WriteLine($"Applications: {applications}");

                        // Submit for signature
                        var applicantId = "9d6woe3qlz73jz0q";
                        var submitted = await payarcAgent.ApplicationService.submit(applicantId);
                        Console.WriteLine($"Submitted Applicant: {submitted}");

                        break;
                    case "disputeService":
                        // List cases
                        var cases = await payarc.DisputeService.list();
                        Console.WriteLine($"List Cases: {cases}");

                        var caseId = "dis_123456";
                        // Get a specific case
                        var specificCase = await payarc.DisputeService.retrieve(caseId);
                        Console.WriteLine($"Case Id with {caseId}: {specificCase}");

                        // Add a document to a case
                        var documentParams = new JObject
                        {
                            {
                                "DocumentDataBase64",
                                "iVBORw0KGgoAAAANSUhEUgAAAIUAAABsCAYAAABEkXF2AAAABHNCSVQICAgIfAhkiAAAAupJREFUeJzt3cFuEkEcx/E/001qUQ+E4NF48GB4BRM9+i59AE16ANlE4wv4Mp5MjI8gZ+ONEMJBAzaWwZsVf2VnstPZpfb7STh06ewu5JuFnSzQ8d5vDfiLa3sHcHiIAoIoIIgCgiggitwbWM/f2vniTe7NoIZ7Dz9Y0X0qy7NHYfbLtn6dfzOoYXPlUl4+IIgCooGXj10ngzM77p81vVmY2Y9vL+xi9Tn4f41HYVZYx3Wb3yws9oWBlw8IooAgCgiigCAKCKKAIAoIooAgCoikGU3nqpvy3qesPvv6+/2+LZfLpHUcsrrPD0cKCKKAIAoIooAgCgiigCAKCOecs7q3iJXbZDLZWVaWZfR4733lLbfZbBbchzZvvV4vy+PmSAFBFBBEAUEUEEQBQRQQRAFR5DzfD81FxMxVpMg9l3HT938fjhQQRAFBFBBEAUEUEEQBQRQQRe5z7SptnYejGkcKCKKAIAoIooAgCgiigCAKiKQoYj6bMB6Pd8aMRqPoz22kfCalzfmXm45nDoIoIIgCgiggiAKCKCCIAiJrFKnfTxHS9vdX5P7+ibZwpIAgCgiigCAKCKKAIAoIooDomNl2352hc+WY3+NYzyf2c345V3EyGNmdwevo8anbr3Lbfu/j+9fndrH69Ofv+48+WtF9JuM4UkAQBQRRQBAFBFFAEAUEUUBUfo9m6jUPzjl7eWr26vRyWVmW9u59GT2+Suo1B4vFImn8/4ojBQRRQBAFBFFAEAUEUUAQBUTHe7/3eorUeYrQ9RSprmP/UtZ/6OP/xfUUqI0oIIgCgiggiqY36Ddz25x/uZZ1PXmcNj60H6H1H/p4sV1F/VvjZx84HJx9IFrl733wexy3U/b3FO7ogR0dD7OsezqdVt4/HFZvNzQ+t9T9C40P6ty9erElfEKsbblnDHNrekYzFu8pIIgCgiggiAKCKCAqzz5Ccr+7T3133fb1DG0//ro4UkAQBQRRQBAFBFFAEAXEb3wL3JblytFeAAAAAElFTkSuQmCC"
                            },
                            { "mimeType", "application/pdf" },
                            { "text", "Additional evidence for the dispute." },
                            { "message", "Submitting dispute case with evidence." }
                        };
                        var result = await payarc.DisputeService.addDocument(caseId, documentParams);
                        Console.WriteLine($"Add Document Result: {result}");
                        break;
                    case "splitCampaignService":
                        // Example: Create a new campaign
                        var newCampaign = new JObject
                        {
                            ["name"] = "Mega bonus Shah2",
                            ["description"] = "Compliment for my favorite customers",
                            ["note"] = "Only for VIPs",
                            ["base_charge"] = 33.33,
                            ["perc_charge"] = 7.77,
                            ["is_default"] = "0",
                            ["accounts"] = new JArray()
                        };

                        var createdCampaign = await payarcAgent.SplitCampaignService.create(newCampaign);
                        Console.WriteLine($"Campaign Created: {createdCampaign}");

                        var createdCampaignId = "";
                        if ((bool)createdCampaign.SelectToken("IsSuccess"))
                        {
                            createdCampaignId = (string)createdCampaign["Data"]["data"].SelectToken("id");
                        }

                        // Example: Get all campaigns
                        var allCampaigns = await payarc.SplitCampaignService.list();
                        Console.WriteLine($"All Campaigns: {allCampaigns}");

                        // Example: Get campaign details
                        var campaignDetails = await payarc.SplitCampaignService.retrieve(createdCampaignId);
                        Console.WriteLine($"Campaign Details: {campaignDetails}");

                        // Example: Update a campaign
                        var updatedData = new JObject
                        {
                            ["budget"] = 6000
                        };

                        var updatedCampaign = await payarc.SplitCampaignService.update(createdCampaignId, updatedData);
                        Console.WriteLine($"Updated Campaign: {updatedCampaign}");

                        // Example: Get all accounts
                        var allAccounts = await payarc.SplitCampaignService.listAccounts();
                        Console.WriteLine($"All Accounts: {allAccounts}");
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
                        await apiRequester.ListSubscriptions();
                        break;
                    default:
                        Console.WriteLine("Nothing to test.");
                        break;
                }
            }
        }

        
      
    }
}