package com.app.sanimex.domain.model.HisCotizacionModel

data class HisCotizacionM(
    val idCotizacion: String,
    val totalCotizacion: Float,
    val idClienteSAP: String,
    val status: String,
    val fecha: String,
    val hora: String,
    val idventa: String
)
