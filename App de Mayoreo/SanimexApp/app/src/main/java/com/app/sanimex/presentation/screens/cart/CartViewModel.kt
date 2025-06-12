package com.app.sanimex.presentation.screens.cart

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.app.sanimex.core.util.Constants
import com.app.sanimex.core.util.onResponse
import com.app.sanimex.domain.usecase.DeleteFromCartUseCase
import com.app.sanimex.domain.usecase.GetCartUseCase
import com.app.sanimex.domain.usecase.SaveFromCartUseCase
import com.app.sanimex.domain.usecase.cart.ChanQuantityDecTenUseCase
import com.app.sanimex.domain.usecase.cart.ChanQuantityIcreTenUseCase
import com.app.sanimex.domain.usecase.cart.ChangeQuantityDecreUseCase
import com.app.sanimex.domain.usecase.cart.ChangeQuantityIncreUseCase
import dagger.hilt.android.lifecycle.HiltViewModel
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.flow.update
import kotlinx.coroutines.launch
import javax.inject.Inject

@HiltViewModel
class CartViewModel @Inject constructor(
    private val getCartUseCase: GetCartUseCase,
    private val deleteFromCartUseCase: DeleteFromCartUseCase,
    private val saveFromCartUseCase: SaveFromCartUseCase,
    private val incrementQuantityUseCase: ChangeQuantityIncreUseCase,
    private val decrementQuantityUseCase: ChangeQuantityDecreUseCase,
    private val incrementTenQuantityUseCase: ChanQuantityIcreTenUseCase,
    private val decrementTenQuantityUseCase: ChanQuantityDecTenUseCase
) : ViewModel() {
    private val _uiState = MutableStateFlow(CartScreenUIState())
    val uiState = _uiState.asStateFlow()

    init {
        getMyCart()
    }

    private fun getMyCart() {
        viewModelScope.launch(Dispatchers.IO) {
            getCartUseCase()
                .onResponse(
                    onLoading = {
                        _uiState.update { it.copy(isLoading = true) }
                    },
                    onSuccess = { response ->
                        _uiState.update {
                            it.copy(
                                isLoading = false,
                                cartItems = response!!.toMutableList()
                            )
                        }
                    },
                    onFailure = {
                        _uiState.update {
                            it.copy(
                                isLoading = false,
                                cartItems = mutableListOf()
                            )
                        }
                    }
                )
        }
    }


    fun deleteFromCart(productID: String) {
        val newList = uiState.value.cartItems.map { it.copy() }.toMutableList()
        newList.removeIf { item -> item.id == productID }
        _uiState.update { it.copy(cartItems = newList) }
        viewModelScope.launch(
            Dispatchers.IO
        ) {
            deleteFromCartUseCase(productID = productID)
                .onResponse(
                    onLoading = {},
                    onSuccess = {},
                    onFailure = {}
                )
        }
    }

    fun saveCart() {
        viewModelScope.launch(
            Dispatchers.IO
        ) {
            saveFromCartUseCase()
                .onResponse(
                    onLoading = {},
                    onSuccess = {
                        _uiState.update { it.copy(guardado = true) }
                    },
                    onFailure = {}
                )
        }
    }

    fun onChangeQuantity(productID: String, isIncrease: Boolean) {
        val item = uiState.value.cartItems.find { it.id == productID }
        item?.let {
            val newQuantity = if (isIncrease) {
                it.quantity + 1
            } else {
                Math.max(it.quantity - 1, 1) // Ensures quantity never goes below 0
            }
            val updatedItem = it.copy(quantity = newQuantity)
            val newList = uiState.value.cartItems.map {
                if (it.id == productID) updatedItem else it
            }
            _uiState.update { it.copy(cartItems = newList.toMutableList()) }
            if (isIncrease) {
                incrementFromCart(productID = productID)
            }else{
                decrementFromCart(productID = productID)
            }
        }
    }

    fun onChangeQuantityTen(productID: String, isIncrease: Boolean) {
        val item = uiState.value.cartItems.find { it.id == productID }
        item?.let {
            val newQuantity = if (isIncrease) {
                it.quantity + 10
            } else {
                Math.max(it.quantity - 10, 1) // Ensures quantity never goes below 0
            }
            val updatedItem = it.copy(quantity = newQuantity)
            val newList = uiState.value.cartItems.map {
                if (it.id == productID) updatedItem else it
            }
            _uiState.update { it.copy(cartItems = newList.toMutableList()) }
            if (isIncrease) {
                incrementFromCartTen(productID = productID)
            }else{
                decrementFromCartTen(productID = productID)
            }
        }
    }

    fun incrementFromCart(productID: String) {
        viewModelScope.launch(
            Dispatchers.IO
        ) {
            incrementQuantityUseCase(productID = productID, Constants.corredor)
                .onResponse(
                    onLoading = {},
                    onSuccess = {},
                    onFailure = {}
                )
        }
    }
    fun incrementFromCartTen(productID: String) {
        viewModelScope.launch(
            Dispatchers.IO
        ) {
            incrementTenQuantityUseCase(productID = productID, Constants.corredor)
                .onResponse(
                    onLoading = {},
                    onSuccess = {},
                    onFailure = {}
                )
        }
    }

    fun decrementFromCart(productID: String) {
        viewModelScope.launch(
            Dispatchers.IO
        ) {
            decrementQuantityUseCase(productID = productID, Constants.corredor)
                .onResponse(
                    onLoading = {},
                    onSuccess = {},
                    onFailure = {}
                )
        }
    }
    fun decrementFromCartTen(productID: String) {
        viewModelScope.launch(
            Dispatchers.IO
        ) {
            decrementTenQuantityUseCase(productID = productID, Constants.corredor)
                .onResponse(
                    onLoading = {},
                    onSuccess = {},
                    onFailure = {}
                )
        }
    }

}