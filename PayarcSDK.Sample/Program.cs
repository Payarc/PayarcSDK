using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PayarcSDK.Entities.PayarcConnectService;
using PayarcSDK.Entities;
using PayarcSDK.Entities.CustomerService;
using PayarcSDK.Models;
using System.Text;
using System.Text.Json;
using JsonException = System.Text.Json.JsonException;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace PayarcSDK.Sample {

    internal class Program {
        static async Task Main(string[] args) {
            // Retrieve the access token
            var accessToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJhdWQiOiIzIiwianRpIjoiODdkNmVkMTYxYmEzOTliZTA1ZWY1YmYxNDE0MTA2NDFmMjFhNDE2Y2JlNDg1NmM2NzA2YWY3NzdjZTMwMGM2NTBiNjY2MDM3Nzc5YTI3MmUiLCJpYXQiOjE3MzgwODI1NzIuMzgzODg4LCJuYmYiOjE3MzgwODI1NzIuMzgzODksImV4cCI6MTc2OTYxODU3Mi4zNjE1MjEsInN1YiI6Ijg1MjYyMzYiLCJzY29wZXMiOltdfQ.qfo1ndtMFdDeEY2xQ9c3zlxUnjkJW0pZ9BvtaRxhhK9XkIHA3DGkKe6eGSPz6FmPW3Upwmggw9m6CFZnSoc2IrzUkJT0-68aP0xAiAT4akkM7P0fdmWFBEMNVqTHu8BYF37xo4_yRzUd5d--RdjAJKrJPBAfQaDw9CnbW5MgsVx-rkNVvIh365MQQ0TfOCmGy6-yoc79l4YwDLVLpIY1xeENAI5JQRvhprfOilSmsag4IxVNtgUg64GaOUx34pLpFUk2RmtikYuTyWcJvniYpUUOPrduIIKImxZEzH_rHD5gcRRetp1GJ8T3zs_wD5BnUKRJZiWVnBNazwoFevfhZj21XJDHzT--asVwH5RAS5oAW-uP0dUZz0BEQEeo1uQ3dTUqcTKUJdo2LqE8S-5KJA2F3OG1m2HqRpURwau0S29U8lPZSPX5d2CgXNFjGjrpBmf30v6vaDu-Q6qJ9sHX0aQ9h7kONnZtR8_TMCwJzT3JbwaLzGrcwJ5eu1YjSr-GRash3SgqAFedENUbq085ybiljl2GQlddgFjjwP7PAtTPiXqpajseIt-ZS7zfAaWL3Nr_7PAlS-QXvwxaJIXiZNn8agB6ZqyDGi9jfjMbzOWA3DZfYkm2UeXm-49FiEVO14wLzLcK7uRQq6zVdq_Hy3k0NxXfmNvdOWrUR85jYqM";

            if (string.IsNullOrEmpty(accessToken)) {
                throw new InvalidOperationException("Access token is missing in the response.");
            }

            Payarc payarc = null;
            Payarc payarcAgent = null;
            Payarc payarcConnect = null;

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
            var agentAccessToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJhdWQiOiIzIiwianRpIjoiMjg3Y2QxMDc3NmY2NTgxNDJiOWExMjQ1NzRlZGFmYjAxZmMyYWFhZDY1YjM1YTExMDMyMWIyYzE0YmFmZTc0MTY5MmNlNTIyNTc3MGM0NTQiLCJpYXQiOjE3MzI1NTk0MjEsIm5iZiI6MTczMjU1OTQyMSwiZXhwIjoxODkwMjM5NDIxLCJzdWIiOiI4NTI2MzA3Iiwic2NvcGVzIjpbIioiXX0.y6cBWKIHzx7ElgBxMlLFaE0lyE3pgoluQkRIUJ2XWh5A5qiII3SJt7Spu5_tcXX36zpsiCozXjbrsQ3LN1VXrbeITKUxFVewrbmG6CqawqnVgsxHaUpwVT_0rYQz4b1_N-Enf4cr5I1DgcxUWH9U9Z9koTiurrGTV0TrX1kt-8WcrKzD1RTTZokjSLSfPL3zz4za2f1fVmyWaa8-HAEAmXZz2p0p8cs_FjTkuMdwTD-cf4jJ8_RI7CXSoZhvAktrHJOXnebfKq_PFfbOmSqLhQvzGO9KKAcFn8BsDE9JQ4FW70lsxghVIpeqqDOfrCi5wGy7aVicBvssFx5gnyEBE_UHZ78kUcotQQGS7p5-_VhRdE9UYWOO0hbWrrYeGwBHzNAGomyrhwtFtsaWHWG7KNhhSBWeoalsdavh6BA9gWWbP8y-IGxbTG_XWM8bnwxYh-P1x6vOs_dF4X27_p4hP6iSO_uTFLnNR7AthVRINk96fnV_KafjPYT7_yjVwARKm9zzL79RZB5t9SoHO1NEpupaXgwGH0ROF4_zi_YwwTiTXWCIidHzV7BnlZ3_uT0eBFA0gbSySunBYCYGyLZJ-qCUsptH0O4e7WDsBYfltvu-3SAcfIo7Z-uuD1JjZeLhOoEFVff7aAZ4YpvmhazRB2hCgCzLSGt_HmYbdHQgnzY";

            try {
                payarcAgent = new SdkBuilder()
                .Configure(config => {
                    config.Environment = "sandbox";         // Use sandbox environment
                    config.ApiVersion = "v1";               // Use version 2 of the API
                    config.BearerToken = agentAccessToken;  // Set the Bearer Token
                })
                .Build();

                // Use Payarc services
                Console.WriteLine("SDK initialized successfully.");
            } catch (Exception ex) {
                Console.WriteLine($"Error: {ex.Message}");
            }

            // Payarc Connect 
            string payarcConnectAccessToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJhdWQiOiI0Mzg4IiwianRpIjoiOTI0ODMyZjJiM2Q1MDZiZjU1M2Q0NWQzMWJkNTg0MWQ0ZWRjMjdmMjI4ODg4NWU4NWQzMDdmNjk3MWJmYjMxMTJhZjYyYzhmN2MyZTlhZTciLCJpYXQiOjE2MTExNzUxNjgsIm5iZiI6MTYxMTE3NTE2OCwiZXhwIjoxNzY4ODU1MTY4LCJzdWIiOiIxNTY1MyIsInNjb3BlcyI6IioifQ.bYo6ZQ4Jg3wjT_KibvLGpmTpWgapBfyJOXxH-1boMbVyzmj9oO_o8NpLu4aR8vGt4ZcCwmqWkuAJkYdDij0DeDuqI_7IJcBK7hRHBR4tjRbo2plmc44xnxFp5G-NbXC3lj620L2lfgBheyMRAhpkaLfwaVBQvOsq829kNmSlPhom_OhTmyBEDZi5oTFg44vKi4LfI9gORlV0wBFELrcjWoodTsMJHDk_Tiuxwkdf81XvaM6uIiJUTgnnPZM4LDINHbi9YQZ7HYORSIFn2gOyfdGSwTiY5gi13vC-ISDZxBxQWN61JMEwIheaFTubmNgUTvn7gSsp8rnSLo1Hm7p_Mh5lg6Jf2Z89509KRgO5X3iQMWMWmvAX3leSYUi0ngAXQBGdEHlyUNNy0S3dh-fJzkyFpQxkftUDX3ZKbJxCd4H4Vfe5WpgmEdjhD2wb6RI1GnPBkG6SwGy6kcHGjNKxK4hFBKZPCSwWJD7VgJP-eXQMU2J-i9tcc-zp4Acb4qjWe02FYBMKxY6FmDpFpLSvRZGXdH5Xegw6kfDIZWJF-mOB5g0ISFC_tjfxza544iEIOXlYkKzkCNXO0XbJUH6XFFv0Obd74VBrfPaHR-zxbgDmqHFRH_6bWIGAbwiwK3S8GG5RwDpk5uvEaC2F6V0M_o7ePEint8u6BCCK8WYPm7g";

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

            var testService = "customerService";
            //var testService = "applicationService";
            //var testService = "disputeService";
            //var testService = "splitCampaignService";
            //var testService = "chargeService";
            //var testService = "payarcConnect";

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
                        AccountNumber = "1234567890",
                        RoutingNumber = "123456789",
                        FirstName = "Test",
                        LastName = "Account",
                        AccountType = "Personal Checking",
                        SecCode = "TEL"
                    });

                    bankData.Add(new BankData {
                        AccountNumber = "1234567890",
                        RoutingNumber = "123456789",
                        FirstName = "Test2",
                        LastName = "Account2",
                        AccountType = "Personal Checking",
                        SecCode = "TEL"
                    });

                    // List customers

                    var listOptions = new OptionsData() {
                        Limit = 10,
                        Page = 1,
                    };

                    var customers = await payarc.CustomerService.List(listOptions);
                    //Console.WriteLine($"List of Customers: {customers}");
                    Console.WriteLine("Customers Data");
                    for (int i = 0; i < customers?.Data?.Count; i++) {
                        var t = customers.Data[i];
                        Console.WriteLine(customers.Data[i]);
                    }
                    Console.WriteLine("Pagination Data");
                    Console.WriteLine(customers?.Pagination["total"]);
                    Console.WriteLine(customers?.Pagination["count"]);
                    Console.WriteLine(customers?.Pagination["per_page"]);
                    Console.WriteLine(customers?.Pagination["current_page"]);
                    Console.WriteLine(customers?.Pagination["total_pages"]);


                    //customerId = customers["data"]
                    //	.Children<JObject>()
                    //	.Where(p => (string)p.SelectToken("name") == "Shah Test7")
                    //	.Select(p => (string)p.SelectToken("customer_id")).FirstOrDefault();

                    var testAction = "createCustomer";
                    //var testAction = "updateCustomer";
                    //var testAction = "deleteCustomer";

                    switch (testAction) {
                        case "createCustomer":
                            // Create a new customer
                            CustomerInfoData newCustomerData = new CustomerInfoData {
                                Name = "Shah Test11",
                                Email = "shah@test11.com",
                                Phone = 1234567890
                            };

                            newCustomerData.Cards = cardData;
                            newCustomerData.BankAccounts = bankData;
                            BaseResponse createdCustomer = await payarc.CustomerService.Create(newCustomerData);
                            Console.WriteLine($"Created Customer: {createdCustomer}");
                            // Retrieve a customer
                            customerId = createdCustomer.ObjectId;
                            customerId = customerId.StartsWith("cus_") ? customerId.Substring(4) : customerId;
                            BaseResponse customer = await payarc.CustomerService.Retrieve(customerId);
                            Console.WriteLine($"Retrieved Customer: {customer}");
                            break;
                        case "updateCustomer":
                            // Update a customer
                            CustomerInfoData customerData = new CustomerInfoData {
                                Name = "Shah Test9",
                                Email = "shahupdate2@sdk.com",
                                Phone = 2222222222
                            };
                            customerData.Cards = cardData;
                            customerData.BankAccounts = bankData;
                            var updatedCustomer = await payarc.CustomerService.Update(customerId, customerData);
                            Console.WriteLine($"Updated Customer: {updatedCustomer}");
                            break;
                        case "deleteCustomer":
                            // Delete a customer
                            var deleted = await payarc.CustomerService.Delete(customerId);
                            Console.WriteLine($"Deleted Customer: {deleted}");
                            break;
                        default:
                            Console.WriteLine("Nothing to test.");
                            break;
                    }
                    break;
                case "applicationService":
                    // Add a lead
                    var merccandidate = new JObject {
                        ["Lead"] = new JObject {
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

                    try {
                        var addedLead = await payarcAgent.ApplicationService.Create(merccandidate);
                        Console.WriteLine($"Added Lead: {addedLead}");
                    } catch (HttpRequestException ex) {
                        Console.WriteLine($"Request failed: {ex.Message}");
                    } catch (Exception ex) {
                        Console.WriteLine($"Unexpected error: {ex.Message}");
                    }

                    // List Applications
                    var queryParamsApplications = new Dictionary<string, string> {
                        ["limit"] = "10",
                        ["page"] = "1"
                    };

                    // Retrieve applications
                    var applications = await payarcAgent.ApplicationService.List(queryParamsApplications);
                    Console.WriteLine($"Applications: {applications}");

                    // Submit for signature
                    var applicantId = "9d6woe3qlz73jz0q";
                    var submitted = await payarcAgent.ApplicationService.Submit(applicantId);
                    Console.WriteLine($"Submitted Applicant: {submitted}");

                    break;
                case "disputeService":
                    // List cases
                    var cases = await payarc.DisputeService.List();
                    Console.WriteLine($"List Cases: {cases}");

                    var caseId = "dis_123456";
                    // Get a specific case
                    var specificCase = await payarc.DisputeService.Retrieve(caseId);
                    Console.WriteLine($"Case Id with {caseId}: {specificCase}");

                    // Add a document to a case
                    var documentParams = new JObject
                    {
                        { "DocumentDataBase64", "iVBORw0KGgoAAAANSUhEUgAAAIUAAABsCAYAAABEkXF2AAAABHNCSVQICAgIfAhkiAAAAupJREFUeJzt3cFuEkEcx/E/001qUQ+E4NF48GB4BRM9+i59AE16ANlE4wv4Mp5MjI8gZ+ONEMJBAzaWwZsVf2VnstPZpfb7STh06ewu5JuFnSzQ8d5vDfiLa3sHcHiIAoIoIIgCgiggitwbWM/f2vniTe7NoIZ7Dz9Y0X0qy7NHYfbLtn6dfzOoYXPlUl4+IIgCooGXj10ngzM77p81vVmY2Y9vL+xi9Tn4f41HYVZYx3Wb3yws9oWBlw8IooAgCgiigCAKCKKAIAoIooAgCoikGU3nqpvy3qesPvv6+/2+LZfLpHUcsrrPD0cKCKKAIAoIooAgCgiigCAKCOecs7q3iJXbZDLZWVaWZfR4733lLbfZbBbchzZvvV4vy+PmSAFBFBBEAUEUEEQBQRQQRAFR5DzfD81FxMxVpMg9l3HT938fjhQQRAFBFBBEAUEUEEQBQRQQRe5z7SptnYejGkcKCKKAIAoIooAgCgiigCAKiKQoYj6bMB6Pd8aMRqPoz22kfCalzfmXm45nDoIoIIgCgiggiAKCKCCIAiJrFKnfTxHS9vdX5P7+ibZwpIAgCgiigCAKCKKAIAoIooDomNl2352hc+WY3+NYzyf2c345V3EyGNmdwevo8anbr3Lbfu/j+9fndrH69Ofv+48+WtF9JuM4UkAQBQRRQBAFBFFAEAUEUUBUfo9m6jUPzjl7eWr26vRyWVmW9u59GT2+Suo1B4vFImn8/4ojBQRRQBAFBFFAEAUEUUAQBUTHe7/3eorUeYrQ9RSprmP/UtZ/6OP/xfUUqI0oIIgCgiggiqY36Ddz25x/uZZ1PXmcNj60H6H1H/p4sV1F/VvjZx84HJx9IFrl733wexy3U/b3FO7ogR0dD7OsezqdVt4/HFZvNzQ+t9T9C40P6ty9erElfEKsbblnDHNrekYzFu8pIIgCgiggiAKCKCAqzz5Ccr+7T3133fb1DG0//ro4UkAQBQRRQBAFBFFAEAXEb3wL3JblytFeAAAAAElFTkSuQmCC" },
                        { "mimeType", "application/pdf" },
                        { "text", "Additional evidence for the dispute." },
                        { "message", "Submitting dispute case with evidence." }
                    };
                    var result = await payarc.DisputeService.AddDocument(caseId, documentParams);
                    Console.WriteLine($"Add Document Result: {result}");
                    break;
                case "splitCampaignService":
                    // Example: Create a new campaign
                    var newCampaign = new JObject {
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
                    if ((bool)createdCampaign.SelectToken("IsSuccess")) {
                        createdCampaignId = (string)createdCampaign["data"].SelectToken("id");
                    }

                    // Example: Get all campaigns
                    var allCampaigns = await payarc.SplitCampaignService.list();
                    Console.WriteLine($"All Campaigns: {allCampaigns}");

                    // Example: Get campaign details
                    var campaignDetails = await payarc.SplitCampaignService.retrieve(createdCampaignId);
                    Console.WriteLine($"Campaign Details: {campaignDetails}");

                    // Example: Update a campaign
                    var updatedData = new JObject {
                        ["budget"] = 6000
                    };

                    var updatedCampaign = await payarc.SplitCampaignService.update(createdCampaignId, updatedData);
                    Console.WriteLine($"Updated Campaign: {updatedCampaign}");

                    // Example: Get all accounts
                    var allAccounts = await payarc.SplitCampaignService.listAccounts();
                    Console.WriteLine($"All Accounts: {allAccounts}");
                    break;
                case "chargeService":

                    //var testChargeAction = "CreateCharge";
                    //var testChargeAction = "RefundChargeById";
                    //var testChargeAction = "RefundChargeByObject";
                    //var testChargeAction = "CreateChargeByCardId";
                    //var testChargeAction = "CreateChargeByCustomerId";
                    //var testChargeAction = "GetChargeById";
                    //var testChargeAction = "CreateChargeByToken";
                    var testChargeAction = "ListCharges";

                    switch (testChargeAction) {
                        case "CreateCharge":
                            try {
                                var options = new ChargeCreateOptions {
                                    Amount = 635,
                                    Source = new SourceNestedOptions {
                                        CardNumber = "4012000098765439",
                                        ExpMonth = "03",
                                        ExpYear = "2025",
                                        CountyCode = "USA",
                                        City = "GreenWitch",
                                        AddressLine1 = "123 Main Street",
                                        ZipCode = "12345",
                                        State = "CA"
                                    },
                                    Currency = "usd"
                                };
                                var charge = await payarc.Charges.Create(options);
                                Console.WriteLine("Charge Data");
                                Console.WriteLine(charge);
                                Console.WriteLine("Raw Data");
                                var json = JsonSerializer.Deserialize<JsonDocument>(charge?.RawData);
                                Console.WriteLine(json.RootElement.GetProperty("id"));
                            } catch (Exception ex) {
                                Console.WriteLine(ex.Message);
                            }
                            break;
                        case "RefundChargeById":
                            try {
                                string charge = "ch_BoLWODoBLLDyLOXy";
                                var refund = await payarc.Charges.CreateRefund(charge, null);
                                Console.WriteLine("Refund Data");
                                Console.WriteLine(refund);
                            } catch (Exception ex) {
                                Console.WriteLine(ex.Message);
                            }
                            break;
                        case "RefundChargeByObject":
                            try {
                                ChargeResponseData? charge = (ChargeResponseData?)await payarc.Charges.Retrieve("ch_DMWbOLWbyyoRLOBX");
                                var refund = await charge.CreateRefund(null);
                                Console.WriteLine("Refund Data");
                                Console.WriteLine(refund);
                            } catch (Exception e) {
                                Console.WriteLine(e);
                                throw;
                            }
                            break;
                        case "CreateChargeByCardId":
                            try {
                                var options = new ChargeCreateOptions {
                                    Amount = 155,
                                    Source = new SourceNestedOptions {
                                        CardId = "card_Ly9v09NN2P59M0m1",
                                        CustomerId = "cus_jMNKVMPKnNxPVnDp"
                                    },
                                    Currency = "usd"
                                };
                                var charge = await payarc.Charges.Create(options);
                                Console.WriteLine("Charge Data");
                                Console.WriteLine(charge);
                                Console.WriteLine("Raw Data");
                                Console.WriteLine(charge?.RawData);
                            } catch (Exception ex) {
                                Console.WriteLine(ex.Message);
                            }
                            break;
                        case "CreateChargeByCustomerId":
                            try {
                                var options = new ChargeCreateOptions {
                                    Amount = 255,
                                    Source = new SourceNestedOptions {
                                        CustomerId = "cus_jMNKVMPKnNxPVnDp"
                                    },
                                    Currency = "usd"
                                };
                                var charge = await payarc.Charges.Create(options);
                                Console.WriteLine("Charge Data");
                                Console.WriteLine(charge);
                                Console.WriteLine("Raw Data");
                                Console.WriteLine(charge?.RawData);
                            } catch (Exception ex) {
                                Console.WriteLine(ex.Message);
                            }
                            break;
                        case "GetChargeById":
                            try {
                                // var charge = await Payarc.Charges.Retrieve("ch_MnBROWLXBBXnoOWL");
                                var charge = await payarc.Charges.Retrieve("ch_XMbnObBXDDbMXORo");
                                Console.WriteLine("Get charge By Id Data");
                                Console.WriteLine(charge);
                                Console.WriteLine("Raw Data");
                                Console.WriteLine(charge?.RawData);
                            } catch (Exception ex) {
                                Console.WriteLine(ex.Message);
                            }
                            break;
                        case "CreateChargeByToken":
                            try {
                                var options = new ChargeCreateOptions {
                                    Amount = 175,
                                    Source = new SourceNestedOptions {
                                        TokenId = "tok_mLY0wmYlL0mNEw8q"
                                    },
                                    Currency = "usd"
                                };
                                var charge = await payarc.Charges.Create(options);
                                Console.WriteLine("Charge Data");
                                Console.WriteLine(charge);
                                Console.WriteLine("Raw Data");
                                Console.WriteLine(charge?.RawData);
                            } catch (Exception ex) {
                                Console.WriteLine(ex.Message);
                            }
                            break;
                        case "ListCharges":
                            try {
                                var options = new OptionsData() {
                                    Limit = 25,
                                    Page = 1,
                                };
                                var responseData = await payarc.Charges.List(options);
                                Console.WriteLine("Charges Data");
                                for (int i = 0; i < responseData?.Data?.Count; i++) {
                                    var t = responseData.Data[i];
                                    Console.WriteLine(responseData.Data[i]);
                                }
                                Console.WriteLine("Pagination Data");
                                Console.WriteLine(responseData?.Pagination["total"]);
                                Console.WriteLine(responseData?.Pagination["count"]);
                                Console.WriteLine(responseData?.Pagination["per_page"]);
                                Console.WriteLine(responseData?.Pagination["current_page"]);
                                Console.WriteLine(responseData?.Pagination["total_pages"]);
                                // Console.WriteLine("Raw Data");
                                // Console.WriteLine(responseData?.RawData);
                            } catch (Exception e) {
                                Console.WriteLine(e);
                                throw;
                            }
                            break;
                        default:
                            Console.WriteLine("Nothing to test.");
                            break;
                    }
                    break;
                case "payarcConnect":
                    string deviceSerialNo = "1851085026"; // switch this to serial number of the device you are testing with

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
            Console.WriteLine("Finished");
        }
    }
}