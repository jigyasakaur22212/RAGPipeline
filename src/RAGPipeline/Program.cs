using Azure.Services;
using Core.Services;
using Infrastructure.Services;
using LLM.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IChunkingData, ChunkingData>();
builder.Services.AddSingleton<IEmbedding, Embedding>();
builder.Services.AddSingleton<ILLMGeneration, LLMGeneration>();
builder.Services.AddSingleton<ICosmosDb, CosmosDb>();
builder.Services.AddSingleton<IRouteDb, RouteDb>();
builder.Services.AddSingleton<IIngestionPipeline, IngestionPipeline>();
builder.Services.AddSingleton<IRetrivalPipeline, RetrivalPipeline>();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
