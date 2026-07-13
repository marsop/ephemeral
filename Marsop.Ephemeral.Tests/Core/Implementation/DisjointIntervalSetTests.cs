using System;
using System.Collections.Generic;
using FluentAssertions;
using Marsop.Ephemeral.Core;
using Marsop.Ephemeral.Numerics;
using Xunit;

namespace Marsop.Ephemeral.Tests.Core.Implementation;

public class DisjointIntervalSetTests
{
    private static readonly ILengthOperator<int, int> Operator = IntDefaultLengthOperator.Instance;

    [Fact]
    public void Constructor_LengthOperatorNull_ThrowsArgumentNullException()
    {
        Action act = () => new DisjointIntervalSet<int, int>(null!);
        act.Should().Throw<ArgumentNullException>().WithParameterName("lengthOperator");
    }

    [Fact]
    public void Constructor_ParamsArrayNull_ThrowsArgumentNullException()
    {
        IBasicInterval<int>[]? intervals = null;
        Action act = () => new DisjointIntervalSet<int, int>(Operator, intervals!);
        act.Should().Throw<ArgumentNullException>().WithParameterName("intervals");
    }

    [Fact]
    public void Constructor_ParamsArray_AddsIntervals()
    {
        var interval = IntInterval.CreateClosed(1, 5);
        var set = new DisjointIntervalSet<int, int>(Operator, interval);

        set.Count.Should().Be(1);
        set.Contains(interval).Should().BeTrue();
    }

    [Fact]
    public void Constructor_EnumerableNull_ThrowsArgumentNullException()
    {
        IEnumerable<IBasicInterval<int>>? intervals = null;
        Action act = () => new DisjointIntervalSet<int, int>(Operator, intervals!);
        act.Should().Throw<ArgumentNullException>().WithParameterName("intervals");
    }

    [Fact]
    public void Constructor_Enumerable_AddsIntervals()
    {
        var intervals = new List<IBasicInterval<int>> { IntInterval.CreateClosed(1, 5), IntInterval.CreateClosed(6, 10) };
        var set = new DisjointIntervalSet<int, int>(Operator, intervals);

        set.Count.Should().Be(2);
        set.Contains(intervals[0]).Should().BeTrue();
        set.Contains(intervals[1]).Should().BeTrue();
    }

    [Fact]
    public void Count_ReturnsCorrectNumberOfIntervals()
    {
        var set = new DisjointIntervalSet<int, int>(Operator, IntInterval.CreateClosed(1, 5), IntInterval.CreateClosed(7, 10));
        set.Count.Should().Be(2);
    }

    [Fact]
    public void End_ReturnsMaximumEndOfIntervals()
    {
        var set = new DisjointIntervalSet<int, int>(Operator, IntInterval.CreateClosed(1, 5), IntInterval.CreateClosed(7, 10));
        set.End.Should().Be(10);
    }

    [Fact]
    public void Start_ReturnsMinimumStartOfIntervals()
    {
        var set = new DisjointIntervalSet<int, int>(Operator, IntInterval.CreateClosed(3, 5), IntInterval.CreateClosed(7, 10));
        set.Start.Should().Be(3);
    }

    [Fact]
    public void IsContiguous_WithContiguousIntervals_ReturnsTrue()
    {
        var set = new DisjointIntervalSet<int, int>(Operator, IntInterval.CreateClosed(1, 5), IntInterval.CreateOpen(5, 10));
        set.IsContiguous.Should().BeTrue();
    }

    [Fact]
    public void IsContiguous_WithNonContiguousIntervals_ReturnsFalse()
    {
        var set = new DisjointIntervalSet<int, int>(Operator, IntInterval.CreateClosed(1, 5), IntInterval.CreateClosed(7, 10));
        set.IsContiguous.Should().BeFalse();
    }

    [Fact]
    public void IsReadOnly_ReturnsFalse()
    {
        var set = new DisjointIntervalSet<int, int>(Operator);
        set.IsReadOnly.Should().BeFalse();
    }

    [Fact]
    public void Indexer_GetsAndSetsCorrectly()
    {
        var interval1 = IntInterval.CreateClosed(1, 5);
        var intervalOut = IntInterval.CreateClosed(6, 10);
        var intervalNew = IntInterval.CreateClosed(11, 15);
        var set = new DisjointIntervalSet<int, int>(Operator, interval1, intervalOut);

        set[1].Should().Be(intervalOut);

        Action act = () => set[1] = intervalNew;
        act.Should().Throw<NotSupportedException>();
    }

    [Fact]
    public void Add_NullItem_ThrowsArgumentNullException()
    {
        var set = new DisjointIntervalSet<int, int>(Operator);
        Action act = () => set.Add(null!);
        act.Should().Throw<ArgumentNullException>().WithParameterName("item");
    }

