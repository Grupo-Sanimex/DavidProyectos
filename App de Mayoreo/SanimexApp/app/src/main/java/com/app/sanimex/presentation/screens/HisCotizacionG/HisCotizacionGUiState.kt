package com.app.sanimex.presentation.screens.HisCotizacionG

import com.app.sanimex.core.util.Constants
import com.app.sanimex.domain.model.HisCotizacionModel.HisCotizacionGerente

data class HisCotizacionGUiState(
    val isLoading: Boolean = false,
    val hisCotizaciones: List<HisCotizacionGerente> = emptyList(),
    val error: String? = null,
    val selectedDate: String = Constants.fechaSeleccionada
)
