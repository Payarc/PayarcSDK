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
2. Add the following rows and update wiht your environment-specific values.

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
- Documentation for existing payment API provided by Payarc can be found on https://docs.payarc.net/
- Documentation for existing candidate merchant management API can be found on https://docs.apply.payarc.net/

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
