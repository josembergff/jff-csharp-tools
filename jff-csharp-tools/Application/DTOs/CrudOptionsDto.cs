namespace JffCsharpTools.Application.DTOs
{
    public class CrudDto
    {
        public bool ExcludeGetAll { get; set; } = false;
        public bool ExcludeGetById { get; set; } = false;
        public bool ExcludeCreate { get; set; } = false;
        public bool ExcludeUpdate { get; set; } = false;
        public bool ExcludeDelete { get; set; } = false;
        public bool IgnoreCurrentUser { get; set; } = false;

        public CrudDto()
        {
        }

        public CrudDto(bool excludeGetAll = false, bool excludeGetById = false, bool excludeCreate = false, bool excludeUpdate = false, bool excludeDelete = false, bool ignoreCurrentUser = false)
        {
            ExcludeGetAll = excludeGetAll;
            ExcludeGetById = excludeGetById;
            ExcludeCreate = excludeCreate;
            ExcludeUpdate = excludeUpdate;
            ExcludeDelete = excludeDelete;
            IgnoreCurrentUser = ignoreCurrentUser;
        }
    }
}