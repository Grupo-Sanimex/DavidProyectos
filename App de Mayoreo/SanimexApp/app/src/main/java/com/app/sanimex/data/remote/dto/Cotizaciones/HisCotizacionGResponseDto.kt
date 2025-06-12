package com.app.sanimex.data.remote.dto.Cotizaciones

import com.app.sanimex.domain.model.HisCotizacionModel.HisCotizacionGerente

/**
 * Data class que representa la respuesta de la API para el histórico de cotizaciones de gerentes.
 *
 * Contiene una lista de objetos [HistoricoVtaMay], cada uno representando la información
 * de un gerente en el histórico de ventas.
 *
 * @property historicoVtaMay La lista de objetos [HistoricoVtaMay] que contienen la información del histórico de ventas de los gerentes.
 * @author David Duarte
 * @version 1.0
 */
data class HisCotizacionGResponseDto(
    val historicoVtaMay: List<HistoricoVtaMay>
){
    /**
     * Data class interna que representa la información de un gerente en el histórico de ventas (Venta Mayor).
     *
     * Contiene detalles como el nombre, apellidos, número de empleado e ID del dispositivo del gerente.
     *
     * @property nombre El nombre del gerente.
     * @property aPaterno El apellido paterno del gerente.
     * @property aMaterno El apellido materno del gerente.
     * @property numEmpleado El número de empleado del gerente.
     * @property idDispositivo El ID del dispositivo asociado al gerente.
     */
    data class HistoricoVtaMay(
        val nombre: String,
        val aPaterno: String,
        val aMaterno: String,
        val numEmpleado: String,
        val  idDispositivo :String
    ) {
        /**
         * Función para mapear un objeto [HistoricoVtaMay] a un objeto [HisCotizacionGerente].
         *
         * Esta función facilita la conversión entre el DTO de la API y el modelo de dominio
         * utilizado en la capa de presentación ([HisCotizacionGerente]).
         *
         * @return Un nuevo objeto [HisCotizacionGerente] con los datos correspondientes del [HistoricoVtaMay].
         */
        fun toHistoricoVtaGerenteItem(): HisCotizacionGerente {
            return HisCotizacionGerente(
                nombre = nombre,
                aPaterno = aPaterno,
                aMaterno = aMaterno,
                numEmpleado = numEmpleado,
                idDispositivo = idDispositivo
            )
        }
    }
}
