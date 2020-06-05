using System.Collections.Generic;
using System.Linq;

namespace HistoricalAnalysis
{
    public class Histogram
    {
        public IList<HistogramDatum> GetHistogramData(decimal[] simulations, int num = 30)
        {
            var histogramDataArray = new HistogramDatum[num];
            decimal lastCumulativeFrequency = 0;
            decimal dblNum = num;
            var globalXMax = simulations.Max();
            var globalXMin = simulations.Min();
            var sortedSimulations = new List<decimal>(simulations);
            sortedSimulations.Sort();
            for (int cnt = 1; cnt <= num; cnt++)
            {
                var histogramData = new HistogramDatum();
                var interval = (cnt / dblNum) * (globalXMax - globalXMin) + globalXMin;
                histogramData.Interval = interval;
                var index = sortedSimulations.BinarySearch(interval);
                decimal cumulativeCount = index < 0 ? ~index : index + 1;
                decimal totalCount = simulations.Length;
                var cumulativeFrequency = cumulativeCount / totalCount;
                var frequency = cumulativeFrequency - lastCumulativeFrequency;
                histogramData.Frequency = frequency;
                lastCumulativeFrequency = cumulativeFrequency;
                histogramDataArray[cnt - 1] = histogramData;
            }
            return histogramDataArray;
        }
    }
}
