package com.app.sanimex.presentation.screens.HisCotizacionGDetalle

import android.util.Log
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.app.sanimex.core.util.Constants
import com.app.sanimex.core.util.onResponse
import com.app.sanimex.domain.usecase.Cotizacion.GetHisCtoGDetalleUseCase
import dagger.hilt.android.lifecycle.HiltViewModel
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.flow.update
import kotlinx.coroutines.launch
import javax.inject.Inject

@HiltViewModel
class HisCotizacionGDViewModel @Inject constructor(
    private val getHisCotizacionUseCase: GetHisCtoGDetalleUseCase
) : ViewModel() {

    private val _uiState = MutableStateFlow(HisCotizacionGDUiState())
    val uiState: StateFlow<HisCotizacionGDUiState> = _uiState.asStateFlow()

    fun verifieDate(fecha: String, idVistador: String) {
        if (Constants.fechaGDetalle.isNotEmpty() && Constants.idVisitadorGD.isNotEmpty()) {
            getHisCotizacion(Constants.fechaGDetalle, Constants.idVisitadorGD)
        }else{
            Constants.fechaGDetalle = fecha
            Constants.idVisitadorGD = idVistador
            getHisCotizacion(fecha, idVistador)
        }
    }

    fun getHisCotizacion(fecha: String, idVistador: String) {
        viewModelScope.launch(Dispatchers.IO) {
            Log.d("hisCotizaciones ViewModel", "Getting sucursales for date: $fecha")
            getHisCotizacionUseCase(fecha, idVistador).onResponse(
                onLoading = {
                    Log.d("SucursalViewModel", "Loading state")
                    _uiState.update {
                        it.copy(
                            isLoading = true,
                            error = null
                        )
                    }
                },
                onSuccess = { hisCotizaciones ->
                    Log.d(
                        "hisCotizaciones ViewModel",
                        "Success state with Cotizaciones: $hisCotizaciones"
                    )
                    if (hisCotizaciones != null) {
                        Log.d("SucursalViewModel", "Sucursales size: ${hisCotizaciones.size}")
                        _uiState.update {
                            it.copy(
                                isLoading = false,
                                hisCotizaciones = hisCotizaciones,
                                selectedDate = fecha,
                                error = null
                            )
                        }
                    } else {
                        Log.d("SucursalViewModel", "Sucursales is null")
                        _uiState.update {
                            it.copy(
                                isLoading = false,
                                hisCotizaciones = emptyList(),
                                selectedDate = fecha,
                                error = null
                            )
                        }
                    }
                },
                onFailure = {
                    //  Log.e("SucursalViewModel", "Error state: ${error?.message}")
                    _uiState.update {
                        it.copy(
                            isLoading = false,
                            hisCotizaciones = emptyList(),
                            error = "Error al cargar las Cotizaciones Seleccione otra fecha"
                        )
                    }
                }
            )
        }
    }

}
