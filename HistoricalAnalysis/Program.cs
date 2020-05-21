using System;
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
        }

    }

}
