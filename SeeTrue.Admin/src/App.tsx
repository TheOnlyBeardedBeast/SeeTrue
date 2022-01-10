import { Client as Styletron } from "styletron-engine-atomic";
import { Provider as StyletronProvider } from "styletron-react";
import { DarkTheme, BaseProvider, styled } from "baseui";
import { ToasterContainer } from "baseui/toast";
import {
  Navigation,
  AppRouter,
  SeeTrueProvider,
  ConfirmationProvider,
} from "./Modules";
import "./App.css";
import { QueryClient, QueryClientProvider } from "react-query";

const queryClient = new QueryClient();

const engine = new Styletron();
const Container = styled("div", {
  height: "100vh",
  background: DarkTheme.colors.background,
});
export default function Hello() {
  return (
    <StyletronProvider value={engine}>
      <BaseProvider theme={DarkTheme}>
        <QueryClientProvider client={queryClient}>
          <SeeTrueProvider>
            <ToasterContainer />
            <ConfirmationProvider>
              <Container>
                <Navigation />
                <AppRouter />
              </Container>
            </ConfirmationProvider>
          </SeeTrueProvider>
        </QueryClientProvider>
      </BaseProvider>
    </StyletronProvider>
  );
}
