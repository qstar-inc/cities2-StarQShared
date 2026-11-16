export const clamp = (v: number, min?: number, max?: number) => {
  if (min != null && v < min) return min;
  if (max != null && v > max) return max;
  return v;
};

export const roundToStep = (v: number, step: number) => {
  if (step <= 0) return v;
  const decimals = Math.min(6, Math.max(0, Math.ceil(-Math.log10(step))));
  return Number((Math.round(v / step) * step).toFixed(decimals));
};
