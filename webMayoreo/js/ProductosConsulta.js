document.addEventListener("DOMContentLoaded", function () {
    const token = localStorage.getItem('authToken');
// Verificar si hay token
if (!token) {
    // Si no hay token, redirigir al login
    window.location.href = 'login.html';
} else {
    const datos = JSON.parse(sessionStorage.getItem('consultasProductos')) || {};
    const idUsuario = datos.idUsuario;
    const Fecha = datos.Fecha;
    Consultas(idUsuario, Fecha);
}

       // Asociar la función al botón
       const botonRegresar = document.getElementById("botonRegresar");
       if (botonRegresar) {
           botonRegresar.addEventListener("click", function () {
                   window.location.href = `index.html`;
           });
       } else {
           console.error("El botón con id 'botonRegresar' no se encontró.");
       }
// consultar cotizaciones por dia
async function  Consultas(idUsuario, Fecha) {
    try {
        const token = localStorage.getItem('authToken');
        if (!token) {
            alert('Token no encontrado. Inicie sesión nuevamente.');
            return;
        }
        //const response = await fetch(`http://localhost:8080/api/ReporteWeb/ConsultasClientes?idUsuario=${idUsuario}&Fecha=${Fecha}`, {
        const response = await fetch(`http://34.29.199.7:8080/api/ReporteWeb/ConsultasClientes?idUsuario=${idUsuario}&Fecha=${Fecha}`, {
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

        if (data.status === 'success' && data.data && Array.isArray(data.data.consultas)) {
            const consultas = data.data.consultas;
            const tbody = document.getElementById('Cotizaciones');

            if (!tbody) {
                console.error('No se encontró el elemento con id "Cotizaciones"');
                return;
            }
            // Limpiar el contenido existente del tbody
            tbody.innerHTML = '';

            if (consultas.length === 0) {
                const row = document.createElement('tr');
                row.innerHTML = `
                    <td colspan="8" class="text-center">No existen cotizaciones registradas.</td>
                `;
                tbody.appendChild(row);
            } else {
                consultas.forEach((item, index) => {
                    const row = document.createElement('tr');
                
                    // Celdas de texto
                    row.innerHTML = `
                        <th scope="row">${index + 1}</th>
                        <td>${item.consulta.numCliente}</td>
                        <td>${item.consulta.idSAP || 'N/A'}</td>
                        <td>${item.consulta.claveArticulo || 'N/A'}</td>
                        <td>${item.consulta.vecesConsultado || 'N/A'}</td>
                    `;            
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
    
});

