package com.app.sanimex.domain.model

/**
 * Data class que representa un producto encontrado en una búsqueda.
 *
 * Contiene la información básica de un producto resultante de una búsqueda,
 * incluyendo su identificador único, nombre, URL de la imagen y una breve descripción.
 *
 * @property id El identificador único del producto.
 * @property name El nombre del producto.
 * @property image La URL de la imagen del producto.
 * @property description Una breve descripción del producto.
 * @author David Duarte
 * @version 1.0
 */
data class BusquedaProducto(
    val id: String,
    val name: String,
    val image: String,
    val description: String
)
