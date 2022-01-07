import React from "react";
import { Redirect, Route, Router, Switch } from "wouter";
import { Authorize, EmailEditor, Emails, Users, useSeeTrue } from ".";

export const AppRouter: React.FC = () => {
  const seeTrue = useSeeTrue();

  return (
    <Router>
      {seeTrue.authorized ? (
        <Switch>
          <Route path="/users">
            <Users />
          </Route>
          <Route path="/users/create">Users create</Route>
          <Route path="/emails">
            <Emails />
          </Route>
          <Route path="/emails/create">
            <EmailEditor />
          </Route>
          <Route>
            <Redirect to="/users" />
          </Route>
        </Switch>
      ) : (
        <Switch>
          <Route path="/">
            <Authorize />
          </Route>
          <Route>
            <Redirect to="/" />
          </Route>
        </Switch>
      )}
    </Router>
  );
};
