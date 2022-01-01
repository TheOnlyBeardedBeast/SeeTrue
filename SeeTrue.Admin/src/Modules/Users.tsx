import React, { useState } from "react";
import { Checkbox } from "baseui/checkbox";
import { StyledLink as Link } from "baseui/link";
import { TableBuilder, TableBuilderColumn } from "baseui/table-semantic";
import { styled } from "baseui";
import { DataPagination } from ".";

const CustomTableBuilder = styled(TableBuilder, {
  maxWidth: "1307px",
  margin: "20px auto",
});

export const Users: React.FC = () => {
  const [data, setData] = useState([
    {
      id: 10,
      email: "tes@email.com",
      confirmed: false,
      selected: false,
    },
    {
      id: 11,
      email: "tes@email.com",
      confirmed: false,
      selected: false,
    },
    {
      id: 12,
      email: "tes@email.com",
      confirmed: false,
      selected: false,
    },
  ]);
  const hasAny = Boolean(data.length);
  const hasAll = hasAny && data.every((x) => x.selected);
  const hasSome = hasAny && data.some((x) => x.selected);
  function toggleAll() {
    setData((data) =>
      data.map((row) => ({
        ...row,
        selected: !hasAll,
      }))
    );
  }
  function toggle(event: any) {
    const { name, checked } = event.currentTarget;
    setData((data) =>
      data.map((row) => ({
        ...row,
        selected: String(row.id) === name ? checked : row.selected,
      }))
    );
  }
  return (
    <>
      <CustomTableBuilder data={data}>
        <TableBuilderColumn
          overrides={{
            TableHeadCell: { style: { width: "1%" } },
            TableBodyCell: { style: { width: "1%" } },
          }}
          header={
            <Checkbox
              checked={hasAll}
              isIndeterminate={!hasAll && hasSome}
              onChange={toggleAll}
            />
          }
        >
          {(row) => (
            <Checkbox name={row.id} checked={row.selected} onChange={toggle} />
          )}
        </TableBuilderColumn>
        <TableBuilderColumn header="Email">
          {(row) => <Link href={row.email}>{row.email}</Link>}
        </TableBuilderColumn>
        <TableBuilderColumn header="Confirmed">
          {(row) => (row.confirmed ? <span>True</span> : <span>False</span>)}
        </TableBuilderColumn>
      </CustomTableBuilder>
      <DataPagination />
    </>
  );
};
