// <copyright file="BasicMetricInterval.cs" company="Marsop">
//     https://github.com/marsop/ephemeral
// </copyright>

using System;
using Marsop.Ephemeral.Core.Interfaces;

namespace Marsop.Ephemeral.Core.Implementation;

public record BasicMetricInterval<TBoundary, TLength> :
    AbstractMetricInterval<TBoundary, TLength>
    where TBoundary : notnull, IComparable<TBoundary>
{
    public BasicMetricInterval(
        TBoundary start,
        TBoundary end,
        bool startIncluded,
        bool endIncluded,
        ILengthOperator<TBoundary, TLength> lengthOperator)
        : base(start, end, startIncluded, endIncluded)
    {
        if (lengthOperator is null)
        {
            throw new ArgumentNullException(nameof(lengthOperator));
        }

        _length = lengthOperator.Measure(start, end);
    }

    private readonly TLength _length;

    public override TLength Length()  => _length;
}
