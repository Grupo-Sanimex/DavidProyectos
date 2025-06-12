package com.app.sanimex.presentation.screens.single_product


import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.app.sanimex.core.util.Constants
import com.app.sanimex.core.util.onResponse
import com.app.sanimex.domain.model.Producto.ClientData
import com.app.sanimex.domain.usecase.AddToCartUseCase
import com.app.sanimex.domain.usecase.DeleteFromCartUseCase
import com.app.sanimex.domain.usecase.GetProducto.GetSingleClientUseCase
import com.app.sanimex.domain.usecase.GetSingleProductUseCase
import com.app.sanimex.domain.usecase.SearchUseCase
import dagger.hilt.android.lifecycle.HiltViewModel
import kotlinx.coroutines.DelicateCoroutinesApi
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.Job
import kotlinx.coroutines.delay
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.flow.update
import kotlinx.coroutines.launch
import javax.inject.Inject

@HiltViewModel
class SingleProductViewModel @Inject constructor(
    private val searchUseCase: SearchUseCase,
    private val getSingleProductUseCase: GetSingleProductUseCase,
    private val getSingleClientUseCase: GetSingleClientUseCase,
    private val addToCartUseCase: AddToCartUseCase,
    private val deleteFromCartUseCase: DeleteFromCartUseCase,
) : ViewModel() {

    private val _uiState = MutableStateFlow(SingleProductScreenUIState())
    val uiState = _uiState.asStateFlow()


    @OptIn(DelicateCoroutinesApi::class)
    fun onEmailChanged(newValue: String) {

        val processedValue = newValue.filter { it.isDigit() }.take(7)
        _uiState.update { it.copy(cliente = processedValue) }
        cleanClient()
        // Lanzar una corrutina para ejecutar getReporteData() después de 5 segundos
        //GlobalScope.launch {
           // delay(3000) // Esperar 3000 milisegundos (3 segundos)
            //getProductData()
        //}
    }
    fun onTipoEntregaChanged(newValue: Boolean) {
        _uiState.update { it.copy(tipoEntrega = newValue)  }
        getClientData()
    }
    fun ontipoPagoChanged(newValue: Boolean) {
        _uiState.update { it.copy(tipoPago = newValue)  }
        getClientData()
    }


    private var searchJob: Job? = null

    fun onSearchQueryChanged(newValue: String) {
        _uiState.update { it.copy(searchQuery = newValue) }
        if (newValue.isEmpty()) {
            searchJob?.cancel()
            _uiState.update { it.copy(searchResultsList = emptyList()) }
        } else {
            searchForProduct()
        }
    }
    private fun searchForProduct() {
        searchJob?.cancel()
        searchJob = viewModelScope.launch(Dispatchers.IO) {
            delay(500)
            searchUseCase(query = uiState.value.searchQuery).onResponse(
                onLoading = {},
                onSuccess = { results ->
                    _uiState.update { it.copy(searchResultsList = results!!.map { product -> product.name }) }
                },
                onFailure = {
                    _uiState.update { it.copy(searchResultsList = emptyList()) }
                }
            )
        }
    }


    fun onAddToCartClicked() {
        viewModelScope.launch {
            addToCartUseCase(
                uiState.value.productData.productInfo.id,
                descripsion =  uiState.value.productData.productInfo.description,
                corredor = Constants.corredor,
                uiState.value.clientData.clientInfo.precioFinal,
                cantidad = 1,
                claveCliente = Constants.ClaveCliente,
                recoge = Constants.tipoEntrega,
                contado =  Constants.tipoPago,
                tipoConsulta = Constants.tipoConsulta
            ).onResponse(
                onLoading = {
                    _uiState.update { it.copy(isButtonLoading = true) }
                },
                onSuccess = {
                    _uiState.update {
                        it.copy(
                            isButtonLoading = false,
                            isAddButton = false
                        )
                    }
                },
                onFailure = {
                    _uiState.update { it.copy(isButtonLoading = false, isErrorCart = true) }
                }
            )
        }
    }
    fun onResetErrorCart() {
        _uiState.update { it.copy(isErrorCart = false) }
    }
    fun onResetErrorCliente() {
        _uiState.update { it.copy(isErrorCliente = false) }
    }

    fun onDeleteFromCartClicked() {
        viewModelScope.launch {
            deleteFromCartUseCase(
                uiState.value.productData.productInfo.id
            ).onResponse(
                onLoading = {
                    _uiState.update { it.copy(isButtonLoading = true) }
                },
                onSuccess = {
                    _uiState.update { it.copy(isButtonLoading = false, isAddButton = true) }
                },
                onFailure = {
                    _uiState.update { it.copy(isButtonLoading = false) }
                }
            )
        }
    }

    fun getProductData(productID: String) {
        Constants.productID = productID
        viewModelScope.launch(Dispatchers.IO) {
            getSingleProductUseCase(
                productID = productID,
                corredor = Constants.corredor,
            ).onResponse(
                onLoading = {
                    _uiState.update { it.copy(isLoading = true) }
                },
                onSuccess = { result ->
                    _uiState.update {
                        it.copy(
                            isLoading = false,
                            productData = result!!,
                            isError = false,
                            isAddButton = !result.productInfo.isInCart
                        )
                    }
                    if (Constants.cliente != ""){
                        getClientData()
                    }
                },
                onFailure = { msg ->
                    _uiState.update {
                        it.copy(
                            isLoading = false,
                            isError = true,
                            errorMsg = msg
                        )
                    }
                }
            )
        }
    }
    fun getClientData() {
        Constants.cliente = uiState.value.cliente
        Constants.ClaveCliente = uiState.value.cliente
        Constants.tipoEntrega = uiState.value.tipoEntrega
        Constants.tipoPago = uiState.value.tipoPago
        Constants.descripsion = uiState.value.productData.productInfo.description
        Constants.precioFinal = uiState.value.productData.productInfo.precioProducto
        Constants.cantidad = 1
        Constants.tipoConsulta
        viewModelScope.launch(Dispatchers.IO) {
            getSingleClientUseCase(
                productID = Constants.productID,
                corredor = Constants.corredor,
                ClaveCliente = uiState.value.cliente,
                tipoEntrega =  Constants.tipoEntrega,
                tipoPago = Constants.tipoPago,
                TipoConsulta = Constants.tipoConsulta,
                idDireccion = Constants.idDireccion
            ).onResponse(
                onLoading = {
                    _uiState.update { it.copy(isLoading = true) }
                },
                onSuccess = { result ->
                    _uiState.update {
                        it.copy(
                            isLoading = false,
                            clientData = result!!,
                            isError = false
                        )
                    }
                },
                onFailure = { msg ->
                    _uiState.update {
                        it.copy(
                            isLoading = false,
                            isErrorCliente = true,
                            errorMsg = msg
                        )
                    }
                }
            )
        }
    }

    fun cleanClient(){
        _uiState.update { it.copy(tipoPago = false) } // Desmarcar el Checkbox
        _uiState.update { it.copy(tipoEntrega = false) } // Desmarcar el Checkbox
        _uiState.update { it.copy(clientData = ClientData()) } // Desmarcar el Checkbox
    }
}