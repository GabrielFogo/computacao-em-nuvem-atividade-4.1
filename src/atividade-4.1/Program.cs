using atividade_4._1.Context;
using atividade_4._1.Endpoints;
using atividade_4._1.Extensions;
using atividade_4._1.Services.Produtos;
using atividade_4._1.Services.StorageService;
using atividade_4._1.Settings;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AplicacaoDbContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

builder.Services.AddOpenApi();
builder.Services.AddAntiforgery();
builder.Services.AddSwaggerGen();
builder.Services.Configure<GoogleCloudSettings>(builder.Configuration.GetSection("GoogleCloud"));
builder.Services.AddScoped<IProdutoService, ProdutoService>();
builder.Services.AddScoped<IStorageService, GoogleCloudStorageService>();

var app = builder.Build();

app.MapProdutoEndpoints();
app.UseAntiforgery();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

// Cria as tabelas e insere os dados iniciais
await app.ApplyMigrationsAndSeedAsync();

app.Run();
