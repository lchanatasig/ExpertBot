using ExpertMed.Models;
using ExpertMed.Services;
using Microsoft.EntityFrameworkCore;
using Rotativa.Core;

namespace ExpertMed
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configuraci�n de la base de datos
            builder.Services.AddDbContext<DbExpertmedContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("conexion")));

            // Habilitar Razor Pages con recompilaci�n en tiempo de ejecuci�n
            builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

            // Registrar IHttpContextAccessor
            builder.Services.AddHttpContextAccessor();

            // Registrar servicios personalizados
            builder.Services.AddScoped<AuthenticationService>();
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<SelectsService>();
            builder.Services.AddScoped<PatientService>();
            builder.Services.AddScoped<AppointmentService>();
            builder.Services.AddScoped<ConsultationService>();

            // Configuraci�n de controladores y vistas
            builder.Services.AddControllersWithViews();

            // Configuraci�n de la sesi�n
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Tiempo de expiraci�n de la sesi�n
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonTimeOnlyConverter());
            });

            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            var app = builder.Build();

            // Habilitar el uso de sesiones
            app.UseSession();

            // Configuraci�n del pipeline HTTP
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            // Configuraci�n de endpoints
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Authentication}/{action=SignIn}/{id?}");
            });

            // Configuraci�n de Rotativa
            Rotativa.AspNetCore.RotativaConfiguration.Setup(app.Environment.WebRootPath, "../Rotativa");

            app.Run();
        }
    }
}