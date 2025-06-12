using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventarioDatos.Migrations
{
    /// <inheritdoc />
    public partial class octavaVersion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Equipo_Departamento_DepartamentoIdDepartamento",
                table: "Equipo");

            migrationBuilder.AlterColumn<int>(
                name: "DepartamentoIdDepartamento",
                table: "Equipo",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Equipo_Departamento_DepartamentoIdDepartamento",
                table: "Equipo",
                column: "DepartamentoIdDepartamento",
                principalTable: "Departamento",
                principalColumn: "IdDepartamento");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Equipo_Departamento_DepartamentoIdDepartamento",
                table: "Equipo");

            migrationBuilder.AlterColumn<int>(
                name: "DepartamentoIdDepartamento",
                table: "Equipo",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Equipo_Departamento_DepartamentoIdDepartamento",
                table: "Equipo",
                column: "DepartamentoIdDepartamento",
                principalTable: "Departamento",
                principalColumn: "IdDepartamento",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
