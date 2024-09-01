
using Freelando.Api.Converters;
using Freelando.Api.Requests;
using Freelando.Dados;
using Freelando.Modelo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Freelando.Api.Endpoints;

public static class EspecialidadeExtension
{
    public static void AddEndPointEspecialidade(this WebApplication app)
    {
        app.MapGet("/especialidades", async ([FromServices] EspecialidadeConverter converter, [FromServices] FreelandoContext contexto) =>
        {
            var especialidades = converter.EntityListToResponseList(contexto.Especialidades.ToList());
            return Results.Ok((especialidades));
        }).WithTags("Especialidade").WithOpenApi();

        app.MapPost("/especialidade", async ([FromServices] EspecialidadeConverter converter, [FromServices] FreelandoContext contexto, EspecialidadeRequest especialidadeRequest) =>
        {
            var especialidade = converter.RequestToEntity(especialidadeRequest);
            await contexto.Especialidades.AddAsync(especialidade);
            await contexto.SaveChangesAsync();
            return Results.Created($"/especialidade/{especialidade.Id}", especialidade);
        }).WithTags("Especialidade").WithOpenApi();

        app.MapPut("/especialidade/{id}", async ([FromServices] EspecialidadeConverter converter, [FromServices] FreelandoContext contexto, EspecialidadeRequest especialidadeRequest, Guid id) =>
        {
            var especialidade = await contexto.Especialidades.FindAsync(id);
            if(especialidade is null)
            {
                return Results.NotFound($"Especialidade {id} não encontrada");
            }
            var especialidadeAtualizada = converter.RequestToEntity(especialidadeRequest);
            especialidade.Descricao = especialidadeAtualizada.Descricao;
            await contexto.SaveChangesAsync();
            return Results.Ok(especialidade);
        }).WithTags("Especialidade").WithOpenApi();

        app.MapDelete("/especialidade/{id}", async ([FromServices] EspecialidadeConverter converter, [FromServices] FreelandoContext contexto, Guid id) =>
        {
            var especialidade = await contexto.Especialidades.FindAsync(id);
            if (especialidade is null)
            {
                return Results.NotFound($"Especialidade {id} não encontrada");
            }
            contexto.Especialidades.Remove(especialidade);
            await contexto.SaveChangesAsync();
            return Results.NoContent();
        }).WithTags("Especialidade").WithOpenApi();

        app.MapGet("/especialidade/{letraInicial}", async ([FromServices] EspecialidadeConverter converter, [FromServices] FreelandoContext contexto, string letraInicial) =>
        {
            if(letraInicial.Length != 1 || string.IsNullOrEmpty(letraInicial))
            {
                return Results.BadRequest("Por favor informar apenas uma letra para pesquisa.");
            }
            if (!char.IsUpper(letraInicial[0]))
            {
                return Results.BadRequest("Por favor informar uma letra maíscula para pesquisa.");
            }
            Expression<Func<Especialidade, bool>> filtroExpression = null;
            filtroExpression = especialidade => especialidade.Descricao.StartsWith(letraInicial);
            IQueryable<Especialidade> especialidades = contexto.Especialidades;
            especialidades = especialidades.Where(filtroExpression);
            return Results.Ok((especialidades));
        }).WithTags("Especialidade").WithOpenApi();
    }
}