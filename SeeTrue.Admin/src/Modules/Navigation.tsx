import * as React from "react";
import { AppNavBar, NavItemT, setItemActive } from "baseui/app-nav-bar";
import { Delete } from "baseui/icon";
import { useLocation } from "wouter";
import { useSeeTrue } from ".";

export const Navigation = () => {
  const seeTrue = useSeeTrue();
  const [location, setLocation] = useLocation();
  const [mainItems, setMainItems] = React.useState([
    {
      label: "Users",
      active: true,
      children: [
        { label: "All Users", active: true, info: "/users" },
        { label: "Create User", info: "/users/create" },
      ],
    },
    {
      label: "Emails",
      navExitIcon: Delete,
      children: [
        { label: "All Emails", info: "/emails" },
        { label: "Create Email", info: "/emails/create" },
      ],
    },
  ]);

  if (!seeTrue.authorized) {
    return null;
  }

  return (
    <AppNavBar
      title="SeeTrue"
      mainItems={mainItems}
      onMainItemSelect={(item) => {
        console.log(item.children?.[0].info ?? item.info);
        setLocation(item.children?.[0].info ?? item.info);
        setMainItems(((prev: any) =>
          setItemActive(prev, item.children?.[0] ?? item)) as any);
      }}
      username="Administrator"
      usernameSubtitle="SeeTrue Administrator"
      userItems={[{ label: "Logout" }]}
      onUserItemSelect={(item) => item.label == "Logout" && seeTrue.logout()}
    />
  );
};
