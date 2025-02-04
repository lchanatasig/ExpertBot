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

        public async Task<IActionResult> MedicalForm(int consultationId)
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

            // Tamaño de página A4 estándar
            var document = Document.Create(container =>
            {

                container.Page(page =>
                {
                    page.Margin(20);
                    page.Size(598, 845);

                    page.DefaultTextStyle(x => x.FontFamily("Arial").FontSize(10));

                    // Header con una tabla de 6 columnas
                    page.Header().Border(2).BorderColor("#808080").Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(100); // Establecimiento
                            columns.ConstantColumn(100); // Nombre
                            columns.ConstantColumn(100); // Apellido
                            columns.ConstantColumn(70); // Sexo
                            columns.ConstantColumn(70); // Edad
                            columns.ConstantColumn(118); // Nº Historia Clínica
                        });

                        // Fila de encabezados
                        table.Cell().Border(1).BorderColor("#808080").Element(CellStyle => CellStyle.Background("#ccffcc"))
                            .MinHeight(14).AlignCenter().PaddingTop(3).Text("ESTABLECIMIENTO").FontSize(6);
                        table.Cell().Border(1).BorderColor("#808080").Element(CellStyle => CellStyle.Background("#ccffcc"))
                            .MinHeight(14).AlignCenter().PaddingTop(3).Text("NOMBRE").FontSize(6);
                        table.Cell().Border(1).BorderColor("#808080").Element(CellStyle => CellStyle.Background("#ccffcc"))
                            .MinHeight(14).AlignCenter().PaddingTop(3).Text("APELLIDO").FontSize(6);
                        table.Cell().Border(1).BorderColor("#808080").Element(CellStyle => CellStyle.Background("#ccffcc"))
                            .MinHeight(14).AlignCenter().PaddingTop(3).Text("SEXO").FontSize(6);
                        table.Cell().Border(1).BorderColor("#808080").Element(CellStyle => CellStyle.Background("#ccffcc"))
                            .MinHeight(14).AlignCenter().PaddingTop(3).Text("EDAD").FontSize(6);
                        table.Cell().Border(1).BorderColor("#808080").Element(CellStyle => CellStyle.Background("#ccffcc"))
                            .MinHeight(14).AlignCenter().PaddingTop(3).Text("Nº HISTORIA CLÍNICA").FontSize(6);






                        // Fila de contenido
                        table.Cell().Border(1).BorderColor("#808080").MinHeight(7).AlignCenter().PaddingTop(3).Element(CellStyle => CellStyle.Background("#FFFFFF")).Text(consulta.DetailsPatient.PatientAddress)
                            .FontSize(7);
                        table.Cell().Border(1).BorderColor("#808080").MinHeight(7).AlignCenter().PaddingTop(3)
                            .Element(CellStyle => CellStyle.Background("#FFFFFF")).Text(consulta.DetailsPatient.PatientFirstsurname).FontSize(7);
                        table.Cell().Border(1).BorderColor("#808080").MinHeight(7).AlignCenter().PaddingTop(3)
                            .Element(CellStyle => CellStyle.Background("#FFFFFF")).Text(consulta.DetailsPatient.PatientFirstname).FontSize(7);
                        table.Cell().Border(1).BorderColor("#808080").MinHeight(7).AlignCenter().PaddingTop(3)
                            .Element(CellStyle => CellStyle.Background("#FFFFFF")).Text(consulta.DetailsPatient.PatientGenderName).FontSize(7);
                        table.Cell().Border(1).BorderColor("#808080").MinHeight(7).AlignCenter().PaddingTop(3)
                            .Element(CellStyle => CellStyle.Background("#FFFFFF")).Text(consulta.DetailsPatient.PatientAge).FontSize(7);
                        table.Cell().Border(1).BorderColor("#808080").MinHeight(7).AlignCenter().PaddingTop(3)
                            .Element(CellStyle => CellStyle.Background("#FFFFFF")).Text(consulta.DetailsPatient.PatientDocumentnumber).FontSize(7);
                    });

                    // Contenido principal con múltiples tablas
                    page.Content().PaddingTop(6).Column(contentColumn =>
                    {
                        // Primera tabla
                        contentColumn.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(557);
                            });

                            // Fila de encabezado
                            table.Cell().MinHeight(14).Border(2).BorderColor("#808080").Element(CellStyle =>
                                CellStyle.Background("#ccccff")).PaddingLeft(3).Text("1. MOTIVO DE CONSULTA").FontSize(10).Bold();

                            // Fila de datos
                            table.Cell().MinHeight(14).Border(2).BorderColor(Colors.Grey.Medium).Text($"{consulta.Consultation.ConsultationReason}").FontSize(10);
                        });

                        // Segunda tabla
                        contentColumn.Item().PaddingTop(7).Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(557);
                            });

                            // Fila de encabezado
                            table.Cell().MinHeight(14).Border(2).BorderColor("#808080").Element(CellStyle =>
                                CellStyle.Background("#ccccff")).PaddingLeft(3).Text("2. ANTECEDENTES PERSONALES").FontSize(10).Bold();

                            // Fila de datos
                            table.Cell().MinHeight(14).BorderLeft(2).BorderBottom(1).BorderRight(2).BorderColor("#808080").Text($"{consulta.Consultation.ConsultationPersonalbackground}").FontSize(10);
                            // Celda para Alergias
                            // Celda para Alergias
                            table.Cell().MinHeight(14).BorderLeft(2).BorderBottom(1).BorderRight(2).BorderColor("#808080")
     .Column(column =>
     {
         if (consulta.Consultation.AllergiesConsultations != null && consulta.Consultation.AllergiesConsultations.Any())
         {
             // Obtener la lista de nombres de cirugías a partir de los IDs
             var surgeriesName = consulta.Consultation.AllergiesConsultations
                 .Select(surgery => consulta.AllergiesTypes
                 .FirstOrDefault(type => type.CatalogId == surgery.AllergiesCatalogid)?.CatalogName ?? "N/A")
                 .ToList();

             // Unir los nombres de las cirugías en una sola cadena
             var cirugiasTexto = string.Join(", ", surgeriesName);

             column.Item().Text(text =>
             {
                 // "Cirugías:" en negrita
                 text.Span("Alergias:").Bold().FontSize(10);
                 // Nombres de cirugías sin negrita
                 text.Span($" {cirugiasTexto}.").FontSize(8);
             });
         }
         else
         {
             column.Item().Text("Alergias: No se registraron cirugías.").FontSize(10);
         }
     });





                            // Celda para Cirugías
                            table.Cell().MinHeight(14).BorderLeft(2).BorderBottom(1).BorderRight(2).BorderColor("#808080")
      .Column(column =>
      {
          if (consulta.Consultation.SurgeriesConsultations != null && consulta.Consultation.SurgeriesConsultations.Any())
          {
              // Obtener la lista de nombres de cirugías a partir de los IDs
              var surgeriesName = consulta.Consultation.SurgeriesConsultations
                  .Select(surgery => consulta.SurgeriesTypes
                  .FirstOrDefault(type => type.CatalogId == surgery.SurgeriesCatalogid)?.CatalogName ?? "N/A")
                  .ToList();

              // Unir los nombres de las cirugías en una sola cadena
              var cirugiasTexto = string.Join(", ", surgeriesName);

              column.Item().Text(text =>
              {
                  // "Cirugías:" en negrita
                  text.Span("Cirugías:").Bold().FontSize(10);
                  // Nombres de cirugías sin negrita
                  text.Span($" {cirugiasTexto}.").FontSize(8);
              });
          }
          else
          {
              column.Item().Text("Cirugías: No se registraron cirugías.").FontSize(10);
          }
      });






                        });

                        // Tercera tabla

                        contentColumn.Item().PaddingTop(7).Border(2).BorderColor("808080").Table(table =>
                        {
                            // Definir las columnas de la tabla para el encabezado
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(557); // Encabezado general ocupa toda la fila
                            });

                            // Fila de encabezado general "3 ANTECEDENTES FAMILIARES"
                            table.Cell().MinHeight(14).BorderLeft(2).BorderRight(2).BorderTop(2).BorderColor("#808080").Element(CellStyle =>
                                CellStyle.Background("#ccccff")).AlignLeft().PaddingTop(3).Text("3 ANTECEDENTES FAMILIARES").FontSize(10).Bold();

                            table.Cell().Element(CellStyle =>
                            {
                                // Crear una tabla interna con varias columnas dentro de la celda "padre"
                                CellStyle.Background("#ffffff").BorderLeft(2).BorderRight(2).BorderColor("#808080").Table(nestedTable =>
                                {
                                    // Añadir columnas dentro de la tabla anidada
                                    nestedTable.ColumnsDefinition(columns =>
                                    {
                                        columns.ConstantColumn(27); // Primera columna
                                        columns.ConstantColumn(28); // Segunda columna
                                        columns.ConstantColumn(28); // Tercera columna
                                        columns.ConstantColumn(28); // Tercera columna
                                        columns.ConstantColumn(28); // Tercera columna
                                        columns.ConstantColumn(28); // Tercera columna
                                        columns.ConstantColumn(28); // Tercera columna
                                        columns.ConstantColumn(28); // Tercera columna
                                        columns.ConstantColumn(28); // Tercera columna
                                        columns.ConstantColumn(28); // Tercera columna
                                        columns.ConstantColumn(28); // Tercera columna
                                        columns.ConstantColumn(28); // Tercera columna
                                        columns.ConstantColumn(28); // Tercera columna
                                        columns.ConstantColumn(28); // Tercera columna
                                        columns.ConstantColumn(28); // Tercera columna
                                        columns.ConstantColumn(28); // Tercera columna
                                        columns.ConstantColumn(28); // Tercera columna
                                        columns.ConstantColumn(28); // Tercera columna
                                        columns.ConstantColumn(29); // Tercera columna
                                        columns.ConstantColumn(24); // Tercera columna


                                    });

                                    // Fila dentro de la tabla anidada 
                                    nestedTable.Cell().BorderRight(1).BorderTop(1).BorderBottom(1).BorderColor("#C6C2C2").Background("#ccffcc").MinHeight(10).MinWidth(3).Text("1.\nCARDIOPATIA").FontSize(5).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(10).MinWidth(3).PaddingTop(6).Text(consulta.Consultation.FamiliaryBackground.FamiliaryBackgroundHeartdisease == true ? "X" : "").FontSize(7).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ccffcc").MinHeight(10).MinWidth(3).Text("2. \nDIABETES").FontSize(5).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(10).MinWidth(3).PaddingTop(6).Text(consulta.Consultation.FamiliaryBackground.FamiliaryBackgroundDiabetes == true ? "X" : "").FontSize(7).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ccffcc").MinHeight(15).MinWidth(3).Text("3. ENF.CARDIOVASCULAR\n").FontSize(5).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(10).MinWidth(3).PaddingTop(6).Text(consulta.Consultation.FamiliaryBackground.FamiliaryBackgroundDxcardiovascular == true ? "X" : "").FontSize(7).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ccffcc").MinHeight(10).MinWidth(3).Text("4.  HIPERTENSION").FontSize(5).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(10).MinWidth(3).PaddingTop(6).Text(consulta.Consultation.FamiliaryBackground.FamiliaryBackgroundHypertension == true ? "X" : "").FontSize(7).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ccffcc").MinHeight(10).MinWidth(3).Text("5.\nCANCER").FontSize(5).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(10).MinWidth(3).PaddingTop(6).Text(consulta.Consultation.FamiliaryBackground.FamiliaryBackgroundCancer == true ? "X" : "").FontSize(7).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ccffcc").MinHeight(10).MinWidth(3).Text("6. TUBERCULOSIS").FontSize(5).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(10).MinWidth(3).PaddingTop(6).Text(consulta.Consultation.FamiliaryBackground.FamiliaryBackgroundTuberculosis == true ? "X" : "").FontSize(7).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ccffcc").MinHeight(10).MinWidth(3).Text("7.ENF MENTAL").FontSize(5).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(10).MinWidth(3).PaddingTop(6).Text(consulta.Consultation.FamiliaryBackground.FamiliaryBackgroundDxmental == true ? "X" : "").FontSize(7).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ccffcc").MinHeight(10).MinWidth(3).Text("8. ENF INFECCIOSA").FontSize(5).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(10).MinWidth(3).PaddingTop(6).Text(consulta.Consultation.FamiliaryBackground.FamiliaryBackgroundDxinfectious == true ? "X" : "").FontSize(7).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ccffcc").MinHeight(10).MinWidth(3).Text("9. MAL FORMACION").FontSize(5).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(10).MinWidth(3).PaddingTop(6).Text(consulta.Consultation.FamiliaryBackground.FamiliaryBackgroundMalformation == true ? "X" : "").FontSize(7).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ccffcc").MinHeight(10).MinWidth(3).Text("10 OTRO").FontSize(5).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(10).MinWidth(3).PaddingTop(6).Text(consulta.Consultation.FamiliaryBackground.FamiliaryBackgroundOther == true ? "X" : "").FontSize(7).AlignCenter();

                                });
                            });

                            // Crear las observaciones para cada patología con parentesco u observación
                            var observaciones = new List<string>();

                            var familyMembers = consulta.FamilyMember; // Lista de relaciones familiares

                            if (consulta.Consultation.FamiliaryBackground != null)
                            {
                                void AgregarObservacion(string titulo, int? relacionId, string observacion)
                                {
                                    if (relacionId.HasValue || !string.IsNullOrEmpty(observacion))
                                    {
                                        // Buscar el nombre de la relación en la lista de familiares
                                        var relacionNombre = familyMembers?.FirstOrDefault(c => c.CatalogId == relacionId)?.CatalogName ?? "N/A";

                                        // Agregar a la lista de observaciones
                                        observaciones.Add($"{titulo}: Relación - {relacionNombre}, Observación - {observacion}");
                                    }
                                }

                                // Llamadas a la función para agregar cada patología
                                AgregarObservacion("Cardiopatía", consulta.Consultation.FamiliaryBackground.FamiliaryBackgroundRelatshcatalogHeartdisease,
                                                   consulta.Consultation.FamiliaryBackground.FamiliaryBackgroundHeartdiseaseObservation);

                                AgregarObservacion("Diabetes", consulta.Consultation.FamiliaryBackground.FamiliaryBackgroundRelatshcatalogDiabetes,
                                                   consulta.Consultation.FamiliaryBackground.FamiliaryBackgroundDiabetesObservation);

                                AgregarObservacion("Enf. Cardiovascular", consulta.Consultation.FamiliaryBackground.FamiliaryBackgroundRelatshcatalogDxcardiovascular,
                                                   consulta.Consultation.FamiliaryBackground.FamiliaryBackgroundDxcardiovascularObservation);

                                AgregarObservacion("Hipertensión", consulta.Consultation.FamiliaryBackground.FamiliaryBackgroundRelatshcatalogHypertension,
                                                   consulta.Consultation.FamiliaryBackground.FamiliaryBackgroundHypertensionObservation);

                                AgregarObservacion("Cáncer", consulta.Consultation.FamiliaryBackground.FamiliaryBackgroundRelatshcatalogCancer,
                                                   consulta.Consultation.FamiliaryBackground.FamiliaryBackgroundCancerObservation);

                                AgregarObservacion("Tuberculosis", consulta.Consultation.FamiliaryBackground.FamiliaryBackgroundRelatshTuberculosis,
                                                   consulta.Consultation.FamiliaryBackground.FamiliaryBackgroundTuberculosisObservation);

                                AgregarObservacion("Enf. Mental", consulta.Consultation.FamiliaryBackground.FamiliaryBackgroundRelatshcatalogDxmental,
                                                   consulta.Consultation.FamiliaryBackground.FamiliaryBackgroundDxmentalObservation);

                                AgregarObservacion("Enf. Infecciosa", consulta.Consultation.FamiliaryBackground.FamiliaryBackgroundRelatshcatalogDxinfectious,
                                                   consulta.Consultation.FamiliaryBackground.FamiliaryBackgroundDxinfectiousObservation);

                                AgregarObservacion("Mal Formación", consulta.Consultation.FamiliaryBackground.FamiliaryBackgroundRelatshcatalogMalformation,
                                                   consulta.Consultation.FamiliaryBackground.FamiliaryBackgroundMalformationObservation);

                                AgregarObservacion("Otro", consulta.Consultation.FamiliaryBackground.FamiliaryBackgroundRelatshcatalogOther,
                                                   consulta.Consultation.FamiliaryBackground.FamiliaryBackgroundOtherObservation);
                            }

                            // Renderizar observaciones en la tabla si hay alguna
                            if (observaciones.Any())
                            {
                                foreach (var observacion in observaciones)
                                {
                                    var observacionFormateada = char.ToUpper(observacion[0]) + observacion.Substring(1).ToLower();

                                    table.Cell().BorderLeft(2).BorderBottom(1).BorderRight(2).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(12).MinWidth(3)
                                        .Text(observacionFormateada).FontSize(9).AlignStart();
                                }
                            }
                            else
                            {
                                table.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(12).MinWidth(3)
                                    .Text("").FontSize(9).AlignStart();
                            }

                            table.Cell().MinHeight(16).BorderLeft(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Text("").FontSize(10);


                        });
                        //CUARTA TABLA
                        contentColumn.Item().PaddingTop(7).Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(557);
                            });

                            // Fila de encabezado
                            table.Cell().MinHeight(14).Border(2).BorderColor("#808080").Element(CellStyle =>
                                CellStyle.Background("#ccccff")).PaddingLeft(3).Text("4 ENFERMEDAD O PROBLEMA ACTUAL").FontSize(10).Bold();

                            var texto = consulta.Consultation.ConsultationDisease;

                            // Definir un límite de caracteres que se ajuste a una celda.
                            var limiteCaracteresPorFila = 700;

                            // Dividir el texto en fragmentos según el límite
                            var partesTexto = Enumerable.Range(0, (texto.Length + limiteCaracteresPorFila - 1) / limiteCaracteresPorFila)
                                                        .Select(i => texto.Substring(i * limiteCaracteresPorFila, Math.Min(limiteCaracteresPorFila, texto.Length - i * limiteCaracteresPorFila)))
                                                        .ToList();

                            // Generar las filas dinámicamente
                            foreach (var parte in partesTexto)
                            {
                                table.Cell().BorderLeft(2).MinHeight(14).BorderBottom(1).BorderRight(2).BorderColor("#808080").Text(parte).FontSize(10);

                                // Las siguientes celdas serán "quemadas" (vacías)
                                table.Cell().BorderLeft(2).MinHeight(14).BorderBottom(1).BorderRight(2).BorderColor("#808080").Text("").FontSize(10);
                                table.Cell().BorderLeft(2).MinHeight(14).BorderBottom(1).BorderRight(2).BorderColor("#808080").Text("").FontSize(10);
                                table.Cell().BorderLeft(2).MinHeight(14).BorderBottom(1).BorderRight(2).BorderColor("#808080").Text("").FontSize(10);
                                table.Cell().BorderLeft(2).MinHeight(14).BorderBottom(1).BorderRight(2).BorderColor("#808080").Text("").FontSize(10);
                                table.Cell().BorderLeft(2).MinHeight(14).BorderBottom(2).BorderRight(2).BorderColor("#808080").Text("").FontSize(10);
                            }
                        });
                        //QUINTA TABLA
                        contentColumn.Item().PaddingTop(7).Table(table =>
                        {
                            // Definir las columnas de la tabla para el encabezado
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(557); // Encabezado general ocupa toda la fila
                            });

                            // Fila de encabezado general "3 ANTECEDENTES FAMILIARES"
                            table.Cell().MinHeight(14).BorderLeft(2).BorderRight(2).BorderTop(2).BorderColor("#808080").Element(CellStyle =>
                                CellStyle.Background("#ccccff")).AlignLeft().PaddingTop(3).PaddingLeft(3).Text("5 REVISIÓN ACTUAL DE ÓRGANOS Y SISTEMAS").FontSize(10).Bold();

                            table.Cell().Element(CellStyle =>
                            {
                                // Crear una tabla interna con varias columnas dentro de la celda "padre"
                                CellStyle.Background("#ffffff").BorderLeft(2).BorderRight(2).BorderColor("#808080").Table(nestedTable =>
                                {
                                    // Añadir columnas dentro de la tabla anidada
                                    nestedTable.ColumnsDefinition(columns =>
                                    {

                                        columns.ConstantColumn(79); // Tercera columna
                                        columns.ConstantColumn(16); // Tercera columna
                                        columns.ConstantColumn(16); // Tercera columna
                                        columns.ConstantColumn(79); // Tercera columna
                                        columns.ConstantColumn(16); // Tercera columna
                                        columns.ConstantColumn(16); // Tercera columna
                                        columns.ConstantColumn(79); // Tercera columna
                                        columns.ConstantColumn(16); // Tercera columna
                                        columns.ConstantColumn(16); // Tercera columna
                                        columns.ConstantColumn(79); // Tercera columna
                                        columns.ConstantColumn(16); // Tercera columna
                                        columns.ConstantColumn(16); // Tercera columna
                                        columns.ConstantColumn(79); // Tercera columna
                                        columns.ConstantColumn(16); // Tercera columna
                                        columns.ConstantColumn(17); // Tercera columna


                                    });

                                    // Fila dentro de la tabla anidada 
                                    nestedTable.Cell().BorderTop(1).BorderBottom(1).BorderColor("#C6C2C2").Background("#99ccff").MinHeight(10).MinWidth(3).Text("").FontSize(5).Bold().AlignEnd();
                                    nestedTable.Cell().BorderTop(1).BorderColor("#C6C2C2").Background("#99ccff").MinHeight(10).MinWidth(3).Text("CP").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().BorderTop(1).BorderColor("#C6C2C2").Background("#99ccff").MinHeight(10).MinWidth(3).Text("SP").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().BorderTop(1).BorderColor("#C6C2C2").Background("#99ccff").MinHeight(10).MinWidth(3).Text("").Bold().FontSize(5).Bold().AlignEnd();
                                    nestedTable.Cell().BorderTop(1).BorderColor("#C6C2C2").Background("#99ccff").MinHeight(10).MinWidth(3).Text("CP").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().BorderTop(1).BorderColor("#C6C2C2").Background("#99ccff").MinHeight(10).MinWidth(3).Text("SP").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().BorderTop(1).BorderColor("#C6C2C2").Background("#99ccff").MinHeight(10).MinWidth(3).Text("").Bold().FontSize(5).Bold().AlignEnd();
                                    nestedTable.Cell().BorderTop(1).BorderColor("#C6C2C2").Background("#99ccff").MinHeight(10).MinWidth(3).Text("CP").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().BorderTop(1).BorderColor("#C6C2C2").Background("#99ccff").MinHeight(10).MinWidth(3).Text("SP").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().BorderTop(1).BorderColor("#C6C2C2").Background("#99ccff").MinHeight(10).MinWidth(3).Text("").Bold().FontSize(5).Bold().AlignEnd();
                                    nestedTable.Cell().BorderTop(1).BorderColor("#C6C2C2").Background("#99ccff").MinHeight(10).MinWidth(3).Text("CP").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().BorderTop(1).BorderColor("#C6C2C2").Background("#99ccff").MinHeight(10).MinWidth(3).Text("SP").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().BorderTop(1).BorderColor("#C6C2C2").Background("#99ccff").MinHeight(10).MinWidth(3).Text("").Bold().FontSize(5).Bold().AlignEnd();
                                    nestedTable.Cell().BorderTop(1).BorderColor("#C6C2C2").Background("#99ccff").MinHeight(10).MinWidth(3).Text("CP").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().BorderTop(1).BorderColor("#C6C2C2").Background("#99ccff").MinHeight(10).MinWidth(3).Text("SP").Bold().FontSize(5).AlignCenter();



                                });
                            });
                            table.Cell().Element(CellStyle =>
                            {
                                // Crear una tabla interna con varias columnas dentro de la celda "padre"
                                CellStyle.Background("#ffffff").BorderLeft(2).BorderRight(2).BorderColor("#808080").Table(nestedTable =>
                                {
                                    // Añadir columnas dentro de la tabla anidada
                                    nestedTable.ColumnsDefinition(columns =>
                                    {

                                        columns.ConstantColumn(79); // Tercera columna
                                        columns.ConstantColumn(16); // Tercera columna
                                        columns.ConstantColumn(16); // Tercera columna
                                        columns.ConstantColumn(79); // Tercera columna
                                        columns.ConstantColumn(16); // Tercera columna
                                        columns.ConstantColumn(16); // Tercera columna
                                        columns.ConstantColumn(79); // Tercera columna
                                        columns.ConstantColumn(16); // Tercera columna
                                        columns.ConstantColumn(16); // Tercera columna
                                        columns.ConstantColumn(79); // Tercera columna
                                        columns.ConstantColumn(16); // Tercera columna
                                        columns.ConstantColumn(16); // Tercera columna
                                        columns.ConstantColumn(79); // Tercera columna
                                        columns.ConstantColumn(17); // Tercera columna
                                        columns.ConstantColumn(16); // Tercera columna


                                    });

                                    // Fila dentro de la tabla anidada 
                                    nestedTable.Cell().BorderRight(1).BorderTop(1).BorderBottom(1).BorderColor("#C6C2C2").Background("#ccffcc").MinHeight(10).MinWidth(3).Text("1 ÓRGANO DE LOS\r\nSENTIDOS").FontSize(6).Bold().AlignEnd();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(10).MinWidth(3).Text(consulta.Consultation.OrgansSystem.OrganssystemsOrgansenses == true ? "X" : "").FontSize(7).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(10).MinWidth(3).Text(consulta.Consultation.OrgansSystem.OrganssystemsOrgansenses == true ? "" : "X").FontSize(7).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ccffcc").MinHeight(10).MinWidth(3).Text("3 CARDIO\r\nVASCULAR").FontSize(6).Bold().AlignEnd();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(10).MinWidth(3).Text(consulta.Consultation.OrgansSystem.OrganssystemsCardiovascular == true ? "X" : "").FontSize(7).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(10).MinWidth(3).Text(consulta.Consultation.OrgansSystem.OrganssystemsCardiovascular == true ? "" : "X").FontSize(7).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ccffcc").MinHeight(10).MinWidth(3).Text("5.  GENITAL").FontSize(6).Bold().AlignEnd();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(10).MinWidth(3).Text(consulta.Consultation.OrgansSystem.OrganssystemsGenital == true ? "X" : "").FontSize(7).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(10).MinWidth(3).Text(consulta.Consultation.OrgansSystem.OrganssystemsGenital == true ? "" : "X").FontSize(7).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ccffcc").MinHeight(10).MinWidth(3).Text("7. MÚSCULO\r\nESQUELÉTICO").FontSize(6).Bold().AlignEnd();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(10).MinWidth(3).Text(consulta.Consultation.OrgansSystem.OrganssystemsSkeletalM == true ? "" : "X").FontSize(7).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(10).MinWidth(3).Text(consulta.Consultation.OrgansSystem.OrganssystemsSkeletalM == true ? "X" : "").FontSize(7).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ccffcc").MinHeight(10).MinWidth(3).Text("9. HEMO LINFÁTICO").FontSize(6).Bold().AlignEnd();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(10).MinWidth(3).Text(consulta.Consultation.OrgansSystem.OrganssystemsLymphatic == true ? "X" : "").FontSize(7).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(10).MinWidth(3).Text(consulta.Consultation.OrgansSystem.OrganssystemsLymphatic == true ? "" : "X").FontSize(7).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ccffcc").MinHeight(10).MinWidth(3).Text("2. RESPIRATORIO").FontSize(6).Bold().AlignEnd();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(10).MinWidth(3).Text(consulta.Consultation.OrgansSystem.OrganssystemsRespiratory == true ? "X" : "").FontSize(7).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(10).MinWidth(3).Text(consulta.Consultation.OrgansSystem.OrganssystemsRespiratory == true ? "" : "X").FontSize(7).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ccffcc").MinHeight(10).MinWidth(3).Text("4. DIGESTIVO").FontSize(6).Bold().AlignEnd();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(10).MinWidth(3).Text(consulta.Consultation.OrgansSystem.OrganssystemsDigestive == true ? "X" : "").FontSize(7).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(10).MinWidth(3).Text(consulta.Consultation.OrgansSystem.OrganssystemsDigestive == true ? "" : "X").FontSize(7).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ccffcc").MinHeight(10).MinWidth(3).Text("6. URINARIO").FontSize(6).Bold().AlignEnd();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(10).MinWidth(3).Text(consulta.Consultation.OrgansSystem.OrganssystemsUrinary == true ? "X" : "").FontSize(7).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(10).MinWidth(3).Text(consulta.Consultation.OrgansSystem.OrganssystemsUrinary == true ? "" : "X").FontSize(7).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ccffcc").MinHeight(10).MinWidth(3).Text("8. ENDOCRINO").FontSize(6).Bold().AlignEnd();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(10).MinWidth(3).Text(consulta.Consultation.OrgansSystem.OrganssystemsEndrocrine == true ? "X" : "").FontSize(7).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(10).MinWidth(3).Text(consulta.Consultation.OrgansSystem.OrganssystemsEndrocrine == true ? "" : "X").FontSize(7).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ccffcc").MinHeight(10).MinWidth(3).Text("10. NERVIOSO").FontSize(6).Bold().AlignEnd();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(10).MinWidth(3).Text(consulta.Consultation.OrgansSystem.OrganssystemsNervous == true ? "X" : "").FontSize(7).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(10).MinWidth(3).Text(consulta.Consultation.OrgansSystem.OrganssystemsNervous == true ? "" : "X").FontSize(7).AlignCenter();
                                });
                            });
                            // Lista de observaciones (puedes ajustarla según tu modelo de datos real)
                            var observaciones = new List<string>
{
    consulta.Consultation.OrgansSystem.OrganssystemsOrgansensesObs,
    consulta.Consultation.OrgansSystem.OrganssystemsCardiovascularObs,
    consulta.Consultation.OrgansSystem.OrganssystemsGenitalObs,
    consulta.Consultation.OrgansSystem.OrganssystemsSkeletalMObs,
    consulta.Consultation.OrgansSystem.OrganssystemsLymphaticObs,
    consulta.Consultation.OrgansSystem.OrganssystemsRespiratoryObs,
    consulta.Consultation.OrgansSystem.OrganssystemsDigestiveObs,
    consulta.Consultation.OrgansSystem.OrganssystemsUrinaryObs,
    consulta.Consultation.OrgansSystem.OrganssystemsEndocrine,
    consulta.Consultation.OrgansSystem.OrganssystemsNervousObs,
    // Agrega más observaciones aquí
};

                            // Iterar sobre las observaciones para generar las filas dinámicamente
                            foreach (var observacion in observaciones.Where(o => !string.IsNullOrEmpty(o)))
                            {
                                // Primera celda con la observación
                                table.Cell()
                                    .MinHeight(13)
                                    .BorderLeft(2)
                                    .BorderRight(2)
                                    .BorderBottom(1)
                                    .BorderColor("#808080")
                                    .Text(observacion)
                                    .FontSize(10);

                                // Las siguientes celdas están quemadas (vacías)
                            }



                        });
                        //SEXTA TABLA
                        contentColumn.Item().PaddingTop(7).Table(table =>
                        {
                            // Definir las columnas de la tabla para el encabezado
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(557); // Encabezado general ocupa toda la fila
                            });

                            // Fila de encabezado general "3 ANTECEDENTES FAMILIARES"
                            table.Cell().MinHeight(14).BorderLeft(2).BorderRight(2).BorderTop(2).BorderColor("#808080").Element(CellStyle =>
                                CellStyle.Background("#ccccff")).AlignLeft().PaddingTop(3).PaddingLeft(3).Text("6 SIGNOS VITALES Y ANTROPOMETRIA").FontSize(10).Bold();
                            table.Cell().Element(CellStyle =>
                            {
                                // Crear una tabla interna con varias columnas dentro de la celda "padre"
                                CellStyle.Background("#ffffff").BorderLeft(2).BorderRight(2).BorderColor("#808080").Table(nestedTable =>
                                {
                                    // Añadir columnas dentro de la tabla anidada
                                    nestedTable.ColumnsDefinition(columns =>
                                    {

                                        columns.ConstantColumn(92); // Tercera columna
                                        columns.ConstantColumn(92); // Tercera columna
                                        columns.ConstantColumn(92); // Tercera columna
                                        columns.ConstantColumn(92); // Tercera columna
                                        columns.ConstantColumn(92); // Tercera columna
                                        columns.ConstantColumn(95); // Tercera columna


                                    });

                                    // Fila dentro de la tabla anidada 
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ccffcc").MinHeight(10).MinWidth(3).Text("FECHA DE MEDICIÓN").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).AlignCenter().Text(consulta.Consultation.ConsultationCreationdate).Bold().FontSize(7);
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text("").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text("").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text("").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text("").Bold().FontSize(5).AlignCenter();
                                    // Fila dentro de la tabla anidada 
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ccffcc").MinHeight(10).MinWidth(3).Text("TEMPERATURA °C").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text(consulta.Consultation.ConsultationTemperature).Bold().FontSize(7).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text("").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text("").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text("").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text("").Bold().FontSize(5).AlignCenter();


                                });
                            });
                            table.Cell().Element(CellStyle =>
                            {
                                // Crear una tabla interna con varias columnas dentro de la celda "padre"
                                CellStyle.Background("#ffffff").BorderLeft(2).BorderRight(2).BorderColor("#808080").Table(nestedTable =>
                                {
                                    // Añadir columnas dentro de la tabla anidada
                                    nestedTable.ColumnsDefinition(columns =>
                                    {

                                        columns.ConstantColumn(92); // Tercera columna
                                        columns.ConstantColumn(46); // Tercera columna
                                        columns.ConstantColumn(46); // Tercera columna
                                        columns.ConstantColumn(46); // Tercera columna
                                        columns.ConstantColumn(46); // Tercera columna
                                        columns.ConstantColumn(46); // Tercera columna
                                        columns.ConstantColumn(46); // Tercera columna
                                        columns.ConstantColumn(46); // Tercera columna
                                        columns.ConstantColumn(46); // Tercera columna
                                        columns.ConstantColumn(46); // Tercera columna
                                        columns.ConstantColumn(47); // Tercera columna


                                    });

                                    // Fila dentro de la tabla anidada 
                                    nestedTable.Cell().BorderRight(1).BorderTop(1).BorderBottom(1).BorderColor("#C6C2C2").Background("#ccffcc").MinHeight(10).MinWidth(3).Text("PRESIÓN ARTERIAL").FontSize(5).Bold().AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text(consulta.Consultation.ConsultationBloodpressuredAs).FontSize(7).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text(consulta.Consultation.ConsultationBloodpresuredDis).FontSize(7).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();

                                });
                            });

                            table.Cell().BorderBottom(2).BorderColor("808080").Element(CellStyle =>
                            {
                                // Crear una tabla interna con varias columnas dentro de la celda "padre"
                                CellStyle.Background("#ffffff").BorderLeft(2).BorderRight(2).BorderColor("#808080").Table(nestedTable =>
                                {
                                    // Añadir columnas dentro de la tabla anidada
                                    nestedTable.ColumnsDefinition(columns =>
                                    {

                                        columns.ConstantColumn(46); // Tercera columna
                                        columns.ConstantColumn(46); // Tercera columna
                                        columns.ConstantColumn(46); // Tercera columna
                                        columns.ConstantColumn(46); // Tercera columna
                                        columns.ConstantColumn(46); // Tercera columna
                                        columns.ConstantColumn(46); // Tercera columna
                                        columns.ConstantColumn(46); // Tercera columna
                                        columns.ConstantColumn(46); // Tercera columna
                                        columns.ConstantColumn(46); // Tercera columna
                                        columns.ConstantColumn(46); // Tercera columna
                                        columns.ConstantColumn(46); // Tercera columna
                                        columns.ConstantColumn(47); // Tercera columna


                                    });

                                    // Fila dentro de la tabla anidada 
                                    nestedTable.Cell().BorderRight(1).BorderTop(1).BorderBottom(1).BorderColor("#C6C2C2").Background("#ccffcc").MinHeight(10).MinWidth(3).Text("PULSO / min").FontSize(5).Bold().AlignCenter();
                                    nestedTable.Cell().BorderRight(1).BorderTop(1).BorderBottom(1).BorderColor("#C6C2C2").Background("#ccffcc").MinHeight(10).MinWidth(3).Text("FRECUENCIA\r\nRESPIRATORIA").FontSize(5).Bold().AlignCenter();

                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text(consulta.Consultation.ConsultationPulse).FontSize(7).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text(consulta.Consultation.ConsultationRespirationrate).FontSize(7).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();

                                    nestedTable.Cell().BorderRight(1).BorderBottom(2).BorderColor("#808080").Background("#ccffcc").MinHeight(10).MinWidth(3).Text("PESO / Kg").FontSize(5).Bold().AlignCenter();
                                    nestedTable.Cell().BorderRight(1).BorderBottom(2).BorderColor("#808080").Background("#ccffcc").MinHeight(10).MinWidth(3).Text("TALLA / cm").FontSize(5).Bold().AlignCenter();

                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text(consulta.Consultation.ConsultationSize).FontSize(7).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text(consulta.Consultation.ConsultationWeight).FontSize(7).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();

                                });
                            });



                        });

                        //SEPTIMA TABLA
                        contentColumn.Item().PaddingTop(7).Table(table =>
                        {
                            // Definir las columnas de la tabla principal para el encabezado
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(185); // Primera columna
                                columns.ConstantColumn(185); // Segunda columna
                                columns.ConstantColumn(187); // Tercera columna
                            });

                            // Fila de encabezado "7 EXAMEN FÍSICO REGIONAL"
                            table.Cell().MinHeight(14).BorderLeft(2).BorderTop(2).BorderColor("#808080").Element(CellStyle =>
                                CellStyle.Background("#ccccff")).AlignLeft().PaddingTop(3).PaddingLeft(3).Text("7 EXAMEN FÍSICO REGIONAL ").FontSize(10).Bold();

                            table.Cell().MinHeight(14).BorderTop(2).BorderColor("#808080").Element(CellStyle =>
                                CellStyle.Background("#ccccff")).AlignLeft().PaddingTop(3).PaddingLeft(70).Text("CP = CON EVIDENCIA DE PATOLOGÍA: MARCAR \"X\" Y DESCRIBIR ").FontSize(6).Bold().AlignCenter();

                            table.Cell().MinHeight(14).BorderRight(2).BorderTop(2).BorderColor("#808080").Element(CellStyle =>
                                CellStyle.Background("#ccccff")).AlignLeft().PaddingTop(3).PaddingLeft(65).Text("SP = SIN EVIDENCIA DE PATOLOGÍA:\r\n MARCAR \"X\" Y NO DESCRIBIR\r\n").FontSize(6).Bold().AlignCenter();

                            // Aquí creamos una nueva celda para la tabla interna con 18 columnas
                            table.Cell().ColumnSpan(3).Element(CellStyle =>
                            {
                                // Crear una tabla interna con 18 columnas
                                CellStyle.Background("#ffffff").BorderLeft(2).BorderRight(2).BorderColor("#808080").Table(nestedTable =>
                                {
                                    // Definir 18 columnas dentro de la tabla anidada
                                    nestedTable.ColumnsDefinition(columns =>
                                    {
                                        columns.ConstantColumn(70);  // Columna 1
                                        columns.ConstantColumn(16);  // Columna 2
                                        columns.ConstantColumn(10);  // Columna 3
                                        columns.ConstantColumn(70);  // Columna 4
                                        columns.ConstantColumn(10);  // Columna 5
                                        columns.ConstantColumn(10);  // Columna 6
                                        columns.ConstantColumn(70);  // Columna 7
                                        columns.ConstantColumn(10);  // Columna 8
                                        columns.ConstantColumn(10);  // Columna 9
                                        columns.ConstantColumn(70);  // Columna 10
                                        columns.ConstantColumn(10);  // Columna 11
                                        columns.ConstantColumn(10);  // Columna 12
                                        columns.ConstantColumn(70);  // Columna 13
                                        columns.ConstantColumn(10);  // Columna 14
                                        columns.ConstantColumn(10);  // Columna 15
                                        columns.ConstantColumn(79);  // Columna 16
                                        columns.ConstantColumn(10);  // Columna 17
                                        columns.ConstantColumn(10);  // Columna 18
                                    });

                                    // Fila dentro de la tabla anidada con 18 celdas creadas manualmente
                                    // Aquí agregas todas las celdas para la tabla de 18 columnas
                                    nestedTable.Cell().BorderTop(1).BorderBottom(1).BorderColor("#C6C2C2").Background("#99ccff").MinHeight(10).MinWidth(3).Text("").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().BorderTop(1).BorderBottom(1).BorderColor("#C6C2C2").Background("#99ccff").MinHeight(10).MinWidth(3).Text("CP").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().BorderTop(1).BorderLeft(2).BorderBottom(1).BorderColor("#C6C2C2").Background("#99ccff").MinHeight(10).MinWidth(3).Text("SP").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().BorderTop(1).BorderBottom(1).BorderColor("#C6C2C2").Background("#99ccff").MinHeight(10).MinWidth(3).Text("").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().BorderTop(1).BorderBottom(1).BorderColor("#C6C2C2").Background("#99ccff").MinHeight(10).MinWidth(3).Text("CP").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().BorderTop(1).BorderLeft(2).BorderBottom(1).BorderColor("#C6C2C2").Background("#99ccff").MinHeight(10).MinWidth(3).Text("SP").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().BorderTop(1).BorderBottom(1).BorderColor("#C6C2C2").Background("#99ccff").MinHeight(10).MinWidth(3).Text("").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().BorderTop(1).BorderBottom(1).BorderColor("#C6C2C2").Background("#99ccff").MinHeight(10).MinWidth(3).Text("CP").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().BorderTop(1).BorderLeft(2).BorderBottom(1).BorderColor("#C6C2C2").Background("#99ccff").MinHeight(10).MinWidth(3).Text("SP").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().BorderTop(1).BorderBottom(1).BorderColor("#C6C2C2").Background("#99ccff").MinHeight(10).MinWidth(3).Text("").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().BorderTop(1).BorderBottom(1).BorderColor("#C6C2C2").Background("#99ccff").MinHeight(10).MinWidth(3).Text("CP").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().BorderTop(1).BorderLeft(2).BorderBottom(1).BorderColor("#C6C2C2").Background("#99ccff").MinHeight(10).MinWidth(3).Text("SP").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().BorderTop(1).BorderBottom(1).BorderColor("#C6C2C2").Background("#99ccff").MinHeight(10).MinWidth(3).Text("").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().BorderTop(1).BorderBottom(1).BorderColor("#C6C2C2").Background("#99ccff").MinHeight(10).MinWidth(3).Text("CP").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().BorderTop(1).BorderLeft(2).BorderBottom(1).BorderColor("#C6C2C2").Background("#99ccff").MinHeight(10).MinWidth(3).Text("SP").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().BorderTop(1).BorderBottom(1).BorderColor("#C6C2C2").Background("#99ccff").MinHeight(10).MinWidth(3).Text("").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().BorderTop(1).BorderBottom(1).BorderColor("#C6C2C2").Background("#99ccff").MinHeight(10).MinWidth(3).Text("CP").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().BorderTop(1).BorderLeft(2).BorderBottom(1).BorderColor("#C6C2C2").Background("#99ccff").MinHeight(10).MinWidth(3).Text("SP").Bold().FontSize(5).AlignCenter();
                                    // Continúa agregando las celdas necesarias
                                    nestedTable.Cell().BorderTop(1).BorderBottom(1).BorderColor("#C6C2C2").Background("#ccffcc").MinHeight(12).MinWidth(3).Text("1. CABEZA").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().BorderTop(1).BorderBottom(1).BorderLeft(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(12).MinWidth(3).Text(consulta.Consultation.PhysicalExamination.PhysicalexaminationHead == true ? "X" : "").Bold().FontSize(7).AlignCenter();
                                    nestedTable.Cell().BorderTop(1).BorderBottom(1).BorderLeft(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(12).MinWidth(3).Text(consulta.Consultation.PhysicalExamination.PhysicalexaminationHead == true ? "" : "X").Bold().FontSize(7).AlignCenter();

                                    nestedTable.Cell().BorderTop(1).BorderBottom(1).BorderColor("#C6C2C2").Background("#ccffcc").MinHeight(12).MinWidth(3).Text("2. CUELLO").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().BorderTop(1).BorderBottom(1).BorderLeft(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(12).MinWidth(3).Text(consulta.Consultation.PhysicalExamination.PhysicalexaminationNeck == true ? "X" : "").Bold().FontSize(7).AlignCenter();
                                    nestedTable.Cell().BorderTop(1).BorderBottom(1).BorderLeft(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(12).MinWidth(3).Text(consulta.Consultation.PhysicalExamination.PhysicalexaminationNeck == true ? "" : "X").Bold().FontSize(7).AlignCenter();

                                    nestedTable.Cell().BorderTop(1).BorderBottom(1).BorderColor("#C6C2C2").Background("#ccffcc").MinHeight(10).MinWidth(3).Text("3. TÓRAX").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().BorderTop(1).BorderBottom(1).BorderLeft(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(12).MinWidth(3).Text(consulta.Consultation.PhysicalExamination.PhysicalexaminationChest == true ? "X" : "").Bold().FontSize(7).AlignCenter();
                                    nestedTable.Cell().BorderTop(1).BorderBottom(1).BorderLeft(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(12).MinWidth(3).Text(consulta.Consultation.PhysicalExamination.PhysicalexaminationChest == true ? "" : "X").Bold().FontSize(7).AlignCenter();

                                    nestedTable.Cell().BorderTop(1).BorderBottom(1).BorderColor("#C6C2C2").Background("#ccffcc").MinHeight(12).MinWidth(3).Text("4. ABDOMEN").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().BorderTop(1).BorderBottom(1).BorderLeft(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(12).MinWidth(3).Text(consulta.Consultation.PhysicalExamination.PhysicalexaminationAbdomen == true ? "X" : "").Bold().FontSize(7).AlignCenter();
                                    nestedTable.Cell().BorderTop(1).BorderBottom(1).BorderLeft(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(12).MinWidth(3).Text(consulta.Consultation.PhysicalExamination.PhysicalexaminationAbdomen == true ? "" : "X").Bold().FontSize(7).AlignCenter();

                                    nestedTable.Cell().BorderTop(1).BorderBottom(1).BorderColor("#C6C2C2").Background("#ccffcc").MinHeight(12).MinWidth(3).Text("5. PELVIS").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().BorderTop(1).BorderBottom(1).BorderLeft(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(12).MinWidth(3).Text(consulta.Consultation.PhysicalExamination.PhysicalexaminationPelvis == true ? "X" : "").Bold().FontSize(7).AlignCenter();
                                    nestedTable.Cell().BorderTop(1).BorderBottom(1).BorderLeft(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(12).MinWidth(3).Text(consulta.Consultation.PhysicalExamination.PhysicalexaminationPelvis == true ? "" : "X").Bold().FontSize(7).AlignCenter();

                                    nestedTable.Cell().BorderTop(1).BorderBottom(1).BorderColor("#C6C2C2").Background("#ccffcc").MinHeight(12).MinWidth(3).Text("6 . EXTREMIDADES").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().BorderTop(1).BorderBottom(1).BorderLeft(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(12).MinWidth(3).Text(consulta.Consultation.PhysicalExamination.PhysicalexaminationLimbs == true ? "X" : "").Bold().FontSize(7).AlignCenter();
                                    nestedTable.Cell().BorderTop(1).BorderBottom(1).BorderLeft(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(12).MinWidth(3).Text(consulta.Consultation.PhysicalExamination.PhysicalexaminationLimbs == true ? "" : "X").Bold().FontSize(7).AlignCenter();


                                });
                            });

                            // Aquí agregamos una segunda tabla anidada que abarque todo el ancho
                            table.Cell().ColumnSpan(3).Element(CellStyle =>
                            {
                                // Crear una tabla interna con una sola columna
                                CellStyle.Background("#ffffff").BorderLeft(2).BorderRight(2).BorderBottom(2).BorderColor("#808080").Table(nestedTable =>
                                {
                                    // Definir una sola columna
                                    nestedTable.ColumnsDefinition(columns =>
                                    {
                                        columns.RelativeColumn(1); // Una columna que abarca todo el ancho
                                    });

                                    if (!string.IsNullOrEmpty(consulta.Consultation.PhysicalExamination.PhysicalexaminationHeadObs))
                                    {
                                        nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(12).MinWidth(3)
                                            .Text(consulta.Consultation.PhysicalExamination.PhysicalexaminationNeckObs).FontSize(9).AlignStart();
                                    }

                                    if (!string.IsNullOrEmpty(consulta.Consultation.PhysicalExamination.PhysicalexaminationHeadObs))
                                    {
                                        nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(12).MinWidth(3)
                                            .Text(consulta.Consultation.PhysicalExamination.PhysicalexaminationNeckObs).FontSize(9).AlignStart();
                                    }

                                    if (!string.IsNullOrEmpty(consulta.Consultation.PhysicalExamination.PhysicalexaminationChestObs))
                                    {
                                        nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(12).MinWidth(3)
                                            .Text(consulta.Consultation.PhysicalExamination.PhysicalexaminationChestObs).FontSize(9).AlignStart();
                                    }

                                    if (!string.IsNullOrEmpty(consulta.Consultation.PhysicalExamination.PhysicalexaminationAbdomenObs))
                                    {
                                        nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(12).MinWidth(3)
                                            .Text(consulta.Consultation.PhysicalExamination.PhysicalexaminationAbdomenObs).FontSize(9).AlignStart();
                                    }

                                    if (!string.IsNullOrEmpty(consulta.Consultation.PhysicalExamination.PhysicalexaminationPelvisObs))
                                    {
                                        nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(12).MinWidth(3)
                                            .Text(consulta.Consultation.PhysicalExamination.PhysicalexaminationPelvisObs).FontSize(9).AlignStart();
                                    }

                                    if (!string.IsNullOrEmpty(consulta.Consultation.PhysicalExamination.PhysicalexaminationLimbsObs))
                                    {
                                        nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(12).MinWidth(3)
                                            .Text(consulta.Consultation.PhysicalExamination.PhysicalexaminationLimbsObs).FontSize(9).AlignStart();
                                    }


                                });
                            });
                        });

                        //OCTAVA TABLA

                        contentColumn.Item().PaddingTop(7).Table(table =>
                        {
                            // Definir las columnas de la tabla principal con tamaños específicos
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(170);  // Columna 1 (Diagnóstico 1)
                                columns.ConstantColumn(95);   // Columna 2 (CIE Diagnóstico 1)
                                columns.ConstantColumn(19);   // Columna 3 (PRE Diagnóstico 1)
                                columns.ConstantColumn(20);   // Columna 4 (DEF Diagnóstico 1)
                                columns.ConstantColumn(12);   // Espacio entre diagnósticos
                                columns.ConstantColumn(193);  // Columna 5 (Diagnóstico 2)
                                columns.ConstantColumn(13);   // Columna 6 (CIE Diagnóstico 2)
                                columns.ConstantColumn(16);   // Columna 7 (PRE Diagnóstico 2)
                                columns.ConstantColumn(18);   // Columna 8 (DEF Diagnóstico 2)
                            });

                            // Fila de encabezado "8 DIAGNOSTICO"
                            table.Cell().MinHeight(14).BorderLeft(2).BorderTop(2).BorderBottom(2).BorderColor("#808080").Element(CellStyle =>
                                CellStyle.Background("#ccccff")).AlignLeft().PaddingTop(3).PaddingLeft(3).Text("8 DIAGNOSTICO").FontSize(10).Bold();

                            table.Cell().MinHeight(14).BorderTop(2).BorderBottom(2).BorderColor("#808080").Element(CellStyle =>
                                CellStyle.Background("#ccccff")).AlignLeft().PaddingTop(3).Text("PRE = PRESUNTIVO\r\nDEF = DEFINITIVO").FontSize(7).Bold();

                            // Encabezados de la tabla
                            table.Cell().MinHeight(14).BorderTop(2).BorderBottom(2).BorderColor("#808080").Element(CellStyle =>
                                CellStyle.Background("#ccccff")).AlignCenter().PaddingTop(3).MinWidth(2).Text("CIE").FontSize(6).Bold();
                            table.Cell().MinHeight(14).BorderTop(2).BorderBottom(2).BorderColor("#808080").Element(CellStyle =>
                                CellStyle.Background("#ccccff")).AlignCenter().PaddingTop(3).MinWidth(2).Text("PRE").FontSize(6).Bold();
                            table.Cell().MinHeight(14).BorderTop(2).BorderBottom(2).BorderColor("#808080").Element(CellStyle =>
                                CellStyle.Background("#ccccff")).AlignCenter().PaddingTop(3).MinWidth(2).Text("DEF").FontSize(6).Bold();
                            table.Cell().MinHeight(14).BorderTop(2).BorderBottom(2).BorderColor("#808080").Element(CellStyle =>
                                CellStyle.Background("#ccccff")).AlignCenter().PaddingTop(3).MinWidth(2).Text("").FontSize(6).Bold();
                            table.Cell().MinHeight(14).BorderTop(2).BorderBottom(2).BorderColor("#808080").Element(CellStyle =>
                                CellStyle.Background("#ccccff")).AlignCenter().PaddingTop(3).MinWidth(2).Text("CIE").FontSize(6).Bold();
                            table.Cell().MinHeight(14).BorderTop(2).BorderBottom(2).BorderColor("#808080").Element(CellStyle =>
                                CellStyle.Background("#ccccff")).AlignCenter().PaddingTop(3).MinWidth(2).Text("PRE").FontSize(6).Bold();
                            table.Cell().MinHeight(14).BorderTop(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Element(CellStyle =>
                                CellStyle.Background("#ccccff")).AlignCenter().PaddingTop(3).MinWidth(2).Text("DEF").FontSize(6).Bold();

                            // Crear una tabla dentro de la celda para los diagnósticos
                            table.Cell().ColumnSpan(9).Element(CellStyle =>
                            {
                                CellStyle.Background("#ffffff").BorderLeft(2).BorderRight(2).BorderBottom(2).BorderColor("#808080").Table(nestedTable =>
                                {
                                    // Definir columnas dentro de la subtabla para los diagnósticos
                                    nestedTable.ColumnsDefinition(columns =>
                                    {
                                        columns.ConstantColumn(14);  // Columna 1
                                        columns.ConstantColumn(250); // Columna 2
                                        columns.ConstantColumn(20);  // Columna 3
                                        columns.ConstantColumn(18);  // Columna 4
                                        columns.ConstantColumn(16);  // Columna 5
                                        columns.ConstantColumn(14);  // Columna 6
                                        columns.RelativeColumn(2);   // Columna 7
                                        columns.ConstantColumn(18);  // Columna 8
                                        columns.ConstantColumn(18);  // Columna 9
                                        columns.ConstantColumn(20);  // Columna 10
                                    });

                                    // Agregar diagnósticos en pares
                                    int rowIndex = 1;
                                    for (int i = 0; i < consulta.Consultation.DiagnosisConsultations.Count; i += 2)
                                    {
                                        var diagnostico1 = consulta.Diagnoses[i]; // Primer diagnóstico en la fila
                                        var diagnostico2 = (i + 1 < consulta.Diagnoses.Count) ? consulta.Diagnoses[i + 1] : null;

                                        // Buscar las consultas relacionadas con los diagnósticos
                                        var consultaDiagnostico1 = consulta.Consultation.DiagnosisConsultations.FirstOrDefault(dc => dc.DiagnosisDiagnosisid == diagnostico1.DiagnosisId);
                                        var consultaDiagnostico2 = diagnostico2 != null ? consulta.Consultation.DiagnosisConsultations.FirstOrDefault(dc => dc.DiagnosisDiagnosisid == diagnostico2.DiagnosisId) : null;

                                        // Columna 1 (Número de fila o algún identificador)
                                        nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ccffcc").MinHeight(12).MinWidth(3)
                                            .Text(rowIndex.ToString()).FontSize(9).AlignCenter();

                                        // Columna 2: Diagnóstico 1
                                        nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ffffff").MinHeight(12)
                                            .Text(diagnostico1.DiagnosisName).FontSize(9).AlignCenter();

                                        // Columna 3: CIE10 Diagnóstico 1
                                        nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ffffff").MinHeight(12)
                                            .Text(diagnostico1.DiagnosisCie10.ToString()).FontSize(9).AlignCenter();

                                        // Columna 4: Presuntivo Diagnóstico 1
                                        nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(12)
                                            .Text(consultaDiagnostico1?.DiagnosisPresumptive == true ? "X" : "").FontSize(9).AlignCenter();

                                        // Columna 5: Definitivo Diagnóstico 1
                                        nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(12)
                                            .Text(consultaDiagnostico1?.DiagnosisDefinitive == true ? "X" : "").FontSize(9).AlignCenter();

                                        nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ccffcc").MinHeight(12).MinWidth(3)
                                            .Text(rowIndex.ToString()).FontSize(9).AlignCenter();

                                        if (diagnostico2 != null)
                                        {
                                            // Columna 6: Diagnóstico 2 (si existe)
                                            nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ffffff").MinHeight(12)
                                                .Text(diagnostico2.DiagnosisName).FontSize(9).AlignCenter();

                                            // Columna 7: CIE10 Diagnóstico 2
                                            nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ffffff").MinHeight(12)
                                                .Text(diagnostico2.DiagnosisCie10.ToString()).FontSize(9).AlignCenter();

                                            // Columna 8: Presuntivo Diagnóstico 2
                                            nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(12)
                                                .Text(consultaDiagnostico2?.DiagnosisPresumptive == true ? "X" : "").FontSize(9).AlignCenter();

                                            // Columna 9: Definitivo Diagnóstico 2
                                            nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(12)
                                                .Text(consultaDiagnostico2?.DiagnosisDefinitive == true ? "X" : "").FontSize(9).AlignCenter();
                                        }
                                        else
                                        {
                                            // Si no hay un segundo diagnóstico, llenar las celdas con espacios vacíos
                                            for (int j = 0; j < 4; j++)
                                            {
                                                nestedTable.Cell().Border(1).BorderColor("#ffffff").Background("#ffffff").MinHeight(12).Text("");
                                            }
                                        }

                                        rowIndex++;
                                    }


                                });
                            });
                        });


                        //Novena TABLA

                        contentColumn.Item().PaddingTop(7).Table(table =>
                        {
                            // Definir las columnas de la tabla principal con dos columnas
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(278);  // Columna 1
                                columns.ConstantColumn(278);  // Columna 2
                            });

                            // Fila de encabezado "9 PLANES DE TRATAMIENTO"
                            table.Cell().MinHeight(14).BorderLeft(2).BorderTop(2).BorderBottom(2).BorderColor("#808080").Element(CellStyle =>
                                CellStyle.Background("#ccccff")).AlignLeft().PaddingTop(3).PaddingLeft(3).Text("9 PLANES DE TRATAMIENTO ").FontSize(10).Bold();

                            // Segunda columna con la descripción
                            table.Cell().MinHeight(14).BorderTop(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Element(CellStyle =>
                                CellStyle.Background("#ccccff")).AlignRight().PaddingTop(3).Text("REGISTRAR LOS PLANES: DIAGNOSTICO, TERAPÉUTICO Y\r\nEDUCACIONAL").FontSize(7);

                            // Subtabla debajo del encabezado que abarca todo el ancho
                            table.Cell().ColumnSpan(2).Element(CellStyle =>
                            {
                                // Crear una subtabla con una columna y cuatro filas de manera estática
                                CellStyle.Background("#ffffff").BorderLeft(2).BorderRight(2).BorderBottom(2).BorderColor("#808080").Table(nestedTable =>
                                {
                                    // Definir una sola columna en la subtabla
                                    nestedTable.ColumnsDefinition(columns =>
                                    {
                                        columns.RelativeColumn(1);  // Una columna que abarca todo el ancho
                                    });

                                    // Primera fila
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ffffff").MinHeight(20).MinWidth(3).PaddingTop(3)
                                        .Text(consulta.Consultation.ConsultationTreatmentplan).FontSize(9).AlignLeft();

                                    // Segunda fila
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ffffff").MinHeight(20).MinWidth(3)
                                        .Text(" ").FontSize(9).AlignLeft();

                                    // Tercera fila
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ffffff").MinHeight(20).MinWidth(3)
                                        .Text("").FontSize(9).AlignLeft();

                                    // Cuarta fila
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ffffff").MinHeight(20).MinWidth(3)
                                        .Text("").FontSize(9).AlignLeft();
                                });
                            });
                        });

                        contentColumn.Item().PaddingTop(50).Table(table =>
                        {
                            // Definir las columnas de la tabla principal con medidas específicas en puntos (pt)
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(54);  // Columna 1 (FECHA)
                                columns.ConstantColumn(57);  // Columna 2 (Valor de FECHA)
                                columns.ConstantColumn(30);  // Columna 3 (HORA)
                                columns.ConstantColumn(54);  // Columna 4 (Valor de HORA)
                                columns.ConstantColumn(57);  // Columna 5 (NOMBRE DEL PROFESIONAL)
                                columns.ConstantColumn(100); // Columna 6 (Valor del NOMBRE)
                                columns.ConstantColumn(57);  // Columna 7 (Número del Profesional)
                                columns.ConstantColumn(50);  // Columna 8 (FIRMA)
                                columns.ConstantColumn(40);  // Columna 9 (Campo vacío para FIRMA)
                                columns.ConstantColumn(30);  // Columna 10 (HOJA)
                                columns.ConstantColumn(22);  // Columna 11 (Valor de HOJA)
                            });

                            // Fila con las celdas del content que abarcan el ancho completo de la página
                            table.Cell().Element(CellStyle => CellStyle.Background("#ccffcc").Border(1)).AlignCenter().Text("FECHA").FontSize(8);
                            table.Cell().Element(CellStyle => CellStyle.Background("#FFFFFF").Border(1)).AlignCenter().Text(DateTime.Now.ToString("dd/MM/yyyy")).FontSize(8);
                            table.Cell().Element(CellStyle => CellStyle.Background("#ccffcc").Border(1)).AlignCenter().Text("HORA").FontSize(8);
                            table.Cell()
          .Background("#ffffff")
          .Border(1)
          .AlignCenter()
          .Text(DateTime.Now.ToString("HH:mm"))  // Formato de hora en 24 horas
          .FontSize(8);

                            table.Cell().Element(CellStyle => CellStyle.Background("#ccffcc").Border(1)).AlignCenter().Text("NOMBRE DEL\r\nPROFESIONAL").FontSize(7);
                            table.Cell().Element(CellStyle => CellStyle.Background("#ffffff").Border(1)).AlignCenter().Text(consulta.Consultation.UsersNames + consulta.Consultation.UsersSurcenames).FontSize(6);
                            table.Cell().Element(CellStyle => CellStyle.Background("#ffffff").Border(1)).AlignCenter().Text("sd").FontSize(8);
                            table.Cell().Element(CellStyle => CellStyle.Background("#ccffcc").Border(1)).AlignCenter().Text("FIRMA").FontSize(8);
                            table.Cell().Element(CellStyle => CellStyle.Background("#ffffff").Border(1)).AlignCenter().Text("").FontSize(8);
                            table.Cell().Element(CellStyle => CellStyle.Background("#ccffcc").Border(1)).AlignCenter().Text("NUMERO DE HOJA").FontSize(4);
                            table.Cell().Element(CellStyle => CellStyle.Background("#ffffff").Border(1)).AlignCenter().Text("1").FontSize(8);
                        });



                    });

                    // Footer de la página
                    // Footer de la página
                    page.Footer().Height(20).PaddingHorizontal(2).Row(row =>
                    {
                        // Texto a la izquierda
                        row.RelativeItem().AlignLeft().Text(text =>
                        {
                            text.Span("SNS-MSP / HCU-form.002 / 2008")
                                .FontSize(7)
                                .Bold();
                        });

                        // Texto a la derecha
                        row.RelativeItem().AlignRight().Text(text =>
                        {
                            text.Span("CONSULTA EXTERNA - ANAMNESIS Y EXAMEN FÍSICO")
                                .FontSize(9)
                                .Bold();
                        });
                    });






                });
                // Segunda página

                container.Page(page =>
                {
                    page.Margin(20);
                    page.Size(598, 845);
                    page.DefaultTextStyle(x => x.FontFamily("Arial").FontSize(10));

                    // Contenido de la segunda página con una sola tabla de 5 columnas
                    page.Content().Column(column =>
                    {
                        // Primera tabla de cinco columnas
                        column.Item().Table(table =>
                        {
                            // Definir las columnas de la tabla principal con cinco columnas
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(136);  // Columna 1
                                columns.ConstantColumn(136);  // Columna 2
                                columns.ConstantColumn(10);   // Columna 3 (espaciador)
                                columns.ConstantColumn(136);  // Columna 4
                                columns.ConstantColumn(136);  // Columna 5
                            });

                            // Fila de datos
                            table.Cell().MinHeight(15).BorderLeft(2).BorderTop(2).BorderBottom(2).BorderColor("#808080").Background("#ccccff")
                                .Text("10 EVOLUCIÓN").FontSize(10).AlignLeft().Bold();

                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Background("#ccccff")
                                .Text("FIRMAR AL PIE DE CADA NOTA").FontSize(7).AlignEnd();

                            table.Cell().MinHeight(20).BorderColor("#808080").Background("#ffffff")
                                .Text("").FontSize(9).AlignLeft().Bold();

                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderLeft(2).BorderColor("#808080").Background("#ccccff")
                                .Text("11 PRESCRIPCIONES").FontSize(9).AlignLeft().Bold();

                            table.Cell().MinHeight(15).BorderRight(2).BorderBottom(2).BorderTop(2).BorderColor("#808080").Background("#ccccff")
                                .Text("FIRMAR AL PIE DE CADA PRESCRIPCIÓN").FontSize(5).AlignRight();
                        });

                        // Espacio entre tablas
                        column.Item().Text("REGISTRAR EN ROJO LA ADMINISTRACIÓN DE FÁRMACOS Y OTROS PRODUCTOS (ENFERMERÍA)").FontSize(8).Light().AlignEnd();

                        // Primera tabla de cinco columnas
                        column.Item().Table(table =>
                        {
                            // Definir las columnas de la tabla principal con cinco columnas
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(60);  // Columna 1
                                columns.ConstantColumn(30);  // Columna 2
                                columns.ConstantColumn(182);  // Columna 2
                                columns.ConstantColumn(13);   // Columna 3 (espaciador)
                                columns.ConstantColumn(220);  // Columna 1
                                columns.ConstantColumn(50);  // Columna 2

                            });

                            // Fila de datos
                            table.Cell().MinHeight(15).BorderLeft(2).BorderRight(1).BorderTop(2).BorderBottom(2).BorderColor("#808080").Background("#ccffcc")
                                .Text("\nFECHA\r\n(DIA/MES/AÑO)").FontSize(8).AlignCenter();

                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Background("#ccffcc")
                                .Text("\nHORA").FontSize(7).AlignCenter();
                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Background("#ccffcc")
                                .Text("\nNOTAS DE EVOLUCIÓN").FontSize(7).AlignCenter().Bold();

                            table.Cell().MinHeight(15).BorderColor("#808080").BorderLeft(2).BorderRight(2).Background("#ffffff")
                                .Text("").FontSize(9).AlignLeft().Bold();

                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderLeft(2).BorderRight(1).BorderColor("#808080").Background("#ccffcc")
                                .Text("FARMACOTERAPIA E INDICACIONES\r\n(PARA ENFERMERÍA Y OTRO PERSONAL)").FontSize(7).AlignCenter();

                            table.Cell().MinHeight(20).BorderRight(2).BorderBottom(2).BorderTop(2).BorderColor("#808080").Background("#ccffcc")
                                .Text("ADMINISTR.\r\nFÁRMACOS\r\nY OTROS").FontSize(7).AlignCenter();

                            //Notas abajo
                            table.Cell().MinHeight(15).BorderLeft(2).BorderRight(1).BorderTop(2).BorderBottom(2).BorderColor("#808080").Background("#FFFFFF")
                             .Text("").FontSize(8).AlignCenter();

                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();
                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter().Bold();

                            table.Cell().MinHeight(15).BorderColor("#808080").BorderLeft(2).BorderRight(2).Background("#ffffff")
                                .Text("").FontSize(9).AlignLeft().Bold();

                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderLeft(2).BorderRight(1).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();

                            table.Cell().MinHeight(20).BorderRight(2).BorderBottom(2).BorderTop(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();
                            table.Cell().MinHeight(15).BorderLeft(2).BorderRight(1).BorderTop(2).BorderBottom(2).BorderColor("#808080").Background("#FFFFFF")
                          .Text("").FontSize(8).AlignCenter();

                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();
                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter().Bold();

                            table.Cell().MinHeight(15).BorderColor("#808080").BorderLeft(2).BorderRight(2).Background("#ffffff")
                                .Text("").FontSize(9).AlignLeft().Bold();

                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderLeft(2).BorderRight(1).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();

                            table.Cell().MinHeight(20).BorderRight(2).BorderBottom(2).BorderTop(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();
                            table.Cell().MinHeight(15).BorderLeft(2).BorderRight(1).BorderTop(2).BorderBottom(2).BorderColor("#808080").Background("#FFFFFF")
                          .Text("").FontSize(8).AlignCenter();

                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();
                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter().Bold();

                            table.Cell().MinHeight(15).BorderColor("#808080").BorderLeft(2).BorderRight(2).Background("#ffffff")
                                .Text("").FontSize(9).AlignLeft().Bold();

                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderLeft(2).BorderRight(1).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();

                            table.Cell().MinHeight(20).BorderRight(2).BorderBottom(2).BorderTop(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();
                            table.Cell().MinHeight(15).BorderLeft(2).BorderRight(1).BorderTop(2).BorderBottom(2).BorderColor("#808080").Background("#FFFFFF")
                          .Text("").FontSize(8).AlignCenter();

                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();
                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter().Bold();

                            table.Cell().MinHeight(15).BorderColor("#808080").BorderLeft(2).BorderRight(2).Background("#ffffff")
                                .Text("").FontSize(9).AlignLeft().Bold();

                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderLeft(2).BorderRight(1).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();

                            table.Cell().MinHeight(20).BorderRight(2).BorderBottom(2).BorderTop(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();
                            table.Cell().MinHeight(15).BorderLeft(2).BorderRight(1).BorderTop(2).BorderBottom(2).BorderColor("#808080").Background("#FFFFFF")
                          .Text("").FontSize(8).AlignCenter();

                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();
                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter().Bold();

                            table.Cell().MinHeight(15).BorderColor("#808080").BorderLeft(2).BorderRight(2).Background("#ffffff")
                                .Text("").FontSize(9).AlignLeft().Bold();

                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderLeft(2).BorderRight(1).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();

                            table.Cell().MinHeight(20).BorderRight(2).BorderBottom(2).BorderTop(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();
                            table.Cell().MinHeight(15).BorderLeft(2).BorderRight(1).BorderTop(2).BorderBottom(2).BorderColor("#808080").Background("#FFFFFF")
                          .Text("").FontSize(8).AlignCenter();

                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();
                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter().Bold();

                            table.Cell().MinHeight(15).BorderColor("#808080").BorderLeft(2).BorderRight(2).Background("#ffffff")
                                .Text("").FontSize(9).AlignLeft().Bold();

                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderLeft(2).BorderRight(1).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();

                            table.Cell().MinHeight(20).BorderRight(2).BorderBottom(2).BorderTop(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();
                            table.Cell().MinHeight(15).BorderLeft(2).BorderRight(1).BorderTop(2).BorderBottom(2).BorderColor("#808080").Background("#FFFFFF")
                          .Text("").FontSize(8).AlignCenter();

                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();
                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter().Bold();

                            table.Cell().MinHeight(15).BorderColor("#808080").BorderLeft(2).BorderRight(2).Background("#ffffff")
                                .Text("").FontSize(9).AlignLeft().Bold();

                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderLeft(2).BorderRight(1).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();

                            table.Cell().MinHeight(20).BorderRight(2).BorderBottom(2).BorderTop(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();
                            table.Cell().MinHeight(15).BorderLeft(2).BorderRight(1).BorderTop(2).BorderBottom(2).BorderColor("#808080").Background("#FFFFFF")
                          .Text("").FontSize(8).AlignCenter();

                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();
                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter().Bold();

                            table.Cell().MinHeight(15).BorderColor("#808080").BorderLeft(2).BorderRight(2).Background("#ffffff")
                                .Text("").FontSize(9).AlignLeft().Bold();

                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderLeft(2).BorderRight(1).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();

                            table.Cell().MinHeight(20).BorderRight(2).BorderBottom(2).BorderTop(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();
                            table.Cell().MinHeight(15).BorderLeft(2).BorderRight(1).BorderTop(2).BorderBottom(2).BorderColor("#808080").Background("#FFFFFF")
                          .Text("").FontSize(8).AlignCenter();

                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();
                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter().Bold();

                            table.Cell().MinHeight(15).BorderColor("#808080").BorderLeft(2).BorderRight(2).Background("#ffffff")
                                .Text("").FontSize(9).AlignLeft().Bold();

                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderLeft(2).BorderRight(1).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();

                            table.Cell().MinHeight(20).BorderRight(2).BorderBottom(2).BorderTop(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();
                            table.Cell().MinHeight(15).BorderLeft(2).BorderRight(1).BorderTop(2).BorderBottom(2).BorderColor("#808080").Background("#FFFFFF")
                          .Text("").FontSize(8).AlignCenter();

                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();
                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter().Bold();

                            table.Cell().MinHeight(15).BorderColor("#808080").BorderLeft(2).BorderRight(2).Background("#ffffff")
                                .Text("").FontSize(9).AlignLeft().Bold();

                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderLeft(2).BorderRight(1).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();

                            table.Cell().MinHeight(20).BorderRight(2).BorderBottom(2).BorderTop(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();
                            table.Cell().MinHeight(15).BorderLeft(2).BorderRight(1).BorderTop(2).BorderBottom(2).BorderColor("#808080").Background("#FFFFFF")
                          .Text("").FontSize(8).AlignCenter();

                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();
                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter().Bold();

                            table.Cell().MinHeight(15).BorderColor("#808080").BorderLeft(2).BorderRight(2).Background("#ffffff")
                                .Text("").FontSize(9).AlignLeft().Bold();

                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderLeft(2).BorderRight(1).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();

                            table.Cell().MinHeight(20).BorderRight(2).BorderBottom(2).BorderTop(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();
                            table.Cell().MinHeight(15).BorderLeft(2).BorderRight(1).BorderTop(2).BorderBottom(2).BorderColor("#808080").Background("#FFFFFF")
                          .Text("").FontSize(8).AlignCenter();

                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();
                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter().Bold();

                            table.Cell().MinHeight(15).BorderColor("#808080").BorderLeft(2).BorderRight(2).Background("#ffffff")
                                .Text("").FontSize(9).AlignLeft().Bold();

                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderLeft(2).BorderRight(1).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();

                            table.Cell().MinHeight(20).BorderRight(2).BorderBottom(2).BorderTop(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();
                            table.Cell().MinHeight(15).BorderLeft(2).BorderRight(1).BorderTop(2).BorderBottom(2).BorderColor("#808080").Background("#FFFFFF")
                          .Text("").FontSize(8).AlignCenter();

                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();
                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter().Bold();

                            table.Cell().MinHeight(15).BorderColor("#808080").BorderLeft(2).BorderRight(2).Background("#ffffff")
                                .Text("").FontSize(9).AlignLeft().Bold();

                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderLeft(2).BorderRight(1).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();

                            table.Cell().MinHeight(20).BorderRight(2).BorderBottom(2).BorderTop(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();
                            table.Cell().MinHeight(15).BorderLeft(2).BorderRight(1).BorderTop(2).BorderBottom(2).BorderColor("#808080").Background("#FFFFFF")
                          .Text("").FontSize(8).AlignCenter();

                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();
                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter().Bold();

                            table.Cell().MinHeight(15).BorderColor("#808080").BorderLeft(2).BorderRight(2).Background("#ffffff")
                                .Text("").FontSize(9).AlignLeft().Bold();

                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderLeft(2).BorderRight(1).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();

                            table.Cell().MinHeight(20).BorderRight(2).BorderBottom(2).BorderTop(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();
                            table.Cell().MinHeight(15).BorderLeft(2).BorderRight(1).BorderTop(2).BorderBottom(2).BorderColor("#808080").Background("#FFFFFF")
                          .Text("").FontSize(8).AlignCenter();

                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();
                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter().Bold();

                            table.Cell().MinHeight(15).BorderColor("#808080").BorderLeft(2).BorderRight(2).Background("#ffffff")
                                .Text("").FontSize(9).AlignLeft().Bold();

                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderLeft(2).BorderRight(1).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();

                            table.Cell().MinHeight(20).BorderRight(2).BorderBottom(2).BorderTop(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();
                            table.Cell().MinHeight(15).BorderLeft(2).BorderRight(1).BorderTop(2).BorderBottom(2).BorderColor("#808080").Background("#FFFFFF")
                          .Text("").FontSize(8).AlignCenter();

                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();
                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter().Bold();

                            table.Cell().MinHeight(15).BorderColor("#808080").BorderLeft(2).BorderRight(2).Background("#ffffff")
                                .Text("").FontSize(9).AlignLeft().Bold();

                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderLeft(2).BorderRight(1).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();

                            table.Cell().MinHeight(20).BorderRight(2).BorderBottom(2).BorderTop(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();
                            table.Cell().MinHeight(15).BorderLeft(2).BorderRight(1).BorderTop(2).BorderBottom(2).BorderColor("#808080").Background("#FFFFFF")
                          .Text("").FontSize(8).AlignCenter();

                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();
                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter().Bold();

                            table.Cell().MinHeight(15).BorderColor("#808080").BorderLeft(2).BorderRight(2).Background("#ffffff")
                                .Text("").FontSize(9).AlignLeft().Bold();

                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderLeft(2).BorderRight(1).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();

                            table.Cell().MinHeight(20).BorderRight(2).BorderBottom(2).BorderTop(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();
                            table.Cell().MinHeight(15).BorderLeft(2).BorderRight(1).BorderTop(2).BorderBottom(2).BorderColor("#808080").Background("#FFFFFF")
                       .Text("").FontSize(8).AlignCenter();

                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();
                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter().Bold();

                            table.Cell().MinHeight(15).BorderColor("#808080").BorderLeft(2).BorderRight(2).Background("#ffffff")
                                .Text("").FontSize(9).AlignLeft().Bold();

                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderLeft(2).BorderRight(1).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();

                            table.Cell().MinHeight(20).BorderRight(2).BorderBottom(2).BorderTop(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();
                            table.Cell().MinHeight(15).BorderLeft(2).BorderRight(1).BorderTop(2).BorderBottom(2).BorderColor("#808080").Background("#FFFFFF")
                          .Text("").FontSize(8).AlignCenter();

                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();
                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter().Bold();

                            table.Cell().MinHeight(15).BorderColor("#808080").BorderLeft(2).BorderRight(2).Background("#ffffff")
                                .Text("").FontSize(9).AlignLeft().Bold();

                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderLeft(2).BorderRight(1).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();

                            table.Cell().MinHeight(20).BorderRight(2).BorderBottom(2).BorderTop(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();
                            table.Cell().MinHeight(15).BorderLeft(2).BorderRight(1).BorderTop(2).BorderBottom(2).BorderColor("#808080").Background("#FFFFFF")
                          .Text("").FontSize(8).AlignCenter();

                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();
                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter().Bold();

                            table.Cell().MinHeight(15).BorderColor("#808080").BorderLeft(2).BorderRight(2).Background("#ffffff")
                                .Text("").FontSize(9).AlignLeft().Bold();

                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderLeft(2).BorderRight(1).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();

                            table.Cell().MinHeight(20).BorderRight(2).BorderBottom(2).BorderTop(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();
                            table.Cell().MinHeight(15).BorderLeft(2).BorderRight(1).BorderTop(2).BorderBottom(2).BorderColor("#808080").Background("#FFFFFF")
                          .Text("").FontSize(8).AlignCenter();

                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();
                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter().Bold();

                            table.Cell().MinHeight(15).BorderColor("#808080").BorderLeft(2).BorderRight(2).Background("#ffffff")
                                .Text("").FontSize(9).AlignLeft().Bold();

                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderLeft(2).BorderRight(1).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();

                            table.Cell().MinHeight(20).BorderRight(2).BorderBottom(2).BorderTop(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();
                            table.Cell().MinHeight(15).BorderLeft(2).BorderRight(1).BorderTop(2).BorderBottom(2).BorderColor("#808080").Background("#FFFFFF")
                          .Text("").FontSize(8).AlignCenter();

                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();
                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter().Bold();

                            table.Cell().MinHeight(15).BorderColor("#808080").BorderLeft(2).BorderRight(2).Background("#ffffff")
                                .Text("").FontSize(9).AlignLeft().Bold();

                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderLeft(2).BorderRight(1).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();

                            table.Cell().MinHeight(20).BorderRight(2).BorderBottom(2).BorderTop(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();
                            table.Cell().MinHeight(15).BorderLeft(2).BorderRight(1).BorderTop(2).BorderBottom(2).BorderColor("#808080").Background("#FFFFFF")
                          .Text("").FontSize(8).AlignCenter();

                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();
                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter().Bold();

                            table.Cell().MinHeight(15).BorderColor("#808080").BorderLeft(2).BorderRight(2).Background("#ffffff")
                                .Text("").FontSize(9).AlignLeft().Bold();

                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderLeft(2).BorderRight(1).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();

                            table.Cell().MinHeight(20).BorderRight(2).BorderBottom(2).BorderTop(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();
                            table.Cell().MinHeight(15).BorderLeft(2).BorderRight(1).BorderTop(2).BorderBottom(2).BorderColor("#808080").Background("#FFFFFF")
                          .Text("").FontSize(8).AlignCenter();

                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();
                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter().Bold();

                            table.Cell().MinHeight(15).BorderColor("#808080").BorderLeft(2).BorderRight(2).Background("#ffffff")
                                .Text("").FontSize(9).AlignLeft().Bold();

                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderLeft(2).BorderRight(1).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();

                            table.Cell().MinHeight(20).BorderRight(2).BorderBottom(2).BorderTop(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();
                            table.Cell().MinHeight(15).BorderLeft(2).BorderRight(1).BorderTop(2).BorderBottom(2).BorderColor("#808080").Background("#FFFFFF")
                          .Text("").FontSize(8).AlignCenter();

                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();
                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter().Bold();

                            table.Cell().MinHeight(15).BorderColor("#808080").BorderLeft(2).BorderRight(2).Background("#ffffff")
                                .Text("").FontSize(9).AlignLeft().Bold();

                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderLeft(2).BorderRight(1).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();

                            table.Cell().MinHeight(20).BorderRight(2).BorderBottom(2).BorderTop(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();
                            table.Cell().MinHeight(15).BorderLeft(2).BorderRight(1).BorderTop(2).BorderBottom(2).BorderColor("#808080").Background("#FFFFFF")
                          .Text("").FontSize(8).AlignCenter();

                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();
                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter().Bold();

                            table.Cell().MinHeight(15).BorderColor("#808080").BorderLeft(2).BorderRight(2).Background("#ffffff")
                                .Text("").FontSize(9).AlignLeft().Bold();

                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderLeft(2).BorderRight(1).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();

                            table.Cell().MinHeight(20).BorderRight(2).BorderBottom(2).BorderTop(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();
                            table.Cell().MinHeight(15).BorderLeft(2).BorderRight(1).BorderTop(2).BorderBottom(2).BorderColor("#808080").Background("#FFFFFF")
                          .Text("").FontSize(8).AlignCenter();

                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();
                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter().Bold();

                            table.Cell().MinHeight(15).BorderColor("#808080").BorderLeft(2).BorderRight(2).Background("#ffffff")
                                .Text("").FontSize(9).AlignLeft().Bold();

                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderLeft(2).BorderRight(1).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();

                            table.Cell().MinHeight(20).BorderRight(2).BorderBottom(2).BorderTop(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();
                            table.Cell().MinHeight(15).BorderLeft(2).BorderRight(1).BorderTop(2).BorderBottom(2).BorderColor("#808080").Background("#FFFFFF")
                          .Text("").FontSize(8).AlignCenter();

                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();
                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter().Bold();

                            table.Cell().MinHeight(15).BorderColor("#808080").BorderLeft(2).BorderRight(2).Background("#ffffff")
                                .Text("").FontSize(9).AlignLeft().Bold();

                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderLeft(2).BorderRight(1).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();

                            table.Cell().MinHeight(20).BorderRight(2).BorderBottom(2).BorderTop(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();
                            table.Cell().MinHeight(15).BorderLeft(2).BorderRight(1).BorderTop(2).BorderBottom(2).BorderColor("#808080").Background("#FFFFFF")
                          .Text("").FontSize(8).AlignCenter();

                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();
                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter().Bold();

                            table.Cell().MinHeight(15).BorderColor("#808080").BorderLeft(2).BorderRight(2).Background("#ffffff")
                                .Text("").FontSize(9).AlignLeft().Bold();

                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderLeft(2).BorderRight(1).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();

                            table.Cell().MinHeight(20).BorderRight(2).BorderBottom(2).BorderTop(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();
                            table.Cell().MinHeight(15).BorderLeft(2).BorderRight(1).BorderTop(2).BorderBottom(2).BorderColor("#808080").Background("#FFFFFF")
                          .Text("").FontSize(8).AlignCenter();

                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();
                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter().Bold();

                            table.Cell().MinHeight(15).BorderColor("#808080").BorderLeft(2).BorderRight(2).Background("#ffffff")
                                .Text("").FontSize(9).AlignLeft().Bold();

                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderLeft(2).BorderRight(1).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();

                            table.Cell().MinHeight(20).BorderRight(2).BorderBottom(2).BorderTop(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();
                            table.Cell().MinHeight(15).BorderLeft(2).BorderRight(1).BorderTop(2).BorderBottom(2).BorderColor("#808080").Background("#FFFFFF")
                          .Text("").FontSize(8).AlignCenter();

                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();
                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter().Bold();

                            table.Cell().MinHeight(15).BorderColor("#808080").BorderLeft(2).BorderRight(2).Background("#ffffff")
                                .Text("").FontSize(9).AlignLeft().Bold();

                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderLeft(2).BorderRight(1).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();

                            table.Cell().MinHeight(20).BorderRight(2).BorderBottom(2).BorderTop(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();
                            table.Cell().MinHeight(15).BorderLeft(2).BorderRight(1).BorderTop(2).BorderBottom(2).BorderColor("#808080").Background("#FFFFFF")
                          .Text("").FontSize(8).AlignCenter();

                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();
                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter().Bold();

                            table.Cell().MinHeight(15).BorderColor("#808080").BorderLeft(2).BorderRight(2).Background("#ffffff")
                                .Text("").FontSize(9).AlignLeft().Bold();

                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderLeft(2).BorderRight(1).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();

                            table.Cell().MinHeight(20).BorderRight(2).BorderBottom(2).BorderTop(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();
                            table.Cell().MinHeight(15).BorderLeft(2).BorderRight(1).BorderTop(2).BorderBottom(2).BorderColor("#808080").Background("#FFFFFF")
                          .Text("").FontSize(8).AlignCenter();

                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();
                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter().Bold();

                            table.Cell().MinHeight(15).BorderColor("#808080").BorderLeft(2).BorderRight(2).Background("#ffffff")
                                .Text("").FontSize(9).AlignLeft().Bold();

                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderLeft(2).BorderRight(1).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();

                            table.Cell().MinHeight(20).BorderRight(2).BorderBottom(2).BorderTop(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();
                            table.Cell().MinHeight(15).BorderLeft(2).BorderRight(1).BorderTop(2).BorderBottom(2).BorderColor("#808080").Background("#FFFFFF")
                          .Text("").FontSize(8).AlignCenter();

                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();
                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter().Bold();

                            table.Cell().MinHeight(15).BorderColor("#808080").BorderLeft(2).BorderRight(2).Background("#ffffff")
                                .Text("").FontSize(9).AlignLeft().Bold();

                            table.Cell().MinHeight(15).BorderTop(2).BorderBottom(2).BorderLeft(2).BorderRight(1).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();

                            table.Cell().MinHeight(20).BorderRight(2).BorderBottom(2).BorderTop(2).BorderColor("#808080").Background("#FFFFFF")
                                .Text("").FontSize(7).AlignCenter();
                        });

                    });




                    // Footer de la segunda página
                    page.Footer().Height(20).PaddingHorizontal(2).Row(row =>
                    {
                        row.RelativeItem().AlignLeft().Text(text =>
                        {
                            text.Span("SNS-MSP / HCU-form.002 / 2008").FontSize(7).Bold();
                        });

                        row.RelativeItem().AlignRight().Text(text =>
                        {
                            text.Span("CONSULTA EXTERNA - ANAMNESIS Y EXAMEN FÍSICO").FontSize(9).Bold();
                        });
                    });
                });


            });

            byte[] pdfBytes = document.GeneratePdf();

            // Devuelve el archivo PDF al navegador.
            return File(pdfBytes, "application/pdf", "MedicationRecipe.pdf");
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

        public async Task<IActionResult> ImageDoc(int consultationId)
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
