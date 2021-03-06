using System;
using System.Collections;
using System.Collections.Generic;
using Xunit.Sdk;

namespace Xunit.Should
{
	/// <summary>
	/// Extensions which provide assertions to classes derived from <see cref="IEnumerable"/> and <see cref="IEnumerable{T}"/>.
	/// </summary>
	internal static class IEnumerableExtensions
	{
		/// <summary>
		/// Verifies that a collection is empty.
		/// </summary>
		/// <param name="collection">The collection to be inspected</param>
		/// <exception cref="ArgumentNullException">Thrown when the collection is null</exception>
		/// <exception cref="EmptyException">Thrown when the collection is not empty</exception>
		public static void ShouldBeEmpty(this IEnumerable collection)
		{
			Assert.Empty(collection);
		}

		/// <summary>
		/// Verifies that a collection contains a given object.
		/// </summary>
		/// <typeparam name="T">The type of the object to be verified</typeparam>
		/// <param name="collection">The collection to be inspected</param>
		/// <param name="expected">The object expected to be in the collection</param>
		/// <exception cref="ContainsException">Thrown when the object is not present in the collection</exception>
		public static void ShouldContain<T>(this IEnumerable<T> collection, T expected)
		{
			Assert.Contains(expected, collection);
		}

		/// <summary>
		/// Verifies that a collection contains a given object, using a comparer.
		/// </summary>
		/// <typeparam name="T">The type of the object to be verified</typeparam>
		/// <param name="collection">The collection to be inspected</param>
		/// <param name="expected">The object expected to be in the collection</param>
		/// <param name="comparer">The comparer used to equate objects in the collection with the expected object</param>
		/// <exception cref="ContainsException">Thrown when the object is not present in the collection</exception>
		public static void ShouldContain<T>(this IEnumerable<T> collection, T expected, IEqualityComparer<T> comparer)
		{
			Assert.Contains(expected, collection, comparer);
		}

		public static void ShouldEqual<T>(this IEnumerable<T> actual, IEnumerable<T> expected)
		{
			ShouldEqual(actual, expected, Comparer<T>.Default);
		}

		public static void ShouldEqual<T>(this IEnumerable<T> actual, IEnumerable<T> expected, IComparer<T> comparer)
		{
			IEnumerator<T> actualEnumerator = actual.GetEnumerator();
			Assert.NotNull(actualEnumerator);
			IEnumerator<T> expectedEnumerator = expected.GetEnumerator();
			Assert.NotNull(expectedEnumerator);

			//test values
			bool actualHasValue;
			bool expectedHasValue;
			int listIndex = 0;
			while (true)
			{
				actualHasValue = actualEnumerator.MoveNext();
				expectedHasValue = expectedEnumerator.MoveNext();

				if (!actualHasValue || !expectedHasValue)
				{
					break;
				}
				if (comparer.Compare(actualEnumerator.Current, expectedEnumerator.Current) != 0)
				{
					string message = string.Format("Enumerable<{4}> not equal at position {1}.{0}Actual:    {2}{0}Expected:  {3}", Environment.NewLine, listIndex, actualEnumerator.Current, expectedEnumerator.Current, typeof(T));
					throw new AssertException(message);
				}
				listIndex++;
			}

			//ensure no remaining values after list compare
			if (actualHasValue || expectedHasValue)
			{
				string message = string.Format("Enumerables not equal in length. {0} has more items", actualHasValue ? "Actual" : "Expected");
				throw new AssertException(message);
			}
		}

		/// <summary>
		/// Verifies that a collection is not empty.
		/// </summary>
		/// <param name="collection">The collection to be inspected</param>
		/// <exception cref="ArgumentNullException">Thrown when a null collection is passed</exception>
		/// <exception cref="NotEmptyException">Thrown when the collection is empty</exception>
		public static void ShouldNotBeEmpty(this IEnumerable collection)
		{
			Assert.NotEmpty(collection);
		}

		/// <summary>
		/// Verifies that a collection does not contain a given object.
		/// </summary>
		/// <typeparam name="T">The type of the object to be compared</typeparam>
		/// <param name="expected">The object that is expected not to be in the collection</param>
		/// <param name="collection">The collection to be inspected</param>
		/// <exception cref="DoesNotContainException">Thrown when the object is present inside the container</exception>
		public static void ShouldNotContain<T>(this IEnumerable<T> collection, T expected)
		{
			Assert.DoesNotContain(expected, collection);
		}

		/// <summary>
		/// Verifies that a collection does not contain a given object, using a comparer.
		/// </summary>
		/// <typeparam name="T">The type of the object to be compared</typeparam>
		/// <param name="expected">The object that is expected not to be in the collection</param>
		/// <param name="collection">The collection to be inspected</param>
		/// <param name="comparer">The comparer used to equate objects in the collection with the expected object</param>
		/// <exception cref="DoesNotContainException">Thrown when the object is present inside the container</exception>
		public static void ShouldNotContain<T>(this IEnumerable<T> collection, T expected, IEqualityComparer<T> comparer)
		{
			Assert.DoesNotContain(expected, collection, comparer);
		}
	}
}