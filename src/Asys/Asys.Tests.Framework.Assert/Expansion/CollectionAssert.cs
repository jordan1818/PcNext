﻿using System.Diagnostics.CodeAnalysis;

namespace Asys.Tests.Framework.Asserts;

/// <summary>
/// A static assertion set
/// for <see cref="ICollectionAssert"/> implementation.
/// </summary>
public static partial class Assert
{
    /// <summary>
    /// Verifies that an item is contained within the collection, using a default comparer.
    /// </summary>
    /// <typeparam name="T">The type of the objects to be compared.</typeparam>
    /// <param name="expected">The expected item within the collection.</param>
    /// <param name="collection">The collection to validate the item.</param>
    /// <exception cref="AssertException">Thrown when the collection does not include the item.</exception>
    public static void Contains<T>(T? expected, IEnumerable<T?> collection) => Asserter.Contains(expected, collection);

    /// <summary>
    /// Verifies that an item is contained within the collection, using a comparer.
    /// </summary>
    /// <typeparam name="T">The type of the objects to be compared.</typeparam>
    /// <param name="expected">The expected item within the collection.</param>
    /// <param name="collection">The collection to validate the item.</param>
    /// <param name="comparer">The comparer object for the object. If null, it'll use the default comparer.</param>
    /// <exception cref="AssertException">Thrown when the collection does not include the item.</exception>
    public static void Contains<T>(T? expected, IEnumerable<T?> collection, [AllowNull] IEqualityComparer<T>? comparer = null) => Asserter.Contains(expected, collection, comparer);
}
