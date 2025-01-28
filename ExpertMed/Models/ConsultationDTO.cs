namespace ExpertMed.Models
{
    public class ConsultationDTO
    {

        public DateTime ConsultationCreationDate { get; set; }
        public int ConsultationUserCreate { get; set; }
        public int ConsultationPatient { get; set; }
        public int ConsultationSpeciality { get; set; }
        public string ConsultationHistoryClinic { get; set; }
        public string ConsultationReason { get; set; }
        public string ConsultationDisease { get; set; }
        public string ConsultationFamiliaryName { get; set; }
        public string ConsultationWarningSigns { get; set; }
        public string ConsultationNonPharmacological { get; set; }
        public int ConsultationFamiliaryType { get; set; }
        public string ConsultationFamiliaryPhone { get; set; }
        public string ConsultationTemperature { get; set; }
        public string ConsultationRespirationRate { get; set; }
        public string ConsultationBloodPressureAS { get; set; }
        public string ConsultationBloodPressureDIS { get; set; }
        public string ConsultationPulse { get; set; }
        public string ConsultationWeight { get; set; }
        public string ConsultationSize { get; set; }
        public string ConsultationTreatmentPlan { get; set; }
        public string ConsultationObservation { get; set; }
        public string ConsultationPersonalBackground { get; set; }
        public int ConsultationDisabilityDays { get; set; }
        public int ConsultationType { get; set; }
        public int ConsultationStatus { get; set; }

        public string DiagnosticsJson { get; set; }
        public string AllergiesJson { get; set; }
        public string ImagesJson { get; set; }
        public string LaboratoriesJson { get; set; }
        public string MedicationsJson { get; set; }
        public string SurgeriesJson { get; set; }
        public string FamiliaryBackgroundJson { get; set; }
        public string OrgansSystemsJson { get; set; }
        public string PhysicalExaminationJson { get; set; }
    }
}
