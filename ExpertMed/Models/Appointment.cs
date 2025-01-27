using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpertMed.Models
{
    public partial class Appointment
    {
        public int AppointmentId { get; set; }

        public DateTime? AppointmentCreatedate { get; set; }

        public DateTime? AppointmentModifydate { get; set; }

        public int? AppointmentCreateuser { get; set; }

        public int? AppointmentModifyuser { get; set; }

        public DateTime AppointmentDate { get; set; }

        public TimeOnly AppointmentHour { get; set; }

        public int? AppointmentPatientid { get; set; }

        public int? AppointmentConsultationid { get; set; }

        public int? AppointmentStatus { get; set; }

        // Propiedades añadidas para los nombres completos
        public string PatientName { get; set; }  // Nombre completo del paciente
        public string DoctorName { get; set; }   // Nombre completo del doctor


        // Relacionados con el paciente, doctor y usuario
        public virtual Consultation? AppointmentConsultation { get; set; }

        public virtual User? AppointmentCreateuserNavigation { get; set; }

        public virtual User? AppointmentModifyuserNavigation { get; set; }

        public virtual Patient? AppointmentPatient { get; set; }

        public virtual ICollection<AssistantDoctorAppointment> AssistantDoctorAppointments { get; set; } = new List<AssistantDoctorAppointment>();
    }


}
