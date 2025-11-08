using System.Globalization;

namespace StarQ.Shared.Extensions
{
    public class MathHelper
    {
        private static readonly CultureInfo Inv = CultureInfo.InvariantCulture;

        public enum NumberFormat
        {
            TwoDPMax,
            ThreeDPMax,
            TwoDP,
            ThreeDP,
        }

        public static string ConvertToString(
            float value,
            NumberFormat numberFormat = NumberFormat.TwoDPMax
        ) =>
            numberFormat switch
            {
                NumberFormat.TwoDPMax => value.ToString("#,0.##", Inv),
                NumberFormat.ThreeDPMax => value.ToString("#,0.###", Inv),
                NumberFormat.TwoDP => value.ToString("#,0.00", Inv),
                NumberFormat.ThreeDP => value.ToString("#,0.000", Inv),
                _ => value.ToString(),
            };

        public static string ConvertToString(
            int value,
            NumberFormat numberFormat = NumberFormat.TwoDPMax
        ) => ConvertToString((float)value, numberFormat);
    }
}
