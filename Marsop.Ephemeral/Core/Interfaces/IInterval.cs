// <copyright file="IInterval.cs" company="Marsop">
//     https://github.com/marsop/ephemeral
// </copyright>

namespace Marsop.Ephemeral.Core.Interfaces;

using System;

public interface IInterval<TBoundary, TLength> :
    IMetricInterval<TBoundary, TLength>
    where TBoundary : notnull, IComparable<TBoundary>
{ 
    public ILengthOperator<TBoundary, TLength> LengthOperator { get; }
}