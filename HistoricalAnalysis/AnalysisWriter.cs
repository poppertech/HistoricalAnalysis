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
                var analysis = new Analysis(simulatedAnnualReturns);
                var json = JsonConvert.SerializeObject(analysis);
                File.WriteAllText(Path.Combine(Config.SubDirectories[2].FullName, ticker + ".json"), json);
            }
        }

    }
}
