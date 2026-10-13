import { FOCUS_AUTO } from "cs2/input";
import { FC, ReactElement, useState } from "react";
import {
  panelThemes,
  styleEditorHierarcy as styleEditorHierarchy,
  styleEditorPanel,
} from "shared/style";
import { CollapsiblePanel } from "shared/vanilla";
import workspaceStyle from "styles/workspace.module.scss";

interface EditorWorkspaceBaseProps {
  id: string;
  title: string;
  visible: boolean;
  content: ReactElement;
  footer?: ReactElement;
  icon?: string;
  left?: number;
  posX?: number;
  posY?: number;
  sipAligned?: boolean;
  onClose?: () => void;
}

export const EditorWorkspaceBase: FC<EditorWorkspaceBaseProps> = (
  props: EditorWorkspaceBaseProps,
) => {
  const [expanded, setExpanded] = useState(true);

  if (!props.visible) return null;

  return (
    <CollapsiblePanel
      className={`${workspaceStyle.EditorPanel} ${styleEditorPanel.panel} ${styleEditorHierarchy.panel}`}
      theme={panelThemes.editor}
      headerText={props.title}
      expanded={expanded}
      onToggleExpanded={setExpanded}
      focusKey={FOCUS_AUTO}
    >
      {props.content}
    </CollapsiblePanel>
  );
};
