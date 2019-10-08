using System;
using System.Collections.Generic;

namespace Marsop.Ephemeral
{
    public class IntervalStartComparer : IComparer<IInterval>
    {
        public int Compare(IInterval x, IInterval y)
        {
            var startComparison = x.Start.CompareTo(y.Start);
            if (startComparison != 0)
                return startComparison;

            if (x.StartIncluded && !y.StartIncluded)
                return -1;

            else if (!x.StartIncluded && y.StartIncluded)
                return 1;

            else return 0;
        }
    }
}