// <copyright file="IntervalStartComparer.cs" company="Marsop">
//     https://github.com/marsop/ephemeral
// </copyright>

namespace Marsop.Ephemeral
{
    using System.Collections.Generic;

    /// <summary>
    /// Interval starting point comparer class
    /// </summary>
    public class IntervalStartComparer : IComparer<IInterval>
    {
        /// <inheritdoc cref="IComparer{T}.Compare"/>
        public int Compare(IInterval x, IInterval y)
        {
            var startComparison = x.Start.CompareTo(y.Start);
            if (startComparison != 0)
            {
                return startComparison;
            }

            if (x.StartIncluded && !y.StartIncluded)
            {
                return -1;
            }
            else if (!x.StartIncluded && y.StartIncluded)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}