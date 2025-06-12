package com.app.sanimex.presentation.screens.HisCotizacionGDetalle

import com.app.sanimex.domain.model.HisCotizacionModel.HisCotizacionM

data class HisCotizacionGDUiState(
    val isLoading: Boolean = false,
    val hisCotizaciones: List<HisCotizacionM> = emptyList(),
    val error: String? = null,
    val selectedDate: String = ""
)
