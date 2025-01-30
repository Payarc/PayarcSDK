using System.Text.Json;
using PayarcSDK.Entities;
using PayarcSDK.Entities.Billing.Subscriptions;

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
                    Page = 1,
                    Search = "test"
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
                    Name = "Monthly billing regular test update 2",
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
        
        public async Task DeletePlanById()
        {
            try
            {
                await _payarc.Billing.Plan.Delete("plan_50c1b33a");
                Console.WriteLine("Plan was Deleted");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task CreateSubscription()
        {
            try
            {
                var plans = await _payarc.Billing.Plan.List(new PlanListOptions {Search = "test"});
                var options = new SubscriptionCreateOptions()
                {
                    CustomerId = "cus_DPNMVjx4AMNNVnjA"
                };
                if (plans?.Data?.Count > 0)
                {
                    var plan = (PlanResponseData?)plans?.Data?[8];
                    if (plan != null)
                    {
                        var subscription = await plan.CreateSubscription(options) as SubscriptionResponseData;
                        Console.WriteLine("Subscription was Created");
                        Console.WriteLine(subscription);
                        Console.WriteLine("Raw Data");
                        Console.WriteLine(subscription?.RawData);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        public async Task CreateSubscriptionById()
        {
            try
            {
                var options = new SubscriptionCreateOptions()
                {
                    CustomerId = "cus_DPNMVjx4AMNNVnjA"
                };
                var subScription = await _payarc.Billing.Plan.CreateSubscription("plan_08dbb4bb", options) as SubscriptionResponseData;
                Console.WriteLine("Subscription was Created");
                Console.WriteLine(subScription);
                Console.WriteLine("Raw Data");
                Console.WriteLine(subScription?.RawData);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task UpdateSubscription()
        {
            try
            {
                var options = new UpdateSubscriptionOptions()
                {
                    Description = "new tax percent 50",
                    TaxPercent = 50.0
                };
                var subs = await _payarc.Billing.Subscription.Update("sub_lRV0PjPVxXxjgxXr", options) as SubscriptionResponseData;
                Console.WriteLine("Subscription was Updated");
                Console.WriteLine(subs);
                Console.WriteLine("Raw Data");
                Console.WriteLine(subs?.RawData);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        public async Task UpdateSubscriptionByObject()
        {
            try
            {
                var subs = await _payarc.Billing.Subscription.List(new SubscriptionListOptions() {Search = "test"});
                var options = new UpdateSubscriptionOptions()
                {
                    Description = "new tax percent 55.3",
                    TaxPercent = 55.3
                };
                if (subs?.Data?.Count > 0)
                {
                    var sub = (SubscriptionResponseData?)subs?.Data?[0];
                    if (sub != null)
                    {
                        var subscription = await sub.Update(options) as SubscriptionResponseData;
                        Console.WriteLine("Subscription was Created");
                        Console.WriteLine(subscription);
                        Console.WriteLine("Raw Data");
                        Console.WriteLine(subscription?.RawData);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task CancelSubscription()
        {
            try
            {
                var subs = await _payarc.Billing.Subscription.Cancel("sub_lRV0PjPVxXxjgxXr") as SubscriptionResponseData;
                Console.WriteLine("Subscription was Canceled");
                Console.WriteLine(subs);
                Console.WriteLine("Raw Data");
                Console.WriteLine(subs?.RawData);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public async Task ListSubscriptions()
        {
            try
            {
                var options = new SubscriptionListOptions()
                {
                   Search = "test"
                };
                var subscription = await _payarc.Billing.Subscription.List(options);
                Console.WriteLine("Subscriptions Data");
                for (int i = 0; i < subscription?.Data?.Count; i++)
                {
                    var t = subscription.Data[i];
                    Console.WriteLine(subscription.Data[i]);
                }
                Console.WriteLine("Pagination Data");
                Console.WriteLine($"Total: {subscription?.Pagination["total"]}");
                Console.WriteLine($"Count: {subscription?.Pagination["count"]}");
                Console.WriteLine($"Per Page {subscription?.Pagination["per_page"]}");
                Console.WriteLine($"Current Page {subscription?.Pagination["current_page"]}");
                Console.WriteLine($"Total Pages: {subscription?.Pagination["total_pages"]}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
}