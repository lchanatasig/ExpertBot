
document.getElementById('selectDiagnosis').addEventListener('click', function () {
    const select = document.getElementById('DiagnosisConsultation');
    const selectedOption = select.options[select.selectedIndex];

    // Verificar que se haya seleccionado un diagnóstico
    if (selectedOption.value !== "") {
        const diagnosisId = selectedOption.value;
        const diagnosisName = selectedOption.getAttribute('data-name') || selectedOption.innerText; // Fallback al texto visible

        // Verificar si el data-name se obtiene correctamente
        console.log('Diagnosis ID:', diagnosisId);
        console.log('Diagnosis Name:', diagnosisName); // Verifica si el nombre se está recuperando correctamente

        // Crear una nueva fila en la tabla
        const tableBody = document.querySelector('#selectedDiagnosesTable tbody');
        const row = document.createElement('tr');

        // Crear las celdas de la fila
        const diagnosisIdCell = document.createElement('td');
        diagnosisIdCell.textContent = diagnosisId; // Mostrar el ID del diagnóstico
        diagnosisIdCell.hidden = true; // Ocultar la celda

        const diagnosisNameCell = document.createElement('td');
        diagnosisNameCell.textContent = diagnosisName; // Mostrar el nombre del diagnóstico

        const presumptiveCell = document.createElement('td');
        presumptiveCell.innerHTML = `
            <input type="checkbox" name="presumptive_${diagnosisId}" id="presumptive_${diagnosisId}">
        `;

        const definitiveCell = document.createElement('td');
        definitiveCell.innerHTML = `
            <input type="checkbox" name="definitive_${diagnosisId}" id="definitive_${diagnosisId}">
        `;

        const actionsCell = document.createElement('td');
        actionsCell.innerHTML = `
            <button type="button" class="btn btn-outline-danger btn-icon waves-effect waves-light" onclick="removeDiagnosisRow(this)">
                <i class="ri-delete-bin-5-line"></i>
            </button>
        `;

        // Añadir las celdas a la fila
        row.appendChild(diagnosisIdCell);
        row.appendChild(diagnosisNameCell);
        row.appendChild(presumptiveCell);
        row.appendChild(definitiveCell);
        row.appendChild(actionsCell);

        // Añadir la fila al cuerpo de la tabla
        tableBody.appendChild(row);
    }
});

// Función para eliminar la fila
function removeDiagnosisRow(button) {
    const row = button.closest('tr');
    row.remove();
}
document.getElementById('selectMedications').addEventListener('click', function () {
    const select = document.getElementById('MedicationsConsultation');
    const selectedOption = select.options[select.selectedIndex];

    // Verificar que se haya seleccionado un medicamento
    if (selectedOption.value !== "") {
        const medicationsId = selectedOption.value;
        const medicationsName = selectedOption.getAttribute('data-name') || selectedOption.innerText; // Fallback al texto visible

        // Verificar si el data-name se obtiene correctamente
        console.log('Medications ID:', medicationsId);
        console.log('Medications Name:', medicationsName); // Verifica si el nombre se está recuperando correctamente

        // Crear una nueva fila en la tabla
        const tableBody = document.querySelector('#selectedMedicationsTable tbody');
        const row = document.createElement('tr');

        // Crear las celdas de la fila
        const medicationsIdCell = document.createElement('td');
        medicationsIdCell.textContent = medicationsId; // Mostrar el ID del medicamento
        medicationsIdCell.hidden = true; // Ocultar la celda

        const medicationsNameCell = document.createElement('td');
        medicationsNameCell.textContent = medicationsName; // Mostrar el nombre del medicamento

        const amountCell = document.createElement('td');
        amountCell.innerHTML = `
            <input type="number" name="amount_${medicationsId}" id="amount_${medicationsId}" class="form-control" min="1">
        `; // Campo para la cantidad

        const observationCell = document.createElement('td');
        observationCell.innerHTML = `
            <input type="text" name="observation_${medicationsId}" id="observation_${medicationsId}" class="form-control">
        `; // Campo para observaciones

        const actionsCell = document.createElement('td');
        actionsCell.innerHTML = `
            <button type="button" class="btn btn-outline-danger btn-icon waves-effect waves-light" onclick="removeMedicationsRow(this)">
                <i class="ri-delete-bin-5-line"></i>
            </button>
        `; // Botón para eliminar la fila

        // Añadir las celdas a la fila
        row.appendChild(medicationsIdCell);
        row.appendChild(medicationsNameCell);
        row.appendChild(amountCell);
        row.appendChild(observationCell);
        row.appendChild(actionsCell);

        // Añadir la fila al cuerpo de la tabla
        tableBody.appendChild(row);
    }
});

