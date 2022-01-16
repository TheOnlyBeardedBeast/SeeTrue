import React from "react";
import { FormControl } from "baseui/form-control";
import { Input } from "baseui/input";
import { Select } from "baseui/select";
import { Textarea } from "baseui/textarea";
import { Checkbox, LABEL_PLACEMENT } from "baseui/checkbox";
import { Button } from "baseui/button";
import { Grid, Cell } from "baseui/layout-grid";
import { Controller, useForm } from "react-hook-form";
import { UserResponse, useSeeTrue } from ".";
import { useLocation } from "wouter";
import { toaster } from "baseui/toast";

interface UserEditorProps {
  defaultData?: UserResponse;
}

export const UserEditor: React.FC<UserEditorProps> = ({ defaultData }) => {
  const seeTrue = useSeeTrue();
  const { register, control, handleSubmit } = useForm({
    defaultValues: {
      id: defaultData?.id ?? undefined,
      email: defaultData?.email ?? undefined,
      password: undefined,
      role: defaultData?.role ? [{ key: defaultData.role }] : undefined,
      audience: defaultData?.aud ? [{ key: defaultData.aud }] : undefined,
      language: defaultData?.language
        ? [
            {
              key: defaultData?.language.toUpperCase(),
              value: defaultData?.language,
            },
          ]
        : undefined,
      userMetaData: JSON.stringify({ name: "name" }, null, 2),
      confirmed: !!defaultData?.confirmedAt ?? false,
    },
  });

  const [_, setLocation] = useLocation();

  const onSubmit = (value: any) => {
    const data = {
      ...value,
      userMetaData: JSON.parse(value.userMetaData),
      audience: value.audience[0].key,
      language: value.language[0].value,
      role: value.role[0].key,
    };

    if (defaultData) {
      seeTrue.api
        ?.updateUser(data)
        .then(() => setLocation("/users"))
        .catch(() => toaster.negative(<>Failed to update user</>, {}));
    } else {
      seeTrue.api
        ?.createUser(data)
        .then(() => setLocation("/users"))
        .catch(() => toaster.negative(<>Failed to create user</>, {}));
    }
  };

  return (
    <Grid>
      <Cell span={[12, 6, 6]}>
        <form onSubmit={handleSubmit(onSubmit)}>
          <input type="hidden" {...register("id")} />
          <FormControl label="Email">
            <Controller
              name="email"
              control={control}
              render={({ field }) => (
                <Input
                  type="email"
                  required
                  value={field.value}
                  onChange={field.onChange}
                  placeholder="Email"
                  clearOnEscape
                />
              )}
            />
          </FormControl>
          <FormControl label="Password">
            <Controller
              name="password"
              control={control}
              render={({ field }) => (
                <Input
                  required={!defaultData}
                  type="password"
                  value={field.value}
                  onChange={field.onChange}
                  placeholder="Password"
                  clearOnEscape
                />
              )}
            />
          </FormControl>
          <FormControl label="Audience">
            <Controller
              name="audience"
              control={control}
              render={({ field }) => (
                <Select
                  required
                  id="audience-select"
                  options={seeTrue.settings.audiences.map((e) => ({
                    key: e,
                  }))}
                  labelKey="key"
                  valueKey="key"
                  onChange={(e) => field.onChange(e.value)}
                  value={field.value}
                />
              )}
            />
          </FormControl>
          <FormControl label="Role">
            <Controller
              name="role"
              control={control}
              render={({ field }) => (
                <Select
                  required
                  id="role-select"
                  options={seeTrue.settings.roles.map((e) => ({
                    key: e,
                  }))}
                  labelKey="key"
                  valueKey="key"
                  onChange={(e) => field.onChange(e.value)}
                  value={field.value}
                />
              )}
            />
          </FormControl>
          <FormControl label="Language">
            <Controller
              name="language"
              control={control}
              render={({ field }) => (
                <Select
                  required
                  id="language-select"
                  options={seeTrue.settings.languages.map((e) => ({
                    key: e.toUpperCase(),
                    value: e,
                  }))}
                  labelKey="key"
                  valueKey="value"
                  onChange={(e) => field.onChange(e.value)}
                  value={field.value}
                />
              )}
            />
          </FormControl>
          <FormControl label="User meta data(JSON)">
            <Controller
              name="userMetaData"
              control={control}
              render={({ field }) => (
                <Textarea
                  onChange={field.onChange}
                  value={field.value}
                  clearOnEscape
                />
              )}
            />
          </FormControl>
          <FormControl>
            <Controller
              name="confirmed"
              control={control}
              render={({ field }) => (
                <Checkbox
                  onChange={field.onChange}
                  checked={field.value}
                  labelPlacement={LABEL_PLACEMENT.right}
                >
                  Confirmed
                </Checkbox>
              )}
            />
          </FormControl>
          <Button $style={{ width: "100%" }} type="submit">
            Submit
          </Button>
        </form>
      </Cell>
    </Grid>
  );
};
