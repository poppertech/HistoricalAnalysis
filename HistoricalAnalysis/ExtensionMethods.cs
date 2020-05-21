using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace HistoricalAnalysis
{
    public static class ExtensionMethods
    {
        public static void DeleteExistingData(this DirectoryInfo[] directoryInfos)
        {
            foreach (var directoryInfo in directoryInfos)
                Array.ForEach(directoryInfo.GetFiles(), (fileInfo) => fileInfo.Delete());
        }

        public static void DownloadRawData(this string[] fileNames)
        {
            foreach (var fileName in fileNames)
            {
                var filePath = $@"{Config.SubDirectories[0].FullName}\{fileName}.csv";
                var relativeUri = $"{fileName}?period1={Config.StartDate}&period2={Config.EndDate}&interval=1d&events=history";
                var url = new Uri(Config.BaseQueryUri, relativeUri);
                using (var webClient = new WebClient())
                {
                    webClient.DownloadFile(url, filePath);
                }
            }
        }

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
