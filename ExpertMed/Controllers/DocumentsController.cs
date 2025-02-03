using ExpertMed.Models;
using ExpertMed.Services;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using Rotativa.AspNetCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;


namespace ExpertMed.Controllers
{
    public class DocumentsController : Controller
    {
        private readonly UserService _usersService;
        private readonly ILogger<DocumentsController> _logger;
        private readonly SelectsService _selectService;
        private readonly PatientService _patientService;
        private readonly ConsultationService _consultationService;

        //Inyección de dependencias
        public DocumentsController(UserService usersService, ILogger<DocumentsController> logger, SelectsService selectService, PatientService patientService, ConsultationService consultationService)
        {
            _usersService = usersService;
            _logger = logger;
            _selectService = selectService;
            _patientService = patientService;
            _consultationService = consultationService;
        }


        [HttpGet]
        public async Task<IActionResult> MedicalCertificate(int consultationId)
        {
            // Obtener los detalles de la consulta
            var consultation = _consultationService.GetConsultationDetails(consultationId);

            // Verificar si la consulta existe
            if (consultation == null)
            {
                TempData["ErrorMessage"] = "Consulta no encontrada.";
                return RedirectToAction("Index", "Home");
            }

            // Obtener el patientId de la consulta
            var patientId = consultation.ConsultationPatient;

            // Obtener los detalles del paciente
            var patient = await _patientService.GetPatientFullByIdAsync(patientId);
            if (patient == null)
            {
                TempData["ErrorMessage"] = "Paciente no encontrado.";
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
                Laboratories = laboratories,
                Consultation = consultation // Agregar los detalles de la consulta al ViewModel
            };

            // Retornar la vista en PDF usando Rotativa
            return new ViewAsPdf("MedicalCertificate", viewModel)
            {
                PageSize = (Rotativa.AspNetCore.Options.Size?)Rotativa.Core.Options.Size.A4,
                PageOrientation = (Rotativa.AspNetCore.Options.Orientation?)Rotativa.Core.Options.Orientation.Portrait
            };
        }

        public async Task <IActionResult> MedicalForm(int consultationId)
        {
            // Obtener los detalles de la consulta
            var consultation = _consultationService.GetConsultationDetails(consultationId);

            // Verificar si la consulta existe
            if (consultation == null)
            {
                TempData["ErrorMessage"] = "Consulta no encontrada.";
                return RedirectToAction("Index", "Home");
            }

            // Obtener el patientId de la consulta
            var patientId = consultation.ConsultationPatient;

            // Obtener los detalles del paciente
            var patient = await _patientService.GetPatientFullByIdAsync(patientId);
            if (patient == null)
            {
                TempData["ErrorMessage"] = "Paciente no encontrado.";
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
            var consulta = new NewPatientViewModel
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
                Laboratories = laboratories,
                Consultation = consultation // Agregar los detalles de la consulta al ViewModel
            };
            return new ViewAsPdf("MedicalForm",consulta)
            {
                PageSize = (Rotativa.AspNetCore.Options.Size?)Rotativa.Core.Options.Size.A4,
            };
        }

        public async Task<IActionResult> MedicationRecipe(int consultationId)
        {
            // Obtener los detalles de la consulta
            var consultation = _consultationService.GetConsultationDetails(consultationId);

            // Verificar si la consulta existe
            if (consultation == null)
            {
                TempData["ErrorMessage"] = "Consulta no encontrada.";
                return RedirectToAction("Index", "Home");
            }

            // Obtener el patientId de la consulta
            var patientId = consultation.ConsultationPatient;

            // Obtener los detalles del paciente
            var patient = await _patientService.GetPatientFullByIdAsync(patientId);
            if (patient == null)
            {
                TempData["ErrorMessage"] = "Paciente no encontrado.";
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
            var consulta = new NewPatientViewModel
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
                Laboratories = laboratories,
                Consultation = consultation // Agregar los detalles de la consulta al ViewModel
            };
            // Tamaño de página A5 con márgenes grandes
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);

                    page.Size(1194, 847); // Tamaño A3 horizontal

                    page.DefaultTextStyle(x => x.FontFamily("Arial")); // Cambiar la familia de fuentes a Arial

                    page.Header().Row(row =>
                    {
                        row.ConstantItem(550).Column(col =>
                        {
                            col.Item().Text("Dr:" + consulta.Consultation.UsersNames + consulta.Consultation.UsersSurcenames).Bold().FontSize(14); // Encabezado más grande
                            col.Item().Text("Especialista en: " + consulta.Consultation.SpecialityName).FontSize(12);
                            col.Item().Text("e-mail: " + consulta.Consultation.UsersEmail).FontSize(11);
                            col.Item().Text("Teléfonos: " + consulta.Consultation.UsersPhone).FontSize(11);
                            col.Item().PaddingTop(2).Text("").FontSize(11);
                            col.Item().PaddingVertical(5).LineHorizontal(1).LineColor(Colors.Grey.Lighten1);
                        });

                        row.AutoItem().PaddingHorizontal(10).LineVertical(1).LineColor(Colors.Grey.Lighten1);

                        row.RelativeColumn().Column(col =>
                        {
                            col.Item().Text("Dr:" + consulta.Consultation.UsersNames + consulta.Consultation.UsersSurcenames).Bold().FontSize(14); // Encabezado más grande
                            col.Item().Text("Especialista en: " + consulta.Consultation.SpecialityName).FontSize(12);
                            col.Item().Text("e-mail: " + consulta.Consultation.UsersEmail).FontSize(11);
                            col.Item().Text("Teléfonos: " + consulta.Consultation.UsersPhone).FontSize(11);
                            col.Item().PaddingTop(2).Text("").FontSize(11);
                            col.Item().PaddingVertical(5).LineHorizontal(1).LineColor(Colors.Grey.Lighten1);
                        });
                    });


