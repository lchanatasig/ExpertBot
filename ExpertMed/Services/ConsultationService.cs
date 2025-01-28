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


         public async Task CrearConsultaAsync(
         string usuariocreacionConsulta,
         string historialConsulta,
         int pacienteConsultaP,
         string motivoConsulta,
         string enfermedadConsulta,
         string nombreparienteConsulta,
         string signosalarmaConsulta,
         string reconofarmacologicas,
         int tipoparienteConsulta,
         string telefonoParienteConsulta,
         string temperaturaConsulta,
         string frecuenciarespiratoriaConsulta,
         string presionarterialsistolicaConsulta,
         string presionarterialdiastolicaConsulta,
         string pulsoConsulta,
         string pesoConsulta,
         string tallaConsulta,
         string plantratamientoConsulta,
         string observacionConsulta,
         string antecedentespersonalesConsulta,
         int diasincapacidadConsulta,
         int medicoConsultaD,
         int especialidadId,
         int tipoConsultaC,
         string notasevolucionConsulta,
         string consultaprincipalConsulta,
         int estadoConsultaC,
         // Parámetros para órganos y sistemas
         bool? orgSentidos,
         string obserOrgSentidos,
         bool? respiratorio,
         string obserRespiratorio,
         bool? cardioVascular,
         string obserCardioVascular,
         bool? digestivo,
         string obserDigestivo,
         bool? genital,
         string obserGenital,
         bool? urinario,
         string obserUrinario,
         bool? mEsqueletico,
         string obserMEsqueletico,
         bool? endocrino,
         string obserEndocrino,
         bool? linfatico,
         string obserLinfatico,
         bool? nervioso,
         string obserNervioso,
         // Parámetros para examen físico
         bool? cabeza,
         string obserCabeza,
         bool? cuello,
         string obserCuello,
         bool? torax,
         string obserTorax,
         bool? abdomen,
         string obserAbdomen,
         bool? pelvis,
         string obserPelvis,
         bool? extremidades,
         string obserExtremidades,
         // Parámetros para antecedentes familiares
         bool? cardiopatia,
         string obserCardiopatia,
         int? parentescocatalogoCardiopatia,
         bool? diabetes,
         string obserDiabetes,
         int? parentescocatalogoDiabetes,
         bool? enfCardiovascular,
         string obserEnfCardiovascular,
         int? parentescocatalogoEnfCardiovascular,
         bool? hipertension,
         string obserHipertension,
         int? parentescocatalogoHipertension,
         bool? cancer,
         string obserCancer,
         int? parentescocatalogoCancer,
         bool? tuberculosis,
         string obserTuberculosis,
         int? parentescocatalogoTuberculosis,
         bool? enfMental,
         string obserEnfMental,
         int? parentescocatalogoEnfMental,
         bool? enfInfecciosa,
         string obserEnfInfecciosa,
         int? parentescocatalogoEnfInfecciosa,
         bool? malFormacion,
         string obserMalFormacion,
         int? parentescocatalogoMalFormacion,
         bool? otro,
         string obserOtro,
         int? parentescocatalogoOtro,
         // Tablas relacionadas
         List<ConsultaAlergiaDTO> alergias,
         List<ConsultaCirugiaDTO> cirugias,
         List<ConsultaMedicamentoDTO> medicamentos,
         List<ConsultaLaboratorioDTO> laboratorios,
         List<ConsultaImagenDTO> imagenes,
         List<ConsultaDiagnosticoDTO> diagnosticos)
        {
            using (var connection = new SqlConnection(_dbContext.Database.GetConnectionString()))
            {
                using (var command = new SqlCommand("sp_CreateConsultation", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Parámetros de consulta
                    AddSqlParameter(command, "@usuariocreacion_consulta", usuariocreacionConsulta);
                    AddSqlParameter(command, "@historial_consulta", historialConsulta);
                    AddSqlParameter(command, "@paciente_consulta_p", pacienteConsultaP);
                    AddSqlParameter(command, "@motivo_consulta", motivoConsulta);
                    AddSqlParameter(command, "@enfermedad_consulta", enfermedadConsulta);
                    AddSqlParameter(command, "@nombrepariente_consulta", nombreparienteConsulta);
                    AddSqlParameter(command, "@signosalarma_consulta", signosalarmaConsulta);
                    AddSqlParameter(command, "@reconofarmacologicas", reconofarmacologicas);
                    AddSqlParameter(command, "@tipopariente_consulta", tipoparienteConsulta);
                    AddSqlParameter(command, "@telefono_pariente_consulta", telefonoParienteConsulta);
                    AddSqlParameter(command, "@temperatura_consulta", temperaturaConsulta);
                    AddSqlParameter(command, "@frecuenciarespiratoria_consulta", frecuenciarespiratoriaConsulta);
                    AddSqlParameter(command, "@presionarterialsistolica_consulta", presionarterialsistolicaConsulta);
                    AddSqlParameter(command, "@presionarterialdiastolica_consulta", presionarterialdiastolicaConsulta);
                    AddSqlParameter(command, "@pulso_consulta", pulsoConsulta);
                    AddSqlParameter(command, "@peso_consulta", pesoConsulta);
                    AddSqlParameter(command, "@talla_consulta", tallaConsulta);
                    AddSqlParameter(command, "@plantratamiento_consulta", plantratamientoConsulta);
                    AddSqlParameter(command, "@observacion_consulta", observacionConsulta);
                    AddSqlParameter(command, "@antecedentespersonales_consulta", antecedentespersonalesConsulta);
                    AddSqlParameter(command, "@diasincapacidad_consulta", diasincapacidadConsulta);
                    AddSqlParameter(command, "@medico_consulta_d", medicoConsultaD);
                    AddSqlParameter(command, "@especialidad_id", especialidadId);
                    AddSqlParameter(command, "@tipo_consulta_c", tipoConsultaC);
                    AddSqlParameter(command, "@notasevolucion_consulta", notasevolucionConsulta);
                    AddSqlParameter(command, "@consultaprincipal_consulta", consultaprincipalConsulta);
                    AddSqlParameter(command, "@estado_consulta_c", estadoConsultaC);

                    // Parámetros de órganos y sistemas
                    AddSqlParameter(command, "@org_sentidos", orgSentidos);
                    AddSqlParameter(command, "@obser_org_sentidos", obserOrgSentidos);
                    AddSqlParameter(command, "@respiratorio", respiratorio);
                    AddSqlParameter(command, "@obser_respiratorio", obserRespiratorio);
                    AddSqlParameter(command, "@cardio_vascular", cardioVascular);
                    AddSqlParameter(command, "@obser_cardio_vascular", obserCardioVascular);
                    AddSqlParameter(command, "@digestivo", digestivo);
                    AddSqlParameter(command, "@obser_digestivo", obserDigestivo);
                    AddSqlParameter(command, "@genital", genital);
                    AddSqlParameter(command, "@obser_genital", obserGenital);
                    AddSqlParameter(command, "@urinario", urinario);
                    AddSqlParameter(command, "@obser_urinario", obserUrinario);
                    AddSqlParameter(command, "@m_esqueletico", mEsqueletico);
                    AddSqlParameter(command, "@obser_m_esqueletico", obserMEsqueletico);
                    AddSqlParameter(command, "@endocrino", endocrino);
                    AddSqlParameter(command, "@obser_endocrino", obserEndocrino);
                    AddSqlParameter(command, "@linfatico", linfatico);
                    AddSqlParameter(command, "@obser_linfatico", obserLinfatico);
                    AddSqlParameter(command, "@nervioso", nervioso);
                    AddSqlParameter(command, "@obser_nervioso", obserNervioso);

                    // Parámetros de examen físico
                    AddSqlParameter(command, "@cabeza", cabeza);
                    AddSqlParameter(command, "@obser_cabeza", obserCabeza);
                    AddSqlParameter(command, "@cuello", cuello);
                    AddSqlParameter(command, "@obser_cuello", obserCuello);
                    AddSqlParameter(command, "@torax", torax);
                    AddSqlParameter(command, "@obser_torax", obserTorax);
                    AddSqlParameter(command, "@abdomen", abdomen);
                    AddSqlParameter(command, "@obser_abdomen", obserAbdomen);
                    AddSqlParameter(command, "@pelvis", pelvis);
                    AddSqlParameter(command, "@obser_pelvis", obserPelvis);
                    AddSqlParameter(command, "@extremidades", extremidades);
                    AddSqlParameter(command, "@obser_extremidades", obserExtremidades);

                    // Parámetros de antecedentes familiares
                    AddSqlParameter(command, "@cardiopatia", cardiopatia);
                    AddSqlParameter(command, "@obser_cardiopatia", obserCardiopatia);
                    AddSqlParameter(command, "@parentescocatalogo_cardiopatia", parentescocatalogoCardiopatia);
                    AddSqlParameter(command, "@diabetes", diabetes);
                    AddSqlParameter(command, "@obser_diabetes", obserDiabetes);
                    AddSqlParameter(command, "@parentescocatalogo_diabetes", parentescocatalogoDiabetes);
                    AddSqlParameter(command, "@enf_cardiovascular", enfCardiovascular);
                    AddSqlParameter(command, "@obser_enf_cardiovascular", obserEnfCardiovascular);
                    AddSqlParameter(command, "@parentescocatalogo_enfcardiovascular", parentescocatalogoEnfCardiovascular);
                    AddSqlParameter(command, "@hipertension", hipertension);
                    AddSqlParameter(command, "@obser_hipertension", obserHipertension);
                    AddSqlParameter(command, "@parentescocatalogo_hipertension", parentescocatalogoHipertension);
                    AddSqlParameter(command, "@cancer", cancer);
                    AddSqlParameter(command, "@obser_cancer", obserCancer);
                    AddSqlParameter(command, "@parentescocatalogo_cancer", parentescocatalogoCancer);
                    AddSqlParameter(command, "@tuberculosis", tuberculosis);
                    AddSqlParameter(command, "@obser_tuberculosis", obserTuberculosis);
                    AddSqlParameter(command, "@parentescocatalogo_tuberculosis", parentescocatalogoTuberculosis);
                    AddSqlParameter(command, "@enf_mental", enfMental);
                    AddSqlParameter(command, "@obser_enf_mental", obserEnfMental);
                    AddSqlParameter(command, "@parentescocatalogo_enfmental", parentescocatalogoEnfMental);
                    AddSqlParameter(command, "@enf_infecciosa", enfInfecciosa);
                    AddSqlParameter(command, "@obser_enf_infecciosa", obserEnfInfecciosa);
                    AddSqlParameter(command, "@parentescocatalogo_enfinfecciosa", parentescocatalogoEnfInfecciosa);
                    AddSqlParameter(command, "@mal_formacion", malFormacion);
                    AddSqlParameter(command, "@obser_mal_formacion", obserMalFormacion);
                    AddSqlParameter(command, "@parentescocatalogo_malformacion", parentescocatalogoMalFormacion);
                    AddSqlParameter(command, "@otro", otro);
                    AddSqlParameter(command, "@obser_otro", obserOtro);
                    AddSqlParameter(command, "@parentescocatalogo_otro", parentescocatalogoOtro);

                    // Tablas relacionadas (se inicializan con CreateDataTable)
                    AddSqlParameter(command, "@Alergias", CreateDataTable(alergias));
                    AddSqlParameter(command, "@Cirugias", CreateDataTable(cirugias));
                    AddSqlParameter(command, "@Medicamentos", CreateDataTable(medicamentos));
                    AddSqlParameter(command, "@Laboratorio", CreateDataTable(laboratorios));
                    AddSqlParameter(command, "@Imagenes", CreateDataTable(imagenes));
                    AddSqlParameter(command, "@Diagnosticos", CreateDataTable(diagnosticos));

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


    }
}