// Función para eliminar la fila
function removeMedicationsRow(button) {
    const row = button.closest('tr');
    row.remove();
}

document.getElementById('selectImages').addEventListener('click', function () {
    const select = document.getElementById('ImagesConsultation');
    const selectedOption = select.options[select.selectedIndex];

    // Verificar que se haya seleccionado una imagen
    if (selectedOption.value !== "") {
        const imagesId = selectedOption.value;
        const imagesName = selectedOption.getAttribute('data-name') || selectedOption.innerText; // Fallback al texto visible

        // Verificar si el data-name se obtiene correctamente
        console.log('Images ID:', imagesId);
        console.log('Images Name:', imagesName); // Verifica si el nombre se está recuperando correctamente

        // Crear una nueva fila en la tabla
        const tableBody = document.querySelector('#selectedImagesTable tbody');
        const row = document.createElement('tr');

        // Crear las celdas de la fila
        const imagesIdCell = document.createElement('td');
        imagesIdCell.textContent = imagesId; // Mostrar el ID de la imagen
        imagesIdCell.hidden = true; // Ocultar la celda

        const imagesNameCell = document.createElement('td');
        imagesNameCell.textContent = imagesName; // Mostrar el nombre de la imagen

        const amountCell = document.createElement('td');
        amountCell.innerHTML = `
            <input type="number" name="amount_${imagesId}" id="amount_${imagesId}" class="form-control" min="1">
        `; // Campo para la cantidad

        const observationCell = document.createElement('td');
        observationCell.innerHTML = `
            <input type="text" name="observation_${imagesId}" id="observation_${imagesId}" class="form-control">
        `; // Campo para observaciones
        const actionsCell = document.createElement('td');
        actionsCell.innerHTML = `
    <button type="button" class="btn btn-outline-danger btn-icon waves-effect waves-light" onclick="removeImagesRow(this)">
        <i class="ri-delete-bin-5-line"></i>
    </button>
`;

        // Añadir las celdas a la fila
        row.appendChild(imagesIdCell);
        row.appendChild(imagesNameCell);
        row.appendChild(amountCell);
        row.appendChild(observationCell);
        row.appendChild(actionsCell);

        // Añadir la fila al cuerpo de la tabla
        tableBody.appendChild(row);
    }
});

// Función para eliminar la fila
function removeImagesRow(button) {
    const row = button.closest('tr');
    row.remove();
}


