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


        [HttpGet]
        public IActionResult GetAvailableHours([FromQuery] int userId, [FromQuery] DateTime date, [FromQuery] int? doctorUserId = null)
        {
            try
            {
                // Si doctorUserId es nulo, lo que indica que no es asistente, llamamos al servicio de la manera normal
                List<string> availableHours = _appointmentService.GetAvailableHours(userId, date, doctorUserId);

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



        [HttpPost]
        public async Task<IActionResult> CreateAppointment([FromBody] Appointment request, [FromQuery] int? doctorUserId = null)
        {
            if (request == null)
            {
                return BadRequest(new { success = false, message = "Request body is null" });
            }

            try
            {
                var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
                var perfilId = HttpContext.Session.GetInt32("PerfilId");

                if (usuarioId == null)
                {
                    return Unauthorized(new { success = false, message = "User not logged in or session expired." });
                }

                // Convertir la hora desde string (por ejemplo "16:00") a TimeOnly
                TimeOnly appointmentHour;
                if (!TimeOnly.TryParse(request.AppointmentHour.ToString(), out appointmentHour))
                {
                    return BadRequest(new { success = false, message = "Invalid appointment hour format" });
                }

                // Crear el objeto de la cita
                var appointment = new Appointment
                {
                    AppointmentCreatedate = DateTime.Now,
                    AppointmentModifydate = DateTime.Now,
                    AppointmentCreateuser = usuarioId.Value,
                    AppointmentModifyuser = usuarioId.Value,
                    AppointmentDate = request.AppointmentDate,
                    AppointmentHour = appointmentHour,  // Asignar la hora convertida
                    AppointmentPatientid = request.AppointmentPatientid,
                    AppointmentStatus = 1
                };

                // Llamar al servicio para crear la cita
                await _appointmentService.CreateAppointmentAsync(appointment, doctorUserId);

                return Ok(new { success = true, message = "Appointment created successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }


        [HttpGet]
        public async Task<IActionResult> AppointmentGetById(int id, int userProfile)
        {
            try
            {
                var appointment = _appointmentService.GetAppointmentById(id, userProfile);

                if (appointment == null)
                {
                    return NotFound(new { message = "Appointment Not Found" });
                }

                // Convertir TimeOnly a string con formato "HH:mm"
                string appointmentTime = appointment.AppointmentHour.ToString("HH:mm");

                // Crear la respuesta con DoctorUserId condicional
                var response = new
                {
                    Patient = appointment.AppointmentPatientid,
                    Date = appointment.AppointmentDate.ToString("yyyy-MM-dd"),
                    Time = appointmentTime,
                    DoctorUserId = (userProfile == 3) ? appointment.DoctorUserId : (int?)null  // Condicional para DoctorUserId
                };

                return Json(response);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error fetching appointment: {ex.Message}");
                return StatusCode(500, new { message = "An error occurred while processing your request." });
            }
        }



        // En el controlador de tu backend
        [HttpPost("ModifyAppointment")]
        public async Task<IActionResult> ModifyAppointment([FromBody] Appointment request)
        {
            try
            {
                var usuarioId = HttpContext.Session.GetInt32("UsuarioId");

                // L�gica para modificar la cita
                var appointment = new Appointment
                {
                    AppointmentId = request.AppointmentId,                  // ID de la cita a modificar
                    AppointmentModifydate = DateTime.Now,                   // Fecha de modificaci�n
                    AppointmentModifyuser = usuarioId ?? 0,                 // Usuario que realiza la modificaci�n
                    AppointmentDate = request.AppointmentDate,              // Nueva fecha de la cita
                    AppointmentHour = request.AppointmentHour,              // Nueva hora de la cita
                    AppointmentPatientid = request.AppointmentPatientid,    // ID del paciente
                    AppointmentStatus = request.AppointmentStatus ?? 1      // Estado de la cita (por defecto 1 si no se especifica)
                };


                await _appointmentService.ModifyAppointmentAsync(appointment);

                return Ok(new { success = true, message = "Appointment modified successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }


        [HttpPost("desactivate")]
        public IActionResult DesactivateAppointment([FromBody] Appointment request)
        {
            // Validar que la petici�n sea correcta
            if (request.AppointmentId <= 0 || request.AppointmentModifyuser <= 0)
            {
                return BadRequest(new { message = "Los par�metros proporcionados no son v�lidos." });
            }

            try
            {
                // Llamar al servicio para desactivar la cita
                _appointmentService.DesactivateAppointment(request.AppointmentId, request.AppointmentModifyuser ?? 0);

                // Retornar una respuesta exitosa en formato JSON
                return Ok(new { message = "Cita desactivada correctamente." });
            }
            catch (Exception ex)
            {
                // En caso de error, devolver mensaje de error en formato JSON
                return StatusCode(500, new { message = $"Error al desactivar la cita: {ex.Message}" });
            }
        }

    }
}
