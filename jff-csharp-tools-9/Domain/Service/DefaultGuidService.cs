
using System;
using JffCsharpTools9.Domain.Interface.Service;
using Microsoft.EntityFrameworkCore;

namespace JffCsharpTools9.Domain.Service
{
    [Obsolete("Use DefaultGuidService in JffCsharpTools9.Application.Services instead for better separation of concerns and to avoid confusion with actual service implementations.")]
    public class DefaultGuidService<T> : IDefaultGuidService<T> where T : DbContext
    {
    }
}