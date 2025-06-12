package com.app.sanimex.presentation.screens.VisitadorActivo

import com.app.sanimex.domain.model.Visitador.VisitadorActivo

data class VisitadorActivoScreenUiState(
    val isLoading: Boolean = false,
    val visitadores: List<VisitadorActivo> = emptyList(),
    val error: String? = null,
    val claveSucursal: String = "",
    val nombreSucursal: String = "",
    val fecha: String = ""
) 