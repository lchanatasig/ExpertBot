using ExpertMed.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Numerics;
using System.Text.Json;

namespace ExpertMed.Services
{
    public class PatientService
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<PatientService> _logger;
        private readonly DbExpertmedContext _dbContext;

        public PatientService(IHttpContextAccessor httpContextAccessor, ILogger<PatientService> logger, DbExpertmedContext dbContext)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<List<User>> GetDoctorsByAssistantAsync(int assistantUserId)
        {
            var doctors = new List<User>();

            try
            {
                using (var connection = new SqlConnection(_dbContext.Database.GetConnectionString()))
                using (var command = new SqlCommand("GetDoctorsByAssistant", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@AssistantUserId", assistantUserId);

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var doctor = new User
                            {
                                UsersId = reader.GetInt32(reader.GetOrdinal("users_id")),
                                UsersNames = reader.IsDBNull(reader.GetOrdinal("users_names")) ? null : reader.GetString(reader.GetOrdinal("users_names")),
                                UsersSurcenames = reader.IsDBNull(reader.GetOrdinal("users_surcenames")) ? null : reader.GetString(reader.GetOrdinal("users_surcenames"))

                                // Agrega otros campos según la estructura de la tabla "users"
                            };

                            doctors.Add(doctor);
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                // Log más detallado para errores de base de datos
                throw new Exception($"Error al obtener los médicos asociados para el asistente {assistantUserId}: {sqlEx.Message}", sqlEx);
            }
            catch (Exception ex)
            {
                // Log más detallado para errores genéricos
                throw new Exception($"Error inesperado al obtener los médicos asociados: {ex.Message}", ex);
            }

            return doctors;
        }

        //Método para obtener todos los pacientes, el administrador o perfil 1 tiene todos los pacientes,
        //el perfil 2 tiene solo los pacientes de el
        // Método para obtener todos los pacientes

        public async Task<List<Patient>> GetAllPatientsAsync(int userProfile, int? userId = null)
        {
            try
            {
                // Definir los parámetros para el procedimiento almacenado
                var parameters = new[]
                {
            new SqlParameter("@UserProfile", userProfile),
            new SqlParameter("@UserID", userId ?? (object)DBNull.Value)
        };

                // Ejecutar el procedimiento almacenado y convertir el resultado en lista
                var patients = await _dbContext.Patients
                    .FromSqlRaw("EXEC sp_ListAllPatients @UserProfile, @UserID", parameters)
                    .ToListAsync(); // Usar ToListAsync() en lugar de .AsEnumerable().ToList()

                // Cargar relaciones necesarias explícitamente
                foreach (var patient in patients)
                {
                    // Esto es útil si necesitas cargar relaciones adicionales como PatientNationalityNavigation o PatientCreationuserNavigation
                    // Usamos 'LoadAsync' solo cuando sea necesario, pero si tienes muchas relaciones o muchos pacientes, puede afectar el rendimiento
                    await _dbContext.Entry(patient)
                        .Reference(p => p.PatientNationalityNavigation)
                        .LoadAsync();

                    await _dbContext.Entry(patient)
                        .Reference(p => p.PatientCreationuserNavigation)
                        .LoadAsync();
                }

                return patients;
            }
            catch (SqlException sqlEx)
            {
                // Log de error en caso de falla SQL
                _logger.LogError(sqlEx, "Error al ejecutar el procedimiento almacenado en la base de datos.");
                throw; // Rethrow para que el error sea manejado o visto en un nivel superior
            }
            catch (Exception ex)
            {
                // Log de error general
                _logger.LogError(ex, "Error al obtener los pacientes.");
                throw; // Rethrow para que el error sea manejado o visto en un nivel superior
            }
        }


        // Método para crear un nuevo paciente
        public async Task<int> CreatePatientAsync(Patient patient, int? doctorUserId = null)
        {
            using (var connection = new SqlConnection(_dbContext.Database.GetDbConnection().ConnectionString))
            {
                using (var command = new SqlCommand("sp_CreatePatient", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Agregar parámetros desde el modelo Patient
                    command.Parameters.AddWithValue("@patient_creationuser", patient.PatientCreationuser);
                    command.Parameters.AddWithValue("@patient_modificationuser", patient.PatientModificationuser);
                    command.Parameters.AddWithValue("@patient_documenttype", patient.PatientDocumenttype);
                    command.Parameters.AddWithValue("@patient_documentnumber", patient.PatientDocumentnumber);
                    command.Parameters.AddWithValue("@patient_firstname", patient.PatientFirstname);
                    command.Parameters.AddWithValue("@patient_middlename", patient.PatientMiddlename ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@patient_firstsurname", patient.PatientFirstsurname);
                    command.Parameters.AddWithValue("@patient_secondlastname", patient.PatientSecondlastname ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@patient_gender", patient.PatientGender);
                    command.Parameters.AddWithValue("@patient_birthdate", patient.PatientBirthdate);
                    command.Parameters.AddWithValue("@patient_age", patient.PatientAge);
                    command.Parameters.AddWithValue("@patient_bloodtype", patient.PatientBloodtype);
                    command.Parameters.AddWithValue("@patient_donor", patient.PatientDonor ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@patient_maritalstatus", patient.PatientMaritalstatus);
                    command.Parameters.AddWithValue("@patient_vocational_training", patient.PatientVocationalTraining);
                    command.Parameters.AddWithValue("@patient_landline_phone", patient.PatientLandlinePhone ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@patient_cellular_phone", patient.PatientCellularPhone ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@patient_email", patient.PatientEmail ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@patient_nationality", patient.PatientNationality);
                    command.Parameters.AddWithValue("@patient_province", patient.PatientProvince);
                    command.Parameters.AddWithValue("@patient_address", patient.PatientAddress);
                    command.Parameters.AddWithValue("@patient_ocupation", patient.PatientOcupation ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@patient_company", patient.PatientCompany ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@patient_healt_insurance", patient.PatientHealtInsurance);
                    command.Parameters.AddWithValue("@patient_status", patient.PatientStatus);

                    // Parámetro opcional: doctor_userid
                    if (doctorUserId.HasValue)
                    {
                        command.Parameters.AddWithValue("@doctor_userid", doctorUserId.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@doctor_userid", DBNull.Value);
                    }

                    try
                    {
                        await connection.OpenAsync();

                        // Ejecutar el procedimiento almacenado
                        string jsonResult = null;
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                jsonResult = reader.GetString(0);
                            }
                        }

                        if (string.IsNullOrEmpty(jsonResult))
                        {
                            throw new Exception("Error inesperado: No se obtuvo ningún resultado del procedimiento almacenado.");
                        }

                        // Deserializar el resultado JSON
                        using (JsonDocument document = JsonDocument.Parse(jsonResult))
                        {
                            var root = document.RootElement;

                            // Validar el resultado
                            if (root.TryGetProperty("success", out var success) && success.GetInt32() == 1)
                            {
                                if (root.TryGetProperty("patientId", out var patientId))
                                {
                                    return patientId.GetInt32();
                                }
                                else
                                {
                                    throw new Exception("El campo 'patientId' no se encuentra en el resultado.");
                                }
                            }
                            else
                            {
                                string errorMessage = root.TryGetProperty("message", out var message)
                                    ? message.GetString()
                                    : "Error al crear el paciente.";
                                throw new Exception(errorMessage);
                            }
                        }
                    }
                    finally
                    {
                        if (connection.State == ConnectionState.Open)
                        {
                            await connection.CloseAsync();
                        }
                    }
                }
            }
        }

        // Método para activar o desactivar al usuario
        public async Task<(bool success, string message)> DesactiveOrActivePatientAsync(int patientId, int status)
        {
            try
            {
                // Crear la conexión
                using (var connection = _dbContext.Database.GetDbConnection())
                {
                    await connection.OpenAsync();

                    // Crear el comando para ejecutar el SP
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "sp_DesactiveOrActivePatient";
                        command.CommandType = CommandType.StoredProcedure;

                        // Parámetros del procedimiento almacenado
                        command.Parameters.Add(new SqlParameter("@PatientId", SqlDbType.Int) { Value = patientId });
                        command.Parameters.Add(new SqlParameter("@Status", SqlDbType.Int) { Value = status });

                        // Ejecutar el comando y obtener la respuesta
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (reader.Read())
                            {
                                string success = reader["success"].ToString();
                                string message = reader["message"].ToString();

                                if (success == "true")
                                {
                                    return (true, message);
                                }
                                else
                                {
                                    return (false, message);
                                }
                            }
                            else
                            {
                                return (false, "No se recibió respuesta válida del procedimiento.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al activar/desactivar el paciente.");
                return (false, "Ocurrió un error al procesar la solicitud.");
            }

        }


        public async Task<int> UpdatePatientAsync(Patient patient, int? doctorUserId = null)
        {
            using (var connection = new SqlConnection(_dbContext.Database.GetDbConnection().ConnectionString))
            {
                using (var command = new SqlCommand("sp_UpdatePatient", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Adding parameters from Patient object
                    command.Parameters.AddWithValue("@patient_id", patient.PatientId);
                    command.Parameters.AddWithValue("@patient_modificationuser", patient.PatientModificationuser);
                    command.Parameters.AddWithValue("@patient_documenttype", patient.PatientDocumenttype);
                    command.Parameters.AddWithValue("@patient_documentnumber", patient.PatientDocumentnumber);
                    command.Parameters.AddWithValue("@patient_firstname", patient.PatientFirstname);
                    command.Parameters.AddWithValue("@patient_middlename", patient.PatientMiddlename ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@patient_firstsurname", patient.PatientFirstsurname);
                    command.Parameters.AddWithValue("@patient_secondlastname", patient.PatientSecondlastname ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@patient_gender", patient.PatientGender);
                    command.Parameters.AddWithValue("@patient_birthdate", patient.PatientBirthdate);
                    command.Parameters.AddWithValue("@patient_age", patient.PatientAge);
                    command.Parameters.AddWithValue("@patient_bloodtype", patient.PatientBloodtype);
                    command.Parameters.AddWithValue("@patient_donor", patient.PatientDonor ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@patient_maritalstatus", patient.PatientMaritalstatus);
                    command.Parameters.AddWithValue("@patient_vocational_training", patient.PatientVocationalTraining);
                    command.Parameters.AddWithValue("@patient_landline_phone", patient.PatientLandlinePhone ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@patient_cellular_phone", patient.PatientCellularPhone ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@patient_email", patient.PatientEmail ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@patient_nationality", patient.PatientNationality);
                    command.Parameters.AddWithValue("@patient_province", patient.PatientProvince);
                    command.Parameters.AddWithValue("@patient_address", patient.PatientAddress);
                    command.Parameters.AddWithValue("@patient_ocupation", patient.PatientOcupation ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@patient_company", patient.PatientCompany ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@patient_health_insurance", patient.PatientHealtInsurance);
                    command.Parameters.AddWithValue("@patient_code", patient.PatientCode ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@patient_status", patient.PatientStatus);

                    // Si doctor_userid está presente, agregar el parámetro
                    if (doctorUserId.HasValue)
                    {
                        command.Parameters.AddWithValue("@doctor_userid", doctorUserId.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@doctor_userid", DBNull.Value);
                    }

                    try
                    {
                        await connection.OpenAsync();

                        // Execute the stored procedure
                        string jsonResult = null;
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                jsonResult = reader.GetString(0);
                            }
                        }

                        if (string.IsNullOrEmpty(jsonResult))
                        {
                            throw new Exception("Error inesperado: No se obtuvo ningún resultado del procedimiento almacenado.");
                        }

                        // Deserialize the JSON result
                        using (JsonDocument document = JsonDocument.Parse(jsonResult))
                        {
                            var root = document.RootElement;

                            // Validate the result
                            if (root.TryGetProperty("success", out var success) && success.GetInt32() == 1)
                            {
                                if (root.TryGetProperty("patientId", out var patientId))
                                {
                                    return patientId.GetInt32();
                                }
                                else
                                {
                                    throw new Exception("El campo 'patientId' no se encuentra en el resultado.");
                                }
                            }
                            else
                            {
                                string errorMessage = root.TryGetProperty("message", out var message)
                                    ? message.GetString()
                                    : "Error al actualizar el paciente.";
                                throw new Exception(errorMessage);
                            }
                        }
                    }
                    finally
                    {
                        if (connection.State == ConnectionState.Open)
                        {
                            await connection.CloseAsync();
                        }
                    }
                }
            }
        }


        public async Task<DetailsPatientConsult> GetPatientDataByIdAsync(int patientId)
        {
            DetailsPatientConsult patient = null;
            var doctors = new List<DoctorPatient>();

            try
            {
                using (var connection = new SqlConnection(_dbContext.Database.GetConnectionString()))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand("sp_GetPatientById", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Agregar el parámetro del ID del paciente
                        command.Parameters.Add(new SqlParameter("@PatientId", SqlDbType.Int) { Value = patientId });

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            // Leer los datos del paciente
                            if (await reader.ReadAsync())
                            {
                                patient = new DetailsPatientConsult
                                {
                                    PatientId = reader.GetInt32(reader.GetOrdinal("patient_id")),
                                    PatientCreationdate = reader.GetDateTime(reader.GetOrdinal("patient_creationdate")),
                                    PatientModificationdate = reader.GetDateTime(reader.GetOrdinal("patient_modificationdate")),
                                    PatientCreationuser = reader.GetInt32(reader.GetOrdinal("patient_creationuser")),
                                    PatientModificationuser = reader.GetInt32(reader.GetOrdinal("patient_modificationuser")),
                                    PatientDocumenttype = reader.GetInt32(reader.GetOrdinal("patient_documenttype")),
                                    PatientDocumentnumber = reader.IsDBNull(reader.GetOrdinal("patient_documentnumber")) ? null : reader.GetString(reader.GetOrdinal("patient_documentnumber")),
                                    PatientFirstname = reader.IsDBNull(reader.GetOrdinal("patient_firstname")) ? null : reader.GetString(reader.GetOrdinal("patient_firstname")),
                                    PatientMiddlename = reader.IsDBNull(reader.GetOrdinal("patient_middlename")) ? null : reader.GetString(reader.GetOrdinal("patient_middlename")),
                                    PatientFirstsurname = reader.IsDBNull(reader.GetOrdinal("patient_firstsurname")) ? null : reader.GetString(reader.GetOrdinal("patient_firstsurname")),
                                    PatientSecondlastname = reader.IsDBNull(reader.GetOrdinal("patient_secondlastname")) ? null : reader.GetString(reader.GetOrdinal("patient_secondlastname")),
                                    PatientGender = reader.IsDBNull(reader.GetOrdinal("patient_gender")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("patient_gender")),
                                    PatientBirthdate = reader.IsDBNull(reader.GetOrdinal("patient_birthdate")) ? (DateOnly?)null : DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("patient_birthdate"))),
                                    PatientAge = reader.GetInt32(reader.GetOrdinal("patient_age")),
                                    PatientBloodtype = reader.IsDBNull(reader.GetOrdinal("patient_bloodtype")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("patient_bloodtype")),
                                    PatientDonor = reader.IsDBNull(reader.GetOrdinal("patient_donor")) ? null : reader.GetString(reader.GetOrdinal("patient_donor")),
                                    PatientMaritalstatus = reader.IsDBNull(reader.GetOrdinal("patient_maritalstatus")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("patient_maritalstatus")),
                                    PatientVocationalTraining = reader.IsDBNull(reader.GetOrdinal("patient_vocational_training")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("patient_vocational_training")),
                                    PatientLandlinePhone = reader.IsDBNull(reader.GetOrdinal("patient_landline_phone")) ? null : reader.GetString(reader.GetOrdinal("patient_landline_phone")),
                                    PatientCellularPhone = reader.IsDBNull(reader.GetOrdinal("patient_cellular_phone")) ? null : reader.GetString(reader.GetOrdinal("patient_cellular_phone")),
                                    PatientEmail = reader.IsDBNull(reader.GetOrdinal("patient_email")) ? null : reader.GetString(reader.GetOrdinal("patient_email")),
                                    PatientNationality = reader.IsDBNull(reader.GetOrdinal("patient_nationality")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("patient_nationality")),
                                    PatientProvince = reader.IsDBNull(reader.GetOrdinal("patient_province")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("patient_province")),
                                    PatientAddress = reader.IsDBNull(reader.GetOrdinal("patient_address")) ? null : reader.GetString(reader.GetOrdinal("patient_address")),
                                    PatientOcupation = reader.IsDBNull(reader.GetOrdinal("patient_ocupation")) ? null: reader.GetString(reader.GetOrdinal("patient_ocupation")),
                                    PatientCompany = reader.IsDBNull(reader.GetOrdinal("patient_company")) ? null : reader.GetString(reader.GetOrdinal("patient_company")),
                                    PatientHealthInsurance = reader.IsDBNull(reader.GetOrdinal("patient_healt_insurance")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("patient_healt_insurance")),
                                    PatientCode = reader.IsDBNull(reader.GetOrdinal("patient_code")) ? null : reader.GetString(reader.GetOrdinal("patient_code")),
                                    PatientStatus = reader.GetInt32(reader.GetOrdinal("patient_status"))
                                };
                            }

                            // Leer los médicos asociados al paciente
                            if (await reader.NextResultAsync())
                            {
                                while (await reader.ReadAsync())
                                {
                                    var doctor = new DoctorPatient
                                    {
                                        DoctorUserid = reader.GetInt32(reader.GetOrdinal("doctor_userid")),
                                        RelationshipStatus = reader.GetInt32(reader.GetOrdinal("relationship_status"))
                                    };
                                    doctors.Add(doctor);
                                }
                            }
                        }
                    }
                }

                // Asignar la lista de médicos al paciente
                if (patient != null)
                {
                    patient.Doctors = doctors;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error fetching patient data: {ex.Message}");
                // Manejo de errores (puedes agregar más log o re-throw la excepción si es necesario)
            }

            return patient;
        }

        public async Task<Patient> GetPatientDetailsAsync(int patientId)
        {
            Patient patientDetails = null;

            using (var connection = new SqlConnection(_dbContext.Database.GetConnectionString()))
            {
                try
                {
                    // Abrir la conexión
                    await connection.OpenAsync();

                    // Configurar el comando para ejecutar el procedimiento almacenado
                    using (var command = new SqlCommand("sp_GetPatientById", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@PatientId", patientId);

                        // Ejecutar el comando y leer los resultados
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                // Mapear los datos del paciente
                                patientDetails = new Patient
                                {
                                    PatientId = reader.GetInt32(reader.GetOrdinal("patient_id")),
                                    PatientCreationdate = reader.IsDBNull(reader.GetOrdinal("patient_creationdate"))
                                        ? (DateTime?)null
                                        : reader.GetDateTime(reader.GetOrdinal("patient_creationdate")),
                                    PatientModificationdate = reader.IsDBNull(reader.GetOrdinal("patient_modificationdate"))
                                        ? (DateTime?)null
                                        : reader.GetDateTime(reader.GetOrdinal("patient_modificationdate")),
                                    PatientCreationuser = reader.IsDBNull(reader.GetOrdinal("patient_creationuser"))
                                        ? (int?)null
                                        : reader.GetInt32(reader.GetOrdinal("patient_creationuser")),
                                    PatientModificationuser = reader.IsDBNull(reader.GetOrdinal("patient_modificationuser"))
                                        ? (int?)null
                                        : reader.GetInt32(reader.GetOrdinal("patient_modificationuser")),
                                    PatientDocumenttype = reader.IsDBNull(reader.GetOrdinal("patient_documenttype"))
                                        ? (int?)null
                                        : reader.GetInt32(reader.GetOrdinal("patient_documenttype")),
                                    PatientDocumentnumber = reader.GetString(reader.GetOrdinal("patient_documentnumber")),
                                    PatientFirstname = reader.GetString(reader.GetOrdinal("patient_firstname")),
                                    PatientMiddlename = reader.IsDBNull(reader.GetOrdinal("patient_middlename"))
                                        ? null
                                        : reader.GetString(reader.GetOrdinal("patient_middlename")),
                                    PatientFirstsurname = reader.GetString(reader.GetOrdinal("patient_firstsurname")),
                                    PatientSecondlastname = reader.IsDBNull(reader.GetOrdinal("patient_secondlastname"))
                                        ? null
                                        : reader.GetString(reader.GetOrdinal("patient_secondlastname")),
                                    PatientGender = reader.IsDBNull(reader.GetOrdinal("patient_gender"))
                                        ? (int?)null
                                        : reader.GetInt32(reader.GetOrdinal("patient_gender")),
                                    PatientBirthdate = reader.IsDBNull(reader.GetOrdinal("patient_birthdate"))
                                        ? (DateOnly?)null
                                        : DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("patient_birthdate"))),
                                    PatientAge = reader.IsDBNull(reader.GetOrdinal("patient_age"))
                                        ? (int?)null
                                        : reader.GetInt32(reader.GetOrdinal("patient_age")),
                                    PatientBloodtype = reader.IsDBNull(reader.GetOrdinal("patient_bloodtype"))
                                        ? (int?)null
                                        : reader.GetInt32(reader.GetOrdinal("patient_bloodtype")),
                                    PatientDonor = reader.IsDBNull(reader.GetOrdinal("patient_donor"))
                                        ? null
                                        : reader.GetString(reader.GetOrdinal("patient_donor")),
                                    PatientMaritalstatus = reader.IsDBNull(reader.GetOrdinal("patient_maritalstatus"))
                                        ? (int?)null
                                        : reader.GetInt32(reader.GetOrdinal("patient_maritalstatus")),
                                    PatientVocationalTraining = reader.IsDBNull(reader.GetOrdinal("patient_vocational_training"))
                                        ? (int?)null
                                        : reader.GetInt32(reader.GetOrdinal("patient_vocational_training")),
                                    PatientLandlinePhone = reader.IsDBNull(reader.GetOrdinal("patient_landline_phone"))
                                        ? null
                                        : reader.GetString(reader.GetOrdinal("patient_landline_phone")),
                                    PatientCellularPhone = reader.IsDBNull(reader.GetOrdinal("patient_cellular_phone"))
                                        ? null
                                        : reader.GetString(reader.GetOrdinal("patient_cellular_phone")),
                                    PatientEmail = reader.GetString(reader.GetOrdinal("patient_email")),
                                    PatientNationality = reader.IsDBNull(reader.GetOrdinal("patient_nationality"))
                                        ? (int?)null
                                        : reader.GetInt32(reader.GetOrdinal("patient_nationality")),
                                    PatientProvince = reader.IsDBNull(reader.GetOrdinal("patient_province"))
                                        ? (int?)null
                                        : reader.GetInt32(reader.GetOrdinal("patient_province")),
                                    PatientAddress = reader.IsDBNull(reader.GetOrdinal("patient_address"))
                                        ? null
                                        : reader.GetString(reader.GetOrdinal("patient_address")),
                                    PatientOcupation = reader.IsDBNull(reader.GetOrdinal("patient_ocupation"))
                                        ? null
                                        : reader.GetString(reader.GetOrdinal("patient_ocupation")),
                                    PatientCompany = reader.IsDBNull(reader.GetOrdinal("patient_company"))
                                        ? null
                                        : reader.GetString(reader.GetOrdinal("patient_company")),
                                    PatientHealtInsurance = reader.IsDBNull(reader.GetOrdinal("patient_healt_insurance"))
                                        ? (int?)null
                                        : reader.GetInt32(reader.GetOrdinal("patient_healt_insurance")),
                                    PatientCode = reader.GetString(reader.GetOrdinal("patient_code")),
                                    PatientStatus = reader.GetInt32(reader.GetOrdinal("patient_status"))
                                };
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Manejo de errores, loguear el error si es necesario
                    throw new Exception("Error al obtener los detalles del paciente", ex);
                }
            }

            return patientDetails;
        }


        //Traes todos los daros del paciente 
        public async Task<DetailsPatientConsult> GetPatientFullByIdAsync(int patientId)
        {
            DetailsPatientConsult patient = null;

            try
            {
                using (var connection = new SqlConnection(_dbContext.Database.GetConnectionString()))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand("sp_GetPatientFullData", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Agregar el parámetro del ID del paciente
                        command.Parameters.Add(new SqlParameter("@PatientId", SqlDbType.Int) { Value = patientId });

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                // Mapear los datos del lector a un objeto de DetailsPatientConsult
                                patient = new DetailsPatientConsult
                                {
                                    PatientId = reader.GetInt32(reader.GetOrdinal("patient_id")),
                                    PatientCreationdate = reader.GetDateTime(reader.GetOrdinal("patient_creationdate")),
                                    PatientModificationdate = reader.GetDateTime(reader.GetOrdinal("patient_modificationdate")),
                                    PatientCreationuser = reader.GetInt32(reader.GetOrdinal("patient_creationuser")),
                                    PatientModificationuser = reader.GetInt32(reader.GetOrdinal("patient_modificationuser")),
                                    PatientDocumenttype = reader.GetInt32(reader.GetOrdinal("patient_documenttype")),
                                    PatientDocumentnumber = reader.IsDBNull(reader.GetOrdinal("patient_documentnumber")) ? null : reader.GetString(reader.GetOrdinal("patient_documentnumber")),
                                    PatientFirstname = reader.IsDBNull(reader.GetOrdinal("patient_firstname")) ? null : reader.GetString(reader.GetOrdinal("patient_firstname")),
                                    PatientMiddlename = reader.IsDBNull(reader.GetOrdinal("patient_middlename"))? null : reader.GetString(reader.GetOrdinal("patient_middlename")),
                                    PatientFirstsurname = reader.IsDBNull(reader.GetOrdinal("patient_firstsurname")) ? null : reader.GetString(reader.GetOrdinal("patient_firstsurname")),
                                    PatientSecondlastname = reader.IsDBNull(reader.GetOrdinal("patient_secondlastname")) ? null : reader.GetString(reader.GetOrdinal("patient_secondlastname")),
                                    PatientGender = reader.IsDBNull(reader.GetOrdinal("patient_gender")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("patient_gender")),
                                    PatientGenderName = reader.IsDBNull(reader.GetOrdinal("patient_gender_name")) ? null : reader.GetString(reader.GetOrdinal("patient_gender_name")),
                                    PatientBirthdate = reader.IsDBNull(reader.GetOrdinal("patient_birthdate")) ? (DateOnly?)null : DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("patient_birthdate"))),
                                    PatientAge = reader.GetInt32(reader.GetOrdinal("patient_age")),
                                    PatientBloodtype = reader.IsDBNull(reader.GetOrdinal("patient_bloodtype")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("patient_bloodtype")),
                                    PatientBloodtypeName = reader.IsDBNull(reader.GetOrdinal("patient_bloodtype_name")) ? null : reader.GetString(reader.GetOrdinal("patient_bloodtype_name")),
                                    PatientDonor = reader.IsDBNull(reader.GetOrdinal("patient_donor"))? null : reader.GetString(reader.GetOrdinal("patient_donor")),
                                    PatientMaritalstatus = reader.IsDBNull(reader.GetOrdinal("patient_maritalstatus")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("patient_maritalstatus")),
                                    PatientMaritalstatusName = reader.IsDBNull(reader.GetOrdinal("patient_maritalstatus_name")) ? null : reader.GetString(reader.GetOrdinal("patient_maritalstatus_name")),
                                    PatientVocationalTraining = reader.IsDBNull(reader.GetOrdinal("patient_vocational_training")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("patient_vocational_training")),
                                    PatientVocationalTrainingName = reader.IsDBNull(reader.GetOrdinal("patient_vocational_training_name")) ? null : reader.GetString(reader.GetOrdinal("patient_vocational_training_name")),
                                    PatientLandlinePhone = reader.IsDBNull(reader.GetOrdinal("patient_landline_phone")) ? null : reader.GetString(reader.GetOrdinal("patient_landline_phone")),
                                    PatientCellularPhone = reader.IsDBNull(reader.GetOrdinal("patient_cellular_phone")) ? null : reader.GetString(reader.GetOrdinal("patient_cellular_phone")),
                                    PatientEmail = reader.IsDBNull(reader.GetOrdinal("patient_email")) ? null : reader.GetString(reader.GetOrdinal("patient_email")),
                                    PatientNationality = reader.IsDBNull(reader.GetOrdinal("patient_nationality")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("patient_nationality")),
                                    PatientNationalityName = reader.IsDBNull(reader.GetOrdinal("patient_nationality_name")) ? null : reader.GetString(reader.GetOrdinal("patient_nationality_name")),
                                    PatientProvince = reader.IsDBNull(reader.GetOrdinal("patient_province")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("patient_province")),
                                    PatientProvinceName = reader.IsDBNull(reader.GetOrdinal("patient_province_name")) ? null : reader.GetString(reader.GetOrdinal("patient_province_name")),
                                    PatientAddress = reader.IsDBNull(reader.GetOrdinal("patient_address")) ? null : reader.GetString(reader.GetOrdinal("patient_address")),
                                    PatientOcupation = reader.IsDBNull(reader.GetOrdinal("patient_ocupation")) ? null : reader.GetString(reader.GetOrdinal("patient_ocupation")),
                                    PatientCompany = reader.IsDBNull(reader.GetOrdinal("patient_company")) ? null : reader.GetString(reader.GetOrdinal("patient_company")),
                                    PatientHealthInsurance = reader.IsDBNull(reader.GetOrdinal("patient_healt_insurance")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("patient_healt_insurance")),
                                    PatientHealthInsuranceName = reader.GetString(reader.GetOrdinal("patient_health_insurance_name")),
                                    PatientCode = reader.IsDBNull(reader.GetOrdinal("patient_code")) ? null : reader.GetString(reader.GetOrdinal("patient_code")),
                                    PatientStatus = reader.GetInt32(reader.GetOrdinal("patient_status"))
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error fetching patient data: {ex.Message}");
                // Manejo de errores (puedes agregar más log o re-throw la excepción si es necesario)
            }

            return patient;
        }



    }
}
