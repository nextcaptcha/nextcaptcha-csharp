# NextCaptcha SDK for C#

NextCaptcha is a powerful captcha solving service that helps developers easily integrate captcha solving capabilities
into their C# projects. With NextCaptcha, you can solve a wide range of captcha types, including reCAPTCHA v2, reCAPTCHA
v3, reCAPTCHA Enterprise,reCAPTCHA Mobile, hCaptcha, hCaptcha Enterprise, and FunCaptcha. Our SDK provides a simple and
intuitive API that allows you to seamlessly integrate captcha solving into your applications.

## Features

Support for multiple captcha types:

* reCAPTCHA v2
* reCAPTCHA v2 Enterprise
* reCAPTCHA v3
* reCAPTCHA Mobile
* hCaptcha
* hCaptcha Enterprise
* FunCaptcha
* Easy integration with C# projects
* Asynchronous API calls for non-blocking execution
* Configurable timeouts and logging options
* Balance retrieval for monitoring API usage

## Installation

To use the NextCaptcha SDK in your C# project, you can either download the source code directly or install it via NuGet.

## NuGet Installation

```bash
Install-Package NextCaptchaSDK
```

## Manual Installation

1. Download the source code from the repository.
2. Add the NextCaptchaAPI.cs file to your project.
3. Make sure you have the required dependencies installed:
    * System.Net.Http
    * Newtonsoft.Json

## Usage

To start using the NextCaptcha SDK, create an instance of the NextCaptchaAPI class with your client key and optional
parameters:

```csharp
string clientKey = "YOUR_CLIENT_KEY";
string solftId = ""; // Optional
string callbackUrl = ""; // Optional
bool openLog = true; // Optional

var nextCaptchaAPI = new NextCaptchaAPI(clientKey, solftId, callbackUrl, openLog);
```

## Solving Captchas

The SDK provides methods for solving different types of captchas. Here are examples for each captcha type:

### Solving reCAPTCHA v2

```csharp
string websiteUrl = "https://example.com";
string websiteKey = "YOUR_WEBSITE_KEY";
string recaptchaDataSValue = ""; // Optional
bool isInvisible = false; // Optional
string apiDomain = ""; // Optional

var result = await nextCaptchaAPI.SolveRecaptchaV2Async(websiteUrl, websiteKey, recaptchaDataSValue, isInvisible, apiDomain);
```

### Solving reCAPTCHA v2 Enterprise

```csharp
string websiteUrl = "https://example.com";
string websiteKey = "YOUR_WEBSITE_KEY";
Dictionary<string, object> enterprisePayload = null; // Optional
bool isInvisible = false; // Optional
string apiDomain = ""; // Optional
var result = await nextCaptchaAPI.SolveRecaptchaV2EnterpriseAsync(websiteUrl, websiteKey, enterprisePayload, isInvisible, apiDomain);
```

### Solving reCAPTCHA v3

```csharp
string websiteUrl = "https://example.com";
string websiteKey = "YOUR_WEBSITE_KEY";
string pageAction = ""; // Optional
string apiDomain = ""; // Optional
string proxyType = ""; // Optional
string proxyAddress = ""; // Optional
int proxyPort = 0; // Optional
string proxyLogin = ""; // Optional
string proxyPassword = ""; // Optional

var result = await nextCaptchaAPI.SolveRecaptchaV3Async(websiteUrl, websiteKey, pageAction, apiDomain, proxyType, proxyAddress, proxyPort, proxyLogin, proxyPassword);
```

### Solving reCAPTCHA Mobile

```csharp
string appKey = "YOUR_APP_KEY";
string appPackageName = ""; // Optional
string appAction = ""; // Optional

var result = await nextCaptchaAPI.SolveRecaptchaMobileAsync(appKey, appPackageName, appAction);
```

### Solving hCaptcha

```csharp
string websiteUrl = "https://example.com";
string websiteKey = "YOUR_WEBSITE_KEY";
bool isInvisible = false; // Optional
Dictionary<string, object> enterprisePayload = null; // Optional
string proxyType = ""; // Optional
string proxyAddress = ""; // Optional
int proxyPort = 0; // Optional
string proxyLogin = ""; // Optional
string proxyPassword = ""; // Optional

var result = await nextCaptchaAPI.SolveHCaptchaAsync(websiteUrl, websiteKey, isInvisible, enterprisePayload, proxyType, proxyAddress, proxyPort, proxyLogin, proxyPassword);
```

### Solving hCaptcha Enterprise

```csharp
string websiteUrl = "https://example.com";
string websiteKey = "YOUR_WEBSITE_KEY";
Dictionary<string, object> enterprisePayload = null; // Optional
bool isInvisible = false; // Optional
string proxyType = ""; // Optional
string proxyAddress = ""; // Optional
int proxyPort = 0; // Optional
string proxyLogin = ""; // Optional
string proxyPassword = ""; // Optional

var result = await nextCaptchaAPI.SolveHCaptchaEnterpriseAsync(websiteUrl, websiteKey, enterprisePayload, isInvisible, proxyType, proxyAddress, proxyPort, proxyLogin, proxyPassword);
```

