import React from "react";
import { Api } from ".";

interface ISeeTrueContext {
  authorized: boolean;
  api?: Api;
  authorize: (apikey: string) => Promise<void>;
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
  const [authorized, setAuthorized] = React.useState<boolean>(false);
  const api = React.useMemo(() => new Api(host), []);

  const authorize = React.useCallback(async (apiKey: string) => {
    const result = await api.authorize({ apiKey });

    setAuthorized(result);
  }, []);

  const context = {
    authorized,
    authorize,
    api,
  };

  return (
    <SeeTrueContext.Provider value={context}>
      {children}
    </SeeTrueContext.Provider>
  );
};
