import { ReactNode, Context } from "react";
import { getModule } from "cs2/modding";

export interface SectionTheme {
  section: string;
  header: string;
  content: string;
  locked: string;
  first: string;
  last: string;
  "title-bar": string;
  titleBar: string;
  title: string;
  "toggle-button": string;
  toggleButton: string;
}

export interface SectionContextValue {
  theme: SectionTheme;
  expanded: boolean;
}

export interface SectionProps {
  focusKey?: unknown;
  header?: ReactNode;
  tooltip?: ReactNode;
  theme?: Partial<SectionTheme>;
  initialFocused?: unknown;
  direction?: unknown;
  className?: string;
  children?: ReactNode;
  onFocusChange?: (focused: boolean) => void;
  onRefocus?: () => void;
}

const sectionPath = "game-ui/common/section/section.tsx";
const contextPath = "game-ui/common/section/section-context.ts";
const themePath = "game-ui/common/section/themes/panel-section.module.scss";

export const Section = getModule(
  sectionPath,
  "Section",
) as React.FC<SectionProps>;

export const SectionContext = getModule(
  contextPath,
  "SectionContext",
) as Context<SectionContextValue>;

export const defaultSectionContext = getModule(
  contextPath,
  "defaultSectionContext",
) as SectionContextValue;

export const panelSectionTheme = getModule(
  themePath,
  "classes",
) as SectionTheme;
