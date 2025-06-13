async function Ruta(claveSucursal, numeroEmpleado, fechaUnitaria) {
    try {
      const token = localStorage.getItem('authToken');
      if (!token) {
        alert('Token no encontrado. Inicie sesión nuevamente.');
        return;
      }
  
      const response = await fetch(`http://34.29.199.7:8080/api/Ubicacion/UbicacionesMaps?claveSucursal=${claveSucursal}&numeroEmpleado=${numeroEmpleado}&fechaUnitaria=${fechaUnitaria}`, {
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
  
  let map;
  
  function initMap() {
    map = new google.maps.Map(document.getElementById("map"), {
      zoom: 14,
      center: { lat: 19.0433, lng: -98.198 },
    });
  
    // Ejemplo con valores de prueba
    Ruta(1, 1001, '2024-04-09'); // Puedes modificar con tus parámetros reales
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
  
      const marker = new google.maps.Marker({
        position,
        map,
        label: `${i + 1}`,
        title: `${ubicacion.direccion} - ${ubicacion.horaUnitaria}`
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
  