document.getElementById('selectLaboratories').addEventListener('click', function () {
    const select = document.getElementById('LaboratoriesConsultation');
    const selectedOption = select.options[select.selectedIndex];

    // Verificar que se haya seleccionado un laboratorio
    if (selectedOption.value !== "") {
        const laboratoriesId = selectedOption.value;
        const laboratoriesName = selectedOption.getAttribute('data-name') || selectedOption.innerText; // Fallback al texto visible

        // Verificar si el data-name se obtiene correctamente
        console.log('Laboratories ID:', laboratoriesId);
        console.log('Laboratories Name:', laboratoriesName); // Verifica si el nombre se está recuperando correctamente

        // Crear una nueva fila en la tabla
        const tableBody = document.querySelector('#selectedLaboratoriesTable tbody');
        const row = document.createElement('tr');

        // Crear las celdas de la fila
        const laboratoriesIdCell = document.createElement('td');
        laboratoriesIdCell.textContent = laboratoriesId; // Mostrar el ID del laboratorio
        laboratoriesIdCell.hidden = true; // Ocultar la celda

        const laboratoriesNameCell = document.createElement('td');
        laboratoriesNameCell.textContent = laboratoriesName; // Mostrar el nombre del laboratorio

        const amountCell = document.createElement('td');
        amountCell.innerHTML = `
            <input type="number" name="amount_${laboratoriesId}" id="amount_${laboratoriesId}" class="form-control" min="1">
        `; // Campo para la cantidad

        const observationCell = document.createElement('td');
        observationCell.innerHTML = `
            <input type="text" name="observation_${laboratoriesId}" id="observation_${laboratoriesId}" class="form-control">
        `; // Campo para observaciones

        const actionsCell = document.createElement('td');
        actionsCell.innerHTML = `
            <button type="button" class="btn btn-outline-danger btn-icon waves-effect waves-light" onclick="removeLaboratoriesRow(this)">
                <i class="ri-delete-bin-5-line"></i>
            </button>
        `; // Botón para eliminar la fila

        // Añadir las celdas a la fila
        row.appendChild(laboratoriesIdCell);
        row.appendChild(laboratoriesNameCell);
        row.appendChild(amountCell);
        row.appendChild(observationCell);
        row.appendChild(actionsCell);

        // Añadir la fila al cuerpo de la tabla
        tableBody.appendChild(row);
    }
});

// Función para eliminar la fila
function removeLaboratoriesRow(button) {
    const row = button.closest('tr');
    row.remove();
}



