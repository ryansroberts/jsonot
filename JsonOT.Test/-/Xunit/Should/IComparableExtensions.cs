using System;
using Xunit.Sdk;

namespace Xunit.Should
{
	internal static class ComparableShouldExtensions
	{
		public static void ShouldBeGreaterThan<T>(this T actual, T expected) where T : IComparable<T>
		{
			if (actual.CompareTo(expected) <= 0)
			{
				throw new RangeException(actual, expected, false, null, false);
			}
		}

		public static void ShouldBeGreaterThanOrEqual<T>(this T actual, T expected) where T : IComparable<T>
		{
			if (actual.CompareTo(expected) < 0)
			{
				throw new RangeException(actual, expected, true, null, false);
			}
		}

		public static void ShouldBeLessThan<T>(this T actual, T expected) where T : IComparable<T>
		{
			if (actual.CompareTo(expected) >= 0)
			{
				throw new RangeException(actual, null, false, expected, false);
			}
		}

		public static void ShouldBeLessThanOrEqual<T>(this T actual, T expected) where T : IComparable<T>
		{
			if (actual.CompareTo(expected) > 0)
			{
				throw new RangeException(actual, null, false, expected, true);
			}
		}
	}

	internal class RangeException : AssertException
	{
		private readonly string _actual;
		private readonly string _high;
		private readonly bool _highInclusive;
		private readonly string _low;
		private readonly bool _lowInclusive;

		/// <summary>
		/// Initializes a new instance of the <see cref="RangeException"/> class.
		/// </summary>
		/// <param name="actual">The actual.</param>
		/// <param name="low">The low.</param>
		/// <param name="lowInclusive">if set to <c>true</c> low is considered in the range.</param>
		/// <param name="high">The high.</param>
		/// <param name="highInclusive">if set to <c>true</c> high is considered in the range.</param>
		/// <remarks></remarks>
		public RangeException(object actual, object low, bool lowInclusive, object high, bool highInclusive)
			: base("Assert.InRange() Failure")
		{
			_actual = actual == null ? null : actual.ToString();
			_low = low == null ? null : low.ToString();
			_lowInclusive = lowInclusive;
			_high = high == null ? null : high.ToString();
			_highInclusive = highInclusive;
		}

		/// <summary>
		/// Gets the actual object value
		/// </summary>
		public string Actual
		{
			get { return _actual; }
		}

		/// <summary>
		/// Gets the high value of the range
		/// </summary>
		public string High
		{
			get { return _high; }
		}

		/// <summary>
		/// Gets a value indicating whether high is considered in the range.
		/// </summary>
		/// <remarks></remarks>
		public bool HighInclusive
		{
			get { return _highInclusive; }
		}

		/// <summary>
		/// Gets the low value of the range
		/// </summary>
		public string Low
		{
			get { return _low; }
		}

		/// <summary>
		/// Gets a value indicating whether low is considered in the range.
		/// </summary>
		/// <remarks></remarks>
		public bool LowInclusive
		{
			get { return _lowInclusive; }
		}

		/// <summary>
		/// Gets a message that describes the current exception.
		/// </summary>
		/// <returns>
		/// The error message that explains the reason for the exception, or an empty string("").
		/// </returns>
		public override string Message
		{
			get
			{
				return string.Format("{0}\r\nRange:  {1}{2} - {3}{4}\r\nActual: {5}",
				                     base.Message,
				                     LowInclusive ? "[" : "(",
				                     Low,
				                     High,
				                     HighInclusive ? "]" : ")",
				                     Actual ?? "(null)");
			}
		}
	}
}