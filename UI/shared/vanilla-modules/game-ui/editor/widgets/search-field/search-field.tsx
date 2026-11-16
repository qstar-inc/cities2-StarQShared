import { getModule } from "cs2/modding";
import { FC, ReactNode } from "react";

export const SearchField = getModule(
  "game-ui/editor/widgets/search-field/search-field.tsx",
  "SearchField",
) as FC<{
  value: string;
  onChange: (value: string) => void;
  theme?: Partial<Record<string, string>>;
  placeholder?: ReactNode;
}>;

export const SearchFieldTheme = getModule(
  "game-ui/editor/widgets/search-field/search-field.module.scss",
  "classes",
) as Record<string, string>;

export const BoundSearchField = getModule(
  "game-ui/editor/widgets/search-field/search-field.tsx",
  "BoundSearchField",
) as FC<any>;
