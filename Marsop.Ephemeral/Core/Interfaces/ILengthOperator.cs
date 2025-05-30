// <copyright file="ILengthOperator.cs" company="Marsop">
//     https://github.com/marsop/ephemeral
// </copyright>

using System;

namespace Marsop.Ephemeral.Core.Interfaces;

public interface ILengthOperator<TBoundary, TLength> : 
    ICanMeasure<TBoundary, TLength>
    where TBoundary : notnull, IComparable<TBoundary>
{
    TBoundary Apply(TBoundary boundary, TLength length);
}
