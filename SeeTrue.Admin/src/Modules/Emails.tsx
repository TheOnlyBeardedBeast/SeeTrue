import React from "react";
import { Checkbox } from "baseui/checkbox";
import { StyledLink as Link } from "baseui/link";
import { TableBuilder, TableBuilderColumn } from "baseui/table-semantic";
import { styled } from "baseui";
import {
  DataPagination,
  PaginationResponse,
  useSeeTrue,
  // useConfirmation,
  MailResponse,
} from ".";
import { Button, SHAPE, SIZE } from "baseui/button";
import { X, PencilSimple } from "phosphor-react";
// import { toaster } from "baseui/toast";
import { Notification, KIND } from "baseui/notification";
import { useLocation } from "wouter";
import { Cell, Grid } from "baseui/layout-grid";

const ActionButton = styled(Button, {
  margin: "0 5px",
});

export const Emails: React.FC = () => {
  const seeTrue = useSeeTrue();
  const [data, setData] = React.useState<PaginationResponse<MailResponse>>();
  const [selections, setSelections] = React.useState<Set<string>>(new Set());
  const [_location, setLocation] = useLocation();
  // const { confirm, close } = useConfirmation();

  React.useEffect(() => {
    seeTrue.api?.getMails().then((data) => setData(data));
  }, []);

  const onPage = (page: number) =>
    seeTrue.api?.getMails(page).then((data) => setData(data));

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

  // const deleteUser = async (id: string) => {
  //   try {
  //     await seeTrue.api?.deleteUser(id);
  //     await seeTrue.api?.getUsers(data?.page).then((data) => setData(data));

  //     toaster.positive(<>User deleted.</>, {});
  //   } catch (error) {
  //     toaster.negative(<>User delete failed!</>, {});
  //   } finally {
  //     close();
  //   }
  // };

  // const deleteUserClick = (id: string) => async () => {
  //   const item = data?.items.find((e) => e.id === id);
  //   if (item) {
  //     await confirm({
  //       message: `Do you wish to delete ${item?.email}`,
  //       header: "Remove user",
  //       action: () => deleteUser(item.id),
  //     });
  //   }
  // };

  const navigateToEmailDetail = (id: string) => () =>
    setLocation(`/emails/${id}`);

  if (!data) {
    return null;
  }

  if (data?.itemCount === 0 && data.page === 1) {
    return (
      <Notification
        overrides={{
          Body: {
            style: { width: "auto", maxWidth: "1307px", margin: "20px auto" },
          },
        }}
        kind={KIND.negative}
      >
        {() => "There are no emails in the database."}
      </Notification>
    );
  }

  return data ? (
    <Grid>
      <Cell span={12}>
        <TableBuilder
          overrides={{ Root: { style: { marginTop: "20px" } } }}
          data={data?.items}
        >
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
            {(row: MailResponse) => (
              <Checkbox
                name={row.id}
                checked={selections.has(row.id)}
                onChange={toggle}
              />
            )}
          </TableBuilderColumn>
          <TableBuilderColumn header="Type">
            {(row: MailResponse) =>
              Object.entries(seeTrue.settings.emailTypes).find(
                ([key, value]) => value === row.type
              )?.[0]
            }
          </TableBuilderColumn>
          <TableBuilderColumn header="Language">
            {(row: MailResponse) => row.language?.toUpperCase()}
          </TableBuilderColumn>
          <TableBuilderColumn header="Audience">
            {(row: MailResponse) => row.audience}
          </TableBuilderColumn>
          <TableBuilderColumn header="Actions">
            {(row) => (
              <>
                <ActionButton
                  // onClick={deleteUserClick(row.id)}
                  size={SIZE.mini}
                  shape={SHAPE.circle}
                >
                  <X />
                </ActionButton>
                <ActionButton
                  onClick={navigateToEmailDetail(row.id)}
                  size={SIZE.mini}
                  shape={SHAPE.circle}
                >
                  <PencilSimple />
                </ActionButton>
              </>
            )}
          </TableBuilderColumn>
        </TableBuilder>
        <DataPagination
          page={data.page}
          numPages={Math.ceil(data.itemCount / data.perPage)}
          onPage={onPage}
        />
      </Cell>
    </Grid>
  ) : null;
};
