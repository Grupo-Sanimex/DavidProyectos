package com.app.sanimex.presentation.screens.offer

import com.app.sanimex.domain.model.HistoricoVtaMayItem


data class OfferScreenUIState(
    val isLoading: Boolean = true,
    val invoiceList: List<HistoricoVtaMayItem> = emptyList()
)