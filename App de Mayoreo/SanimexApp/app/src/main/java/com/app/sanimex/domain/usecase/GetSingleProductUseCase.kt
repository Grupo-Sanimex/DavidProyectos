package com.app.sanimex.domain.usecase

import com.app.sanimex.core.util.Resource
import com.app.sanimex.domain.model.ProductData
import com.app.sanimex.domain.repository.NetworkRepository
import kotlinx.coroutines.flow.Flow
import javax.inject.Inject

class GetSingleProductUseCase @Inject constructor(
    private val networkRepository: NetworkRepository
) {
    suspend operator fun invoke(productID: String, corredor: String): Flow<Resource<ProductData>> {
        return networkRepository.getSingleProduct(id = productID, corredor = corredor)
    }
}