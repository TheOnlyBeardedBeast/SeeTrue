<div id="top"></div>

<!-- PROJECT SHIELDS -->
<!--
*** I'm using markdown "reference style" links for readability.
*** Reference links are enclosed in brackets [ ] instead of parentheses ( ).
*** See the bottom of this document for the declaration of the reference variables
*** for contributors-url, forks-url, etc. This is an optional, concise syntax you may use.
*** https://www.markdownguide.org/basic-syntax/#reference-style-links
-->
[![Contributors][contributors-shield]][contributors-url]
[![Forks][forks-shield]][forks-url]
[![Stargazers][stars-shield]][stars-url]
[![Issues][issues-shield]][issues-url]
[![MIT License][license-shield]][license-url]



<!-- PROJECT LOGO -->
<br />
<div align="center">
  <a href="https://github.com/TheOnlyBeardedBeast/SeeTrue">
    <img width="256px" src="./SeeTrue.Admin/src/Assets/SeeTrueIcon.png" />
  </a>

  <h3 align="center">SeeTrue</h3>

  <p align="center">
    An authentication service to fulfill your needs.
    <br />
    <a href="https://github.com/TheOnlyBeardedBeast/SeeTrue"><strong>Explore the docs »</strong></a>
    <br />
    <br />
    <a href="https://github.com/TheOnlyBeardedBeast/SeeTrue/issues">Report Bug</a>
    ·
    <a href="https://github.com/TheOnlyBeardedBeast/SeeTrue/issues">Request Feature</a>
  </p>
</div>



<!-- TABLE OF CONTENTS -->
<details>
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
      <ul>
        <li><a href="#built-with">Built With</a></li>
      </ul>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#prerequisites">Prerequisites</a></li>
        <li><a href="#installation">Installation</a></li>
      </ul>
    </li>
    <li><a href="#usage">Usage</a></li>
    <li><a href="#roadmap">Roadmap</a></li>
    <li><a href="#contributing">Contributing</a></li>
    <li><a href="#license">License</a></li>
    <li><a href="#contact">Contact</a></li>
    <li><a href="#acknowledgments">Acknowledgments</a></li>
  </ol>
</details>



<!-- ABOUT THE PROJECT -->
## About The Project

SeeTrue is an authentication service, which handless user authetication, token management, mailing for you.

SeeTrue is composed by the following components:
* [SeeTrue.API](https://github.com/TheOnlyBeardedBeast/SeeTrue/tree/master/SeeTrue.API) - a fast API written in dotnet core 6, fully dockerized and customizable through environment variables.
* [SeeTrue.Admin](https://github.com/TheOnlyBeardedBeast/SeeTrue/tree/master/SeeTrue.Admin) - a standalone web app to connect to your SeeTrue.API instance to manage your users and email templates.
* [SeeTrue.Client](https://github.com/TheOnlyBeardedBeast/SeeTrue/tree/master/SeeTrue.Client) a node | browser javascript client to interact with your SeeTrue.API instance.
* [SeeTrue.Client.React](https://github.com/TheOnlyBeardedBeast/SeeTrue/tree/master/SeeTrue.Client.React) - A react package which wraps SeeTrue.Client int a React context, and it provides a hook to acces the client, check authentication status, user information, tokens.

SeeTrue started as [GoTrue](https://github.com/netlify/gotrue) reimplementation, but the project quickly shifted into a different direction. Please check out [GoTrue](https://github.com/netlify/gotrue) and support them with a star :smile: 

<p align="right">(<a href="#top">back to top</a>)</p>



### Built With

Built with hard work, tears, sweat, dedication and love.

* [.Net 6.0](https://dotnet.microsoft.com/en-us/)
* [MediatR](https://github.com/jbogard/MediatR)
* [React.js](https://reactjs.org/)
* [Base web](https://baseweb.design/)
* [TypeScript](https://www.typescriptlang.org/)
* [Wouter](https://github.com/molefrog/wouter)
* [React Hook Form](https://react-hook-form.com/)
* [Docker](https://www.docker.com/)
* [MJML](https://mjml.io/)

<p align="right">(<a href="#top">back to top</a>)</p>



<!-- GETTING STARTED -->
## Getting Started

TODO: Add description

### Prerequisites

TODO: Add prerequisites description

### Installation

TODO: Add Instalation description

<p align="right">(<a href="#top">back to top</a>)</p>



<!-- USAGE EXAMPLES -->
## Usage

TODO: Add usage description

<p align="right">(<a href="#top">back to top</a>)</p>



<!-- ROADMAP -->
## Roadmap

- [ ] Publish docker images
- [ ] Publish npm packages
- [ ] Implement webhooks
- [ ] SQLite support

See the [open issues](https://github.com/TheOnlyBeardedBeast/SeeTrue/issues) for a full list of proposed features (and known issues).

<p align="right">(<a href="#top">back to top</a>)</p>



<!-- CONTRIBUTING -->
## Contributing

Contributions are what make the open source community such an amazing place to learn, inspire, and create. Any contributions you make are **greatly appreciated**.

If you have a suggestion that would make this better, please fork the repo and create a pull request. You can also simply open an issue with the tag "enhancement".
Don't forget to give the project a star! Thanks again!

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

<p align="right">(<a href="#top">back to top</a>)</p>



<!-- LICENSE -->
## License

Distributed under the MIT License. See `LICENSE` for more information.

<p align="right">(<a href="#top">back to top</a>)</p>



<!-- CONTACT -->
## Contact

Project Link: [https://github.com/TheOnlyBeardedBeast/SeeTrue](https://github.com/TheOnlyBeardedBeast/SeeTrue)

<p align="right">(<a href="#top">back to top</a>)</p>



<!-- ACKNOWLEDGMENTS -->
## Acknowledgments

* [GoTrue](https://github.com/netlify/gotrue)
* [Supabase.GoTrue](https://supabase.com/docs/gotrue/server/about)
* [JWT](https://jwt.io/)
* [Best-README-Template](https://github.com/othneildrew/Best-README-Template)

<p align="right">(<a href="#top">back to top</a>)</p>



<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[contributors-shield]: https://img.shields.io/github/contributors/TheOnlyBeardedBeast/SeeTrue.svg?style=for-the-badge
[contributors-url]: https://github.com/TheOnlyBeardedBeast/SeeTrue/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/TheOnlyBeardedBeast/SeeTrue.svg?style=for-the-badge
[forks-url]: https://github.com/TheOnlyBeardedBeast/SeeTrue/network/members
[stars-shield]: https://img.shields.io/github/stars/TheOnlyBeardedBeast/SeeTrue.svg?style=for-the-badge
[stars-url]: https://github.com/TheOnlyBeardedBeast/SeeTrue/stargazers
[issues-shield]: https://img.shields.io/github/issues/TheOnlyBeardedBeast/SeeTrue.svg?style=for-the-badge
[issues-url]: https://github.com/TheOnlyBeardedBeast/SeeTrue/issues
[license-shield]: https://img.shields.io/github/license/TheOnlyBeardedBeast/SeeTrue.svg?style=for-the-badge
[license-url]: https://github.com/TheOnlyBeardedBeast/SeeTrue/blob/master/LICENSE