### Solving FunCaptcha

```csharp
string websitePublicKey = "YOUR_WEBSITE_PUBLIC_KEY";
string websiteUrl = ""; // Optional
string data = ""; // Optional
string proxyType = ""; // Optional
string proxyAddress = ""; // Optional
int proxyPort = 0; // Optional
string proxyLogin = ""; // Optional
string proxyPassword = ""; // Optional

var result = await nextCaptchaAPI.SolveFunCaptchaAsync(websitePublicKey, websiteUrl, data, proxyType, proxyAddress, proxyPort, proxyLogin, proxyPassword);
```

## Retrieving Balance

You can retrieve your account balance using the GetBalanceAsync method:

```csharp
string balance = await nextCaptchaAPI.GetBalanceAsync();
Console.WriteLine($"Account balance: {balance}");
```

## Error Handling

The SDK returns a dictionary containing the result or error details. You can check the status key to determine if the
task was successful or encountered an error. If the status is "failed", you can retrieve the error details from the
errorId and errorDescription keys.

```csharp
var result = await nextCaptchaAPI.SolveRecaptchaV2Async(websiteUrl, websiteKey);

if (result["status"] == "failed")
{
string errorId = result["errorId"];
string errorDescription = result["errorDescription"];
Console.WriteLine($"Error: {errorId} - {errorDescription}");
}
else
{
string solution = result["solution"];
Console.WriteLine($"Captcha solved: {solution}");
}
```

## Logging

By default, the SDK logs various events and errors to the console. You can disable logging by setting the openLog
parameter to false when creating an instance of the NextCaptchaAPI class.

```csharp
bool openLog = false;
var nextCaptchaAPI = new NextCaptchaAPI(clientKey, solftId, callbackUrl, openLog);
```

## Timeout

The SDK has a default timeout of 45 seconds for waiting for the captcha to be solved. If the timeout is exceeded, the
SDK will return an error with the errorId set to "12" and errorDescription set to "Timeout".

## Dependencies

The NextCaptcha SDK has the following dependencies:

* System.Net.Http for making HTTP requests to the NextCaptcha API.
* Newtonsoft.Json for JSON serialization and deserialization.
  Make sure you have these dependencies installed in your project before using the SDK.

## Support

If you encounter any issues or have questions regarding the NextCaptcha SDK, please contact our support team at
support@nextcaptcha.com or visit our website at https://nextcaptcha.com for more information and documentation.

## Why Choose NextCaptcha?

NextCaptcha offers several advantages over other captcha solving services:

1. **High Success Rate**: Our advanced algorithms and machine learning models enable us to achieve a high success rate
   in solving captchas. This means you can rely on NextCaptcha to accurately solve captchas and improve the efficiency
   of your automated processes.

2. **Fast Response Times**: We understand the importance of speed in captcha solving. NextCaptcha delivers fast response
   times, ensuring that you can quickly retrieve the solved captchas and proceed with your tasks without significant
   delays.

3. **Easy Integration**: Our SDK is designed with simplicity in mind. With just a few lines of code, you can integrate
   NextCaptcha into your C# projects and start solving captchas seamlessly. The SDK provides intuitive methods and clear
   documentation to make the integration process smooth and hassle-free.

4. **Flexible Pricing**: We offer flexible pricing plans to cater to the diverse needs of our customers. Whether you
   have a small-scale project or a large-scale enterprise application, NextCaptcha provides pricing options that suit
   your requirements and budget.

5. **Excellent Customer Support**: Our dedicated customer support team is always ready to assist you with any questions,
   concerns, or technical issues you may encounter. We value our customers and strive to provide prompt and helpful
   support to ensure a positive experience with NextCaptcha.

## Getting Started

To get started with NextCaptcha, follow these steps:

1. Sign up for a NextCaptcha account at https://nextcaptcha.com and obtain your client key.

2. Install the NextCaptcha SDK in your C# project using NuGet or by manually adding the source code.

3. Create an instance of the NextCaptchaAPI class with your client key and optional parameters.

4. Use the appropriate methods provided by the SDK to solve the desired captcha type.

5. Handle the captcha solving result and integrate it into your application logic.

6. Monitor your account balance and usage using the GetBalanceAsync method.

For detailed code examples and further assistance, please refer to the SDK documentation and our website.

## Conclusion

NextCaptcha provides a powerful and easy-to-use captcha solving service for C# developers. With our SDK, you can
efficiently solve various types of captchas and enhance the functionality of your applications. Whether you need to
automate form submissions, scrape websites, or perform other tasks that require captcha solving, NextCaptcha has got you
covered.

Start leveraging the capabilities of NextCaptcha today and take your C# projects to the next level!

Happy captcha solving with NextCaptcha!