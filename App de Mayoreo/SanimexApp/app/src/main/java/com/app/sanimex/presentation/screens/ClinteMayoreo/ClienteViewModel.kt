package com.app.sanimex.presentation.screens.ClinteMayoreo
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.app.sanimex.core.util.Constants
import com.app.sanimex.core.util.onResponse
import com.app.sanimex.domain.usecase.Cliente.ClienteUseCase
import dagger.hilt.android.lifecycle.HiltViewModel
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.flow.update
import kotlinx.coroutines.launch
import javax.inject.Inject
@HiltViewModel
class ClienteViewModel @Inject constructor(
) : ViewModel() {
    private val _uiState = MutableStateFlow(ClienteUiState())
    val uiState = _uiState.asStateFlow()


    fun onConsultaButtonClicked() {
        Constants.tipoConsulta = false
        _uiState.update {
            it.copy(
                isLoginSuccessful = true,
            )
        }
       /* viewModelScope.launch(Dispatchers.IO) {
            clienteUseCase(
                claveCliente = uiState.value.claveCliente,
                corredor = Constants.corredor,
            ).onResponse(
                onSuccess = { response ->
                    if (response != null) {
                        if (response.message == "Cliente encontrado") {
                            Constants.cliente = uiState.value.claveCliente
                            _uiState.update {
                                it.copy(
                                    isLoading = false,
                                    isLoginSuccessful = true,
                                    isError = false
                                )
                            }
                        }else{
                            _uiState.update {
                                it.copy(
                                    isLoading = false, isErrorFailed = true
                                )
                            }
                        }
                    }
                },
                onFailure = { msg ->
                    _uiState.update { it.copy(isLoading = false, isErrorFailed = true) }
                },
                onLoading = {
                    _uiState.update { it.copy(isLoading = true, isError = false) }
                }
            )
        }
        */
    }

    fun onVisitaButtonClicked() {
        Constants.tipoConsulta = true
        _uiState.update {
            it.copy(
                isLoginSuccessful = true,
            )
        }
       /* viewModelScope.launch(Dispatchers.IO) {
            clienteUseCase(
                claveCliente = uiState.value.claveCliente,
                corredor = Constants.corredor,
            ).onResponse(
                onSuccess = { response ->
                    if (response != null) {
                        if (response.message == "Cliente encontrado") {
                            Constants.cliente = uiState.value.claveCliente
                            _uiState.update {
                                it.copy(
                                    isLoading = false,
                                    isLoginSuccessful = true,
                                    isError = false
                                )
                            }
                        }else{
                            _uiState.update {
                                it.copy(
                                    isLoading = false,
                                    isLoginSuccessful = false,
                                    isError = true,
                                    errorMessage = response.message
                                )
                            }
                        }
                    }
                },
                onFailure = { msg ->
                    _uiState.update { it.copy(isLoading = false, isErrorFailed = true) }
                },
                onLoading = {
                    _uiState.update { it.copy(isLoading = true, isError = false) }
                }
            )
        }
        */
    }
}