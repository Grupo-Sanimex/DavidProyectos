document.addEventListener('DOMContentLoaded', function () {
  const formulario = document.querySelector('form');

  formulario.addEventListener('submit', async function (e) {
    e.preventDefault();

    const numeroUsuario = document.getElementById('email').value;
    const password = document.getElementById('password').value;

    if (!numeroUsuario || !password) {
      alert('Por favor, completa todos los campos');
      return;
    }
    try {
      let resultado = await autenticar(numeroUsuario, password);

      // Si falla el primer método, intenta el segundo
      if (!resultado || resultado.status !== 'success') {
        resultado = await accesoPOS(numeroUsuario, password);
      }

      if (resultado && resultado.status === 'success') {
        if(resultado.idRol === 10) {
          window.location.href = 'Gerente.html';
        }else{
          window.location.href = 'index.html';
        }
        
      } else {
        alert('Error de autenticación: ' + (resultado?.message || 'Credenciales incorrectas'));
      }
    } catch (error) {
      console.error('Error durante la autenticación:', error);
      alert('Ocurrió un error durante la autenticación');
    }
  });
});

async function autenticar(numeroUsuario, password) {
  try {
    //const response = await fetch(`http://localhost:8080/api/AuthWeb/AccesoWeb?numeroUsuario=${numeroUsuario}&password=${password}`, {
    const response = await fetch(`http://34.29.199.7:8080/api/AuthWeb/AccesoWeb?numeroUsuario=${numeroUsuario}&password=${password}`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      }
    });

    const data = await response.json();

    if (data.status === 'success') {
      guardarSesion(data);
    }

    return data;
  } catch (error) {
    console.error('Error en la solicitud (autenticar):', error);
    return null;
  }
}

async function accesoPOS(numeroUsuario, password) {
  try {
    //const response = await fetch(`http://localhost:8080/api/Autenticacion/Acceso?email=${numeroUsuario}&password=${password}`, {
      const response = await fetch(`http://34.29.199.7:8080/api/Autenticacion/Acceso?email=${numeroUsuario}&password=${password}`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      }
    });

    const data = await response.json();

    if (data.status === 'success') {
      if (data.idRol != "10") {
        alert('Tu no tienes acceso a esta Sistema.');
      }else{
        guardarSesion(data);
        return data;
      }
   
    }


  } catch (error) {
    console.error('Error en la solicitud (POS):', error);
    return null;
  }
}

function guardarSesion(data) {
  // Guardar el token y los datos del usuario en localStorage
  localStorage.setItem('authToken', data.token);
  localStorage.setItem('userData', JSON.stringify(data.data.user));
  localStorage.setItem('idRol', data.idRol);
  localStorage.setItem('idGerente', data.data.user.id);
}

function cerrarSesion() {
  localStorage.removeItem('authToken');
  localStorage.removeItem('userData');
  localStorage.removeItem('idRol');
  window.location.href = 'login.html';
}