package com.app.sanimex.presentation.screens.account

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.app.sanimex.core.util.Constants.IS_LOGGED_IN_KEY
import com.app.sanimex.core.util.Constants.USER_TOKEN_KEY
import com.app.sanimex.core.util.onResponse
import com.app.sanimex.domain.usecase.GetMyInfoUseCase
import com.app.sanimex.domain.usecase.RemoveFromDataStoreUseCase
import dagger.hilt.android.lifecycle.HiltViewModel
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.flow.update
import kotlinx.coroutines.launch
import javax.inject.Inject

@HiltViewModel
class AccountViewModel @Inject constructor(
    private val getMyInfoUseCase: GetMyInfoUseCase, // Caso de uso para obtener información del usuario
    private val removeFromDataStoreUseCase: RemoveFromDataStoreUseCase // Caso de uso para eliminar datos del DataStore
) : ViewModel() { // Hereda de ViewModel para manejar la lógica de la interfaz de usuario
    private val _uiState = MutableStateFlow(AccountUiState()) // Estado de la interfaz de usuario (mutable)
    val uiState = _uiState.asStateFlow() // Expone el estado de la interfaz como un StateFlow inmutable

    init {
        getMyProfile() // Llama a la función para obtener el perfil del usuario al inicializar el ViewModel
    }

    private fun getMyProfile() {
        // Lanza una coroutine en el alcance del ViewModel
        viewModelScope.launch(Dispatchers.IO) { // Utiliza el dispatcher IO para operaciones en segundo plano
            getMyInfoUseCase() // Llama al caso de uso para obtener la información del usuario
                .onResponse(
                    onLoading = { // Callback para manejar el estado de carga
                        _uiState.update { it.copy(isLoading = true) } // Actualiza el estado para indicar que está cargando
                    },
                    onSuccess = { response -> // Callback para manejar la respuesta exitosa
                        _uiState.update { it.copy(isLoading = false, me = response) } // Actualiza el estado con la información del usuario
                    },
                    onFailure = { // Callback para manejar errores
                        _uiState.update { it.copy(isLoading = false) } // Actualiza el estado para dejar de indicar que está cargando
                    }
                )
        }
    }

    fun logout() {
        // Lanza una coroutine en el alcance del ViewModel para cerrar sesión
        viewModelScope.launch(Dispatchers.IO) {
            removeFromDataStoreUseCase(key = IS_LOGGED_IN_KEY) // Elimina la clave de inicio de sesión del DataStore
            removeFromDataStoreUseCase(key = USER_TOKEN_KEY) // Elimina el token del usuario del DataStore
        }
    }
}