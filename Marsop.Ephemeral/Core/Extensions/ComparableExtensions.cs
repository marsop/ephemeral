using System;

namespace Marsop.Ephemeral.Core;

public static class ComparableExtensions
{
    public static bool IsGreaterThan<T>(this T value, T other) where T : IComparable<T>
    {
        return value.CompareTo(other) > 0;
    }

    public static bool IsLessThan<T>(this T value, T other) where T : IComparable<T>
    {
        return value.CompareTo(other) < 0;
    }

    public static bool IsGreaterOrEqualThan<T>(this T value, T other) where T : IComparable<T>
    {
        return value.CompareTo(other) >= 0;
    }

    public static bool IsLessOrEqualThan<T>(this T value, T other) where T : IComparable<T>
    {
        return value.CompareTo(other) <= 0;
    }

    public static bool IsEqualTo<T>(this T value, T other) where T : IComparable<T>
    {
        return value.CompareTo(other) == 0;
    }

    public static bool IsBetweenBothIncluded<T>(this T current, T min, T max) where T : IComparable<T>
    {
        if (max.IsLessThan(min))
        {
            throw new ArgumentOutOfRangeException(nameof(max));
        }

        if (current.IsLessThan(min))
        {
            return false;
        }

        if (max.IsLessThan(current))
        {
            return false;
        }

        return true;
    }

    public static bool IsBetweenMaxIncluded<T>(this T current, T min, T max) where T : IComparable<T>
    {
        if (max.IsLessThan(min))
        {
            throw new ArgumentOutOfRangeException(nameof(max));
        }

        if (current.IsLessOrEqualThan(min))
        {
            return false;
        }

        if (max.IsLessThan(current))
        {
            return false;
        }

        return true;
    }

    public static bool IsBetweenMinIncluded<T>(this T current, T min, T max) where T : IComparable<T>
    {
        if (max.IsLessThan(min))
        {
            throw new ArgumentOutOfRangeException(nameof(max));
        }

        if (current.IsLessThan(min))
        {
            return false;
        }

        if (max.IsLessOrEqualThan(current))
        {
            return false;
        }

        return true;
    }

    public static bool IsBetweenExcluded<T>(this T current, T min, T max) where T : IComparable<T>
    {
        if (max.IsLessOrEqualThan(min))
        {
            throw new ArgumentOutOfRangeException(nameof(max));
        }

        if (current.IsLessOrEqualThan(min))
        {
            return false;
        }

        if (max.IsLessOrEqualThan(current))
        {
            return false;
        }

        return true;
    }
}