using System;
using System.IO;

namespace HistoricalAnalysis
{
    public static class Config
    {
        public static string StartDate = "1262217600";
        public static string EndDate = "2000000000";
        public static string BaseDirectoryPath = @"C:\Users\bwynn\Desktop\HistoricalAnalysis";
        public static DirectoryInfo BaseDirectory = new DirectoryInfo(BaseDirectoryPath);
        public static DirectoryInfo[] SubDirectories = BaseDirectory.GetDirectories();
        public static Uri BaseQueryUri = new Uri(@"https://query1.finance.yahoo.com/v7/finance/download/");
        public static string[] StockTickers = new[] { "SPY", "UNH" };
        public static string ParentTicker = StockTickers[0];
        public static Intervals IntervalPercentiles = new Intervals { Worst = .1M, Likely = .5M, Best = .9M };
        public static int TradingDaysPerYear = 252;
        public static int BaseCumulativeReturn = 100;
        public static int NumberSimulatedAnnualReturns = 10000;
    }
}
