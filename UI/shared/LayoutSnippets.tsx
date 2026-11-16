import { ReactElement } from "react";
import commonStyle from "styles/common.module.scss";

export const ButtonGroup = ({
  width,
  style = {},
  content,
}: {
  width?: string;
  style?: React.CSSProperties;
  content: ReactElement;
}) => {
  return (
    <div
      className={`${commonStyle.ButtonGroup} ${commonStyle.MarginLeft3r}`}
      style={{
        ...style,
        width,
      }}
    >
      {content}
    </div>
  );
};
