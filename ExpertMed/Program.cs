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

            // Configuración de la base de datos
            builder.Services.AddDbContext<DbExpertmedContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("conexion")));

            // Habilitar Razor Pages con recompilación en tiempo de ejecución
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

            // Configuración de controladores y vistas
            builder.Services.AddControllersWithViews();

            // Configuración de la sesión
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Tiempo de expiración de la sesión
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

            // Configuración del pipeline HTTP
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            // Configuración de endpoints
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Authentication}/{action=SignIn}/{id?}");
            });

            // Configuración de Rotativa
            Rotativa.AspNetCore.RotativaConfiguration.Setup(app.Environment.WebRootPath, "../Rotativa");

            app.Run();
        }
    }
}