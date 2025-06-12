package com.app.sanimex.presentation.screens.cart

import com.app.sanimex.domain.model.CarritoFinal

data class CartScreenUIState(
    val isLoading: Boolean = true,
    val cartItems: MutableList<CarritoFinal> = mutableListOf(),
    val guardado : Boolean = false
)
