using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventarioDatos.Migrations
{
    /// <inheritdoc />
    public partial class sextaVersion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Empleado_Departamento_IdDepartamento",
                table: "Empleado");

            migrationBuilder.DropForeignKey(
                name: "FK_Empleado_Ubicacion_IdUbicacion",
                table: "Empleado");

            migrationBuilder.DropForeignKey(
                name: "FK_Equipo_Departamento_IdDepartamento",
                table: "Equipo");

            migrationBuilder.DropForeignKey(
                name: "FK_Equipo_Empleado_IdEmpleado",
                table: "Equipo");

            migrationBuilder.DropForeignKey(
                name: "FK_Equipo_Ubicacion_IdUbicacion",
                table: "Equipo");

            migrationBuilder.DropForeignKey(
                name: "FK_LicenciaOffice_Equipo_IdEquipo",
                table: "LicenciaOffice");

            migrationBuilder.AlterColumn<int>(
                name: "IdEquipo",
                table: "LicenciaOffice",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "IdUbicacion",
                table: "Equipo",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "IdEmpleado",
                table: "Equipo",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "IdDepartamento",
                table: "Equipo",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "IdUbicacion",
                table: "Empleado",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "IdDepartamento",
                table: "Empleado",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Empleado_Departamento_IdDepartamento",
                table: "Empleado",
                column: "IdDepartamento",
                principalTable: "Departamento",
                principalColumn: "IdDepartamento");

            migrationBuilder.AddForeignKey(
                name: "FK_Empleado_Ubicacion_IdUbicacion",
                table: "Empleado",
                column: "IdUbicacion",
                principalTable: "Ubicacion",
                principalColumn: "IdUbicacion");

            migrationBuilder.AddForeignKey(
                name: "FK_Equipo_Departamento_IdDepartamento",
                table: "Equipo",
                column: "IdDepartamento",
                principalTable: "Departamento",
                principalColumn: "IdDepartamento");

            migrationBuilder.AddForeignKey(
                name: "FK_Equipo_Empleado_IdEmpleado",
                table: "Equipo",
                column: "IdEmpleado",
                principalTable: "Empleado",
                principalColumn: "IdEmpleado");

            migrationBuilder.AddForeignKey(
                name: "FK_Equipo_Ubicacion_IdUbicacion",
                table: "Equipo",
                column: "IdUbicacion",
                principalTable: "Ubicacion",
                principalColumn: "IdUbicacion");

            migrationBuilder.AddForeignKey(
                name: "FK_LicenciaOffice_Equipo_IdEquipo",
                table: "LicenciaOffice",
                column: "IdEquipo",
                principalTable: "Equipo",
                principalColumn: "IdEquipo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Empleado_Departamento_IdDepartamento",
                table: "Empleado");

            migrationBuilder.DropForeignKey(
                name: "FK_Empleado_Ubicacion_IdUbicacion",
                table: "Empleado");

            migrationBuilder.DropForeignKey(
                name: "FK_Equipo_Departamento_IdDepartamento",
                table: "Equipo");

            migrationBuilder.DropForeignKey(
                name: "FK_Equipo_Empleado_IdEmpleado",
                table: "Equipo");

            migrationBuilder.DropForeignKey(
                name: "FK_Equipo_Ubicacion_IdUbicacion",
                table: "Equipo");

            migrationBuilder.DropForeignKey(
                name: "FK_LicenciaOffice_Equipo_IdEquipo",
                table: "LicenciaOffice");

            migrationBuilder.AlterColumn<int>(
                name: "IdEquipo",
                table: "LicenciaOffice",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "IdUbicacion",
                table: "Equipo",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "IdEmpleado",
                table: "Equipo",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "IdDepartamento",
                table: "Equipo",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "IdUbicacion",
                table: "Empleado",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "IdDepartamento",
                table: "Empleado",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Empleado_Departamento_IdDepartamento",
                table: "Empleado",
                column: "IdDepartamento",
                principalTable: "Departamento",
                principalColumn: "IdDepartamento",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Empleado_Ubicacion_IdUbicacion",
                table: "Empleado",
                column: "IdUbicacion",
                principalTable: "Ubicacion",
                principalColumn: "IdUbicacion",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Equipo_Departamento_IdDepartamento",
                table: "Equipo",
                column: "IdDepartamento",
                principalTable: "Departamento",
                principalColumn: "IdDepartamento",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Equipo_Empleado_IdEmpleado",
                table: "Equipo",
                column: "IdEmpleado",
                principalTable: "Empleado",
                principalColumn: "IdEmpleado",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Equipo_Ubicacion_IdUbicacion",
                table: "Equipo",
                column: "IdUbicacion",
                principalTable: "Ubicacion",
                principalColumn: "IdUbicacion",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LicenciaOffice_Equipo_IdEquipo",
                table: "LicenciaOffice",
                column: "IdEquipo",
                principalTable: "Equipo",
                principalColumn: "IdEquipo",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
