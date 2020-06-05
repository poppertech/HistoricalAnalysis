using System;
using System.Linq;

namespace HistoricalAnalysis
{
    public class UnconditionalAnalysis
    {
        public UnconditionalAnalysis() { }

        public UnconditionalAnalysis(decimal[] returns)
        {
            Intervals = new Intervals(returns);
            Statistics = new Statistics(returns);
            Histogram = new Histogram().GetHistogramData(returns).ToArray();
        }

        public Intervals Intervals { get; set; }

        public Statistics Statistics { get; set; }

        public HistogramDatum[] Histogram { get; set; }

    }
}
