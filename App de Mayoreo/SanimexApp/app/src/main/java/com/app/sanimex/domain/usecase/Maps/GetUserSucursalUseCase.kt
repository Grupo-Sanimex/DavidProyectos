package com.app.sanimex.domain.usecase.Maps

import com.app.sanimex.core.util.Resource
import com.app.sanimex.domain.model.MapsSucursal.MapsUserSucursal
import com.app.sanimex.domain.repository.NetworkRepository
import kotlinx.coroutines.flow.Flow
import javax.inject.Inject

class GetUserSucursalUseCase @Inject constructor(
    private val networkRepository: NetworkRepository
) {
    suspend operator fun invoke(fecha: String): Flow<Resource<List<MapsUserSucursal>>> {
        return networkRepository.getUserSucursal(fecha)
    }
}