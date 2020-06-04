using System;
using System.Collections.Generic;

namespace HistoricalAnalysis
{
    public class GroupedReturns
    {
        public GroupedReturns(
            HistoricalIntervals parentIntervals,
            decimal[] parentRetts,
            decimal[] childRetts)
        {
            LeftTail = new List<decimal>();
            LeftNormal = new List<decimal>();
            RightNormal = new List<decimal>();
            RightTail = new List<decimal>();

            for (int cnt = 0; cnt < Config.NumberSimulatedAnnualReturns; cnt++)
            {
                var parentReturn = parentRetts[cnt];
                var childReturn = childRetts[cnt];
                if (parentReturn < parentIntervals.Worst)
                    LeftTail.Add(childReturn);
                else if (parentReturn < parentIntervals.Likely)
                    LeftNormal.Add(childReturn);
                else if (parentReturn < parentIntervals.Best)
                    RightNormal.Add(childReturn);
                else
                    RightTail.Add(childReturn);
            }
        }

        public List<decimal> LeftTail { get; set; }
        public List<decimal> LeftNormal { get; set; }
        public List<decimal> RightNormal { get; set; }
        public List<decimal> RightTail { get; set; }

        public decimal[] GetReturnsByTailType(TailType tailType)
        {
            switch (tailType)
            {
                case TailType.LeftTail:
                    return LeftTail.ToArray();
                case TailType.LeftNormal:
                    return LeftNormal.ToArray();
                case TailType.RightNormal:
                    return RightNormal.ToArray();
                case TailType.RightTail:
                    return RightTail.ToArray();
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
