using System;
using System.Collections.Generic;

namespace HistoricalAnalysis
{
    public class GroupedReturns
    {
        public GroupedReturns(
            Intervals parentIntervals,
            decimal[] parentRetts,
            decimal[] childRetts)
        {
            LeftTail = new List<decimal>();
            LeftNormal = new List<decimal>();
            RightNormal = new List<decimal>();
            RightTail = new List<decimal>();

            ParentLeftTail = new List<decimal>();
            ParentLeftNormal = new List<decimal>();
            ParentRightNormal = new List<decimal>();
            ParentRightTail = new List<decimal>();

            for (int cnt = 0; cnt < parentRetts.Length; cnt++)
            {
                var parentReturn = parentRetts[cnt];
                var childReturn = childRetts[cnt];
                if (parentReturn < parentIntervals.Worst)
                {
                    LeftTail.Add(childReturn);
                    ParentLeftTail.Add(parentReturn);
                }                    
                else if (parentReturn < parentIntervals.Likely)
                {
                    LeftNormal.Add(childReturn);
                    ParentLeftNormal.Add(parentReturn);
                }
                else if (parentReturn < parentIntervals.Best)
                {
                    RightNormal.Add(childReturn);
                    ParentRightNormal.Add(parentReturn);
                }
                else
                {
                    RightTail.Add(childReturn);
                    ParentRightTail.Add(parentReturn);
                }
            }
        }

        public List<decimal> LeftTail { get; set; }
        public List<decimal> LeftNormal { get; set; }
        public List<decimal> RightNormal { get; set; }
        public List<decimal> RightTail { get; set; }

        public List<decimal> ParentLeftTail { get; set; }
        public List<decimal> ParentLeftNormal { get; set; }
        public List<decimal> ParentRightNormal { get; set; }
        public List<decimal> ParentRightTail { get; set; }

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

        public decimal[] GetParentReturnsByTailType(TailType tailType)
        {
            switch (tailType)
            {
                case TailType.LeftTail:
                    return ParentLeftTail.ToArray();
                case TailType.LeftNormal:
                    return ParentLeftNormal.ToArray();
                case TailType.RightNormal:
                    return ParentRightNormal.ToArray();
                case TailType.RightTail:
                    return ParentRightTail.ToArray();
                default:
                    throw new NotImplementedException();
            }
        }


    }
}
