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
            //CreateSimulatedReturns(returnsDictionaries);
        }

        public static void CreateSimulatedReturns(Dictionary<string, Dictionary<DateTime, decimal>> returnsDictionaries)
        {
            var parentDates = returnsDictionaries[Config.ParentTicker].Keys;
            foreach (var ticker in returnsDictionaries.Keys)
            {
                var childDates = returnsDictionaries[ticker].Keys;
                var combinedDates = parentDates.Intersect(childDates);
                
            }
        }

        public static ConditionalHistoricalIntervals CreateConditionalIntervals(decimal[] parentRetts, decimal[] childRetts)
        {
            var parentSimAnnRetts = new decimal[Config.NumberSimulatedAnnualReturns];
            var childSimAnnRetts = new decimal[Config.NumberSimulatedAnnualReturns];
            for (int cnt = 0; cnt < Config.NumberSimulatedAnnualReturns; cnt++)
            {
                var indices = GetRandomIndices(childRetts.Length);
                parentSimAnnRetts[cnt] = SimulateAnnualReturn(parentRetts, indices);
                childSimAnnRetts[cnt] = SimulateAnnualReturn(childRetts, indices);
            }
            var parentIntervals = new HistoricalIntervals(parentSimAnnRetts);
            var groupedReturns = GroupChildReturnsByParentInterval(parentIntervals, parentSimAnnRetts, childSimAnnRetts);
            var conditionalIntervals = new ConditionalHistoricalIntervals();
            foreach (TailType tailType in Enum.GetValues(typeof(TailType)))
            {
                var intervalReturns = groupedReturns.GetReturnsByTailType(tailType);
                var childInterval = new HistoricalIntervals(intervalReturns);
                conditionalIntervals.SetIntervalByTailType(tailType, childInterval);
            }
            return conditionalIntervals;
        }

        public static int[] GetRandomIndices(int maxValue)
        {
            var rand = new Random();
            var indices = new int[Config.TradingDaysPerYear];
            for (int cnt = 0; cnt < indices.Length; cnt++)
            {
                indices[cnt] = rand.Next(0, maxValue);
            }
            return indices;
        }

        public static GroupedReturns GroupChildReturnsByParentInterval(
            HistoricalIntervals parentIntervals,
            decimal[] parentRetts,
            decimal[] childRetts)
        {
            var groupedReturns = new GroupedReturns();

            for (int cnt = 0; cnt < Config.NumberSimulatedAnnualReturns; cnt++)
            {
                var parentReturn = parentRetts[cnt];
                var childReturn = childRetts[cnt];
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

        public static decimal SimulateAnnualReturn(decimal[] retts, int[] indices)
        {
            var simRetts = new decimal[Config.TradingDaysPerYear];
            var simCumRetts = new decimal[Config.TradingDaysPerYear];
            for (int cnt = 0; cnt < Config.TradingDaysPerYear; cnt++)
            {
                var index = indices[cnt];
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
