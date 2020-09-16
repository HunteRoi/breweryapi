[![Stargazers][stars-shield]][stars-url]
[![Issues][issues-shield]][issues-url]
[![MIT License][license-shield]][license-url]

<br />
<p align="center">
  <a href="https://github.com/hunteroi/breweryapi">
    <img src="docs/logo.png" alt="Logo" width="80" height="80">
  </a>

  <h3 align="center">Brewery API</h3>

  <p align="center">
    An API to get beers, breweries and related wholesalers.
    <br />
    <a href="https://github.com/hunteroi/breweryapi/issues">Report Bug</a>
    ·
    <a href="https://github.com/hunteroi/breweryapi/issues">Request Feature</a>
  </p>
</p>



<!-- TABLE OF CONTENTS -->
## Table of Contents

- [Table of Contents](#table-of-contents)
- [About The Project](#about-the-project)
  - [Built With](#built-with)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
- [Roadmap](#roadmap)
- [License](#license)
- [Contact](#contact)



<!-- ABOUT THE PROJECT -->
## About The Project

The codebase of this project has been taken from my own template located [here](https://github.com/hunteroi/aspnetcore3.1-backend-boilerplate). A few features were removed from that template in order to lower the complexity of the project and to ease of the development of **Brewery API**.

### Built With

* .NET Core 3.1
* SQL Server 2019
* NUnit 3



<!-- GETTING STARTED -->
## Getting Started

To get a local copy up and running follow these simple steps.

### Prerequisites

You need to have [.NET Core 3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1) installed and running. Type the following command to get more information on the installed version of the framework:
```sh
dotnet help
```
You also need to create an environment variable to store the connection string to your SQL Server database. This variable should be named `boilerplate_ConnectionStrings__DbContext`.

### Installation
 
1. Clone the repo
```sh
git clone https://github.com/hunteroi/breweryapi.git
```
2. Install NuGet packages
```sh
dotnet restore
```
3. Build the project
```sh
dotnet build
```
4. Run it
```sh
dotnet run
```



<!-- ROADMAP -->
## Roadmap

See the [open issues](https://github.com/hunteroi/breweryapi/issues) for a list of proposed features (and known issues).



<!-- LICENSE -->
## License

Distributed under the MIT License. See `LICENSE` for more information.



<!-- CONTACT -->
## Contact

Tinaël Devresse - [@tinaeldvs](https://twitter.com/tinaeldvs) - me@tinaeldevresse.eu

Project Link: [https://github.com/hunteroi/breweryapi](https://github.com/hunteroi/breweryapi)


<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[stars-shield]: https://img.shields.io/github/stars/hunteroi/breweryapi.svg?style=flat-square
[stars-url]: https://github.com/hunteroi/breweryapi/stargazers
[issues-shield]: https://img.shields.io/github/issues/hunteroi/breweryapi.svg?style=flat-square
[issues-url]: https://github.com/hunteroi/breweryapi/issues
[license-shield]: https://img.shields.io/github/license/hunteroi/breweryapi.svg?style=flat-square
[license-url]: https://github.com/hunteroi/breweryapi/blob/master/LICENSE.md
