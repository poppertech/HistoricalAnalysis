using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;

namespace HistoricalAnalysis
{
    class Program
    {
        static void Main(string[] args)
        {
            var downloadContext = new DownloadContext
            {
                StartDate = "1262217600",
                EndDate = "2000000000",
                StockTicker = "UNH",
                DownloadDirectory = @"C:\Users\bwynn\Desktop\HistoricalAnalysis\RawData",
                ReturnsDirectory = @"C:\Users\bwynn\Desktop\HistoricalAnalysis\ReturnsData",
                BaseQueryUri = new Uri(@"https://query1.finance.yahoo.com/v7/finance/download/")
            };
            //DeleteExistingRawData(downloadContext.DownloadDirectory);
            //DownloadRawData(downloadContext);
            ReturnsFilesWriter.CreateReturnsFiles(downloadContext);
        }

        public static void DeleteExistingRawData(string downloadDirectory)
        {
            var directoryInfo = new DirectoryInfo(downloadDirectory);

            Array.ForEach(directoryInfo.GetFiles(), (fileInfo) => fileInfo.Delete());
            
        }

        public static void DownloadRawData(DownloadContext ctx)
        {
            var fileName = $@"{ctx.DownloadDirectory}\{ctx.StockTicker}.csv";
            var relativeUri = $"{ctx.StockTicker}?period1={ctx.StartDate}&period2={ctx.EndDate}&interval=1d&events=history";
            var url = new Uri(ctx.BaseQueryUri, relativeUri);
            var webClient = new WebClient();
            webClient.DownloadFile(url, fileName);
        }
    }

    public static class ReturnsFilesWriter
    {
        public static void CreateReturnsFiles(DownloadContext ctx)
        {
            var directoryInfo = new DirectoryInfo(ctx.DownloadDirectory);
            var fileInfos = directoryInfo.GetFiles();

            foreach (var fileInfo in fileInfos)
            {
                var returnsFileContent = CreateReturnsFileContent(fileInfo);
                var fileName = Path.GetFileNameWithoutExtension(fileInfo.Name);
                var returnsFileName = $"{fileName}Returns{fileInfo.Extension}";
                var returnsFilePath = Path.Combine(ctx.ReturnsDirectory, returnsFileName);
                File.WriteAllText(returnsFilePath, returnsFileContent);
            }

        }

        private static string CreateReturnsFileContent(FileInfo fileInfo)
        {
            const int firstLine = 1;
            var stringBuilder = new StringBuilder();
            var lines = File.ReadAllLines(fileInfo.FullName);

            decimal lastClose = 0;
            for (int cnt = firstLine; cnt < lines.Length; cnt++)
            {
                var line = lines[cnt];
                var splitLine = line.Split(",");
                var date = ParseDate(splitLine);
                var adjustedClose = ParseAdjustedClose(splitLine);
                if (cnt == firstLine && date.HasValue && adjustedClose.HasValue)
                {
                    lastClose = adjustedClose.Value;
                }
                else if (cnt > firstLine && date.HasValue && adjustedClose.HasValue && lastClose != adjustedClose)
                {
                    var totalReturn = adjustedClose / lastClose - 1;
                    lastClose = adjustedClose.Value;
                    stringBuilder.AppendLine(string.Join(",", date.Value.ToString("MM/dd/yyyy", DateTimeFormatInfo.InvariantInfo), totalReturn));
                }
            }

            return stringBuilder.ToString();
        }

        private static DateTime? ParseDate(string[] splitLine)
        {
            var isParsedDate = DateTime.TryParse(splitLine[0], out DateTime date);
            if (isParsedDate && date > DateTime.MinValue)
                return date;

            return null;
        }

        private static decimal? ParseAdjustedClose(string[] splitLine)
        {
            var isParsedAdjustedClose = decimal.TryParse(splitLine[5], out decimal adjustedClose);
            if (isParsedAdjustedClose && adjustedClose > 0)
                return adjustedClose;

            return null;
        }
    }

    public class DownloadContext
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string StockTicker { get; set; }
        public string DownloadDirectory { get; set; }
        public string ReturnsDirectory { get; set; }
        public Uri BaseQueryUri { get; set; }
    }

}
