// <copyright file="IMetricInterval.cs" company="Marsop">
//     https://github.com/marsop/ephemeral
// </copyright>

namespace Marsop.Ephemeral.Core.Interfaces;

using System;

/// <summary>
/// Interface for classes implementing an interval
/// </summary>
public interface IMetricInterval<TBoundary, TLength>: 
    IBasicInterval<TBoundary>,
    IHasLength<TLength>
    where TBoundary : notnull, IComparable<TBoundary> {}
