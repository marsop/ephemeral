// <copyright file="IHasLengthOperator.cs" company="Marsop">
//     https://github.com/marsop/ephemeral
// </copyright>

namespace Marsop.Ephemeral.Core.Interfaces;

public interface IHasLengthOperator<TBoundary, TLength>
{
    ILengthOperator<TBoundary, TLength> LengthOperator { get; }
}
