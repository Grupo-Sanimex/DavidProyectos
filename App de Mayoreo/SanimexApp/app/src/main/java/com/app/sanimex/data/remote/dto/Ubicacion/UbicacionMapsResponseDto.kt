package com.app.sanimex.data.remote.dto.Ubicacion

import com.app.sanimex.domain.model.Ubicacion.UbicacionMaps
import com.google.gson.annotations.SerializedName

data class UbicacionMapsResponseDto(
    @SerializedName("ubicaciones")
    val ubicaciones: List<UbicacionDto>
) {
    data class UbicacionDto(
        val id: Int,
        val direccion: String,
        val latitud: Double,
        val longitud: Double,
        val horaUnitaria: String
    ) {
        fun toUbicacionMaps(): UbicacionMaps {
            return UbicacionMaps(
                id = id,
                direccion = direccion,
                latitud = latitud,
                longitud = longitud,
                horaUnitaria = horaUnitaria
            )
        }
    }
} 