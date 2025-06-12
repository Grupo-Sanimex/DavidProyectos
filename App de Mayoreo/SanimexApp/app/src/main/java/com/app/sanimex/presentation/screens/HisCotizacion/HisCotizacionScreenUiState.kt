package com.app.sanimex.presentation.screens.HisCotizacion

import com.app.sanimex.domain.model.HisCotizacionModel.HisCotizacionM

data class HisCotizacionScreenUiState(
    val isLoading: Boolean = false,
    val hisCotizaciones: List<HisCotizacionM> = emptyList(),
    val error: String? = null,
    val selectedDate: String = ""
)
