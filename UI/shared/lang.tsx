import { Unit } from "cs2/l10n";
import mod from "mod.json";
import { LocalizedNumber } from "shared/vanilla-type-fix";

let _translate: ((key: string) => string) | null = null;

export function initTranslationService(
  translate: (id: string, fallback?: string | null) => string | null,
) {
  _translate = (key: string) => translate(key) ?? key;
}

export function GetAssetName(name: string) {
  const tr = FindTranslation(`Assets.NAME[${name}]`, true);
  if (tr.startsWith("Assets.NAME[")) return name;
  return tr;
}

export function FindTranslation(str: string, vanilla: boolean = false) {
  if (!_translate) {
    return str;
  }
  if (vanilla) return _translate(str) ?? `${str}`;
  return _translate(`${mod.id}.${str}`) ?? `${str}`;
}

export function nicifyVariableName(name?: string | null): string {
  if (!name) return "";

  if (name.startsWith("m_")) {
    name = name.substring(2);
  } else if (name.startsWith("Get")) {
    name = name.substring(3);
  }

  name = name.replace(/\B([A-Z][a-z])/g, " $1");
  name = name.replace(/([^A-Z\s])([A-Z])/g, "$1 $2");
  name = name.replace(/(?<![\d\s]|\dx|\d-)(\d)/g, " $1");
  name = name.replace(/^([a-z])/, m => m.toUpperCase());

  return name;
}

export function formatNumber(value: number) {
  if (Number.isInteger(value)) return value;
  return Number(value.toFixed(2));
}

export function localizedNumber(value: number, unit: Unit = Unit.Integer) {
  return <LocalizedNumber value={formatNumber(value)} unit={unit} />;
}

export function localizedFloat2Number(value: number) {
  if (Number.isInteger(value)) {
    return localizedNumber(value);
  }
  return localizedNumber(value, Unit.FloatTwoFractions);
}