                    page.Content().Column(content =>
                    {
                        // Primera fila
                        content.Item().Row(row =>
                        {
                            row.ConstantItem(400).Column(col =>
                            {

                                col.Item().PaddingTop(10)
                                    .Text(text =>
                                    {
                                        text.Span("Fecha: ").FontSize(11).Bold(); // Solo "Fecha:" en negrita
                                        text.Span(consulta.Consultation.ConsultationCreationdate.ToString()).FontSize(11); // Valor de la fecha sin negrita
                                    });


                                col.Item().PaddingTop(2)
                                     .Text(text =>
                                     {
                                         text.Span("Apellidos: ").FontSize(11).Bold(); // Solo "Fecha:" en negrita
                                         text.Span(consulta.DetailsPatient.PatientFirstsurname + " " + consulta.DetailsPatient.PatientSecondlastname).FontSize(11); // Valor de la fecha sin negrita
                                     });
                                col.Item().PaddingTop(2)
                                    .Text(text =>
                                    {
                                        text.Span("Nombres: ").FontSize(11).Bold(); // Solo "Fecha:" en negrita
                                        text.Span(consulta.DetailsPatient.PatientFirstname + " " + consulta.DetailsPatient.PatientMiddlename).FontSize(11); // Valor de la fecha sin negrita
                                    });
                                col.Item().PaddingTop(2)
                                    .Text(text =>
                                    {
                                        text.Span("Edad: ").FontSize(11).Bold(); // Solo "Fecha:" en negrita
                                        text.Span(consulta.DetailsPatient.PatientAge.ToString()).FontSize(11); // Valor de la fecha sin negrita
                                    });
                                col.Item().PaddingTop(2)
       .Text(text =>
       {
           text.Span("Alergias: ").FontSize(11).Bold();

          
               var allergyNames = consulta.Consultation.AllergiesConsultations?
               .Select(alle => consulta.AllergiesTypes
               .FirstOrDefault(d => d.CatalogId == alle.AllergiesCatalogid)?.CatalogName ?? "N/A")
               .ToList() ?? new List<string>();
           

           text.Span(string.Join(", ", allergyNames)).FontSize(11); // Mostrar las alergias en oración
       });


                                // Información de alergias

                                // Información de diagnósticos
                                col.Item().Text(text =>
                                {
                                    text.Span("Diagnóstico: ").FontSize(11).Bold();
                                    var ultimoDiagnosticoDefinitivo = consulta.Consultation.DiagnosisConsultations?
                                                           .Where(d => d.DiagnosisDefinitive == true)
                                                           .OrderByDescending(d => d.DiagnosisDiagnosisid)
                                                           .FirstOrDefault();

                                    var nombreDiagnostico = ultimoDiagnosticoDefinitivo != null
                                    ? consulta.Diagnoses.FirstOrDefault(d => d.DiagnosisId == ultimoDiagnosticoDefinitivo.DiagnosisDiagnosisid)?.DiagnosisName ?? "N/A"
                                    : "N/A";
                                    text.Span(nombreDiagnostico).FontSize(11); // Mostrar la oración con los diagnósticos
                                });


                            });

                            row.ConstantItem(150).Column(col =>
                            {
                                // Muestra todos los secuenciales de los medicamentos
                                if (consulta.Medications.Any())
                                {
                                    col.Item().PaddingTop(2)
                                   .Text(text =>
                                   {
                                       text.Span("Receta: ").FontSize(11).Bold(); // Solo "Fecha:" en negrita
                                       text.Span(consulta.Consultation.MedicationsConsultations.First().MedicationsSequential.ToString()).FontSize(11); // Valor de la fecha sin negrita
                                   });
                                }
                                else
                                {
                                    col.Item().PaddingTop(10).Text("Secuencial: No hay medicamentos").Bold().FontSize(14);
                                }

                                col.Item().PaddingTop(2)
                               .Text(text =>
                               {
                                   text.Span("CC: ").FontSize(11).Bold(); // Solo "Fecha:" en negrita
                                   text.Span(consulta.DetailsPatient.PatientDocumentnumber.ToString()).FontSize(11); // Valor de la fecha sin negrita
                               });
                            });

                            row.AutoItem().PaddingHorizontal(10).LineVertical(1).LineColor(Colors.Grey.Lighten1);

                            row.ConstantItem(400).Column(col =>
                            {

                                col.Item().PaddingTop(10)
                                    .Text(text =>
                                    {
                                        text.Span("Fecha: ").FontSize(11).Bold(); // Solo "Fecha:" en negrita
                                        text.Span(consulta.Consultation.ConsultationCreationdate.ToString()).FontSize(11); // Valor de la fecha sin negrita
                                    });


                                col.Item().PaddingTop(2)
                                     .Text(text =>
                                     {
                                         text.Span("Apellidos: ").FontSize(11).Bold(); // Solo "Fecha:" en negrita
                                         text.Span(consulta.DetailsPatient.PatientFirstsurname + " " + consulta.DetailsPatient.PatientSecondlastname).FontSize(11); // Valor de la fecha sin negrita
                                     });
                                col.Item().PaddingTop(2)
                                    .Text(text =>
                                    {
                                        text.Span("Nombres: ").FontSize(11).Bold(); // Solo "Fecha:" en negrita
                                        text.Span(consulta.DetailsPatient.PatientFirstname + " " + consulta.DetailsPatient.PatientMiddlename).FontSize(11); // Valor de la fecha sin negrita
                                    });
                                col.Item().PaddingTop(2)
                                    .Text(text =>
                                    {
                                        text.Span("Edad: ").FontSize(11).Bold(); // Solo "Fecha:" en negrita
                                        text.Span(consulta.DetailsPatient.PatientAge.ToString()).FontSize(11); // Valor de la fecha sin negrita
                                    });
                                col.Item().PaddingTop(2)
       .Text(text =>
       {
           text.Span("Alergias: ").FontSize(11).Bold();



           var allergyNames = consulta.Consultation.AllergiesConsultations?
           .Select(alle => consulta.AllergiesTypes
           .FirstOrDefault(d => d.CatalogId == alle.AllergiesCatalogid)?.CatalogName ?? "N/A")
           .ToList() ?? new List<string>();


           text.Span(string.Join(", ", allergyNames)).FontSize(11); // Mostrar las alergias en oración
       });


                                // Información de alergias

                                // Información de diagnósticos
                                col.Item().Text(text =>
                                {
                                    text.Span("Diagnóstico: ").FontSize(11).Bold();

                                     var ultimoDiagnosticoDefinitivo = consulta.Consultation.DiagnosisConsultations?
                                                           .Where(d => d.DiagnosisDefinitive == true)
                                                           .OrderByDescending(d => d.DiagnosisDiagnosisid)
                                                           .FirstOrDefault();

                                    var nombreDiagnostico = ultimoDiagnosticoDefinitivo != null
                                    ? consulta.Diagnoses.FirstOrDefault(d => d.DiagnosisId == ultimoDiagnosticoDefinitivo.DiagnosisDiagnosisid)?.DiagnosisName ?? "N/A"
                                    : "N/A";
                                    text.Span(nombreDiagnostico).FontSize(11); // Mostrar la oración con los diagnósticos

                                });


                            });

                            row.ConstantItem(150).Column(col =>
                            {
                                // Muestra todos los secuenciales de los medicamentos
                                if (consulta.Medications.Any())
                                {
                                    col.Item().PaddingTop(2)
                                   .Text(text =>
                                   {
                                       text.Span("Receta: ").FontSize(11).Bold(); // Solo "Fecha:" en negrita
                                       text.Span(consulta.Consultation.MedicationsConsultations.First().MedicationsSequential.ToString()).FontSize(11); // Valor de la fecha sin negrita
                                   });
                                }
                                else
                                {
                                    col.Item().PaddingTop(10).Text("Secuencial: No hay medicamentos").Bold().FontSize(14);
                                }
                                col.Item().PaddingTop(2)
                                .Text(text =>
                                {
                                    text.Span("CC: ").FontSize(11).Bold(); // Solo "Fecha:" en negrita
                                    text.Span(consulta.DetailsPatient.PatientDocumentnumber.ToString()).FontSize(11); // Valor de la fecha sin negrita
                                });
                            });


                        });

                        // Segunda fila (Nueva fila)
                        content.Item().Row(row =>
                        {
                            row.ConstantItem(550).PaddingTop(50).Column(col =>
                            {
                                col.Item().PaddingBottom(6).Text("PRESCRIPCIÓN").Bold().FontSize(14);

                                // Filtrar los medicamentos de la consulta actual
                                var medicationsList = consulta.Consultation.MedicationsConsultations?
                                    .Select(med =>
                                    {
                                        var medication = consulta.Medications.FirstOrDefault(d => d.MedicationsId == med.MedicationsMedicationsid);
                                        return medication != null
                                            ? $"({medication.MedicationsCie10}) {medication.MedicationsName} - X: {med.MedicationsAmount}"
                                            : "N/A";
                                    })
                                    .ToList() ?? new List<string>();

                                foreach (var medicamento in medicationsList)
                                {
                                    col.Item().Text(medicamento).FontSize(12);
                                }
                            });

                            row.AutoItem().PaddingHorizontal(10).LineVertical(1).LineColor(Colors.Grey.Lighten1);

                            row.ConstantItem(550).PaddingTop(50).Column(col =>
                            {
                                col.Item().PaddingBottom(7).Text("INDICACIONES").Bold().FontSize(14);

                                // Filtrar las indicaciones de la consulta actual
                                var indicationsList = consulta.Consultation.MedicationsConsultations?
                                    .Select(med =>
                                    {
                                        var medication = consulta.Medications.FirstOrDefault(d => d.MedicationsId == med.MedicationsMedicationsid);
                                        return medication != null
                                            ? $"({medication.MedicationsCie10}) {medication.MedicationsName} - Observación: {med.MedicationsObservation}"
                                            : "N/A";
                                    })
                                    .ToList() ?? new List<string>();

                                foreach (var indicacion in indicationsList)
                                {
                                    col.Item().MinWidth(300).Text(indicacion).FontSize(12);
                                }

                                // Rellenar espacio si hay menos de 6 medicamentos
                                for (int i = indicationsList.Count; i < 6; i++)
                                {
                                    col.Item().MinWidth(300).Text("").FontSize(12);
                                }
                            });
                        });


                        // Tercera fila (Nueva fila)

                        content.Item().Row(row =>
                        {
                            row.ConstantItem(550).PaddingTop(20).Column(col =>
                            {

                            });

                            row.AutoItem().PaddingHorizontal(10).LineVertical(1).LineColor(Colors.Grey.Lighten1);

                            row.ConstantItem(550).PaddingTop(200).Column(col =>
                            {
                                col.Item().Text("Recomendaciones no Farmacologicas").Bold().FontSize(14); // Nuevo contenido
                                col.Item().Text(consulta.Consultation.ConsultationNonpharmacologycal).FontSize(12);
                                col.Item().Text("").FontSize(12);
                                col.Item().Text("").FontSize(12);
                                col.Item().Text("").FontSize(12);
                                col.Item().Text("").FontSize(12);
                                col.Item().Text("").FontSize(12);
                                col.Item().PaddingTop(30).Text("").FontSize(12);




                            });


                        });



                    });

