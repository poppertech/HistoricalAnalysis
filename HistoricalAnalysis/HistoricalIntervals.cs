using System;
using System.Linq;

namespace HistoricalAnalysis
{
    public class HistoricalIntervals
    {
        public HistoricalIntervals() { }

        public HistoricalIntervals(decimal[] returns)
        {
            var count = returns.Length;
            Array.Sort(returns);
            var worstIndex = (int)(count * Config.IntervalPercentiles.Worst);
            var likelyIndex = (int)(count * Config.IntervalPercentiles.Likely);
            var bestIndex = (int)(count * Config.IntervalPercentiles.Best);
            Minimum = returns[0];
            Worst = returns[worstIndex];
            Likely = returns[likelyIndex];
            Best = returns[bestIndex];
            Maximum = returns[count - 1];
        }

        public decimal Minimum { get; set; }
        public decimal Worst { get; set; }
        public decimal Likely { get; set; }
        public decimal Best { get; set; }
        public decimal Maximum { get; set; }
    }
}
