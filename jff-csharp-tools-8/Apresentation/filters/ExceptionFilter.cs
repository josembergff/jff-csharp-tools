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

namespace JffCsharpTools8.Apresentation.Filters
{
    /// <summary>
    /// Global exception filter that handles and categorizes different types of exceptions across the application.
    /// This filter intercepts unhandled exceptions, logs them appropriately, and converts them into standardized API responses.
    /// Different exception types are mapped to specific HTTP status codes and user-friendly error messages.
    /// In DEBUG mode, detailed error information including stack traces is included in the response.
    /// </summary>
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        /// <summary>
        /// Logger instance for recording exception details and system events
        /// </summary>
        private readonly ILogger<ExceptionFilter> logger;

        /// <summary>
        /// Initializes a new instance of the ExceptionFilter class.
        /// </summary>
        /// <param name="logger">The logger instance used for logging exceptions and system events</param>
        public ExceptionFilter(ILogger<ExceptionFilter> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Handles exceptions that occur during action execution.
        /// Maps different exception types to appropriate HTTP status codes and error messages.
        /// Logs exceptions with appropriate severity levels and event constants for monitoring.
        /// </summary>
        /// <param name="context">The exception context containing details about the exception and the request</param>
        public override void OnException(ExceptionContext context)
        {
            // Create standardized response object for all exceptions
            var returnObj = new DefaultResponseModel<object>();

#if DEBUG
            // In debug mode, include detailed error information for developers
            returnObj.Error = context.Exception.Message;
            returnObj.BaseException = context.Exception.GetBaseException().Message;
            returnObj.StackTrace = context.Exception.StackTrace;
#endif

            // Handle authorization and token-related exceptions
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
            // Handle identity mapping exceptions
            else if (context.Exception is IdentityNotMappedException)
            {
                returnObj.Message = "Identity mapping failure.";

            }
            else
            {
                // Handle all other unhandled exceptions
                returnObj.Message = "An unexpected error occurred.";
                returnObj.StatusCode = HttpStatusCode.InternalServerError;
                logger.LogError(EventsLogConstant.Generic_Exception_System, context.Exception, returnObj.Message);
            }
        }
    }
}