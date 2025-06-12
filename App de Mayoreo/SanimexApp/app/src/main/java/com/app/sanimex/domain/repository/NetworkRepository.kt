package com.app.sanimex.domain.repository

import com.app.sanimex.core.util.Resource
import com.app.sanimex.domain.model.BusquedaProducto
import com.app.sanimex.domain.model.CarritoFinal
import com.app.sanimex.domain.model.DireccionResponse
import com.app.sanimex.domain.model.HisCotizacionModel.HisCotizacionDetalle
import com.app.sanimex.domain.model.HisCotizacionModel.HisCotizacionGerente
import com.app.sanimex.domain.model.HisCotizacionModel.HisCotizacionM
import com.app.sanimex.domain.model.HistoricoVtaMayItem
import com.app.sanimex.domain.model.LoginResponse
import com.app.sanimex.domain.model.MapsSucursal.MapsUserSucursal
import com.app.sanimex.domain.model.ProductData
import com.app.sanimex.domain.model.Producto.ClientData
import com.app.sanimex.domain.model.Sucursal
import com.app.sanimex.domain.model.User
import com.app.sanimex.domain.model.Visitador.VisitadorActivo
import com.app.sanimex.domain.model.Ubicacion.UbicacionMaps
import com.app.sanimex.domain.usecase.Cliente.ClienteResponse
import kotlinx.coroutines.flow.Flow

/**
 * Interfaz que define las operaciones para interactuar con la capa de red de la aplicación Sanimex.
 *
 * Cada función suspendida en esta interfaz representa una llamada a un endpoint específico de la API,
 * devolviendo un [Flow] de [Resource] que encapsula el estado de la petición (cargando, éxito o fallo)
 * y los datos o el mensaje de error correspondientes.
 *
 * @author David Duarte
 * @version 1.0
 */
interface NetworkRepository {

    /**
     * Realiza la llamada a la API para el inicio de sesión.
     *
     * @param email El correo electrónico del usuario.
     * @param password La contraseña del usuario.
     * @return Un [Flow] de [Resource]<[LoginResponse]> que emite el resultado de la operación.
     */
    suspend fun login(
        email: String,
        password: String
    ): Flow<Resource<LoginResponse>>

    /**
     * Realiza la llamada a la API para obtener la información de un cliente.
     *
     * @param claveCliente El ID del cliente.
     * @param corredor El código del corredor.
     * @return Un [Flow] de [Resource]<[ClienteResponse]> que emite el resultado de la operación.
     */
    suspend fun cliente(
        claveCliente: String,
        corredor: String
    ): Flow<Resource<ClienteResponse>>

    /**
     * Realiza la llamada a la API para agregar una nueva dirección.
     *
     * @param direccion La dirección a guardar.
     * @param latitud La latitud de la ubicación.
     * @param longitu La longitud de la ubicación.
     * @param claveSucursal La clave de la sucursal asociada.
     * @param tipoIngreso El tipo de ingreso de la ubicación.
     * @return Un [Flow] de [Resource]<[Unit]> que emite el resultado de la operación.
     */
    suspend fun addAddress(
        direccion: String,
        latitud: Double,
        longitu: Double,
        claveSucursal: String,
        tipoIngreso: Boolean
    ): Flow<Resource<DireccionResponse>>

    /**
     * Realiza la llamada a la API para buscar productos.
     *
     * @param query La cadena de búsqueda.
     * @return Un [Flow] de [Resource]<[List]<[BusquedaProducto]>> que emite el resultado de la búsqueda.
     */
    suspend fun search(query: String): Flow<Resource<List<BusquedaProducto>>>

    /**
     * Realiza la llamada a la API para obtener la información de un solo producto.
     *
     * @param id El ID del producto.
     * @param corredor El código del corredor.
     * @return Un [Flow] de [Resource]<[ProductData]> que emite el resultado de la operación.
     */
    suspend fun getSingleProduct(id: String, corredor: String): Flow<Resource<ProductData>>

