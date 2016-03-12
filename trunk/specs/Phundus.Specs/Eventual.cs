namespace Phundus.Specs
{
    using System;
    using System.Threading;
    using NUnit.Framework;
    using RestSharp;
    using TechTalk.SpecFlow.Assist;

    public class Eventual
    {
        public static T NotDefault<T>(Func<T> func)
        {
            for (var i = 1; i <= 5; i++)
            {
                var result = func();
                if (!Equals(result, default(T)))
                    return result;

                Thread.Sleep(i*200);
            }

            return default(T);
        }

// ReSharper disable once InconsistentNaming
        public static IRestResponse<T> StatusCode2xx<T>(Func<IRestResponse<T>> func)
        {
            IRestResponse<T> result = null;
            for (var i = 1; i <= 5; i++)
            {
                result = func();
                var statusCode = (int) result.StatusCode;
                if (statusCode >= 200 && statusCode < 300)
                    return result;

                Thread.Sleep(i*200);
            }
            return result;
        }

        public static void NoComparisonException(Action action)
        {
            NoException<ComparisonException>(action);
        }

        public static void NoAssertionException(Action action)
        {
            NoException<AssertionException>(action);
        }

        private static void NoException<T>(Action action) where T : Exception
        {
            T exception = default(T);
            for (var i = 1; i <= 5; i++)
            {
                try
                {
                    action();
                    return;
                }
                catch (T ex)
                {
                    exception = ex;
                }
            }
            if (exception != null)
                throw exception;
        }
    }
}