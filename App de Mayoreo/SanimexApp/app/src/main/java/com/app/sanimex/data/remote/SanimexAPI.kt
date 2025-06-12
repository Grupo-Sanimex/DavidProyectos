package com.app.sanimex.data.remote

import com.app.sanimex.data.remote.dto.CartResponseDto
import com.app.sanimex.data.remote.dto.Cotizaciones.HisCotizacionGResponseDto
import com.app.sanimex.data.remote.dto.Cotizaciones.HisCotizacionResponseDto
import com.app.sanimex.data.remote.dto.Cotizaciones.HisCtoDetalleResponseDto
import com.app.sanimex.data.remote.dto.DireccionResponseDto
import com.app.sanimex.data.remote.dto.LoginResponseDto
import com.app.sanimex.data.remote.dto.MapsSucursal.MapsSucursalResponseDto
import com.app.sanimex.data.remote.dto.MeResponseDto
import com.app.sanimex.data.remote.dto.ProductInfoClientDto
import com.app.sanimex.data.remote.dto.ProductInfoDto
import com.app.sanimex.data.remote.dto.ReporteResponseDto
import com.app.sanimex.data.remote.dto.SearchResponseDto
import com.app.sanimex.data.remote.dto.SucursalResponseDto
import com.app.sanimex.data.remote.dto.WishlistResponseDto
import com.app.sanimex.data.remote.dto.Visitador.VisitadorActivoResponseDto
import com.app.sanimex.data.remote.dto.Ubicacion.UbicacionMapsResponseDto
import com.app.sanimex.domain.model.Cliente.ClienteResponseDto
import retrofit2.http.DELETE
import retrofit2.http.GET
import retrofit2.http.POST
import retrofit2.http.Query

/**
 * Interfaz que define los endpoints de la API de Sanimex utilizando Retrofit.
 *
 * Cada función suspendida en esta interfaz representa una llamada a un endpoint específico de la API,
 * indicando el método HTTP utilizado ([POST], [GET], [DELETE]) y la ruta relativa.
 * Los parámetros de la función se corresponden con los parámetros de la consulta o el cuerpo de la petición.
 *
 * @author David Duarte
 * @version 1.0
 */
interface SanimexAPI {
    /**
     * Endpoint para la autenticación de usuarios (inicio de sesión).
     *
     * Realiza una petición [POST] al endpoint "Autenticacion/Acceso", enviando el email y la contraseña
     * como parámetros de consulta.
     *
     * @param email El email del usuario.
     * @param password La contraseña del usuario.
     * @return Un objeto [LoginResponseDto] que contiene la respuesta del servidor tras el intento de inicio de sesión.
     */
    @POST("Autenticacion/Acceso")
    suspend fun login(
        @Query("email") email: String,
        @Query("password") password: String
    ): LoginResponseDto
    /**
     * Endpoint para obtener la información de un cliente mayorista.
     *
     * Realiza una petición [GET] al endpoint "Cliente/ClientesMayoreo", requiriendo el ID del cliente
     * y el código del corredor como parámetros de consulta.
     *
     * @param claveCliente El ID del cliente mayorista.
     * @param corredor El código del corredor asociado.
     * @return Un objeto [ClienteResponseDto] que contiene la información del cliente.
     */
    @GET("Cliente/ClientesMayoreo")
    suspend fun cliente(
        @Query("idCliente") claveCliente: String,
        @Query("empresa") corredor: String
    ): ClienteResponseDto

    /**
     * Endpoint para realizar búsquedas de productos.
     *
     * Realiza una petición [GET] al endpoint "Producto/Buscar", enviando la consulta de búsqueda
     * como un parámetro de consulta.
     *
     * @param query La cadena de búsqueda.
     * @return Un objeto [SearchResponseDto] que contiene los resultados de la búsqueda.
     */
    @GET("Producto/Buscar")
    suspend fun search(
        @Query("busqueda") query: String
    ): SearchResponseDto