    /**
     * Realiza la llamada a la API para obtener la información de un solo producto para un cliente específico.
     *
     * @param id El ID del producto.
     * @param corredor El código del corredor.
     * @param ClaveCliente La clave del cliente.
     * @param tipoEntrega Indica el tipo de entrega.
     * @param tipoPago Indica el tipo de pago.
     * @param TipoConsulta Indica el tipo de consulta.
     * @return Un [Flow] de [Resource]<[ClientData]> que emite el resultado de la operación.
     */
    suspend fun getSingleClient(
        id: String,
        corredor: String,
        ClaveCliente: String,
        tipoEntrega: Boolean,
        tipoPago: Boolean,
        TipoConsulta: Boolean,
        idDireccion: Int
    ): Flow<Resource<ClientData>>

    /**
     * Realiza la llamada a la API para agregar un producto al carrito de compras.
     *
     * @param productID El ID del producto a agregar.
     * @param descripsion La descripción del producto.
     * @param corredor El código del corredor.
     * @param precioFinal El precio final del producto.
     * @param cantidad La cantidad del producto a agregar.
     * @param claveCliente La clave del cliente.
     * @param recoge Indica si el cliente recogerá el producto.
     * @param contado Indica si el pago es al contado.
     * @param tipoConsulta El tipo de consulta.
     * @return Un [Flow] de [Resource]<[Unit]> que emite el resultado de la operación.
     */
    suspend fun addToCart(
        productID: String,
        descripsion: String,
        corredor: String,
        precioFinal: Float,
        cantidad: Int,
        claveCliente: String,
        recoge: Boolean,
        contado: Boolean,
        tipoConsulta: Boolean
    ): Flow<Resource<Unit>>

    /**
     * Realiza la llamada a la API para eliminar un producto del carrito de compras.
     *
     * @param productID El ID del producto a eliminar.
     * @return Un [Flow] de [Resource]<[Unit]> que emite el resultado de la operación.
     */
    suspend fun removeFromCart(productID: String): Flow<Resource<Unit>>

    /**
     * Realiza la llamada a la API para incrementar en uno la cantidad de un producto en el carrito.
     *
     * @param productID El ID del producto a incrementar.
     * @param corredor El código del corredor.
     * @return Un [Flow] de [Resource]<[Unit]> que emite el resultado de la operación.
     */
    suspend fun sumaUnoFromCart(productID: String, corredor: String): Flow<Resource<Unit>>

    /**
     * Realiza la llamada a la API para incrementar en diez la cantidad de un producto en el carrito.
     *
     * @param productID El ID del producto a incrementar.
     * @param corredor El código del corredor.
     * @return Un [Flow] de [Resource]<[Unit]> que emite el resultado de la operación.
     */
    suspend fun sumaDiezFromCart(productID: String, corredor: String): Flow<Resource<Unit>>

    /**
     * Realiza la llamada a la API para decrementar en uno la cantidad de un producto en el carrito.
     *
     * @param productID El ID del producto a decrementar.
     * @param corredor El código del corredor.
     * @return Un [Flow] de [Resource]<[Unit]> que emite el resultado de la operación.
     */
    suspend fun removeUnoFromCart(productID: String, corredor: String): Flow<Resource<Unit>>

    /**
     * Realiza la llamada a la API para decrementar en diez la cantidad de un producto en el carrito.
     *
     * @param productID El ID del producto a decrementar.
     * @param corredor El código del corredor.
     * @return Un [Flow] de [Resource]<[Unit]> que emite el resultado de la operación.
     */
    suspend fun removeDiezFromCart(productID: String, corredor: String): Flow<Resource<Unit>>

    /**
     * Realiza la llamada a la API para guardar el carrito de compras.
     *
     * @return Un [Flow] de [Resource]<[Unit]> que emite el resultado de la operación.
     */
    suspend fun saveCart(): Flow<Resource<Unit>>

    /**
     * Realiza la llamada a la API para obtener los productos del carrito de compras del usuario.
     *
     * @return Un [Flow] de [Resource]<[List]<[CarritoFinal]>> que emite la lista de productos en el carrito.
     */
    suspend fun getMyCart(): Flow<Resource<List<CarritoFinal>>>

    /**
     * Realiza la llamada a la API para obtener la ubicación de la sucursal del usuario para una fecha específica.
     *
     * @param fecha La fecha para la que se solicita la ubicación de la sucursal.
     * @return Un [Flow] de [Resource]<[List]<[MapsUserSucursal]>> que emite la lista de ubicaciones de la sucursal.
     */
    suspend fun getUserSucursal(fecha: String): Flow<Resource<List<MapsUserSucursal>>>

