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
                ConditionalAnalysis = new ConditionalAnalysis(simulatedAnnualReturns);
            }
        }

        public UnconditionalAnalysis UnconditionalAnalysis { get; set; }

        public ConditionalAnalysis ConditionalAnalysis { get; set; }
    }
}
