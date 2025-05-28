using System;
using Marsop.Ephemeral.Exceptions;
using Marsop.Ephemeral.Extensions;
using Marsop.Ephemeral.Interfaces;

namespace Marsop.Ephemeral.Implementation;

public class BasicInterval<TBoundary> : IBasicInterval<TBoundary>
    where TBoundary : IComparable<TBoundary>
{
    public TBoundary Start { get; }
    public TBoundary End { get; }
    public bool StartIncluded { get; }
    public bool EndIncluded { get; }

    public BasicInterval(TBoundary start, TBoundary end, bool startIncluded, bool endIncluded)
    {
        Start = start;
        End = end;
        StartIncluded = startIncluded;
        EndIncluded = endIncluded;

        if (!IsValid)
        {
            throw new InvalidLengthException(GetTextualRepresentation());
        }
    }

        /// <summary>
    /// Get a representation of the interval.
    /// Open intervals are represented with parenthesis (a,b)
    /// Close intervals are represented with brakets [a,b]
    /// </summary>
    /// <returns>a <see cref="String"/> that represent the interval</returns>
    protected string GetTextualRepresentation()
    {
        var startDelimiter = StartIncluded ? "[" : "(";
        var endDelimiter = EndIncluded ? "]" : ")";
        return $"{startDelimiter}{Start} => {End}{endDelimiter}";
    }

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return GetTextualRepresentation();
    }

    public bool IsValid => Start.IsLessThan(End) || Start.IsEqualTo(End) && StartIncluded && EndIncluded;
}