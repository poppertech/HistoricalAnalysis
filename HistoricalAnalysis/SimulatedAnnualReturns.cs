using System;

namespace HistoricalAnalysis
{
    public class SimulatedAnnualReturns
    {
        public SimulatedAnnualReturns(decimal[] parentRetts, decimal[] childRetts = null)
        {
            Parent = new decimal[Config.NumberSimulatedAnnualReturns];
            Child = childRetts != null && childRetts.Length > 0 ? new decimal[Config.NumberSimulatedAnnualReturns] : new decimal[0];
            for (int cnt = 0; cnt < Config.NumberSimulatedAnnualReturns; cnt++)
            {
                var indices = GetRandomIndices(parentRetts.Length);
                Parent[cnt] = SimulateAnnualReturn(parentRetts, indices);
                if(childRetts != null && childRetts.Length > 0)
                    Child[cnt] = SimulateAnnualReturn(childRetts, indices);
            }
        }

        public decimal[] Parent { get; set; }

        public decimal[] Child { get; set; }

        private static decimal SimulateAnnualReturn(decimal[] retts, int[] indices)
        {
            var simRetts = new decimal[Config.TradingDaysPerYear];
            var simCumRetts = new decimal[Config.TradingDaysPerYear];
            for (int cnt = 0; cnt < Config.TradingDaysPerYear; cnt++)
            {
                var index = indices[cnt];
                simRetts[cnt] = retts[index];
                if (cnt == 0)
                {
                    simCumRetts[0] = Config.BaseCumulativeReturn * (1 + simRetts[0]);
                }
                else
                {
                    simCumRetts[cnt] = simCumRetts[cnt - 1] * (1 + simRetts[cnt]);
                }
            }
            return simCumRetts[Config.TradingDaysPerYear - 1];
        }

        private static int[] GetRandomIndices(int maxValue)
        {
            var rand = new Random();
            var indices = new int[Config.TradingDaysPerYear];
            for (int cnt = 0; cnt < indices.Length; cnt++)
            {
                indices[cnt] = rand.Next(0, maxValue);
            }
            return indices;
        }

    }
}
