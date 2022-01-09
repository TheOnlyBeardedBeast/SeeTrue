import React from "react";
import { Api } from ".";

interface ISeeTrueContext {
  authorized: boolean;
  api?: Api;
  authorize: (apikey: string) => Promise<void>;
  logout: () => void;
}

const SeeTrueContext = React.createContext<ISeeTrueContext | null>(null);

export const useSeeTrue = () => {
  var context = React.useContext(SeeTrueContext);

  if (!context) {
    throw new Error("Context not initialized");
  }

  return context;
};

interface SeeTrueProviderProps {
  host?: string;
}

export const SeeTrueProvider: React.FC<SeeTrueProviderProps> = ({
  children,
  host = "http://localhost:5000",
}) => {
  const [apiKey, setApiKey] = React.useState<string | null>(null);
  const api = React.useMemo(() => {
    return new Api(host);
  }, []);

  const authorize = React.useCallback(async (_apiKey: string) => {
    try {
      await api.authorize({ apiKey: _apiKey });
      api.apiKey = _apiKey;
      setApiKey(_apiKey);
    } catch (error) {
      console.log(error);
    }
  }, []);

  const logout = () => {
    api.apiKey = undefined;
    setApiKey(null);
  };

  const context = {
    authorized: !!apiKey,
    authorize,
    logout,
    api,
  };

  return (
    <SeeTrueContext.Provider value={context}>
      {children}
    </SeeTrueContext.Provider>
  );
};
