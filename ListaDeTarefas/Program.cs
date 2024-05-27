using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ListaDeTarefas.Data;
using ListaDeTarefas;
using AutoMapper;
using ListaDeTarefas.Dto;
using ListaDeTarefas.Modelos;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ListaDeTarefasContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ListaDeTarefasContext") ?? throw new InvalidOperationException("Connection string 'ListaDeTarefasContext' not found.")));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var config = new AutoMapper.MapperConfiguration(cfg =>
{
    cfg.CreateMap<TarefaRequest, Tarefa>();
});

IMapper mapper = config.CreateMapper();
builder.Services.AddSingleton(mapper);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapTarefaEndpoints();

app.Run();
