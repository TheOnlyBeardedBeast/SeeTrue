import React from "react";
import { Router, Switch, Route, Redirect } from "wouter";
import { useSeeTrue } from "seetrue.client.react";
import { UserDetail } from "./UserDetail";
import { Login } from "./SignIn";
import { SignUp } from "./SignUp";
import { ConfirmSignUp } from "./ConfirmSignup";

export const AuthenticatedRoutes = () => {
  return (
    <Switch>
      <Route path="/">
        <UserDetail />
      </Route>
      <Route>
        <Redirect to="/" />
      </Route>
    </Switch>
  );
};

export const UnAuthenticatedRoutes = () => {
  return (
    <Switch>
      <Route path="/login">
        <Login />
      </Route>
      <Route path="/signup">
        <SignUp />
      </Route>
      <Route path="/confirm-signup/:token">
        {(params) => <ConfirmSignUp token={params.token} />}
      </Route>
      <Route>
        <Redirect to="/login" />
      </Route>
    </Switch>
  );
};

export const AppRouter = () => {
  const { isInitializing, isAuthenticated } = useSeeTrue();

  // checking if refresh token exists
  // if refresh token exists, tries to refreshes the authentication
  if (isInitializing) {
    return null;
  }

  return (
    <Router>
      {isAuthenticated ? <AuthenticatedRoutes /> : <UnAuthenticatedRoutes />}
    </Router>
  );
};
