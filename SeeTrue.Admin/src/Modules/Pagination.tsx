import * as React from "react";
import { Pagination } from "baseui/pagination";
import { styled } from "baseui";

const Container = styled("div", {
  display: "flex",
  justifyContent: "center",
  alignItems: "center",
});

export const DataPagination: React.FC = () => {
  const [currentPage, setCurrentPage] = React.useState(1);
  return (
    <Container>
      <Pagination
        numPages={20}
        currentPage={currentPage}
        onPageChange={({ nextPage }) => {
          setCurrentPage(Math.min(Math.max(nextPage, 1), 20));
        }}
      />
    </Container>
  );
};
