using Freelando.Api.Converters;
using Freelando.Api.Requests;
using Freelando.Dados;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Freelando.Api.Endpoints;

public static class ProjetoExtension
{
    public static void AddEndPointProjeto(this WebApplication app)
    {
        app.MapGet("/projetos", async ([FromServices] ProjetoConverter converter, [FromServices] FreelandoContext contexto) =>
        {
            var projetos = converter.EntityListToResponseList(contexto.Projetos.Include(p => p.Cliente).Include(p => p.Especialidades).ToList());
            return Results.Ok(await Task.FromResult(projetos));
        }).WithTags("Projeto").WithOpenApi();
        
        app.MapPost("/projeto", async ([FromServices] ProjetoConverter converter, [FromServices] FreelandoContext contexto, ProjetoRequest projetoRequest) =>
        {
            var projeto = converter.RequestToEntity(projetoRequest);
            await contexto.Projetos.AddAsync(projeto);
            await contexto.SaveChangesAsync();
            return Results.Created($"/projeto/{projeto.Id}", projeto);
        }).WithTags("Projeto").WithOpenApi();

        app.MapPut("/projeto/{id}", async ([FromServices] ProjetoConverter converter, [FromServices] FreelandoContext contexto, ProjetoRequest projetoRequest, Guid id) =>
        {
            var projeto = await contexto.Projetos.FindAsync(id);
            if(projeto is null)
            {
                return Results.NotFound($"Projeto {id} não encontrado.");
            }
            var projetoAtualizado = converter.RequestToEntity(projetoRequest);
            projeto.Descricao = projetoAtualizado.Descricao;
            await contexto.SaveChangesAsync();
            return Results.Ok(projeto);
        }).WithTags("Projeto").WithOpenApi();

        app.MapDelete("/projeto/{id}", async ([FromServices] ProjetoConverter converter, [FromServices] FreelandoContext contexto, Guid id) =>
        {
            var projeto = await contexto.Projetos.FindAsync(id);
            if (projeto is null)
            {
                return Results.NotFound($"Projeto {id} não encontrado.");
            }
            contexto.Projetos.Remove(projeto);
            await contexto.SaveChangesAsync();
            return Results.NoContent();
        }).WithTags("Projeto").WithOpenApi();
    }
}