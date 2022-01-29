import React from "react";
import ReactDOM from "react-dom";
import { AppRouter } from "./Router";
import { SeeTrueProvider } from "seetrue.client.react";

ReactDOM.render(
  <React.StrictMode>
    <SeeTrueProvider
      host="http://localhost:5000"
      audience="http://localhost:5000"
    >
      <AppRouter />
    </SeeTrueProvider>
  </React.StrictMode>,
  document.getElementById("root")
);
