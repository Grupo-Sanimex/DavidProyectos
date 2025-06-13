// Este código debe estar en tu archivo JS (por ejemplo, auth.js)

document.addEventListener('DOMContentLoaded', function() {

  const token = localStorage.getItem('authToken');

  if (token) {
      // Si no hay token, redirigir al login
      window.location.href = 'index.html';
  }else{
    // Seleccionar el formulario - nota que no tiene un id, así que seleccionamos por tag
    const formulario = document.querySelector('form');
    
    formulario.addEventListener('submit', async function(e) {
      // Prevenir que el formulario se envíe de forma predeterminada
      e.preventDefault();
      
      // Obtener los valores de los campos
      const email = document.getElementById('email').value;
      const password = document.getElementById('password').value;
      
      // Verificar que los campos no estén vacíos
      if (!email || !password) {
        alert('Por favor, completa todos los campos');
        return;
      }
      
      // Llamar a la función de autenticación (que definimos anteriormente)
      try {
        const resultado = await autenticar(email, password);
        
        if (resultado && resultado.status === 'success') {
          // Redirigir al usuario a la página principal o dashboard
          alert('¡Autenticación exitosa!');
          window.location.href = 'index.html'; // o la página que desees
        } else {
          // Mostrar mensaje de error
          alert('Error de autenticación: ' + (resultado?.message || 'Credenciales incorrectas'));
        }
      } catch (error) {
        console.error('Error durante la autenticación:', error);
        alert('Ocurrió un error durante la autenticación');
      }
    });
  }
});
  
  // La función de autenticación que ya teníamos
  async function autenticar(email, password) {
    try {
      const response = await fetch(`http://localhost:8080/api/Autenticacion/Acceso?email=${email}&password=${password}`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        }
    });
  
      const data = await response.json();
      
      if (data.status === 'success') {
        // Almacenar el token en localStorage para mantener la sesión
        localStorage.setItem('authToken', data.token);
        localStorage.setItem('userData', JSON.stringify(data.data.user));
        localStorage.setItem('idRol', data.idRol);
         // Redirigir al usuario a la página de login si no está autenticado
        window.location.href = 'login.html';
        return data;
      } else {
        console.error('Error de autenticación:', data.message);
        return data; // Devolvemos el objeto para poder ver el mensaje de error
      }
    } catch (error) {
      console.error('Error en la solicitud:', error);
      throw error; // Lanzamos el error para manejarlo en el evento submit
    }
  }