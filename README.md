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
    var charge = payarc.Charges.Create(new ChargeCreateOptions {
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
    var charge = payarc.Charges.Create(new ChargeCreateOptions {
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
    var charge = payarc.Charges.Create(new ChargeCreateOptions {
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
    var charge = payarc.Charges.Create(new ChargeCreateOptions {
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
    var charge = payarc.Charges.Create(new ChargeCreateOptions {
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
    var charge = payarc.Charges.Retrieve("ch_1J*****3");
    Console.WriteLine($"Charge retrieved {JsonSerializer.Serialize(charge)}");
} catch (Exception ex) {
    Console.WriteLine($"Error detected: {ex.Message}");
}
```

### Example: Retrieve an ACH Charge

#### This example shows how to retrieve a specific ACH charge by its ID:
```csharp
try {
    var charge = payarc.Charges.Retrieve("ach_1J*****3");
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
    var charges = payarc.Charges.List(new BaseListOptions {
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

    var charge = payarc.Charges.CreateRefund(id, options);
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

try {
    var customer = payarc.Customers.Create(customerData);
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
    var customer = payarc.Customers.Retrieve("cus_j*******p");
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
    var customers = payarc.Customers.List(new OptionsData {
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

    var customer = payarc.Customers.Update(id, new CustomerInfoData {
        Name = "Bai Doncho 3",
        Description = "Example customer",
        Phone = 1234567890
    });
    Console.WriteLine($"Customer updated: {JsonConvert.SerializeObject(customer)}");
} catch (Exception e) {
    Console.WriteLine($"Error detected: {e.Message}");
}
```

### Example: Update an Already Found Customer

#### This example shows how to update a customer object:
```csharp
try {
    $customer = $payarc->customers->retrieve('cus_j*******p');
    $customer = $customer['update']([
        "name" => "Bai Doncho 4",
        "description" => "Senior Example customer",
        "phone" => "1234567895"
    ]);
    echo "Customer updated: " . json_encode($customer) . "\n";
} catch (Throwable $e) {
    echo "Error detected: " . $e->getMessage() . "\n";
}
```

### Example: Add a New Card to a Customer

This example shows how to add a new card to an existing customer:
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

    var customer = payarc.Customers.Update("cus_j*******p", customerData);
    Console.WriteLine($"Card added: {JsonConvert.SerializeObject(customer)}");
} catch (Exception e) {
    Console.WriteLine($"Error detected: {e.Message}");
}
```

### Example: Add a New Bank Account to a Customer

This example shows how to add new bank account to a customer. See full list of bank account attributes in API documentation.
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

    var customer = payarc.Customers.Update("cus_j*******p", customerData);
    Console.WriteLine($"Bank account added: {JsonConvert.SerializeObject(customer)}");
} catch (Exception e) {
    Console.WriteLine($"Error detected: {e.Message}");
}
```

## Deleting a Customer

### Example: Delete Customer

This example shows how to delete customer. See more details in API documentation.
```csharp
  try {
    var customer = payarc.Customers.Delete("cus_j*******p");                        
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
