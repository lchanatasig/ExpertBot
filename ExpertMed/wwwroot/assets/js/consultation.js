
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


