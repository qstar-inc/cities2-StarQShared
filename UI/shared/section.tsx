export const SectionParams: Record<string, boolean> = {};

export function GetSectionOpen(key: string, inital: boolean) {
  if (!(key in SectionParams)) {
    SectionParams[key] = inital;
  }
  return SectionParams[key];
}

export function SetSectionOpen(key: string, value: boolean) {
  SectionParams[key] = value;
}
