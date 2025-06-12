package com.app.sanimex.domain.model

data class CorredorItem(
    val codigo : String,
    val claveSAP : String,
    val nombreSucursal : String,
    val stockLibre : Int = 0,
    val stockDispo : Int = 0
)
