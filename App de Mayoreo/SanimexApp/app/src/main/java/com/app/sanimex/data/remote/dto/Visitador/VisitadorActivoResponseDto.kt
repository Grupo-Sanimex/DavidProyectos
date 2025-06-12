package com.app.sanimex.data.remote.dto.Visitador

import com.app.sanimex.domain.model.Visitador.VisitadorActivo
import com.google.gson.annotations.SerializedName

data class VisitadorActivoResponseDto(
    @SerializedName("visitadorActivo")
    val visitadores: List<VisitadorDto>
) {
    data class VisitadorDto(
        val claveSucursal: String,
        val nombreSucursal: String,
        val idUsuario: String,
        val numeroEmpleado: String,
        val nombre: String,
        val aPaterno: String,
        val aMaterno: String
    ) {
        fun toVisitadorActivo(): VisitadorActivo {
            return VisitadorActivo(
                claveSucursal = claveSucursal,
                nombreSucursal = nombreSucursal,
                idUsuario = idUsuario,
                numeroEmpleado = numeroEmpleado,
                nombre = nombre,
                aPaterno = aPaterno,
                aMaterno = aMaterno
            )
        }
    }
} 