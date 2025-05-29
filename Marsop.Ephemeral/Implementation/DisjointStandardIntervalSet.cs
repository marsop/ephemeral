// <copyright file="DisjointStandardIntervalSet.cs" company="Marsop">
//     https://github.com/marsop/ephemeral
// </copyright>

using System;
using Marsop.Ephemeral.Interfaces;

namespace Marsop.Ephemeral.Implementation;

public class DisjointStandardIntervalSet : 
    DisjointIntervalSet<DateTimeOffset, TimeSpan>
{
    public DisjointStandardIntervalSet() :
        base(DateTimeOffsetStandardLengthOperator.Instance)
    {
    }

    public DisjointStandardIntervalSet(params IInterval<DateTimeOffset, TimeSpan>[] intervals) :
        base(DateTimeOffsetStandardLengthOperator.Instance,intervals)
    {
    }
}
