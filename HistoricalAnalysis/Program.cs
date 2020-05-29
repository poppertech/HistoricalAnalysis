using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
            //Config.SubDirectories.DeleteExistingData();
            //Config.StockTickers.DownloadRawData();
            //ReturnsFilesWriter.CreateReturnsFiles();
            IntervalsWriter.CreateIntervalsFiles();
        }

    }
}
