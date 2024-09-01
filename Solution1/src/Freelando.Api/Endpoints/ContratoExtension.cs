﻿using Freelando.Api.Converters;
using Freelando.Api.Requests;
using Freelando.Dados;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Freelando.Api.Endpoints;

public static class ContratoExtension
{
    public static void AddEndPointContrato(this WebApplication app)
    {
        app.MapGet("/contratos", async ([FromServices] ContratoConverter converter, [FromServices] FreelandoContext contexto) =>
        {
            var contrato = converter.EntityListToResponseList(contexto.Contratos.ToList());
            var entries = contexto.ChangeTracker.Entries();
            return Results.Ok(await Task.FromResult(contrato));
        }).WithTags("Contrato").WithOpenApi();

        app.MapPost("/contrato", async ([FromServices] ContratoConverter converter, [FromServices] FreelandoContext contexto, ContratoRequest contratoRequest) =>
        {
            var contrato = converter.RequestToEntity(contratoRequest);
            await contexto.Contratos.AddAsync(contrato);
            await contexto.SaveChangesAsync();
            return Results.Created($"/contrato/{contrato.Id}", contrato);
        }).WithTags("Contrato").WithOpenApi();

        app.MapPut("/contrato/{id}", async ([FromServices] ContratoConverter converter, [FromServices] FreelandoContext contexto, ContratoRequest contratoRequest, Guid id) =>
        {
            var contrato = await contexto.Contratos.FindAsync(id);
            if (contrato is null)
            {
                return Results.NotFound($"Contrato {id} não encontrado.");
            }
            var contratoAtualizado = converter.RequestToEntity(contratoRequest);
            contrato.Valor = contratoAtualizado.Valor;
            await contexto.SaveChangesAsync();
            return Results.Ok(contrato);
        }).WithTags("Contrato").WithOpenApi();

        app.MapDelete("/contrato/{id}", async ([FromServices] ContratoConverter converter, [FromServices] FreelandoContext contexto, Guid id) =>
        {
            var contrato = await contexto.Contratos.FindAsync(id);
            if (contrato is null)
            {
                return Results.NotFound($"Contrato {id} não encontrado.");
            }
            contexto.Contratos.Remove(contrato);
            await contexto.SaveChangesAsync();
            return Results.NoContent();
        }).WithTags("Contrato").WithOpenApi();
    }
}