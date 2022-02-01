import * as React from 'react';
import {
  SeeTrueClient,
  TokenPair,
  TokenChangeAction,
  UserResponse,
  UserChangeAction,
} from 'seetrue.client';

export interface ISeeTrueContext {
  isInitializing: boolean;
  isAuthenticated?: boolean;
  client: SeeTrueClient;
  user?: UserResponse;
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
  tokenLifeTime: number;
}

const TOKENKEY = 'stjid';

export const SeeTrueProvider: React.FC<SeeTrueProviderProps> = ({
  children,
  audience,
  host,
}) => {
  const [isAuthenticated, setIsAuthenticated] = React.useState<
    boolean | undefined
  >(undefined);
  const [isInitializing, setInitializing] = React.useState<boolean>(true);
  const [user, setUser] = React.useState<UserResponse | undefined>(undefined);

  const onTokenChange: TokenChangeAction = React.useCallback(
    (tokens?: TokenPair) => {
      setIsAuthenticated(!!tokens?.access_token);
      if (tokens?.access_token && tokens?.refresh_token) {
        localStorage.setItem(TOKENKEY, tokens.refresh_token);
      } else {
        localStorage.removeItem(TOKENKEY);
      }
    },
    []
  );

  const onUserChange: UserChangeAction = React.useCallback(
    (user?: UserResponse) => {
      setUser(user);
    },
    []
  );

  const client = React.useMemo(() => {
    return new SeeTrueClient(host, audience, onTokenChange, onUserChange);
  }, [host]);

  const init = async (refresh_token: string | null) => {
    if (refresh_token) {
      client.tokens = { refresh_token };

      try {
        await client.refresh();
      } catch (error) {
        setIsAuthenticated(false);
        localStorage.removeItem(TOKENKEY);
      }
    } else {
      setIsAuthenticated(false);
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
      isAuthenticated,
      isInitializing,
      user,
    }),
    [isAuthenticated, isInitializing, client, user]
  );

  return (
    <SeeTrueContext.Provider value={context}>
      {children}
    </SeeTrueContext.Provider>
  );
};
