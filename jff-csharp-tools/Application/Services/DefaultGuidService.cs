using JffCsharpTools.Domain.Interfaces.Repositories;
using JffCsharpTools.Application.Interfaces;

namespace JffCsharpTools.Application.Services
{
    public class DefaultGuidService : DefaultGuidService<IDefaultGuidRepository>, IDefaultGuidService
    {
        public DefaultGuidService(IDefaultGuidRepository defaultGuidRepository) : base(defaultGuidRepository)
        {
        }
    }
}