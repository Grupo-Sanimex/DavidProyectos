package com.app.sanimex.presentation.screens.favorites

import androidx.compose.runtime.mutableStateListOf
import androidx.compose.runtime.snapshots.SnapshotStateList
import com.app.sanimex.core.util.Constants
import com.app.sanimex.domain.model.Sucursal

data class FavoritesScreenUiState(
    val isLoading: Boolean = true,
    val wishlist: SnapshotStateList<Sucursal> = mutableStateListOf(),
    val error: String? = null,
    val errorDireccion : Boolean = false,
    val idRol : Int = Constants.idRol,
    val isPermissionDenied: Boolean = false, // Nuevo estado
    val errorMessage: String? = null
)
