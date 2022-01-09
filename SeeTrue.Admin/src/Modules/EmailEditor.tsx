import React from "react";
import Editor from "@monaco-editor/react";
// @ts-ignore
import mjml2html from "mjml-browser";
import debounce from "lodash.debounce";
import { Tabs, Tab } from "baseui/tabs-motion";
import { toaster } from "baseui/toast";
import { FormControl } from "baseui/form-control";
import { Select } from "baseui/select";
import { Textarea } from "baseui/textarea";
import { useForm, Controller } from "react-hook-form";

const iFrameContent = `<html><head><script type="module"> window.addEventListener('message', (event)=>{const{type, value}=event.data; if (type==='html'){document.body.innerHTML=value;}})</script></head><body></body></html>`;

const devValue = `<mjml>
<mj-body>
  <mj-section>
    <mj-column>
      <mj-text font-size="25px" color="#000000" font-weight="bold" font-family="roboto">SeeTrue</mj-text>

    </mj-column>
    
  </mj-section>
  <mj-section>
    <mj-column>
      <mj-text align="center" font-size="20px" color="#000000" font-family="roboto" line-height="1.5"><p>Hi <strong>{{name}}</strong>,<br/> please confirm your registration.<br/></p></mj-text>
      <mj-button href="https://frontendurl.com/confirm{{token}}" background-color="#ffffff" color="#000000" font-size="20px" border="2px solid #000000">Confirm</mj-button>
    </mj-column>
  </mj-section>
  <mj-section>
    <mj-column>
      <mj-divider border-width="1px"/>
      <mj-text mj-class="footer-text" align="center" padding="30px 20px">
        <p style="Margin:0; padding-bottom:10px; font-size:10px; line-height:15px; Margin-bottom:10px; color:#111111; font-family: 'Open Sans', 'Raleway', Arial, Helvetica, sans-serif;">SeeTrue is an authentication service used by companies and developers.
        </p>
        <mj-social font-size="15px" icon-size="30px" mode="horizontal">
        <mj-social-element name="facebook" href="https://mjml.io/">
          Facebook
        </mj-social-element>
          .
        <mj-social-element name="google" href="https://mjml.io/">
          Google
        </mj-social-element>
          .
        <mj-social-element  name="twitter" href="https://mjml.io/">
          Twitter
        </mj-social-element>
      </mj-social>
      </mj-text>
    </mj-column>
  </mj-section>
</mj-body>
</mjml>`;

const tabsOverrides = {
  Root: {
    style: () => ({
      paddingTop: 0,
      paddingLeft: 0,
      paddingRight: 0,
      paddingBottom: 0,
      height: "100%",
    }),
  },
};

const tabOverrides = {
  TabPanel: {
    style: {
      paddingTop: 0,
      paddingLeft: 0,
      paddingRight: 0,
      paddingBottom: 0,
      height: "100%",
    },
  },
};

export const EmailEditor: React.FC = () => {
  const contentRef = React.useRef<HTMLIFrameElement | null>(null);
  const [activeTab, setActiveTab] = React.useState<React.Key>(0);

  const { register, control } = useForm();

  const contentRefHandler = React.useCallback((node: HTMLIFrameElement) => {
    contentRef.current = node;
    handleChange();
  }, []);

  const editorRef = React.useRef<any>(null);

  const handleEditorDidMount = (editor: any, _monaco: any) => {
    editorRef.current = editor;
    handleChange();
  };

  const htmlRef = React.useRef<any>(null);

  const handleHtmlDidMount = (editor: any, _monaco: any) => {
    htmlRef.current = editor;
    handleChange();
  };

  const handleChange = () => {
    try {
      const html = {
        type: "html",
        value: editorRef?.current?.getValue()
          ? mjml2html(editorRef?.current?.getValue()).html
          : "",
      };

      contentRef?.current?.contentWindow?.postMessage(html, "*");
      if (htmlRef?.current) {
        htmlRef.current.setValue(html.value);
      }
    } catch (error) {
      toaster.negative(<>Invalid mjml value!</>, {});
    }
  };

  const debounceChange = React.useCallback(debounce(handleChange, 500), []);

  const handleTabChange = ({ activeKey }: { activeKey: React.Key }) => {
    setActiveTab(activeKey);
  };

  return (
    <div style={{ display: "flex", height: "100%", overflow: "hidden" }}>
      <Editor
        height="100%"
        width="50%"
        onChange={debounceChange}
        defaultLanguage="html"
        defaultValue={devValue}
        theme="hc-black"
        onMount={handleEditorDidMount}
      />
      <div style={{ height: "100%", width: "50%" }}>
        <Tabs
          activeKey={activeTab}
          onChange={handleTabChange}
          overrides={tabsOverrides}
        >
          <Tab title="Settings" overrides={tabOverrides}>
            <form>
              <FormControl label="Audience">
                <Controller
                  name="audience"
                  control={control}
                  render={({ field }) => (
                    <Select
                      id="select-id"
                      options={[{ key: "http://localhost:5000" }]}
                      labelKey="key"
                      valueKey="key"
                      onChange={(e) => field.onChange(e.value)}
                      value={field.value}
                    />
                  )}
                />
              </FormControl>
              <FormControl label="Subject">
                <Textarea placeholder="Controlled Input" clearOnEscape />
              </FormControl>
              <FormControl label="Type">
                <Controller
                  name="type"
                  control={control}
                  render={({ field }) => (
                    <Select
                      id="select-id"
                      options={[{ key: 0, type: "Confirmation" }]}
                      labelKey="type"
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
                      id="select-id"
                      options={[{ key: "EN", language: "English" }]}
                      labelKey="language"
                      valueKey="key"
                      onChange={(e) => field.onChange(e.value)}
                      value={field.value}
                    />
                  )}
                />
              </FormControl>
            </form>
          </Tab>
          <Tab title="Render" overrides={tabOverrides}>
            {/* Big thanks https://joyofcode.xyz/avoid-flashing-iframe */}
            <iframe
              onLoad={handleChange}
              ref={contentRefHandler}
              style={{ height: "100%", width: "100%", background: "#fff" }}
              srcDoc={iFrameContent}
            />
          </Tab>
          <Tab title="HTML (readonly)" overrides={tabOverrides}>
            <Editor
              height="100%"
              width="100%"
              defaultLanguage="html"
              theme="hc-black"
              onMount={handleHtmlDidMount}
              options={{
                readOnly: true,
              }}
            />
          </Tab>
        </Tabs>
      </div>
    </div>
  );
};
