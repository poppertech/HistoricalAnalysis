namespace HistoricalAnalysis
{
    public class Analysis
    {
        public Analysis(SimulatedAnnualReturns simulatedAnnualReturns)
        {

            if (simulatedAnnualReturns.Child == null || simulatedAnnualReturns.Child.Length == 0)
            {
                UnconditionalAnalysis = new UnconditionalAnalysis(simulatedAnnualReturns.Parent);
            }
            else
            {
                UnconditionalAnalysis = new UnconditionalAnalysis(simulatedAnnualReturns.Child);
                var parentIntervals = new Intervals(simulatedAnnualReturns.Parent);
                var groupedReturns = new GroupedReturns(parentIntervals, simulatedAnnualReturns.Parent, simulatedAnnualReturns.Child);
                ConditionalAnalysis = new ConditionalAnalysis(groupedReturns);
            }
        }

        public UnconditionalAnalysis UnconditionalAnalysis { get; set; }

        public ConditionalAnalysis ConditionalAnalysis { get; set; }
    }
}
