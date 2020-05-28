using System;
using System.Collections.Generic;

namespace HistoricalAnalysis
{
    public class GroupedReturns
    {
        public GroupedReturns()
        {
            LeftTail = new List<decimal>();
            LeftNormal = new List<decimal>();
            RightNormal = new List<decimal>();
            RightTail = new List<decimal>();
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
