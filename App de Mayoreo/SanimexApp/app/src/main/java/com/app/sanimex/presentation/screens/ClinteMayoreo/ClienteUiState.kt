package com.app.sanimex.presentation.screens.ClinteMayoreo

data class ClienteUiState(
    val isLoading: Boolean = false,
    val isLoginSuccessful: Boolean = false,
    val isError: Boolean = false,
    val isErrorFailed: Boolean = false,
    val errorMessage: String = ""
)
