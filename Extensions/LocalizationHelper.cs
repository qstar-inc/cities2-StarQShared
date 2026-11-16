namespace StarQ.Shared.Extensions
{
    public static class LocalizationHelper
    {
        public enum LocaleIdGettable
        {
            OK,
            Cancel,
            Close,
            Yes,
            No,
        }

        public static string GetLocaleId(LocaleIdGettable gettable)
        {
            return gettable switch
            {
                LocaleIdGettable.OK => "Common.OK",
                LocaleIdGettable.Cancel => "Common.CANCEL",
                LocaleIdGettable.Close => "Common.CLOSE",
                LocaleIdGettable.Yes => "Common.YES",
                LocaleIdGettable.No => "Common.NO",
                _ => gettable.ToString(),
            };
        }
    }
}
