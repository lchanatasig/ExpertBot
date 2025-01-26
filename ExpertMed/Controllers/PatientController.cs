﻿using ExpertMed.Models;
using ExpertMed.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Numerics;
using System.Text.Json;

namespace ExpertMed.Controllers
{
    public class PatientController : Controller
    {
        private readonly UserService _usersService;
        private readonly ILogger<PatientController> _logger;
        private readonly SelectsService _selectService;
        private readonly PatientService _patientService;

        //Inyección de dependencias
        public PatientController(UserService usersService, ILogger<PatientController> logger, SelectsService selectService, PatientService patientService)
        {
            _usersService = usersService;
            _logger = logger;
            _selectService = selectService;
            _patientService = patientService;
        }

        public async Task<IActionResult> PatientList()
        {
            try
            {
                // Obtén el ID del usuario y el perfil desde la sesión
                var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
                var perfilId = HttpContext.Session.GetInt32("PerfilId");

                // Verifica que los valores existan (manejo básico de errores)
                if (perfilId == null || usuarioId == null)
                {
                    _logger.LogError("El perfil del usuario o el ID del usuario no están disponibles en la sesión.");
                    return View("Error"); // O redirige a una página de error o login
                }

                // Llama al servicio con los valores obtenidos de la sesión
                var patients = await _patientService.GetAllPatientsAsync(perfilId.Value, usuarioId.Value);

                // Pasa los pacientes a la vista
                return View(patients);
            }
            catch (Exception ex)
            {
                // Manejo de errores
                _logger.LogError(ex, "Error al obtener la lista de pacientes.");
                return View("Error"); // O una vista de error personalizada
            }
        }
        // Acción para activar o desactivar un paciente

        [HttpPost]
        public async Task<IActionResult> ChangePatientStatus(int patientId, int status)
        {
            var result = await _patientService.DesactiveOrActivePatientAsync(patientId, status);

            if (result.success)
            {
                TempData["SuccessMessage"] = result.message; // Mensaje de éxito
            }
            else
            {
                TempData["ErrorMessage"] = result.message; // Mensaje de error
            }

            return RedirectToAction("PatientList"); // Redirigir a la lista de usuarios
        }

        [HttpGet]
        public async Task<IActionResult> NewPatient()
        {
            try
            {
                
                // Obtén el ID del usuario y el perfil desde la sesión
                var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
                var perfilId = HttpContext.Session.GetInt32("PerfilId");
                // Obtener los tipos de género
                var genderTypes = await _selectService.GetGenderTypeAsync();

                // Obtener los tipos de sangre
                var bloodTypes = await _selectService.GetBloodTypeAsync();

                // Obtener los tipos de documentos
                var documentTypes = await _selectService.GetDocumentTypeAsync();

                // Obtener los tipos de estado civil
                var civilTypes = await _selectService.GetCivilTypeAsync();

                // Obtener los tipos de formación profesional
                var professionalTrainingTypes = await _selectService.GetProfessionaltrainingTypeAsync();

                // Obtener los tipos de seguros de salud
                var sureHealthTypes = await _selectService.GetSureHealtTypeAsync();

                // Obtener las nacionalidades
                var countries = await _selectService.GetAllCountriesAsync();

                // Obtener las provincias
                var provinces = await _selectService.GetAllProvinceAsync();

                var doctors = await _patientService.GetDoctorsByAssistantAsync(usuarioId ?? 0);

           

                // Crear un objeto de vista modelo para pasar los datos a la vista
                var viewModel = new NewPatientViewModel
                {
                    GenderTypes = genderTypes,
                    BloodTypes = bloodTypes,
                    DocumentTypes = documentTypes,
                    CivilTypes = civilTypes,
                    ProfessionalTrainingTypes = professionalTrainingTypes,
                    SureHealthTypes = sureHealthTypes,
                    Countries = countries,
                    Provinces = provinces,
                    Users = doctors // Agregamos los médicos al ViewModel
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                // Manejo de excepciones personalizado
                TempData["ErrorMessage"] = "Error inesperado: " + ex.Message;

                // Obtén el ID del usuario y el perfil desde la sesión
                var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
                var perfilId = HttpContext.Session.GetInt32("PerfilId");
                // Obtener los tipos de género
                var genderTypes = await _selectService.GetGenderTypeAsync();

                // Obtener los tipos de sangre
                var bloodTypes = await _selectService.GetBloodTypeAsync();

                // Obtener los tipos de documentos
                var documentTypes = await _selectService.GetDocumentTypeAsync();

                // Obtener los tipos de estado civil
                var civilTypes = await _selectService.GetCivilTypeAsync();

                // Obtener los tipos de formación profesional
                var professionalTrainingTypes = await _selectService.GetProfessionaltrainingTypeAsync();

                // Obtener los tipos de seguros de salud
                var sureHealthTypes = await _selectService.GetSureHealtTypeAsync();

                // Obtener las nacionalidades
                var countries = await _selectService.GetAllCountriesAsync();

                // Obtener las provincias
                var provinces = await _selectService.GetAllProvinceAsync();

                var doctors = await _patientService.GetDoctorsByAssistantAsync(usuarioId ?? 0);



                // Crear un objeto de vista modelo para pasar los datos a la vista
                var viewModel = new NewPatientViewModel
                {
                    GenderTypes = genderTypes,
                    BloodTypes = bloodTypes,
                    DocumentTypes = documentTypes,
                    CivilTypes = civilTypes,
                    ProfessionalTrainingTypes = professionalTrainingTypes,
                    SureHealthTypes = sureHealthTypes,
                    Countries = countries,
                    Provinces = provinces,
                    Users = doctors // Agregamos los médicos al ViewModel
                };

                return View(viewModel);
            }
        }


        // Controlador para la creación de un nuevo paciente
        [HttpPost]
        public async Task<IActionResult> NewPatient(Patient patient, int? doctorUserId = null)
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

                return BadRequest(new
                {
                    success = 0,
                    message = "Datos inválidos. Por favor, revisa los campos e intenta de nuevo."
                });
            }

