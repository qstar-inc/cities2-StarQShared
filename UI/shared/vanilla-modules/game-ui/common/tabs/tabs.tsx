import { ReactNode } from "react";
import { getModule } from "cs2/modding";

export interface TabProps {
  id: string;
  selectedId: string | null | undefined;
  onSelect: (id: string) => void;
  uiTag?: string;
  disabled?: boolean;
  locked?: boolean;
  selectSound?: unknown;
  className?: string;
  children?: ReactNode;
}

export interface TabBarProps {
  className?: string;
  children?: ReactNode;
}

export interface TabNavProps {
  tabs: string[];
  selectedTab: string | null | undefined;
  onSelect: (id: string) => void;
  selectPreviousSound?: unknown;
  selectNextSound?: unknown;
  children?: ReactNode;
}

const path = "game-ui/common/tabs/tabs.tsx";

export const TabNav = getModule(path, "TabNav") as React.FC<TabNavProps>;
export const TabBar = getModule(path, "TabBar") as React.FC<TabBarProps>;
export const Tab = getModule(path, "Tab") as React.FC<TabProps>;
