using ExpertMed.Models;
using ExpertMed.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExpertMed.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserService _userService;
        private readonly SelectsService _selectsService;
        private readonly ILogger<UsersController> _logger;


        public UsersController(UserService userService, SelectsService selectsService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _selectsService = selectsService;
            _logger = logger;
        }

        public IActionResult UserList()
        {
            try
            {
                // Llama al servicio para obtener la lista de usuarios
                List<Users> users = _userService.GetAllUsers();

                // Envía los datos al View
                return View(users);
            }
            catch (Exception ex)
            {
                // Manejo de errores (puede loguear el error si es necesario)
                ViewBag.ErrorMessage = "Ocurrió un error al obtener la lista de usuarios.";
                return View("Error");
            }
        }

        // Acción para activar o desactivar un usuario
        [HttpPost]
        public async Task<IActionResult> ChangeUserStatus(int userId, int status)
        {
            var result = await _userService.DesactiveOrActiveUserAsync(userId, status);

            if (result.success)
            {
                TempData["SuccessMessage"] = result.message; // Mensaje de éxito
            }
            else
            {
                TempData["ErrorMessage"] = result.message; // Mensaje de error
            }

            return RedirectToAction("UserList"); // Redirigir a la lista de usuarios
        }

        [HttpGet]
        public async Task<IActionResult> NewUser()
        {
            try
            {
                // Consume ambos servicios
                var profiles = await _selectsService.GetAllProfilesAsync();
                var specialties = await _selectsService.GetAllSpecialtiesAsync();
                var medics = await _selectsService.GetAllMedicsAsync();
                var countries = await _selectsService.GetAllCountriesAsync();
                var percentage = await _selectsService.GetAllVatPercentageAsync();

                // Crea un ViewModel para pasar ambos conjuntos de datos a la vista
                var viewModel = new NewUserViewModel
                {
                    Profiles = profiles,
                    Specialties = specialties,
                    Users = medics,
                    Countries = countries,
                    VatBillings = percentage,
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                // Manejo de errores, por ejemplo, redirigir a una página de error o mostrar un mensaje
                _logger.LogError(ex, "Error al cargar datos para NewUser.");
                return View("Error"); // Asegúrate de tener una vista "Error"
            }
        }


        [HttpPost]
        public async Task<IActionResult> NewUser(UserViewModel usuario, IFormFile? ProfilePhoto, string? selectedDoctorIds)
        {
            // Verificar si el modelo es válido
            if (!ModelState.IsValid)
            {
                foreach (var state in ModelState)
                {
                    var key = state.Key; // Nombre del campo
                    var errors = state.Value.Errors; // Lista de errores

                    foreach (var error in errors)
                    {
                        // Registra los errores en un log, consola o TempData
                        Console.WriteLine($"Campo: {key}, Error: {error.ErrorMessage}");


                    }
                }

                TempData["ErrorMessage"] = "Datos inválidos. Por favor, revisa los campos e intenta de nuevo.";

                // Consume ambos servicios para recargar datos
                var profiles = await _selectsService.GetAllProfilesAsync();
                var specialties = await _selectsService.GetAllSpecialtiesAsync();
                var medics = await _selectsService.GetAllMedicsAsync();
                var countries = await _selectsService.GetAllCountriesAsync();
                var percentage = await _selectsService.GetAllVatPercentageAsync();

                var viewModel = new NewUserViewModel
                {
                    Profiles = profiles,
                    Specialties = specialties,
                    Users = medics,
                    Countries = countries,
                    VatBillings = percentage,
                };

                return View(viewModel);
            }

            // Convierte los archivos a byte[] si fueron proporcionados
            usuario.UserProfilephoto = await ConvertFileToByteArray(ProfilePhoto);

            // Procesa los IDs de médicos seleccionados
            List<int>? associatedDoctorIds = null;
            if (!string.IsNullOrEmpty(selectedDoctorIds))
            {
                // Convierte la cadena separada por comas en una lista de enteros
                associatedDoctorIds = selectedDoctorIds
                    .Split(',')
                    .Select(id => int.Parse(id))
                    .ToList();
            }
           


            try
            {
                // Llamar al servicio para crear el usuario y asociar los médicos
                int idUsuarioCreado = await _userService.CreateUserAsync(usuario, associatedDoctorIds);
                
                // Si el proceso de creación fue exitoso
                TempData["SuccessMessage"] = "User created successfully.";
                return RedirectToAction("UserList", "Users");
            }
            catch (Exception ex)
            {
                // Manejo de excepciones personalizado
                if (ex.Message.Contains("El usuario con este CI ya existe."))
                {
                    TempData["ErrorMessage"] = "El usuario con este CI ya existe.";
                }
                else if (ex.Message.Contains("El nombre de usuario ya existe."))
                {
                    TempData["ErrorMessage"] = "El nombre de usuario ya existe.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Error inesperado: " + ex.Message;
                }

                // Consume ambos servicios para recargar datos
                var profiles = await _selectsService.GetAllProfilesAsync();
                var specialties = await _selectsService.GetAllSpecialtiesAsync();
                var medics = await _selectsService.GetAllMedicsAsync();
                var countries = await _selectsService.GetAllCountriesAsync();
                var percentage = await _selectsService.GetAllVatPercentageAsync();

                var viewModel = new NewUserViewModel
                {
                    Profiles = profiles,
                    Specialties = specialties,
                    Users = medics,
                    Countries = countries,
                    VatBillings = percentage,
                };

                return View(viewModel);
            }
        }


        [HttpGet]
        public async Task<IActionResult> UpdateUser(int id)
        {
            // Obtener los detalles del usuario (incluyendo médicos si es asistente)
            var user = await _userService.GetUserDetailsAsync(id);

            // Si el usuario no existe, devolver una respuesta de "No encontrado"
            if (user == null)
            {
                return NotFound("User Not Found");
            }

            // Obtener las listas de perfiles, especialidades, establecimientos y médicos
            var profiles = await _selectsService.GetAllProfilesAsync();
            var specialties = await _selectsService.GetAllSpecialtiesAsync();
            var countries = await _selectsService.GetAllCountriesAsync();
            var percentage = await _selectsService.GetAllVatPercentageAsync();

            var medics = await _selectsService.GetAllMedicsAsync();

            // Crear un ViewModel para pasar tanto el usuario como las listas a la vista
            var viewModel = new NewUserViewModel
            {
                User = user,  // Pasar el usuario obtenido al ViewModel
                Profiles = profiles,
                Specialties = specialties,
                Countries = countries,
                Users = medics,
                VatBillings = percentage,
                AssociatedDoctors = user.Doctors // Incluir los médicos asociados si es asistente

            };

            // Devolver la vista con el ViewModel poblado
            return View(viewModel);
        }


        [HttpGet]
        public async Task<IActionResult> UpdateUserP(int id)
        {
            // Obtener los detalles del usuario (incluyendo médicos si es asistente)
            var user = await _userService.GetUserDetailsAsync(id);

            // Si el usuario no existe, devolver una respuesta de "No encontrado"
            if (user == null)
            {
                return NotFound("User Not Found");
            }

            // Obtener las listas de perfiles, especialidades, establecimientos y médicos
            var profiles = await _selectsService.GetAllProfilesAsync();
            var specialties = await _selectsService.GetAllSpecialtiesAsync();
            var countries = await _selectsService.GetAllCountriesAsync();
            var percentage = await _selectsService.GetAllVatPercentageAsync();

            var medics = await _selectsService.GetAllMedicsAsync();

            // Crear un ViewModel para pasar tanto el usuario como las listas a la vista
            var viewModel = new NewUserViewModel
            {
                User = user,  // Pasar el usuario obtenido al ViewModel
                Profiles = profiles,
                Specialties = specialties,
                Countries = countries,
                Users = medics,
                VatBillings = percentage,
                AssociatedDoctors = user.Doctors // Incluir los médicos asociados si es asistente

            };

            // Devolver la vista con el ViewModel poblado
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser(UserViewModel usuario,  IFormFile? ProfilePhoto, string? selectedDoctorIds, int id)
        {
            // Verificar si el modelo es válido
            if (!ModelState.IsValid)
            {
                foreach (var state in ModelState)
                {
                    var key = state.Key; // Nombre del campo
                    var errors = state.Value.Errors; // Lista de errores

                    foreach (var error in errors)
                    {
                        // Registra los errores en un log o consola
                        Console.WriteLine($"Campo: {key}, Error: {error.ErrorMessage}");
                    }
                }

                TempData["ErrorMessage"] = "Datos inválidos. Por favor, revisa los campos e intenta de nuevo.";
                return await ReloadUserEditView(id);
            }

            // Convierte los archivos a byte[] si fueron proporcionados
            usuario.UserProfilephoto = await ConvertFileToByteArray(ProfilePhoto);

            // Procesa los IDs de médicos seleccionados
            List<int>? associatedDoctorIds = null;
            if (!string.IsNullOrEmpty(selectedDoctorIds))
            {
                associatedDoctorIds = selectedDoctorIds
                    .Split(',')
                    .Where(id => int.TryParse(id, out _)) // Verifica si cada ID puede ser convertido a un entero
                    .Select(id => int.Parse(id))
                    .ToList();
            }

            // Procesa los días de trabajo seleccionados

            try
            {
                // Llama al servicio para actualizar el usuario
                await _userService.UpdateUserAsync(id, usuario, associatedDoctorIds);

                // Si el proceso de actualización fue exitoso
                TempData["SuccessMessage"] = "Usuario actualizado exitosamente.";
                return RedirectToAction("UserList", "Users");
            }
            catch (Exception ex)
            {
                // Manejo de excepciones personalizado
                if (ex.Message.Contains("El usuario no existe."))
                {
                    TempData["ErrorMessage"] = "El usuario no existe.";
                }
                else if (ex.Message.Contains("El número de documento ya existe."))
                {
                    TempData["ErrorMessage"] = "El número de documento ya existe.";
                }
                else if (ex.Message.Contains("El nombre de usuario ya está registrado."))
                {
                    TempData["ErrorMessage"] = "El nombre de usuario ya está registrado.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Error inesperado: " + ex.Message;
                }

                return await ReloadUserEditView(id);
            }
        }

        private async Task<IActionResult> ReloadUserEditView(int id)
        {
            // Obtener los detalles del usuario
            var user = await _userService.GetUserDetailsAsync(id);

            // Si el usuario no existe, devolver una respuesta de "No encontrado"
            if (user == null)
            {
                return NotFound("Usuario no encontrado.");
            }

            // Obtener datos necesarios para la vista
            var profiles = await _selectsService.GetAllProfilesAsync();
            var specialties = await _selectsService.GetAllSpecialtiesAsync();
            var medics = await _selectsService.GetAllMedicsAsync();
            var countries = await _selectsService.GetAllCountriesAsync();
            var percentage = await _selectsService.GetAllVatPercentageAsync();

            var viewModel = new NewUserViewModel
            {
                User = user,
                Profiles = profiles,
                Specialties = specialties,
                Users = medics,
                Countries = countries,
                VatBillings = percentage,
            };

            return View(viewModel);
        }

        public IActionResult ProfileSimple()
        {
            return View();
        }

        //METODO PARA CONVERTIR ARCHIVOS
        private async Task<byte[]> ConvertFileToByteArray(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    return memoryStream.ToArray();
                }
            }
            return null;
        }

    }
}
