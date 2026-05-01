
using System;
using JffCsharpTools8.Domain.Interface.Service;
using Microsoft.EntityFrameworkCore;

namespace JffCsharpTools8.Domain.Service
{
    [Obsolete("Use DefaultService in JffCsharpTools8.Application.Services instead for better separation of concerns and to avoid confusion with actual service implementations.")]
    public class DefaultService<T> : IDefaultService<T> where T : DbContext
    {
    }
}