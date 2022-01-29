import * as React from 'react';
import {
  SeeTrueClient,
  TokenPair,
  TokenChangeAction,
  UserResponse,
} from 'seetrue.client';

export interface ISeeTrueContext {
  isInitializing: boolean;
  isAuthenticated: boolean | undefined;
  client: SeeTrueClient;
  user: UserResponse | null;
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

export interface SeeTrueState {
  user: UserResponse | null;
  isAuthenticated?: boolean;
}

const TOKENKEY = 'stjid';

export const SeeTrueProvider: React.FC<SeeTrueProviderProps> = ({
  children,
  audience,
  host,
}) => {
  const [state, setState] = React.useState<SeeTrueState>({
    user: null,
    isAuthenticated: undefined,
  });
  const [initializing, setInitializing] = React.useState<boolean>(true);

  const onTokenChange: TokenChangeAction = React.useCallback(
    (tokens?: TokenPair) => {
      setState({
        user: !!tokens?.access_token ? state.user : null,
        isAuthenticated: !!tokens?.access_token,
      });
      if (tokens?.access_token && tokens?.refresh_token) {
        localStorage.setItem(TOKENKEY, tokens.refresh_token);
      } else {
        localStorage.removeItem(TOKENKEY);
      }
    },
    []
  );

  const client = React.useMemo(() => {
    return new SeeTrueClient(host, audience, onTokenChange);
  }, [host]);

  const init = async (refresh_token: string | null) => {
    if (refresh_token) {
      client.tokens = { refresh_token };

      try {
        const resp = await client.refresh();
        setState({
          user: resp.user,
          isAuthenticated: true,
        });
      } catch (error) {
        setState({ ...state, isAuthenticated: false });
        localStorage.removeItem(TOKENKEY);
      }
    } else {
      setState({ ...state, isAuthenticated: false });
    }
    setInitializing(false);
  };

  React.useEffect(() => {
    const refresh_token = localStorage.getItem(TOKENKEY);

    init(refresh_token);
  }, []);

  const context = React.useMemo(
    () => ({
      client,
      isAuthenticated: state.isAuthenticated,
      isInitializing: initializing,
      user: state.user,
    }),
    [state.isAuthenticated, initializing, client]
  );

  return (
    <SeeTrueContext.Provider value={context}>
      {children}
    </SeeTrueContext.Provider>
  );
};
