using System;
using Microsoft.EntityFrameworkCore;

namespace JffCsharpTools8.Domain.Interface.Service
{
    [Obsolete("Use IDefaultService in JffCsharpTools8.Application.Interfaces instead for better separation of concerns and to avoid confusion with actual service implementations.")]
    public interface IDefaultService<T> where T : DbContext
    {
    }

}