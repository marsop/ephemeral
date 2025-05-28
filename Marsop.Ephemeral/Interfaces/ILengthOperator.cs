// <copyright file="ILengthOperator.cs" company="Marsop">
//     https://github.com/marsop/ephemeral
// </copyright>

using Marsop.Ephemeral.Extensions;

namespace Marsop.Ephemeral.Interfaces;

public interface ILengthOperator<TBoundary, TLength>
{
    TBoundary Apply(TBoundary boundary, TLength length);

    TLength Measure(TBoundary boundary1, TBoundary boundary2);

    TLength Zero();
}
