import React from "react";
import { useSeeTrue } from "seetrue.client.react";

interface ConfirmSignUpProps {
  token: string;
}

export const ConfirmSignUp: React.FC<ConfirmSignUpProps> = ({ token }) => {
  const [confirmed, setIsconfirmed] = React.useState<boolean | null>(null);

  const { client } = useSeeTrue();

  React.useEffect(() => {
    try {
      client.verifySignup(token);
    } catch (error) {
      setIsconfirmed(false);
    }
  }, [token]);

  if (confirmed === null || confirmed === true) {
    <div
      style={{
        height: "100ch",
        display: "flex",
        justifyContent: "center",
        alignItems: "center",
      }}
    >
      <span>Loading</span>
    </div>;
  }

  return (
    <div
      style={{
        height: "100ch",
        display: "flex",
        justifyContent: "center",
        alignItems: "center",
      }}
    >
      {!confirmed ? <span>Something went wront</span> : <span>Logged in</span>}
    </div>
  );
};
