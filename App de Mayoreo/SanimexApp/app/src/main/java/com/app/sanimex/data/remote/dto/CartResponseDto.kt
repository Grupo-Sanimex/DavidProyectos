package com.app.sanimex.data.remote.dto

import com.app.sanimex.domain.model.CarritoFinal
data class CartResponseDto(
    val `data`: Data,
    val status: String
) {
    data class Data(
        val cart: Cart
    ) {
        data class Cart(
            val items: List<Item>
        ) {
            data class Item(
                val _id: String,
                val id: String,
                val product: Product,
                val quantity: Int
            ) {
                data class Product(
                    val _id: String,
                    val discount: Int?,
                    val id: String,
                    val images: List<String>,
                    val inCart: Boolean,
                    val isFav: Boolean,
                    val name: String,
                    val price: Float,
                    val recoge: Boolean,
                    val contado: Boolean,
                    val cliente: String
                )

                fun toCartItem(): CarritoFinal {
                    return CarritoFinal(
                        product = this.product.name,
                        quantity = this.quantity,
                        id = this.product._id,
                        discount = this.product.discount,
                        price = this.product.price,
                        isFav = this.product.isFav,
                        image = if (this.product.images.isNotEmpty()) this.product.images.first() else "",
                        recoge = this.product.recoge,
                        contado = this.product.contado,
                        cliente = this.product.cliente
                    )
                }
            }
        }
    }
}