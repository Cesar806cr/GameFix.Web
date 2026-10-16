using Gamefix.Application.Profiles;
using Gamefix.Application.Services.Implementations;
using Gamefix.Application.Services.Interfaces;
using Gamefix.Infraestructure.Data;
using Gamefix.Infraestructure.Repository.Implementations;
using Gamefix.Infraestructure.Repository.Interfaces;
using Gamefix.Web.Middleware;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configurar Repository
builder.Services.AddTransient<IRepositoryProducto, RepositoryProducto>();
builder.Services.AddTransient<IRepositoryFactura, RepositoryFactura>();
builder.Services.AddTransient<IRepositoryReserva, RepositoryReserva>();
builder.Services.AddTransient<IRepositoryServicio, RepositoryServicio>();

// Configurar Services
builder.Services.AddTransient<IServiceProducto, ServiceProducto>();
builder.Services.AddTransient<IServiceFactura, ServiceFactura>();
builder.Services.AddTransient<IServiceReserva, ServiceReserva>();
builder.Services.AddTransient<IServiceServicio, ServiceServicio>();

// Configurar AutoMapper
builder.Services.AddAutoMapper(
    config => { },
    typeof(ProductoProfile),
    typeof(FacturaProfile),
    typeof(ReservaProfile),
    typeof(ServicioProfile)
);

// Configurar conexion a la base de datos SQL
builder.Services.AddDbContext<GameFixContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("SqlServerDataBase")
    );

    if (builder.Environment.IsDevelopment())
    {
        options.EnableSensitiveDataLogging();
    }
});

// Configuraciin de Serilog
var logger = new LoggerConfiguration()
    // Limitar la informaci�n de depuraci�n de Microsoft
    .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
    .Enrich.FromLogContext()

    // Mostrar informaciin en la consola
    .WriteTo.Console(LogEventLevel.Information)

    // Logs informativos
    .WriteTo.Logger(log => log
        .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information)
        .WriteTo.File(
            @"Logs\Info-.log",
            shared: true,
            encoding: Encoding.ASCII,
            rollingInterval: RollingInterval.Day
        ))

    // Logs de depuracion
    .WriteTo.Logger(log => log
        .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Debug)
        .WriteTo.File(
            @"Logs\Debug-.log",
            shared: true,
            encoding: Encoding.ASCII,
            rollingInterval: RollingInterval.Day
        ))

    // Logs de advertencia
    .WriteTo.Logger(log => log
        .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Warning)
        .WriteTo.File(
            @"Logs\Warning-.log",
            shared: true,
            encoding: Encoding.ASCII,
            rollingInterval: RollingInterval.Day
        ))

    // Logs de error
    .WriteTo.Logger(log => log
        .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error)
        .WriteTo.File(
            @"Logs\Error-.log",
            shared: true,
            encoding: Encoding.ASCII,
            rollingInterval: RollingInterval.Day
        ))

    // Logs criticos
    .WriteTo.Logger(log => log
        .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Fatal)
        .WriteTo.File(
            @"Logs\Fatal-.log",
            shared: true,
            encoding: Encoding.ASCII,
            rollingInterval: RollingInterval.Day
        ))
    .CreateLogger();

builder.Host.UseSerilog(logger);

var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

else
{
    // Error control Middleware 
    app.UseMiddleware<ErrorHandlingMiddleware>();
}


// Activar soporte de registro de solicitudes con Serilog
app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Activar Antiforgery para proteger contra ataques CSRF (Cross-Site Request Forgery)
app.UseAntiforgery();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();