    /**
     * Realiza la llamada a la API para obtener la lista de sucursales en la lista de deseos del usuario.
     *
     * @return Un [Flow] de [Resource]<[List]<[Sucursal]>> que emite la lista de sucursales en la lista de deseos.
     */
    suspend fun getWishlist(): Flow<Resource<List<Sucursal>>>

    /**
     * Realiza la llamada a la API para obtener la información del usuario actual.
     *
     * @return Un [Flow] de [Resource]<[User]> que emite la información del usuario.
     */
    suspend fun getMyInfo(): Flow<Resource<User>>

    /**
     * Realiza la llamada a la API para obtener la lista de sucursales del usuario.
     *
     * @return Un [Flow] de [Resource]<[List]<[Sucursal]>> que emite la lista de sucursales del usuario.
     */
    suspend fun getMySucursal(): Flow<Resource<List<Sucursal>>>

    /**
     * Realiza la llamada a la API para obtener la lista de productos en oferta.
     *
     * @return Un [Flow] de [Resource]<[List]<[HistoricoVtaMayItem]>> que emite la lista de productos en oferta.
     */
    suspend fun getOfferProducts(): Flow<Resource<List<HistoricoVtaMayItem>>>

    /**
     * Realiza la llamada a la API para obtener la lista de visitadores activos para una sucursal y fecha específicas.
     *
     * @param claveSucursal La clave de la sucursal.
     * @param fecha La fecha para la que se solicitan los visitadores activos.
     * @return Un [Flow] de [Resource]<[List]<[VisitadorActivo]>> que emite la lista de visitadores activos.
     */
    suspend fun getVisitadoresActivos(
        claveSucursal: String,
        fecha: String
    ): Flow<Resource<List<VisitadorActivo>>>

    /**
     * Realiza la llamada a la API para obtener las ubicaciones en el mapa de un visitador para una sucursal, empleado y fecha específicos.
     *
     * @param claveSucursal La clave de la sucursal.
     * @param numeroEmpleado El número de empleado del visitador.
     * @param fecha La fecha para la que se solicitan las ubicaciones.
     * @return Un [Flow] de [Resource]<[List]<[UbicacionMaps]>> que emite la lista de ubicaciones en el mapa.
     */
    suspend fun getUbicacionesMaps(
        claveSucursal: String,
        numeroEmpleado: String,
        fecha: String
    ): Flow<Resource<List<UbicacionMaps>>>

    /**
     * Realiza la llamada a la API para obtener el histórico de cotizaciones para una fecha específica.
     *
     * @param fecha La fecha para la que se solicita el histórico de cotizaciones.
     * @return Un [Flow] de [Resource]<[List]<[HisCotizacionM]>> que emite la lista del histórico de cotizaciones.
     */
    suspend fun getHisCotizacion(
        fecha: String
    ): Flow<Resource<List<HisCotizacionM>>>

    /**
     * Realiza la llamada a la API para obtener el detalle del histórico de cotizaciones de un visitador para una fecha específica.
     *
     * @param fecha La fecha para la que se solicita el detalle del histórico de cotizaciones.
     * @param idVistador El ID del visitador.
     * @return Un [Flow] de [Resource]<[List]<[HisCotizacionM]>> que emite la lista del detalle del histórico de cotizaciones.
     */
    suspend fun getHisCtoGdetalle(
        fecha: String,
        idVistador: String
    ): Flow<Resource<List<HisCotizacionM>>>

    /**
     * Realiza la llamada a la API para obtener el histórico de cotizaciones de gerentes para una fecha específica.
     *
     * @param fecha La fecha para la que se solicita el histórico de cotizaciones de gerentes.
     * @return Un [Flow] de [Resource]<[List]<[HisCotizacionGerente]>> que emite la lista del histórico de cotizaciones de gerentes.
     */
    suspend fun getHisCotizacionGerente(
        fecha: String
    ): Flow<Resource<List<HisCotizacionGerente>>>

    /**
     * Realiza la llamada a la API para obtener el detalle de una cotización específica por su ID.
     *
     * @param idCotizacion El ID de la cotización para la que se solicita el detalle.
     * @return Un [Flow] de [Resource]<[List]<[HisCotizacionDetalle]>> que emite la lista del detalle de la cotización.
     */
    suspend fun getHisCotizacionDetalle(
        idCotizacion: String
    ): Flow<Resource<List<HisCotizacionDetalle>>>
}