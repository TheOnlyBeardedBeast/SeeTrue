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
      <form onSubmit={handleSubmit}>
        <Input inputRef={inputRef} placeholder="ApiKey" clearOnEscape />
        <Button type="submit">Authorize</Button>
      </form>
    </Container>
  );
};
