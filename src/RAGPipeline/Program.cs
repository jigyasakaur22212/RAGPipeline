using Azure.Services;
using Core.Services;
using Infrastructure.Services;
using LLM.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IIngestionPipeline, IngestionPipeline>();
builder.Services.AddSingleton<IEmbedding, Embedding>();
builder.Services.AddSingleton<IRouteDb, RouteDb>();
builder.Services.AddSingleton<IChunkingData, ChunkingData>();
builder.Services.AddSingleton<ICosmosDb, CosmosDb>();
builder.Services.AddSingleton<IRetrivalPipeline, RetrivalPipeline>();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
