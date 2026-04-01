using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestauranteApp.Migrations
{
    [DbContext(typeof(RestauranteApp.Data.AppDbContext))]
    [Migration("20260401201000_AddLoginAndRolesToClientes")]
    public partial class AddLoginAndRolesToClientes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Perfil",
                table: "Clientes",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "Cliente");

            migrationBuilder.AddColumn<string>(
                name: "Senha",
                table: "Clientes",
                type: "nvarchar(60)",
                maxLength: 60,
                nullable: false,
                defaultValue: "1234");

            migrationBuilder.Sql(@"
IF NOT EXISTS (SELECT 1 FROM Clientes WHERE Email = 'admin@fogomesa.com')
BEGIN
    SET IDENTITY_INSERT Clientes ON;
    INSERT INTO Clientes (IdCliente, Nome, Telefone, Email, Senha, Perfil)
    VALUES (999, 'Administrador', '(00) 00000-0000', 'admin@fogomesa.com', 'admin123', 'Administrador');
    SET IDENTITY_INSERT Clientes OFF;
END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Clientes WHERE Email = 'admin@fogomesa.com';");

            migrationBuilder.DropColumn(
                name: "Perfil",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "Senha",
                table: "Clientes");
        }
    }
}
