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
        bool filterCurrentUser = true)
        where TContext : DbContext
        where TEntity : DefaultEntity<TEntity>, new()
    {
        group.MapGet(route, async (HttpContext ctx) => filterCurrentUser ? (await service.GetByUser<TEntity>(ctx.CurrentUserId())).ReturnResult() : (await service.Get<TEntity>()).ReturnResult());

        group.MapGet($"{route}/{{id}}", async (HttpContext ctx, int id) =>
            (await service.GetByKey<TEntity, int>(ctx.CurrentUserId(), id, filterCurrentUser: filterCurrentUser)).ReturnResult());

        group.MapPost(route, async (HttpContext ctx, TEntity req) =>
             (await service.Create(ctx.CurrentUserId(), req, filterCurrentUser: filterCurrentUser)).ReturnResult());

        group.MapPut($"{route}/{{id}}", async (HttpContext ctx, int id, TEntity req) =>
            (await service.UpdateByKey(ctx.CurrentUserId(), req, id, filterCurrentUser: filterCurrentUser)).ReturnResult());

        group.MapDelete($"{route}/{{id}}", async (HttpContext ctx, int id) =>
            (await service.DeleteByKey<TEntity, int>(ctx.CurrentUserId(), id, filterCurrentUser: filterCurrentUser)).ReturnResult());

        return group;
    }
}