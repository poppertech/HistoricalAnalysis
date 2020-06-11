using System;
using System.Collections.Generic;
using System.Linq;

namespace HistoricalAnalysis
{
    public class Statistics
    {
        public Statistics(decimal[] retts, decimal[] parentRetts = null)
        {
            var inputReturns = retts.Select(rett => (double)rett).ToArray();
            Mean = inputReturns.Average();
            var deMeanedReturns = inputReturns.Select(rett => rett - Mean).ToArray();

            Stdev = CalculateStdev(deMeanedReturns);
            Skew = CalculateSkew(deMeanedReturns);
            Kurt = CalculateKurt(deMeanedReturns);
            if(parentRetts != null && parentRetts.Any())
            {
                var parentInputReturns = parentRetts.Select(rett => (double)rett).ToArray();
                var parentMean = parentInputReturns.Average();
                var parentDemeanedReturns = parentInputReturns.Select(rett => rett - Mean).ToArray();
                var parentStdev = CalculateStdev(parentDemeanedReturns);
                Covariance = CalculateCovariance(deMeanedReturns, parentDemeanedReturns);
                Correlation = Covariance / (Stdev * parentStdev);
                Beta = Covariance / Math.Pow(parentStdev, 2);
            }
        }

        public double Mean { get; private set; }
        public double Stdev { get; private set; }
        public double Skew { get; private set; }
        public double Kurt { get; private set; }

        public double? Covariance { get; private set; }
        public double? Beta { get; private set; }
        public double? Correlation { get; private set; }

        private double CalculateStdev(IList<double> deMeanedReturns)
        {
            double sumSq = deMeanedReturns.Sum(rett => Math.Pow(rett, 2));
            return Math.Pow(sumSq / (deMeanedReturns.Count - 1), .5);
        }

        private double CalculateSkew(IList<double> deMeanedReturns)
        {
            var count = deMeanedReturns.Count;
            double sumCube = deMeanedReturns.Sum(rett => Math.Pow(rett, 3));
            return (count / ((count - 1) * (count - 2))) * (sumCube / Math.Pow(Stdev, 3));
        }

        private double CalculateKurt(IList<double> deMeanedReturns)
        {
            var count = deMeanedReturns.Count;
            double sumPow4 = deMeanedReturns.Sum(rett => Math.Pow(rett, 4));
            double coef = (((count) * (count + 1)) / ((count - 1) * (count - 2) * (count - 3)));
            double adjFact = (-3 * ((Math.Pow(count - 1, 2)) / ((count - 2) * (count - 3))));
            return (coef * (sumPow4 / Math.Pow(Stdev, 4)) + adjFact);
        }

        private double CalculateCovariance(IList<double> deMeanedReturns, IList<double> parentDemeanedReturns)
        {
            double sumSq = 0;
            for (int cnt = 0; cnt < deMeanedReturns.Count; cnt++)
            {
                sumSq += deMeanedReturns[cnt] * parentDemeanedReturns[cnt];
            }
            return sumSq / (deMeanedReturns.Count - 1);
        }
    }
}
