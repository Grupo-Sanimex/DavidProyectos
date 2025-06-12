package com.app.sanimex.domain.usecase.Cotizacion

import com.app.sanimex.core.util.Resource
import com.app.sanimex.domain.model.HisCotizacionModel.HisCotizacionDetalle
import com.app.sanimex.domain.repository.NetworkRepository
import kotlinx.coroutines.flow.Flow
import javax.inject.Inject

class GetHisCtoDetalleUseCase@Inject constructor(
    private val networkRepository: NetworkRepository
)  {
    suspend operator fun invoke(idCotizacion: String): Flow<Resource<List<HisCotizacionDetalle>>> {
        return networkRepository.getHisCotizacionDetalle(idCotizacion)
    }
}