using ExpertMed.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ExpertMed.Services
{
    public class AppointmentService
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<AppointmentService> _logger;
        private readonly DbExpertmedContext _dbContext;

        public AppointmentService(IHttpContextAccessor httpContextAccessor, ILogger<AppointmentService> logger, DbExpertmedContext dbContext)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public class AvailableHour
        {
            public TimeSpan AvailableTime { get; set; }
        }


        public async Task<List<Appointment>> GetAllAppointmentAsync(int userProfile, int appointmentStatus, int? userId = null)
        {
            try
            {
                // Definir los parámetros para el procedimiento almacenado
                var parameters = new[]
                {
            new SqlParameter("@UserProfile", userProfile),
            new SqlParameter("@UserID", userId ?? (object)DBNull.Value),
            new SqlParameter("@AppointmentStatus", appointmentStatus)
        };

                // Ejecutar el procedimiento almacenado y convertir el resultado en lista
                var appointments = await _dbContext.Appointments
                    .FromSqlRaw("EXEC sp_ListAllAppointment @UserProfile, @UserID,@AppointmentStatus", parameters)
                    .ToListAsync(); // Usar ToListAsync() en lugar de .AsEnumerable().ToList()

                // Cargar relaciones necesarias explícitamente
                foreach (var appointment in appointments)
                {
                    // Esto es útil si necesitas cargar relaciones adicionales como PatientNationalityNavigation o PatientCreationuserNavigation
                    // Usamos 'LoadAsync' solo cuando sea necesario, pero si tienes muchas relaciones o muchos pacientes, puede afectar el rendimiento
                    await _dbContext.Entry(appointment)
                        .Reference(p => p.AppointmentCreateuserNavigation)
                        .LoadAsync();
                }

                return appointments;
            }
            catch (SqlException sqlEx)
            {
                // Log de error en caso de falla SQL
                _logger.LogError(sqlEx, "Error al ejecutar el procedimiento almacenado en la base de datos.");
                throw; // Rethrow para que el error sea manejado o visto en un nivel superior
            }
            catch (Exception ex)
            {
                // Log de error general
                _logger.LogError(ex, "Error al obtener los pacientes.");
                throw; // Rethrow para que el error sea manejado o visto en un nivel superior
            }
        }


        //Obtener horas disponibles por medico
        public List<string> GetAvailableHours(int userId, DateTime date)
        {
            List<string> availableHours = new List<string>();

            using (SqlConnection conn = new SqlConnection(_dbContext.Database.GetConnectionString()))
            {
                try
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand("sp_GetAvailableHours", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int)).Value = userId;
                        cmd.Parameters.Add(new SqlParameter("@Date", SqlDbType.Date)).Value = date;

                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            availableHours.Add(reader["AvailableTime"].ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error fetching available hours", ex);
                }
            }

            return availableHours;
        }

    }
}
