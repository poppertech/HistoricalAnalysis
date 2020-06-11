using System;

namespace HistoricalAnalysis
{
    public class ConditionalAnalysis
    {
        public ConditionalAnalysis(SimulatedAnnualReturns simulatedAnnualReturns) {
            var parentIntervals = new Intervals(simulatedAnnualReturns.Parent);
            var groupedReturns = new GroupedReturns(parentIntervals, simulatedAnnualReturns.Parent, simulatedAnnualReturns.Child);
            foreach (TailType tailType in Enum.GetValues(typeof(TailType)))
            {
                var intervalReturns = groupedReturns.GetReturnsByTailType(tailType);
                var parentIntervalReturns = groupedReturns.GetParentReturnsByTailType(tailType);
                var unconditionalAnalysis = new UnconditionalAnalysis(intervalReturns, parentIntervalReturns);
                SetAnalysisByTailType(tailType, unconditionalAnalysis);
            }
        }

        public UnconditionalAnalysis LeftTail { get; set; }
        public UnconditionalAnalysis LeftNormal { get; set; }
        public UnconditionalAnalysis RightNormal { get; set; }
        public UnconditionalAnalysis RightTail { get; set; }

        private void SetAnalysisByTailType(TailType tailType, UnconditionalAnalysis intervals)
        {
            switch (tailType)
            {
                case TailType.LeftTail:
                    LeftTail = intervals;
                    return;
                case TailType.LeftNormal:
                    LeftNormal = intervals;
                    return;
                case TailType.RightNormal:
                    RightNormal = intervals;
                    return;
                case TailType.RightTail:
                    RightTail = intervals;
                    return;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
