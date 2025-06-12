package com.app.sanimex.presentation.screens.HisCtoDetalle

import com.app.sanimex.domain.model.HisCotizacionModel.HisCotizacionDetalle

data class HisCtoDetalleUiState(
    val isLoading: Boolean = false,
    val hisCotizaciones: List<HisCotizacionDetalle> = emptyList(),
    val error: String? = null,
    val selectedDate: String = ""
)
