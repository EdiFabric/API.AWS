# EdiNation InHouse API for AWS

## 1. Overview
EdiNation InHouse EDI API allows you to run EdiNation's EDI translation and validation API in your own cloud or on-prem environment(s).  
This tutorial uses the example AWS Lambda that comes with the EdiNation Inhouse product to demonstrate how to publish EdiNation Inhouse API as an AWS Lambda and API Gateway in your own AWS account(s).

## 2. Requirements
- [EdiNation Inhouse subscription](https://buy.stripe.com/bIY9BK615epH2rufZ4) - this contains the in-house EDI API installation files EdiFabric.dll and EdiFabric.Api.dll, and an example Visual Studio 2022 solution that implements an Azure Function.
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/), which supports .NET 8.0 (or .NET 6.0). 
- If you don't have an [Azure subscription](https://docs.microsoft.com/en-us/azure/guides/developer/azure-developer-guide#understanding-accounts-subscriptions-and-billing), create an [Azure free account](https://azure.microsoft.com/free/?ref=microsoft.com&utm_source=microsoft.com&utm_medium=docs&utm_campaign=visualstudio) before you begin.
- [Download Postman](https://www.postman.com/downloads/) - it's an application to consume/test your API.
- EDI test file(s) - the API supports X12, EDIFACT, EANCOM, HL7, NCPDP, VDA, and EDIGAS. If you don't have a test file, use one of ours - [X12 HIPAA](https://support.edifabric.com/hc/en-us/sections/360001487352-X12-HIPAA-Files-Templates), [X12](https://support.edifabric.com/hc/en-us/sections/360005274077-X12-Files-Templates), [EDIFACT](https://support.edifabric.com/hc/en-us/sections/360005274137-EDIFACT-Files-Templates).

## 3. EdiNation Inhouse account and installation files
[Sign up for EdiNation Inhouse](https://buy.stripe.com/bIY9BK615epH2rufZ4) with a professional email address to receive the in-house EDI API installation files EdiFabric.dll and EdiFabric.Api.dll.

> NOTE: You won't be able to use this edition of EdiFabric on its own or as a substitute for a product you previously obtained from edifabric.com.

## 4. Setup
Rebuild the solution. If there are any build errors, contact us at https://support.edifabric.com/hc/en-us/requests/new for assistance.

## 5. Getting started
To get started, follow the steps in the [How to use InHouse EDI API as AWS Lambda](https://support.edifabric.com/hc/en-us/articles/7726206553361-How-to-use-EDI-API-as-AWS-Lambda) article.  

## 6. Warranty
*The source code in these example projects is strictly for demonstrational purposes and is provided "AS IS" without warranty of any kind, whether expressed or implied, including but not limited to the implied warranties of merchantability and/or fitness for a particular purpose.*

## 7. Additional information

[What is Inhouse EDI API](https://support.edifabric.com/hc/en-us/articles/7745692421521-What-is-Inhouse-EDI-API-)

[How to use EDI API as Azure Function](https://support.edifabric.com/hc/en-us/articles/7726202434449-How-to-use-Inhouse-EDI-API-as-Azure-Function)

[How to use EDI API as ASP.NET Core](https://support.edifabric.com/hc/en-us/articles/9586899018013-How-to-use-EDI-API-as-ASP-NET-Core)

[Support](https://support.edifabric.com/hc/en-us/requests/new)
### 2025 © EdiFabric
