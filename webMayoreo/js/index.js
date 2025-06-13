document.addEventListener("DOMContentLoaded", function () {
    const token = localStorage.getItem('authToken');
// Verificar si hay token
if (!token) {
    // Si no hay token, redirigir al login
    window.location.href = 'login.html';
} else {
    const datos = JSON.parse(sessionStorage.getItem('navegacionDatos')) || {};
    const calendar = document.getElementById('calendar');
    const fechaUnitaria = datos.fechaUnitaria || calendar.value;
    const idGerente = datos.idGerente || '0';
        // Establecer fecha inicial
        calendar.value = fechaUnitaria;
        getSelectedDate(); // Mostrar la fecha inicial
    Gerentes(idGerente);
    Cotizaciones(idGerente, fechaUnitaria);
}

    // ValidarToken();
    const select = document.getElementById('gerentes');
    async function Gerentes(idGerente) {

        try {
            const token = localStorage.getItem('authToken');
            //console.log('Token:', token);
            if (!token) {
                alert('Token no encontrado. Inicie sesión nuevamente.');
                return;
            }
            //const response = await fetch('http://localhost:8080/api/ReporteWeb/Gerentes', {
            const response = await fetch('http://34.29.199.7:8080/api/ReporteWeb/Gerentes', {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                }
            });
           // console.log('Response status:', response.status);
    
            if (!response.ok) {
                throw new Error(`Error en la solicitud: ${response.status}`);
            }    
            const data = await response.json();
            console.log('Respuesta de la API:', data);
    
            if (data.status === 'success' && data.data && Array.isArray(data.data.gerentes)) {
                const gerentes = data.data.gerentes;
                const select = document.getElementById('gerentes');
    
                if (!select) {
                    console.error('No se encontró el elemento con id "gerentes"');
                    return;
                }
    
                gerentes.forEach(item => {
                    const { gerente } = item;
                
                    const option = document.createElement('option');
                    option.value = gerente.idGerente;
                    option.textContent = `${gerente.nombre} ${gerente.apellidoP} (nEmpleado: ${gerente.numEmpleado})`;
                
                    // Suponiendo que tienes el idGerente deseado en una variable, por ejemplo, idGerenteSeleccionado
                    if (gerente.idGerente === idGerente) {
                        option.selected = true;
                    }
                
                    select.appendChild(option);
                });
            } else {
                //localStorage.setItem('authToken', '');
                alert('Error al obtener las sucursales: ' + (data.message || 'Error desconocido'));
                 // Si no hay token, redirigir al login
                window.location.href = 'login.html';
            }
        } catch (error) {
            //localStorage.setItem('authToken', '');
            alert('Ocurrió un error al obtener las sucursales');
            window.location.href = 'login.html';
        }
    } 


// consultar cotizaciones por dia

async function Cotizaciones(idGerente, Fecha) {
    try {
        const token = localStorage.getItem('authToken');
        if (!token) {
            alert('Token no encontrado. Inicie sesión nuevamente.');
            return;
        }
        //const response = await fetch(`http://localhost:8080/api/ReporteWeb/Cotizacion?idGerente=${idGerente}&Fecha=${Fecha}`, {
        const response = await fetch(`http://34.29.199.7:8080/api/ReporteWeb/Cotizacion?idGerente=${idGerente}&Fecha=${Fecha}`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            }
        });

        if (!response.ok) {
            throw new Error(`Error en la solicitud: ${response.status}`);
        }

        const data = await response.json();
        console.log('Respuesta de la API:', data);

        if (data.status === 'success' && data.data && Array.isArray(data.data.cotizaciones)) {
            const cotizaciones = data.data.cotizaciones;
            const tbody = document.getElementById('Cotizaciones');

            if (!tbody) {
                console.error('No se encontró el elemento con id "Cotizaciones"');
                return;
            }
            // Limpiar el contenido existente del tbody
            tbody.innerHTML = '';
            // Agrega la hora para que el constructor Date use hora local
            const calendar = document.getElementById('calendar');
            const selectedDate = calendar.value;

            if (cotizaciones.length === 0) {
                const row = document.createElement('tr');
                row.innerHTML = `
                    <td colspan="8" class="text-center">No existen cotizaciones registradas.</td>
                `;
                tbody.appendChild(row);
            } else {
                cotizaciones.forEach((item, index) => {
                    const row = document.createElement('tr');
                
                    // Celdas de texto
                    row.innerHTML = `
                        <th scope="row">${index + 1}</th>
                        <td>${item.gerente.sucursal}</td>
                        <td>${item.gerente.nombre}</td>
                        <td>${item.gerente.apellidoM || ''}</td>
                        <td>${item.gerente.status || 'N/A'}</td>
                        <td>${item.gerente.totalCotizacion || 'N/A'}</td>
                        <td>${item.gerente.idClienteSAP || 'N/A'}</td>
                        <td>${item.gerente.idventa || 'N/A'}</td>
                    `;
                if (item.gerente.tipo_consulta === '1') {
                    // Celda para el botón
                    const tdBtn = document.createElement('td');
                    const btn = document.createElement('button');
                    btn.className = 'btn btn-primary';
                    btn.setAttribute('data-toggle', 'modal');
                    btn.setAttribute('data-target', '#myModal');
                    btn.textContent = 'Ver Visitas';

                     // Evento click para el botón
    btn.addEventListener('click', () => {
        irAVisitador(
            item.gerente.claveSucursal,
            item.gerente.idDispositivo,
            selectedDate,
            idGerente
        );
    });

    tdBtn.appendChild(btn);
    row.appendChild(tdBtn);
}else{

                        // Celda para el botón
                        const tdBtnC = document.createElement('td');
                        const btnC = document.createElement('button');
                        btnC.className = 'btn btn-primary';
                        btnC.setAttribute('data-toggle', 'modal');
                        btnC.setAttribute('data-target', '#myModal');
                        btnC.textContent = 'Ver Consultas';
    
                         // Evento click para el botón
        btnC.addEventListener('click', () => {
            irAConsultas(
                item.gerente.claveSucursal,
                item.gerente.idDispositivo,
                selectedDate,
                idGerente
            );
        });
    
        tdBtnC.appendChild(btnC);
        row.appendChild(tdBtnC);
                   
                }

    // Agregar la celda de la productosConsulta

 // Celda para el botón
                        const tdBtnD = document.createElement('td');
                        const btnD = document.createElement('button');
                        btnD.className = 'btn btn-primary';
                        btnD.setAttribute('data-toggle', 'modal');
                        btnD.setAttribute('data-target', '#myModal');
                        btnD.textContent = 'Productos';
    
                         // Evento click para el botón
        btnD.addEventListener('click', () => {
            irAProductoConsultas(
                item.gerente.claveSucursal,
                item.gerente.idDispositivo,
                selectedDate,
                idGerente
            );
        });
    
        tdBtnD.appendChild(btnD);
        row.appendChild(tdBtnD);
                tbody.appendChild(row);

            

            });          
            }
            
        } else {
            alert('Error al obtener las sucursales: ' + (data.message || 'Error desconocido'));
        }
    } catch (error) {
        //alert('Ocurrió un error al obtener las Cotizaciones'); <td><a href="Visitador.html" onclick="${irAVisitador(item.gerente.claveSucursal,item.gerente.idDispositivo,selectedDate,idGerente)}">Ruta</a></td>
    }
}

    // Qué irónica es la vida cuando dejas de esperar, es cuando realmente empiezas a recibir todo

    function irAVisitador(claveSucursal, numeroEmpleado,fechaUnitaria, idGerente) {
        const datos = {
            claveSucursal: claveSucursal,
            numeroEmpleado: numeroEmpleado,
            fechaUnitaria: fechaUnitaria,
            idGerente: idGerente
        };  
        // Guardar en sessionStorage
        sessionStorage.setItem('navegacionDatos', JSON.stringify(datos));
        window.location.href = 'visitador.html';
        //esperarYMostrarRuta();
    }

    function irAConsultas(claveSucursal, numeroEmpleado,fechaUnitaria, idGerente) {
        const datos = {
            claveSucursal: claveSucursal,
            numeroEmpleado: numeroEmpleado,
            fechaUnitaria: fechaUnitaria,
            idGerente: idGerente
        };  
        // Guardar en sessionStorage
        sessionStorage.setItem('navegacionDatos', JSON.stringify(datos));
        window.location.href = 'Consultas.html';
        //esperarYMostrarRuta();
    }

    function irAProductoConsultas(claveSucursal, numeroEmpleado,fechaUnitaria, idGerente) {
        const datos = {
            claveSucursal: claveSucursal,
            numeroEmpleado: numeroEmpleado,
            fechaUnitaria: fechaUnitaria,
            idGerente: idGerente
        };  
        // Guardar en sessionStorage
        sessionStorage.setItem('navegacionDatos', JSON.stringify(datos));
        const datosProducto = {
            idUsuario: numeroEmpleado,
            Fecha: fechaUnitaria
        };  
        // Guardar en sessionStorage
        sessionStorage.setItem('consultasProductos', JSON.stringify(datosProducto));
        window.location.href = 'ProductosConsulta.html';
        //esperarYMostrarRuta();
    }

