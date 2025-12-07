using ProyectoSW01.Data;
using Rotativa.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Registrar el repositorio de Usuario
builder.Services.AddScoped<UsuarioRepository>();

//Registrar el repositorio de Cliente
builder.Services.AddScoped <ClienteRepository>();

//Registrar el repositorio de Vehiculo
builder.Services.AddScoped<VehiculoRepository>();

//Registrar el repositorio de Servicio
builder.Services.AddScoped<ServicioRepository>();

//Registrar el repositorio de Diagnostico
builder.Services.AddScoped<DiagnosticoRepository>();

//Registrar el repositorio de Mantenimiento
builder.Services.AddScoped<MantenimientoRepository>();

//Registrar el repositorio de Rol
builder.Services.AddScoped<RolRepository>();

//Registrar el repositorio de Repuesto
builder.Services.AddScoped<RepuestoRepository>();

//Registrar el repositorio de ServicioRepuesto
builder.Services.AddScoped<ServicioRepuestoRepository>();

//Registrar el repositorio de DiagnosticoServicio
builder.Services.AddScoped<DiagnosticoServicioRepository>();

//Registrar el repositorio de OrdenTrabajo
builder.Services.AddScoped<OrdenTrabajoRepository>();

//Registrar el repositorio de Carrito
builder.Services.AddScoped<CarritoRepository>();



builder.Services.AddSession();
var app = builder.Build();

// ? Config de Rotativa
RotativaConfiguration.Setup(app.Environment.WebRootPath, "Rotativa");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