                    page.Footer().Column(footer =>
                    {// Nueva fila del footer
                        footer.Item().Row(row =>
                        {
                            row.ConstantItem(300).Column(col =>
                            {
                            });
                            row.ConstantItem(230).Border(1).Column(col =>
                            {
                                col.Item().Text("Dispensada").FontSize(11).AlignStart();
                                col.Item().Text("").FontSize(11).AlignStart();
                                col.Item().Text("Completamente:......... Parcialmente:.........").FontSize(11).AlignStart();
                                col.Item().Text("").FontSize(11).AlignStart();

                                col.Item().Text("Proxima Cita").FontSize(11).AlignStart();
                                col.Item().Text("_____________").FontSize(11).AlignCenter();


                            });

                            row.AutoItem().PaddingHorizontal(30).LineVertical(1).LineColor(Colors.Grey.Lighten1);

                            row.ConstantItem(300).Column(col =>
                            {
                            });
                            row.ConstantItem(230).Border(1).Column(col =>
                            {
                                col.Item().Text("Dispensada").FontSize(11).AlignStart();
                                col.Item().Text("").FontSize(11).AlignStart();
                                col.Item().Text("Completamente:......... Parcialmente:.........").FontSize(11).AlignStart();
                                col.Item().Text("").FontSize(11).AlignStart();

                                col.Item().Text("Proxima Cita").FontSize(11).AlignStart();
                                col.Item().Text("_____________").FontSize(11).AlignCenter();


                            });
                        });

                        // Primera fila del footer
                        footer.Item().Row(row =>
                        {
                            row.ConstantItem(530).Column(col =>
                            {
                                col.Item().PaddingVertical(5).LineHorizontal(1).LineColor(Colors.Grey.Lighten1);
                                col.Item().Text(consulta.Consultation.UsersEstablishmentAddress).FontSize(11).AlignCenter();
                            });

                            row.AutoItem().PaddingHorizontal(30).LineVertical(1).LineColor(Colors.Grey.Lighten1);

                            row.RelativeColumn().Column(col =>
                            {
                                col.Item().PaddingVertical(5).LineHorizontal(1).LineColor(Colors.Grey.Lighten1);
                                col.Item().Text(consulta.Consultation.UsersEstablishmentAddress).FontSize(11).AlignCenter();
                            });
                        });


                    });

                });

            });
            // Generar el PDF en bytes.
            // La forma de generar el PDF depende de la versión de QuestPDF, pero generalmente es algo como:
            byte[] pdfBytes = document.GeneratePdf();

            // Devuelve el archivo PDF al navegador.
            return File(pdfBytes, "application/pdf", "MedicationRecipe.pdf");

        }




        public async Task<IActionResult> LaboratoryDoc(int consultationId)
        {
            // Obtener los detalles de la consulta
            var consultation = _consultationService.GetConsultationDetails(consultationId);

            // Verificar si la consulta existe
            if (consultation == null)
            {
                TempData["ErrorMessage"] = "Consulta no encontrada.";
                return RedirectToAction("Index", "Home");
            }

            // Obtener el patientId de la consulta
            var patientId = consultation.ConsultationPatient;

            // Obtener los detalles del paciente
            var patient = await _patientService.GetPatientFullByIdAsync(patientId);
            if (patient == null)
            {
                TempData["ErrorMessage"] = "Paciente no encontrado.";
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
            var consulta = new NewPatientViewModel
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
                Laboratories = laboratories,
                Consultation = consultation // Agregar los detalles de la consulta al ViewModel
            };
            // Formato especializado de laboratorio, tamaño A4 con márgenes pequeños
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);

                    page.Size(1194, 847); // Tamaño A3 horizontal

                    page.DefaultTextStyle(x => x.FontFamily("Arial")); // Cambiar la familia de fuentes a Arial

                    page.Header().Row(row =>
                    {
                        row.ConstantItem(550).Column(col =>
                        {
                            col.Item().Text("Dr:" + consulta.Consultation.UsersNames + consulta.Consultation.UsersSurcenames).Bold().FontSize(14); // Encabezado más grande
                            col.Item().Text("Especialista en: " + consulta.Consultation.SpecialityName).FontSize(12);
                            col.Item().Text("e-mail: " + consulta.Consultation.UsersEmail).FontSize(11);
                            col.Item().Text("Teléfonos: " + consulta.Consultation.UsersPhone).FontSize(11);
                            col.Item().PaddingTop(2).Text("").FontSize(11);
                            col.Item().PaddingVertical(5).LineHorizontal(1).LineColor(Colors.Grey.Lighten1);
                        });

                        row.AutoItem().PaddingHorizontal(10).LineVertical(1).LineColor(Colors.Grey.Lighten1);

                        row.RelativeColumn().Column(col =>
                        {
                            col.Item().Text("Dr:" + consulta.Consultation.UsersNames + consulta.Consultation.UsersSurcenames).Bold().FontSize(14); // Encabezado más grande
                            col.Item().Text("Especialista en: " + consulta.Consultation.SpecialityName).FontSize(12);
                            col.Item().Text("e-mail: " + consulta.Consultation.UsersEmail).FontSize(11);
                            col.Item().Text("Teléfonos: " + consulta.Consultation.UsersPhone).FontSize(11);
                            col.Item().PaddingTop(2).Text("").FontSize(11);
                            col.Item().PaddingVertical(5).LineHorizontal(1).LineColor(Colors.Grey.Lighten1);
                        });
                    });


                    page.Content().Column(content =>
                    {
                        // Primera fila
                        content.Item().Row(row =>
                        {
                            row.ConstantItem(400).Column(col =>
                            {
                                col.Item().PaddingTop(10)
                                   .Text(text =>
                                   {
                                       text.Span("Fecha: ").FontSize(11).Bold(); // Solo "Fecha:" en negrita
                                       text.Span(consulta.Consultation.ConsultationCreationdate.ToString()).FontSize(11); // Valor de la fecha sin negrita
                                   });


                                col.Item().PaddingTop(2)
                                     .Text(text =>
                                     {
                                         text.Span("Apellidos: ").FontSize(11).Bold(); // Solo "Fecha:" en negrita
                                         text.Span(consulta.DetailsPatient.PatientFirstsurname + " " + consulta.DetailsPatient.PatientSecondlastname).FontSize(11); // Valor de la fecha sin negrita
                                     });
                                col.Item().PaddingTop(2)
                                    .Text(text =>
                                    {
                                        text.Span("Nombres: ").FontSize(11).Bold(); // Solo "Fecha:" en negrita
                                        text.Span(consulta.DetailsPatient.PatientFirstname + " " + consulta.DetailsPatient.PatientMiddlename).FontSize(11); // Valor de la fecha sin negrita
                                    });
                                col.Item().PaddingTop(2)
                                    .Text(text =>
                                    {
                                        text.Span("Edad: ").FontSize(11).Bold(); // Solo "Fecha:" en negrita
                                        text.Span(consulta.DetailsPatient.PatientAge.ToString()).FontSize(11); // Valor de la fecha sin negrita
                                    });
                                // Información de diagnósticos
                                col.Item().Text(text =>
                                {
                                    text.Span("Diagnóstico: ").FontSize(11).Bold();

                                    var ultimoDiagnosticoDefinitivo = consulta.Consultation.DiagnosisConsultations?
                                                           .Where(d => d.DiagnosisDefinitive == true)
                                                           .OrderByDescending(d => d.DiagnosisDiagnosisid)
                                                           .FirstOrDefault();

                                    var nombreDiagnostico = ultimoDiagnosticoDefinitivo != null
                                    ? consulta.Diagnoses.FirstOrDefault(d => d.DiagnosisId == ultimoDiagnosticoDefinitivo.DiagnosisDiagnosisid)?.DiagnosisName ?? "N/A"
                                    : "N/A";
                                    text.Span(nombreDiagnostico).FontSize(11); // Mostrar la oración con los diagnósticos

                                });
                            });

                            row.ConstantItem(150).Column(col =>
                            {
                                // Muestra todos los secuenciales de los medicamentos
                                if (consulta.Consultation.LaboratoriesConsultations.Any())
                                {
                                    col.Item().PaddingTop(2)
                                   .Text(text =>
                                   {
                                       text.Span("Receta: ").FontSize(11).Bold(); // Solo "Fecha:" en negrita
                                       text.Span(consulta.Consultation.LaboratoriesConsultations.First().LaboratoriesSequential.ToString()).FontSize(11); // Valor de la fecha sin negrita
                                   });
                                }
                                else
                                {
                                    col.Item().PaddingTop(10).Text("Secuencial: No hay laboratorios").Bold().FontSize(14);
                                }

                                col.Item().PaddingTop(2)
                               .Text(text =>
                               {
                                   text.Span("CC: ").FontSize(11).Bold(); // Solo "Fecha:" en negrita
                                   text.Span(consulta.DetailsPatient.PatientDocumentnumber.ToString()).FontSize(11); // Valor de la fecha sin negrita
                               });
                            });

                            row.AutoItem().PaddingHorizontal(10).LineVertical(1).LineColor(Colors.Grey.Lighten1);

                            row.ConstantItem(400).Column(col =>
                            {
                                col.Item().PaddingTop(10)
                                   .Text(text =>
                                   {
                                       text.Span("Fecha: ").FontSize(11).Bold(); // Solo "Fecha:" en negrita
                                       text.Span(consulta.Consultation.ConsultationCreationdate.ToString()).FontSize(11); // Valor de la fecha sin negrita
                                   });


                                col.Item().PaddingTop(2)
                                     .Text(text =>
                                     {
                                         text.Span("Apellidos: ").FontSize(11).Bold(); // Solo "Fecha:" en negrita
                                         text.Span(consulta.DetailsPatient.PatientFirstsurname + " " + consulta.DetailsPatient.PatientSecondlastname).FontSize(11); // Valor de la fecha sin negrita
                                     });
                                col.Item().PaddingTop(2)
                                    .Text(text =>
                                    {
                                        text.Span("Nombres: ").FontSize(11).Bold(); // Solo "Fecha:" en negrita
                                        text.Span(consulta.DetailsPatient.PatientFirstname + " " + consulta.DetailsPatient.PatientMiddlename).FontSize(11); // Valor de la fecha sin negrita
                                    });
                                col.Item().PaddingTop(2)
                                    .Text(text =>
                                    {
                                        text.Span("Edad: ").FontSize(11).Bold(); // Solo "Fecha:" en negrita
                                        text.Span(consulta.DetailsPatient.PatientAge.ToString()).FontSize(11); // Valor de la fecha sin negrita
                                    });
                                // Información de diagnósticos
                                col.Item().Text(text =>
                                {
                                    text.Span("Diagnóstico: ").FontSize(11).Bold();
                                    var ultimoDiagnosticoDefinitivo = consulta.Consultation.DiagnosisConsultations?
                                                          .Where(d => d.DiagnosisDefinitive == true)
                                                          .OrderByDescending(d => d.DiagnosisDiagnosisid)
                                                          .FirstOrDefault();

                                    var nombreDiagnostico = ultimoDiagnosticoDefinitivo != null
                                    ? consulta.Diagnoses.FirstOrDefault(d => d.DiagnosisId == ultimoDiagnosticoDefinitivo.DiagnosisDiagnosisid)?.DiagnosisName ?? "N/A"
                                    : "N/A";
                                    text.Span(nombreDiagnostico).FontSize(11); // Mostrar la oración con los diagnósticos
                                });
                            });

                            row.ConstantItem(150).Column(col =>
                            {
                                // Muestra todos los secuenciales de los medicamentos
                                if (consulta.Consultation.LaboratoriesConsultations.Any())
                                {
                                    col.Item().PaddingTop(2)
                                   .Text(text =>
                                   {
                                       text.Span("Receta: ").FontSize(11).Bold(); // Solo "Fecha:" en negrita
                                       text.Span(consulta.Consultation.LaboratoriesConsultations.First().LaboratoriesSequential.ToString()).FontSize(11); // Valor de la fecha sin negrita
                                   });
                                }
                                else
                                {
                                    col.Item().PaddingTop(10).Text("Secuencial: No hay medicamentos").Bold().FontSize(14);
                                }

                                col.Item().PaddingTop(2)
                               .Text(text =>
                               {
                                   text.Span("CC: ").FontSize(11).Bold(); // Solo "Fecha:" en negrita
                                   text.Span(consulta.DetailsPatient.PatientDocumentnumber.ToString()).FontSize(11); // Valor de la fecha sin negrita
                               });
                            });
                        });

                        // Segunda fila (Nueva fila)
                        // Segunda fila (Nueva fila)
                        content.Item().Row(row =>
                        {
                            row.ConstantItem(550).PaddingTop(50).Column(col =>
                            {
                                col.Item().PaddingBottom(6).Text("LABORATORIO").Bold().FontSize(14); // Título
                                                                                                     // Recorre la lista de laboratorios y muestra su información
                                                                                                     // Filtrar los medicamentos de la consulta actual
                                var laboratoriosList = consulta.Consultation.LaboratoriesConsultations?
                                    .Select(med =>
                                    {
                                        var laboratorio = consulta.Laboratories.FirstOrDefault(d => d.LaboratoriesId == med.LaboratoriesLaboratoriesid);
                                        return laboratorio != null
                                            ? $"({laboratorio.LaboratoriesCie10}) {laboratorio.LaboratoriesName} - X: {med.LaboratoriesAmount}"
                                            : "N/A";
                                    })
                                    .ToList() ?? new List<string>();

                                foreach (var medicamento in laboratoriosList)
                                {
                                    col.Item().Text(medicamento).FontSize(12);
                                }
                            });

                            row.AutoItem().PaddingHorizontal(10).LineVertical(1).LineColor(Colors.Grey.Lighten1);

                            row.ConstantItem(550).PaddingTop(50).Column(col =>
                            {
                                col.Item().PaddingBottom(20).Text("OBSERVACIONES").Bold().FontSize(14); // Título
                                                                                                        // Recorre la lista de laboratorios y muestra las observaciones
                                                                                                        // Filtrar las indicaciones de la consulta actual
                                var indicationsList = consulta.Consultation.LaboratoriesConsultations?
                                    .Select(med =>
                                    {
                                        var medication = consulta.Laboratories.FirstOrDefault(d => d.LaboratoriesId == med.LaboratoriesLaboratoriesid);
                                        return medication != null
                                            ? $"({medication.LaboratoriesCie10}) {medication.LaboratoriesName} - Observación: {med.LaboratoriesObservation}"
                                            : "N/A";
                                    })
                                    .ToList() ?? new List<string>();

                                foreach (var indicacion in indicationsList)
                                {
                                    col.Item().MinWidth(300).Text(indicacion).FontSize(12);
                                }

                                // Rellenar espacio si hay menos de 6 medicamentos
                                for (int i = indicationsList.Count; i < 6; i++)
                                {
                                    col.Item().MinWidth(300).Text("").FontSize(12);
                                }
                            });
                        });









                    });

                    page.Footer().Column(footer =>
                    {// Nueva fila del footer


                        // Primera fila del footer
                        footer.Item().Row(row =>
                        {
                            row.ConstantItem(530).Column(col =>
                            {
                                col.Item().PaddingVertical(5).LineHorizontal(1).LineColor(Colors.Grey.Lighten1);
                                col.Item().Text(consulta.Consultation.UsersEstablishmentAddress).FontSize(11).AlignCenter();
                            });

                            row.AutoItem().PaddingHorizontal(30).LineVertical(1).LineColor(Colors.Grey.Lighten1);

                            row.RelativeColumn().Column(col =>
                            {
                                col.Item().PaddingVertical(5).LineHorizontal(1).LineColor(Colors.Grey.Lighten1);
                                col.Item().Text(consulta.Consultation.UsersEstablishmentAddress).FontSize(11).AlignCenter();
                            });
                        });


                    });

                });
            });
            // La forma de generar el PDF depende de la versión de QuestPDF, pero generalmente es algo como:
            byte[] pdfBytes = document.GeneratePdf();

            // Devuelve el archivo PDF al navegador.
            return File(pdfBytes, "application/pdf", "Laboratories.pdf");

        }

        public async Task <IActionResult> ImageDoc(int consultationId)
        {
            // Obtener los detalles de la consulta
            var consultation = _consultationService.GetConsultationDetails(consultationId);

            // Verificar si la consulta existe
            if (consultation == null)
            {
                TempData["ErrorMessage"] = "Consulta no encontrada.";
                return RedirectToAction("Index", "Home");
            }

            // Obtener el patientId de la consulta
            var patientId = consultation.ConsultationPatient;

            // Obtener los detalles del paciente
            var patient = await _patientService.GetPatientFullByIdAsync(patientId);
            if (patient == null)
            {
                TempData["ErrorMessage"] = "Paciente no encontrado.";
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
            var consulta = new NewPatientViewModel
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
                Laboratories = laboratories,
                Consultation = consultation // Agregar los detalles de la consulta al ViewModel
            };

            // Informe de imagenología, tamaño A3 con orientación horizontal
            var document= Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);

                    page.Size(1194, 847); // Tamaño A3 horizontal

                    page.DefaultTextStyle(x => x.FontFamily("Arial")); // Cambiar la familia de fuentes a Arial

                    page.Header().Row(row =>
                    {
                        row.ConstantItem(550).Column(col =>
                        {
                            col.Item().Text("Dr:" + consulta.Consultation.UsersNames + consulta.Consultation.UsersSurcenames).Bold().FontSize(14); // Encabezado más grande
                            col.Item().Text("Especialista en: " + consulta.Consultation.SpecialityName).FontSize(12);
                            col.Item().Text("e-mail: " + consulta.Consultation.UsersEmail).FontSize(11);
                            col.Item().Text("Teléfonos: " + consulta.Consultation.UsersPhone).FontSize(11);
                            col.Item().PaddingTop(2).Text("").FontSize(11);
                            col.Item().PaddingVertical(5).LineHorizontal(1).LineColor(Colors.Grey.Lighten1);
                        });

                        row.AutoItem().PaddingHorizontal(10).LineVertical(1).LineColor(Colors.Grey.Lighten1);

                        row.RelativeColumn().Column(col =>
                        {
                            col.Item().Text("Dr:" + consulta.Consultation.UsersNames + consulta.Consultation.UsersSurcenames).Bold().FontSize(14); // Encabezado más grande
                            col.Item().Text("Especialista en: " + consulta.Consultation.SpecialityName).FontSize(12);
                            col.Item().Text("e-mail: " + consulta.Consultation.UsersEmail).FontSize(11);
                            col.Item().Text("Teléfonos: " + consulta.Consultation.UsersPhone).FontSize(11);
                            col.Item().PaddingTop(2).Text("").FontSize(11);
                            col.Item().PaddingVertical(5).LineHorizontal(1).LineColor(Colors.Grey.Lighten1);
                        });
                    });


                    page.Content().Column(content =>
                    {
                        // Primera fila
                        content.Item().Row(row =>
                        {
                            row.ConstantItem(275).Column(col =>
                            {
                                col.Item().PaddingTop(10).Text("Fecha: " + consulta.Consultation.ConsultationCreationdate).FontSize(11).Bold();
                                col.Item().Text("Apellidos: " + consulta.DetailsPatient.PatientFirstsurname + consulta.DetailsPatient.PatientSecondlastname).FontSize(11).Bold();
                                col.Item().Text("Nombres: " + consulta.DetailsPatient.PatientFirstname + consulta.DetailsPatient.PatientMiddlename).FontSize(11).Bold();
                                col.Item().Text("Edad: " + consulta.DetailsPatient.PatientAge).FontSize(11).Bold();
                                col.Item().Text(text =>
                                {
                                    text.Span("Diagnóstico: ").FontSize(11).Bold();

                                    var ultimoDiagnosticoDefinitivo = consulta.Consultation.DiagnosisConsultations?
                                                          .Where(d => d.DiagnosisDefinitive == true)
                                                          .OrderByDescending(d => d.DiagnosisDiagnosisid)
                                                          .FirstOrDefault();

                                    var nombreDiagnostico = ultimoDiagnosticoDefinitivo != null
                                    ? consulta.Diagnoses.FirstOrDefault(d => d.DiagnosisId == ultimoDiagnosticoDefinitivo.DiagnosisDiagnosisid)?.DiagnosisName ?? "N/A"
                                    : "N/A";
                                    text.Span(nombreDiagnostico).FontSize(11); // Mostrar la oración con los diagnósticos

                                });

                            });

                            row.ConstantItem(150).Column(col =>
                            {
                                // Muestra todos los secuenciales de los medicamentos
                                if (consulta.Consultation.ImagesConsultations.Any())
                                {
                                    col.Item().PaddingTop(2)
                                   .Text(text =>
                                   {
                                       text.Span("Receta: ").FontSize(11).Bold(); // Solo "Fecha:" en negrita
                                       text.Span(consulta.Consultation.ImagesConsultations.First().ImagesSequential.ToString()).FontSize(11); // Valor de la fecha sin negrita
                                   });
                                }
                                else
                                {
                                    col.Item().PaddingTop(10).Text("Secuencial: No hay medicamentos").Bold().FontSize(14);
                                }

                                col.Item().PaddingTop(2)
                               .Text(text =>
                               {
                                   text.Span("CC: ").FontSize(11).Bold(); // Solo "Fecha:" en negrita
                                   text.Span(consulta.DetailsPatient.PatientDocumentnumber.ToString()).FontSize(11); // Valor de la fecha sin negrita
                               });
                            });

                            row.AutoItem().PaddingHorizontal(10).LineVertical(1).LineColor(Colors.Grey.Lighten1);

                            row.ConstantItem(275).Column(col =>
                            {
                                col.Item().PaddingTop(10).Text("Fecha: " + consulta.Consultation.ConsultationCreationdate).FontSize(11).Bold();
                                col.Item().Text("Apellidos: " + consulta.DetailsPatient.PatientFirstsurname + consulta.DetailsPatient.PatientSecondlastname).FontSize(11).Bold();
                                col.Item().Text("Nombres: " + consulta.DetailsPatient.PatientFirstname + consulta.DetailsPatient.PatientMiddlename).FontSize(11).Bold();
                                col.Item().Text("Edad: " + consulta.DetailsPatient.PatientAge).FontSize(11).Bold();
                                col.Item().Text(text =>
                                {
                                    text.Span("Diagnóstico: ").FontSize(11).Bold();

                                    var ultimoDiagnosticoDefinitivo = consulta.Consultation.DiagnosisConsultations?
                                                          .Where(d => d.DiagnosisDefinitive == true)
                                                          .OrderByDescending(d => d.DiagnosisDiagnosisid)
                                                          .FirstOrDefault();

                                    var nombreDiagnostico = ultimoDiagnosticoDefinitivo != null
                                    ? consulta.Diagnoses.FirstOrDefault(d => d.DiagnosisId == ultimoDiagnosticoDefinitivo.DiagnosisDiagnosisid)?.DiagnosisName ?? "N/A"
                                    : "N/A";
                                    text.Span(nombreDiagnostico).FontSize(11); // Mostrar la oración con los diagnósticos

                                });
                            });

                            row.ConstantItem(150).Column(col =>
                            {
                                // Muestra todos los secuenciales de los medicamentos
                                if (consulta.Consultation.ImagesConsultations.Any())
                                {
                                    col.Item().PaddingTop(2)
                                   .Text(text =>
                                   {
                                       text.Span("Receta: ").FontSize(11).Bold(); // Solo "Fecha:" en negrita
                                       text.Span(consulta.Consultation.ImagesConsultations.First().ImagesSequential.ToString()).FontSize(11); // Valor de la fecha sin negrita
                                   });
                                }
                                else
                                {
                                    col.Item().PaddingTop(10).Text("Secuencial: No hay medicamentos").Bold().FontSize(14);
                                }

                                col.Item().PaddingTop(2)
                               .Text(text =>
                               {
                                   text.Span("CC: ").FontSize(11).Bold(); // Solo "Fecha:" en negrita
                                   text.Span(consulta.DetailsPatient.PatientDocumentnumber.ToString()).FontSize(11); // Valor de la fecha sin negrita
                               });
                            });
                        });

                        // Segunda fila (Nueva fila)
                        content.Item().Row(row =>
                        {
                            row.ConstantItem(530).PaddingTop(70).Column(col =>
                            {
                                col.Item().Text("IMÁGENES").Bold().FontSize(14); // Título
                                                                                 // Recorre la lista de imágenes y muestra su información
                                var imageList = consulta.Consultation.ImagesConsultations?
                                     .Select(med =>
                                     {
                                         var laboratorio = consulta.Images.FirstOrDefault(d => d.ImagesId == med.ImagesImagesid);
                                         return laboratorio != null
                                             ? $"({laboratorio.ImagesCie10}) {laboratorio.ImagesName} - X: {med.ImagesAmount}"
                                             : "N/A";
                                     })
                                     .ToList() ?? new List<string>();

                                foreach (var medicamento in imageList)
                                {
                                    col.Item().Text(medicamento).FontSize(12);
                                }
                            });

                            

                            row.AutoItem().PaddingHorizontal(10).LineVertical(1).LineColor(Colors.Grey.Lighten1);

                            row.ConstantItem(550).PaddingTop(50).Column(col =>
                            {
                                col.Item().PaddingBottom(20).Text("OBSERVACIONES").Bold().FontSize(14); // Título
                                                                                                        // Recorre la lista de imágenes y muestra las observaciones
                                var indicationsList = consulta.Consultation.ImagesConsultations?
                                  .Select(med =>
                                  {
                                      var medication = consulta.Images.FirstOrDefault(d => d.ImagesId == med.ImagesImagesid);
                                      return medication != null
                                          ? $"({medication.ImagesCie10}) {medication.ImagesName} - Observación: {med.ImagesObservation}"
                                          : "N/A";
                                  })
                                  .ToList() ?? new List<string>();

                                foreach (var indicacion in indicationsList)
                                {
                                    col.Item().MinWidth(300).Text(indicacion).FontSize(12);
                                }

                                // Rellenar espacio si hay menos de 6 medicamentos
                                for (int i = indicationsList.Count; i < 6; i++)
                                {
                                    col.Item().MinWidth(300).Text("").FontSize(12);
                                }
                            });
                        });


                    });

                    page.Footer().Column(footer =>
                    {// Nueva fila del footer


                        // Primera fila del footer
                        footer.Item().Row(row =>
                        {
                            row.ConstantItem(530).Column(col =>
                            {
                                col.Item().PaddingVertical(5).LineHorizontal(1).LineColor(Colors.Grey.Lighten1);
                                col.Item().Text(consulta.Consultation.UsersEstablishmentAddress).FontSize(11).AlignCenter();
                            });

                            row.AutoItem().PaddingHorizontal(30).LineVertical(1).LineColor(Colors.Grey.Lighten1);

                            row.RelativeColumn().Column(col =>
                            {
                                col.Item().PaddingVertical(5).LineHorizontal(1).LineColor(Colors.Grey.Lighten1);
                                col.Item().Text(consulta.Consultation.UsersEstablishmentAddress).FontSize(11).AlignCenter();
                            });
                        });


                    });

                });
            });

            // La forma de generar el PDF depende de la versión de QuestPDF, pero generalmente es algo como:
            byte[] pdfBytes = document.GeneratePdf();

            // Devuelve el archivo PDF al navegador.
            return File(pdfBytes, "application/pdf", "Images.pdf");
        }
    }
}
