// <copyright file="IInterval.cs" company="Marsop">
//     https://github.com/marsop/ephemeral
// </copyright>

namespace Marsop.Ephemeral.Interfaces;

using System;

/// <summary>
/// Interface for classes implementing an interval
/// </summary>
public interface IDateTimeOffsetInterval : IInterval<DateTimeOffset, TimeSpan> {}