using System.Collections.Generic;

namespace HistoricalAnalysis
{
    public class Intervals
    {
        public Intervals() { }

        public Intervals(decimal[] returns)
        {
            var count = returns.Length;
            var sortedReturns = new List<decimal>(returns);
            sortedReturns.Sort();
            var worstIndex = (int)(count * Config.IntervalPercentiles.Worst);
            var likelyIndex = (int)(count * Config.IntervalPercentiles.Likely);
            var bestIndex = (int)(count * Config.IntervalPercentiles.Best);
            Minimum = sortedReturns[0];
            Worst = sortedReturns[worstIndex];
            Likely = sortedReturns[likelyIndex];
            Best = sortedReturns[bestIndex];
            Maximum = sortedReturns[count - 1];
        }

        public decimal Minimum { get; set; }
        public decimal Worst { get; set; }
        public decimal Likely { get; set; }
        public decimal Best { get; set; }
        public decimal Maximum { get; set; }

    }
}
