package com.app.sanimex.data.remote.dto

import com.app.sanimex.domain.model.DireccionResponse

data class DireccionResponseDto(
    val idDireccion: Int
){
    fun toDireccionResponse(): DireccionResponse {
        return DireccionResponse(
            idDireccion = idDireccion
        )
    }
}

