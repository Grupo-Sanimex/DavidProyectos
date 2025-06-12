using InventarioDatos.Datos;
using InventarioNegocio.Departamentos;
using InventarioNegocio.Empleados;
using InventarioNegocio.Equipos;
using InventarioNegocio.LincenciasOffice;
using InventarioNegocio.Roles;
using InventarioNegocio.Ubicaciones;
using InventarioNegocio.Usuarios;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using InventarioNegocio.Herramientas;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var misReglasCors = "ReglasCors";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: misReglasCors,
                      builder =>
                      {
                          builder.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                      });
});

// Add services to the container.  

builder.Services.AddDbContext<DatosDbContext>(options =>
   options.UseMySql(
       builder.Configuration.GetConnectionString("DefaultConnection"),
       ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))));
builder.Services.AddScoped<IDepartamentoNegocio, DepartamentoNegocio>();
builder.Services.AddScoped<IEmpleadoNegocio, EmpleadoNegocio>();
builder.Services.AddScoped<IEquipoNegocio, EquipoNegocio>();
builder.Services.AddScoped<ILicenciaOfficeNegocio, LicenciaOfficeNegocio>();
builder.Services.AddScoped<IRolNegocio, RolNegocio>();
builder.Services.AddScoped<IUbicacionNegocio, UbicacionNegocio>();
builder.Services.AddScoped<IUsuariosNegocio, UsuariosNegocio>();
builder.Services.AddScoped<Md5>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//========================================= JWT =======================================
builder.Configuration.AddJsonFile("appsettings.json");
var secretKey = builder.Configuration.GetSection("settings").GetSection("secretKey").ToString();// "=Codig0Estudiant3=";
var keyBytes = Encoding.UTF8.GetBytes(secretKey);

builder.Services.AddAuthentication(config => {

    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(config => {
    config.RequireHttpsMetadata = false;
    config.SaveToken = false;
    config.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(misReglasCors);

app.UseAuthorization();

app.MapControllers();

app.Run();
