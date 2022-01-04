import React from "react";
import { Input } from "baseui/input";
import { Button } from "baseui/button";
import { styled } from "baseui";
import { useSeeTrue } from ".";

const Container = styled("div", {
  display: "flex",
  flexDirection: "column",
  alignItems: "center",
  justifyContent: "center",
  minHeight: "100%",
});

const Form = styled("form", {
  width: "100%",
  maxWidth: "400px",
});

export const Authorize: React.FC = () => {
  const seeTrue = useSeeTrue();
  const inputRef = React.useRef<HTMLInputElement>(null);

  const handleSubmit = (event: React.FormEvent) => {
    event.preventDefault();
    inputRef.current?.value && seeTrue.authorize(inputRef.current?.value);
  };

  return (
    <Container>
      <Form onSubmit={handleSubmit}>
        <Input
          type="password"
          inputRef={inputRef}
          placeholder="ApiKey"
          clearOnEscape
        />
        <Button $style={{ marginTop: "20px", width: "100%" }} type="submit">
          Authorize
        </Button>
      </Form>
    </Container>
  );
};
