
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RestauranteApp.Data;

#nullable disable

namespace RestauranteApp.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("RestauranteApp.Models.Categoria", b =>
                {
                    b.Property<int>("IdCategoria")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdCategoria"));

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("IdCategoria");

                    b.HasIndex("Nome")
                        .IsUnique();

                    b.ToTable("Categorias");

                    b.HasData(
                        new
                        {
                            IdCategoria = 1,
                            Nome = "Entradas"
                        },
                        new
                        {
                            IdCategoria = 2,
                            Nome = "Pratos Principais"
                        },
                        new
                        {
                            IdCategoria = 3,
                            Nome = "Sobremesas"
                        },
                        new
                        {
                            IdCategoria = 4,
                            Nome = "Bebidas"
                        });
                });

            modelBuilder.Entity("RestauranteApp.Models.Cliente", b =>
                {
                    b.Property<int>("IdCliente")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdCliente"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Telefone")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("IdCliente");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Clientes");
                });

            modelBuilder.Entity("RestauranteApp.Models.Mesa", b =>
                {
                    b.Property<int>("IdMesa")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdMesa"));

                    b.Property<int>("Capacidade")
                        .HasColumnType("int");

                    b.Property<int>("Numero")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("IdMesa");

                    b.HasIndex("Numero")
                        .IsUnique();

                    b.ToTable("Mesas");

                    b.HasData(
                        new
                        {
                            IdMesa = 1,
                            Capacidade = 2,
                            Numero = 1,
                            Status = "disponivel"
                        },
                        new
                        {
                            IdMesa = 2,
                            Capacidade = 4,
                            Numero = 2,
                            Status = "disponivel"
                        },
                        new
                        {
                            IdMesa = 3,
                            Capacidade = 6,
                            Numero = 3,
                            Status = "disponivel"
                        },
                        new
                        {
                            IdMesa = 4,
                            Capacidade = 8,
                            Numero = 4,
                            Status = "disponivel"
                        });
                });

            modelBuilder.Entity("RestauranteApp.Models.MetodoPagamento", b =>
                {
                    b.Property<int>("IdMetodo")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdMetodo"));

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("IdMetodo");

                    b.HasIndex("Nome")
                        .IsUnique();

                    b.ToTable("MetodosPagamento");

                    b.HasData(
                        new
                        {
                            IdMetodo = 1,
                            Nome = "Dinheiro"
                        },
                        new
                        {
                            IdMetodo = 2,
                            Nome = "Cartão Débito"
                        },
                        new
                        {
                            IdMetodo = 3,
                            Nome = "Cartão Crédito"
                        },
                        new
                        {
                            IdMetodo = 4,
                            Nome = "Pix"
                        });
                });

            modelBuilder.Entity("RestauranteApp.Models.Pagamento", b =>
                {
                    b.Property<int>("IdPagamento")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdPagamento"));

                    b.Property<DateTime?>("DataPagamento")
                        .HasColumnType("datetime2");

                    b.Property<int>("IdMetodo")
                        .HasColumnType("int");

                    b.Property<int>("IdReserva")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<decimal>("Valor")
                        .HasColumnType("decimal(10,2)");

                    b.HasKey("IdPagamento");

                    b.HasIndex("IdMetodo");

                    b.HasIndex("IdReserva")
                        .IsUnique();

                    b.ToTable("Pagamentos");
                });

            modelBuilder.Entity("RestauranteApp.Models.PedidoReserva", b =>
                {
                    b.Property<int>("IdReserva")
                        .HasColumnType("int");

                    b.Property<int>("IdProduto")
                        .HasColumnType("int");

                    b.Property<int>("Quantidade")
                        .HasColumnType("int");

                    b.HasKey("IdReserva", "IdProduto");

                    b.HasIndex("IdProduto");

                    b.ToTable("PedidosReserva");
                });

            modelBuilder.Entity("RestauranteApp.Models.Produto", b =>
                {
                    b.Property<int>("IdProduto")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdProduto"));

                    b.Property<int>("IdCategoria")
                        .HasColumnType("int");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<decimal>("Preco")
                        .HasColumnType("decimal(10,2)");

                    b.HasKey("IdProduto");

                    b.HasIndex("IdCategoria");

                    b.ToTable("Produtos");
                });

            modelBuilder.Entity("RestauranteApp.Models.Reserva", b =>
                {
                    b.Property<int>("IdReserva")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdReserva"));

                    b.Property<DateTime>("DataReserva")
                        .HasColumnType("datetime2");

                    b.Property<TimeSpan>("Horario")
                        .HasColumnType("time");

                    b.Property<int>("IdCliente")
                        .HasColumnType("int");

                    b.Property<int>("IdMesa")
                        .HasColumnType("int");

                    b.Property<int>("QuantidadePessoas")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("IdReserva");

                    b.HasIndex("IdCliente");

                    b.HasIndex("IdMesa");

                    b.ToTable("Reservas");
                });

            modelBuilder.Entity("RestauranteApp.Models.RevisaoPedido", b =>
                {
                    b.Property<int>("IdRevisao")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdRevisao"));

                    b.Property<DateTime>("DataRevisao")
                        .HasColumnType("datetime2");

                    b.Property<int>("IdReserva")
                        .HasColumnType("int");

                    b.Property<string>("Observacoes")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<decimal?>("TotalEstimado")
                        .HasColumnType("decimal(10,2)");

                    b.HasKey("IdRevisao");

                    b.HasIndex("IdReserva")
                        .IsUnique();

                    b.ToTable("RevisoesPedido");
                });

            modelBuilder.Entity("RestauranteApp.Models.Pagamento", b =>
                {
                    b.HasOne("RestauranteApp.Models.MetodoPagamento", "MetodoPagamento")
                        .WithMany("Pagamentos")
                        .HasForeignKey("IdMetodo")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("RestauranteApp.Models.Reserva", "Reserva")
                        .WithOne("Pagamento")
                        .HasForeignKey("RestauranteApp.Models.Pagamento", "IdReserva")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("MetodoPagamento");

                    b.Navigation("Reserva");
                });

            modelBuilder.Entity("RestauranteApp.Models.PedidoReserva", b =>
                {
                    b.HasOne("RestauranteApp.Models.Produto", "Produto")
                        .WithMany("PedidosReserva")
                        .HasForeignKey("IdProduto")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("RestauranteApp.Models.Reserva", "Reserva")
                        .WithMany("PedidosReserva")
                        .HasForeignKey("IdReserva")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Produto");

                    b.Navigation("Reserva");
                });

            modelBuilder.Entity("RestauranteApp.Models.Produto", b =>
                {
                    b.HasOne("RestauranteApp.Models.Categoria", "Categoria")
                        .WithMany("Produtos")
                        .HasForeignKey("IdCategoria")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Categoria");
                });

            modelBuilder.Entity("RestauranteApp.Models.Reserva", b =>
                {
                    b.HasOne("RestauranteApp.Models.Cliente", "Cliente")
                        .WithMany("Reservas")
                        .HasForeignKey("IdCliente")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("RestauranteApp.Models.Mesa", "Mesa")
                        .WithMany("Reservas")
                        .HasForeignKey("IdMesa")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Cliente");

                    b.Navigation("Mesa");
                });

            modelBuilder.Entity("RestauranteApp.Models.RevisaoPedido", b =>
                {
                    b.HasOne("RestauranteApp.Models.Reserva", "Reserva")
                        .WithOne("RevisaoPedido")
                        .HasForeignKey("RestauranteApp.Models.RevisaoPedido", "IdReserva")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Reserva");
                });

            modelBuilder.Entity("RestauranteApp.Models.Categoria", b =>
                {
                    b.Navigation("Produtos");
                });

            modelBuilder.Entity("RestauranteApp.Models.Cliente", b =>
                {
                    b.Navigation("Reservas");
                });

            modelBuilder.Entity("RestauranteApp.Models.Mesa", b =>
                {
                    b.Navigation("Reservas");
                });

            modelBuilder.Entity("RestauranteApp.Models.MetodoPagamento", b =>
                {
                    b.Navigation("Pagamentos");
                });

            modelBuilder.Entity("RestauranteApp.Models.Produto", b =>
                {
                    b.Navigation("PedidosReserva");
                });

            modelBuilder.Entity("RestauranteApp.Models.Reserva", b =>
                {
                    b.Navigation("Pagamento");

                    b.Navigation("PedidosReserva");

                    b.Navigation("RevisaoPedido");
                });

        }
    }
}
