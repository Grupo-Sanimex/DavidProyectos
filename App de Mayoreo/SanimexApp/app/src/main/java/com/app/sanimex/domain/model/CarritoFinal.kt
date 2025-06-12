package com.app.sanimex.domain.model

data class CarritoFinal(
    val product: String,
    val quantity: Int,
    val id: String,
    val discount: Int?,
    val price: Float,
    val isFav: Boolean,
    val image: String,
    val recoge:Boolean,
    val contado:Boolean,
    val cliente: String
)