    /**
     * Endpoint para obtener la información de un producto específico para un cliente, utilizando su código de barras.
     *
     * Realiza una petición [GET] al endpoint "Producto/CBarraCliente", requiriendo el ID del producto,
     * el código del corredor, la clave del cliente, el tipo de entrega, el tipo de pago y el tipo de consulta
     * como parámetros de consulta.
     *
     * @param id El ID del producto (código de barras).
     * @param corredor El código del corredor asociado.
     * @param ClaveCliente La clave del cliente.
     * @param tipoEntrega Indica el tipo de entrega.
     * @param tipoPago Indica el tipo de pago.
     * @param TipoConsulta Indica el tipo de consulta.
     * @return Un objeto [ProductInfoClientDto] que contiene la información del producto para el cliente.
     */
    @GET("Producto/CBarraCliente")
    suspend fun getSingleProductoClient(
        @Query("product_id") id: String,
        @Query("centrosCorredor") corredor: String,
        @Query("ClaveCliente") ClaveCliente: String,
        @Query("tipoEntrega") tipoEntrega: Boolean,
        @Query("tipoPago") tipoPago: Boolean,
        @Query("TipoConsulta") TipoConsulta: Boolean,
        @Query("idUbicacion") idDireccion: Int
    ): ProductInfoClientDto

    /**
     * Endpoint para obtener la información de un producto específico utilizando su código de barras (sin información específica del cliente).
     *
     * Realiza una petición [GET] al endpoint "Producto/CBarraUnitario", requiriendo el ID del producto
     * y el código del corredor como parámetros de consulta.
     *
     * @param id El ID del producto (código de barras).
     * @param corredor El código del corredor asociado.
     * @return Un objeto [ProductInfoDto] que contiene la información del producto.
     */
    @GET("Producto/CBarraUnitario")
    suspend fun getSingleProduct(
        @Query("product_id") id: String,
        @Query("centrosCorredor") corredor: String
    ): ProductInfoDto

    /**
     * Endpoint para agregar un producto al carrito de compras.
     *
     * Realiza una petición [POST] al endpoint "Carrito/agregar/", enviando la información del producto
     * como parámetros de consulta.
     *
     * @param productID El ID del producto a agregar.
     * @param descripsion La descripción del producto.
     * @param corredor El código del corredor asociado.
     * @param precioFinal El precio final del producto.
     * @param cantidad La cantidad del producto a agregar.
     * @param claveCliente La clave del cliente.
     * @param recoge Indica si el cliente recogerá el producto.
     * @param contado Indica si el pago es al contado.
     * @param tipoConsulta El tipo de consulta.
     */
    @POST("Carrito/agregar/")
    suspend fun addProductToCart(
        @Query("codigo") productID: String,
        @Query("descripsion") descripsion: String,
        @Query("sucursal") corredor: String,
        @Query("precioFinal") precioFinal: Float,
        @Query("cantidad") cantidad: Int,
        @Query("claveCliente") claveCliente: String,
        @Query("Recoge") recoge: Boolean,
        @Query("Contado") contado: Boolean,
        @Query("tipo_consulta") tipoConsulta: Boolean
    )

    /**
     * Endpoint para eliminar un producto del carrito de compras.
     *
     * Realiza una petición [DELETE] al endpoint "Carrito/eliminar", requiriendo el ID del producto
     * como parámetro de consulta.
     *
     * @param productID El ID del producto a eliminar.
     */
    @DELETE("Carrito/eliminar")
    suspend fun deleteFromCart(
        @Query("codigo") productID: String
    )

    /**
     * Endpoint para incrementar en uno la cantidad de un producto en el carrito de compras.
     *
     * Realiza una petición [DELETE] al endpoint "Carrito/incrementar", requiriendo el ID del producto
     * y el código del corredor como parámetros de consulta. (Nota: Aunque la operación sea de incremento,
     * la API utiliza el método DELETE).
     *
     * @param productID El ID del producto a incrementar.
     * @param corredor El código del corredor asociado.
     */
    @DELETE("Carrito/incrementar")
    suspend fun sumaUnoFromCart(
        @Query("codigo") productID: String,
        @Query("sucursal") corredor: String
    )

    /**
     * Endpoint para incrementar en diez la cantidad de un producto en el carrito de compras.
     *
     * Realiza una petición [DELETE] al endpoint "Carrito/incrementarDiez", requiriendo el ID del producto
     * y el código del corredor como parámetros de consulta. (Nota: Aunque la operación sea de incremento,
     * la API utiliza el método DELETE).
     *
     * @param productID El ID del producto a incrementar.
     * @param corredor El código del corredor asociado.
     */
    @DELETE("Carrito/incrementarDiez")
    suspend fun sumaDiezFromCart(
        @Query("codigo") productID: String,
        @Query("sucursal") corredor: String
    )

    /**
     * Endpoint para decrementar en uno la cantidad de un producto en el carrito de compras.
     *
     * Realiza una petición [DELETE] al endpoint "Carrito/decrementar", requiriendo el ID del producto
     * y el código del corredor como parámetros de consulta. (Nota: Aunque la operación sea de decremento,
     * la API utiliza el método DELETE).
     *
     * @param productID El ID del producto a decrementar.
     * @param corredor El código del corredor asociado.
     */
    @DELETE("Carrito/decrementar")
    suspend fun removeUnoFromCart(
        @Query("codigo") productID: String,
        @Query("sucursal") corredor: String
    )

