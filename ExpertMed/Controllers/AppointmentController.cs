using ExpertMed.Models;
using ExpertMed.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExpertMed.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly ILogger<AppointmentController> _logger;
        private readonly AppointmentService _appointmentService;

        public AppointmentController(ILogger<AppointmentController> logger, AppointmentService appointmentService)
        {
            _logger = logger;
            _appointmentService = appointmentService;
        }
        public class ErrorViewModel
        {
            public string Message { get; set; }
        }

        [HttpGet]
        public async Task<IActionResult> AppointmentList(int appointmentStatus = 1)
        {
            try
            {
                // Obtener información del usuario desde la sesión
                var userId = HttpContext.Session.GetInt32("UsuarioId");
                var userProfile = HttpContext.Session.GetInt32("PerfilId");

                // Verificar si los valores necesarios están presentes
                if (!userId.HasValue || !userProfile.HasValue)
                {
                    TempData["Error"] = "Por favor, inicie sesión para continuar.";
                    return RedirectToAction("Login", "Account");
                }

                // Establecer valores en ViewBag para usarlos en la vista
                ViewBag.CurrentStatus = appointmentStatus;
                ViewBag.UserProfile = userProfile.Value;
                ViewBag.UserId = userId.Value;

                // Obtener citas a través del servicio
                var appointments = await _appointmentService.GetAllAppointmentAsync(
                    userProfile.Value,
                    appointmentStatus,
                    userId
                );

                // Verificar si no se obtuvieron citas
                if (appointments == null || !appointments.Any())
                {
                    TempData["Info"] = "No se encontraron citas para los parámetros especificados.";
                    return View(new List<Appointment>());
                }

                // Retornar la vista con las citas obtenidas
                return View(appointments);
            }
            catch (Exception ex)
            {
                // Registrar error y retornar una vista vacía con mensaje de error
                _logger.LogError($"Unhandled exception in AppointmentList: {ex.Message}");
                TempData["Error"] = "Ocurrió un error inesperado. Inténtalo de nuevo más tarde.";
                return View(new List<Appointment>());
            }
        }


        [HttpGet("available-hours")]
        public IActionResult GetAvailableHours([FromQuery] int userId, [FromQuery] DateTime date)
        {
            try
            {
                List<string> availableHours = _appointmentService.GetAvailableHours(userId, date);

                if (availableHours.Count == 0)
                {
                    TempData["ErrorMessage"] = "No available hours for the selected date.";  // Almacenar el mensaje en TempData
                    return NoContent();  // Si no hay horas disponibles, devolver un estado 204 No Content
                }

                return Ok(availableHours);  // Si hay horas disponibles, devolverlas con un estado 200 OK
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";  // Almacenar el mensaje de error en TempData
                return StatusCode(500, new { message = ex.Message });  // Manejo de errores en caso de fallos en el servicio
            }
        }



    }
}
