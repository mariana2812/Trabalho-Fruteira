using Trabalho_Fruteira.Models;
using Microsoft.EntityFrameworkCore;

namespace Trabalho_Fruteira.Data
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(
            DbContextOptions<AppDbContext> options
        )
            : base(options)
        {
        }

        protected override void OnModelCreating(
            ModelBuilder modelBuilder
        )
        {

            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(AppDbContext).Assembly
            );

        }

        public DbSet<Fruta> Fruta { get; set; }

        public DbSet<Cliente> Cliente { get; set; }

        public DbSet<Categoria> Categoria { get; set; }

        public DbSet<Pedido> Pedido { get; set; }

        public DbSet<ItemPedido> ItemPedido { get; set; }

    }
}