using System;
using Xunit.Sdk;

namespace Xunit.Should
{
	/// <summary>
	/// Extensions which provide assertions to classes derived from <see cref="string"/>.
	/// </summary>
	/// <remarks></remarks>
	internal static class StringExtensions
	{
		/// <summary>
		/// Verifies that a string contains a given sub-string, using the given comparison type.
		/// </summary>
		/// <param name="actual">The string to be inspected</param>
		/// <param name="fragment">The sub-string expected to be in the string</param>
		/// <param name="comparisonType">The type of string comparison to perform</param>
		/// <exception cref="ContainsException">Thrown when the sub-string is not present inside the string</exception>
		/// <remarks></remarks>
		public static void ShouldContain(this string actual,
		                                 string fragment,
		                                 StringComparison comparisonType = StringComparison.CurrentCulture)
		{
			Assert.Contains(fragment, actual, comparisonType);
		}

		/// <summary>
		/// Verifies that a string ends with a given sub-string, using the given comparison type.
		/// </summary>
		/// <param name="actual">The actual.</param>
		/// <param name="ending">The expected end.</param>
		/// <param name="stringComparison">The string comparison.</param>
		/// <remarks></remarks>
		public static void ShouldEndWith(this string actual,
		                                 string ending,
		                                 StringComparison stringComparison = StringComparison.CurrentCulture)
		{
			if (actual.Length < ending.Length)
			{
				throw new EqualException(ending, actual);
			}
			string temp = actual.Substring(actual.Length - ending.Length);
			Assert.Equal(ending, temp, stringComparison.GetComparer());
		}

		/// <summary>
		/// Verifies that a string equals a given string, using the given comparison type.
		/// </summary>
		/// <param name="actual">The actual.</param>
		/// <param name="expected">The expected.</param>
		/// <param name="comparisonType">Type of the comparison.</param>
		/// <remarks></remarks>
		public static void ShouldEqual(this string actual,
		                               string expected,
		                               StringComparison comparisonType = StringComparison.CurrentCulture)
		{
			Assert.Equal(expected, actual, comparisonType.GetComparer());
		}

		/// <summary>
		/// Verifies that a string does not contain a given sub-string, using the given comparison type.
		/// </summary>
		/// <param name="actual">The string to be inspected</param>
		/// <param name="fragment">The sub-string which is expected not to be in the string</param>
		/// <param name="comparisonType">The type of string comparison to perform</param>
		/// <exception cref="DoesNotContainException">Thrown when the sub-string is present inside the given string</exception>
		public static void ShouldNotContain(this string actual,
		                                    string fragment,
		                                    StringComparison comparisonType = StringComparison.CurrentCulture)
		{
			Assert.DoesNotContain(fragment, actual, comparisonType);
		}

		public static void ShouldNotEndWith(this string actual,
		                                    string ending,
		                                    StringComparison comparisonType = StringComparison.CurrentCulture)
		{
			if (actual.Length < ending.Length)
			{
				return;
			}
			string temp = actual.Substring(actual.Length - ending.Length);
			Assert.NotEqual(ending, temp, comparisonType.GetComparer());
		}

		public static void ShouldNotStartWith(this string actual,
		                                      string begining,
		                                      StringComparison comparisonType = StringComparison.CurrentCulture)
		{
			if (actual.Length < begining.Length)
			{
				return;
			}
			string temp = actual.Substring(0, begining.Length);
			Assert.NotEqual(begining, temp, comparisonType.GetComparer());
		}

		/// <summary>
		/// Verifies that a string starts with a given sub-string, using the given comparison type.
		/// </summary>
		/// <param name="actual">The actual.</param>
		/// <param name="begining">The expected start.</param>
		/// <param name="comparisonType">The string comparison.</param>
		/// <remarks></remarks>
		public static void ShouldStartWith(this string actual,
		                                   string begining,
		                                   StringComparison comparisonType = StringComparison.CurrentCulture)
		{
			if (actual.Length < begining.Length)
			{
				throw new EqualException(begining, actual);
			}
			string temp = actual.Substring(0, begining.Length);
			Assert.Equal(begining, temp, comparisonType.GetComparer());
		}
	}
}