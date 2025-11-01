using System.Globalization;

namespace StarQ.Shared.Extensions
{
    public class MathHelper
    {
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
        )
        {
            return numberFormat switch
            {
                NumberFormat.TwoDPMax => value.ToString("#,0.##", CultureInfo.InvariantCulture),
                NumberFormat.ThreeDPMax => value.ToString("#,0.###", CultureInfo.InvariantCulture),
                NumberFormat.TwoDP => value.ToString("#,0.00", CultureInfo.InvariantCulture),
                NumberFormat.ThreeDP => value.ToString("#,0.000", CultureInfo.InvariantCulture),
                _ => value.ToString(),
            };
        }

        public static string ConvertToString(
            int value,
            NumberFormat numberFormat = NumberFormat.TwoDPMax
        ) => ConvertToString((float)value, numberFormat);
    }
}
