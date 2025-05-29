// <copyright file="DisjointStandardIntervalSet.cs" company="Marsop">
//     https://github.com/marsop/ephemeral
// </copyright>

using System;
using Marsop.Ephemeral.Core.Implementation;
using Marsop.Ephemeral.Core.Interfaces;

namespace Marsop.Ephemeral.Temporal;

public class DisjointStandardIntervalSet : 
    DisjointIntervalSet<DateTimeOffset, TimeSpan>
{
    public DisjointStandardIntervalSet(params IMetricInterval<DateTimeOffset, TimeSpan>[] intervals) :
        base(DateTimeOffsetStandardLengthOperator.Instance,intervals)
    {
    }
}