document.addEventListener("DOMContentLoaded", function () {
    const form = document.getElementById("yourFormId");

    form.addEventListener("submit", async function (event) {
        event.preventDefault(); // Evita el envío tradicional del formulario

        // Captura los valores de los campos necesarios
        const consultationData = {
            consultationCreationDate: document.getElementById("consultationCreationDate").value,
            consultationUserCreate: parseInt(document.getElementById("consultationUserCreate").value),
            consultationPatient: parseInt(document.getElementById("consultationPatient").value),
            consultationSpeciality: parseInt(document.getElementById("consultationSpeciality").value),
            consultationHistoryClinic: document.getElementById("consultationHistoryClinic").value,
            consultationReason: document.getElementById("consultationReason").value,
            consultationDisease: document.getElementById("consultationDisease").value,
            consultationFamiliaryName: document.getElementById("consultationFamiliaryName").value,
            consultationWarningSigns: document.getElementById("consultationWarningSigns").value,
            consultationNonPharmacological: document.getElementById("consultationNonPharmacological").value,
            consultationFamiliaryType: parseInt(document.getElementById("consultationFamiliaryType").value),
            consultationFamiliaryPhone: document.getElementById("consultationFamiliaryPhone").value,
            consultationTemperature: document.getElementById("consultationTemperature").value,
            consultationRespirationRate: document.getElementById("consultationRespirationRate").value,
            consultationBloodPressureAS: document.getElementById("consultationBloodPressureAS").value,
            consultationBloodPressureDIS: document.getElementById("consultationBloodPressureDIS").value,
            consultationPulse: document.getElementById("consultationPulse").value,
            consultationWeight: document.getElementById("consultationWeight").value,
            consultationSize: document.getElementById("consultationSize").value,
            consultationTreatmentPlan: document.getElementById("consultationTreatmentPlan").value,
            consultationObservation: document.getElementById("consultationObservation").value,
            consultationPersonalBackground: document.getElementById("consultationPersonalBackground").value,
            consultationDisabilityDays: parseInt(document.getElementById("consultationDisabilityDays").value),
            consultationType: parseInt(document.getElementById("consultationType").value),
            consultationStatus: parseInt(document.getElementById("consultationStatus").value),

            // Campos JSON capturados de los select múltiples
            DiagnosticsJson: getSelectedOptions("diagnosticsSelect"),
            AllergiesJson: getSelectedOptions("allergiesSelect"),
            ImagesJson: getSelectedOptions("imagesSelect"),
            LaboratoriesJson: getSelectedOptions("laboratoriesSelect"),
            MedicationsJson: getSelectedOptions("medicationsSelect"),
            SurgeriesJson: getSelectedOptions("surgeriesSelect"),
            FamiliaryBackgroundJson: getFamiliaryBackgroundData(), // Aquí agregamos los datos familiares
            OrgansSystemsJson: getSelectedOptions("organsSystemsSelect"),
            PhysicalExaminationJson: getSelectedOptions("physicalExaminationSelect")
        };

        // Enviar al backend con fetch
        try {
            const response = await fetch("/api/consultations", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(consultationData)
            });

            if (response.ok) {
                const result = await response.json();
                alert("Consulta creada exitosamente");
                console.log(result);
            } else {
                console.error("Error en la creación de la consulta", await response.text());
                alert("Hubo un error al crear la consulta");
            }
        } catch (error) {
            console.error("Error en el fetch", error);
            alert("Error inesperado");
        }
    });

    /**
     * Función para capturar las opciones seleccionadas de un select múltiple.
     */
    function getSelectedOptions(selectId) {
        const select = document.getElementById(selectId);
        const selectedOptions = Array.from(select.selectedOptions).map(option => option.value);
        return JSON.stringify(selectedOptions); // Devuelve un string JSON
    }

    /**
     * Función para capturar los datos de antecedentes familiares (checkbox y select).
     */
    function getFamiliaryBackgroundData() {
        const familyBackgroundData = {
            heartDisease: getSelectData("heartDiseaseSelect"),
            diabetes: getSelectData("diabetesSelect"),
            cardiovascularDisease: getSelectData("cardiovascularSelect"),
            hypertension: getSelectData("hypertensionSelect"),
            cancer: getSelectData("cancerSelect"),
            tuberculosis: getSelectData("tuberculosisSelect"),
            mentalIllness: getSelectData("mentalIllnessSelect"),
            infectiousDisease: getSelectData("infectiousDiseaseSelect"),
            malformation: getSelectData("malformationSelect"),
            other: getSelectData("otherSelect")
        };
        return JSON.stringify(familyBackgroundData); // Devuelve un string JSON
    }

    /**
     * Captura los datos de los select relacionados con las enfermedades familiares.
     */
    function getSelectData(selectId) {
        const select = document.getElementById(selectId);
        const selectedValue = select ? select.value : "";
        return selectedValue;
    }
});


//Speech
var recognition;
var recognizing = false;

function toggleDictation(textareaId, iconId) {
    if (recognizing) {
        stopDictation(iconId);
    } else {
        startDictation(textareaId, iconId);
    }
}

function startDictation(textareaId, iconId) {
    if (window.hasOwnProperty('webkitSpeechRecognition')) {
        recognition = new webkitSpeechRecognition();

        recognition.continuous = true; // Permite que la grabación sea continua
        recognition.interimResults = false;

        recognition.lang = "es-ES"; // Cambia el idioma según sea necesario

        recognition.onstart = function () {
            recognizing = true;
            updateIconState(iconId);
            console.log("Reconocimiento de voz iniciado. Por favor, hable.");
        };

        recognition.onresult = function (event) {
            const newText = event.results[event.results.length - 1][0].transcript;
            document.getElementById(textareaId).value += ' ' + newText; // Concatena al texto existente
        };

        recognition.onerror = function (event) {
            console.error("Error en el reconocimiento de voz: ", event.error);
        };

        recognition.onend = function () {
            recognizing = false;
            updateIconState(iconId);
            console.log("El reconocimiento de voz ha finalizado.");
        };

        recognition.start();
    } else {
        alert("Tu navegador no soporta el reconocimiento de voz.");
    }
}

