using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace HistoricalAnalysis
{
    public static class SimulateAnnualReturnFactory
    {
        public static SimulatedAnnualReturns Create(Dictionary<string, Dictionary<DateTime, decimal>> returnsDictionaries, string ticker)
        {
            if (ticker == Config.ParentTicker)
            {
                var parentDictionary = returnsDictionaries[Config.ParentTicker];
                var parentRetts = parentDictionary.Values.ToArray();
                //File.WriteAllLines(@"C:\Users\bwynn\Desktop\HistoricalAnalysis\SPYOnlyReturnsCheck.csv", Array.ConvertAll(parentRetts, (d) => d.ToString()));
                return new SimulatedAnnualReturns(parentRetts);
            }
            else
            {
                var parentDictionary = returnsDictionaries[Config.ParentTicker];
                var parentDates = parentDictionary.Keys;
                var childDictionary = returnsDictionaries[ticker];
                var childDates = childDictionary.Keys;
                var combinedDates = parentDates.Intersect(childDates);
                var parentRetts = combinedDates.Select(cd => parentDictionary[cd]).ToArray();
                //File.WriteAllLines(@"C:\Users\bwynn\Desktop\HistoricalAnalysis\SPYUNHReturnsCheck.csv", Array.ConvertAll(parentRetts, (d) => d.ToString()));
                var childRetts = combinedDates.Select(cd => childDictionary[cd]).ToArray();
                //File.WriteAllLines(@"C:\Users\bwynn\Desktop\HistoricalAnalysis\UNHReturnsCheck.csv", Array.ConvertAll(childRetts, (d) => d.ToString()));
                return new SimulatedAnnualReturns(parentRetts, childRetts);
            }
        }

    }
}
