import { Unit } from "cs2/l10n";
import { useEffect, useState } from "react";
import {
  infoRowModule,
  sipTextInputModule,
  textElipsisInputModule,
  textElipsisInputThemeModule,
} from "shared/style";
import { LocalizedNumber } from "shared/vanilla-type-fix";
import commonStyle from "styles/common.module.scss";

export const Switch = ({
  checked = false,
  onChange,
  disabled = false,
}: {
  checked?: boolean;
  onChange?: (value: boolean) => void;
  disabled?: boolean;
}) => {
  const toggle = () => {
    if (disabled) return;
    onChange?.(!checked);
  };

  return (
    <button
      type="button"
      className={`${commonStyle.ToggleSwitchCustom} ${checked ? `${commonStyle.on}` : ""} ${disabled ? `${commonStyle.disabled}` : ""}`}
      onClick={toggle}
      aria-checked={checked}
      role="switch"
    >
      <span className={`${commonStyle.ToggleSwitchCustomCircle}`} />
    </button>
  );
};

export const NumberInputSnippet = ({
  value,
  inputPrefix,
  onCommit,
}: {
  value: number;
  inputPrefix?: string;
  onCommit: (newValue: number) => void;
}) => {
  const [editValue, setEditValue] = useState<string>(`${value}`);

  useEffect(() => {
    setEditValue(`${value}`);
  }, [value]);

  const applyIfChanged = () => {
    const parsed = Number.parseFloat(editValue);

    if (!Number.isNaN(parsed) && Math.abs(parsed - value) > 1e-6) {
      if (Number.isFinite(parsed)) onCommit(parsed);
    }
  };
  return (
    <>
      <div style={{ width: "100rem" }}>
        <div className={textElipsisInputThemeModule.wrapper}>
          <div
            className={`${textElipsisInputModule.container} ${sipTextInputModule.container} ${commonStyle.NoMarginSide} ${commonStyle.InputBoxCustom}`}
            style={{ height: "32rem" }}
          >
            <input
              className={`${textElipsisInputModule.input} ${sipTextInputModule.input}`}
              maxLength={10}
              type="text"
              value={editValue}
              style={{ textAlign: "right" }}
              onChange={e => setEditValue(e.currentTarget.value)}
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
