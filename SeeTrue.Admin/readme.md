<div id="top"></div>

<!-- PROJECT LOGO -->
<br />
<div align="center">
  <a href="https://github.com/TheOnlyBeardedBeast/SeeTrue">
    <img width="256px" src="https://github.com/TheOnlyBeardedBeast/SeeTrue/blob/master/SeeTrue.Admin/src/Assets/SeeTrueIcon.png?raw=true" />
  </a>

  <h3 align="center">SeeTrue.Admin</h3>

  <p align="center">
    A UI web application to manager your SeeTrue.API instance users and email templates.
    <br />
    <a href="https://github.com/TheOnlyBeardedBeast/SeeTrue/tree/master/SeeTrue.API"><strong>Explore the SeeTrue.API</strong></a>
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
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#prerequisites">Prerequisites</a></li>
        <li><a href="#usage">Usage</a>
            <ul>
                <li><a href="#environment-configuration">Environment configuration</a></li>
            </ul>
        </li>
      </ul>
    </li>
    <li><a href="#screenshots">Screenshots</a></li>
    <li><a href="#roadmap">Roadmap</a></li>
    <li><a href="#contributing">Contributing</a></li>
    <li><a href="#license">License</a></li>
    <li><a href="#contact">Contact</a></li>
    <li><a href="#acknowledgments">Acknowledgments</a></li>
  </ol>
</details>



<!-- ABOUT THE PROJECT -->
## About The Project

SeeTrue.Admin is a UI application which conects to you [SeeTrue.API](https://github.com/TheOnlyBeardedBeast/SeeTrue/tree/master/SeeTrue.API) instance. It provides a nice way to manage your users and also gives you a nice email template editor supporting MJML responsive email templates with mustache templating engine.

<p align="right">(<a href="#top">back to top</a>)</p>



### Built With

Built with hard work, tears, sweat, dedication and love.

* [React.js](https://reactjs.org/)
* [Base web](https://baseweb.design/)
* [TypeScript](https://www.typescriptlang.org/)
* [Wouter](https://github.com/molefrog/wouter)
* [React Hook Form](https://react-hook-form.com/)
* [Docker](https://www.docker.com/)
* [MJML](https://mjml.io/)

<p align="right">(<a href="#top">back to top</a>)</p>


## Getting Started

Please note, this is just a management UI for [SeeTrue.API](https://github.com/TheOnlyBeardedBeast/SeeTrue/tree/master/SeeTrue.API) 

### Prerequisites

* Docker

Optionally you can use and tweak the [`docker-compose`](https://github.com/TheOnlyBeardedBeast/SeeTrue/blob/master/compose.yml) file

### Usage

```shell
docker run theonlybeardedbeast/seetrue.admin -p 9998:9998 -d
```

Visit your host on the port `9998` and login in in the UI

## Screenshots

![Login to SeeTrue.Admin](https://github.com/TheOnlyBeardedBeast/SeeTrue/blob/master/Screenshots/AdminLogin.png?raw=true)

![Users](https://raw.githubusercontent.com/TheOnlyBeardedBeast/SeeTrue/master/Screenshots/AdminUsers.png?raw=true)

![User Detail | Edit](https://raw.githubusercontent.com/TheOnlyBeardedBeast/SeeTrue/master/Screenshots/AdminUsersDetail.png?raw=true)

![Emails](https://raw.githubusercontent.com/TheOnlyBeardedBeast/SeeTrue/master/Screenshots/AdminEmails.png?raw=true)

![Email settings](https://raw.githubusercontent.com/TheOnlyBeardedBeast/SeeTrue/master/Screenshots/AdminEmailSettings.png?raw=true)

![Email render](https://raw.githubusercontent.com/TheOnlyBeardedBeast/SeeTrue/master/Screenshots/AdminEmailRender.png?raw=true)

![Email html](https://raw.githubusercontent.com/TheOnlyBeardedBeast/SeeTrue/master/Screenshots/AdminEmailHtml.png?raw=true)

<!-- ROADMAP -->
## Roadmap

- [ ] Implement webhooks
- [ ] Action logs
- [ ] SQLite support
- [ ] Email template provider API
- [ ] Non SMTP email sender

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



