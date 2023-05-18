# Network Utility

The Network Utility is a tool that can ping hosts or addresses, scan ports, look up registered domains, look up network statistics, and CSV exporting.

You can find the source code on [GitHub](https://github.com/joshuamainsmith/NetworkUtility).

## Getting Started

To get started with this, download the repository from [GitHub](https://github.com/joshuamainsmith/NetworkUtility).

A release version will be added later. For now, it can be run with Microsoft Visual Studio or with the release binary under root\NetworkUtility\NetworkUtility\bin\Release\net6.0.

A proper console menu is also in the works.

### Prerequisites

You will need [.NET 6](https://learn.microsoft.com/en-us/dotnet/core/install/windows?tabs=net70) for this program. 

Navigate to the link above and follow the instructions to download and install the .NET framework.

### Installing
Three packages will be installed to use this program.
1. [Spectre.Console](https://spectreconsole.net/) by Patrik Svensson, Phil Scott, Nils Andresen

Install the NuGet package
```
dotnet add package Spectre.Console
```
2. [WhoisClient.NET](https://www.nuget.org/packages/WhoisClient.NET) by J.Sakamoto, Keith J. Jones

Install the NuGet package
```
PM> Install-Package WhoisClient.NET
```
3. [CsvHelper](https://joshclose.github.io/CsvHelper/) by Josh Close

Install with Package Manager Console
```
PM> Install-Package CsvHelper
```
.NET CLI Console
```
dotnet add package CsvHelper
```

## Running the tests

To run the unit tests, you will need to download the xUnit framework from [their websit](https://xunit.net/) or from the NuGet store in Visual Studio.
Use Test Explorer to run the unit tests.

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details
