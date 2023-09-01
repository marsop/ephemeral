using Marsop.Ephemeral.Exceptions;
using Marsop.Ephemeral.Extensions;
using Marsop.Ephemeral.Interfaces;
using Optional;
using System;

namespace Marsop.Ephemeral.Implementation;

public class GenericInterval<T> : IGenericInterval<T>
    where T : notnull, IComparable<T>
{
    /// <inheritdoc cref="IGenericInterval{TBoundary}.End"/>
    public T End { get; }

    /// <inheritdoc cref="IGenericInterval{TBoundary}.EndIncluded"/>
    public bool EndIncluded { get; }

    /// <inheritdoc cref="IGenericInterval{TBoundary}.Start"/>
    public T Start { get; }

    /// <inheritdoc cref="IGenericInterval{TBoundary}.StartIncluded"/>
    public bool StartIncluded { get; }

    /// <summary>
    /// Checks if the current <see cref="Interval"/> has coherent starting and ending points
    /// </summary>
    /// <returns><code>true</code> if starting and ending points are valid, <code>false</code> otherwise</returns>
    public bool IsValid => Start.IsLessThan(End) || (Start.IsEqualTo(End) && StartIncluded && EndIncluded);

    public GenericInterval(T start, T end, bool startIncluded, bool endIncluded)
    {
        Start = start;
        End = end;

        StartIncluded = startIncluded;
        EndIncluded = endIncluded;

        if (!IsValid)
        {
            throw new InvalidDurationException(GetTextualRepresentation());
        }
    }

    /// <summary>
    /// Intersect two intervals
    /// </summary>
    /// <param name="first">the first <see cref="IGenericInterval{TBoundary}"/> instance</param>
    /// <param name="second">the second <see cref="IGenericInterval{TBoundary}"/> instance</param>
    /// <returns>a new <see cref="GenericInterval{TBoundary}"/> if an intersection exists</returns>
    /// <exception cref="ArgumentNullException">an exception is thrown if at least one of the given parameters is <code>null</code></exception>
    public static Option<GenericInterval<T>> Intersect(IGenericInterval<T> first, IGenericInterval<T> second)
    {
        if (first is null)
        {
            throw new ArgumentNullException(nameof(first));
        }

        if (second is null)
        {
            throw new ArgumentNullException(nameof(second));
        }

        var maxStart = first.Start.IsGreaterThan(second.Start) ? first.Start : second.Start;
        var minEnd = first.End.IsLessThan(second.End) ? first.End : second.End;

        if (minEnd.IsLessThan(maxStart))
        {
            return Option.None<GenericInterval<T>>();
        }

        if (minEnd.IsEqualTo(maxStart) && (!first.Covers(minEnd) || !second.Covers(minEnd)))
        {
            return Option.None<GenericInterval<T>>();
        }

        var startIncluded = first.Covers(maxStart) && second.Covers(maxStart);
        var endIncluded = first.Covers(minEnd) && second.Covers(minEnd);

        return new GenericInterval<T>(maxStart, minEnd, startIncluded, endIncluded).Some();
    }

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return GetTextualRepresentation();
    }

    /// <summary>
    /// Get a representation of the interval.
    /// Open intervals are represented with parenthesis (a,b)
    /// Close intervals are represented with brakets [a,b]
    /// </summary>
    /// <returns>a <see cref="String"/> that represent the interval</returns>
    private string GetTextualRepresentation()
    {
        var startDelimiter = StartIncluded ? "[" : "(";
        var endDelimiter = EndIncluded ? "]" : ")";
        return $"{startDelimiter}{Start} => {End}{endDelimiter}";
    }
}