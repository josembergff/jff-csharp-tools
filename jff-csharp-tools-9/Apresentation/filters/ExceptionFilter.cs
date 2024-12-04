using JffCsharpTools.Domain.Constants;
using JffCsharpTools.Domain.Model;
using JffCsharpTools9.Apresentation.Exceptions;
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
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger<ExceptionFilter> logger;

        public ExceptionFilter(ILogger<ExceptionFilter> logger)
        {
            this.logger = logger;
        }

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
        }
    }
}