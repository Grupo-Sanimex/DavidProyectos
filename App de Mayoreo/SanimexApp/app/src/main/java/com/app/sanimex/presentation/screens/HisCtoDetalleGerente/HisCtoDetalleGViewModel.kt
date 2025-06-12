package com.app.sanimex.presentation.screens.HisCtoDetalleGerente

import android.util.Log
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.app.sanimex.core.util.Constants
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
class HisCtoDetalleGViewModel@Inject constructor (
    private val getHisCtoDetalleUseCase: GetHisCtoDetalleUseCase
): ViewModel(){

    private val _uiState = MutableStateFlow(HisCtoDetalleGerenteUiState())
    val uiState = _uiState.asStateFlow()

    fun getHisCtoDetalle(idCotizacion: String){
        viewModelScope.launch(Dispatchers.IO) {
            getHisCtoDetalleUseCase(idCotizacion).onResponse(
                onLoading = {
                    _uiState.update { it.copy(
                        isLoading = true,
                        error = null
                    ) }
                },
                onSuccess = { hisCtoDetalle ->
                    if (hisCtoDetalle != null) {
                        _uiState.update {
                            it.copy(
                                isLoading = false,
                                hisCotizaciones = hisCtoDetalle,
                                error = null
                            )
                        }
                    } else {
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