using System;
using Microsoft.EntityFrameworkCore;

namespace JffCsharpTools9.Domain.Interface.Service
{
    [Obsolete("Use IDefaultGuidService in JffCsharpTools9.Application.Interfaces instead for better separation of concerns and to avoid confusion with actual service implementations.")]
    public interface IDefaultGuidService<T> where T : DbContext
    {
    }

}