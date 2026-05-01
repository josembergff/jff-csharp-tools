using System;
using JffCsharpTools.Domain.Entity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using JffCsharpTools.Application.DTOs;
using JffCsharpTools9.Application.Interfaces;

public static class CrudGuidEndpoints
{
    public static RouteGroupBuilder MapGuidCrud<TEntity, TContext>(
        this RouteGroupBuilder group,
        string route,
        IDefaultGuidService<TContext> service,
        CrudDto CrudDto = default)
        where TContext : DbContext
        where TEntity : DefaultGuidEntity<TEntity>, new()
    {
        if (!CrudDto.ExcludeGetAll)
            group.MapGet(route, async (HttpContext ctx) => CrudDto.IgnoreCurrentUser ? (await service.GetByUser<TEntity>(ctx.CurrentGuidUserId())).ReturnResult() : (await service.Get<TEntity>()).ReturnResult());

        if (!CrudDto.ExcludeGetById)
            group.MapGet($"{route}/{{id}}", async (HttpContext ctx, Guid id) =>
            (await service.GetByKey<TEntity, Guid>(ctx.CurrentGuidUserId(), id, filterCurrentUser: CrudDto.IgnoreCurrentUser)).ReturnResult());

        if (!CrudDto.ExcludeCreate)
            group.MapPost(route, async (HttpContext ctx, TEntity req) =>
             (await service.Create(ctx.CurrentGuidUserId(), req, filterCurrentUser: CrudDto.IgnoreCurrentUser)).ReturnResult());

        if (!CrudDto.ExcludeUpdate)
            group.MapPut($"{route}/{{id}}", async (HttpContext ctx, Guid id, TEntity req) =>
            (await service.UpdateByKey(ctx.CurrentGuidUserId(), req, id, filterCurrentUser: CrudDto.IgnoreCurrentUser)).ReturnResult());

        if (!CrudDto.ExcludeDelete)
            group.MapDelete($"{route}/{{id}}", async (HttpContext ctx, Guid id) =>
            (await service.DeleteByKey<TEntity, Guid>(ctx.CurrentGuidUserId(), id, filterCurrentUser: CrudDto.IgnoreCurrentUser)).ReturnResult());

        return group;
    }
}