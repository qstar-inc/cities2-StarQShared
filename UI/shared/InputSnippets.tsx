import { Unit } from "cs2/l10n";
import { useEffect, useRef, useState } from "react";
import { clamp, roundToStep } from "shared/helpers/math";
import {
    infoRowModule, sipTextInputModule, textElipsisInputModule, textElipsisInputThemeModule
} from "shared/style";
import { LocalizedNumber } from "shared/vanilla-type-fix";
import commonStyle from "styles/common.module.scss";

export const Switch = ({
  checked = false,
  onChange,
  onClick,
  style = {},
  disabled = false,
}: {
  checked?: boolean;
  onChange?: (value: boolean) => void;
  onClick?: (event: React.MouseEvent<HTMLButtonElement>) => void;
  style?: React.CSSProperties;
  disabled?: boolean;
}) => {
  const toggle = (event: React.MouseEvent<HTMLButtonElement>) => {
    onClick?.(event);

    if (disabled) return;
    onChange?.(!checked);
  };

  return (
    <button
      type="button"
      className={`${commonStyle.ToggleSwitchCustom} ${checked ? `${commonStyle.on}` : ""} ${disabled ? `${commonStyle.disabled}` : ""}`}
      style={style}
      onClick={toggle}
      aria-checked={checked}
      role="switch"
    >
      <span className={`${commonStyle.ToggleSwitchCustomCircle}`} />
    </button>
  );
};

const DRAG_THRESHOLD_PX = 3;

export const NumberInputSnippet = ({
  value,
  inputPrefix,
  onCommit,
  allowDrag = true,
  unit = Unit.FloatThreeFractions,
  integer = false,
  step = 0.01,
  min,
  max,
}: {
  value: number;
  inputPrefix?: string;
  onCommit: (newValue: number) => void;
  allowDrag?: boolean;
  unit?: Unit;
  integer?: boolean;
  step?: number;
  min?: number;
  max?: number;
}) => {
  const [editValue, setEditValue] = useState<string>(`${value}`);
  const [dragging, setDragging] = useState(false);
  const inputRef = useRef<HTMLInputElement>(null);
  const allowed = integer ? /^-?\d*$/ : /^-?\d*\.?\d*$/;

  const latest = useRef({ value, onCommit, step, min, max, integer });
  latest.current = { value, onCommit, step, min, max, integer };

  useEffect(() => {
    setEditValue(`${value}`);
  }, [value]);

  const applyIfChanged = () => {
    const parsed = Number.parseFloat(editValue);

    if (!Number.isFinite(parsed)) {
      setEditValue(`${value}`);
      return;
    }

    const next = clamp(integer ? Math.round(parsed) : parsed, min, max);

    if (Math.abs(next - value) > 1e-6) {
      onCommit(next);
    } else {
      setEditValue(`${value}`);
    }
  };

  const onMouseDown = (e: React.MouseEvent) => {
    if (!allowDrag) return;
    if (e.button !== 0) return;
    if (document.activeElement === inputRef.current) return;
    e.preventDefault();

    const startX = e.clientX;
    const startValue = latest.current.value;
    let moved = false;
    let lastSent = startValue;

    const onMove = (ev: MouseEvent) => {
      const dx = ev.clientX - startX;

      if (!moved) {
        if (Math.abs(dx) < DRAG_THRESHOLD_PX) return;
        moved = true;
        setDragging(true);
      }

      const { step, min, max, onCommit, integer } = latest.current;
      const multiplier = ev.shiftKey ? 0.1 : ev.ctrlKey || ev.altKey ? 10 : 1;

      let next = startValue + dx * step * multiplier;
      next = integer
        ? Math.round(next)
        : roundToStep(next, step * (ev.shiftKey ? 0.1 : 1));
      next = clamp(next, min, max);

      if (Math.abs(next - lastSent) > 1e-9) {
        lastSent = next;
        setEditValue(`${next}`);
        onCommit(next);
      }
    };

    const onUp = () => {
      window.removeEventListener("mousemove", onMove);
      window.removeEventListener("mouseup", onUp);
      setDragging(false);

      if (!moved) {
        inputRef.current?.focus();
        inputRef.current?.select();
      }
    };

    window.addEventListener("mousemove", onMove);
    window.addEventListener("mouseup", onUp);
  };

  return (
    <>
      <div style={{ width: "100rem" }}>
        <div className={textElipsisInputThemeModule.wrapper}>
          <div
            className={`${textElipsisInputModule.container} ${sipTextInputModule.container} ${commonStyle.NoMarginSide} ${commonStyle.InputBoxCustom}`}
            style={{
              height: "32rem",
              cursor: allowDrag ? "ew-resize" : undefined,
              userSelect: allowDrag ? "none" : undefined,
            }}
            onMouseDown={onMouseDown}
          >
            <input
              ref={inputRef}
              className={`${commonStyle.Input} ${allowDrag ? commonStyle.InputDrag : ""}${allowDrag && dragging ? commonStyle.InputDragging : ""} ${textElipsisInputModule.input} ${sipTextInputModule.input}`}
              maxLength={10}
              type="text"
              value={editValue}
              onChange={e => {
                const v = e.currentTarget.value;
                if (allowed.test(v)) setEditValue(v);
              }}
              onKeyDown={e => {
                if (e.key === "Enter") {
                  e.currentTarget.blur();
                }
              }}
              onBlur={applyIfChanged}
            />
            <div
              className={`${infoRowModule.right} ${textElipsisInputModule.label} ${sipTextInputModule.label} ${commonStyle.InputBoxCustom}`}
            >
              {inputPrefix}{" "}
              {
                <LocalizedNumber
                  value={value}
                  unit={Unit.FloatThreeFractions}
                />
              }
            </div>
          </div>
        </div>
      </div>
    </>
  );
};
