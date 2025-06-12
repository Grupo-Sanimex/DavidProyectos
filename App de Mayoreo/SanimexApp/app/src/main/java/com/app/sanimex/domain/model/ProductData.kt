package com.app.sanimex.domain.model

data class ProductData(
    val productInfo: SingleProductInfo = SingleProductInfo(),
    val disponibles: List<CorredorItem> = emptyList()
)
