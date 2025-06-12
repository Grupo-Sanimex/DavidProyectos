package com.app.sanimex.data.remote.dto

import com.app.sanimex.domain.model.LoginResponse

data class LoginResponseDto(
    val `data`: Data,
    val status: String,
    val token: String,
    val idRol: Int,
    val message: String
) {
    data class Data(
        val user: User
    ) {
        data class User(
            val __v: Int,
            val _id: String,
            val status: Boolean,
            val correo: String,
            val id: String,
            val nombre: String,
            val aPaterno: String,
            val idSucursal: String,
            val idPermiso: String,
            val numEmpleado: String,
            val idRol: Int
        )
    }

    fun toLoginResponse(): LoginResponse {
        return LoginResponse(
            status = status,
            token = token,
            idRol = idRol,
            message = message
        )
    }
}