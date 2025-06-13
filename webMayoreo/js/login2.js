// Función para realizar la autenticación
async function autenticar(email, password) {
    try {
      const response = await fetch('http://34.29.199.7:8080/api/Autenticacion/Acceso', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({
          correo: email,
          password: password
        })
      });
  
      const data = await response.json();
      
      if (data.status === 'success') {
        // Almacenar el token en localStorage para mantener la sesión
        localStorage.setItem('authToken', data.token);
        localStorage.setItem('userData', JSON.stringify(data.data.user));
        localStorage.setItem('idRol', data.idRol);
        
        console.log('Autenticación exitosa:', data.message);
        return data;
      } else {
        console.error('Error de autenticación:', data.message);
        return null;
      }
    } catch (error) {
      console.error('Error en la solicitud:', error);
      return null;
    }
  }
  
  // Función para verificar si el usuario está autenticado
  function estaAutenticado() {
    return localStorage.getItem('authToken') !== null;
  }
  
  // Función para incluir el token en solicitudes posteriores
  async function solicitudAutenticada(url, metodo = 'GET', datos = null) {
    if (!estaAutenticado()) {
      console.error('Usuario no autenticado');
      return null;
    }
  
    try {
      const opciones = {
        method: metodo,
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${localStorage.getItem('authToken')}`
        }
      };
  
      if (datos && (metodo === 'POST' || metodo === 'PUT')) {
        opciones.body = JSON.stringify(datos);
      }
  
      const response = await fetch(url, opciones);
      return await response.json();
    } catch (error) {
      console.error('Error en la solicitud autenticada:', error);
      return null;
    }
  }
  
  // Ejemplo de uso
  document.getElementById('loginForm').addEventListener('submit', async function(e) {
    e.preventDefault();
    
    const email = document.getElementById('email').value;
    const password = document.getElementById('password').value;
    
    const resultado = await autenticar(email, password);
    
    if (resultado) {
      // Redirigir al usuario a la página principal o dashboard
      window.location.href = 'dashboard.html';
    } else {
      // Mostrar mensaje de error
      document.getElementById('mensajeError').textContent = 'Credenciales incorrectas';
    }
  });
  
  // Ejemplo de cómo proteger rutas/páginas
  function protegerRuta() {
    if (!estaAutenticado()) {
      // Redirigir al usuario a la página de login si no está autenticado
      window.location.href = 'login.html';
    }
  }
  
  // Verificar autenticación cuando se carga una página protegida
  document.addEventListener('DOMContentLoaded', function() {
    // Ejecutar en páginas protegidas (excepto login)
    if (!window.location.pathname.includes('login.html')) {
      protegerRuta();
    }
  });


  
  async function Sucursales() {
    try {
        const token = localStorage.getItem('authToken');
        //console.log('Token:', token);
        if (!token) {
            alert('Token no encontrado. Inicie sesión nuevamente.');
            return;
        }
        const response = await fetch('http://localhost:8080/api/Sucursal/SupervisorSucursal', {
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

        if (data.status === 'success' && data.data && Array.isArray(data.data.wishlists)) {
            const wishlists = data.data.wishlists;
            const select = document.getElementById('sucursales');

            if (!select) {
                console.error('No se encontró el elemento con id "sucursales"');
                return;
            }

            select.innerHTML = '<option value="">Seleccione una sucursal</option>'; // Opción por defecto

            wishlists.forEach(item => {
                const { product } = item;

                const option = document.createElement('option');
                option.value = product._idSucursal;
                option.textContent = `${product.nombreSucursal} (SAP: ${product.idSAP})`;

                select.appendChild(option);
            });
        } else {
            localStorage.setItem('authToken', '');
            alert('Error al obtener las sucursales: ' + (data.message || 'Error desconocido'));
             // Si no hay token, redirigir al login
            window.location.href = 'login.html';
        }
    } catch (error) {
        localStorage.setItem('authToken', '');
        alert('Ocurrió un error al obtener las sucursales');
        window.location.href = 'login.html';
    }
}