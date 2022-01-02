import React, { useState } from "react";
import { Checkbox } from "baseui/checkbox";
import { StyledLink as Link } from "baseui/link";
import { TableBuilder, TableBuilderColumn } from "baseui/table-semantic";
import { styled } from "baseui";
import { Api, DataPagination, UsersResponse } from ".";

const CustomTableBuilder = styled(TableBuilder, {
  maxWidth: "1307px",
  margin: "20px auto",
});

const client = new Api("http://localhost:5000", "SuperSecureApiKey");

export const Users: React.FC = () => {
  const [data, setData] = useState<UsersResponse>();
  const [selections, setSelections] = useState<Set<string>>(new Set());

  React.useEffect(() => {
    client.getUsers().then((data) => setData(data));
  }, []);

  const onPage = (page: number) =>
    client.getUsers(page).then((data) => setData(data));

  const hasAny = Boolean(data?.perPage);
  const hasAll = hasAny && selections.size === data?.perPage;
  const hasSome =
    hasAny && selections.size !== data?.perPage && selections.size != 0;
  function toggleAll() {
    if (hasSome) {
      setSelections(new Set());
    } else {
      setSelections(() => new Set(data?.items.map((e) => e.id) ?? []));
    }
  }

  function toggle(event: any) {
    const { name, checked } = event.currentTarget;
    const items = Array.from(selections.values());

    if (checked) {
      setSelections(new Set([...items, name]));
    } else {
      setSelections(new Set(items.filter((e) => e != name)));
    }
  }

  return data ? (
    <>
      <CustomTableBuilder data={data?.items}>
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
            <Checkbox
              name={row.id}
              checked={selections.has(row.id)}
              onChange={toggle}
            />
          )}
        </TableBuilderColumn>
        <TableBuilderColumn header="Email">
          {(row) => <Link href={`mailTo:${row.email}`}>{row.email}</Link>}
        </TableBuilderColumn>
        <TableBuilderColumn header="Confirmed">
          {(row) => (row.confirmed ? <span>True</span> : <span>False</span>)}
        </TableBuilderColumn>
        <TableBuilderColumn header="Role">
          {(row) => row.role}
        </TableBuilderColumn>
        <TableBuilderColumn header="Created">
          {(row) => row.createdAt}
        </TableBuilderColumn>
        <TableBuilderColumn header="Las Sign In">
          {(row) => row.lastSignInAt}
        </TableBuilderColumn>
      </CustomTableBuilder>
      <DataPagination
        page={data.page}
        numPages={Math.ceil(data.itemCount / data.perPage)}
        onPage={onPage}
      />
    </>
  ) : null;
};
