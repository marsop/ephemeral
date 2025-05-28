// <copyright file="IGenericIntervalWithLength.cs" company="Marsop">
//     https://github.com/marsop/ephemeral
// </copyright>

namespace Marsop.Ephemeral.Interfaces;

using System;

/// <summary>
/// Interface for classes implementing an interval
/// </summary>
public interface IGenericIntervalWithLength<TBoundary, TLength>
: IGenericInterval<TBoundary>, IHasLength<TLength>
    where TBoundary : notnull, IComparable<TBoundary>
    where TLength : notnull, IComparable<TLength> {}
