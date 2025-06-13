document.addEventListener("DOMContentLoaded", function () {
    const token = localStorage.getItem('authToken');
// Verificar si hay token
if (!token) {
    // Si no hay token, redirigir al login
    window.location.href = 'login.html';
} else {
    
       // Asociar la función al botón
       const botonRegresar = document.getElementById("botonRegresar");
       if (botonRegresar) {
           botonRegresar.addEventListener("click", function () {
            const idRol = localStorage.getItem('idRol');
            if (idRol === '10') {
              window.location.href = `Gerente.html`;
            } else {
              window.location.href = `index.html`;
            }
           });
       } else {
           console.error("El botón con id 'botonRegresar' no se encontró.");
       }

        // Asociar la función al botón
        const botonConsultar = document.getElementById("ConsultarCliente");
        if (botonConsultar) {
            botonConsultar.addEventListener("click", function () {
                Reporte()   
            });
        } else {
            console.error("El botón con id 'ConsultarCliente' no se encontró.");
        }

// consultar cotizaciones por dia
async function Reporte() {
    // Obtener el valor del input
    const claveCliente = document.getElementById('claveCliente').value;
// Aquí puedes usar el valor de claveCliente
console.log('Clave Cliente:', claveCliente);
    try {
        const token = localStorage.getItem('authToken');
        if (!token) {
            alert('Token no encontrado. Inicie sesión nuevamente.');
            return;
        }
        //const response = await fetch(`http://localhost:8080/api/Cliente/HistoricoVtaMay?ClienteSap=${claveCliente}`, {
        const response = await fetch(`http://34.29.199.7:8080/api/Cliente/HistoricoVtaMay?ClienteSap=${claveCliente}`, {
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

        if (data.status === 'success' && data.data && Array.isArray(data.data.product)) {
            const reporte = data.data.product;
            const tbody = document.getElementById('Cotizaciones');
            const paginationContainer = document.getElementById('pagination');
            const searchInput = document.getElementById('searchInput');
            const totalImporte = document.getElementById('totalImporte');
            const totalCantidad = document.getElementById('totalCantidad');
        
            if (!tbody || !paginationContainer || !searchInput || !totalImporte || !totalCantidad) {
              console.error('No se encontraron los elementos necesarios');
              return;
            }
        
            // Configuración de paginación
            const rowsPerPage = 10;
            let currentPage = 1;
            let filteredData = reporte;
        
            // Función para calcular los totales
            function calculateTotals(data) {
              let sumImporte = 0;
              let sumCantidad = 0;
        
              data.forEach((item) => {
                // Manejar importE_ACTUAL
                if (typeof item.importE_ACTUAL === 'number' && !isNaN(item.importE_ACTUAL)) {
                  sumImporte += item.importE_ACTUAL;
                }
                // Manejar cantidaD_ACTUAL
                if (typeof item.cantidaD_ACTUAL === 'number' && !isNaN(item.cantidaD_ACTUAL)) {
                  sumCantidad += item.cantidaD_ACTUAL;
                }
              });
        
              return { sumImporte, sumCantidad };
            }
        
            // Función para renderizar la tabla
            function displayTable(page, data) {
              tbody.innerHTML = '';
              const start = (page - 1) * rowsPerPage;
              const end = start + rowsPerPage;
              const paginatedItems = data.slice(start, end);
        
              if (paginatedItems.length === 0) {
                const row = document.createElement('tr');
                row.innerHTML = `
                  <td colspan="5" class="text-center">No se encontraron resultados.</td>
                `;
                tbody.appendChild(row);
              } else {
                paginatedItems.forEach((item, index) => {
                  const row = document.createElement('tr');
                  row.innerHTML = `
                    <th scope="row">${start + index + 1}</th>
                    <td>${item.codigo}</td>
                    <td>${item.descripcion || 'N/A'}</td>
                    <td>${item.importE_ACTUAL || 'N/A'}</td>
                    <td>${item.cantidaD_ACTUAL || 'N/A'}</td>
                  `;
                  tbody.appendChild(row);
                });
              }
        
              // Actualizar los totales
              const { sumImporte, sumCantidad } = calculateTotals(data);
              totalImporte.textContent = sumImporte.toFixed(2); // Formato con 2 decimales
              totalCantidad.textContent = sumCantidad;
            }
        
            // Función para renderizar los controles de paginación
            function setupPagination(data) {
              paginationContainer.innerHTML = '';
              const pageCount = Math.ceil(data.length / rowsPerPage);
        
              const prevButton = document.createElement('button');
              prevButton.innerText = 'Anterior';
              prevButton.className = 'btn btn-secondary mx-1';
              prevButton.disabled = currentPage === 1;
              prevButton.addEventListener('click', () => {
                if (currentPage > 1) {
                  currentPage--;
                  displayTable(currentPage, filteredData);
                  setupPagination(filteredData);
                }
              });
              paginationContainer.appendChild(prevButton);
        
              for (let i = 1; i <= pageCount; i++) {
                const pageButton = document.createElement('button');
                pageButton.innerText = i;
                pageButton.className = `btn ${i === currentPage ? 'btn-primary' : 'btn-outline-primary'} mx-1`;
                pageButton.addEventListener('click', () => {
                  currentPage = i;
                  displayTable(currentPage, filteredData);
                  setupPagination(filteredData);
                });
                paginationContainer.appendChild(pageButton);
              }
        
              const nextButton = document.createElement('button');
              nextButton.innerText = 'Siguiente';
              nextButton.className = 'btn btn-secondary mx-1';
              nextButton.disabled = currentPage === pageCount;
              nextButton.addEventListener('click', () => {
                if (currentPage < pageCount) {
                  currentPage++;
                  displayTable(currentPage, filteredData);
                  setupPagination(filteredData);
                }
              });
              paginationContainer.appendChild(nextButton);
            }
        
            // Función para filtrar los datos
            function filterData(searchTerm) {
              searchTerm = searchTerm.toLowerCase();
              filteredData = reporte.filter((item) => {
                return (
                  item.codigo.toLowerCase().includes(searchTerm) ||
                  (item.descripcion && item.descripcion.toLowerCase().includes(searchTerm))
                );
              });
              currentPage = 1;
              displayTable(currentPage, filteredData);
              setupPagination(filteredData);
            }
        
            // Evento de búsqueda
            searchInput.addEventListener('input', (e) => {
              filterData(e.target.value);
            });
        
            // Renderizar la tabla y la paginación inicial
            displayTable(currentPage, filteredData);
            setupPagination(filteredData);
          } else {
            alert('Error al obtener las sucursales: ' + (data.message || 'Error desconocido'));
          }
    } catch (error) {
        //alert('Ocurrió un error al obtener las Cotizaciones'); <td><a href="Visitador.html" onclick="${irAVisitador(item.gerente.claveSucursal,item.gerente.idDispositivo,selectedDate,idGerente)}">Ruta</a></td>
    }
}
let timeout;
searchInput.addEventListener('input', (e) => {
  clearTimeout(timeout);
  timeout = setTimeout(() => filterData(e.target.value), 300);
});

}
    
});

