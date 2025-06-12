package com.app.sanimex.domain.usecase.Visitador

import com.app.sanimex.core.util.Resource
import com.app.sanimex.domain.model.Visitador.VisitadorActivo
import com.app.sanimex.domain.repository.NetworkRepository
import kotlinx.coroutines.flow.Flow
import javax.inject.Inject

class GetVisitadoresActivosUseCase @Inject constructor(
    private val networkRepository: NetworkRepository
) {
    suspend operator fun invoke(
        claveSucursal: String,
        fecha: String
    ): Flow<Resource<List<VisitadorActivo>>> {
        return networkRepository.getVisitadoresActivos(claveSucursal, fecha)
    }
} 