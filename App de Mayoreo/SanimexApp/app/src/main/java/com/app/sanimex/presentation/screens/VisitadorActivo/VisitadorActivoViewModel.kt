package com.app.sanimex.presentation.screens.VisitadorActivo

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.app.sanimex.core.util.Constants
import com.app.sanimex.core.util.onResponse
import com.app.sanimex.domain.usecase.Visitador.GetVisitadoresActivosUseCase
import dagger.hilt.android.lifecycle.HiltViewModel
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.flow.update
import kotlinx.coroutines.launch
import javax.inject.Inject

@HiltViewModel
class VisitadorActivoViewModel @Inject constructor(
    private val getVisitadoresActivosUseCase: GetVisitadoresActivosUseCase
) : ViewModel() {
    private val _uiState = MutableStateFlow(VisitadorActivoScreenUiState())
    val uiState = _uiState.asStateFlow()

    fun getVisitadores(claveSucursal: String, fecha: String) {
        viewModelScope.launch(Dispatchers.IO) {
            _uiState.update { it.copy(claveSucursal = claveSucursal, fecha = fecha) }
            
            getVisitadoresActivosUseCase(claveSucursal, fecha).onResponse(
                onLoading = {
                    _uiState.update { it.copy(
                        isLoading = true,
                        error = null
                    ) }
                },
                onSuccess = { visitadores ->
                    _uiState.update { 
                        it.copy(
                            isLoading = false,
                            visitadores = visitadores ?: emptyList(),
                            error = null
                        )
                    }
                },
                onFailure = {
                    _uiState.update { 
                        it.copy(
                            isLoading = false,
                            visitadores = emptyList(),
                           // error = error?.localizedMessage ?: "Error al cargar los visitadores"
                        )
                    }
                }
            )
        }
    }
} 