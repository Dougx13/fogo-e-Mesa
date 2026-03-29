using Microsoft.EntityFrameworkCore;
using RestauranteApp.Models;

namespace RestauranteApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // ── DbSets ──────────────────────────────────────────────
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Mesa> Mesas { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Reserva> Reservas { get; set; }
     
        public DbSet<MetodoPagamento> MetodosPagamento { get; set; }
        public DbSet<Pagamento> Pagamentos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ── CLIENTES ────────────────────────────────────────
            modelBuilder.Entity<Cliente>(e =>
            {
                e.HasKey(c => c.IdCliente);
                e.Property(c => c.Email).IsRequired().HasMaxLength(100);
                e.HasIndex(c => c.Email).IsUnique();
                e.Property(c => c.Telefone).HasMaxLength(20);
            });

            // ── MESAS ───────────────────────────────────────────
            modelBuilder.Entity<Mesa>(e =>
            {
                e.HasKey(m => m.IdMesa);
                e.HasIndex(m => m.Numero).IsUnique();
            });

            // ── CATEGORIAS 
            modelBuilder.Entity<Categoria>(e =>
            {
                e.HasKey(c => c.IdCategoria);
                e.HasIndex(c => c.Nome).IsUnique();
            });

            // ── PRODUTOS
            modelBuilder.Entity<Produto>(e =>
            {
                e.HasKey(p => p.IdProduto);
                e.Property(p => p.Preco).HasColumnType("decimal(10,2)").IsRequired();
                e.HasOne(p => p.Categoria)
                 .WithMany(c => c.Produtos)
                 .HasForeignKey(p => p.IdCategoria)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // ── RESERVAS
            modelBuilder.Entity<Reserva>(e =>
            {
                e.HasKey(r => r.IdReserva);
                e.Property(r => r.PedidoReserva).HasMaxLength(500);
                e.HasOne(r => r.Cliente)
                 .WithMany(c => c.Reservas)
                 .HasForeignKey(r => r.IdCliente)
                 .OnDelete(DeleteBehavior.Restrict);
                e.HasOne(r => r.Mesa)
                 .WithMany(m => m.Reservas)
                 .HasForeignKey(r => r.IdMesa)
                 .OnDelete(DeleteBehavior.Restrict);
            });

           
   

   
    
            // ── METODOS_PAGAMENTO 
            modelBuilder.Entity<MetodoPagamento>(e =>
            {
                e.HasKey(mp => mp.IdMetodo);
                e.HasIndex(mp => mp.Nome).IsUnique();
            });

            // ── PAGAMENTOS 
            modelBuilder.Entity<Pagamento>(e =>
            {
                e.HasKey(p => p.IdPagamento);
                e.HasIndex(p => p.IdReserva).IsUnique(); // 1:1 com Reserva
                e.Property(p => p.Valor).HasColumnType("decimal(10,2)").IsRequired();
                e.HasOne(p => p.Reserva)
                 .WithOne(r => r.Pagamento)
                 .HasForeignKey<Pagamento>(p => p.IdReserva)
                 .OnDelete(DeleteBehavior.Restrict);
                e.HasOne(p => p.MetodoPagamento)
                 .WithMany(mp => mp.Pagamentos)
                 .HasForeignKey(p => p.IdMetodo)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // ── SEED DATA 
            modelBuilder.Entity<Categoria>().HasData(
                new Categoria { IdCategoria = 1, Nome = "Entradas" },
                new Categoria { IdCategoria = 2, Nome = "Pratos Principais" },
                new Categoria { IdCategoria = 3, Nome = "Sobremesas" },
                new Categoria { IdCategoria = 4, Nome = "Bebidas" }
            );

            modelBuilder.Entity<MetodoPagamento>().HasData(
                new MetodoPagamento { IdMetodo = 1, Nome = "Dinheiro" },
                new MetodoPagamento { IdMetodo = 2, Nome = "Cartão Débito" },
                new MetodoPagamento { IdMetodo = 3, Nome = "Cartão Crédito" },
                new MetodoPagamento { IdMetodo = 4, Nome = "Pix" }
            );

            modelBuilder.Entity<Mesa>().HasData(
                new Mesa { IdMesa = 1, Numero = 1, Capacidade = 2, Status = "disponivel" },
                new Mesa { IdMesa = 2, Numero = 2, Capacidade = 4, Status = "disponivel" },
                new Mesa { IdMesa = 3, Numero = 3, Capacidade = 6, Status = "disponivel" },
                new Mesa { IdMesa = 4, Numero = 4, Capacidade = 8, Status = "disponivel" }
            );
        }
    }
}
