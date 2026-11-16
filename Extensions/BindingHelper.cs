using System.Reflection;

namespace StarQ.Shared.Extensions
{
    public static class BindingHelper
    {
        public const BindingFlags All =
            BindingFlags.Public
            | BindingFlags.NonPublic
            | BindingFlags.Instance
            | BindingFlags.Static;
    }
}
