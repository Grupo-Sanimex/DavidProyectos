package com.app.sanimex.domain.usecase.Cotizacion

import com.app.sanimex.core.util.Resource
import com.app.sanimex.domain.model.HisCotizacionModel.HisCotizacionGerente
import com.app.sanimex.domain.repository.NetworkRepository
import kotlinx.coroutines.flow.Flow
import javax.inject.Inject

class GetHisCotizacionGUseCase@Inject constructor(
    private val networkRepository: NetworkRepository
)  {
    suspend operator fun invoke(fecha: String): Flow<Resource<List<HisCotizacionGerente>>> {
        return networkRepository.getHisCotizacionGerente(fecha)
    }
}