package com.app.sanimex.domain.model

data class User(
    val nombre:String,
    val aPaterno:String,
    val numEmpleado:String,
    val id:String,
    val idSucursal:String,
    val idPermiso:String,
    val status:Boolean,
    val correo:String,
    val idRol:String,
    val telefono:String
)
