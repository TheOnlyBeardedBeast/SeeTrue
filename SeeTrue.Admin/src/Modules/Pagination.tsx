import * as React from "react";
import { Pagination } from "baseui/pagination";
import { styled } from "baseui";

const Container = styled("div", {
  display: "flex",
  justifyContent: "center",
  alignItems: "center",
});

interface DataPaginationProps {
  page: number;
  numPages: number;
  onPage: (nextPage: number) => void;
}

export const DataPagination: React.FC<DataPaginationProps> = ({
  page,
  numPages,
  onPage,
}) => {
  return (
    <Container>
      <Pagination
        numPages={numPages}
        currentPage={page}
        onPageChange={({ nextPage }) => {
          onPage(Math.min(Math.max(nextPage, 1), 20));
        }}
      />
    </Container>
  );
};
