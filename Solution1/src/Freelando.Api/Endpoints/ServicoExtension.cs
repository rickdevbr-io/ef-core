﻿using Freelando.Api.Converters;
using Freelando.Api.Requests;
using Freelando.Dados;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Freelando.Api.Endpoints;

public static class ServicoExtensions
{
    public static void AddEndPointServico(this WebApplication app)
    {
        app.MapGet("/servicos", async ([FromServices] ServicoConverter converter, [FromServices] FreelandoContext contexto) =>
        {
            var servico = converter.EntityListToResponseList(contexto.Servicos.ToList());
            return Results.Ok(await Task.FromResult(servico));
        }).WithTags("Servicos").WithOpenApi();

        app.MapPost("/servico", async ([FromServices] ServicoConverter converter, [FromServices] FreelandoContext contexto, ServicoRequest servicoRequest) =>
        {
            var servico = converter.RequestToEntity(servicoRequest);
            await contexto.Servicos.AddAsync(servico);
            await contexto.SaveChangesAsync();
            return Results.Created($"/servico/{servico.Id}", servico);
        }).WithTags("Servicos").WithOpenApi();

        app.MapPut("/servico/{id}", async ([FromServices] ServicoConverter converter, [FromServices] FreelandoContext contexto, ServicoRequest servicoRequest, Guid id) =>
        {
            var servico = await contexto.Servicos.FindAsync(id);
            if(servico is null)
            {
                return Results.NotFound($"Serviço {id} não encontrado.");
            }
            var servicoAtualizado = converter.RequestToEntity(servicoRequest);
            servico.Descricao = servicoAtualizado.Descricao;
            await contexto.SaveChangesAsync();
            return Results.Ok(servico);
        }).WithTags("Servicos").WithOpenApi();

        app.MapDelete("/servico/{id}", async ([FromServices] ServicoConverter converter, [FromServices] FreelandoContext contexto, Guid id) =>
        {
            var servico = await contexto.Servicos.FindAsync(id);
            if (servico is null)
            {
                return Results.NotFound($"Serviço {id} não encontrado.");
            }
            contexto.Servicos.Remove(servico);
            await contexto.SaveChangesAsync();
            return Results.NoContent();
        }).WithTags("Servicos").WithOpenApi();
    }
}