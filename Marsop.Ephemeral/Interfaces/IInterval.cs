// <copyright file="IInterval.cs" company="Marsop">
//     https://github.com/marsop/ephemeral
// </copyright>

namespace Marsop.Ephemeral.Interfaces;

using System;

public interface IInterval<TBoundary, TLength>
    : IMetricInterval<TBoundary, TLength>
    , ILengthOperator<TBoundary, TLength>
    where TBoundary : notnull, IComparable<TBoundary>
    where TLength : notnull, IComparable<TLength>
{ }