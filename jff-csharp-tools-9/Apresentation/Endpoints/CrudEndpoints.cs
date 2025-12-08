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
        group.MapGet(route, async () => (await service.Get<TEntity>()).ReturnResult());
        group.MapGet($"{route}/{{id}}", async (HttpContext ctx, int id) =>
            (await service.GetByKey<TEntity, int>(ctx.CurrentUserId(), id)).ReturnResult());

        group.MapPost(route, async (HttpContext ctx, TEntity req) =>
            Results.Created(route, await service.Create(ctx.CurrentUserId(), req)));

        group.MapPut($"{route}/{{id}}", async (HttpContext ctx, int id, TEntity req) =>
            Results.Ok(await service.UpdateByKey(ctx.CurrentUserId(), req, id, filterCurrentUser)));

        group.MapDelete($"{route}/{{id}}", async (HttpContext ctx, int id) =>
        {
            await service.DeleteByKey<TEntity, int>(ctx.CurrentUserId(), id);
            return Results.NoContent();
        });

        return group;
    }
}