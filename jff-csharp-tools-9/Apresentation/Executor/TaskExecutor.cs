using System;
using System.Data.Common;
using System.IO;
using System.Net.Mail;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Jff.Csharp.Tools.Domain.Exceptions;
using JffCsharpTools.Apresentation.Exceptions;
using Microsoft.Extensions.Logging;

namespace JffCsharpTools.Apresentation.Executor
{
    /// <summary>
    /// Utility class for executing asynchronous tasks with comprehensive exception handling and logging.
    /// Catches specific exceptions such as UnauthorizedAccessException, TokenException, DomainException, SmtpException, DbException, NullReferenceException, FileNotFoundException, IdentityNotMappedException, and general exceptions, logging them with appropriate severity levels.
    /// </summary>
    public class TaskExecutor
    {
        private readonly ILogger<TaskExecutor> _logger;

        public TaskExecutor(ILogger<TaskExecutor> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Executes a given asynchronous action while handling and logging various exceptions that may occur during its execution.
        /// Catches specific exceptions such as UnauthorizedAccessException, TokenException, DomainException, SmtpException, DbException, NullReferenceException, FileNotFoundException, IdentityNotMappedException, and general exceptions.
        /// </summary>
        /// <param name="action">The asynchronous action to be executed</param>
        /// <param name="stoppingToken">A token to monitor for cancellation requests</param>
        /// <returns>A task representing the asynchronous operation</returns>
        public async Task Execute(Func<CancellationToken, Task> action, CancellationToken stoppingToken)
        {
            try
            {
                await action(stoppingToken);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized error access: {Message}", ex.Message);
            }
            catch (TokenException ex)
            {
                _logger.LogWarning(ex, "Token error: {Message}", ex.Message);
            }
            catch (DomainException ex)
            {
                _logger.LogWarning(ex, "Domain validation failure: {Message}", ex.Message);
            }
            catch (SmtpException ex)
            {
                _logger.LogCritical(ex, "SMTP error: {Message}", ex.Message);
            }
            catch (DbException ex)
            {
                _logger.LogCritical(ex, "Database error: {Message}", ex.Message);
            }
            catch (NullReferenceException ex)
            {
                _logger.LogError(ex, "Null reference error: {Message}", ex.Message);
            }
            catch (FileNotFoundException ex)
            {
                _logger.LogError(ex, "File not found: {Message}", ex.Message);
            }
            catch (IdentityNotMappedException ex)
            {
                _logger.LogError(ex, "Identity mapping error: {Message}", ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "General error occurred: {Message}", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Executes a given asynchronous action while handling and logging various exceptions that may occur during its execution.
        /// Catches specific exceptions such as UnauthorizedAccessException, TokenException, DomainException, SmtpException, DbException, NullReferenceException, FileNotFoundException, IdentityNotMappedException, and general exceptions.
        /// </summary>
        /// <param name="action">The asynchronous action to be executed</param>
        /// <returns>A task representing the asynchronous operation</returns>
        public async Task Execute(Func<Task> action)
        {
            try
            {
                await action();
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized error access: {Message}", ex.Message);
            }
            catch (TokenException ex)
            {
                _logger.LogWarning(ex, "Token error: {Message}", ex.Message);
            }
            catch (DomainException ex)
            {
                _logger.LogWarning(ex, "Domain validation failure: {Message}", ex.Message);
            }
            catch (SmtpException ex)
            {
                _logger.LogCritical(ex, "SMTP error: {Message}", ex.Message);
            }
            catch (DbException ex)
            {
                _logger.LogCritical(ex, "Database error: {Message}", ex.Message);
            }
            catch (NullReferenceException ex)
            {
                _logger.LogError(ex, "Null reference error: {Message}", ex.Message);
            }
            catch (FileNotFoundException ex)
            {
                _logger.LogError(ex, "File not found: {Message}", ex.Message);
            }
            catch (IdentityNotMappedException ex)
            {
                _logger.LogError(ex, "Identity mapping error: {Message}", ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "General error occurred: {Message}", ex.Message);
                throw;
            }
        }
    }
}