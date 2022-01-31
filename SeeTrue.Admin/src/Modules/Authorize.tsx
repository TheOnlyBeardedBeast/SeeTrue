import React from "react";
import { Input } from "baseui/input";
import { Button } from "baseui/button";
import { FormControl } from "baseui/form-control";
import { styled } from "baseui";
import { useSeeTrue } from ".";
import { toaster } from "baseui/toast";
import Logo from "../Assets/SeeTrueIcon.png";
import { Display2 } from "baseui/typography";

const Container = styled("div", {
  display: "flex",
  flexDirection: "column",
  alignItems: "center",
  justifyContent: "center",
  minHeight: "100%",
  padding: "24px 0",
});

const Form = styled("form", {
  width: "100%",
  maxWidth: "400px",
});

export const Authorize: React.FC = () => {
  const seeTrue = useSeeTrue();
  const hostRef = React.useRef<HTMLInputElement>(null);
  const apikeyRef = React.useRef<HTMLInputElement>(null);

  const handleSubmit = (event: React.FormEvent) => {
    event.preventDefault();
    if (apikeyRef.current?.value && hostRef.current?.value) {
      seeTrue
        .authorize(hostRef.current.value, apikeyRef.current!.value)
        .catch(() => {
          toaster.negative(<>Authorization error!</>, {});
        });
    }
  };

  return (
    <Container>
      <img src={Logo} style={{ width: "256px" }} />
      <Display2>SeeTrue</Display2>
      <Form onSubmit={handleSubmit}>
        <FormControl label="SeeTrue host">
          <Input
            type="text"
            inputRef={hostRef}
            placeholder="Host"
            clearOnEscape
          />
        </FormControl>
        <FormControl label="Api key">
          <Input
            type="password"
            inputRef={apikeyRef}
            placeholder="ApiKey"
            clearOnEscape
          />
        </FormControl>
        <Button $style={{ width: "100%" }} type="submit">
          Authorize
        </Button>
      </Form>
    </Container>
  );
};
