// <copyright file="IIntervalFactory.cs" company="Marsop">
//     https://github.com/marsop/ephemeral
// </copyright>

namespace Marsop.Ephemeral.Interfaces;

using System;

public interface IIntervalFactory<TInterval, TBoundary, TLength>
    where TInterval : IInterval<TBoundary, TLength>
    where TBoundary : notnull, IComparable<TBoundary>
    where TLength : notnull, IComparable<TLength>
{
    TInterval Create(
        TBoundary start,
        bool startIncluded,
        TBoundary end,
        bool endIncluded);
}
