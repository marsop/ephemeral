// <copyright file="IInterval.cs" company="Marsop">
//     https://github.com/marsop/ephemeral
// </copyright>

namespace Marsop.Ephemeral.Interfaces;

using System;

public interface IDateTimeInterval : IGenericInterval<DateTime>, IHasDuration<TimeSpan>
{
}