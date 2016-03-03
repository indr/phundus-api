﻿namespace Phundus
{
    using System;

    /// <summary>
    /// Helper class for guard statements, which allow prettier
    ///             code for guard clauses
    /// 
    /// </summary>
    public class Guard
    {
        /// <summary>
        /// Will throw a <see cref="T:System.InvalidOperationException"/> if the assertion
        ///             is true, with the specificied message.
        /// 
        /// </summary>
        /// <param name="assertion">if set to <c>true</c> [assertion].</param><param name="message">The message.</param>
        /// <example>
        /// Sample usage:
        /// 
        /// <code>
        /// Guard.Against(string.IsNullOrEmpty(name), "Name must have a value");
        /// 
        /// </code>
        /// 
        /// </example>
        public static void Against(bool assertion, string message)
        {
            if (assertion)
                throw new InvalidOperationException(message);
        }

        /// <summary>
        /// Will throw exception of type <typeparamref name="TException"/>
        ///             with the specified message if the assertion is true
        /// 
        /// </summary>
        /// <typeparam name="TException"/><param name="assertion">if set to <c>true</c> [assertion].</param><param name="message">The message.</param>
        /// <example>
        /// Sample usage:
        /// 
        /// <code>
        /// 
        /// </code>
        /// 
        /// </example>
        public static void Against<TException>(bool assertion, string message) where TException : Exception
        {
            if (!assertion)
                return;
            throw (TException) Activator.CreateInstance(typeof (TException), new object[]
                                                                                 {
                                                                                     message
                                                                                 });
        }
    }
}