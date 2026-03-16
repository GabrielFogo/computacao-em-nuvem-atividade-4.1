using atividade_4._1.Context;
using Microsoft.EntityFrameworkCore;

namespace atividade_4._1.Extensions;

public static class MigrationAndSeedersRunner
{
    public async static Task ApplyMigrationsAndSeedAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var db = scope.ServiceProvider.GetRequiredService<AplicacaoDbContext>();

        // roda as migrations e popula a tabela
        await db.Database.MigrateAsync();

        await db.SaveChangesAsync();
    }
}
