import { Client as Styletron } from "styletron-engine-atomic";
import { Provider as StyletronProvider } from "styletron-react";
import { DarkTheme, BaseProvider, styled } from "baseui";
import { ToasterContainer } from "baseui/toast";
import { Navigation, AppRouter, SeeTrueProvider } from "./Modules";

const engine = new Styletron();
const Container = styled("div", {
  height: "100vh",
  background: DarkTheme.colors.background,
});
export default function Hello() {
  return (
    <StyletronProvider value={engine}>
      <BaseProvider theme={DarkTheme}>
        <SeeTrueProvider>
          <ToasterContainer />
          <Container>
            <Navigation />
            <AppRouter />
          </Container>
        </SeeTrueProvider>
      </BaseProvider>
    </StyletronProvider>
  );
}
