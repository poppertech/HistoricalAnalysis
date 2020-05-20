using System;
using System.Net;

namespace HistoricalAnalysis
{
    class Program
    {
        static void Main(string[] args)
        {
            //DeleteExistingData();
            //DownloadRawData();
            //ReturnsFilesWriter.CreateReturnsFiles();
            var returnsDictionaries = ReturnsFileReader.CreateReturnsDictionaries();
        }

        public static void DeleteExistingData()
        {
            foreach (var directoryInfo in Config.SubDirectories)
                Array.ForEach(directoryInfo.GetFiles(), (fileInfo) => fileInfo.Delete());

        }

        public static void DownloadRawData()
        {
            var directory = Config.SubDirectories[0];
            foreach (var stockTicker in Config.StockTickers)
            {
                var fileName = $@"{directory.FullName}\{stockTicker}.csv";
                var relativeUri = $"{stockTicker}?period1={Config.StartDate}&period2={Config.EndDate}&interval=1d&events=history";
                var url = new Uri(Config.BaseQueryUri, relativeUri);
                using (var webClient = new WebClient())
                {
                    webClient.DownloadFile(url, fileName);
                }
            }
        }

    }

}
