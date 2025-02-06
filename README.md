# Payarc SDK for C#

The Payarc SDK allows developers to integrate Payarc's payment processing capabilities into their applications with ease. This SDK provides a comprehensive set of APIs to handle transactions, customer management, and candidate merchant management.

## Table of Contents
- [Requirements](#requirements)
- [Installation](#installation)
- [Dependencies](#dependencies)
- [Usage](#usage)
- [API Reference](#api-reference)
- [Examples](#examples)
- [License](#license)

## Requirements
.NET 8 or later.

## Installation
You can install the Payarc SDK package using the .NET CLI with the following command:

```sh
dotnet add package <placeholder_package_name> 
```

## Usage

Before using the Payarc SDK, you must initialize it with your **API key** and the appropriate **environment**. This ensures your requests are authenticated and directed to the correct API endpoint.  

### Available Environments  
Each environment has unique API keys and endpoints:  

- **`prod`** – Production environment (live transactions).  
- **`sandbox`** – Testing environment (no real transactions).  
- **`payarcConnect`** – Production environment for Payarc Connect.  
- **`payarcConnectDev`** – Development/testing for Payarc Connect.

> ⚠️ Keep your API key **secure** and **never expose it** to clients. This information should remain on your server.

### Managing API Credentials  
The examples provided use [dotenv.net](https://www.nuget.org/packages/dotenv.net) to store API credentials securely and load them in the constructor. However, this is **not mandatory**—you may use any configuration method that fits your setup.  

### Candidate Merchant Functionality  
To access **candidate merchant** features, you need an **Agent Identification Token**, which can be obtained from the Payarc portal.

### Configuration Setup

1. Create the `.env` file in the root of your project.
2. Add the following rows and update with your environment-specific values.

    ```ini
    PAYARC_ENV=''
    PAYARC_KEY=''
    AGENT_KEY=''
    PAYARC_VERSION=''
    ```

3. Install the [dotenv.net](https://www.nuget.org/packages/dotenv.net) package

    ```sh
    dotnet add package dotenv.net
    ```

4. Create an instance of the Payarc SDK which can be used to call different methods depending on your business needs. The following code provides a guideline:

    ```csharp
    // Load the environment variables from your .env file
    Dotenv.Load();

    // Now build the Payarc SDK
    Payarc payarc = new SdkBuilder()
        .Configure(config => {
            config.Environment = Environment.GetEnvironmentVariable("PAYARC_ENV") ?? "sandbox";
            config.ApiVersion = Environment.GetEnvironmentVariable("PAYARC_VERSION") ?? "v1"; 
            config.BearerToken = Environment.GetEnvironmentVariable("PAYARC_KEY"); 
        })
        .Build();
    ```

    If there are no errors, you are good to go!

## API Reference
Comprehensive documentation for Payarc's payment processing and candidate merchant management APIs is available at: https://docs.payarc.net/

## Examples
The Payarc SDK is build around the `Payarc` object. From this object you can access properties and function that will support your operations.

#### The `Payarc` object has the following services:
- **Charges** - Manage payments
- **Customers** - Manage customers  
- **Applications** - Manage candidate merchants  
- **SplitCampaigns** - Manage split campaigns
- **Billing**  
    - **Plan** - Manage plans  
    - **Subscription** - Manage plan subscriptions

> [!NOTE]
> The `Payarc` object referenced in the below examples will be created as `payarc`, you can name it whatever you'd like though.

## Service `payarc.Charges`
#### The `payarc.Charges` service facilitates the management of payments within the system, providing the following functions:
- **Create** - Initiates a payment intent or charge with various configurable parameters. Refer to the examples for specific use cases.
- **Retrieve** - Fetches a JSON object `charge` containing detailed information about a specific charge.
- **List** - Returns an object containing two attributes:  `charges` and `pagination`. The `charges` attribute is a list of JSON objects, each providing detailed information about individual charges. The `pagination` attribute contains deatails for navigating through the list of charges.
- **CreateRefund** - Processes a refund for an existing charge.

## Creating a Charge

### Example: Create a Charge with Minimum Information
To create a payment (charge) from a customer, the following minimum information is required:
- `Amount` - Converted to cents (e.g. $1.00 &rarr; 100).
- `Currency` - Must be set to 'usd'.
- `Source` - The credit card to be charged the specified amount.

> [!NOTE]
> For credit card minimum needed attributes are `CardNumber` and the expiration date, `ExpMonth` and `ExpYear`. For full list of attributes see API documentation.

#### This example demonstrates how to create a charge with the minimum required information:
```csharp
try {
    var charge = await payarc.Charges.Create(new ChargeCreateOptions {
        Amount = 2860,
        Currency = "usd",
        Source = new SourceNestedOptions {
            CardNumber = "4012******5439",
            ExpMonth = "03",
            ExpYear = "2025",
        }
    });
    Console.WriteLine($"Charge created: {JsonSerializer.Serialize(charge)}");
} catch (Exception ex) {
    Console.WriteLine($"Error detected: {ex.Message}");
}
```

### Example: Create a Charge by Token
To create a payment (charge) from a customer, the following minimum information is required:
- `Amount` - Converted to cents (e.g. $1.00 &rarr; 100).
- `Currency` - Must be set to 'usd'.
- `Source` An object containing the `TokenId` attribute. This can be obtained by using the [CREATE TOKEN API](https://docs.payarc.net/#ee16415a-8d0c-4a71-a5fe-48257ca410d7) to generate a token.

#### This example demonstrates how to create a charge using a token:
```csharp
try {
    var charge = await payarc.Charges.Create(new ChargeCreateOptions {
        Amount = 1285,
        Currency = "usd",
        Source = new SourceNestedOptions {
            TokenId = "tok_mE*****LL8wYl",
        }
    });
    Console.WriteLine($"Charge created: {JsonSerializer.Serialize(charge)}");
} catch (Exception ex) {
    Console.WriteLine($"Error detected: {ex.Message}");
}
```

### Example: Create a Charge by Card ID
Charges can be generated for a specific credit cards if you have the `CardId` and the associated `CustomerId` linked to the card.

#### This example demonstrates how to create a charge using a card ID:
```csharp
try {
    var charge = await payarc.Charges.Create(new ChargeCreateOptions {
        Amount = 3985,
        Currency = "usd",
        Source = new SourceNestedOptions {
            CardId = "card_Ly9*****59M0m1",
            CustomerId = "cus_j*******PVnDp"
        }
    });
    Console.WriteLine($"Charge created: {JsonSerializer.Serialize(charge)}");
} catch (Exception ex) {
    Console.WriteLine($"Error detected: {ex.Message}");
}
```

### Example: Create a Charge by Bank account ID
Charges can be generated for a specific bank account if you have the `BankAccountId`, which is the bank account of the customer.

#### This example shows how to create an ACH charge using a bank account ID:
```csharp
try {
    var charge = await payarc.Charges.Create(new ChargeCreateOptions {
        Amount = 3785,
        SecCode = "WEB",
        Currency = "usd",
        Source = new SourceNestedOptions {
            BankAccountId = "bnk_eJjbbbbbblL"
        }
    });
    Console.WriteLine($"Charge created: {JsonSerializer.Serialize(charge)}");
} catch (Exception ex) {
    Console.WriteLine($"Error detected: {ex.Message}");
}
```

### Example: Create a Charge with a bank account
Charges can be generated for a bank account if you have the bank account information. Details for the bank account are in the `Source` attribute.

#### This example shows how to create an ACH charge with new bank account:
```csharp
try {
    var charge = await payarc.Charges.Create(new ChargeCreateOptions {
        Amount = 3785,
        SecCode = "WEB",
        Currency = "usd",
        Source = new SourceNestedOptions {
            AccountNumber = "123432575352",
            RoutingNumber = "123345349",
            FirstName = "FirstName",
            LastName = "LastName",
            AccountType = "Personal Savings"
        }
    });
    Console.WriteLine($"Charge created: {JsonSerializer.Serialize(charge)}");
} catch (Exception ex) {
    Console.WriteLine($"Error detected: {ex.Message}");
}
```

## Retrieving a Charge

### Example: Retrieve a Charge

#### This example shows how to retrieve a specific charge by its ID:
```csharp
try {
    var charge = await payarc.Charges.Retrieve("ch_1J*****3");
    Console.WriteLine($"Charge retrieved {JsonSerializer.Serialize(charge)}");
} catch (Exception ex) {
    Console.WriteLine($"Error detected: {ex.Message}");
}
```

### Example: Retrieve an ACH Charge

#### This example shows how to retrieve a specific ACH charge by its ID:
```csharp
try {
    var charge = await payarc.Charges.Retrieve("ach_1J*****3");
    Console.WriteLine($"Charge retrieved: {JsonSerializer.Serialize(charge)}");
} catch (Exception ex) {
    Console.WriteLine($"Error detected: {ex.Message}");
}
```

## Listing Charges

### Example: List Charges with No Constraints

#### This example demonstrates how to list all charges without any constraints:
```csharp
try {
    var charges = await payarc.Charges.List(new BaseListOptions {
        Limit = 25,
        Page = 1
    });
    Console.WriteLine($"Charges list: {JsonSerializer.Serialize(charges)}");
} catch (Exception ex) {
    Console.WriteLine($"Error detected: {ex.Message}");
}
```

## Refunding a Charge

### Example: Refund a Charge
Charges can be refunded using the `ch_` prefix for regular charges and the `ach_` prefix for ACH charges.

#### This example demonstrates how to refund a charge, whether it's a regular charge or an ACH charge, using its respective ID:
```csharp
try {
    string id = "ach_g**********08eA";
    var options = new Dictionary<string, object> {                                
        { "reason", "requested_by_customer" },
        { "description", "The customer returned the product, did not like it" }
    };

    var charge = await payarc.Charges.CreateRefund(id, options);
    Console.WriteLine($"Charge refunded: {JsonConvert.SerializeObject(charge)}");
} catch (Exception e) {
    Console.WriteLine($"Error detected: {e.Message}");
}
```

<br/>

## Service `payarc.Customers`
#### The `payarc.Customers` service manages your customers' personal information, including addresses and payment methods such as credit cards and bank accounts. This data is securely stored for future transactions.
- **Create** - Creates a customer object in the database and generates a unique identifier for future reference and inquiries. See examples and documentation for more details.
- **Retrieve** - Retrieves detailed information about a specific customer from the database.
- **List** - Searches through previously created customers, allowing filtering based on specific criteria. See examples and documentation for more details.
- **Update** - Modifies attributes of an existing customer object.
- **Delete** - Removes a customer object from the database.

## Creating a Customer

### Example: Create a Customer with Credit Card Information

#### This example shows how to create a new customer with credit card information:
```csharp
try {
    var customerData = new Dictionary<string, object> {
        { "email", "anon+50@example.com" },
        { "cards", new List<Dictionary<string, object>> {
                new Dictionary<string, object> {
                    { "card_source", "INTERNET" },
                    { "card_number", "4012000098765439" },
                    { "exp_month", "07" },
                    { "exp_year", "2025" },
                    { "cvv", "997" },
                    { "card_holder_name", "Bat Doncho" },
                    { "address_line1", "123 Main Street" },
                    { "city", "Greenwich" },
                    { "state", "CT" },
                    { "zip", "06830" },
                    { "country", "US" }
                },
                new Dictionary<string, object> {
                    { "card_source", "INTERNET" },
                    { "card_number", "4012000098765439" },
                    { "exp_month", "01" },
                    { "exp_year", "2025" },
                    { "cvv", "998" },
                    { "card_holder_name", "Bat Gancho" },
                    { "address_line1", "123 Main Street Apt 44" },
                    { "city", "Greenwich" },
                    { "state", "CT" },
                    { "zip", "06830" },
                    { "country", "US" }
                }
            }
        }
    };

    var customer = await payarc.Customers.Create(customerData);
    Console.WriteLine($"Customer created: {JsonConvert.SerializeObject(customer)}");
} catch (Exception e) {
    Console.WriteLine($"Error detected: {e.Message}");
}
```

## Retrieving a Customer

### Example: Retrieve a Customer by their ID:

#### This example demonstrates how to retrieve a customer given their unique customer ID:
```csharp
try {
    var customer = await payarc.Customers.Retrieve("cus_j*******p");
    Console.WriteLine($"Customer retrieved: {JsonConvert.SerializeObject(customer)}");
} catch (Exception e) {
    Console.WriteLine($"Error detected: {e.Message}");
}
```

## Listing Customers

### Example: List Customers with a Limit

#### This example demonstrates how to list customers with a specified limit:
```csharp
try {
    var customers = await payarc.Customers.List(new OptionsData {
        Limit = 3
    });
    Console.WriteLine($"Customers retrieved: {JsonConvert.SerializeObject(customers)}");
} catch (Exception e) {
    Console.WriteLine($"Error detected: {e.Message}");
}
```

## Updating a Customer 

### Example: Update a Customer

#### This example demonstrates how to update an existing customer's information with only their customer ID:
```csharp
try {
    string id = "cus_j*******p";

    var customer = await payarc.Customers.Update(id, new CustomerRequestData {
        Name = "Bai Doncho 3",
        Description = "Example customer",
        Phone = 1234567890
    });
    Console.WriteLine($"Customer updated: {JsonConvert.SerializeObject(customer)}");
} catch (Exception e) {
    Console.WriteLine($"Error detected: {e.Message}");
}
```

### Example: Add a New Card to a Customer

#### This example shows how to add a new card to an existing customer:
```csharp
try {
    var newCard = new CardData {
        CardSource = "INTERNET",
        CardNumber = "4012000098765439",
        ExpMonth = "01",
        ExpYear = "2025",
        Cvv = "998",
        CardHolderName = "Bat Gancho",
        AddressLine1 = "123 Main Street Apt 44",
        City = "Greenwich",
        State = "CT",
        Zip = "06830",
        Country = "US"
    };

    var customerData = new CustomerRequestData {
        Cards = new List<CardData> { newCard }
    };

    var customer = await payarc.Customers.Update("cus_j*******p", customerData);
    Console.WriteLine($"Card added: {JsonConvert.SerializeObject(customer)}");
} catch (Exception e) {
    Console.WriteLine($"Error detected: {e.Message}");
}
```

### Example: Add a New Bank Account to a Customer

#### This example shows how to add new bank account to a customer. See full list of bank account attributes in API documentation.
```csharp
try {
    var newBankAccount = new BankData {
        AccountNumber = "1234567890",
        RoutingNumber = "110000000",
        FirstName = "Bat Petio",
        LastName = "The Tsar",
        AccountType = "Personal Savings",
        SecCode = "WEB"
    };

    var customerData = new CustomerRequestData {
        BankAccounts = new List<BankData> { newBankAccount }
    };

    var customer = await payarc.Customers.Update("cus_j*******p", customerData);
    Console.WriteLine($"Bank account added: {JsonConvert.SerializeObject(customer)}");
} catch (Exception e) {
    Console.WriteLine($"Error detected: {e.Message}");
}
```

## Deleting a Customer

### Example: Delete Customer

#### This example shows how to delete customer. See more details in API documentation.
```csharp
  try {
    var customer = await payarc.Customers.Delete("cus_j*******p");                        
    Console.WriteLine($"Customer deleted: {JsonConvert.SerializeObject(customer)}");
} catch (Exception e) {
    Console.WriteLine($"Error detected: {e.Message}");
}
```

<br/>

## Service `payarc.Applications`
#### The `payarc.Applications` service is designed for Agents and ISVs to manage candidate merchants during new customer acquisition. It allows you to create, retrieve, list, and manage the necessary documents for the onboarding process.
- **Create** - Adds a new candidate merchant to the database. Refer to the documentation for available attributes, valid values, and required fields.
- **Retrieve** - Returns details of a specific candidate merchant based on an identifier or an object from the list function.
- **List** - Retrieves a list of application objects representing potential merchants. Use this function to find the relevant identifier.
- **Update** - Modifies the attributes of an existing candidate merchant. This function allows updating certain details such as business information, contact details, etc.
- **Delete** - Removes a candidate merchant's information if no longer needed.
- **AddDocument** - Attaches a base64-encoded document to an existing candidate merchant. For required document types, contact Payarc. See examples for usage.
- **DeleteDocument** - Removes a document when it is no longer valid.
- **ListSubAgents** - Facilitates the creation of a candidate merchant on behalf of another agent.
- **Submit** - Initiates the contract signing process between Payarc and the client.

## Creating a Candidate Merchant

### Example: Create a New Candidate Merchant
When connecting your clients with Payarc, a selection is made based on Payarc's criteria. The process begins by gathering the merchant's information and creating an entry in the database.

#### This example shows you how this process can be intiated:
```csharp
 try {
    var merchantCandidate = new ApplicationInfoData {
        Lead = new Lead {
            Industry = "cbd",
            MerchantName = "Kolio i sie",
            LegalName = "Best Co in w",
            ContactFirstName = "Joan",
            ContactLastName = "Dhow",
            ContactEmail = "contact+25@mail.com",
            DiscountRateProgram = "interchange"
        },
        Owners = new List<Owner> {
            new Owner {
                FirstName = "First",
                LastName = "Last",
                Title = "President",
                OwnershipPct = 100,
                Address = "Somewhere",
                City = "City Of Test",
                SSN = "4546-0034",
                State = "WY",
                ZipCode = "10102",
                BirthDate = "1993-06-24",
                Email = "nikoj@negointeresuva2.com",
                PhoneNo = "2346456784"
            }
        }
    };

    var merchant = await payarc.Applications.Create(merchantCandidate);
    Console.WriteLine($"Merchant candidate created: {JsonConvert.SerializeObject(merchant)}");
} catch (Exception e) {
    Console.WriteLine($"Error detected: {e.Message}");
}
```

### Example: Create Candidate Merchant on Behalf of Another Agent
The `Lead` attribute represents the business, while the `Owners` attribute is an array of objects representing the business owners. This example includes only the minimum required information; however, providing as much detail as possible is recommended for successful boarding. Refer to the documentation for additional details.

#### In some cases, a logged-in user may need to create an application on behalf of another agent. In such instances, the `ObjectId` of the agent must be included in the request sent to the `payarc.Applications.Create` function. To retrieve a list of agents, use the `ListSubAgents` function, as demonstrated in the examples below:
```csharp
try {
    var subAgent = await payarc.Applications.ListSubAgents(new OptionsData {
        Limit = 10,
        Page = 1
    });

    var agentId = subAgent?.Data?.FirstOrDefault()?.ObjectId ?? null;

    var merchantCandidate = new ApplicationInfoData {
        Lead = new Lead {
            Industry = "cbd",
            MerchantName = "chichovoto",
            LegalName = "Best Co in w",
            ContactFirstName = "Lubo",
            ContactLastName = "Penev",
            ContactEmail = "penata@chichovoto.com",
            DiscountRateProgram = "interchange"
        },
        Owners = new List<Owner>
        {
            new Owner
            {
                FirstName = "First",
                LastName = "Last",
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
        },
        agentId = agentId
    };

    var candidate = await payarc.Applications.Create(merchantCandidate);
    Console.WriteLine($"Merchant candidate created: {JsonConvert.SerializeObject(candidate)}");
} catch (Exception e) {
    Console.WriteLine($"Error detected: {e.Message}");
}
```

## Retrieving a Candidate Merchant

### Example: Retrieving a Candidate Merchant
To continue with onboarding process you might need to provide additional information or to inquiry existing leads. You can retrieve candidate merchants with the help of the following example.

#### This examples shows how to retrieve a candidate merchant using their ID:
```csharp
try {
    string id = "app_1J*****3";
    var merchant = await payarc.Applications.Retrieve(id);
    Console.WriteLine($"Merchant candidate retrieved: {JsonConvert.SerializeObject(merchant)}");
} catch (Exception e) {
    Console.WriteLine($"Error detected: {e.Message}");
}
```

## Listing Candidate Merchants

### Example: List All Candidate Merchants For an Agent

#### This example shows how to list all candidate merchants for a specified agent:
```csharp
try {				
    var applications = await payarc.Applications.List(new OptionsData { 
        Limit = 25,
        Page = 1
    });
    Console.WriteLine($"Applications retrieved: {JsonConvert.SerializeObject(applications)}");
} catch (Exception e) {
    Console.WriteLine($"Error detected: {e.Message}");
}
```

## Updating a Candidate Merchant

### Example: Update a Candidate Merchant using their ID:

#### This example shows how to update specific properties of a candidate merchant
```csharp
try {
    string id = "app_1J*****3";
    
    var payload = new ApplicationInfoData {
        MerchantBankAccountNo = "999999999",
        MerchantBankRoutingNo = "1848505",
        BankInstitutionName = "Bank of Kolio"
    };

    var updatedCandidate = await payarc.Applications.Update(id, payload);
    Console.WriteLine($"Candidate merchant updated: {JsonConvert.SerializeObject(updatedCandidate)}");
} catch (Exception e) {
    Console.WriteLine($"Error detected: {e.Message}");
}
```

## Deleting a Candidate Merchant

### Example: Delete a Candidate Merchant using their ID:

#### This example shows how to delete a candidate merchant using their specific ID:
```csharp
try {
    string id = "app_1J*****3";

    var deletedCandidate = await payarc.Applications.Delete(id);
    Console.WriteLine($"Candidate merchant deleted: {JsonConvert.SerializeObject(deletedCandidate)}");
} catch (Exception e) {
    Console.WriteLine($"Error detected: {e.Message}");
}
```

## Adding Documents to a Candidate Merchant Application

### Example: Add Supporting Documents to a Candidate Merchant

#### This example shows how to add documents to a candidate merchant

```csharp
try {
    string id = "app_1J*****3";

    var document = new List<MerchantDocument> {
        new MerchantDocument {
            DocumentType = "Business Bank Statement",
            DocumentName = "sample document 1",
            DocumentIndex = 12246,
            DocumentDataBase64 = "data:image/jpeg;base64,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"
        }
    };

    var addedDocument = await payarc.Applications.AddDocument(id, document);
    Console.WriteLine($"Document added: {JsonConvert.SerializeObject(addedDocument)}");
} catch (Exception e) {
    Console.WriteLine($"Error detected: {e.Message}");
}
```

## Deleting Documents from a Candidate Merchant Application

### Example: Delete a Document from a Candidate Merchant Application Using their ID and the Document ID

#### This example shows how to delete a document from a candidate merchant application using that candidate merchant's ID and the ID of the document you want to delete:
```csharp
try {
    string applicantId = "app_1J*****3";
    string documentId = "doc_1J*****3";

    var deletedDoc = await payarc.Applications.DeleteDocument(applicantId, documentId);
    Console.WriteLine($"Document deleted: {JsonConvert.SerializeObject(deletedDoc)}");
} catch (Exception e) {
    Console.WriteLine($"Error detected: {e.Message}");
}
```

<!-- ### Example: Deleting A Document From a Candidate Merchant By Fetching the Newest Merchant Candidate and Deleting The Last Added Document

#### This example shows how to delete a document by listing all of the candidate merchants, retrieving the newest one added to the system, and then deleting the first document associated with that merchant candidate:

```csharp
try {
    var response = await payarc.Applications.List(new OptionsData {
        Limit = 25,
        Page = 1
    });

    string applicantId = response?.Data?.FirstOrDefault()?.ObjectId ?? null;

    if (applicantId != null) {
        var details = await payarc.Applications.Retrieve(applicantId);
        string documentId = details?.ObjectId ?? null; // fix this line here

        if (documentId != null) {
            var deletedDoc = await payarc.Applications.DeleteDocument(applicantId, documentId);
            Console.WriteLine($"Document deleted: {JsonConvert.SerializeObject(deletedDoc)}");
        } else {
            Console.WriteLine("No document to delete.");
        }
    } else {
        Console.WriteLine("No candidate merchants found.");
    }
} catch (Exception e) {
    Console.WriteLine($"Error detected: {e.Message}");
}
``` -->

## Listing Subagents

### Example: List All Subagents with a Limit

#### This example shows how to list all subagents, using a limit:
```csharp
try {                
    var subAgent = await payarc.Applications.ListSubAgents(new OptionsData {
        Limit = 25,
        Page = 1
    });
    Console.WriteLine($"Subagents retrieved: {JsonConvert.SerializeObject(subAgent)}");
} catch (Exception e) {
    Console.WriteLine($"Error detected: {e.Message}");
}   
```

### Service `payarc.Billing`
#### The `payarc.Billing` service is responsible for managing recurring payments. It currently includes the `Plan` service for handling plans and the `Subscription` service for managing plan subscriptions.

### Service `payarc.Billing.Plan` 
#### The `payarc.Billing.Plan` service provides detailed information about each plan, including identification details, payment request rules, and additional attributes.
- **Create** - Programmatically create new plan objects to meet client needs.
- **List** - Retrieve a list of available plans.
- **Retrieve** - Obtain detailed information about a specific plan.
- **Update** - Modify details of a plan.
- **Delete** - Remove a plan when it is no longer needed.
- **CreateSubscription**: Generate a customer subscription based on a selected plan.

> [!NOTE]
> Based on the defined plans, you can create subscriptions. A scheduled job will automatically process and collect payments (charges) from customers according to the plan's payment schedule.

### Service `payarc.Billing.Subscription`
#### This abstraction encapsulate information for subscription the link between customer and plan. it has following methods:
**List** - Retrieve a list of all subscriptions.
**Cancel** - Stop and cancel an active subscription.
**Update** - Modify a specific subscription's details.

# Payarc Connect
The following functionality will pertain only to user who are utilizing the Payarc Connect integration:

### Login
This function must be called and completed before any other functionality can be used. 
```csharp
try {
    var result = await payarc.PayarcConnect.Login();
    Console.WriteLine("Result: " + JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true }));
} catch (Exception ex) {
    Console.WriteLine("Error detected: " + ex.Message);
}
```

### Sale
Initiate a sale remotely on your PAX terminal

| Parameter | Usage |
| --- | --- |
| TenderType | CREDIT, DEBIT |
| ECRRefNum | Unique code for this transaction provided by the user. This code will be used later for **voids.** |
| Amount | Amount to capture. Format is $$$$$$$CC |
| DeviceSerialNo | Serial number of your PAX terminal |
```csharp
try {
    var result = await payarc.PayarcConnect.Sale(
        tenderType: "CREDIT",
        ecrRefNum: "REF123",
        amount: "100",
        deviceSerialNo: "12345"
    );
    Console.WriteLine("Result: " + JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true }));
} catch (Exception ex) {
    Console.WriteLine("Error detected: " + ex.Message);
}
```

### Void
Initiate a void remotely on your PAX terminal

| Parameter | Usage |
| --- | --- |
| PayarcTransactionId | Unique code of a previous transaction. Required to do a void. Charge ID on Payarc Portal. |
| DeviceSerialNo | Serial number of your PAX terminal |
```csharp
try {
    var result = await payarc.PayarcConnect.Void(
        payarcTransactionId: "12345",
        deviceSerialNo: "12345"
    );
    Console.WriteLine("Result: " + JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true }));
} catch (Exception ex) {
    Console.WriteLine("Error detected: " + ex.Message);
}
```
### Refund
Initiate a refund remotely on your PAX terminal

| Parameter | Usage |
| --- | --- |
| Amount | Amount to capture. Format is $$$$$$$CC |
| PayarcTransactionId | Unique code of a previous transaction. Required to do a refund. Charge ID on Payarc Portal. |
| DeviceSerialNo | Serial number of your PAX terminal |
```csharp
try {
    var result = await payarc.PayarcConnect.Refund(
        amount: "100",
        payarcTransactionId: "12345",
        deviceSerialNo: "12345"
    );
    Console.WriteLine("Result: " + JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true }));
} catch (Exception ex) {
    Console.WriteLine("Error detected: " + ex.Message);
}
```
### Blind Credit
Initiate a blind credit remotely on your PAX terminal

| Parameter | Usage |
| --- | --- |
| ECRRefNum | Unique code for this transaction provided by the user. |
| Amount | Amount to capture. Format is $$$$$$$CC |
| Token | Required for Refund. Found in PaxResponse.ExtData |
| ExpDate | Required for Refund. Found in PaxResponse.ExtData. Expiration date of card used in sale |
| DeviceSerialNo | Serial number of your PAX terminal |
```csharp
try {
    var result = await payarc.PayarcConnect.BlindCredit(
        ecrRefNum: "REF123",
        amount: "100",
        token: "ABC123",
        expDate: "0000",
        deviceSerialNo: "12345"
    );
    Console.WriteLine("Result: " + JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true }));
} catch (Exception ex) {
    Console.WriteLine("Error detected: " + ex.Message);
}
```
### Auth
Initiate an auth remotely on your PAX terminal

| Parameter | Usage |
| --- | --- |
| ECRRefNum | Unique code for this transaction provided by the user |
| Amount | Amount to capture. Format is $$$$$$$CC |
| DeviceSerialNo | Serial number of your PAX terminal |
```csharp
try {
    var result = await payarc.PayarcConnect.Auth(
        ecrRefNum: "REF123",
        amount: "100",
        deviceSerialNo: "12345"
    );
    Console.WriteLine("Result: " + JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true }));
} catch (Exception ex) {
    Console.WriteLine("Error detected: " + ex.Message);
}
```
### Post Auth
Initiate a post auth remotely on your PAX terminal

| Parameter | Usage |
| --- | --- |
| ECRRefNum | Unique code for this transaction provided by the user |
| OrigRefNum | This number, found in the response as `RefNum`, is obtained from the response object of an auth transaction. |
| Amount | Amount to capture. Cannot exceed auth amount. If you need to exceed the auth amount, perform another sale and the auth will fall off. Format is $$$$$$$CC |
| DeviceSerialNo | Serial number of your PAX terminal |
```csharp
try {
    var result = await payarc.PayarcConnect.PostAuth(
        ecrRefNum: "REF123",
        origRefNum: "123",
        amount: "100",
        deviceSerialNo: "12345"
    );
    Console.WriteLine("Result: " + JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true }));
} catch (Exception ex) {
    Console.WriteLine("Error detected: " + ex.Message);
}
```
### Last Transaction
Returns the response object from the last transaction

```csharp
try {
    var result = await payarc.PayarcConnect.LastTransaction(
        deviceSerialNo: "12345"
    );
    Console.WriteLine("Result: " + JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true }));
} catch (Exception ex) {
    Console.WriteLine("Error detected: " + ex.Message);
}
```
### Server Info
 Returns the status of the server

```csharp
try {
    var result = await payarc.PayarcConnect.ServerInfo();
    Console.WriteLine("Result: " + JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true }));
} catch (Exception ex) {
    Console.WriteLine("Error detected: " + ex.Message);
}
```

### Terminals
Returns a list of registered terminal for merchant

```csharp
try {
    var result = await payarc.PayarcConnect.Terminals();
    Console.WriteLine("Result: " + JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true }));
} catch (Exception ex) {
    Console.WriteLine("Error detected: " + ex.Message);
}
```

## License [MIT](./LICENSE)
