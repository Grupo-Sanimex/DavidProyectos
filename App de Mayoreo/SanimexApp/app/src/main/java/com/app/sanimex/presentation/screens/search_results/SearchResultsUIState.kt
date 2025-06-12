package com.app.sanimex.presentation.screens.search_results

import com.app.sanimex.domain.model.BusquedaProducto

data class SearchResultsUIState(
    val resultsList: List<BusquedaProducto> = mutableListOf(),
    val originalResults: List<BusquedaProducto> = emptyList(),
    val searchQuery: String = "",
    val isLoading: Boolean = false
)
