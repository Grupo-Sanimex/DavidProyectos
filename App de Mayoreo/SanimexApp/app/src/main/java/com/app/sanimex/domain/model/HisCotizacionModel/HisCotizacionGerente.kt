package com.app.sanimex.domain.model.HisCotizacionModel

/**
 * Data class que representa la información de un gerente en el histórico de cotizaciones.
 *
 * Contiene los datos personales y de identificación de un gerente asociado a las cotizaciones históricas,
 * como su nombre completo, número de empleado e ID del dispositivo utilizado.
 *
 * @property nombre El nombre del gerente.
 * @property aPaterno El apellido paterno del gerente.
 * @property aMaterno El apellido materno del gerente.
 * @property numEmpleado El número de empleado del gerente.
 * @property idDispositivo El identificador único del dispositivo asociado al gerente.
 * @author David Duarte
 * @version 1.0
 */
data class HisCotizacionGerente(
    val nombre: String,
    val aPaterno : String,
    val aMaterno : String,
    val numEmpleado: String,
    val idDispositivo: String
)
