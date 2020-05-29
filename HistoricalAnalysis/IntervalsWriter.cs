using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace HistoricalAnalysis
{
    public static class IntervalsWriter
    {
        public static void CreateIntervalsFiles()
        {
            var returnsDictionaries = ReturnsFileReader.CreateReturnsDictionaries();
            foreach (var ticker in returnsDictionaries.Keys)
            {
                string jsonIntervals;
                if (ticker != Config.ParentTicker)
                {
                    jsonIntervals = CreateChildIntervalContent(returnsDictionaries, ticker);
                }
                else
                {
                    jsonIntervals = CreateParentIntervalContent(returnsDictionaries);
                }
                File.WriteAllText(Path.Combine(Config.SubDirectories[2].FullName, ticker + ".json"), jsonIntervals);
            }
        }

        private static string CreateParentIntervalContent(Dictionary<string, Dictionary<DateTime, decimal>> returnsDictionaries)
        {
            var parentDictionary = returnsDictionaries[Config.ParentTicker];
            var parentRetts = parentDictionary.Values.ToArray();
            var unConditionalIntervals = CreateUnConditionalIntervals(parentRetts);
            var jsonIntervals = JsonConvert.SerializeObject(unConditionalIntervals);
            return jsonIntervals;
        }

        private static string CreateChildIntervalContent(Dictionary<string, Dictionary<DateTime, decimal>> returnsDictionaries, string ticker)
        {
            var parentDictionary = returnsDictionaries[Config.ParentTicker];
            var parentDates = parentDictionary.Keys;
            var childDictionary = returnsDictionaries[ticker];
            var childDates = childDictionary.Keys;
            var combinedDates = parentDates.Intersect(childDates);
            var parentRetts = combinedDates.Select(cd => parentDictionary[cd]).ToArray();
            var childRetts = combinedDates.Select(cd => childDictionary[cd]).ToArray();
            var conditionalIntervals = CreateConditionalIntervals(parentRetts, childRetts);
            var jsonIntervals = JsonConvert.SerializeObject(conditionalIntervals);
            return jsonIntervals;
        }

        private static HistoricalIntervals CreateUnConditionalIntervals(decimal[] parentRetts)
        {
            var parentSimAnnRetts = new decimal[Config.NumberSimulatedAnnualReturns];
            for (int cnt = 0; cnt < Config.NumberSimulatedAnnualReturns; cnt++)
            {
                var indices = GetRandomIndices(parentRetts.Length);
                parentSimAnnRetts[cnt] = SimulateAnnualReturn(parentRetts, indices);
            }
            var parentIntervals = new HistoricalIntervals(parentSimAnnRetts);
            return parentIntervals;
        }

        private static ConditionalHistoricalIntervals CreateConditionalIntervals(decimal[] parentRetts, decimal[] childRetts)
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
            var conditionalIntervals = new ConditionalHistoricalIntervals(groupedReturns);
            return conditionalIntervals;
        }

        private static int[] GetRandomIndices(int maxValue)
        {
            var rand = new Random();
            var indices = new int[Config.TradingDaysPerYear];
            for (int cnt = 0; cnt < indices.Length; cnt++)
            {
                indices[cnt] = rand.Next(0, maxValue);
            }
            return indices;
        }

        private static GroupedReturns GroupChildReturnsByParentInterval(
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

        private static decimal SimulateAnnualReturn(decimal[] retts, int[] indices)
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
