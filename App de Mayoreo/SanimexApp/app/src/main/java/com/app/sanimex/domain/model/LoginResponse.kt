package com.app.sanimex.domain.model

data class LoginResponse(
    val status: String,
    val token: String,
    val idRol : Int,
    val message: String
)
