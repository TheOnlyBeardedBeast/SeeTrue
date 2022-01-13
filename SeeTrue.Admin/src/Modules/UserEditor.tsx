import React from "react";
import { FormControl } from "baseui/form-control";
import { Input } from "baseui/input";
import { Select } from "baseui/select";
import { Textarea } from "baseui/textarea";
import { Checkbox, LABEL_PLACEMENT } from "baseui/checkbox";
import { Button } from "baseui/button";
import { Grid, Cell } from "baseui/layout-grid";

export const UserEditor: React.FC = () => {
  return (
    <Grid>
      <Cell span={[12, 6, 6]}>
        <form>
          <FormControl label="Email">
            <Input placeholder="Controlled Input" clearOnEscape />
          </FormControl>
          <FormControl label="Password">
            <Input placeholder="Controlled Input" clearOnEscape />
          </FormControl>
          <FormControl label="Role">
            <Select
              options={[
                { id: "AliceBlue", color: "#F0F8FF" },
                { id: "AntiqueWhite", color: "#FAEBD7" },
                { id: "Aqua", color: "#00FFFF" },
                { id: "Aquamarine", color: "#7FFFD4" },
                { id: "Azure", color: "#F0FFFF" },
                { id: "Beige", color: "#F5F5DC" },
              ]}
              labelKey="id"
              valueKey="color"
            />
          </FormControl>
          <FormControl label="User meta data(JSON)">
            <Textarea />
          </FormControl>
          <FormControl>
            <Checkbox labelPlacement={LABEL_PLACEMENT.right}>
              Confirmed
            </Checkbox>
          </FormControl>
          <Button $style={{ width: "100%" }} type="submit">
            Submit
          </Button>
        </form>
      </Cell>
    </Grid>
  );
};
