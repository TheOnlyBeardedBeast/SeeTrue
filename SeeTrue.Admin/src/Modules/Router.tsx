import React from "react";
import { Redirect, Route, Router, Switch } from "wouter";
import {
  EmailEditor,
  EmailDetail,
  useSeeTrue,
  UserEditor,
  Authorize,
  Emails,
  Users,
  UserDetail,
} from ".";

export const AppRouter: React.FC = () => {
  const seeTrue = useSeeTrue();

  return (
    <Router>
      {seeTrue.authorized ? (
        <Switch>
          <Route path="/users">
            <Users />
          </Route>
          <Route path="/users/create">
            <UserEditor />
          </Route>
          <Route path="/users/:id">
            {(params) => <UserDetail id={params.id} />}
          </Route>
          <Route path="/emails">
            <Emails />
          </Route>
          <Route path="/emails/create">
            <EmailEditor />
          </Route>
          <Route path="/emails/:id">
            {(params) => <EmailDetail id={params.id} />}
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
