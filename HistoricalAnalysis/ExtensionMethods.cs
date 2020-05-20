using System;
using System.Collections.Generic;
using System.Text;

namespace HistoricalAnalysis
{
    public static class ExtensionMethods
    {
        public static DateTime? ParseDate(this string dateString)
        {
            var isParsedDate = DateTime.TryParse(dateString, out DateTime date);
            if (isParsedDate && date > DateTime.MinValue)
                return date;

            return null;
        }

        public static decimal? ParseDecimal(this string decimalString, bool isZeroAllowed = false)
        {
            var isParsedDecimal = decimal.TryParse(decimalString, out decimal parsedDecimal);
            if (isParsedDecimal && (isZeroAllowed || parsedDecimal != 0))
                return parsedDecimal;

            return null;
        }
    }
}
