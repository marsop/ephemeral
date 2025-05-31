// <copyright file="IHasDuration.cs" company="Marsop">
//     https://github.com/marsop/ephemeral
// </copyright>

namespace Marsop.Ephemeral.Core;

public interface IHasLength<out TLength>
{
    /// <summary>
    /// Gets the difference between start and end as <see cref="TLength"/>
    /// </summary>
    TLength DefaultMeasure();
}