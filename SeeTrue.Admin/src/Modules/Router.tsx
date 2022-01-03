import React from "react";
import { Redirect, Route, Router } from "wouter";
import { EmailEditor, Users } from ".";

export const AppRouter: React.FC = () => {
  return (
    <Router>
      <Route path="/users">
        <Users />
      </Route>
      <Route path="/users/create">Users create</Route>
      <Route path="/emails">Emails</Route>
      <Route path="/emails/create">
        <EmailEditor />
      </Route>
      <Route>
        <Redirect to="/users" />
      </Route>
    </Router>
  );
};
