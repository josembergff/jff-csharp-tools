using JffCsharpTools.Domain.Entity;
using JffCsharpTools9.Domain.Interface.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

public static class CrudEndpoints
{
    public static RouteGroupBuilder MapCrud<TEntity, TContext>(
        this RouteGroupBuilder group,
        string route,
        IDefaultService<TContext> service,
        CrudOptions crudOptions = default)
        where TContext : DbContext
        where TEntity : DefaultEntity<TEntity>, new()
    {
        if (!crudOptions.ExcludeGetAll)
            group.MapGet(route, async (HttpContext ctx) => crudOptions.IgnoreCurrentUser ? (await service.GetByUser<TEntity>(ctx.CurrentUserId())).ReturnResult() : (await service.Get<TEntity>()).ReturnResult());

        if (!crudOptions.ExcludeGetById)
            group.MapGet($"{route}/{{id}}", async (HttpContext ctx, int id) =>
            (await service.GetByKey<TEntity, int>(ctx.CurrentUserId(), id, filterCurrentUser: crudOptions.IgnoreCurrentUser)).ReturnResult());
        if (!crudOptions.ExcludeCreate)
            group.MapPost(route, async (HttpContext ctx, TEntity req) =>
             (await service.Create(ctx.CurrentUserId(), req, filterCurrentUser: crudOptions.IgnoreCurrentUser)).ReturnResult());

        if (!crudOptions.ExcludeUpdate)
            group.MapPut($"{route}/{{id}}", async (HttpContext ctx, int id, TEntity req) =>
            (await service.UpdateByKey(ctx.CurrentUserId(), req, id, filterCurrentUser: crudOptions.IgnoreCurrentUser)).ReturnResult());

        if (!crudOptions.ExcludeDelete)
            group.MapDelete($"{route}/{{id}}", async (HttpContext ctx, int id) =>
            (await service.DeleteByKey<TEntity, int>(ctx.CurrentUserId(), id, filterCurrentUser: crudOptions.IgnoreCurrentUser)).ReturnResult());

        return group;
    }
}