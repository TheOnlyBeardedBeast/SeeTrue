<div id="top"></div>



<!-- PROJECT LOGO -->
<br />
<div align="center">
  <a href="https://github.com/TheOnlyBeardedBeast/SeeTrue">
    <img width="256px" src="../SeeTrue.Admin/src/Assets/SeeTrueIcon.png" />
  </a>

  <h3 align="center">SeeTrue.Client.React</h3>

  <p align="center">
    A library which provides a seamles connection to your SeeTrue.API instance.
    <br />
    <a href="https://github.com/TheOnlyBeardedBeast/SeeTrue/tree/master/SeeTrue.Client.React"><strong>Explore the docs »</strong></a>
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
    <li><a href="#contributing">Contributing</a></li>
    <li><a href="#license">License</a></li>
    <li><a href="#contact">Contact</a></li>
  </ol>
</details>



<!-- ABOUT THE PROJECT -->
## About The Project

[SeeTrue.Client.React](https://github.com/TheOnlyBeardedBeast/SeeTrue/tree/master/SeeTrue.Client.React) is a library which enables a seamless connection between your React application and a [SeeTrue.API](https://github.com/TheOnlyBeardedBeast/SeeTrue/tree/master/SeeTrue.API) instance. On the background [SeeTrue.Client.React](https://github.com/TheOnlyBeardedBeast/SeeTrue/tree/master/SeeTrue.Client.React) uses [SeeTrue.Client](https://github.com/TheOnlyBeardedBeast/SeeTrue/tree/master/SeeTrue.Client) inside a React context, and it provides access to the authetication state and an API client through a hook.

SeeTrue.Client.React provides the following functionality:
* Access to the authentication state
* Acces to the autheticated user
* Automatic token refresh on the background
* Acces to the API client [SeeTrue.Client](https://github.com/TheOnlyBeardedBeast/SeeTrue/tree/master/SeeTrue.Client)


<p align="right">(<a href="#top">back to top</a>)</p>



### Built With

Built with hard work, tears, sweat, dedication and love.


* [React.js](https://reactjs.org/)
* [TypeScript](https://www.typescriptlang.org/)

<p align="right">(<a href="#top">back to top</a>)</p>



<!-- GETTING STARTED -->
## Getting Started
### Prerequisites

You need `npm` or `yarn` installed, you should also have a SeeTrue instance runinng. 

### Installation

using npm:
```bash
npm install seetrue.client.react
```

using yarn:
```bash
yarn add seetrue.client.react
```

<p align="right">(<a href="#top">back to top</a>)</p>



<!-- USAGE EXAMPLES -->
## Usage

To access the context we have to init a provider, which will setup our client connection, it also checks the localstorage for a refresh token, and if it founds a refresh token it tries to refresh your session. 

### Setup

In the root of you application initialize the `SeeTrueProvider`.

1. Import the `SeeTrueProvider`
```typescript
import { SeeTrueProvider } from "seetrue.client.react";
```
2. Initialize the `SeeTrueProvider`
```jsx
  <SeeTrueProvider
    host="http://localhost:5000"
    audience="http://localhost:5000"
    tokenLifeTime={60000}
  >
    <App />
  </SeeTrueProvider>
```

| Prop          | Type    | Default value | Required | Description                                                                                  |
|---------------|---------|---------------|----------|----------------------------------------------------------------------------------------------|
| host          | string  |               | true     | The host of your SeeTrue.API instance                                                        |
| audience      | string  |               | true     | A supported audience from your SeeTrue.API instance config                                   |
| tokenLifeTime | number? | 3600000       | false    | Access token lifetime in miliseconds (also represents the background token refresh interval) |

### Context usage

Import the hook
```typescript
import { useSeeTrue } from "seetrue.client.react";
```

Use the hook:
```typescript
const { isInitializing, isAuthenticated, user, client } = useSeeTrue();
```


| Prop            | Type     | Description                                                                                                                                                                                                                                                                                              |
|-----------------|----------|----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| isInitializing  | boolean  | Initializing state, when you open the app, the app checks the refresh token then refreshes the user.  While this initial check happens we cannot determinate if the user is logged in or not. We can show a loader \| splash screen before we have a value which desribes the real authentication state. |
| isAuthenticated | boolean? | It tells us if the user is logged in or not. Before the app is initialized the value is undefined, after it can be true or false based on the initialization process.                                                                                                                                    |
| user            | object?  | A user object providing account information about thr currently logged in user. Undefined if the user is not authenticated, [docs](https://github.com/TheOnlyBeardedBeast/SeeTrue/blob/master/SeeTrue.Client/docs/interfaces/types.UserResponse.md)                                                                                                                                                    |
| client          | object   | A SeeTrue.Client instance, which enables us to interact with the SeeTrue.API [docs](https://github.com/TheOnlyBeardedBeast/SeeTrue/blob/master/SeeTrue.Client/docs/classes/client.SeeTrueClient.md)                                                                                                                                                                                                                            |

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



