using JffCsharpTools.Application.Interfaces;
using JffCsharpTools.Domain.Interfaces.Repositories;

namespace JffCsharpTools.Application.Services
{
    public class DefaultService : DefaultService<IDefaultRepository>, IDefaultService
    {
        public DefaultService(IDefaultRepository defaultRepository) : base(defaultRepository)
        {
        }
    }
}