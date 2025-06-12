using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventarioDatos.Migrations
{
    /// <inheritdoc />
    public partial class TerceraVersion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuario_Empleado_IdEmpleado",
                table: "Usuario");

            migrationBuilder.DropIndex(
                name: "IX_Usuario_IdEmpleado",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "IdEmpleado",
                table: "Usuario");

            migrationBuilder.AddColumn<int>(
                name: "EmpleadoIdEmpleado",
                table: "Usuario",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_EmpleadoIdEmpleado",
                table: "Usuario",
                column: "EmpleadoIdEmpleado");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuario_Empleado_EmpleadoIdEmpleado",
                table: "Usuario",
                column: "EmpleadoIdEmpleado",
                principalTable: "Empleado",
                principalColumn: "IdEmpleado");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuario_Empleado_EmpleadoIdEmpleado",
                table: "Usuario");

            migrationBuilder.DropIndex(
                name: "IX_Usuario_EmpleadoIdEmpleado",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "EmpleadoIdEmpleado",
                table: "Usuario");

            migrationBuilder.AddColumn<int>(
                name: "IdEmpleado",
                table: "Usuario",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_IdEmpleado",
                table: "Usuario",
                column: "IdEmpleado");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuario_Empleado_IdEmpleado",
                table: "Usuario",
                column: "IdEmpleado",
                principalTable: "Empleado",
                principalColumn: "IdEmpleado",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
