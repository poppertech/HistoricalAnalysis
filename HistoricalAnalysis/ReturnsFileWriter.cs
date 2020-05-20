using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace HistoricalAnalysis
{
    public static class ReturnsFilesWriter
    {
        public static void CreateReturnsFiles()
        {
            var rawDataDirectory = Config.SubDirectories[0];
            var returnsDirectory = Config.SubDirectories[1];
            var rawDataFiles = rawDataDirectory.GetFiles();

            foreach (var rawDataFile in rawDataFiles)
            {
                var returnsFileContent = CreateReturnsFileContent(rawDataFile);
                var returnsFilePath = Path.Combine(returnsDirectory.FullName, rawDataFile.Name);
                File.WriteAllText(returnsFilePath, returnsFileContent);
            }

        }

        private static string CreateReturnsFileContent(FileInfo fileInfo)
        {
            const int firstLine = 1;
            var stringBuilder = new StringBuilder();
            var lines = File.ReadAllLines(fileInfo.FullName);

            decimal lastClose = 0;
            for (int cnt = firstLine; cnt < lines.Length; cnt++)
            {
                var line = lines[cnt];
                var splitLine = line.Split(",");
                var date = splitLine[0].ParseDate();
                var adjustedClose = splitLine[5].ParseDecimal();
                if (cnt == firstLine && date.HasValue && adjustedClose.HasValue)
                {
                    lastClose = adjustedClose.Value;
                }
                else if (cnt > firstLine && date.HasValue && adjustedClose.HasValue && lastClose != adjustedClose)
                {
                    var totalReturn = adjustedClose / lastClose - 1;
                    lastClose = adjustedClose.Value;
                    stringBuilder.AppendLine(string.Join(",", date.Value.ToString("MM/dd/yyyy", DateTimeFormatInfo.InvariantInfo), totalReturn));
                }
            }

            return stringBuilder.ToString();
        }

 
    }
}
