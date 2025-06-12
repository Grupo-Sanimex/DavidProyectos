using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using sanimex.webapi.Datos.Comun;
using sanimex.webapi.Datos.Servicio.CarritoServico;
using sanimex.webapi.Datos.Servicio.ClienteServicio.Implementacion;
using sanimex.webapi.Datos.Servicio.ClienteServicio.Interfaces;
using sanimex.webapi.Datos.Servicio.GerentesSucursalSer;
using sanimex.webapi.Datos.Servicio.LogServicio.implementacion;
using sanimex.webapi.Datos.Servicio.ProductoServicio.Implementacion;
using sanimex.webapi.Datos.Servicio.ReportesServicio;
using sanimex.webapi.Datos.Servicio.SucursalService;
using sanimex.webapi.Datos.Servicio.UbicacionesServicio;
using sanimex.webapi.Datos.Servicio.UsuarioServicio;
using sanimex.webapi.Datos.Servicio.WebServicesSap.Implementacion;
using sanimex.webapi.Datos.Servicio.WebServicesSap.Interfaces;
using sanimex.webapi.Datos.Servicio.WebServicios;
using sanimex.webapi.Negocio.Carrito;
using sanimex.webapi.Negocio.Clientes;
using sanimex.webapi.Negocio.ControlAcceso;
using sanimex.webapi.Negocio.GerentesSucurNegocio;
using sanimex.webapi.Negocio.Logs;
using sanimex.webapi.Negocio.Producto;
using sanimex.webapi.Negocio.reporteWeb;
using sanimex.webapi.Negocio.SapServices;
using sanimex.webapi.Negocio.Sucursales;
using sanimex.webapi.Negocio.Ubicaciones;
using sanimex.webapi.Negocio.Usuarios;
using sanimex.webapi.Negocio.WebServiceSap;
using sanimex.webapi.Negocio.WebsMayoreo;
using sanimex.WebApi.Sap.Services;
using System.Data;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// =================== habilitar reglas cors =======================
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

//========================================= Servicios capa de datos =======================================
// Registrar el servicio de conexion base de datos
builder.Services.AddScoped<IDatos, Datos>();

// Registrar el servicio usuario capa datos
builder.Services.AddScoped<IUsuarioServicio, UsuarioServicio>();
// Registrar el servicio de cliente capa datos
builder.Services.AddScoped<IClienteServicio, ClienteServicio>();

// Registrar el servicio de Producto capa datos
builder.Services.AddScoped<IProductoService, ProductoService>();

// Registrar el servicio de Sucursal capa datos
builder.Services.AddScoped<ISucursalService, SucursalServicie>();
// Registrar el servicio de WebService capa datos
builder.Services.AddScoped<IWebServiceSap, WebServiceSap>();
// Registrar el servicio de Logserivice capa datos
builder.Services.AddScoped<ILogsServicio, LogsServicio>();
// Registrar el servicio de Carroserivice capa datos
builder.Services.AddScoped<ICarritoServicio, CarritoServicio>();

builder.Services.AddScoped<IUbicacionServicio, UbicacionServicio>();

builder.Services.AddScoped<IWebServicio, WebServicio>();

builder.Services.AddScoped<IReporteServicio, ReporteServicio>();

builder.Services.AddScoped<IGerenteSucurServicio, GerenteSucurServicio>();


//========================================= Servicios capa de negocio =======================================
// Registrar el servicio de usuario capa Negocio
builder.Services.AddScoped<IUsuarioNegocio,  UsuarioNegocio>();
// Registrar el servicio de cliente capa Negocio
builder.Services.AddScoped<IClienteNegocio, ClienteNegocio>();

// Registrar el servicio de Producto capa Negocio
builder.Services.AddScoped<IProductoNegocio, ProductoNegocio>();

// Registrar el servicio de Producto capa Negocio
builder.Services.AddScoped<ISucursalNegocio , SucursalNegocio>();

// Registrar el servicio de WebService capa Negocio
builder.Services.AddScoped<IWebServiceNegocio, WebServiceNegocio>();

// Registrar el servicio de SimuladorPedido capa Negocio
builder.Services.AddScoped<ISimuladorPedidoNegocio, SimuladorPedidoNegocio>();

// Registrar el servicio de LogsNegocio capa Negocio
builder.Services.AddScoped<ILogsNegocio, LogsNegocio>();

// Registrar el servicio de CarroNegocio capa Negocio
builder.Services.AddScoped<ICarroNegocio, CarroNegocio>();

builder.Services.AddScoped<IControlAccesoNegocio , ControlAccesoNegocio>();

// Registrar el servicio de cliente capa datos
builder.Services.AddScoped<IClienteService, ClienteService>();

builder.Services.AddScoped<IUbicacionNegocio, UbicacionNegocio>();

builder.Services.AddScoped<IClienteSapNegocio, ClienteSapNegocio>();

builder.Services.AddScoped<IWebMayoreoNegocio, WebMayoreoNegocio>();

builder.Services.AddScoped<IReporteWebNegocio, ReporteWebNegocio>();

builder.Services.AddScoped<IGerenteSucurNegocio, GerenteSucurNegocio>();


//========================================= Servicios capa de presentacion =======================================



// Registrar el servicio de cliente
builder.Services.AddScoped<IDisponibilidadService, DisponibilidadService>();
builder.Services.AddScoped<IDispoMayoreoNegocio, DispoMayoreoNegocio>();
builder.Services.AddScoped<ISimuladorPieza, SimuladorPieza>();
builder.Services.AddScoped<ISimuladorPedidos, SimuladorPedidos>();

//========================================= PRIMERO AGREGAMO JWT =======================================
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

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ==================================== CONFIGURACIÓN DE PUERTO EN PRODUCCION=====================================
// Obtener el puerto desde la variable de entorno o asignar 8000 por defecto
var port = Environment.GetEnvironmentVariable("PORT") ?? "8000";
var url = $"http://*:{port}";

// Configurar la URL del host
builder.WebHost.UseUrls(url);


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//app.UseSwagger();
//app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseCors(misReglasCors);

//========================================= SEGUNDO =======================================
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
