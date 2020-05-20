using System;
using System.Collections.Generic;
using System.IO;

namespace HistoricalAnalysis
{
    public static class ReturnsFileReader
    {
        public static Dictionary<string, Dictionary<DateTime, decimal>> CreateReturnsDictionaries()
        {
            var returnsDirectionaries = new Dictionary<string, Dictionary<DateTime, decimal>>();
            var returnsDirectory = Config.SubDirectories[1];

            var returnsFiles = returnsDirectory.GetFiles();
            foreach (var returnsFile in returnsFiles)
            {
                var returnsDictionary = CreateReturnsDictionary(returnsFile.FullName);
                var ticker = Path.GetFileNameWithoutExtension(returnsFile.Name);
                returnsDirectionaries[ticker] = returnsDictionary;
            }
            return returnsDirectionaries;
        }

        private static Dictionary<DateTime, decimal> CreateReturnsDictionary(string filePath)
        {
            var returnsDictionary = new Dictionary<DateTime, decimal>();
            var lines = File.ReadAllLines(filePath);
            for (int cnt = 0; cnt < lines.Length; cnt++)
            {
                var line = lines[cnt];
                var splitLine = line.Split(",");
                var date = splitLine[0].ParseDate();
                var totalReturn = splitLine[1].ParseDecimal();
                if (date.HasValue && totalReturn.HasValue)
                    returnsDictionary[date.Value] = totalReturn.Value;
            }

            return returnsDictionary;
        }
    }
}
