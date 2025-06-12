package com.app.sanimex.presentation.screens.HisCotizacion

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.app.sanimex.core.util.Constants
import com.app.sanimex.core.util.onResponse
import com.app.sanimex.domain.usecase.Cotizacion.GetHisCotizacionUseCase
import dagger.hilt.android.lifecycle.HiltViewModel
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.flow.update
import kotlinx.coroutines.launch
import java.time.LocalDate
import java.time.format.DateTimeFormatter
import javax.inject.Inject

@HiltViewModel
class HisCotizacionScreenViewModel @Inject constructor(
    private val getHisCotizacionUseCase: GetHisCotizacionUseCase
) : ViewModel() {

    private val _uiState = MutableStateFlow(HisCotizacionScreenUiState())
    val uiState: StateFlow<HisCotizacionScreenUiState> = _uiState.asStateFlow()

    init {
        val formatter = DateTimeFormatter.ofPattern("yyyy-MM-dd")
        val currentDate = LocalDate.now().format(formatter)
        if (Constants.fechaSeleccionada.isNotEmpty()){
            getHisCotizacion(Constants.fechaSeleccionada)
        }else{
            getHisCotizacion(currentDate)
        }
    }

    fun updateSelectedDate(date: String) {
        _uiState.update { it.copy(selectedDate = date) }
        getHisCotizacion(date)
    }

    fun getHisCotizacion(fecha: String) {
        Constants.fechaSeleccionada = fecha
        viewModelScope.launch(Dispatchers.IO) {
            getHisCotizacionUseCase(fecha).onResponse(
                onLoading = {
                    _uiState.update {
                        it.copy(
                            isLoading = true,
                            error = null
                        )
                    }
                },
                onSuccess = { hisCotizaciones ->
                    if (hisCotizaciones != null) {
                        _uiState.update {
                            it.copy(
                                isLoading = false,
                                hisCotizaciones = hisCotizaciones,
                                selectedDate = fecha,
                                error = null
                            )
                        }
                    } else {
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
                            error = "Sin Cotizaciones Seleccione otra fecha"
                        )
                    }
                }
            )
        }
    }

}
