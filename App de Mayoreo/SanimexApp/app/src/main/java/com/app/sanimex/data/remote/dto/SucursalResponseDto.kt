package com.app.sanimex.data.remote.dto

import com.app.sanimex.domain.model.Sucursal


data class SucursalResponseDto(
    val `data`: Data,
    val status: String,
    val idRol : Int
) {
    data class Data(
        val sucursal: List<Sucursal>,
        val idRol: Int
    ) {
        data class Sucursal(
            val _idSucursal: String,
            val idUsuario: String,
            val idSucursal: String,
            val nombreSucursal: String,
            val idSAP: String,
            val idRol: Int
        ){

        fun toSucursal(): com.app.sanimex.domain.model.Sucursal {
            return Sucursal(
                idUsuario = idUsuario,
                idSucursal = idSucursal,
                nombreSucursal = nombreSucursal,
                idSAP = idSAP,
                idRol = idRol
            )
        }
        }
    }
}