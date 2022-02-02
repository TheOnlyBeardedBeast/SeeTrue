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
  tokenLifeTime?: number;
  keyPrefix?: string;
}

const TOKENKEY = 'stjid';
let intervalId: number | undefined = undefined;

export const SeeTrueProvider: React.FC<SeeTrueProviderProps> = ({
  children,
  audience,
  host,
  tokenLifeTime = 3600000,
  keyPrefix = '',
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
        localStorage.setItem(keyPrefix + TOKENKEY, tokens.refresh_token);
      } else {
        localStorage.removeItem(keyPrefix + TOKENKEY);
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

  const onBackgroundTokenUpdate = React.useCallback((event: StorageEvent) => {
    if (event.newValue) {
      client.silentTokenUpdate({ refresh_token: event.newValue });
    } else {
      client.tokens = undefined;
    }
  }, []);

  const onStorageUpdate = React.useCallback((event: StorageEvent) => {
    if (event.key === keyPrefix + TOKENKEY) {
      onBackgroundTokenUpdate(event);
    }
  }, []);

  const client = React.useMemo(() => {
    return new SeeTrueClient(host, audience, onTokenChange, onUserChange);
  }, [host]);

  const init = async (refresh_token: string | null) => {
    if (refresh_token) {
      client.silentTokenUpdate({ refresh_token });

      try {
        await client.refresh();
      } catch (error) {
        setIsAuthenticated(false);
        localStorage.removeItem(keyPrefix + TOKENKEY);
      }
    } else {
      setIsAuthenticated(false);
    }
    setInitializing(false);
  };

  React.useEffect(() => {
    const refresh_token = localStorage.getItem(keyPrefix + TOKENKEY);

    init(refresh_token);
  }, []);

  React.useEffect(() => {
    if (isAuthenticated) {
      intervalId = window.setInterval(
        () => client.refresh(),
        tokenLifeTime - 250
      );
    } else if (!isInitializing && intervalId) {
      window.clearInterval(intervalId);
    }
  }, [isAuthenticated]);

  React.useEffect(() => {
    window.addEventListener('storage', onStorageUpdate);

    return () => {
      window.removeEventListener('storage', onStorageUpdate);
    };
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
