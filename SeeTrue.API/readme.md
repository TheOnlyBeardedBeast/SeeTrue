<div id="top"></div>

<!-- PROJECT LOGO -->
<br />
<div align="center">
  <a href="https://github.com/TheOnlyBeardedBeast/SeeTrue">
    <img width="256px" src="https://github.com/TheOnlyBeardedBeast/SeeTrue/blob/master/SeeTrue.Admin/src/Assets/SeeTrueIcon.png?raw=true" />
  </a>

  <h3 align="center">SeeTrue.API</h3>

  <p align="center">
    An authentication service to fulfill your needs.
    <br />
    <a href="https://github.com/TheOnlyBeardedBeast/SeeTrue/tree/master/SeeTrue.API/docs#seetrueapi"><strong>Explore the docs »</strong></a>
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
    <li><a href="#roadmap">Roadmap</a></li>
    <li><a href="#contributing">Contributing</a></li>
    <li><a href="#license">License</a></li>
    <li><a href="#contact">Contact</a></li>
    <li><a href="#acknowledgments">Acknowledgments</a></li>
  </ol>
</details>



<!-- ABOUT THE PROJECT -->
## About The Project

SeeTrue is an authentication service, which handless user authetication, token management, mailing for you. [SeeTrue.API](https://github.com/TheOnlyBeardedBeast/SeeTrue/tree/master/SeeTrue.API) - a fast API written in dotnet core 6, fully dockerized and customizable through environment variables.

SeeTrue started as [GoTrue](https://github.com/netlify/gotrue) reimplementation, but the project quickly shifted into a different direction. Please check out [GoTrue](https://github.com/netlify/gotrue) and support them with a star :smile: 

<p align="right">(<a href="#top">back to top</a>)</p>



### Built With

Built with hard work, tears, sweat, dedication and love.

* [.Net 6.0](https://dotnet.microsoft.com/en-us/)
* [MediatR](https://github.com/jbogard/MediatR)
* [Docker](https://www.docker.com/)

<p align="right">(<a href="#top">back to top</a>)</p>


## Getting Started

If you would like to continue with the usage please continue reading, but if you are interested in the API please visit the API [docs](https://github.com/TheOnlyBeardedBeast/SeeTrue/tree/master/SeeTrue.API/docs)

### Prerequisites

* Docker
* SMTP access (optionally you can use gmail, or a local mailhog instance)
* Postgres database

Optionally you can use and tweak the [`docker-compose`](https://github.com/TheOnlyBeardedBeast/SeeTrue/blob/master/compose.yml) file

### Usage

```shell
docker run -e SEETRUE_SIGNING_KEY=... -e ...at least all the required env values... -p 9999:9999 theonlybeardedbeast/seetrue.api:latest -d
```

Connect to your SeeTrue.API instance directly through REST endpoints [docs](https://github.com/TheOnlyBeardedBeast/SeeTrue/tree/master/SeeTrue.API/docs#seetrueapi) or use [SeeTrue.Client](https://github.com/TheOnlyBeardedBeast/SeeTrue/tree/master/SeeTrue.Client) if you want to use it in browser or in node and if you are a React developer, you can our [SeeTrue.Client.React](https://github.com/TheOnlyBeardedBeast/SeeTrue/tree/master/SeeTrue.Client.React) which multi tab support, localstorage for refreshtokens, auto token refresh, user data access, React hooks... or manage your instance directly with [SeeTrue.Admin](https://github.com/TheOnlyBeardedBeast/SeeTrue/tree/master/SeeTrue.Admin)

#### Environment configuration

| Env variable | Type | Required | Default | Description |
|---|---|---|---|---|
| SEETRUE_SIGNING_KEY | string | true | - | Used for access_token encrytion |
| SEETRUE_TOKEN_LIFETIME | int | false | 3600 | `access_token` lifetime in seconds |
| SEETRUE_ISSUER | string | false | SeeTrue | Token issuer |
| SEETRUE_AUIDIENCES | string | true | - | Comma separated strings. Represence the supported token audiences. |
| SEETRUE_REFRESH_TOKEN_LIFETIME | number | false | 336 | Represents the `refresh_token` lifetime in **hours** |
| SEETRUE_SMTP_HOST | string | true | - | Host of the SMTP server |
| SEETRUE_SMTP_PORT | number | true | - | Port of the SMTP server |
| SEETRUE_SMTP_USER | string | true | - | SMTP username (in case of gmail,outlook use your email address)  |
| SEETRUE_SMTP_PASS | string | true | - | SMTP password |
| SEETRUE_VERIFICATION_TOKEN_LIFETIME | number | false | 24 | User signup confirmation token lifetime in hours |
| SEETRUE_VALIDATE_ISSUER | boolean | false | true | Enable or disable issuer validation on request  (`true`: enabled, `false`:disabled) |
| SEETRUE_VALIDATE_AUDIENCE | boolean | false | true | Enable or disable audience validation on request  (`true`: enabled, `false`:disabled) |
| SEETRUE_SIGNUP_DISABLED | boolean | false | false | Disable signup |
| SEETRUE_INSTANCE_ID | Guid | false | 00000000-0000-0000-0000-000000000000 | Use it when you want to use multiple SeeTrue.API instances on the same database handling different audiences. On loadbalancing the InstanceId should be same for all the instances in the loadbalancer. |
| SEETRUE_AUTOCONFIRM | boolean | false | false | If autoconfirm is `true`, the user will not get any signup confirmation email, and he will be able to login without any email confirmation. |
| SEETRUE_JWT_DEFAULT_GROUP_NAME | string | false | user | The default user role. |
| SEETRUE_MAGIC_LINK_LIFETIME | number | false | 5 | Lifetime of a one time login link. (Magiclinks wont work in a loadbalancer, unless you are using a sticky session, there are plans to implement a distributed cache, which will handle the issue) |
| SEETRUE_MINIMUM_PASSWORD_LENGTH | number | false | 8 | Minimum password length. If you want add any additionl valdiation please do it on your frontend. |
| SEETRUE_ADMIN_ROLE | string | false | - | If you dont add an admin role, no user will be able to use the admin API (user management, email template management). |
| SEETRUE_API_KEY | string | true | - | An API key to access the user and email template management from you app backend, or from SeeTrue.Admin application. |
| SEETRUE_DB_CONNECTION | string | true | - | Connection string for a postgress DB example `Host=postgress.local;Database=seetrue;Username=postgres;Password=postgres` |
| SEETRUE_LANGUAGES | string | false | en | Comma separated string containing supported languages (language is an option in the signup proccess, for each language you have to create an email template. |
| SEETRUE_ROLES | string | false | - | Roles, which are changeable in by an administrator, these roles dont need to contain the default user role and the admin role, if you dont need any other roles, you can leave this variable empty |
| SEETRUE_INVITE_ENABLED | string | false | false | Enables a user invite system. (If you want an invite only app you can disable signup and enable invites) |
| SEETRUE_NAME_KEY | string | false | Name | A key which specifies a the UserMetaData key which is required in the signup process, this key is used in the email templates to address the user. |
| SEETRUE_ALLOWED_HOSTS | string | true | - | Commaseparated list of hosts, required, you should list all the frontend applications which are using your SeeTrue.API instance so you dont get any CORS errors. |
| SEETRUE_MAILFROM_EMAIL | string | true | - | Email address, specified as the `EmailAddress` of the sender of an email |
| SEETRUE_MAILFROM_NAME | string | true | - | Name, specified as the `Name` of the sender of an email |

<p align="right">(<a href="#top">back to top</a>)</p>



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



