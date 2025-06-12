package com.app.sanimex.presentation.screens.tipoIngreso

data class TipoIngresoUiState(
    val isLoading: Boolean = false,
    val isLoginSuccessful: Boolean = false,
    val isError: Boolean = false,
    val isErrorFailed: Boolean = false,
    val errorDireccion : Boolean = false,
)
