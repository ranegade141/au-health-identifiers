<div align="center">

<img src="https://raw.githubusercontent.com/pgodwin/au-health-identifiers/master/package.png" alt="AuHealthIdentifiers" width="256"/><br />
# Australian Health Identifiers


### A C# library for validating ✅ and generating 📃 Australian Health Identifiers 🪪

[![build](https://github.com/pgodwin/au-health-identifiers/actions/workflows/build.yml/badge.svg)](https://github.com/pgodwin/au-health-identifiers/actions/workflows/build.yml)
[![NuGet Version](https://img.shields.io/nuget/v/AuHealthIdentifiers.svg?style=flat)](https://www.nuget.org/packages/AuHealthIdentifiers/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

</div>

## Features
This project provides C# validators and generators for Australian Health Identifiers, including:

 - [Individual Healthcare Identifier (IHI)](https://www.servicesaustralia.gov.au/individual-healthcare-identifiers)
 - [Healthcare Provider Identifier - Individual (HPI-I)](https://www.servicesaustralia.gov.au/healthcare-identifiers-service-for-health-professionals)
 - [Healthcare Provider Identifier - Organisation (HPI-O)](https://www.servicesaustralia.gov.au/how-to-apply-for-hi-service-for-organisations)
 - [AHPRA Number](https://www.ahpra.gov.au/Registration/Registers-of-Practitioners.aspx)
 - [Medicare Card Number](https://www.servicesaustralia.gov.au/medicare-card)
 - [Veterans' Affairs (DVA) Number](https://www.dva.gov.au)
 - [Provider Number](https://www.servicesaustralia.gov.au/provider-and-prescriber-numbers?context=20)
 - [Prescriber Number](https://www.servicesaustralia.gov.au/provider-and-prescriber-numbers?context=20)

## Installation
You can install the package via NuGet Package Manager with the following command:
```bash
Install-Package AuHealthIdentifiers
```
Or via the .NET CLI:
```bash
dotnet add package AuHealthIdentifiers
```

## Usage
Here's a quick example of how to use the library to validate and generate an Individual Healthcare Identifier (IHI):
```csharp
using AuHealthIds;

IndividualHealthIdentifier id = new IndividualHealthIdentifier();
var isValid = id.IsValid("800360123456789"); // returns true if valid
var newId = id.Generate(); // generates a new conformant IHI
```
Similar classes and methods are available for other identifier types.

The library also includes a static class `IdentifierTools` for simple validation:
```csharp
using AuHealthIds;

IdentifierTools.ValidateId(IdentiferType.IHI, "800360123456789"); // returns true if valid

```

and extension methods for strings:
```csharp
using AuHealthIds;

var isValid = "800360123456789".IsValidIhi(); // returns true if valid
```

## Targets
The library targets .NET Standard 2.0, making it compatible with a wide range of .NET implementations including .NET Core and .NET Framework..

## Contributing
Contributions are welcome! Please fork the repository and submit a pull request with your changes. Please include unit tests for any new validators/fixes.

For major changes, please open an issue first to discuss what you would like to change.


## Credits
 - Details of the algorithms used in this project are based on the descriptions by [Jamie Curmi](https://curmi.com/australian-health-identifiers/). 
   These descriptions made understanding how these identifiers are structured and validated a cinch!

 - Nuget Icon - <a href="https://www.flaticon.com/free-icons/icard" title="icard icons">Icard icons created by HAJICON - Flaticon</a>

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

The nuget icon is licensed under the free [Flaticon License](https://www.flaticon.com/legal) for personal and commercial purpose with attribution.
