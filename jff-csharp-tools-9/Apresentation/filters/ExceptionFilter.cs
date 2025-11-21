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

namespace JffCsharpTools9.Apresentation.Filters
{
    /// <summary>
    /// Global exception filter that handles and standardizes error responses across the application.
    /// Converts various exception types into appropriate HTTP status codes and user-friendly messages.
    /// </summary>
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        /// <summary>
        /// Logger instance for recording exception details
        /// </summary>
        private readonly ILogger<ExceptionFilter> logger;

        /// <summary>
        /// Initializes a new instance of the ExceptionFilter
        /// </summary>
        /// <param name="logger">Logger instance for exception logging</param>
        public ExceptionFilter(ILogger<ExceptionFilter> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Handles exceptions that occur during action execution, converting them into standardized error responses.
        /// Different exception types are mapped to appropriate HTTP status codes and user-friendly messages.
        /// In DEBUG mode, detailed exception information is included in the response.
        /// </summary>
        /// <param name="context">The exception context containing details about the exception and request</param>
        public override void OnException(ExceptionContext context)
        {
            var returnObj = new DefaultResponseModel<object>();
#if DEBUG
            returnObj.Error = context.Exception.Message;
            returnObj.BaseException = context.Exception.GetBaseException().Message;
            returnObj.StackTrace = context.Exception.StackTrace;
#endif
            if (context.Exception is UnauthorizedAccessException || context.Exception is TokenException)
            {
                returnObj.Message = "Unauthorized access.";
                returnObj.StatusCode = HttpStatusCode.Unauthorized;
                logger.LogWarning(EventsLogConstant.Unauthorized_System, returnObj.Message);
            }
            else if (context.Exception is SmtpException)
            {
                returnObj.Message = "Email sending failure.";
                returnObj.StatusCode = HttpStatusCode.FailedDependency;
                logger.LogCritical(EventsLogConstant.Smtp_Exception_System, returnObj.Message);
            }
            else if (context.Exception is FileNotFoundException)
            {
                returnObj.Message = "File not found.";
                returnObj.StatusCode = HttpStatusCode.UnsupportedMediaType;
                logger.LogError(EventsLogConstant.File_NotFound_System, returnObj.Message);
            }
            else if (context.Exception is DbException)
            {
                returnObj.Message = "Database failure.";
                returnObj.StatusCode = HttpStatusCode.FailedDependency;
                logger.LogCritical(EventsLogConstant.DB_Exception_System, returnObj.Message);
            }
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