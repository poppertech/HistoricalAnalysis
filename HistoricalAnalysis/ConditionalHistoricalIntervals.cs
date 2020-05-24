namespace HistoricalAnalysis
{
    public class ConditionalHistoricalIntervals
    {
        public ConditionalHistoricalIntervals() { }

        public HistoricalIntervals LeftTail { get; set; }
        public HistoricalIntervals LeftNormal { get; set; }
        public HistoricalIntervals RightNormal { get; set; }
        public HistoricalIntervals RightTail { get; set; }
    }
}
