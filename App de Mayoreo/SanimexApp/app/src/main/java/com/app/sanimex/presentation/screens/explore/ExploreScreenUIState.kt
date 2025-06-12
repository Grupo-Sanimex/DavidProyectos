package com.app.sanimex.presentation.screens.explore

import com.app.sanimex.core.util.Constants

data class ExploreScreenUIState(
    val isLoading: Boolean = false,
    val searchQuery: String = "",
    val isError: Boolean = false,
    val corredor: String = Constants.corredor,
    val searchResultsList: List<String> = emptyList()
)