    [Fact]
    public void Add_OverlappingItem_ThrowsOverlapException()
    {
        var set = new DisjointIntervalSet<int, int>(Operator, IntInterval.CreateClosed(1, 5));
        Action act = () => set.Add(IntInterval.CreateClosed(3, 7));
        act.Should().Throw<OverlapException>();
    }

    [Fact]
    public void Add_ValidItem_AddsToSet()
    {
        var set = new DisjointIntervalSet<int, int>(Operator);
        var interval = IntInterval.CreateClosed(1, 5);

        set.Add(interval);

        set.Count.Should().Be(1);
        set.Contains(interval).Should().BeTrue();
    }

    [Fact]
    public void Clear_RemovesAllItems()
    {
        var set = new DisjointIntervalSet<int, int>(Operator, IntInterval.CreateClosed(1, 5));
        set.Clear();
        set.Count.Should().Be(0);
    }

    [Fact]
    public void Contains_NullItem_ReturnsFalse()
    {
        var set = new DisjointIntervalSet<int, int>(Operator, IntInterval.CreateClosed(1, 5));
        set.Contains(null!).Should().BeFalse();
    }

    [Fact]
    public void Contains_ValidItem_ReturnsTrueIfItemExists()
    {
        var interval = IntInterval.CreateClosed(1, 5);
        var set = new DisjointIntervalSet<int, int>(Operator, interval);

        set.Contains(interval).Should().BeTrue();
        set.Contains(IntInterval.CreateClosed(6, 10)).Should().BeFalse();
    }

    [Fact]
    public void CopyTo_CopiesItemsToArray()
    {
        var interval = IntInterval.CreateClosed(1, 5);
        var set = new DisjointIntervalSet<int, int>(Operator, interval);
        var array = new IBasicInterval<int>[1];

        set.CopyTo(array, 0);

        array[0].Should().Be(interval);
    }

    [Fact]
    public void GetEnumerator_IteratesThroughItems()
    {
        var interval = IntInterval.CreateClosed(1, 5);
        var set = new DisjointIntervalSet<int, int>(Operator, interval);

        using var enumerator = set.GetEnumerator();
        enumerator.MoveNext().Should().BeTrue();
        enumerator.Current.Should().Be(interval);
        enumerator.MoveNext().Should().BeFalse();
    }

    [Fact]
    public void NonGenericGetEnumerator_IteratesThroughItems()
    {
        var interval = IntInterval.CreateClosed(1, 5);
        var set = new DisjointIntervalSet<int, int>(Operator, interval);

        var enumerator = ((System.Collections.IEnumerable)set).GetEnumerator();
        enumerator.MoveNext().Should().BeTrue();
        enumerator.Current.Should().Be(interval);
        enumerator.MoveNext().Should().BeFalse();
    }

    [Fact]
    public void IndexOf_NullItem_ThrowsArgumentNullException()
    {
        var set = new DisjointIntervalSet<int, int>(Operator, IntInterval.CreateClosed(1, 5));
        Action act = () => set.IndexOf(null!);
        act.Should().Throw<ArgumentNullException>().WithParameterName("item");
    }

    [Fact]
    public void IndexOf_ReturnsCorrectIndex()
    {
        var interval = IntInterval.CreateClosed(1, 5);
        var set = new DisjointIntervalSet<int, int>(Operator, interval);

        set.IndexOf(interval).Should().Be(0);
        set.IndexOf(IntInterval.CreateClosed(6, 10)).Should().Be(-1);
    }

    [Fact]
    public void Insert_ThrowsNotSupportedException()
    {
        var set = new DisjointIntervalSet<int, int>(Operator);
        Action act = () => set.Insert(0, IntInterval.CreateClosed(1, 5));
        act.Should().Throw<NotSupportedException>().WithMessage("The Set is always ordered, please use Add()");
    }

    [Fact]
    public void Remove_NullItem_ReturnsFalse()
    {
        var set = new DisjointIntervalSet<int, int>(Operator, IntInterval.CreateClosed(1, 5));
        set.Remove(null!).Should().BeFalse();
    }

    [Fact]
    public void Remove_RemovesItem()
    {
        var interval = IntInterval.CreateClosed(1, 5);
        var set = new DisjointIntervalSet<int, int>(Operator, interval);

        set.Remove(interval).Should().BeTrue();
        set.Count.Should().Be(0);
    }

    [Fact]
    public void RemoveAt_InvalidIndex_ThrowsArgumentOutOfRangeException()
    {
        var set = new DisjointIntervalSet<int, int>(Operator, IntInterval.CreateClosed(1, 5));

        Action act1 = () => set.RemoveAt(-1);
        act1.Should().Throw<ArgumentOutOfRangeException>();

        Action act2 = () => set.RemoveAt(1);
        act2.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void RemoveAt_ValidIndex_RemovesItem()
    {
        var interval = IntInterval.CreateClosed(1, 5);
        var set = new DisjointIntervalSet<int, int>(Operator, interval);

        set.RemoveAt(0);

        set.Count.Should().Be(0);
    }
}
