import { useLayoutEffect, useRef, useState } from "react";
import { createPortal } from "react-dom";

const HOST_ATTR = "data-starq-editor-left";
const MARKER_ATTR = "data-starq-editor-left-marker";
const STYLE_ID = "starq-editor-left-style";
const CSS = `
[${HOST_ATTR}] { display: flex; flex-direction: column; }
[${HOST_ATTR}] > :first-child.expanded  { height: auto !important; flex: 1 1 0; min-height: 0; }
[${HOST_ATTR}] > :first-child.collapsed { height: auto !important; flex: 0 0 auto; }
`;

const findRoot = (marker: Element | null): Element | null => {
  let el = marker?.nextElementSibling ?? null;
  while (el && el.hasAttribute(MARKER_ATTR)) el = el.nextElementSibling;
  return el;
};

export const EditorWorkspaceAppend =
  (New: any) =>
  (Original: any): any => {
    const Wrapped = (props: any) => {
      const markerRef = useRef<HTMLSpanElement>(null);
      const [inner, setInner] = useState<Element | null>(null);

      useLayoutEffect(() => {
        const el =
          findRoot(markerRef.current)?.firstElementChild?.firstElementChild ??
          null;
        if (!el) return;

        if (!document.getElementById(STYLE_ID)) {
          const s = document.createElement("style");
          s.id = STYLE_ID;
          s.textContent = CSS;
          document.head.appendChild(s);
        }
        el.setAttribute(HOST_ATTR, "");
        setInner(el);
      }, []);

      return (
        <>
          <span
            ref={markerRef}
            {...{ [MARKER_ATTR]: "" }}
            style={{ display: "none" }}
          />
          <Original {...props} />
          {inner && createPortal(<New />, inner)}
        </>
      );
    };
    return Wrapped;
  };
