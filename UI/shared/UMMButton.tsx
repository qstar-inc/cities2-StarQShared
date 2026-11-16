import { useValue } from "cs2/api";
import type { ValueBinding } from "cs2/api"; // adjust to your binding type
import { Button, FOCUS_DISABLED } from "cs2/ui";

export interface UMMButtonProps {
  id: string;
  /** Binding that reports whether the panel is open */
  selected: boolean;
  /** Called when the button is selected; the mod decides what that does */
  onToggle: () => void;
  icon: string;
  tooltip: string;
  variant?: "floating" | "default" | "icon" | "flat" | "round";
  className?: string;
}

export const UMMButton = ({
  id,
  selected,
  onToggle,
  icon,
  tooltip,
  variant = "floating",
  className,
}: UMMButtonProps) => {
  return (
    <Button
      id={id}
      variant={variant}
      className={className}
      selected={selected}
      onSelect={onToggle}
      src={icon}
      focusKey={FOCUS_DISABLED}
      tooltipLabel={tooltip}
    />
  );
};
