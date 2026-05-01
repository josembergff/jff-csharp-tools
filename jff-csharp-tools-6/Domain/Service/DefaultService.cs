
using System;
using JffCsharpTools6.Domain.Interface.Service;
using Microsoft.EntityFrameworkCore;

namespace JffCsharpTools6.Domain.Service
{
    [Obsolete("Use DefaultService in JffCsharpTools6.Application.Services instead for better separation of concerns and to avoid confusion with actual service implementations.")]
    public class DefaultService<T> : IDefaultService<T> where T : DbContext
    {
    }
}