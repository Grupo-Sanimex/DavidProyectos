package com.app.sanimex.presentation.screens.account

import com.app.sanimex.domain.model.User

data class AccountUiState(
    val isLoading: Boolean = true,
    val isError: Boolean = false,
    val me: User? = null
)
