package com.app.sanimex.core.navigation

import com.app.sanimex.core.navigation.ScreenRoutes.ACCOUNT_SCREEN_ROUTE
import com.app.sanimex.core.navigation.ScreenRoutes.CART_SCREEN_ROUTE
import com.app.sanimex.core.navigation.ScreenRoutes.CATEGORY_PRODUCTS_SCREEN_ROUTE
import com.app.sanimex.core.navigation.ScreenRoutes.CLIENTE_SCREEN_ROUTE
import com.app.sanimex.core.navigation.ScreenRoutes.EMAIL_ENTER_SCREEN_ROUTE
import com.app.sanimex.core.navigation.ScreenRoutes.EXPLORE_SCREEN_ROUTE
import com.app.sanimex.core.navigation.ScreenRoutes.FAVORITES_SCREEN_ROUTE
import com.app.sanimex.core.navigation.ScreenRoutes.HISTORIALCOTIZACIONG_SCREEN_ROUTE
import com.app.sanimex.core.navigation.ScreenRoutes.HISTORIALCOTIZACION_SCREEN_ROUTE
import com.app.sanimex.core.navigation.ScreenRoutes.HISTORIAL_COTIZACION_DETALLE_SCREEN_ROUTE
import com.app.sanimex.core.navigation.ScreenRoutes.HIS_COTIZACION_DETALLE_SCREEN_ROUTE
import com.app.sanimex.core.navigation.ScreenRoutes.HIS_CTO_DETALLE_GERENTE_ID_SCREEN_ROUTE
import com.app.sanimex.core.navigation.ScreenRoutes.HOME_SCREEN_ROUTE
import com.app.sanimex.core.navigation.ScreenRoutes.LOGIN_SCREEN_ROUTE
import com.app.sanimex.core.navigation.ScreenRoutes.OFFER_SCREEN_ROUTE
import com.app.sanimex.core.navigation.ScreenRoutes.ORDERS_SCREEN_ROUTE
import com.app.sanimex.core.navigation.ScreenRoutes.SUCURSAL_SCREEN_ROUTE
import com.app.sanimex.core.navigation.ScreenRoutes.SEARCH_RESULTS_SCREEN_ROUTE
import com.app.sanimex.core.navigation.ScreenRoutes.SINGLE_PRODUCT_INFO_ROUTE
import com.app.sanimex.core.navigation.ScreenRoutes.SUCCESS_SCREEN_ROUTE
import com.app.sanimex.core.navigation.ScreenRoutes.MAPSUCURSAL_SCREEN_ROUTE
import com.app.sanimex.core.navigation.ScreenRoutes.RUTAVISITADOR_SCREEN_ROUTE
import com.app.sanimex.core.navigation.ScreenRoutes.TIPO_INGRESO_SCREEN_ROUTE
import com.app.sanimex.core.navigation.ScreenRoutes.VISITADORACTIVO_SCREEN_ROUTE

/**
 * Clase sellada que define las rutas de navegación de la aplicación.
 *
 * Cada objeto dentro de esta clase representa una pantalla diferente de la aplicación
 * y contiene la ruta de navegación asociada. Esto facilita la gestión y el uso de las rutas
 * dentro del sistema de navegación de Compose.
 *
 * @property route La cadena que representa la ruta de navegación única para cada pantalla.
 * @author David Duarte
 * @version 1.0
 */
sealed class AppScreens(val route: String) {
    /**
     * Representa la pantalla de inicio de sesión.
     */
    object Login : AppScreens(route = LOGIN_SCREEN_ROUTE)
    /**
     * Representa la pantalla de registro de nuevos usuarios.
     */
    /**
     * Representa la pantalla principal o "home" de la aplicación.
     */
    object Home : AppScreens(route = HOME_SCREEN_ROUTE)
    /**
     * Representa la pantalla que muestra los resultados de una búsqueda.
     */
    object SearchResults : AppScreens(route = SEARCH_RESULTS_SCREEN_ROUTE)
    object SingleProductInfo : AppScreens(route = SINGLE_PRODUCT_INFO_ROUTE)
    object Explore : AppScreens(route = EXPLORE_SCREEN_ROUTE)
    object Cart : AppScreens(route = CART_SCREEN_ROUTE)
    object Account : AppScreens(route = ACCOUNT_SCREEN_ROUTE)
    object Favorites : AppScreens(route = FAVORITES_SCREEN_ROUTE)
    object Success : AppScreens(route = SUCCESS_SCREEN_ROUTE)
    object Orders : AppScreens(route = ORDERS_SCREEN_ROUTE)
    object Sucursal : AppScreens(route = SUCURSAL_SCREEN_ROUTE)
    object Offer : AppScreens(route = OFFER_SCREEN_ROUTE)
    object CategoryProducts : AppScreens(route = CATEGORY_PRODUCTS_SCREEN_ROUTE)
    object EmailEnter : AppScreens(route = EMAIL_ENTER_SCREEN_ROUTE)
    object MapsSucursal : AppScreens(route = MAPSUCURSAL_SCREEN_ROUTE)
    object VisitadorActivo : AppScreens(route = VISITADORACTIVO_SCREEN_ROUTE)
    object RutaVisitador : AppScreens(route = RUTAVISITADOR_SCREEN_ROUTE)
    object HistorialCotizacion : AppScreens(route = HISTORIALCOTIZACION_SCREEN_ROUTE)
    object HistorialCotizacionGerente : AppScreens(route = HISTORIALCOTIZACIONG_SCREEN_ROUTE)
    object HistorialCotizacionDetalle : AppScreens(route = HISTORIAL_COTIZACION_DETALLE_SCREEN_ROUTE)
    object HisCotizacionDetalleGerente : AppScreens(route = HIS_COTIZACION_DETALLE_SCREEN_ROUTE)
    object HisCtoDetalleGerenteId : AppScreens(route = HIS_CTO_DETALLE_GERENTE_ID_SCREEN_ROUTE)
    object Cliente : AppScreens(route = CLIENTE_SCREEN_ROUTE)
    object TipoIngreso : AppScreens(route = TIPO_INGRESO_SCREEN_ROUTE)
}
