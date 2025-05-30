// <copyright file="ICanMeasure.cs" company="Marsop">
//     https://github.com/marsop/ephemeral
// </copyright>

using System;

namespace Marsop.Ephemeral.Core.Interfaces;

public interface ICanMeasure<TBoundary, TLength>
    where TBoundary : notnull, IComparable<TBoundary>
{
    TLength Measure(IBasicInterval<TBoundary> interval);
}
