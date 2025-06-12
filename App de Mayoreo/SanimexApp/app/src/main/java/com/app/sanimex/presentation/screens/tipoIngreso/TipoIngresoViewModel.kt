package com.app.sanimex.presentation.screens.tipoIngreso

import android.util.Log
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.app.sanimex.core.util.Constants
import com.app.sanimex.core.util.onResponse
import com.app.sanimex.data.remote.dto.DireccionResponseDto
import com.app.sanimex.domain.model.DireccionResponse
import com.app.sanimex.domain.usecase.AddAddressUseCase
import dagger.hilt.android.lifecycle.HiltViewModel
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.flow.update
import kotlinx.coroutines.launch
import javax.inject.Inject

@HiltViewModel
class TipoIngresoViewModel @Inject constructor(
    private val addAddressUseCase: AddAddressUseCase
) : ViewModel(){
    private val _uiState = MutableStateFlow(TipoIngresoUiState())
    val uiState = _uiState.asStateFlow()


    fun addAddress() {
        viewModelScope.launch(Dispatchers.IO) {
            addAddressUseCase(
                direccion = Constants.direccion,
                latitu = Constants.latitud,
                longitu = Constants.longitu,
                claveSucursal = Constants.corredor,
                tipoIngreso = Constants.tipoConsulta
            ).onResponse(
                onSuccess = { response ->
                    Log.d("TipoIngresoViewModel", "onSuccess - response: $response")
                    Log.d("TipoIngresoViewModel", "onSuccess - response type: ${response?.javaClass?.name}")
                    _uiState.update {
                        it.copy(
                            errorDireccion = false
                        )
                    }
                    if (response != null) {
                        // Intenta castear si estás seguro del tipo, o usa un 'is' check
                        if (true) { // Asumiendo que esperas DireccionResponseDto
                            Constants.idDireccion = response.idDireccion
                            Log.d("TipoIngresoViewModel", "idDireccion set to: ${response.idDireccion}")
                        } else {
                            Log.e("TipoIngresoViewModel", "Response is not of type DireccionResponseDto")
                        }
                    } else {
                        Log.w("TipoIngresoViewModel", "Response in onSuccess is null")
                    }
                },
                onFailure = {
                    _uiState.update {
                        it.copy(
                            errorDireccion = true
                        )
                    }
                },
                onLoading = {
                    _uiState.update { it.copy(errorDireccion = false) }
                }
            )
        }
    }
}