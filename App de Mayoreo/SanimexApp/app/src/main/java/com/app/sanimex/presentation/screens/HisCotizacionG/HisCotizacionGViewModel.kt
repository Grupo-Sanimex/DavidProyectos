package com.app.sanimex.presentation.screens.HisCotizacionG

import android.util.Log
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.app.sanimex.core.util.Constants
import com.app.sanimex.core.util.onResponse
import com.app.sanimex.domain.usecase.Cotizacion.GetHisCotizacionGUseCase
import com.app.sanimex.presentation.screens.HisCotizacion.HisCotizacionScreenUiState
import dagger.hilt.android.lifecycle.HiltViewModel
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.flow.update
import kotlinx.coroutines.launch
import java.time.LocalDate
import java.time.format.DateTimeFormatter
import javax.inject.Inject

@HiltViewModel
class HisCotizacionGViewModel @Inject constructor(
    private val getHisCotizacionGUseCase: GetHisCotizacionGUseCase
): ViewModel(){
    private val _uiState = MutableStateFlow(HisCotizacionGUiState())
    val uiState = _uiState.asStateFlow()
    init {
        val formatter = DateTimeFormatter.ofPattern("yyyy-MM-dd")
        val currentDate = LocalDate.now().format(formatter)
        if (Constants.fechaSeleccionada.isNotEmpty()){
            getHisCotizacion(Constants.fechaSeleccionada)
        }else{
            getHisCotizacion(currentDate)
        }

    }
    fun getHisCotizacion(fecha: String){
        Constants.fechaSeleccionada = fecha
        viewModelScope.launch(Dispatchers.IO) {
            Log.d("hisCotizaciones ViewModel", "Getting sucursales for date: $fecha")
            getHisCotizacionGUseCase(fecha).onResponse(
                onLoading = {
                    Log.d("SucursalViewModel", "Loading state")
                    _uiState.update { it.copy(
                        isLoading = true,
                        error = null
                    ) }
                },
                onSuccess = { hisCotizaciones ->
                    Log.d("hisCotizaciones ViewModel", "Success state with Cotizaciones: $hisCotizaciones")
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
                            error = "Error al cargar las sucursales"
                        )
                    }
                }
            )
        }
    }

    fun updateSelectedDate(date: String) {
        Log.d("SucursalViewModel", "Updating date to: $date")
        _uiState.update { it.copy(selectedDate = date) }
        getHisCotizacion(date)
    }
}
