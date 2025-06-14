package com.app.sanimex.presentation.screens.search_results

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.app.sanimex.core.util.onResponse
import com.app.sanimex.domain.model.BusquedaProducto
import com.app.sanimex.domain.usecase.SearchUseCase
import dagger.hilt.android.lifecycle.HiltViewModel
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.Job
import kotlinx.coroutines.delay
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.flow.update
import kotlinx.coroutines.launch
import javax.inject.Inject

@HiltViewModel
class SearchResultsViewModel @Inject constructor(
    private val searchUseCase: SearchUseCase
) : ViewModel() {

    private val _uiState = MutableStateFlow(SearchResultsUIState())
    val uiState = _uiState.asStateFlow()
    private var searchJob: Job? = null

    fun onSearchQueryChanged(newValue: String) {
        _uiState.update { it.copy(searchQuery = newValue) }
        if (newValue.isEmpty()) {
            searchJob?.cancel()
            _uiState.update { it.copy(resultsList = emptyList()) }
        } else {
            searchForProduct()
        }
    }

    fun sortResults() {
    }

    fun filterResults(
    ) {
    }

    private fun searchForProduct() {
        _uiState.update { it.copy(isLoading = true) }
        searchJob?.cancel()
        searchJob = viewModelScope.launch(Dispatchers.IO) {
            delay(500)
            searchUseCase(query = uiState.value.searchQuery).onResponse(
                onLoading = {
                    _uiState.update { it.copy(isLoading = true) }
                },
                onSuccess = { results ->
                    _uiState.update {
                        it.copy(
                            resultsList = results!!,
                            originalResults = results,
                            isLoading = false
                        )
                    }
                },
                onFailure = {
                    _uiState.update { it.copy(isLoading = false) }
                }
            )
        }
    }

    fun setInitialStartingQuery(value: String) {
        onSearchQueryChanged(newValue = value)
    }
}