function stopDictation(iconId) {
    if (recognition && recognizing) {
        recognizing = false;
        recognition.stop();
        updateIconState(iconId);
        console.log("Reconocimiento de voz detenido.");
    }
}

function updateIconState(iconId) {
    var icon = document.getElementById(iconId);

    if (recognizing) {
        icon.classList.remove('ri-mic-fill');
        icon.classList.add('ri-mic-off-fill');
    } else {
        icon.classList.remove('ri-mic-off-fill');
        icon.classList.add('ri-mic-fill');
    }
}

// Asignar eventos de clic a los iconos
document.getElementById('dictationIcon1').addEventListener('click', function () {
    toggleDictation('personalBackground', 'dictationIcon1');
});

document.getElementById('dictationIcon2').addEventListener('click', function () {
    toggleDictation('currentillness', 'dictationIcon2');
});

document.getElementById('dictationIcon3').addEventListener('click', function () {
    toggleDictation('treatmentPlan', 'dictationIcon3');
});

document.getElementById('dictationIcon4').addEventListener('click', function () {
    toggleDictation('nonpharmacological', 'dictationIcon4');
});

document.getElementById('dictationIcon5').addEventListener('click', function () {
    toggleDictation('alarmsigns', 'dictationIcon5');
});
// Asignar eventos de clic al icono
document.getElementById('dictationIcon6').addEventListener('click', function () {
    toggleDictation('observation', 'dictationIcon6');
});



//Preecion Arterial
document.getElementById('bloodPressureInput').addEventListener('input', function (e) {
    let value = e.target.value.replace(/\D/g, ''); // Elimina cualquier caracter que no sea un dígito

    if (value.length > 3) {
        value = value.slice(0, 3) + '/' + value.slice(3); // Inserta el '/'
    }

    e.target.value = value; // Actualiza el campo de entrada con el nuevo valor

    // Opcional: Si deseas actualizar los campos ocultos para diastólica y sistólica
    if (value.length >= 5) {
        document.getElementById('consultation_bloodpresuredDIS').value = value.slice(4, 6);
        document.getElementById('consultation_bloodpressuredAS').value = value.slice(0, 3);
    } else {
        document.getElementById('consultation_bloodpresuredDIS').value = '';
        document.getElementById('consultation_bloodpressuredAS').value = value.slice(0, 3);
    }
});



