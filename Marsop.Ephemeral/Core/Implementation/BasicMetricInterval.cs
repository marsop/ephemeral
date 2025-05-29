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
        _lengthOperator = lengthOperator ?? throw new ArgumentNullException(nameof(lengthOperator));
    }

    private readonly ILengthOperator<TBoundary, TLength> _lengthOperator;

    public override ILengthOperator<TBoundary, TLength> LengthOperator => _lengthOperator;
}
