package com.app.sanimex.domain.usecase

import com.app.sanimex.core.util.Resource
import com.app.sanimex.domain.model.LoginResponse
import com.app.sanimex.domain.repository.NetworkRepository
import kotlinx.coroutines.flow.Flow
import javax.inject.Inject

class LoginUseCase @Inject constructor(
    private val networkRepository: NetworkRepository
) {
    suspend operator fun invoke(email: String, password: String): Flow<Resource<LoginResponse>> {
        return networkRepository.login(email = email, password = password)
    }
}