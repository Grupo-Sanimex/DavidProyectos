package com.app.sanimex.presentation.screens.MapsSucursal

import android.util.Log
import androidx.compose.runtime.toMutableStateList
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.app.sanimex.core.util.Constants
import com.app.sanimex.core.util.onResponse
import com.app.sanimex.domain.usecase.Maps.GetUserSucursalUseCase
import dagger.hilt.android.lifecycle.HiltViewModel
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.flow.update
import kotlinx.coroutines.launch
import java.time.LocalDate
import java.time.format.DateTimeFormatter
import javax.inject.Inject
import retrofit2.HttpException
import java.io.IOException

@HiltViewModel
class MapsSucursalViewModel @Inject constructor(
    private val getUserSucursalUseCase: GetUserSucursalUseCase
) : ViewModel() {
    private val _uiState = MutableStateFlow(MapsSucursalScreenUiState())
    val uiState = _uiState.asStateFlow()

    init {
        val currentDate = LocalDate.now()
        val formatter = DateTimeFormatter.ofPattern("yyyy-MM-dd")
        val formattedDate = currentDate.format(formatter)
        if (Constants.fechaUbicaion.isEmpty()){
            getSucursales(formattedDate)
        }else{
            getSucursales(Constants.fechaUbicaion)
        }
    }

    fun getSucursales(fecha: String) {
        Constants.fechaUbicaion = fecha
        viewModelScope.launch(Dispatchers.IO) {
          //  Log.d("SucursalViewModel", "Getting sucursales for date: $fecha")
            getUserSucursalUseCase(fecha).onResponse(
                onLoading = {
                    _uiState.update { it.copy(
                        isLoading = true,
                        error = null
                    ) }
                },
                onSuccess = { sucursales ->
                    Constants.sucursalUbicacion = sucursales?.firstOrNull()?.nombreSucursal.toString()
                    if (sucursales != null) {
                        _uiState.update { 
                            it.copy(
                                isLoading = false,
                                sucursales = sucursales,
                                selectedDate = fecha,
                                error = null
                            )
                        }
                    } else {
                        Log.d("SucursalViewModel", "Sucursales is null")
                        _uiState.update { 
                            it.copy(
                                isLoading = false,
                                sucursales = emptyList(),
                                selectedDate = fecha,
                                error = null
                            )
                        }
                    }
                },
                onFailure = {
                    _uiState.update { 
                        it.copy(
                            isLoading = false,
                            sucursales = emptyList(),
                            error = "Error al cargar las sucursales"
                        )
                    }
                }
            )
        }
    }

    fun updateSelectedDate(date: String) {
        Log.d("SucursalViewModel", "Updating date to: $date")
        _uiState.update { it.copy(selectedDate = date) }
        getSucursales(date)
    }
}