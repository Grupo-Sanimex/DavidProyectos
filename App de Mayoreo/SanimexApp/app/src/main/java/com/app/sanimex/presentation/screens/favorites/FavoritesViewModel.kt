package com.app.sanimex.presentation.screens.favorites

import androidx.compose.runtime.toMutableStateList
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.app.sanimex.core.util.Constants
import com.app.sanimex.core.util.onResponse
import com.app.sanimex.domain.usecase.GetWishlistUseCase
import dagger.hilt.android.lifecycle.HiltViewModel
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.flow.update
import kotlinx.coroutines.launch
import javax.inject.Inject

@HiltViewModel
class FavoritesViewModel @Inject constructor(
    private val getWishlistUseCase: GetWishlistUseCase,
) : ViewModel() {
    private val _uiState = MutableStateFlow(FavoritesScreenUiState())
    val uiState = _uiState.asStateFlow()

     fun getWishlist() {
         Constants.cliente = ""
         Constants.tipoEntrega = false
         Constants.tipoPago = false
        viewModelScope.launch(Dispatchers.IO) {
            getWishlistUseCase()
                .onResponse(
                    onLoading = {
                        _uiState.update { it.copy(isLoading = true) }
                    },
                    onSuccess = { response ->
                        _uiState.update {
                            it.copy(
                                isLoading = false,
                                wishlist = response!!.toMutableStateList()
                            )
                        }
                        if (response != null) {
                            Constants.idRol = response.firstOrNull()?.idRol ?: 0
                            Constants.idUsuario = response.firstOrNull()?.idUsuario ?: ""
                        } // Extraer el idRol de la primera sucursa
                    },
                    onFailure = {
                        _uiState.update { it.copy(isLoading = false) }
                    }
                )
        }
    }


}