using ExpertMed.Services;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;

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
        public DocumentsController(UserService usersService, ILogger<DocumentsController> logger, SelectsService selectService, PatientService patientService,ConsultationService consultationService)
        {
            _usersService = usersService;
            _logger = logger;
            _selectService = selectService;
            _patientService = patientService;
            _consultationService = consultationService;
        }

       
        public IActionResult MedicalCertificate()
        {
            return new ViewAsPdf("MedicalCertificate")
            {
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait

            };
        }  
        public IActionResult MedicalForm()
        {
            return new ViewAsPdf("MedicalForm")
            {
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
            };
        }

        public IActionResult MedicationRecipe()
        {
            return new ViewAsPdf("MedicationRecipe")
            {
                PageSize = Rotativa.AspNetCore.Options.Size.A5,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape,
            };
        }
        public IActionResult LaboratoryDoc()
        {
            return new ViewAsPdf("LaboratoryDoc")
            {
                PageSize = Rotativa.AspNetCore.Options.Size.A5,
            };
        }

        public IActionResult ImageDoc()
        {
            return new ViewAsPdf("ImageDoc")
            {
                PageSize = Rotativa.AspNetCore.Options.Size.A5,
            };
        }
    }
}
