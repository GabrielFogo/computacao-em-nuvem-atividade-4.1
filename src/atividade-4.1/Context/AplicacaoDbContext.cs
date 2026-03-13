using atividade_4._1.Entities;
using Microsoft.EntityFrameworkCore;

namespace atividade_4._1.Context;

public sealed class AplicacaoDbContext : DbContext
{
    public AplicacaoDbContext(DbContextOptions<AplicacaoDbContext> contextOptions) : base(contextOptions)
    {
    }

    public DbSet<Produto> Produtos { get; set; }
    public DbSet<Imagem> Imagens { get; set; }
}
