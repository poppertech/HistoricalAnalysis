using System;

namespace HistoricalAnalysis
{
    public class ConditionalAnalysis
    {
        public ConditionalAnalysis(GroupedReturns groupedReturns) {
            foreach (TailType tailType in Enum.GetValues(typeof(TailType)))
            {
                var intervalReturns = groupedReturns.GetReturnsByTailType(tailType);
                var unconditionalAnalysis = new UnconditionalAnalysis(intervalReturns);
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
