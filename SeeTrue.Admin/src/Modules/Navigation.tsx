import * as React from "react";
import { AppNavBar, NavItemT, setItemActive } from "baseui/app-nav-bar";
import { useLocation } from "wouter";
import { useSeeTrue } from ".";

export const Navigation = () => {
  const seeTrue = useSeeTrue();
  const [location, setLocation] = useLocation();
  const [mainItems, setMainItems] = React.useState<NavItemT[]>([
    {
      label: "Users",
      children: [
        { label: "All Users", info: "/users" },
        { label: "Create User", info: "/users/create" },
      ],
    },
    {
      label: "Emails",
      children: [
        { label: "All Emails", info: "/emails" },
        { label: "Create Email", info: "/emails/create" },
      ],
    },
  ]);

  React.useEffect(() => {
    setMainItems((prev: any) => {
      const item = mainItems
        .map((e) => e.children)
        .flat()
        .find((e) => e?.info == location);
      if (item) {
        return setItemActive(prev, item) as any;
      }
      return prev;
    });
  }, [location]);

  if (!seeTrue.authorized) {
    return null;
  }

  return (
    <AppNavBar
      title="SeeTrue"
      mainItems={mainItems}
      onMainItemSelect={(item) => {
        setLocation(item.children?.[0].info ?? item.info);
        // setMainItems(((prev: any) =>
        //   setItemActive(prev, item.children?.[0] ?? item)) as any);
      }}
      username="Administrator"
      usernameSubtitle="SeeTrue Administrator"
      userItems={[{ label: "Logout" }]}
      onUserItemSelect={(item) => item.label == "Logout" && seeTrue.logout()}
    />
  );
};
