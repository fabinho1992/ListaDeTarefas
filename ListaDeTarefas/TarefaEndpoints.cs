using Microsoft.EntityFrameworkCore;
using ListaDeTarefas.Data;
using ListaDeTarefas.Modelos;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using ListaDeTarefas.Dto;
using AutoMapper;
namespace ListaDeTarefas;

public static class TarefaEndpoints
{
    
    public static void MapTarefaEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Tarefa").WithTags(nameof(Tarefa));

        group.MapGet("/", async (ListaDeTarefasContext db) =>
        {
            return await db.Tarefa.ToListAsync();
        })
        .WithName("GetAllTarefas")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Tarefa>, NotFound>> (int id, ListaDeTarefasContext db) =>
        {
            return await db.Tarefa.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is Tarefa model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetTarefaById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, TarefaRequest tarefa, ListaDeTarefasContext db) =>
        {
            var affected = await db.Tarefa
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.Titulo, tarefa.Titulo)
                    .SetProperty(m => m.Descricao, tarefa.descricao)
                    .SetProperty(m => m.Prazo, tarefa.Prazo)
                    .SetProperty(m => m.Status, tarefa.status)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateTarefa")
        .WithOpenApi();

        group.MapPost("/", async (TarefaRequest tarefaRequest, ListaDeTarefasContext db) =>
        {
            var tarefa = new Tarefa()
            {
                Titulo = tarefaRequest.Titulo,
                Descricao = tarefaRequest.descricao,
                Prazo = tarefaRequest.Prazo,
                Status = tarefaRequest.status
            };
            db.Tarefa.Add(tarefa);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Tarefa/{tarefa.Id}",tarefa);
        })
        .WithName("CreateTarefa")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, ListaDeTarefasContext db) =>
        {
            var affected = await db.Tarefa
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteTarefa")
        .WithOpenApi();
    }
}
