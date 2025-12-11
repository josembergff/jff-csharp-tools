using System;
using JffCsharpTools.Domain.Entity;
using JffCsharpTools9.Domain.Interface.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

public static class CrudGuidEndpoints
{
    public static RouteGroupBuilder MapGuidCrud<TEntity, TContext>(
        this RouteGroupBuilder group,
        string route,
        IDefaultGuidService<TContext> service,
        bool filterCurrentUser = true)
        where TContext : DbContext
        where TEntity : DefaultGuidEntity<TEntity>, new()
    {
        group.MapGet(route, async (HttpContext ctx) => filterCurrentUser ? (await service.GetByUser<TEntity>(ctx.CurrentGuidUserId())).ReturnResult() : (await service.Get<TEntity>()).ReturnResult());

        group.MapGet($"{route}/{{id}}", async (HttpContext ctx, Guid id) =>
            (await service.GetByKey<TEntity, Guid>(ctx.CurrentGuidUserId(), id, filterCurrentUser: filterCurrentUser)).ReturnResult());

        group.MapPost(route, async (HttpContext ctx, TEntity req) =>
             (await service.Create(ctx.CurrentGuidUserId(), req, filterCurrentUser: filterCurrentUser)).ReturnResult());

        group.MapPut($"{route}/{{id}}", async (HttpContext ctx, Guid id, TEntity req) =>
            (await service.UpdateByKey(ctx.CurrentGuidUserId(), req, id, filterCurrentUser: filterCurrentUser)).ReturnResult());

        group.MapDelete($"{route}/{{id}}", async (HttpContext ctx, Guid id) =>
            (await service.DeleteByKey<TEntity, Guid>(ctx.CurrentGuidUserId(), id, filterCurrentUser: filterCurrentUser)).ReturnResult());
        return group;
    }
}