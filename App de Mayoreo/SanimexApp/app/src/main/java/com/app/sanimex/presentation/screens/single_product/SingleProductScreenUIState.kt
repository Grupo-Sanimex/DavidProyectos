package com.app.sanimex.presentation.screens.single_product

import com.app.sanimex.core.util.Constants
import com.app.sanimex.domain.model.ProductData
import com.app.sanimex.domain.model.Producto.ClientData

data class SingleProductScreenUIState(
    val isLoading: Boolean = true,
    val searchQuery: String = "",
    val searchResultsList: List<String> = emptyList(),
    val productData: ProductData = ProductData(),
    val clientData: ClientData = ClientData(),
    val isError: Boolean = false,
    val errorMsg: String? = "",
    val isButtonLoading: Boolean = false,
    val isAddButton: Boolean = true,
    val isInWishlist: Boolean = false,
    val isUpsertInWishlistLoading: Boolean = false,
    val cliente: String = Constants.cliente,
    val tipoEntrega : Boolean = true,
    val tipoPago : Boolean = true,
    val isErrorCliente: Boolean = false,
    val isErrorCart: Boolean = false,
    val isErrorRecoege: Boolean = false,
    val isContadoRecoege: Boolean = false
)