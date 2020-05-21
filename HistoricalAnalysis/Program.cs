using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace HistoricalAnalysis
{
    class Program
    {
        static void Main(string[] args)
        {
            //Config.SubDirectories.DeleteExistingData();
            //Config.StockTickers.DownloadRawData();
            //ReturnsFilesWriter.CreateReturnsFiles();
            var returnsDictionaries = ReturnsFileReader.CreateReturnsDictionaries();
            CreateIntervals(returnsDictionaries);
        }

        public static void CreateIntervals(Dictionary<string, Dictionary<DateTime, decimal>> returnsDictionaries)
        {
            foreach (var ticker in returnsDictionaries.Keys)
            {
                var returnsDictionary = returnsDictionaries[ticker];
                var returns = returnsDictionary.Values.ToArray();
                var intervals = new HistoricalIntervals(returns);
                var jsonIntervals = JsonConvert.SerializeObject(intervals);
                File.WriteAllText(Path.Combine(Config.SubDirectories[2].FullName, ticker + ".json"), jsonIntervals);
            }
        }

    }

}
