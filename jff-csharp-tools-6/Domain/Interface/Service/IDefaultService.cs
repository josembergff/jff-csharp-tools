using System;
using Microsoft.EntityFrameworkCore;

namespace JffCsharpTools6.Domain.Interface.Service
{
    [Obsolete("Use IDefaultService in JffCsharpTools6.Application.Interfaces instead for better separation of concerns and to avoid confusion with actual service implementations.")]
    public interface IDefaultService<T> where T : DbContext
    {

    }

}