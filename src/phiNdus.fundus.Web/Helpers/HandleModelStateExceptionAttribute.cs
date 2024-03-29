﻿namespace phiNdus.fundus.Web.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;

    public class ModelStateException : Exception
    {
        private ModelStateException()
        {
            Errors = new Dictionary<string, string>();
        }

        public ModelStateException(ModelStateDictionary modelState)
            : this()
        {
            ModelState = modelState;
            if (!modelState.IsValid)
            {
                foreach (KeyValuePair<string, ModelState> state in modelState)
                {
                    if (state.Value.Errors.Count > 0)
                    {
                        Errors.Add(state.Key, state.Value.Errors[0].ErrorMessage);
                    }
                }
            }
        }

        public Dictionary<string, string> Errors { get; private set; }

        public ModelStateDictionary ModelState { get; private set; }

        public override string Message
        {
            get
            {
                if (Errors.Count > 0)
                {
                    return Errors.First().Value;
                }
                return null;
            }
        }
    }

    /// <summary>
    /// Represents errors that occur due to invalid application model state.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public sealed class HandleModelStateExceptionAttribute : FilterAttribute, IExceptionFilter
    {
        #region IExceptionFilter Members

        /// <summary>
        /// Called when an exception occurs and processes <see cref="ModelStateException"/> object.
        /// </summary>
        /// <param name="filterContext">Filter context.</param>
        public void OnException(ExceptionContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }


            // handle modelStateException

            if (filterContext.Exception != null &&
                typeof (ModelStateException).IsInstanceOfType(filterContext.Exception) &&
                !filterContext.ExceptionHandled)
            {
                filterContext.ExceptionHandled = true;

                filterContext.HttpContext.Response.Clear();

                filterContext.HttpContext.Response.ContentEncoding = Encoding.UTF8;

                filterContext.HttpContext.Response.HeaderEncoding = Encoding.UTF8;

                filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;

                filterContext.HttpContext.Response.StatusCode = 400;

                filterContext.Result = new ContentResult
                                           {
                                               Content = (filterContext.Exception as ModelStateException).Message,
                                               ContentEncoding = Encoding.UTF8,
                                           };
            }
        }

        #endregion
    }
}