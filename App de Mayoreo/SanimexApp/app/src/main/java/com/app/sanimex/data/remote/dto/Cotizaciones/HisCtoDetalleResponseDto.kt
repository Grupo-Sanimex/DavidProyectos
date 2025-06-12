package com.app.sanimex.data.remote.dto.Cotizaciones

import com.app.sanimex.domain.model.HisCotizacionModel.HisCotizacionDetalle

/**
 * Data class que representa la respuesta de la API para el detalle de una cotización.
 *
 * Contiene una lista de objetos [CotizacionDetalle], cada uno representando el detalle
 * de un item dentro de la cotización.
 *
 * @property cotizacionDetalle La lista de objetos [CotizacionDetalle] que contienen los detalles de los items de la cotización.
 * @author David Duarte
 * @version 1.0
 */
data class HisCtoDetalleResponseDto(
    val cotizacionDetalle: List<CotizacionDetalle>,
){
    /**
     * Data class interna que representa el detalle de un item dentro de una cotización.
     *
     * Contiene información como el código de barras, la descripción, la cantidad, el precio unitario,
     * el estado, el cliente y la clasificación del producto.
     *
     * @property codebar El código de barras del producto.
     * @property description La descripción del producto.
     * @property cantidad La cantidad del producto en la cotización.
     * @property precioUnitario El precio unitario del producto.
     * @property status El estado del item en la cotización.
     * @property cliente El nombre del cliente asociado a la cotización.
     * @property clasificacion La clasificación del producto.
     */
    data class CotizacionDetalle(
        val codebar : String,
        val description : String,
        val cantidad : String,
        val precioUnitario : Float,
        val status : String,
        val cliente : String,
        val clasificacion : String
    ){
        /**
         * Función para mapear un objeto [CotizacionDetalle] a un objeto [HisCotizacionDetalle].
         *
         * Esta función facilita la conversión entre el DTO de la API y el modelo de dominio
         * utilizado en la capa de presentación ([HisCotizacionDetalle]).
         *
         * @return Un nuevo objeto [HisCotizacionDetalle] con los datos correspondientes del [CotizacionDetalle].
         * Note que la propiedad `cliente` del DTO se mapea a `nombreCliente` en el modelo de dominio.
         */
        fun toCotizacionDetalleItem(): HisCotizacionDetalle {
            return HisCotizacionDetalle(
                codebar = codebar,
                description =  description,
                cantidad = cantidad,
                precioUnitario = precioUnitario,
                status = status,
                nombreCliente = cliente,
                clasificacion = clasificacion
            )
        }
    }
}
