using ExpertMed.Migrations;
using ExpertMed.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ExpertMed.Services
{
    public class ConsultationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<ConsultationService> _logger;
        private readonly DbExpertmedContext _dbContext;

        public ConsultationService(IHttpContextAccessor httpContextAccessor, ILogger<ConsultationService> logger, DbExpertmedContext dbContext)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }


        public async Task<List<Consultation>> GetConsultationsAsync(int userId, int profileId)
        {
            try
            {
                var consultations = await _dbContext.Consultations
                    .FromSqlRaw("EXEC sp_ListAllConsultation @user_id = {0}, @profile_id = {1}", userId, profileId)
                    .ToListAsync();
                // Cargar relaciones necesarias explícitamente
                foreach (var consultation in consultations)
                {
                    // Esto es útil si necesitas cargar relaciones adicionales como PatientNationalityNavigation o PatientCreationuserNavigation
                    // Usamos 'LoadAsync' solo cuando sea necesario, pero si tienes muchas relaciones o muchos pacientes, puede afectar el rendimiento
                    await _dbContext.Entry(consultation)
                        .Reference(p => p.ConsultationPatientNavigation)
                        .LoadAsync();

                    await _dbContext.Entry(consultation)
                        .Reference(p => p.ConsultationUsercreateNavigation)
                        .LoadAsync();

                    await _dbContext.Entry(consultation)
                        .Reference(p => p.ConsultationSpecialityNavigation)
                        .LoadAsync();
                }
                return consultations;
            }
            catch (Exception ex)
            {
                // Manejo de errores (log, excepción personalizada, etc.)
                throw new ApplicationException("Error al obtener las consultas", ex);
            }
        }



        public async Task CrearConsultaAsync(
         DateTime consultation_creationdate,
         int? consultation_usercreate,
         int consultation_sequential,
         int consultation_patient,
         int consultation_speciality,
         string consultation_historyclinic,
         string consultation_reason,
         string consultation_disease,
         string consultation_familiaryname,
         string consultation_warningsings,
         string consultation_nonpharmacologycal,
         int consultation_familiarytype,
         string consultation_familiaryphone,
         string consultation_temperature,
         string consultation_respirationrate,
         string consultation_bloodpressuredAS,
         string consultation_bloodpresuredDIS,
         string consultation_pulse,
         string consultation_weight,
         string consultation_size,
         string consultation_treatmentplan,
         string consultation_observation,
         string consultation_personalbackground,
         int consultation_disablilitydays,
         int consultation_type,
         int consultation_status,
         // Parámetros para órganos y sistemas
         bool? organssystems_organsenses,
         string organssystems_organsenses_Obs,
         bool? organssystems_respiratory,
         string organssystems_respiratory_obs,
         bool? organssystems_cardiovascular,
         string organssystems_cardiovascular_obs,
         bool? organssystems_digestive,
         string organssystems_digestive_obs,
         bool? organssystems_genital,
         string organssystems_genital_obs,
         bool? organssystems_urinary,
         string organssystems_urinary_obs,
         bool? organssystems_skeletal_m,
         string organssystems_skeletal_m_obs,
         bool? organssystems_endrocrine,
         string organssystems_endocrine,
         bool? organssystems_lymphatic,
         string organssystems_lymphatic_obs,
         bool? organssystems_nervous,
         string organssystems_nervous_obs,
         // Parámetros para examen físico
         bool? physicalexamination_head,
         string physicalexamination_head_obs,
         bool? physicalexamination_neck,
         string physicalexamination_neck_obs,
         bool? physicalexamination_chest,
         string physicalexamination_chest_obs,
         bool? physicalexamination_abdomen,
         string physicalexamination_abdomen_obs,
         bool? physicalexamination_pelvis,
         string physicalexamination_pelvis_obs,
         bool? physicalexamination_limbs,
         string physicalexamination_limbs_obs,
         // Parámetros para antecedentes familiares
         bool? familiary_background_heartdisease,
         string familiary_background_heartdisease_observation,
         int? familiary_background_relatshcatalog_heartdisease,
         bool? familiary_background_diabetes,
         string familiary_background_diabetes_observation,
         int? familiary_background_relatshcatalog_diabetes,
         bool? familiary_background_dxcardiovascular,
         string familiary_background_dxcardiovascular_observation,
         int? familiary_background_relatshcatalog_dxcardiovascular,
         bool? familiary_background_hypertension,
         string familiary_background_hypertension_observation,
         int? familiary_background_relatshcatalog_hypertension,
         bool? familiary_background_cancer,
         string familiary_background_cancer_observation,
         int? familiary_background_relatshcatalog_cancer,
         bool? familiary_background_tuberculosis,
         string familiary_background_tuberculosis_observation,
         int? familiary_background_relatsh_tuberculosis,
         bool? familiary_background_dxmental,
         string familiary_background_dxmental_observation,
         int? familiary_background_relatshcatalog_dxmental,
         bool? familiary_background_dxinfectious,
         string familiary_background_dxinfectious_observation,
         int? familiary_background_relatshcatalog_dxinfectious,
         bool? familiary_background_malformation,
         string familiary_background_malformation_observation,
         int? familiary_background_relatshcatalog_malformation,
         bool? familiary_background_other,
         string familiary_background_other_observation,
         int? familiary_background_relatshcatalog_other,
         // Tablas relacionadas
         List<ConsultaAlergiaDTO> allergies_consultation,
         List<ConsultaCirugiaDTO> surgeries_consultation,
         List<ConsultaMedicamentoDTO> medications_consultation,
         List<ConsultaLaboratorioDTO> laboratories_consultation,
         List<ConsultaImagenDTO> images_consutlation,
         List<ConsultaDiagnosticoDTO> diagnosis_consultation)
        {
            using (var connection = new SqlConnection(_dbContext.Database.GetConnectionString()))
            {
                using (var command = new SqlCommand("sp_CreateConsultation", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Parámetros de consulta
                    AddSqlParameter(command, "@consultation_creationdate", DateTime.Today);
                    AddSqlParameter(command, "@consultation_usercreate", consultation_usercreate);
                    AddSqlParameter(command, "@consultation_patient", consultation_patient);
                    AddSqlParameter(command, "@consultation_speciality", consultation_speciality);
                    AddSqlParameter(command, "@consultation_historyclinic", consultation_historyclinic);
                    AddSqlParameter(command, "@consultation_reason", consultation_reason);
                    AddSqlParameter(command, "@consultation_disease", consultation_disease);
                    AddSqlParameter(command, "@consultation_familiaryname", consultation_familiaryname);
                    AddSqlParameter(command, "@consultation_warningsings", consultation_warningsings);
                    AddSqlParameter(command, "@consultation_nonpharmacologycal", consultation_nonpharmacologycal);
                    AddSqlParameter(command, "@consultation_familiarytype", consultation_familiarytype);
                    AddSqlParameter(command, "@consultation_familiaryphone", consultation_familiaryphone);
                    AddSqlParameter(command, "@consultation_temperature", consultation_temperature);
                    AddSqlParameter(command, "@consultation_respirationrate", consultation_respirationrate);
                    AddSqlParameter(command, "@consultation_bloodpressuredAS", consultation_bloodpressuredAS);
                    AddSqlParameter(command, "@consultation_bloodpresuredDIS", consultation_bloodpresuredDIS);
                    AddSqlParameter(command, "@consultation_pulse", consultation_pulse);
                    AddSqlParameter(command, "@consultation_weight", consultation_weight);
                    AddSqlParameter(command, "@consultation_size", consultation_size);
                    AddSqlParameter(command, "@consultation_treatmentplan", consultation_treatmentplan);
                    AddSqlParameter(command, "@consultation_observation", consultation_observation);
                    AddSqlParameter(command, "@consultation_personalbackground", consultation_personalbackground);
                    AddSqlParameter(command, "@consultation_disablilitydays", consultation_disablilitydays);
                    AddSqlParameter(command, "@consultation_type", consultation_speciality);

                    AddSqlParameter(command, "@consultation_status", consultation_status);

                    // Parámetros de órganos y sistemas
                    AddSqlParameter(command, "@organssystems_organsenses", organssystems_organsenses);
                    AddSqlParameter(command, "@organssystems_organsenses_Obs", organssystems_organsenses_Obs);
                    AddSqlParameter(command, "@organssystems_respiratory", organssystems_respiratory);
                    AddSqlParameter(command, "@organssystems_respiratory_obs", organssystems_respiratory_obs);
                    AddSqlParameter(command, "@organssystems_cardiovascular", organssystems_cardiovascular);
                    AddSqlParameter(command, "@organssystems_cardiovascular_obs", organssystems_cardiovascular_obs);
                    AddSqlParameter(command, "@organssystems_digestive", organssystems_digestive);
                    AddSqlParameter(command, "@organssystems_digestive_obs", organssystems_digestive_obs);
                    AddSqlParameter(command, "@organssystems_genital", organssystems_genital);
                    AddSqlParameter(command, "@organssystems_genital_obs", organssystems_genital_obs);
                    AddSqlParameter(command, "@organssystems_urinary", organssystems_urinary);
                    AddSqlParameter(command, "@organssystems_urinary_obs", organssystems_urinary_obs);
                    AddSqlParameter(command, "@organssystems_skeletal_m", organssystems_skeletal_m);
                    AddSqlParameter(command, "@organssystems_skeletal_m_obs", organssystems_skeletal_m_obs);
                    AddSqlParameter(command, "@organssystems_endrocrine", organssystems_endrocrine);
                    AddSqlParameter(command, "@organssystems_endocrine", organssystems_endocrine);
                    AddSqlParameter(command, "@organssystems_lymphatic", organssystems_lymphatic);
                    AddSqlParameter(command, "@organssystems_lymphatic_obs", organssystems_lymphatic_obs);
                    AddSqlParameter(command, "@organssystems_nervous", organssystems_nervous);
                    AddSqlParameter(command, "@organssystems_nervous_obs", organssystems_nervous_obs);

                    // Parámetros de examen físico
                    AddSqlParameter(command, "@physicalexamination_head", physicalexamination_head);
                    AddSqlParameter(command, "@physicalexamination_head_obs", physicalexamination_head_obs);
                    AddSqlParameter(command, "@physicalexamination_neck", physicalexamination_neck);
                    AddSqlParameter(command, "@physicalexamination_neck_obs", physicalexamination_neck_obs);
                    AddSqlParameter(command, "@physicalexamination_chest", physicalexamination_chest);
                    AddSqlParameter(command, "@physicalexamination_chest_obs", physicalexamination_chest_obs);
                    AddSqlParameter(command, "@physicalexamination_abdomen", physicalexamination_abdomen);
                    AddSqlParameter(command, "@physicalexamination_abdomen_obs", physicalexamination_abdomen_obs);
                    AddSqlParameter(command, "@physicalexamination_pelvis", physicalexamination_pelvis);
                    AddSqlParameter(command, "@physicalexamination_pelvis_obs", physicalexamination_pelvis_obs);
                    AddSqlParameter(command, "@physicalexamination_limbs", physicalexamination_limbs);
                    AddSqlParameter(command, "@physicalexamination_limbs_obs", physicalexamination_limbs_obs);

                    // Parámetros de antecedentes familiares
                    AddSqlParameter(command, "@familiary_background_heartdisease", familiary_background_heartdisease);
                    AddSqlParameter(command, "@familiary_background_heartdisease_observation", familiary_background_heartdisease_observation);
                    AddSqlParameter(command, "@familiary_background_relatshcatalog_heartdisease", familiary_background_relatshcatalog_heartdisease);
                    AddSqlParameter(command, "@familiary_background_diabetes", familiary_background_diabetes);
                    AddSqlParameter(command, "@familiary_background_diabetes_observation", familiary_background_diabetes_observation);
                    AddSqlParameter(command, "@familiary_background_relatshcatalog_diabetes", familiary_background_relatshcatalog_diabetes);
                    AddSqlParameter(command, "@familiary_background_dxcardiovascular", familiary_background_dxcardiovascular);
                    AddSqlParameter(command, "@familiary_background_dxcardiovascular_observation", familiary_background_dxcardiovascular_observation);
                    AddSqlParameter(command, "@familiary_background_relatshcatalog_dxcardiovascular", familiary_background_relatshcatalog_dxcardiovascular);
                    AddSqlParameter(command, "@familiary_background_hypertension", familiary_background_hypertension);
                    AddSqlParameter(command, "@familiary_background_hypertension_observation", familiary_background_hypertension_observation);
                    AddSqlParameter(command, "@familiary_background_relatshcatalog_hypertension", familiary_background_relatshcatalog_hypertension);
                    AddSqlParameter(command, "@familiary_background_cancer", familiary_background_cancer);
                    AddSqlParameter(command, "@familiary_background_cancer_observation", familiary_background_cancer_observation);
                    AddSqlParameter(command, "@familiary_background_relatshcatalog_cancer", familiary_background_relatshcatalog_cancer);
                    AddSqlParameter(command, "@familiary_background_tuberculosis", familiary_background_tuberculosis);
                    AddSqlParameter(command, "@familiary_background_tuberculosis_observation", familiary_background_tuberculosis_observation);
                    AddSqlParameter(command, "@familiary_background_relatsh_tuberculosis", familiary_background_relatsh_tuberculosis);
                    AddSqlParameter(command, "@familiary_background_dxmental", familiary_background_dxmental);
                    AddSqlParameter(command, "@familiary_background_dxmental_observation", familiary_background_dxmental_observation);
                    AddSqlParameter(command, "@familiary_background_relatshcatalog_dxmental", familiary_background_relatshcatalog_dxmental);
                    AddSqlParameter(command, "@familiary_background_dxinfectious", familiary_background_dxinfectious);
                    AddSqlParameter(command, "@familiary_background_dxinfectious_observation", familiary_background_dxinfectious_observation);
                    AddSqlParameter(command, "@familiary_background_relatshcatalog_dxinfectious", familiary_background_relatshcatalog_dxinfectious);
                    AddSqlParameter(command, "@familiary_background_malformation", familiary_background_malformation);
                    AddSqlParameter(command, "@familiary_background_malformation_observation", familiary_background_malformation_observation);
                    AddSqlParameter(command, "familiary_background_relatshcatalog_malformation", familiary_background_relatshcatalog_malformation);
                    AddSqlParameter(command, "@familiary_background_other", familiary_background_other);
                    AddSqlParameter(command, "@familiary_background_other_observation", familiary_background_other_observation);
                    AddSqlParameter(command, "@familiary_background_relatshcatalog_other", familiary_background_relatshcatalog_other);

                    // Tablas relacionadas (se inicializan con CreateDataTable)
                    AddSqlParameter(command, "@allergies", CreateDataTable(allergies_consultation));
                    AddSqlParameter(command, "@surgeries", CreateDataTable(surgeries_consultation));
                    AddSqlParameter(command, "@medications", CreateDataTable(medications_consultation));
                    AddSqlParameter(command, "@laboratories", CreateDataTable(laboratories_consultation));
                    AddSqlParameter(command, "@images", CreateDataTable(images_consutlation));
                    AddSqlParameter(command, "@diagnostics", CreateDataTable(diagnosis_consultation));

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        private void AddSqlParameter(SqlCommand command, string paramName, object value)
        {
            if (value == null)
            {
                command.Parameters.AddWithValue(paramName, DBNull.Value);
            }
            else
            {
                command.Parameters.AddWithValue(paramName, value);
            }
        }



        private DataTable CreateDataTable<T>(List<T> list)
        {
            var table = new DataTable();
            var properties = typeof(T).GetProperties();

            // Crear columnas en el DataTable basadas en las propiedades de la clase
            foreach (var prop in properties)
            {
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            // Rellenar las filas del DataTable con los valores de los objetos
            foreach (var item in list)
            {
                var row = table.NewRow();
                foreach (var prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }
                table.Rows.Add(row);
            }

            return table;
        }


        public DataSet GetConsultationDetails(int consultationId)
        {
            using (SqlConnection connection = new SqlConnection(_dbContext.Database.GetConnectionString()))
            {
                using (SqlCommand command = new SqlCommand("sp_GetConsultationDetails", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@consultation_id", consultationId));

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataSet dataSet = new DataSet();
                    adapter.Fill(dataSet);

                    return dataSet;
                }
            }
        }

    }
}
