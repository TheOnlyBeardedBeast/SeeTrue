import React from "react";
import { useQuery } from "react-query";
import { useSeeTrue, MailResponse, EmailEditor } from ".";

export interface EmailDetailProps {
  id: string;
}

export const EmailDetail: React.FC<EmailDetailProps> = ({ id }) => {
  const seeTrue = useSeeTrue();
  const { data, error } = useQuery(["emails", id], async () => {
    const result = await seeTrue.api?.getMail(id);
    return result;
  });

  if (data) {
    return <EmailEditor defaultData={data} />;
  }

  if (error) {
    return <span>error</span>;
  }

  return null;
};
