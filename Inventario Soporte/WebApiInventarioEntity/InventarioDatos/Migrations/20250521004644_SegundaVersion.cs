using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventarioDatos.Migrations
{
    /// <inheritdoc />
    public partial class SegundaVersion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Acceso",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "Correo",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "UsuarioAd",
                table: "Usuario");

            migrationBuilder.RenameColumn(
                name: "UsuarioWindows",
                table: "Usuario",
                newName: "UsuarioSesion");

            migrationBuilder.RenameColumn(
                name: "Pass",
                table: "Usuario",
                newName: "Contracena");

            migrationBuilder.AddColumn<string>(
                name: "Acceso",
                table: "Empleado",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Correo",
                table: "Empleado",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Pass",
                table: "Empleado",
                type: "varchar(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "UsuarioAD",
                table: "Empleado",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "UsuarioWindows",
                table: "Empleado",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Acceso",
                table: "Empleado");

            migrationBuilder.DropColumn(
                name: "Correo",
                table: "Empleado");

            migrationBuilder.DropColumn(
                name: "Pass",
                table: "Empleado");

            migrationBuilder.DropColumn(
                name: "UsuarioAD",
                table: "Empleado");

            migrationBuilder.DropColumn(
                name: "UsuarioWindows",
                table: "Empleado");

            migrationBuilder.RenameColumn(
                name: "UsuarioSesion",
                table: "Usuario",
                newName: "UsuarioWindows");

            migrationBuilder.RenameColumn(
                name: "Contracena",
                table: "Usuario",
                newName: "Pass");

            migrationBuilder.AddColumn<string>(
                name: "Acceso",
                table: "Usuario",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Correo",
                table: "Usuario",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "UsuarioAd",
                table: "Usuario",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
