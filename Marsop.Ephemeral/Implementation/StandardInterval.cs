// <copyright file="StandardInterval.cs" company="Marsop">
//     https://github.com/marsop/ephemeral
// </copyright>

using System;
using Marsop.Ephemeral.Interfaces;

namespace Marsop.Ephemeral.Implementation;

public record StandardInterval :
    AbstractMetricInterval<DateTimeOffset, TimeSpan>,
    IIntervalFactory<StandardInterval, DateTimeOffset>
{
    public StandardInterval(DateTimeOffset start, DateTimeOffset end, bool startIncluded, bool endIncluded) : base(start, end, startIncluded, endIncluded)
    {
    }

    public override ILengthOperator<DateTimeOffset, TimeSpan> LengthOperator =>
        DateTimeOffsetStandardLengthOperator.Instance;

    public StandardInterval CreateFrom(IBasicInterval<DateTimeOffset> interval)
    {
        return new StandardInterval(
            interval.Start,
            interval.End,
            interval.StartIncluded,
            interval.EndIncluded);
    }
}
