package com.app.sanimex.domain.usecase.cart

import com.app.sanimex.core.util.Resource
import com.app.sanimex.domain.repository.NetworkRepository
import kotlinx.coroutines.flow.Flow
import javax.inject.Inject

class ChanQuantityIcreTenUseCase@Inject constructor(
    private val networkRepository: NetworkRepository
) {
    suspend operator fun invoke(productID: String, corredor: String): Flow<Resource<Unit>>
    {
        return networkRepository.sumaDiezFromCart(productID = productID, corredor = corredor)
    }

}