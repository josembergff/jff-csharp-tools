using JffCsharpTools.Application.DTOs;
using JffCsharpTools.Domain.Entity;
using JffCsharpTools9.Application.Interfaces;
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
        CrudDto CrudDto = default)
        where TContext : DbContext
        where TEntity : DefaultEntity<TEntity>, new()
    {
        if (!CrudDto.ExcludeGetAll)
            group.MapGet(route, async (HttpContext ctx) => CrudDto.IgnoreCurrentUser ? (await service.GetByUser<TEntity>(ctx.CurrentUserId())).ReturnResult() : (await service.Get<TEntity>()).ReturnResult());

        if (!CrudDto.ExcludeGetById)
            group.MapGet($"{route}/{{id}}", async (HttpContext ctx, int id) =>
            (await service.GetByKey<TEntity, int>(ctx.CurrentUserId(), id, filterCurrentUser: CrudDto.IgnoreCurrentUser)).ReturnResult());
        if (!CrudDto.ExcludeCreate)
            group.MapPost(route, async (HttpContext ctx, TEntity req) =>
             (await service.Create(ctx.CurrentUserId(), req, filterCurrentUser: CrudDto.IgnoreCurrentUser)).ReturnResult());

        if (!CrudDto.ExcludeUpdate)
            group.MapPut($"{route}/{{id}}", async (HttpContext ctx, int id, TEntity req) =>
            (await service.UpdateByKey(ctx.CurrentUserId(), req, id, filterCurrentUser: CrudDto.IgnoreCurrentUser)).ReturnResult());

        if (!CrudDto.ExcludeDelete)
            group.MapDelete($"{route}/{{id}}", async (HttpContext ctx, int id) =>
            (await service.DeleteByKey<TEntity, int>(ctx.CurrentUserId(), id, filterCurrentUser: CrudDto.IgnoreCurrentUser)).ReturnResult());

        return group;
    }
}