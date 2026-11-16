import { getModule } from "cs2/modding";
import { FC } from "react";

export const GlossaryPanel = getModule(
  "game-ui/game/components/glossary-panel/glossary-panel.tsx",
  "GlossaryPanel",
) as FC<any>;

export const GlossarySearchTheme = getModule(
  "game-ui/editor/widgets/search-field/themes/search-field-glossary.module.scss",
  "classes",
) as Record<string, string>;
