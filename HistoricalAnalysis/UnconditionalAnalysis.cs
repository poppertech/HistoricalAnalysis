namespace HistoricalAnalysis
{
    public class UnconditionalAnalysis
    {
        public UnconditionalAnalysis() { }

        public UnconditionalAnalysis(decimal[] returns, decimal[] parentReturns = null)
        {
            Intervals = new Intervals(returns);
            Statistics = new Statistics(returns, parentReturns);
            Histogram = new Histogram(returns);
        }

        public Intervals Intervals { get; set; }

        public Statistics Statistics { get; set; }

        public Histogram Histogram { get; set; }

    }
}
