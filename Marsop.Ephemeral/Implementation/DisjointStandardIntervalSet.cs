// <copyright file="DisjointStandardIntervalSet.cs" company="Marsop">
//     https://github.com/marsop/ephemeral
// </copyright>

using System;
using Marsop.Ephemeral.Interfaces;

namespace Marsop.Ephemeral.Implementation;

public class DisjointStandardIntervalSet : DisjointIntervalSet
{
    public DisjointStandardIntervalSet() : base()
    {
    }

    public DisjointStandardIntervalSet(params IInterval<DateTimeOffset, TimeSpan>[] intervals) : base(intervals)
    {
    }
}
