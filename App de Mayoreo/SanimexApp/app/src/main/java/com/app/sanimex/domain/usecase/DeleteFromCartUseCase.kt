package com.app.sanimex.domain.usecase

import com.app.sanimex.core.util.Resource
import com.app.sanimex.domain.repository.NetworkRepository
import kotlinx.coroutines.flow.Flow
import javax.inject.Inject

class DeleteFromCartUseCase @Inject constructor(
    private val networkRepository: NetworkRepository
) {
    suspend operator fun invoke(productID: String): Flow<Resource<Unit>> {
        return networkRepository.removeFromCart(productID = productID)
    }
}