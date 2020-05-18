using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace HistoricalAnalysis
{
    class Program
    {
        static void Main(string[] args)
        {
            //DeleteExistingData();
            //DownloadRawData();
            //ReturnsFilesWriter.CreateReturnsFiles();
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

        public static void CreateParentIntervalsFiles()
        {
            var returnsDirectory = Config.SubDirectories[1];
            var parentIntervalsDirectory = Config.SubDirectories[2];
            var parentFiles = returnsDirectory.GetFiles($"Config.ParentTicker*");
            var parentFile = parentFiles[0];
            var childFiles = returnsDirectory.GetFiles().Except(parentFiles);

            var parentDictionary = new Dictionary<DateTime, decimal>();
            var parentLines = File.ReadAllLines(parentFile.FullName);
            foreach (var line in parentLines)
            {
                var splitLine = line.Split(",");
            }



        }
    }

}
