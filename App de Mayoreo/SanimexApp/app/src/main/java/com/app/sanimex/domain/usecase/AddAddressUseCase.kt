package com.app.sanimex.domain.usecase

import com.app.sanimex.core.util.Resource
import com.app.sanimex.domain.model.DireccionResponse
import com.app.sanimex.domain.model.LoginResponse
import com.app.sanimex.domain.repository.NetworkRepository
import kotlinx.coroutines.flow.Flow
import javax.inject.Inject

class AddAddressUseCase @Inject constructor(
    private val networkRepository: NetworkRepository
) {
    suspend operator fun invoke(
        direccion: String,
        latitu: Double,
        longitu: Double,
        claveSucursal: String,
        tipoIngreso: Boolean,
    ) : Flow<Resource<DireccionResponse>> {
        return networkRepository.addAddress (
            direccion = direccion,
            latitud = latitu,
            longitu = longitu,
            claveSucursal = claveSucursal,
            tipoIngreso = tipoIngreso,
        )
    }
}