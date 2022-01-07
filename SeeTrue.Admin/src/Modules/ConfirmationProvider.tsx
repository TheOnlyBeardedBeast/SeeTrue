import React from "react";
import {
  Modal,
  ModalHeader,
  ModalBody,
  ModalFooter,
  ModalButton,
  FocusOnce,
} from "baseui/modal";

interface IConfirmationData {
  header: string;
  message: string;
  action: () => void | Promise<void>;
}

interface IConfirmationContext {
  confirm: (data: IConfirmationData) => void;
  close: () => void;
}

const ConfirmationContext = React.createContext<IConfirmationContext | null>(
  null
);

export const useConfirmation = () => {
  const context = React.useContext(ConfirmationContext);

  if (!context) {
    throw new Error("Confirmation context not initialized");
  }

  return context;
};

export const ConfirmationProvider: React.FC = ({ children }) => {
  const [data, setData] = React.useState<IConfirmationData | null>(null);

  const confirm = React.useCallback((_data: IConfirmationData) => {
    setData(_data);
  }, []);

  const _confirm = React.useCallback(async () => {
    await data?.action();
  }, [data]);

  const close = React.useCallback(() => setData(null), []);

  const context = React.useMemo(
    () => ({
      confirm,
      close,
    }),
    []
  );

  return (
    <ConfirmationContext.Provider value={context}>
      {children}
      <Modal onClose={close} isOpen={!!data}>
        <FocusOnce>
          <ModalHeader>{data?.header}</ModalHeader>
        </FocusOnce>
        <ModalBody>
          <p>{data?.message}</p>
        </ModalBody>
        <ModalFooter>
          <ModalButton kind="tertiary" onClick={close}>
            Close
          </ModalButton>
          <ModalButton autoFocus onClick={_confirm}>
            Confirm
          </ModalButton>
        </ModalFooter>
      </Modal>
    </ConfirmationContext.Provider>
  );
};
