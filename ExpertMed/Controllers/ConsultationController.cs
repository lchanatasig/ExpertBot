using ExpertMed.Models;
using ExpertMed.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExpertMed.Controllers
{
    public class ConsultationController : Controller
    {

        private readonly SelectsService _selectService;
        private readonly PatientService _patientService;
        private readonly ConsultationService _consultationService;
        // Inyección de dependencias
        public ConsultationController(SelectsService selectService, PatientService patientService, ConsultationService consultationService)
        {
            _selectService = selectService;
            _patientService = patientService;
            _consultationService = consultationService;
        }

        public IActionResult ConsultationList()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> NewConsultation(int patientId)
        {
            try
            {
                // Obtener los detalles del paciente
                var patient = await _patientService.GetPatientFullByIdAsync(patientId);

                // Si el paciente no existe, devolver una respuesta de "No encontrado"
                if (patient == null)
                {
                    TempData["ErrorMessage"] = "Patient not found.";
                    return RedirectToAction("Index", "Home");
                }

                // Obtener datos adicionales para la vista
                var genderTypes = await _selectService.GetGenderTypeAsync();
                var bloodTypes = await _selectService.GetBloodTypeAsync();
                var documentTypes = await _selectService.GetDocumentTypeAsync();
                var civilTypes = await _selectService.GetCivilTypeAsync();
                var professionalTrainingTypes = await _selectService.GetProfessionaltrainingTypeAsync();
                var sureHealthTypes = await _selectService.GetSureHealtTypeAsync();
                var countries = await _selectService.GetAllCountriesAsync();
                var provinces = await _selectService.GetAllProvinceAsync();
                var parents = await _selectService.GetRelationshipTypeAsync();
                var allergies = await _selectService.GetAllergiesTypeAsync();
                var surgeries = await _selectService.GetSurgeriesTypeAsync();
                var familyMember = await _selectService.GetFamiliarTypeAsync();
                var diagnosis = await _selectService.GetAllDiagnosisAsync();
                var medications = await _selectService.GetAllMedicationsAsync();
                var images = await _selectService.GetAllImagesAsync();
                var laboratories = await _selectService.GetAllLaboratoriesAsync();


                // Crear el ViewModel
                var viewModel = new NewPatientViewModel
                {
                    DetailsPatient = patient,
                    GenderTypes = genderTypes,
                    BloodTypes = bloodTypes,
                    DocumentTypes = documentTypes,
                    CivilTypes = civilTypes,
                    ProfessionalTrainingTypes = professionalTrainingTypes,
                    SureHealthTypes = sureHealthTypes,
                    Countries = countries,
                    Provinces = provinces,
                    Parents = parents,
                    AllergiesTypes = allergies,
                    SurgeriesTypes = surgeries,
                    FamilyMember = familyMember,
                    Diagnoses = diagnosis,
                    Medications = medications,
                    Images = images,
                    Laboratories = laboratories


                };

                // Retornar la vista con el modelo
                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Unexpected error: " + ex.Message;
                return View();
            }
        }


        [HttpPost]
        public IActionResult CreateConsultation([FromBody] ConsultationDTO consultation)
        {
            try
            {
                // Validación básica
                if (consultation == null)
                {
                    return BadRequest(new { message = "Consulta no válida." });
                }

                // Llamada al servicio para crear la consulta
                _consultationService.CreateConsultation(consultation);

                // Respuesta exitosa
                return Ok(new { message = "Consulta creada exitosamente." });
            }
            catch (Exception ex)
            {
                // Manejo de errores
                return StatusCode(500, new { message = "Ocurrió un error al crear la consulta.", error = ex.Message });
            }
        }

    }
}
