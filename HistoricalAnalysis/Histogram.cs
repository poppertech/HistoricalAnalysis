using System.Collections.Generic;
using System.Linq;

namespace HistoricalAnalysis
{
    public class Histogram
    {
        public Histogram(decimal[] simulations, int num = 30)
        {
            Intervals = new decimal[num];
            Frequencies = new decimal[num];
            decimal lastCumulativeFrequency = 0;
            decimal dblNum = num;
            var globalXMax = simulations.Max();
            var globalXMin = simulations.Min();
            var sortedSimulations = new List<decimal>(simulations);
            sortedSimulations.Sort();
            for (int cnt = 1; cnt <= num; cnt++)
            {
                var interval = (cnt / dblNum) * (globalXMax - globalXMin) + globalXMin;
                Intervals[cnt-1] = interval;
                var index = sortedSimulations.BinarySearch(interval);
                decimal cumulativeCount = index < 0 ? ~index : index + 1;
                decimal totalCount = simulations.Length;
                var cumulativeFrequency = cumulativeCount / totalCount;
                var frequency = cumulativeFrequency - lastCumulativeFrequency;
                Frequencies[cnt-1] = frequency;
                lastCumulativeFrequency = cumulativeFrequency;
            }
        }

        public decimal[] Intervals { get; set; }
        public decimal[] Frequencies { get; set; }
    }
}
