using Marsop.Ephemeral.Core;

namespace Marsop.Ephemeral.Net6.Core;

internal static class IntervalValidationExtensions
{
    internal static void ValidateInterval<TBoundary>(
        this IBasicInterval<TBoundary> interval)
        where TBoundary : notnull, IComparable<TBoundary>
    {
        if (interval is null)
        {
            throw new ArgumentNullException(nameof(interval));
        }

        var isValid = interval.Start.IsLessThan(interval.End)
            || (interval.Start.IsEqualTo(interval.End) && interval.StartIncluded && interval.EndIncluded);

        if (!isValid)
        {
            var startDelimiter = interval.StartIncluded ? "[" : "(";
            var endDelimiter = interval.EndIncluded ? "]" : ")";
            throw new InvalidLengthException($"{startDelimiter}{interval.Start}, {interval.End}{endDelimiter}");
        }
    }
}
