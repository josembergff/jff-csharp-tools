using System;
using System.Threading.Tasks;
using Jff.Csharp.Tools.Domain.Exceptions;
using Microsoft.Extensions.Logging;

public class TaskExecutor
{
    private readonly ILogger<TaskExecutor> _logger;

    public TaskExecutor(ILogger<TaskExecutor> logger)
    {
        _logger = logger;
    }

    public async Task Execute(Func<Task> action)
    {
        try
        {
            await action();
        }
        catch (DomainException ex)
        {
            _logger.LogWarning(ex, "Erro de negócio");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro técnico");
            throw; // importante para retry/DLQ
        }
    }
}