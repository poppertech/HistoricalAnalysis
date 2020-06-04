using Newtonsoft.Json;
using System.IO;

namespace HistoricalAnalysis
{
    public static class AnalysisWriter
    {
        public static void CreateFiles()
        {
            var returnsDictionaries = ReturnsFileReader.CreateReturnsDictionaries();
            foreach (var ticker in returnsDictionaries.Keys)
            {
                var simulatedAnnualReturns = SimulateAnnualReturnFactory.Create(returnsDictionaries, ticker);
                var jsonIntervals = CreateIntervalContent(simulatedAnnualReturns);
                File.WriteAllText(Path.Combine(Config.SubDirectories[2].FullName, ticker + ".json"), jsonIntervals);
            }
        }


        private static string CreateIntervalContent(SimulatedAnnualReturns simulatedAnnualReturns)
        {
            var parentIntervals = new HistoricalIntervals(simulatedAnnualReturns.Parent);
            if (simulatedAnnualReturns.Child == null || simulatedAnnualReturns.Child.Length == 0)
            {
                return JsonConvert.SerializeObject(parentIntervals);
            }
            else
            {
                var groupedReturns = new GroupedReturns(parentIntervals, simulatedAnnualReturns.Parent, simulatedAnnualReturns.Child);
                var conditionalIntervals = new ConditionalHistoricalIntervals(groupedReturns);
                return JsonConvert.SerializeObject(conditionalIntervals);
            }
        }
    }
}
