using Microsoft.EntityFrameworkCore.Migrations;

namespace ZapWeb.Migrations
{
    public partial class AddAlteracaoMensagem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Usuario",
                table: "Mensagens",
                newName: "UsuarioJson");

            migrationBuilder.AddColumn<int>(
                name: "UsuarioId",
                table: "Mensagens",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Mensagens");

            migrationBuilder.RenameColumn(
                name: "UsuarioJson",
                table: "Mensagens",
                newName: "Usuario");
        }
    }
}
