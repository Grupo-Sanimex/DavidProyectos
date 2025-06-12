using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventarioDatos.Migrations
{
    /// <inheritdoc />
    public partial class sentimaVersion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Equipo_Departamento_IdDepartamento",
                table: "Equipo");

            migrationBuilder.DropIndex(
                name: "IX_Equipo_IdDepartamento",
                table: "Equipo");

            migrationBuilder.DropColumn(
                name: "IdDepartamento",
                table: "Equipo");

            migrationBuilder.AddColumn<int>(
                name: "DepartamentoIdDepartamento",
                table: "Equipo",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Equipo_DepartamentoIdDepartamento",
                table: "Equipo",
                column: "DepartamentoIdDepartamento");

            migrationBuilder.AddForeignKey(
                name: "FK_Equipo_Departamento_DepartamentoIdDepartamento",
                table: "Equipo",
                column: "DepartamentoIdDepartamento",
                principalTable: "Departamento",
                principalColumn: "IdDepartamento",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Equipo_Departamento_DepartamentoIdDepartamento",
                table: "Equipo");

            migrationBuilder.DropIndex(
                name: "IX_Equipo_DepartamentoIdDepartamento",
                table: "Equipo");

            migrationBuilder.DropColumn(
                name: "DepartamentoIdDepartamento",
                table: "Equipo");

            migrationBuilder.AddColumn<int>(
                name: "IdDepartamento",
                table: "Equipo",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Equipo_IdDepartamento",
                table: "Equipo",
                column: "IdDepartamento");

            migrationBuilder.AddForeignKey(
                name: "FK_Equipo_Departamento_IdDepartamento",
                table: "Equipo",
                column: "IdDepartamento",
                principalTable: "Departamento",
                principalColumn: "IdDepartamento");
        }
    }
}
