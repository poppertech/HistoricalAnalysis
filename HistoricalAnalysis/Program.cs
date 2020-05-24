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
            var returnsDictionaries = ReturnsFileReader.CreateReturnsDictionaries();
            //CreateIntervals(returnsDictionaries);
            CreateSimulatedReturns(returnsDictionaries);
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

        public static void CreateSimulatedReturns(Dictionary<string, Dictionary<DateTime, decimal>> returnsDictionaries)
        {
            var intervalsDirectory = Config.SubDirectories[2];
            var simulationsDirectory = Config.SubDirectories[3];

            var parentIntervalsPath = Path.Combine(intervalsDirectory.FullName, Config.ParentTicker + ".json");
            var parentIntervalsContent = File.ReadAllText(parentIntervalsPath);
            var parentIntervals = JsonConvert.DeserializeObject<HistoricalIntervals>(parentIntervalsContent);

            var tailDirectories = simulationsDirectory.GetDirectories();
            var parentDictionary = returnsDictionaries[Config.ParentTicker];

            foreach (var ticker in returnsDictionaries.Keys)
            {
                if (ticker == Config.ParentTicker)
                {
                    var contents = CreateSimulatedReturnsContent(parentDictionary.Values.ToArray());
                    var path = Path.Combine(simulationsDirectory.FullName, ticker + ".csv");
                    File.AppendAllText(path, contents);
                }
                else
                {
                    var childDictionary = returnsDictionaries[ticker];
                    var groupedReturns = GroupChildReturnsByParentInterval(parentIntervals, parentDictionary, childDictionary);
                    for (int index = 0; index < tailDirectories.Length; index++)
                    {
                        var tailDirectory = tailDirectories[index];
                        var returns = groupedReturns.GetReturnsByIndex((TailType)index);
                        var contents = CreateSimulatedReturnsContent(returns);
                        var path = Path.Combine(tailDirectory.FullName, ticker + ".csv");
                        File.AppendAllText(path, contents);
                    }
                }
            }
        }

        public static GroupedReturns GroupChildReturnsByParentInterval(
            HistoricalIntervals parentIntervals,
            Dictionary<DateTime, decimal> parentDictionary,
            Dictionary<DateTime, decimal> childDictionary)
        {
            var groupedReturns = new GroupedReturns();
            var combinedDates = parentDictionary.Keys.Intersect(childDictionary.Keys);
            foreach (var date in combinedDates)
            {
                var parentReturn = parentDictionary[date];
                var childReturn = childDictionary[date];
                if (parentReturn < parentIntervals.Worst)
                    groupedReturns.LeftTail.Add(childReturn);
                else if (parentReturn < parentIntervals.Likely)
                    groupedReturns.LeftNormal.Add(childReturn);
                else if (parentReturn < parentIntervals.Best)
                    groupedReturns.RightNormal.Add(childReturn);
                else
                    groupedReturns.RightTail.Add(childReturn);
            }
            return groupedReturns;
        }

        public static string CreateSimulatedReturnsContent(decimal[] retts)
        {
            var stringBuilder = new StringBuilder();
            var simAnnRetts = SimulateAnnualReturns(retts);
            foreach (var rett in simAnnRetts)
            {
                stringBuilder.AppendLine(rett.ToString());
            }
            return stringBuilder.ToString();
        }

        public static decimal[] SimulateAnnualReturns(decimal[] retts)
        {
            var simAnnRetts = new decimal[Config.NumberSimulatedAnnualReturns];
            for (int cnt = 0; cnt < Config.NumberSimulatedAnnualReturns; cnt++)
            {
                simAnnRetts[cnt] = SimulateAnnualReturn(retts);
            }
            return simAnnRetts;
        }

        public static decimal SimulateAnnualReturn(decimal[] retts)
        {
            var rand = new Random();
            var simRetts = new decimal[Config.TradingDaysPerYear];
            var simCumRetts = new decimal[Config.TradingDaysPerYear];
            for (int cnt = 0; cnt < Config.TradingDaysPerYear; cnt++)
            {
                var index = rand.Next(0, retts.Length);
                simRetts[cnt] = retts[index];
                if (cnt == 0)
                {
                    simCumRetts[0] = Config.BaseCumulativeReturn * (1 + simRetts[0]);
                }
                else
                {
                    simCumRetts[cnt] = simCumRetts[cnt - 1] * (1 + simRetts[cnt]);
                }
            }
            return simCumRetts[Config.TradingDaysPerYear - 1];
        }


    }
}
