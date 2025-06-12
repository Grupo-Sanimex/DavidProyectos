package com.app.sanimex.presentation.screens.RutaVisitador

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.app.sanimex.core.util.onResponse
import com.app.sanimex.domain.usecase.Ubicacion.GetUbicacionesMapsUseCase
import com.google.android.gms.maps.model.LatLng
import dagger.hilt.android.lifecycle.HiltViewModel
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.flow.update
import kotlinx.coroutines.launch
import javax.inject.Inject

@HiltViewModel
class RutaVisitadorViewModel @Inject constructor(
    private val getUbicacionesMapsUseCase: GetUbicacionesMapsUseCase
) : ViewModel() {
    private val _uiState = MutableStateFlow(RutaVisitadorScreenUiState())
    val uiState = _uiState.asStateFlow()

    fun getUbicaciones(claveSucursal: String, numeroEmpleado: String, fecha: String) {
        viewModelScope.launch(Dispatchers.IO) {
            _uiState.update { 
                it.copy(
                    claveSucursal = claveSucursal,
                    numeroEmpleado = numeroEmpleado,
                    fecha = fecha
                )
            }
            
            getUbicacionesMapsUseCase(claveSucursal, numeroEmpleado, fecha).onResponse(
                onLoading = {
                    _uiState.update { it.copy(
                        isLoading = true,
                        error = null
                    ) }
                },
                onSuccess = { ubicaciones ->
                    val points = ubicaciones?.map { 
                        LatLng(it.latitud, it.longitud)
                    } ?: emptyList()
                    
                    _uiState.update { 
                        it.copy(
                            isLoading = false,
                            ubicaciones = ubicaciones ?: emptyList(),
                            routePoints = points,
                            error = null
                        )
                    }
                },
                onFailure = {
                    _uiState.update { 
                        it.copy(
                            isLoading = false,
                            ubicaciones = emptyList(),
                           // error = error?.message ?: "Error al cargar las ubicaciones"
                        )
                    }
                }
            )
        }
    }

    fun updateSelectedLocation(location: LatLng) {
        _uiState.update { it.copy(selectedLocation = location) }
    }
} 