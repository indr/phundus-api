namespace Phundus.Common
{
    using System;
    using System.Text.RegularExpressions;

    public class AssertionConcern
    {
        protected AssertionConcern()
        {
        }

        public static void AssertArgumentEquals(object object1, object object2, string message)
        {
            if (!object1.Equals(object2))
            {
                throw new InvalidOperationException(message);
            }
        }

        public static void AssertArgumentFalse(bool value, string message)
        {
            if (value)
            {
                throw new InvalidOperationException(message);
            }
        }

        public static void AssertArgumentGreaterThan(long value, long than, string message)
        {
            if (value <= than)
            {
                throw new InvalidOperationException(message);
            }
        }

        public static void AssertArgumentGreaterThanZero(long value, string message)
        {
            AssertArgumentGreaterThan(value, 0, message);
        }

        public static void AssertArgumentGreaterThan(DateTime value, DateTime than, string message)
        {
            AssertArgumentGreaterThan(value.Ticks, than.Ticks, message);
        }

        public static void AssertArgumentLength(string value, int maximum, string message)
        {
            int length = value.Trim().Length;
            if (length > maximum)
            {
                throw new InvalidOperationException(message);
            }
        }

        public static void AssertArgumentLength(string value, int minimum, int maximum, string message)
        {
            int length = value.Trim().Length;
            if (length < minimum || length > maximum)
            {
                throw new InvalidOperationException(message);
            }
        }

        public static void AssertArgumentMatches(string pattern, string value, string message)
        {
            var regex = new Regex(pattern);
            if (!regex.IsMatch(value))
            {
                throw new InvalidOperationException(message);
            }
        }

        public static void AssertArgumentNotEmpty(DateTime value, string message)
        {
            if (value == DateTime.MinValue)
            {
                throw new InvalidOperationException(message);
            }
        }

        public static void AssertArgumentNotEmpty(string value, string message)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                throw new InvalidOperationException(message);
            }
        }

        public static void AssertArgumentNotEquals(object object1, object object2, string message)
        {
            if (object1.Equals(object2))
            {
                throw new InvalidOperationException(message);
            }
        }

        public static void AssertArgumentNotNull(object object1, string message)
        {
            if (object1 == null)
            {
                throw new InvalidOperationException(message);
            }
        }

        public static void AssertArgumentNotZero(int value, string message)
        {
            if (value == 0)
            {
                throw new InvalidOperationException(message);
            }
        }

        public static void AssertArgumentRange(double value, double minimum, double maximum, string message)
        {
            if (value < minimum || value > maximum)
            {
                throw new InvalidOperationException(message);
            }
        }

        public static void AssertArgumentRange(float value, float minimum, float maximum, string message)
        {
            if (value < minimum || value > maximum)
            {
                throw new InvalidOperationException(message);
            }
        }

        public static void AssertArgumentRange(int value, int minimum, int maximum, string message)
        {
            if (value < minimum || value > maximum)
            {
                throw new InvalidOperationException(message);
            }
        }

        public static void AssertArgumentRange(long value, long minimum, long maximum, string message)
        {
            if (value < minimum || value > maximum)
            {
                throw new InvalidOperationException(message);
            }
        }

        public static void AssertArgumentTrue(bool value, string message)
        {
            if (!value)
            {
                throw new InvalidOperationException(message);
            }
        }

        public static void AssertStateFalse(bool boolValue, string message)
        {
            if (boolValue)
            {
                throw new InvalidOperationException(message);
            }
        }

        public static void AssertStateTrue(bool boolValue, string message)
        {
            if (!boolValue)
            {
                throw new InvalidOperationException(message);
            }
        }
    }
}