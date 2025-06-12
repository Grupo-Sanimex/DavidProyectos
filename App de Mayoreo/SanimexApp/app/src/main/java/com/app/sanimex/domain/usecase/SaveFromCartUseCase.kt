package com.app.sanimex.domain.usecase

import com.app.sanimex.core.util.Resource
import com.app.sanimex.domain.repository.NetworkRepository
import kotlinx.coroutines.flow.Flow
import javax.inject.Inject

class SaveFromCartUseCase @Inject constructor(
    private val networkRepository: NetworkRepository
) {
    suspend operator fun invoke(): Flow<Resource<Unit>> {
        return networkRepository.saveCart()
    }
}