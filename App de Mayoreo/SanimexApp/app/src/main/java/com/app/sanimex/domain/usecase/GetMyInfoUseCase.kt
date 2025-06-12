package com.app.sanimex.domain.usecase

import com.app.sanimex.domain.repository.NetworkRepository
import javax.inject.Inject

class GetMyInfoUseCase @Inject constructor(
    private val networkRepository: NetworkRepository
) {
    suspend operator fun invoke() = networkRepository.getMyInfo()
}