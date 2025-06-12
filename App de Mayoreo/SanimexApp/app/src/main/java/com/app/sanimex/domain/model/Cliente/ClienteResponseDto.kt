package com.app.sanimex.domain.model.Cliente

import com.app.sanimex.domain.usecase.Cliente.ClienteResponse

data class ClienteResponseDto(
    val status: String,
    val message: String
){

    fun toClienteResponse(): ClienteResponse {
        return ClienteResponse(
            status = status,
            message = message
        )
    }
}
