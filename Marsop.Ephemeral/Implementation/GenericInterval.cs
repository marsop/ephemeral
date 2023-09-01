using Marsop.Ephemeral.Exceptions;
using Marsop.Ephemeral.Extensions;
using Marsop.Ephemeral.Interfaces;
using System;

namespace Marsop.Ephemeral.Implementation;

public abstract class GenericInterval<T> : IGenericInterval<T>
    where T : notnull, IComparable<T>, IEquatable<T>
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
    public bool IsValid => Start.IsLessThan(End) || (Start.Equals(End) && StartIncluded && EndIncluded);

    /// <summary>
    /// Initializes a new instance of the <see cref="Interval" /> class
    /// </summary>
    /// <param name="start">the starting <see cref="DateTimeOffset"/></param>
    /// <param name="end">the ending <see cref="DateTimeOffset"/></param>
    /// <param name="startIncluded">a flag indicating whether the starting point is included</param>
    /// <param name="endIncluded">a flag indicating whether the ending point is included</param>
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