import React from "react";
import { useSeeTrue } from "seetrue.client.react";

export const UserDetail: React.FC = () => {
  const { user, client } = useSeeTrue();

  const handleLogout = () => {
    client.logout();
  };

  return (
    <div>
      <p>Hello {user?.email}</p>
      <button onClick={handleLogout}>Logout</button>
    </div>
  );
};
