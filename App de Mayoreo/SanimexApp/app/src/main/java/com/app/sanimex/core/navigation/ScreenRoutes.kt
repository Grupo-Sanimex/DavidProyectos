package com.app.sanimex.core.navigation

/**
 * Objeto que contiene las constantes de las rutas de navegación de la aplicación.
 *
 * Este objeto centraliza la definición de todas las rutas de la aplicación como constantes de cadena.
 * Esto ayuda a garantizar la coherencia y facilita la refactorización de las rutas en caso de ser necesario.
 *
 * @author David Duarte
 * @version 1.0
 */
object ScreenRoutes {
    /**
     * Constante que define la ruta para la pantalla de inicio de sesión.
     */
    const val LOGIN_SCREEN_ROUTE = "LOGIN_SCREEN_ROUTE"
    /**
     * Constante que define la ruta para la pantalla de registro de nuevos usuarios.
     */
    const val SIGNUP_SCREEN_ROUTE = "SIGNUP_SCREEN_ROUTE"
    /**
     * Constante que define la ruta para la pantalla principal o "home" de la aplicación.
     */
    const val HOME_SCREEN_ROUTE = "HOME_SCREEN_ROUTE"
    /**
     * Constante que define la ruta para la pantalla que muestra los resultados de una búsqueda.
     * Incluye un parámetro dinámico `{query}` para la consulta de búsqueda.
     */
    const val SEARCH_RESULTS_SCREEN_ROUTE = "SEARCH_RESULTS_SCREEN_ROUTE/{query}"
    /**
     * Constante que define la ruta para la pantalla que muestra la información detallada de un único producto.
     * Incluye un parámetro dinámico `{product_id}` para el identificador del producto.
     */
    const val SINGLE_PRODUCT_INFO_ROUTE = "SINGLE_PRODUCT_INFO_ROUTE/{product_id}"
    /**
     * Constante que define la ruta para la pantalla de exploración general de la aplicación.
     * Incluye un parámetro dinámico opcional `{categories}` para filtrar por categorías.
     */
    const val EXPLORE_SCREEN_ROUTE = "EXPLORE_SCREEN_ROUTE/{categories}"
    const val CART_SCREEN_ROUTE = "CART_SCREEN_ROUTE"
    const val ACCOUNT_SCREEN_ROUTE = "ACCOUNT_SCREEN_ROUTE"
    const val FAVORITES_SCREEN_ROUTE = "FAVORITES_SCREEN_ROUTE"
    const val SUCCESS_SCREEN_ROUTE = "SUCCESS_SCREEN_ROUTE"
    const val ORDERS_SCREEN_ROUTE = "ORDERS_SCREEN_ROUTE"
    const val SUCURSAL_SCREEN_ROUTE = "SUCURSAL_SCREEN_ROUTE"
    const val OFFER_SCREEN_ROUTE = "OFFER_SCREEN_ROUTE"
    const val CATEGORY_PRODUCTS_SCREEN_ROUTE = "CATEGORY_PRODUCTS_SCREEN_ROUTE/{category_name}"
    const val EMAIL_ENTER_SCREEN_ROUTE = "EMAIL_ENTER_SCREEN_ROUTE"
    const val VISITADORACTIVO_SCREEN_ROUTE = "VISITADORACTIVO_SCREEN_ROUTE/{claveSucursal}/{fecha}"
    const val RUTAVISITADOR_SCREEN_ROUTE = "RUTAVISITADOR_SCREEN_ROUTE/{claveSucursal}/{numeroEmpleado}/{fecha}"
    const val MAPSUCURSAL_SCREEN_ROUTE = "MAPSUCURSAL_SCREEN_ROUTE"
    const val HISTORIALCOTIZACION_SCREEN_ROUTE = "HISTORIALCOTIZACION_SCREEN_ROUTE"
    const val HISTORIALCOTIZACIONG_SCREEN_ROUTE = "HISTORIALCOTIZACIONG_SCREEN_ROUTE"
    const val HISTORIAL_COTIZACION_DETALLE_SCREEN_ROUTE = "HISTORIAL_COTIZACION_DETALLE_SCREEN_ROUTE/{idCotizacion}"
    const val HIS_COTIZACION_DETALLE_SCREEN_ROUTE = "HIS_COTIZACION_DETALLE_SCREEN_ROUTE/{fecha}/{idVistador}"
    const val HIS_CTO_DETALLE_GERENTE_ID_SCREEN_ROUTE = "HIS_CTO_DETALLE_GERENTE_ID_SCREEN_ROUTE/{idCotizacion}"
    const val CLIENTE_SCREEN_ROUTE = "CLIENTE_SCREEN_ROUTE"
    const val TIPO_INGRESO_SCREEN_ROUTE = "TIPO_INGRESO_SCREEN_ROUTE"
}