            try
            {
                // Llamar al servicio para crear el paciente
                int createdPatientId = await _patientService.CreatePatientAsync(patient, doctorUserId);

                // Si el proceso de creación fue exitoso
                TempData["SuccessMessage"] = "Paciente creado exitosamente.";
                return RedirectToAction("PatientList", "Patient");
            }
            catch (Exception ex)
            {
                // Manejo de excepciones personalizado
                if (ex.Message.Contains("El número de documento ya existe."))
                {
                    TempData["ErrorMessage"] = "El número de documento ya existe.";
                }
                else if (ex.Message.Contains("El correo electrónico ya está registrado."))
                {
                    TempData["ErrorMessage"] = "El correo electrónico ya está registrado.";
                }
                else if (ex.Message.Contains("El asistente no está autorizado para este médico."))
                {
                    TempData["ErrorMessage"] = "El asistente no está autorizado para este médico.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Error inesperado: " + ex.Message;
                }


                // Obtén el ID del usuario y el perfil desde la sesión
                var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
                var perfilId = HttpContext.Session.GetInt32("PerfilId");
                // Obtener los tipos de género
                var genderTypes = await _selectService.GetGenderTypeAsync();

                // Obtener los tipos de sangre
                var bloodTypes = await _selectService.GetBloodTypeAsync();

                // Obtener los tipos de documentos
                var documentTypes = await _selectService.GetDocumentTypeAsync();

                // Obtener los tipos de estado civil
                var civilTypes = await _selectService.GetCivilTypeAsync();

                // Obtener los tipos de formación profesional
                var professionalTrainingTypes = await _selectService.GetProfessionaltrainingTypeAsync();

                // Obtener los tipos de seguros de salud
                var sureHealthTypes = await _selectService.GetSureHealtTypeAsync();

                // Obtener las nacionalidades
                var countries = await _selectService.GetAllCountriesAsync();

                // Obtener las provincias
                var provinces = await _selectService.GetAllProvinceAsync();

                var doctors = await _patientService.GetDoctorsByAssistantAsync(usuarioId ?? 0);



                // Crear un objeto de vista modelo para pasar los datos a la vista
                var viewModel = new NewPatientViewModel
                {
                    GenderTypes = genderTypes,
                    BloodTypes = bloodTypes,
                    DocumentTypes = documentTypes,
                    CivilTypes = civilTypes,
                    ProfessionalTrainingTypes = professionalTrainingTypes,
                    SureHealthTypes = sureHealthTypes,
                    Countries = countries,
                    Provinces = provinces,
                    Users = doctors // Agregamos los médicos al ViewModel
                };

                return View(viewModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> UpdatePatient(int id)
        {
            // Obtener los detalles del usuario (incluyendo médicos si es asistente)
            var patient = await _patientService.GetPatientDetailsAsync(id);
            // Si el usuario no existe, devolver una respuesta de "No encontrado"
            if (patient == null)
            {
                return NotFound("Patient Not Found");
            }
            try
            {
                // Obtener los tipos de género
                var genderTypes = await _selectService.GetGenderTypeAsync(); // Asumiendo que tienes un servicio _catalogService

                // Obtener los tipos de sangre
                var bloodTypes = await _selectService.GetBloodTypeAsync(); // Asumiendo que tienes un servicio _catalogService

                // Obtener los tipos de documentos
                var documentTypes = await _selectService.GetDocumentTypeAsync(); // Asumiendo que tienes un servicio _catalogService

                // Obtener los tipos de estado civil
                var civilTypes = await _selectService.GetCivilTypeAsync(); // Asumiendo que tienes un servicio _catalogService

                // Obtener los tipos de formación profesional
                var professionalTrainingTypes = await _selectService.GetProfessionaltrainingTypeAsync(); // Asumiendo que tienes un servicio _catalogService

                // Obtener los tipos de seguros de salud
                var sureHealthTypes = await _selectService.GetSureHealtTypeAsync(); // Asumiendo que tienes un servicio _catalogService
                // Obtener las nacionalidades
                var countries = await _selectService.GetAllCountriesAsync();

                var provinces = await _selectService.GetAllProvinceAsync();


                // Crear un objeto de vista modelo para pasar los datos a la vista
                var viewModel = new NewPatientViewModel
                {
                    Patient = patient,
                    GenderTypes = genderTypes,
                    BloodTypes = bloodTypes,
                    DocumentTypes = documentTypes,
                    CivilTypes = civilTypes,
                    ProfessionalTrainingTypes = professionalTrainingTypes,
                    SureHealthTypes = sureHealthTypes,
                    Countries = countries,
                    Provinces = provinces

                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                // Manejo de excepciones personalizado
                TempData["ErrorMessage"] = "Error inesperado: " + ex.Message;
                return View();
            }


        }


        //Metodo post
        [HttpPost]
        public async Task<IActionResult> UpdatePatient(Patient patient)
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

                return BadRequest(new
                {
                    success = 0,
                    message = "Datos inválidos. Por favor, revisa los campos e intenta de nuevo."
                });
            }

            try
            {
                int patientId;

                // Si el paciente tiene un ID, es una actualización
                if (patient.PatientId > 0)
                {
                    patientId = await _patientService.UpdatePatientAsync(patient);
                    TempData["SuccessMessage"] = "Patient updated successfully.";
                }
                else
                {
                    // Llamar al servicio para crear el paciente
                    patientId = await _patientService.CreatePatientAsync(patient);
                    TempData["SuccessMessage"] = "Patient created successfully.";
                }

                // Redirigir a la lista de pacientes
                return RedirectToAction("PatientList", "Patient");
            }
            catch (Exception ex)
            {
                // Manejo de excepciones personalizado
                if (ex.Message.Contains("El paciente con este CI ya existe."))
                {
                    TempData["ErrorMessage"] = "El paciente con este CI ya existe.";
                }
                else if (ex.Message.Contains("El nombre de paciente ya existe."))
                {
                    TempData["ErrorMessage"] = "El paciente de usuario ya existe.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Error inesperado: " + ex.Message;
                }

                // Consume ambos servicios para recargar datos
                // Obtener los tipos de género
                var genderTypes = await _selectService.GetGenderTypeAsync(); // Asumiendo que tienes un servicio _catalogService

                // Obtener los tipos de sangre
                var bloodTypes = await _selectService.GetBloodTypeAsync(); // Asumiendo que tienes un servicio _catalogService

                // Obtener los tipos de documentos
                var documentTypes = await _selectService.GetDocumentTypeAsync(); // Asumiendo que tienes un servicio _catalogService

                // Obtener los tipos de estado civil
                var civilTypes = await _selectService.GetCivilTypeAsync(); // Asumiendo que tienes un servicio _catalogService

                // Obtener los tipos de formación profesional
                var professionalTrainingTypes = await _selectService.GetProfessionaltrainingTypeAsync(); // Asumiendo que tienes un servicio _catalogService

                // Obtener los tipos de seguros de salud
                var sureHealthTypes = await _selectService.GetSureHealtTypeAsync(); // Asumiendo que tienes un servicio _catalogService
                                                                                    // Obtener las nacionalidades
                var countries = await _selectService.GetAllCountriesAsync();

                var provinces = await _selectService.GetAllProvinceAsync();

                // Crear un objeto de vista modelo para pasar los datos a la vista
                var viewModel = new NewPatientViewModel
                {
                    GenderTypes = genderTypes,
                    BloodTypes = bloodTypes,
                    DocumentTypes = documentTypes,
                    CivilTypes = civilTypes,
                    ProfessionalTrainingTypes = professionalTrainingTypes,
                    SureHealthTypes = sureHealthTypes,
                    Countries = countries,
                    Provinces = provinces
                };

                return View(viewModel);
            }
        }


    }
}
