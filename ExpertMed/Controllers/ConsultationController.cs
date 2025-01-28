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


        //[HttpPost("Crear-Consulta")]
        //public async Task<IActionResult> CrearConsulta([FromBody] Consulta consultaDto)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        var errores = ModelState
        //            .Where(x => x.Value.Errors.Count > 0)
        //            .Select(x => new { Field = x.Key, Errors = x.Value.Errors.Select(e => e.ErrorMessage) })
        //            .ToList();

        //        foreach (var error in errores)
        //        {
        //            Console.WriteLine($"Campo: {error.Field}, Errores: {string.Join(", ", error.Errors)}");
        //        }

        //        return Json(new { success = false, errores });
        //    }



        //    try
        //    {
        //        // Llama al servicio para crear la consulta
        //        await _consultationService.CrearConsultaAsync(
        //            consultaDto.ConsultationUsercreate,
        //            consultaDto.ConsultationHistoryclinic,
        //            consultaDto.ConsultationPatient,
        //            consultaDto.ConsultationReason,
        //            consultaDto.ConsultationDisease,
        //            consultaDto.ConsultationFamiliaryname,
        //            consultaDto.ConsultationWarningsings,
        //            consultaDto.ConsultationNonpharmacologycal,
        //            consultaDto.ConsultationFamiliarytype,
        //            consultaDto.ConsultationFamiliaryphone,
        //            consultaDto.ConsultationTemperature,
        //            consultaDto.ConsultationRespirationrate,
        //            consultaDto.ConsultationBloodpressuredAs,
        //            consultaDto.ConsultationBloodpresuredDis,
        //            consultaDto.ConsultationPulse,
        //            consultaDto.ConsultationWeight,
        //            consultaDto.ConsultationSize,
        //            consultaDto.ConsultationTreatmentplan,
        //            consultaDto.ConsultationObservation,
        //            consultaDto.ConsultationPersonalbackground,
        //            consultaDto.ConsultationDisablilitydays,
        //            consultaDto.ConsultationUsercreate,
        //            consultaDto.ConsultationSpeciality,
        //            consultaDto.ConsultationType ?? 0,
        //            consultaDto.ConsultationStatus,
        //            // Parámetros de órganos y sistemas
        //            consultaDto.OrgansSystem.sense,
        //            consultaDto.OrganosSistemas.ObserOrgSentidos,
        //            consultaDto.OrganosSistemas.Respiratorio,
        //            consultaDto.OrganosSistemas.ObserRespiratorio,
        //            consultaDto.OrganosSistemas.CardioVascular,
        //            consultaDto.OrganosSistemas.ObserCardioVascular,
        //            consultaDto.OrganosSistemas.Digestivo,
        //            consultaDto.OrganosSistemas.ObserDigestivo,
        //            consultaDto.OrganosSistemas.Genital,
        //            consultaDto.OrganosSistemas.ObserGenital,
        //            consultaDto.OrganosSistemas.Urinario,
        //            consultaDto.OrganosSistemas.ObserUrinario,
        //            consultaDto.OrganosSistemas.MEsqueletico,
        //            consultaDto.OrganosSistemas.ObserMEsqueletico,
        //            consultaDto.OrganosSistemas.Endocrino,
        //            consultaDto.OrganosSistemas.ObserEndocrino,
        //            consultaDto.OrganosSistemas.Linfatico,
        //            consultaDto.OrganosSistemas.ObserLinfatico,
        //            consultaDto.OrganosSistemas.Nervioso,
        //            consultaDto.OrganosSistemas.ObserNervioso,
        //            // Parámetros de examen físico
        //            consultaDto.ExamenFisico.Cabeza,
        //            consultaDto.ExamenFisico.ObserCabeza,
        //            consultaDto.ExamenFisico.Cuello,
        //            consultaDto.ExamenFisico.ObserCuello,
        //            consultaDto.ExamenFisico.Torax,
        //            consultaDto.ExamenFisico.ObserTorax,
        //            consultaDto.ExamenFisico.Abdomen,
        //            consultaDto.ExamenFisico.ObserAbdomen,
        //            consultaDto.ExamenFisico.Pelvis,
        //            consultaDto.ExamenFisico.ObserPelvis,
        //            consultaDto.ExamenFisico.Extremidades,
        //            consultaDto.ExamenFisico.ObserExtremidades,
        //            // Parámetros de antecedentes familiares
        //            consultaDto.AntecedentesFamiliares.Cardiopatia,
        //            consultaDto.AntecedentesFamiliares.ObserCardiopatia,
        //            consultaDto.AntecedentesFamiliares.ParentescocatalogoCardiopatia,
        //            consultaDto.AntecedentesFamiliares.Diabetes,
        //            consultaDto.AntecedentesFamiliares.ObserDiabetes,
        //            consultaDto.AntecedentesFamiliares.ParentescocatalogoDiabetes,
        //            consultaDto.AntecedentesFamiliares.EnfCardiovascular,
        //            consultaDto.AntecedentesFamiliares.ObserEnfCardiovascular,
        //            consultaDto.AntecedentesFamiliares.ParentescocatalogoEnfcardiovascular,
        //            consultaDto.AntecedentesFamiliares.Hipertension,
        //            consultaDto.AntecedentesFamiliares.ObserHipertension,
        //            consultaDto.AntecedentesFamiliares.ParentescocatalogoHipertension,
        //            consultaDto.AntecedentesFamiliares.Cancer,
        //            consultaDto.AntecedentesFamiliares.ObserCancer,
        //            consultaDto.AntecedentesFamiliares.ParentescocatalogoCancer,
        //            consultaDto.AntecedentesFamiliares.Tuberculosis,
        //            consultaDto.AntecedentesFamiliares.ObserTuberculosis,
        //            consultaDto.AntecedentesFamiliares.ParentescocatalogoTuberculosis,
        //            consultaDto.AntecedentesFamiliares.EnfMental,
        //            consultaDto.AntecedentesFamiliares.ObserEnfMental,
        //            consultaDto.AntecedentesFamiliares.ParentescocatalogoEnfmental,
        //            consultaDto.AntecedentesFamiliares.EnfInfecciosa,
        //            consultaDto.AntecedentesFamiliares.ObserEnfInfecciosa,
        //            consultaDto.AntecedentesFamiliares.ParentescocatalogoEnfinfecciosa,
        //            consultaDto.AntecedentesFamiliares.MalFormacion,
        //            consultaDto.AntecedentesFamiliares.ObserMalFormacion,
        //            consultaDto.AntecedentesFamiliares.ParentescocatalogoMalformacion,
        //            consultaDto.AntecedentesFamiliares.Otro,
        //            consultaDto.AntecedentesFamiliares.ObserOtro,
        //            consultaDto.AntecedentesFamiliares.ParentescocatalogoOtro,
        //            // Tablas relacionadas
        //            consultaDto.Alergias,
        //            consultaDto.Cirugias,
        //            consultaDto.Medicamentos,
        //            consultaDto.Laboratorios,
        //            consultaDto.Imagenes,
        //            consultaDto.Diagnosticos
        //        );

        //        // Devuelve un JSON con éxito si es una solicitud AJAX
        //        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        //        {
        //            TempData["SuccessMessage"] = $"La consulta creado exitosamente";

        //            return Json(new { success = true, message = "Consulta creada exitosamente" });

        //        }
        //        else
        //        {
        //            // Redirigir en caso de una solicitud normal (no AJAX)
        //            TempData["SuccessMessage"] = "Consulta Registrado.";
        //            return Json(new { success = true, redirectUrl = Url.Action("ListarConsultas") });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["ErrorMessage"] = $"Error al crear el paciente: {ex.Message}";

        //        _logger.LogError(ex, "Error al crear la consulta");

        //        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        //        {
        //            return Json(new { success = false, message = "Ocurrió un error en el servidor.", details = ex.Message });
        //        }
        //        else
        //        {
        //            return View("Error", new ErrorViewModel { RequestId = ex.Message });
        //        }
        //    }
        //}

    }
}
