import * as React from "react";
import { AppNavBar, NavItemT, setItemActive } from "baseui/app-nav-bar";
import { ChevronDown, Delete, Overflow, Upload } from "baseui/icon";

export const Navigation = () => {
  const [mainItems, setMainItems] = React.useState([
    {
      icon: Upload,
      label: "Users",
      children: [
        { icon: Upload, label: "All Users" },
        { icon: Upload, label: "Create User" },
      ],
    },
    {
      active: true,
      icon: ChevronDown,
      label: "Emails",
      navExitIcon: Delete,
      children: [
        { icon: Upload, label: "All Emails" },
        { icon: Upload, label: "Create Email" },
      ],
    },
  ]);
  return (
    <AppNavBar
      title="SeeTrue"
      mainItems={mainItems}
      onMainItemSelect={(item) => {
        setMainItems(((prev: any) => setItemActive(prev, item)) as any);
      }}
      username="Umka Marshmallow"
      usernameSubtitle="5 Stars"
      userItems={[
        { icon: Overflow, label: "Users" },
        { icon: Overflow, label: "User B" },
      ]}
      onUserItemSelect={(item) => console.log(item)}
    />
  );
};
