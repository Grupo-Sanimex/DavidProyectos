package com.app.sanimex.data.remote.dto

import com.app.sanimex.domain.model.User

data class MeResponseDto(
    val user: User
) {
    data class User(
        val __v: Int,
        val _id: String,
        val status: Boolean,
        val correo: String,
        val id: String,
        val idSucursal: String,
        val idPermiso: String,
        val nombre: String,
        val aPaterno: String,
        val numEmpleado: String,
        val telefono: String,
        val idRol: String
    )

    fun toUser(): com.app.sanimex.domain.model.User {
        return User(
            id = user.id,
            correo = user.correo,
            nombre = user.nombre,
            telefono = user.telefono,
            idRol = user.idRol,
            status = user.status,
            idPermiso = user.idPermiso,
            idSucursal = user.idSucursal,
            numEmpleado = user.numEmpleado,
            aPaterno = user.aPaterno
        )
    }
}