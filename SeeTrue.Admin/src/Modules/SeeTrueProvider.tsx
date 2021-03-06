import { toaster } from "baseui/toast";
import React from "react";
import { Api, IAdminSettings } from ".";

interface ISeeTrueContext {
  authorized: boolean;
  api: Api;
  authorize: (host: string, apikey: string) => Promise<void>;
  logout: () => void;
  settings: IAdminSettings;
}

const SeeTrueContext = React.createContext<ISeeTrueContext | null>(null);

export const useSeeTrue = () => {
  var context = React.useContext(SeeTrueContext);

  if (!context) {
    throw new Error("Context not initialized");
  }

  return context;
};

export const SeeTrueProvider: React.FC = ({ children }) => {
  const [apiKey, setApiKey] = React.useState<string | null>(null);
  const [settings, setSettings] = React.useState<IAdminSettings | null>(null);

  const api = React.useMemo(() => {
    return new Api();
  }, []);

  const loadSetting = async () => {
    const _settings = await api.settings();
    setSettings(_settings);
  };

  const authorize = React.useCallback(
    async (_host: string, _apiKey: string) => {
      try {
        api.host = _host;
        await api.authorize({ apiKey: _apiKey });
        api.apiKey = _apiKey;
        setApiKey(_apiKey);
        loadSetting();
      } catch (error) {
        logout();
        toaster.negative(<>Something went wrong</>, {});
      }
    },
    []
  );

  const logout = () => {
    api.host = undefined;
    api.apiKey = undefined;
    setApiKey(null);
  };

  const context = {
    authorized: !!apiKey,
    authorize,
    settings: settings as IAdminSettings,
    logout,
    api,
  };

  return (
    <SeeTrueContext.Provider value={context}>
      {children}
    </SeeTrueContext.Provider>
  );
};
