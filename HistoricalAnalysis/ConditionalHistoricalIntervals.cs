using System;

namespace HistoricalAnalysis
{
    public class ConditionalHistoricalIntervals
    {
        public ConditionalHistoricalIntervals() { }

        public HistoricalIntervals LeftTail { get; set; }
        public HistoricalIntervals LeftNormal { get; set; }
        public HistoricalIntervals RightNormal { get; set; }
        public HistoricalIntervals RightTail { get; set; }

        public void SetIntervalByTailType(TailType tailType, HistoricalIntervals intervals)
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
