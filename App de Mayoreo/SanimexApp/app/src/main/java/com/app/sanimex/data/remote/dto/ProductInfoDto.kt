package com.app.sanimex.data.remote.dto

import com.app.sanimex.domain.model.CorredorItem
import com.app.sanimex.domain.model.ProductData
import com.app.sanimex.domain.model.SingleProductInfo

data class ProductInfoDto(
    val product: Product,
    val disponibles: List<Disponibles>,
    val status: String
) {
    data class Product(
        val id: String,
        val codigo: String,
        val description: String,
        val images: List<String>,
        val weight: String,
        val precioProducto: Float,
        val precioMetroProducto: Float,
        val color: String,
        val squareMeter: String,
        val classification: String,
        val proveedor: String
    ) {
        fun toSingleProductInfo(): SingleProductInfo {
            return SingleProductInfo(
                id = id,
                codigo = codigo,
                description = description,
                images = images,
                weight = weight,
                precioProducto = precioProducto,
                precioMetroProducto = precioMetroProducto,
                color = color,
                squareMeter = squareMeter,
                classification = classification,
                proveedor = proveedor,
                isInCart = false
            )
        }
    }
        data class Disponibles(
            val codigo: String,
            val claveSAP: String,
            val nombreSucursal: String,
            val stockLibre: Int,
            val stockDispo: Int
        ) {
            fun toCommonProduct(): CorredorItem {
                return CorredorItem(
                    codigo = codigo,
                    claveSAP = claveSAP,
                    nombreSucursal = nombreSucursal,
                    stockLibre = stockLibre,
                    stockDispo = stockDispo
                )
            }
        }

    fun toProductData(): ProductData {
        return ProductData(
            productInfo = product.toSingleProductInfo(),
            disponibles = disponibles.map { it.toCommonProduct() }
        )
    }

}