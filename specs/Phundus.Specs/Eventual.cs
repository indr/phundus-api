namespace Phundus.Specs
{
    using System;
    using System.Threading;
    using NUnit.Framework;
    using RestSharp;
    using TechTalk.SpecFlow.Assist;

    public class Eventual
    {
        private const int RetryCount = 5;
        private const int Timeout = 200;

        public static T NotDefault<T>(Func<T> func, bool throwAfterRetrys = false)
        {
            for (var i = 1; i <= RetryCount; i++)
            {
                var result = func();
                if (!Equals(result, default(T)))
                    return result;

                Thread.Sleep(i * Timeout);
            }

            if (throwAfterRetrys)
                throw new Exception("Result is still default(T) after " + RetryCount + " calls.");
            return default(T);
        }

// ReSharper disable once InconsistentNaming
        public static IRestResponse<T> StatusCode2xx<T>(Func<IRestResponse<T>> func)
        {
            IRestResponse<T> result = null;
            for (var i = 1; i <= RetryCount; i++)
            {
                result = func();
                var statusCode = (int) result.StatusCode;
                if (statusCode >= 200 && statusCode < 300)
                    return result;

                Thread.Sleep(i * Timeout);
            }
            return result;
        }

        public static void NoTestException(Action action)
        {
            Exception exception = null;
            for (var i = 1; i <= RetryCount; i++)
            {
                try
                {
                    action();
                    return;
                }
                catch (ComparisonException ex)
                {
                    exception = ex;
                }
                catch (AssertionException ex)
                {
                    exception = ex;
                }
                Thread.Sleep(i * Timeout);
            }
            if (exception != null)
                throw exception;
        }
    }
}