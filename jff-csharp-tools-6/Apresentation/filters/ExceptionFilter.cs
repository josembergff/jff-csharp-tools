using JffCsharpTools.Apresentation.Exceptions;
using JffCsharpTools.Domain.Constants;
using JffCsharpTools.Domain.Model;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Data.Common;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Security.Principal;

namespace JffCsharpTools6.Apresentation.Filters
{
    /// <summary>
    /// Global exception filter that intercepts and handles unhandled exceptions in the application
    /// Converts exceptions into standardized API responses with appropriate HTTP status codes
    /// Provides different error handling based on the exception type
    /// </summary>
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        /// <summary>
        /// Logger instance for recording exception details and application events
        /// </summary>
        private readonly ILogger<ExceptionFilter> logger;

        /// <summary>
        /// Initializes a new instance of the ExceptionFilter
        /// </summary>
        /// <param name="logger">Logger instance for recording exception details and application events</param>
        public ExceptionFilter(ILogger<ExceptionFilter> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Called when an unhandled exception occurs during action execution
        /// Maps different exception types to appropriate HTTP responses and status codes
        /// </summary>
        /// <param name="context">The exception context containing exception details and response information</param>
        public override void OnException(ExceptionContext context)
        {
            var returnObj = new DefaultResponseModel<object>();
#if DEBUG
            // Include detailed error information only in debug builds for security
            returnObj.Error = context.Exception.Message;
            returnObj.BaseException = context.Exception.GetBaseException().Message;
            returnObj.StackTrace = context.Exception.StackTrace;
#endif
            // Handle authorization-related exceptions
            if (context.Exception is UnauthorizedAccessException || context.Exception is TokenException)
            {
                returnObj.Message = "Unauthorized access.";
                returnObj.StatusCode = HttpStatusCode.Unauthorized;
                logger.LogWarning(EventsLogConstant.Unauthorized_System, returnObj.Message);
            }
            // Handle email sending failures
            else if (context.Exception is SmtpException)
            {
                returnObj.Message = "Email sending failure.";
                returnObj.StatusCode = HttpStatusCode.FailedDependency;
                logger.LogCritical(EventsLogConstant.Smtp_Exception_System, returnObj.Message);
            }
            // Handle file not found exceptions
            else if (context.Exception is FileNotFoundException)
            {
                returnObj.Message = "File not found.";
                returnObj.StatusCode = HttpStatusCode.UnsupportedMediaType;
                logger.LogError(EventsLogConstant.File_NotFound_System, returnObj.Message);
            }
            // Handle database-related exceptions
            else if (context.Exception is DbException)
            {
                returnObj.Message = "Database failure.";
                returnObj.StatusCode = HttpStatusCode.FailedDependency;
                logger.LogCritical(EventsLogConstant.DB_Exception_System, returnObj.Message);
            }
            // Handle identity mapping failures
            else if (context.Exception is IdentityNotMappedException)
            {
                returnObj.Message = "Identity mapping failure.";
                returnObj.StatusCode = HttpStatusCode.InternalServerError;
                logger.LogError("Identity mapping failure occurred");
            }
            // Handle any other unexpected exceptions
            else
            {
                returnObj.Message = "An unexpected error occurred.";
                returnObj.StatusCode = HttpStatusCode.InternalServerError;
                logger.LogError(context.Exception, "Unhandled exception occurred");
            }
        }
    }
}