document.getElementById('consultationForm').addEventListener('submit', async function (event) {
    event.preventDefault(); // Evitar el comportamiento por defecto del formulario

    // Parámetros de consulta
    const consultaDto = {
        ConsultationUsercreate: document.getElementById('consultation_usercreate')?.value || null,
        HistorialConsulta: document.getElementById('historiaClinica')?.value || null,
        PacienteId: parseInt(document.getElementById('idPaciente')?.value) || null,
        MotivoConsulta: document.getElementById('motivoConsulta')?.value || null,
        EnfermedadConsulta: document.getElementById('enfermedadProblema')?.value || null,
        NombrePariente: document.getElementById('acompañante')?.value || 'Sin especificar',
        SignosAlarma: document.getElementById('signosAlarma')?.value || 'Sin especificar',
        ReconocimientoFarmacologico: document.getElementById('reconofarmacologicas')?.value || 'Sin especificar',
        TipoPariente: parseInt(document.getElementById('tipoParienteSelect')?.value) || null,
        TelefonoPariente: document.getElementById('telefonoPariente')?.value || 'Sin especificar',
        Temperatura: document.getElementById('temperatura_consulta')?.value || null,
        FrecuenciaRespiratoria: document.getElementById('frecuenciarespiratoria_consulta')?.value || null,
        PresionArterialSistolica: document.getElementById('PresionarterialsistolicaConsulta')?.value || null,
        PresionArterialDiastolica: document.getElementById('PresionarterialdiastolicaConsulta')?.value || null,
        Pulso: document.getElementById('pulso_consulta')?.value || null,
        Peso: document.getElementById('peso_consulta')?.value || null,
        Talla: document.getElementById('talla_consulta')?.value || null,
        PlanTratamiento: document.getElementById('plantratamiento_consulta')?.value || null,
        DiasIncapacidad: parseInt(document.getElementById('diasincapacidad_consulta')?.value) || 0,
        Observacion: document.getElementById('observacion_consulta')?.value || null,
        AntecedentesPersonales: document.getElementById('antecedentespersonales_consulta')?.value || 'Sin Especificar',
        MedicoId: parseInt(document.getElementById('medicoId')?.value) || null,
        EspecialidadId: parseInt(document.getElementById('especialidadId')?.value) || null,
        TipoConsultaId: parseInt(document.getElementById('tipoConsultaC')?.value) || null,
        NotasEvolucion: document.getElementById('notasevolucion_consulta')?.value || 'Sin especificar',
        ConsultaPrincipal: document.getElementById('consultaprincipal_consulta')?.value || null,
        EstadoConsulta: parseInt(document.getElementById('estadoConsultaC')?.value) || null,

        // Parámetros de órganos y sistemas
        OrganosSistemas: {
            OrgSentidos: document.getElementById('consulta-antecedente-checked-orgsentidos')?.checked || null,
            ObserOrgSentidos: document.getElementById('consulta-antecedente-observacion-orgsentidos')?.value || null,
            Respiratorio: document.getElementById('consulta-antecedente-checked-respiratorio')?.checked || null,
            ObserRespiratorio: document.getElementById('consulta-antecedente-observacion-respiratorio')?.value || null,
            CardioVascular: document.getElementById('consulta-antecedente-checked-cardiovascular')?.checked || null,
            ObserCardioVascular: document.getElementById('consulta-antecedente-observacion-cardiovascular')?.value || null,
            Digestivo: document.getElementById('consulta-antecedente-checked-digestivo')?.checked || null,
            ObserDigestivo: document.getElementById('consulta-antecedente-observacion-digestivo')?.value || null,
            Genital: document.getElementById('consulta-antecedente-checked-genital')?.checked || null,
            ObserGenital: document.getElementById('consulta-antecedente-observacion-genital')?.value || null,
            Urinario: document.getElementById('consulta-antecedente-checked-urinario')?.checked || null,
            ObserUrinario: document.getElementById('consulta-antecedente-observacion-urinario')?.value || null,
            MEsqueletico: document.getElementById('consulta-antecedente-checked-mesqueletico')?.checked || null,
            ObserMEsqueletico: document.getElementById('consulta-antecedente-observacion-mesqueletico')?.value || null,
            Endocrino: document.getElementById('consulta-antecedente-checked-endocrino')?.checked || null,
            ObserEndocrino: document.getElementById('consulta-antecedente-observacion-endocrino')?.value || null,
            Linfatico: document.getElementById('consulta-antecedente-checked-linfatico')?.checked || null,
            ObserLinfatico: document.getElementById('consulta-antecedente-observacion-linfatico')?.value || null,
            Nervioso: document.getElementById('consulta-antecedente-checked-nervioso')?.checked || null,
            ObserNervioso: document.getElementById('consulta-antecedente-observacion-nervioso')?.value || null
        },

        // Parámetros de examen físico
        ExamenFisico: {
            Cabeza: document.getElementById('consulta-antecedente-checked-cabeza')?.checked || null,
            ObserCabeza: document.getElementById('consulta-antecedente-observacion-cabeza')?.value || null,
            Cuello: document.getElementById('consulta-antecedente-checked-cuello')?.checked || null,
            ObserCuello: document.getElementById('consulta-antecedente-observacion-cuello')?.value || null,
            Torax: document.getElementById('consulta-antecedente-checked-torax')?.checked || null,
            ObserTorax: document.getElementById('consulta-antecedente-observacion-torax')?.value || null,
            Abdomen: document.getElementById('consulta-antecedente-checked-abdomen')?.checked || null,
            ObserAbdomen: document.getElementById('consulta-antecedente-observacion-abdomen')?.value || null,
            Pelvis: document.getElementById('consulta-antecedente-checked-pelvis')?.checked || null,
            ObserPelvis: document.getElementById('consulta-antecedente-observacion-pelvis')?.value || null,
            Extremidades: document.getElementById('consulta-antecedente-checked-extremidades')?.checked || null,
            ObserExtremidades: document.getElementById('consulta-antecedente-observacion-extremidades')?.value || null
        },

        // Parámetros de antecedentes familiares
        AntecedentesFamiliares: {
            Cardiopatia: document.getElementById('consulta-antecedente-checked-cardiopatia')?.checked || null,
            ObserCardiopatia: document.getElementById('consulta-observacion-cardiopatias')?.value || null,
            ParentescocatalogoCardiopatia: document.getElementById('tipoDocumentoSelectCardiopatia')?.value ? parseInt(document.getElementById('tipoDocumentoSelectCardiopatia')?.value) : null,

            Diabetes: document.getElementById('consulta-antecedente-checked-diabetes')?.checked || null,
            ObserDiabetes: document.getElementById('consulta-antecedente-observacion-diabetes')?.value || null,
            ParentescocatalogoDiabetes: document.getElementById('tipoDocumentoSelectDiabetes')?.value ? parseInt(document.getElementById('tipoDocumentoSelectDiabetes')?.value) : null,

            EnfCardiovascular: document.getElementById('consulta-antecedente-checked-enfcardiovascular')?.checked || null,
            ObserEnfCardiovascular: document.getElementById('consulta-antecedente-observacion-enfcardiovascular')?.value || null,
            ParentescocatalogoEnfCardiovascular: document.getElementById('tipoDocumentoSelectEnfCardiovascular')?.value ? parseInt(document.getElementById('tipoDocumentoSelectEnfCardiovascular')?.value) : null,

            Hipertension: document.getElementById('consulta-antecedente-checked-hipertension')?.checked || null,
            ObserHipertension: document.getElementById('consulta-antecedente-observacion-hipertension')?.value || null,
            ParentescocatalogoHipertension: document.getElementById('tipoDocumentoSelectHipertension')?.value ? parseInt(document.getElementById('tipoDocumentoSelectHipertension')?.value) : null,

            Cancer: document.getElementById('consulta-antecedente-checked-cancer')?.checked || null,
            ObserCancer: document.getElementById('consulta-antecedente-observacion-cancer')?.value || null,
            ParentescocatalogoCancer: document.getElementById('tipoDocumentoSelectCancer')?.value ? parseInt(document.getElementById('tipoDocumentoSelectCancer')?.value) : null,

            Tuberculosis: document.getElementById('consulta-antecedente-checked-tuberculosis')?.checked || null,
            ObserTuberculosis: document.getElementById('consulta-antecedente-observacion-tuberculosis')?.value || null,
            ParentescocatalogoTuberculosis: document.getElementById('tipoDocumentoSelectTuberculosis')?.value ? parseInt(document.getElementById('id="tipoDocumentoSelectTuberculosis"')?.value) : null,

            EnfMental: document.getElementById('consulta-antecedente-checked-enfmental')?.checked || null,
            ObserEnfMental: document.getElementById('consulta-antecedente-observacion-enfmental')?.value || null,
            ParentescocatalogoEnfMental: document.getElementById('tipoDocumentoSelectEnfMental')?.value ? parseInt(document.getElementById('tipoDocumentoSelectEnfMental')?.value) : null,

            EnfInfecciosa: document.getElementById('consulta-antecedente-checked-enfinfecciosa')?.checked || null,
            ObserEnfInfecciosa: document.getElementById('consulta-antecedente-observacion-enfinfecciosa')?.value || null,
            ParentescocatalogoEnfInfecciosa: document.getElementById('tipoDocumentoSelectEnfInfecciosa')?.value ? parseInt(document.getElementById('tipoDocumentoSelectEnfInfecciosa')?.value) : null,

            MalFormacion: document.getElementById('consulta-antecedente-checked-malformacion')?.checked || null,
            ObserMalFormacion: document.getElementById('consulta-antecedente-observacion-malformacion')?.value || null,
            ParentescocatalogoMalFormacion: document.getElementById('tipoDocumentoSelectMalFormacion')?.value ? parseInt(document.getElementById('tipoDocumentoSelectMalFormacion')?.value) : null,

            Otro: document.getElementById('consulta-antecedente-checked-otro')?.checked || null,
            ObserOtro: document.getElementById('consulta-antecedente-observacion-otro')?.value || null,
            ParentescocatalogoOtro: document.getElementById('tipoDocumentoSelectOtro')?.value ? parseInt(document.getElementById('tipoDocumentoSelectOtro')?.value) : null
        },

        // Tablas relacionadas (Arrays de objetos)
        Alergias: Array.from(document.querySelectorAll('#hiddenAlergiaInputsContainer input')).map(input => ({
            CatalogoalergiaId: parseInt(input.value, 10),
            ObservacionAlergias: "",
            EstadoAlergias: 1
        })),
        Cirugias: Array.from(document.querySelectorAll('#hiddenCirugiaInputsContainer input')).map(input => ({
            CatalogocirugiaId: parseInt(input.value, 10),
            ObservacionCirugia: "",
            EstadoCirugias: 1
        })),
        Medicamentos: Array.from(document.querySelectorAll('#medicamentosTableBody tr')).map(tr => ({
            MedicamentoId: parseInt(tr.querySelector('.medicamento-id')?.value || 0),
            DosisMedicamento: tr.querySelector('.dosis-medicamento')?.value || null,
            ObservacionMedicamento: tr.querySelector('.observacion-medicamento')?.value || null,
            SecuencialMedicamento: null,
            EstadoMedicamento: 1
        })),

        Laboratorios: Array.from(document.querySelectorAll('#laboratorioTableBody tr')).map(tr => ({
            CatalogoLaboratorioId: parseInt(tr.querySelector('.laboratorio-id')?.value || 0),
            CantidadLaboratorio: parseInt(tr.querySelector('.cantidad')?.value || 0),
            Observacion: tr.querySelector('.observacion')?.value || null,
            SecuencialLaboratorio: null,
            EstadoLaboratorio: 1
        })),

        Imagenes: Array.from(document.querySelectorAll('#imagenesTableBody tr')).map(tr => ({
            ImagenId: parseInt(tr.querySelector('.imagen-id')?.value || 0),
            ObservacionImagen: tr.querySelector('.observacion-imagen')?.value || null,
            CantidadImagen: parseInt(tr.querySelector('.cantidad-imagen')?.value || 0),
            SecuencialImagen: null,
            EstadoImagen: 1
        })),

        Diagnosticos: Array.from(document.querySelectorAll('#diagnosticoTableBody tr')).map(tr => ({
            DiagnosticoId: parseInt(tr.querySelector('.diagnostico-id')?.value || 0),
            ObservacionDiagnostico: tr.querySelector('.observacion-diagnostico')?.value || null,
            PresuntivoDiagnosticos: tr.querySelector('.presuntivo-diagnostico')?.checked || null,
            DefinitivoDiagnosticos: tr.querySelector('.definitivo-diagnostico')?.checked || null,
            SecuencialDiagnostico: null,
            EstadoDiagnostico: 1
        }))
    };

    // Muestra el JSON generado en la consola para debug
    console.log("JSON generado para consulta:", JSON.stringify(consultaDto));
    console.log("Parámetros de antecedentes familiares:", consultaDto.AntecedentesFamiliares);

    try {
        const response = await fetch(consultaUrl, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(consultaDto)
        });

        const result = await response.json();

        if (response.ok) {
            console.log('Consulta creada exitosamente:', result);
            window.location.href = redirectur; // Cambia esta URL si es diferente

        } else {
            console.error('Error al crear la consulta:', result);
        }
    } catch (error) {
        console.error('Error de la solicitud:', error);
    }
});