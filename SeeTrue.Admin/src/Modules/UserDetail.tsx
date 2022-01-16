import React from "react";
import { UserEditor, useSeeTrue } from ".";

interface UserDetailProps {
  id: string;
}

export const UserDetail: React.FC<UserDetailProps> = ({ id }) => {
  const seeTrue = useSeeTrue();
  const [data, setData] = React.useState<any>({
    loading: true,
    error: false,
    data: null,
  });

  const handleLoading = async () => {
    const newData = { ...data };

    try {
      const result = await seeTrue.api?.getUser(id);
      newData.data = result;
    } catch (error) {
      newData.error = true;
    } finally {
      newData.loading = false;
    }

    setData(newData);
  };

  React.useEffect(() => {
    handleLoading();
  }, []);

  if (data.data) {
    return <UserEditor defaultData={data.data} />;
  }

  if (data.error) {
    return <span>error</span>;
  }

  return <span>empty</span>;
};
