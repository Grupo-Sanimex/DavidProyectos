package com.app.sanimex.domain.usecase.Ubicacion

import com.app.sanimex.core.util.Resource
import com.app.sanimex.domain.model.Ubicacion.UbicacionMaps
import com.app.sanimex.domain.repository.NetworkRepository
import kotlinx.coroutines.flow.Flow
import javax.inject.Inject

class GetUbicacionesMapsUseCase @Inject constructor(
    private val networkRepository: NetworkRepository
) {
    suspend operator fun invoke(
        claveSucursal: String,
        numeroEmpleado: String,
        fecha: String
    ): Flow<Resource<List<UbicacionMaps>>> {
        return networkRepository.getUbicacionesMaps(claveSucursal, numeroEmpleado, fecha)
    }
} 