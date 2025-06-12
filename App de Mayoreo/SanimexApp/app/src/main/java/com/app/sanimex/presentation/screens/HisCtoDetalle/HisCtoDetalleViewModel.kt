package com.app.sanimex.presentation.screens.HisCtoDetalle

import android.util.Log
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.app.sanimex.core.util.onResponse
import com.app.sanimex.domain.usecase.Cotizacion.GetHisCtoDetalleUseCase
import dagger.hilt.android.lifecycle.HiltViewModel
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.flow.update
import kotlinx.coroutines.launch
import javax.inject.Inject
@HiltViewModel
class HisCtoDetalleViewModel@Inject constructor (
    private val getHisCtoDetalleUseCase: GetHisCtoDetalleUseCase
): ViewModel(){

    private val _uiState = MutableStateFlow(HisCtoDetalleUiState())
    val uiState = _uiState.asStateFlow()

    fun getHisCtoDetalle(idCotizacion: String){
        viewModelScope.launch(Dispatchers.IO) {
            Log.d("hisCotizaciones ViewModel", "Sacar Cotizaciones for date: $idCotizacion")
            getHisCtoDetalleUseCase(idCotizacion).onResponse(
                onLoading = {
                    Log.d("SucursalViewModel", "Loading state")
                    _uiState.update { it.copy(
                        isLoading = true,
                        error = null
                    ) }
                },
                onSuccess = { hisCtoDetalle ->
                    Log.d("hisCotizaciones ViewModel", "Success state with Cotizaciones: $hisCtoDetalle")
                    if (hisCtoDetalle != null) {
                        Log.d("SucursalViewModel", "Sucursales size: ${hisCtoDetalle.size}")
                        _uiState.update {
                            it.copy(
                                isLoading = false,
                                hisCotizaciones = hisCtoDetalle,
                                error = null
                            )
                        }
                    } else {
                        Log.d("SucursalViewModel", "Sucursales is null")
                        _uiState.update {
                            it.copy(
                                isLoading = false,
                                hisCotizaciones = emptyList(),
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
}