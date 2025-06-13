document.addEventListener("DOMContentLoaded", function () {
     // Obtener los parámetros de la URL
     const params = new URLSearchParams(window.location.search);
     const idRol = localStorage.getItem('idRol');
     // Recuperar datos de sessionStorage
     const datos = JSON.parse(sessionStorage.getItem('navegacionDatos')) || JSON.parse(sessionStorage.getItem('navegacionGerente'));
      const claveSucursal = datos.claveSucursal || 'No definido';
      const numeroEmpleado = datos.numeroEmpleado || 'No definido';
     const fechaUnitaria = datos.fechaUnitaria || 'No definido';
     const idGerente = datos.idGerente || 'No definido';
          
       // Asociar la función al botón
    const botonRegresar = document.getElementById("botonRegresar");
    if (botonRegresar) {
        botonRegresar.addEventListener("click", function () {
            // Redirigir a la página de Cotizaciones con los parámetros
            if (fechaUnitaria && idGerente) {    
              if (idRol === '8' || idRol === '10') {
                window.location.href = `Gerente.html`;
              } else {
                window.location.href = `index.html`;
              }
            } else {
                console.error("Faltan parámetros: fechaUnitaria o idGerente");
                alert("Error: No se pueden obtener los parámetros necesarios.");
            }
        });
    } else {
        console.error("El botón con id 'botonRegresar' no se encontró.");
    }

        window.initMap = function () {
            map = new google.maps.Map(document.getElementById("map"), {
                zoom: 14,
                center: { lat: 19.0433, lng: -98.198 },
                mapId: "b175df3cc3575c15"
              });
              
    
        function esperarYMostrarRuta() {
            if (map) {
              Ruta(claveSucursal, numeroEmpleado, fechaUnitaria);
            } else {
              setTimeout(esperarYMostrarRuta, 100);
            }
          }
          esperarYMostrarRuta();

 
     //Ruta(claveSucursal, numeroEmpleado, fechaUnitaria);
    
     async function Ruta(claveSucursal, numeroEmpleado, fechaUnitaria) {
        try {
          const token = localStorage.getItem('authToken');
          if (!token) {
            alert('Token no encontrado. Inicie sesión nuevamente.');
            return;
          }
          const response = await fetch(`http://34.29.199.7:8080/api/Ubicacion/UbicacionesMaps?claveSucursal=${claveSucursal}&numeroEmpleado=${numeroEmpleado}&fechaUnitaria=${fechaUnitaria}`, {
          //const response = await fetch(`http://localhost:8080/api/Ubicacion/UbicacionesMaps?claveSucursal=${claveSucursal}&numeroEmpleado=${numeroEmpleado}&fechaUnitaria=${fechaUnitaria}`, {
            headers: {
              'Content-Type': 'application/json',
              'Authorization': `Bearer ${token}`
            }
          });
      
          if (!response.ok) throw new Error(`Error: ${response.status}`);
      
          const data = await response.json();
          console.log('Respuesta de la API:', data);
      
          if (data.ubicaciones && Array.isArray(data.ubicaciones)) {
            // Ordenar por hora
            const ubicacionesOrdenadas = data.ubicaciones.sort((a, b) => a.horaUnitaria.localeCompare(b.horaUnitaria));
            mostrarRutaEnMapa(ubicacionesOrdenadas);
          } else {
            alert('No se encontraron ubicaciones.');
          }
        } catch (error) {
          console.error(error);
          alert('Error al obtener la ruta del visitador.');
        }
      }
      
     
      function mostrarRutaEnMapa(ubicaciones) {
        const bounds = new google.maps.LatLngBounds();
        const rutaLista = document.getElementById('ruta-lista');
        rutaLista.innerHTML = '';
      
        const waypoints = [];
        let origen = null;
        let destino = null;
      
        ubicaciones.forEach((ubicacion, i) => {
          const position = { lat: ubicacion.latitud, lng: ubicacion.longitud };
      
          const { AdvancedMarkerElement } = google.maps.marker;

          const marker = new AdvancedMarkerElement({
            map,
            position,
            title: `${ubicacion.direccion} - ${ubicacion.horaUnitaria}`,
            content: document.createTextNode(`${i + 1}`) // opcional, si deseas mostrar número
          });
      
          bounds.extend(position);
      
          // Mostrar lista ordenada
          const item = document.createElement('li');
          item.className = 'list-group-item';
          item.textContent = `${i + 1}. ${ubicacion.horaUnitaria} - ${ubicacion.direccion}`;
          rutaLista.appendChild(item);
      
          // Para direcciones de ruta
          if (i === 0) {
            origen = position;
          } else if (i === ubicaciones.length - 1) {
            destino = position;
          } else {
            waypoints.push({ location: position, stopover: true });
          }
        });
      
        map.fitBounds(bounds);
      
        if (origen && destino) {
          const directionsService = new google.maps.DirectionsService();
          const directionsRenderer = new google.maps.DirectionsRenderer();
          directionsRenderer.setMap(map);
      
          directionsService.route({
            origin: origen,
            destination: destino,
            waypoints: waypoints,
            travelMode: google.maps.TravelMode.DRIVING
          }, (response, status) => {
            if (status === "OK") {
              directionsRenderer.setDirections(response);
            } else {
              console.error("Error al trazar ruta:", status);
            }
          });
        }
      }
    };
           
});