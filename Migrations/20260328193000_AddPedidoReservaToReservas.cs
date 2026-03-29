using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestauranteApp.Migrations
{
    [DbContext(typeof(RestauranteApp.Data.AppDbContext))]
    [Migration("20260328193000_AddPedidoReservaToReservas")]
    public partial class AddPedidoReservaToReservas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PedidoReserva",
                table: "Reservas",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PedidoReserva",
                table: "Reservas");
        }
    }
}
