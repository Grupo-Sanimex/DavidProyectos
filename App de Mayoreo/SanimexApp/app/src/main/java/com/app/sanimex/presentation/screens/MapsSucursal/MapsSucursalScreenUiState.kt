package com.app.sanimex.presentation.screens.MapsSucursal

import com.app.sanimex.domain.model.MapsSucursal.MapsUserSucursal

data class MapsSucursalScreenUiState(
    val isLoading: Boolean = false,
    val sucursales: List<MapsUserSucursal> = emptyList(),
    val error: String? = null,
    val selectedDate: String = ""
)
