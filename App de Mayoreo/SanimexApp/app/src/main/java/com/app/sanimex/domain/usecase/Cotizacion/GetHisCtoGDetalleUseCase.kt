package com.app.sanimex.domain.usecase.Cotizacion

import com.app.sanimex.core.util.Resource
import com.app.sanimex.domain.model.HisCotizacionModel.HisCotizacionM
import com.app.sanimex.domain.repository.NetworkRepository
import kotlinx.coroutines.flow.Flow
import javax.inject.Inject

class GetHisCtoGDetalleUseCase@Inject constructor(
    private val networkRepository: NetworkRepository
) {
    suspend operator fun invoke(fecha: String, idVistador: String): Flow<Resource<List<HisCotizacionM>>> {
        return networkRepository.getHisCtoGdetalle(fecha, idVistador)
    }
}