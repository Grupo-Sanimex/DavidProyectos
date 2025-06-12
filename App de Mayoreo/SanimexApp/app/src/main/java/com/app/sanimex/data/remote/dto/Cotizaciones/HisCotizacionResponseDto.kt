package com.app.sanimex.data.remote.dto.Cotizaciones

import com.app.sanimex.domain.model.HisCotizacionModel.HisCotizacionM

/**
 * Data class que representa la respuesta de la API para el histórico de cotizaciones.
 *
 * Contiene una lista de objetos [HistoricoVtaMay], cada uno representando la información
 * de una cotización en el histórico de ventas.
 *
 * @property historicoVtaMay La lista de objetos [HistoricoVtaMay] que contienen la información del histórico de ventas.
 * @author David Duarte
 * @version 1.0
 */
data class HisCotizacionResponseDto(
    val historicoVtaMay: List<HistoricoVtaMay>
){
    /**
     * Data class interna que representa la información de una cotización en el histórico de ventas (Venta Mayor).
     *
     * Contiene detalles como el ID de la cotización, el total, el ID del cliente SAP, el estado,
     * la fecha, la hora y el ID de la venta.
     *
     * @property idCotizacion El identificador único de la cotización.
     * @property totalCotizacion El total de la cotización.
     * @property idClienteSAP El ID del cliente en el sistema SAP.
     * @property status El estado actual de la cotización.
     * @property fecha La fecha en la que se realizó la cotización.
     * @property hora La hora en la que se realizó la cotización.
     * @property idventa El identificador único de la venta asociada a la cotización.
     */
    data class HistoricoVtaMay(
        val idCotizacion: String,
        val totalCotizacion: Float,
        val idClienteSAP: String,
        val status: String,
        val fecha :String,
        val hora: String,
        val idventa: String
    ) {
        /**
         * Función para mapear un objeto [HistoricoVtaMay] a un objeto [HisCotizacionM].
         *
         * Esta función facilita la conversión entre el DTO de la API y el modelo de dominio
         * utilizado en la capa de presentación ([HisCotizacionM]).
         *
         * @return Un nuevo objeto [HisCotizacionM] con los datos correspondientes del [HistoricoVtaMay].
         */
        fun toHistoricoVtaMayItem(): HisCotizacionM {
            return HisCotizacionM(
                idCotizacion = idCotizacion,
                totalCotizacion = totalCotizacion,
                idClienteSAP = idClienteSAP,
                status = status,
                fecha = fecha,
                hora = hora,
                idventa = idventa
            )
        }
    }
}
