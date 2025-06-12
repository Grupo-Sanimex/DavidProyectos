package com.app.sanimex.domain.model.HisCotizacionModel


/**
 * Data class que representa el detalle de un item en el histórico de cotizaciones.
 *
 * Contiene información detallada sobre un producto incluido en una cotización histórica,
 * como su código de barras, descripción, cantidad, precio unitario, estado, el nombre del cliente
 * asociado y su clasificación.
 *
 * @property codebar El código de barras del producto.
 * @property description La descripción del producto.
 * @property cantidad La cantidad del producto en la cotización.
 * @property precioUnitario El precio unitario del producto en la cotización.
 * @property status El estado del item en la cotización (ej. Activo, Inactivo).
 * @property nombreCliente El nombre del cliente asociado a la cotización.
 * @property clasificacion La clasificación del producto (ej. Categoría).
 * @author David Duarte
 * @version 1.0
 */
data class HisCotizacionDetalle(
    val codebar: String,
    val description : String,
    val cantidad: String,
    val precioUnitario: Float,
    val status: String,
    val nombreCliente: String,
    val clasificacion: String
)
