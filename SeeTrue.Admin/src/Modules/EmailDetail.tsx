import React from "react";
import { useSeeTrue, EmailEditor } from ".";

export interface EmailDetailProps {
  id: string;
}

export const EmailDetail: React.FC<EmailDetailProps> = ({ id }) => {
  const seeTrue = useSeeTrue();
  const [data, setData] = React.useState<any>({
    loading: true,
    error: false,
    data: null,
  });

  const handleLoading = async () => {
    const newData = { ...data };

    try {
      const result = await seeTrue.api?.getMail(id);
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
    return <EmailEditor defaultData={data.data} />;
  }

  if (data.error) {
    return <span>error</span>;
  }

  return <span>empty</span>;
};
