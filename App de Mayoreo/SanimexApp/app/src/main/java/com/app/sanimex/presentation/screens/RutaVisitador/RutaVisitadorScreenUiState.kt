package com.app.sanimex.presentation.screens.RutaVisitador

import com.app.sanimex.domain.model.Ubicacion.UbicacionMaps
import com.google.android.gms.maps.model.LatLng

data class RutaVisitadorScreenUiState(
    val isLoading: Boolean = false,
    val ubicaciones: List<UbicacionMaps> = emptyList(),
    val error: String? = null,
    val claveSucursal: String = "",
    val numeroEmpleado: String = "",
    val fecha: String = "",
    val selectedLocation: LatLng? = null,
    val routePoints: List<LatLng> = emptyList()
) 