    /**
     * Endpoint para decrementar en diez la cantidad de un producto en el carrito de compras.
     *
     * Realiza una petición [DELETE] al endpoint "Carrito/decrementarDiez", requiriendo el ID del producto
     * y el código del corredor como parámetros de consulta. (Nota: Aunque la operación sea de decremento,
     * la API utiliza el método DELETE).
     *
     * @param productID El ID del producto a decrementar.
     * @param corredor El código del corredor asociado.
     */
    @DELETE("Carrito/decrementarDiez")
    suspend fun removeDiezFromCart(
        @Query("codigo") productID: String,
        @Query("sucursal") corredor: String
    )

    /**
     * Endpoint para guardar el estado actual del carrito de compras.
     *
     * Realiza una petición [POST] al endpoint "Carrito/InsertCarrito". No requiere parámetros de consulta explícitos.
     */
    @POST("Carrito/InsertCarrito")
    suspend fun saveCart()

    /**
     * Endpoint para obtener la lista de productos en el carrito de compras del usuario.
     *
     * Realiza una petición [GET] al endpoint "Carrito/listar".
     *
     * @return Un objeto [CartResponseDto] que contiene la información de los productos en el carrito.
     */
    @GET("Carrito/listar")
    suspend fun getMyCart(): CartResponseDto

    /**
     * Endpoint para agregar una nueva dirección de ubicación.
     *
     * Realiza una petición [POST] al endpoint "Ubicacion/InsertarUbicacion", enviando la información de la dirección
     * como parámetros de consulta.
     *
     * @param direccion La dirección a guardar.
     * @param latitu La latitud de la ubicación.
     * @param longitu La longitud de la ubicación.
     * @param claveSucursal La clave de la sucursal asociada.
     * @param tipoIngreso Indica el tipo de ingreso de la ubicación.
     */
    @POST("Ubicacion/InsertarUbicacion")
    suspend fun addAddress(
        @Query("direccion") direccion: String,
        @Query("latitud") latitu: Double,
        @Query("longitud") longitu: Double,
        @Query("claveSucursal") claveSucursal: String,
        @Query("tipoIngreso") tipoIngreso: Boolean,
    ):DireccionResponseDto

    @GET("Ubicacion/UbicacionSucursal")
    suspend fun getUserSucursal(
        @Query("fechaConsulta") fecha: String
    ): MapsSucursalResponseDto

    @GET("Sucursal/SupervisorSucursal")
    suspend fun getWishlist(): WishlistResponseDto

    @GET("Perfil/perfil")
    suspend fun getMyInfo(): MeResponseDto
    // obtener las sucursales por id de perfil

    @GET("Sucursal/SupervisorSucursal")
    suspend fun getMySucursal(): SucursalResponseDto

    @GET("Cliente/HistoricoVtaMay/")
    suspend fun getOfferProducts(
        @Query("ClienteSap") ClienteSap: String
    ): ReporteResponseDto


    @GET("Ubicacion/VisitadorActivos")
    suspend fun getVisitadoresActivos(
        @Query("claveSucursal") claveSucursal: String,
        @Query("fechaUnitaria") fechaUnitaria: String
    ): VisitadorActivoResponseDto

    @GET("Ubicacion/UbicacionesMaps")
    suspend fun getUbicacionesMaps(
        @Query("claveSucursal") claveSucursal: String,
        @Query("numeroEmpleado") numeroEmpleado: String,
        @Query("fechaUnitaria") fechaUnitaria: String
    ): UbicacionMapsResponseDto

    @GET("Carrito/historialVisitador")
    suspend fun getHisCotizacion(
        @Query("fechaConsulta") fecha: String
    ): HisCotizacionResponseDto
    @GET("Carrito/hisVisitadorGerente")
    suspend fun getHisCtoGdetalle(
        @Query("fechaConsulta") fecha: String,
        @Query("idvistador") idVistador: String
    ): HisCotizacionResponseDto

    @GET("Carrito/historialGerente")
    suspend fun getHisCotizacionGerente(
        @Query("fechaConsulta") fecha: String
    ): HisCotizacionGResponseDto

    @GET("Carrito/CotizacionDetalle")
    suspend fun getHisCotizacionDetalle(
        @Query("idCotizacion") idCotizacion: String
    ): HisCtoDetalleResponseDto

}