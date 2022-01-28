import * as React from 'react';
import { SeeTrueClient, TokenPair, TokenChangeAction } from 'seetrue.client';

export interface ISeeTrueContext {
  isInitializing: boolean;
  isAuthenticated: boolean | undefined;
  client: SeeTrueClient;
}

const SeeTrueContext = React.createContext<ISeeTrueContext | undefined>(
  undefined
);

export const useSeeTrue = () => {
  const context = React.useContext(SeeTrueContext);

  if (!context) {
    throw new Error('SeeTrueContext not initialized.');
  }

  return context;
};

export interface SeeTrueProviderProps {
  audience: string;
  host: string;
}

const TOKENKEY = 'stjid';

export const SeeTrueProvider: React.FC<SeeTrueProviderProps> = ({
  children,
  audience,
  host,
}) => {
  const [state, setState] = React.useState<any>({
    isAuthenticated: undefined,
    isInitializing: true,
  });

  const onTokenChange: TokenChangeAction = React.useCallback(
    (tokens?: TokenPair) => {
      setState({
        isAuthenticated: !!tokens?.access_token,
        isInitializing: false,
      });
      if (tokens?.refresh_token) {
        localStorage.setItem(TOKENKEY, tokens.refresh_token);
      }
    },
    []
  );

  const client = React.useMemo(() => {
    return new SeeTrueClient(host, audience, onTokenChange);
  }, [host]);

  React.useEffect(() => {
    const refresh_token = localStorage.getItem(TOKENKEY);

    if (refresh_token) {
      client.tokens = { refresh_token };
      client.refresh();
    } else {
      setState({ ...state, isAuthenticated: false, isInitializing: false });
    }
  }, []);

  const context = React.useMemo(
    () => ({
      client,
      isAuthenticated: state.isAuthenticated,
      isInitializing: state.isInitializing,
    }),
    [state.isAuthenticated, state.isInitializing, client]
  );

  return (
    <SeeTrueContext.Provider value={context}>
      {children}
    </SeeTrueContext.Provider>
  );
};
