import React from "react";
import Editor from "@monaco-editor/react";
// @ts-ignore
import mjml2html from "mjml-browser";
import debounce from "lodash.debounce";

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

export const EmailEditor: React.FC = () => {
  const contentRef = React.useRef<HTMLIFrameElement | null>(null);

  const contentRefHandler = React.useCallback((node: HTMLIFrameElement) => {
    contentRef.current = node;
    handleChange();
  }, []);

  const editorRef = React.useRef<any>(null);

  const handleEditorDidMount = (editor: any, _monaco: any) => {
    editorRef.current = editor;
    handleChange();
  };

  const handleChange = () => {
    const html = {
      type: "html",
      value: editorRef?.current?.getValue()
        ? mjml2html(editorRef?.current?.getValue()).html
        : "",
    };

    contentRef?.current?.contentWindow?.postMessage(html, "*");
  };

  const debounceChange = React.useCallback(debounce(handleChange, 500), []);

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
      {/* Big thanks https://joyofcode.xyz/avoid-flashing-iframe */}
      <iframe
        onLoad={handleChange}
        ref={contentRefHandler}
        style={{ height: "100%", width: "50%", background: "#fff" }}
        srcDoc={iFrameContent}
      />
    </div>
  );
};
