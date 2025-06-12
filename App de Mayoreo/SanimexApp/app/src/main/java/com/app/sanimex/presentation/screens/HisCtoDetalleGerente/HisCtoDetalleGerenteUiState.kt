package com.app.sanimex.presentation.screens.HisCtoDetalleGerente

import com.app.sanimex.domain.model.HisCotizacionModel.HisCotizacionDetalle

data class HisCtoDetalleGerenteUiState(
    val isLoading: Boolean = false,
    val hisCotizaciones: List<HisCotizacionDetalle> = emptyList(),
    val error: String? = null,
    val selectedDate: String = ""
)

