using System.Text.Json;
using PayarcSDK.Entities;

namespace PayarcSDK.Sample;

public class ApiRequester
{
    private Payarc _payarc;
    
    public ApiRequester(Payarc payarc)
    {
        _payarc = payarc;
    }
    public async Task CreateChargeExample()
        {
            try
            {
                var options = new ChargeCreateOptions
                {
                    Amount = 435,
                    Source = new SourceNestedOptions
                    {
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
                var charge = await _payarc.Charges.Create(options);
                Console.WriteLine("Charge Data");
                Console.WriteLine(charge);
                Console.WriteLine("Raw Data");
                var json = JsonSerializer.Deserialize<JsonDocument>(charge?.RawData);
                Console.WriteLine(json.RootElement.GetProperty("id"));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        
        }
        public async Task GetChargeById()
        {
            try
            {
                var charge = await _payarc.Charges.Retrieve("ch_obLyORByLLbRWOWn");
                Console.WriteLine("Get charge By Id Data");
                Console.WriteLine(charge);
                Console.WriteLine("Raw Data");
                Console.WriteLine(charge?.RawData);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        
        public async Task CreateChargeByCardIdExample()
        {
            try
            {
                var options = new ChargeCreateOptions
                {
                    Amount = 183,
                    Source = new SourceNestedOptions
                    {
                        CardId = "card_Ly9v09NN2P59M0m1",
                        CustomerId = "cus_jMNKVMPKnNxPVnDp"
                    },
                    Currency = "usd"
                };
                var charge = await _payarc.Charges.Create(options);
                Console.WriteLine("Charge Data");
                Console.WriteLine(charge);
                Console.WriteLine("Raw Data");
                Console.WriteLine(charge?.RawData);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        
        }
        
        public async Task CreateChargeByCustomerIdExample()
        {
            try
            {
                var options = new ChargeCreateOptions
                {
                    Amount = 655,
                    Source = new SourceNestedOptions
                    {
                        CustomerId = "cus_jMNKVMPKnNxPVnDp"
                    },
                    Currency = "usd"
                };
                var charge = await _payarc.Charges.Create(options);
                Console.WriteLine("Charge Data");
                Console.WriteLine(charge);
                Console.WriteLine("Raw Data");
                Console.WriteLine(charge?.RawData);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
       
        }

        public  async Task CreateChargeByToken()
        {
            try
            {
                var options = new ChargeCreateOptions
                {
                    Amount = 375,
                    Source = new SourceNestedOptions
                    {
                        TokenId = "tok_mLY0wmYlL0mNEw8q"
                    },
                    Currency = "usd"
                };
                var charge = await _payarc.Charges.Create(options);
                Console.WriteLine("Charge Data");
                Console.WriteLine(charge);
                Console.WriteLine("Raw Data");
                Console.WriteLine(charge?.RawData);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
      
        }
        
        public  async Task CreateACHChargeByBankAccount()
        {
            try
            {
                var options = new ChargeCreateOptions
                {
                    Amount = 3345,
                    SecCode = "WEB",
                    Source = new SourceNestedOptions
                    {
                        BankAccountId = "bnk_dJ356Dkj6ekjaELe" // customer bank account
                    },
                    Currency = "usd"
                };
                var charge = await _payarc.Charges.Create(options);
                Console.WriteLine("Charge Data");
                Console.WriteLine(charge);
                Console.WriteLine("Raw Data");
                Console.WriteLine(charge?.RawData);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        
        public  async Task CreateACHChargeByBankAccountDetails()
        {
            try
            {
                var options = new ChargeCreateOptions
                {
                    Amount = 2345,
                    SecCode = "WEB",
                    Source = new SourceNestedOptions
                    {
                        AccountNumber = "123432575352",
                        RoutingNumber = "123345349",
                        FirstName = "Boris",
                        LastName = "III",
                        AccountType = "Personal Savings"
                            
                    },
                    Currency = "usd"
                };
                var charge = await _payarc.Charges.Create(options);
                Console.WriteLine("Charge Data");
                Console.WriteLine(charge);
                Console.WriteLine("Raw Data");
                Console.WriteLine(charge?.RawData);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public async Task RefundChargeById()
        {
            try
            {
                string charge = "ch_BoLWODoBLLDyLOXy";
                var refund = await _payarc.Charges.CreateRefund(charge, null);
                Console.WriteLine("Refund Data");
                Console.WriteLine(refund);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task RefundChargeByObject()
        {
            try
            {
                var charge = await _payarc.Charges.Retrieve("ch_DMWbOLWbyyoRLOBX") as ChargeResponseData;
                var refund = await charge.CreateRefund(null) as ChargeResponseData;
                Console.WriteLine("Refund Data");
                Console.WriteLine(refund);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        
        public async Task RefundACHChargeByObject()
        {
            try
            {
                var charge = await _payarc.Charges.Retrieve("ach_g9dDE7GDdeDG08eA") as AchChargeResponseData;
                var refund = await charge.CreateRefund(null) as AchChargeResponseData;
                Console.WriteLine("Refund Data");
                Console.WriteLine(refund);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        
        public async Task ListCharges()
        {
            try
            {
                var options = new ChargeListOptions()
                {
                    Limit = 25,
                    Page = 1,
                };
                var responseData = await _payarc.Charges.List(options);
                Console.WriteLine("Charges Data");
                for (int i = 0; i < responseData?.Data?.Count; i++)
                {
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
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
       
        }
        
        public async Task CreatePlan()
        {
            try
            {
                var options = new PlanCreateOptions()
                {
                    Name = "Monthly billing regular test",
                    Amount = 692,
                    Interval = "month",
                    StatementDescriptor = "2025 MerchantT. Rglr srvc",
                };
                var plan = await _payarc.Billing.Plan.Create(options) as PlanResponseData;
                Console.WriteLine("Plan was Created");
                Console.WriteLine(plan);
                Console.WriteLine("Raw Data");
                Console.WriteLine(plan?.RawData);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public async Task getPlanById()
        {
            try
            {
                var plan = await _payarc.Billing.Plan.Retrieve("plan_50c1b33a") as PlanResponseData;
                Console.WriteLine("Plan was Retrieved");
                Console.WriteLine(plan);
                Console.WriteLine("Raw Data");
                Console.WriteLine(plan?.RawData);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        
        public async Task ListPlans()
        {
            try
            {
                var options = new PlanListOptions()
                {
                    Limit = 10,
                    Page = 3,
                };
                var responseData = await _payarc.Billing.Plan.List(options);
                Console.WriteLine("Plans Data");
                for (int i = 0; i < responseData?.Data?.Count; i++)
                {
                    var t = responseData.Data[i];
                    Console.WriteLine(responseData.Data[i]);
                }
                Console.WriteLine("Pagination Data");
                Console.WriteLine($"Total: {responseData?.Pagination["total"]}");
                Console.WriteLine($"Count: {responseData?.Pagination["count"]}");
                Console.WriteLine($"Per Page {responseData?.Pagination["per_page"]}");
                Console.WriteLine($"Current Page {responseData?.Pagination["current_page"]}");
                Console.WriteLine($"Total Pages: {responseData?.Pagination["total_pages"]}");
                // Console.WriteLine("Raw Data");
                // Console.WriteLine(responseData?.RawData);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task UpdatePlanByIdFromList()
        {
            try
            {
                var options = new UpdatePlanOptions()
                {
                    Name = "Monthly billing regular test update",
                };
                var responseData = await _payarc.Billing.Plan.List();
                if (responseData?.Data?.Count > 0)
                {
                    var plan = (PlanResponseData?)responseData.Data[0];
                    if (plan != null)
                    {
                        if (plan.ObjectId != null)
                        {
                            var uPlan = await _payarc.Billing.Plan.Update(plan.ObjectId, options) as PlanResponseData;
                            Console.WriteLine("Plan was Updated");
                            Console.WriteLine(uPlan);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }
        
        public async Task UpdatePlanById()
        {
            try
            {
                var options = new UpdatePlanOptions()
                {
                    Name = "Monthly billing regular test update",
                };
                var uPlan = await _payarc.Billing.Plan.Update("plan_1b82c2af", options) as PlanResponseData;
                Console.WriteLine("Plan was Updated");
                Console.WriteLine(uPlan);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }
}