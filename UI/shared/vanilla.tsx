import { ToggleField, UISound } from "cs2/bindings";
import { StyleProps } from "cs2/input";
import { getModule } from "cs2/modding";
import { ButtonProps, FocusKey, ScrollController } from "cs2/ui";
import { ReactElement, ReactNode } from "react";

interface InfoButtonProps extends ButtonProps {
  label: string;
  icon?: string;
}

export const InfoButton = getModule(
  "game-ui/game/components/selected-info-panel/shared-components/info-button/info-button.tsx",
  "InfoButton",
) as React.FC<InfoButtonProps>;

// export const A = getModule(
//   "game-ui/common/localization/loc.generated.ts",
//   "Loc"
// );

// console.log(A.SelectedInfoPanel.RAW_MATERIALS);

interface ToolButtonProps extends ButtonProps {
  src: string;
  tooltip?: string;
}

export const ToolButton = getModule(
  "game-ui/game/components/tool-options/tool-button/tool-button.tsx",
  "ToolButton",
) as React.FC<ToolButtonProps>;

export const Divider: any = getModule(
  "game-ui/editor/widgets/divider/divider.tsx",
  "Divider",
);

export type SizeProvider = {
  getRenderedRange: () => {
    offset: number;
    size: number;
    startIndex: number;
    endIndex: number;
  };
  getTotalSize: () => number;
};

export type RenderItemFn = (
  itemIndex: number,
  indexInRange: number,
) => ReactElement | null;
type RenderedRangeChangedCallback = (
  startIndex: number,
  endIndex: number,
) => void;

interface VirtualListProps {
  className?: string;
  controller?: ScrollController;
  direction?: "vertical" | "horizontal";
  onRenderedRangeChange?: RenderedRangeChangedCallback;
  renderItem: RenderItemFn;
  sizeProvider: SizeProvider;
  smooth?: boolean;
  style?: Partial<CSSStyleDeclaration>;
}

export const VanillaVirtualList = getModule(
  "game-ui/common/scrolling/virtual-list/virtual-list.tsx",
  "VirtualList",
) as React.FC<VirtualListProps>;

export const useUniformSizeProvider: (
  height: number,
  visible: number,
  extents: number,
) => SizeProvider = getModule(
  "game-ui/common/scrolling/virtual-list/virtual-list-size-provider.ts",
  "useUniformSizeProvider",
);

export const BooleanInput = getModule(
  "game-ui/game/widgets/toggle-field/toggle-field.tsx",
  "BoundToggleField",
) as React.FC<ToggleField>;

interface ToggleProps extends StyleProps {
  focusKey?: FocusKey;
  debugName?: string;
  checked?: boolean;
  disabled?: boolean;
  toggleSound?: UISound | string | null;
  children?: any;
  showHint?: boolean;
  multistate?: boolean;
  onChange?: () => void;
  onMultistateChange?: () => void;
  onMouseOver?: () => void;
  onMouseLeave?: () => void;
}

interface CheckBoxProps extends ToggleProps {
  theme?: any;
}

export const CheckBox = getModule(
  "game-ui/common/input/toggle/checkbox/checkbox.tsx",
  "Checkbox",
) as React.FC<CheckBoxProps>;

export const IntSliderField = getModule(
  "game-ui/editor/widgets/fields/number-slider-field.tsx",
  "IntSliderField",
);

export const IntSlider = getModule(
  "game-ui/common/input/slider/slider.tsx",
  "Slider",
);

export const IntTransformer = getModule(
  "game-ui/common/input/slider/slider.tsx",
  "intTransformer",
);

export const DropdownMultiSection = getModule(
  "game-ui/common/input/dropdown/dropdown.tsx",
  "Dropdown",
);

export const DragHandle = getModule(
  "game-ui/common/input/drag-handle.tsx",
  "DragHandle",
);

interface ResizeContainerProps {
  size: number;
  minSize: number;
  maxSize: number;
  direction: "up" | "down" | "left" | "right" | "none";
  className?: string;
  dragHandleClassName?: string;
  children?: ReactNode;
  onDrag?: (size: number) => void;
  onResize: (size: number) => void;
}

export const ResizeContainer = getModule(
  "game-ui/editor/components/resize-container/resize-container.tsx",
  "ResizeContainer",
) as React.FC<ResizeContainerProps>;

interface CollapsiblePanelProps {
  theme?: object;
  onClose?: () => void;
  expanded?: boolean;
  headerText?: React.ReactNode;
  headerUiTag?: string;
  onToggleExpanded?: (expanded: boolean) => void;
  className?: string;
  children?: React.ReactNode;
  isFocusRoot?: boolean;
  headerIcon?: string | React.ReactNode;
  togglable?: boolean;

  // Additional props forwarded to the underlying panel component.
  [key: string]: unknown;
}

export const CollapsiblePanel = getModule(
  "game-ui/common/panel/collapsible-panel.tsx",
  "CollapsiblePanel",
) as React.FC<CollapsiblePanelProps>;
