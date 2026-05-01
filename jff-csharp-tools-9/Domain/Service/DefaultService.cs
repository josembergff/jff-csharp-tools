
using System;
using JffCsharpTools9.Domain.Interface.Service;
using Microsoft.EntityFrameworkCore;

namespace JffCsharpTools9.Domain.Service
{
    [Obsolete("Use DefaultService in JffCsharpTools9.Application.Services instead for better separation of concerns and to avoid confusion with actual service implementations.")]
    public class DefaultService<T> : IDefaultService<T> where T : DbContext
    {
    }
}