// Call the function
//Cotizaciones(1670, "2025-04-03");

// Función para obtener y mostrar la fecha seleccionada
function getSelectedDate() {
    const calendar = document.getElementById('calendar');
    const dateDisplay = document.getElementById('selected-date');
    const idGerente = document.getElementById('gerentes').value; // Valor por defecto 1620
    
    const selectedDate = calendar.value;
    if (selectedDate) {
        // Agrega la hora para que el constructor Date use hora local
        const date = new Date(selectedDate + 'T00:00:00');
    
        const formattedDate = date.toLocaleDateString('es-ES', {
            weekday: 'long',
            year: 'numeric',
            month: 'long',
            day: 'numeric'
        });
    
        dateDisplay.textContent = `Fecha seleccionada: ${formattedDate}`;
        dateDisplay.className = 'alert alert-success';
        
        // Llamar a Cotizaciones con la fecha seleccionada
       Cotizaciones(idGerente, selectedDate);
        
    } else {
        dateDisplay.textContent = 'Por favor selecciona una fecha';
        dateDisplay.className = 'alert alert-info';
    }
}
    // Escuchar cambios en el calendario
    calendar.addEventListener('change', function() {
        getSelectedDate(); // Actualizar con la nueva fecha seleccionada
    });

    select.addEventListener('change', function () {
        getSelectedDate();
    });

           // Asociar la función al botón
           const botonRegresar = document.getElementById("botonCerrarSesion");
           if (botonRegresar) {
               botonRegresar.addEventListener("click", function () {
                localStorage.removeItem('authToken');
                localStorage.removeItem('userData');
                localStorage.removeItem('idRol');
                window.location.href = 'login.html';
               });
           } else {
               console.error("El botón con id 'botonRegresar' no se encontró.");
           }
    
});



