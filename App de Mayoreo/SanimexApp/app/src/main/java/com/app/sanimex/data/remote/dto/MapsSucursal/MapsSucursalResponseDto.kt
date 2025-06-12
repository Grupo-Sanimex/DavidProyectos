package com.app.sanimex.data.remote.dto.MapsSucursal

import com.app.sanimex.domain.model.MapsSucursal.MapsUserSucursal

data class MapsSucursalResponseDto(
    val address: List<Addres>? // Marca como nullable si es posible
){
    data class Addres(
        val claveSucursal: String,
        val nombreSucursal: String
    ) {
        fun toMapsSucursal(): MapsUserSucursal {
            return MapsUserSucursal(
                claveSucursal = claveSucursal,
                nombreSucursal = nombreSucursal
            )
        }
    }
}
