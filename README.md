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
The Payrc SDK is build around the `Payarc` object. From this object you can access properties and function that will support your operations.

### The `Payarc` object has the following services:
- **Charges** - Manage payments
- **Customers** - Manage customers  
- **Applications** - Manage candidate merchants  
- **SplitCampaigns** - Manage split campaigns  
- **Billing**  
    - **Plan** - Manage plans  
    - **Subscription** - Manage plan subscriptions

> [!NOTE]
> The `Payarc` object referenced in the below examples will be created as `payarc`, you can name it whatever you'd like though.

### Service `payarc.Charges`
#### The `payarc.Charges` service facilitates the management of payments within the system, providing the following functions:
- **Create** - Initiates a payment intent or charge with various configurable parameters. Refer to the examples for specific use cases.
- **Retrieve** - Fetches a JSON object `charge` containing detailed information about a specific charge.
- **List** - returns an object containing two attributes:  `charges` and `pagination`. The `charges` attribute is a list of JSON objects, each providing detailed information about individual charges. The `pagination` attribute contains deatails for navigating through the list of charges.
- **CreateRefund** - Processes a refund for an existing charge.

### Service `payarc.Customer`
#### The `payarc.Customer` service manages your customers' personal information, including addresses and payment methods such as credit cards and bank accounts. This data is securely stored for future transactions.
- **Create** - Creates a customer object in the database and generates a unique identifier for future reference and inquiries. See examples and documentation for more details.
- **Retrieve** - Retrieves detailed information about a specific customer from the database.
- **List** - Searches through previously created customers, allowing filtering based on specific criteria. See examples and documentation for more details.
- **Update** - Modifies attributes of an existing customer object.
- **Delete** - Removes a customer object from the database.

### Service `payarc.Applications`
##### The `payarc.Applications` service is designed for Agents and ISVs to manage candidate merchants during new customer acquisition. It allows you to create, retrieve, list, and manage the necessary documents for the onboarding process.
- **Create** - Adds a new candidate merchant to the database. See the documentation for available attributes, valid values, and required fields.
**List** - Returns a list of application objects representing potential merchants. Use this function to find the relevant identifier.
**Retrieve** - based on identifier or an object returned from list function, this function will return details 
**Delete** - in case candidate merchant is no longer needed it will remove information for it.
**AddDocument** - this function is adding base64 encoded document to existing candidate merchant. For different types of document required in the process contact Payarc. See examples how the function could be invoked
**DeleteDocument** - this function removes document, when document is no longer valid.
**ListSubAgents** - this function is usefull to create candidate in behalf of other agent.
**Submit** - this function initialize the process of sing off contract between Payarc and your client

### Service `payarc.Billing`
The payarc.Billing service is responsible for managing recurring payments. It currently includes the `Plan` service for handling plans and the `Subscription` service for managing plan subscriptions.

#### Service `payarc.Billing.Plan` 
#### This Service contains information specific for each plan like identification details, rules for payment request and additional information. This SERVICE has methods for:
    create - you can programmatically created new objects to meet client's needs,
    list - inquiry available plans,
    retrieve - collect detailed information for a plan,
    update - modify details of a plan,
    delete - remove plan when no longer needed,
    create_subscription: issue a subscription for a customer from a plan.
Based on plans you can create subscription. Time scheduled job will request and collect payments (charges) according plan schedule from customer.

#### Service `payarc.Billing.Subscription`
This abstraction encapsulate information for subscription the link between customer and plan. it has following methods:
    list - enumerate subscriptions,
    cancel - stop and cancel active subscription,
    update - modify details of a subscription

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
