package com.app.sanimex.data.repository

import android.util.Log
import com.app.sanimex.core.util.Constants
import com.app.sanimex.core.util.Resource
import com.app.sanimex.data.remote.SanimexAPI
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
import com.app.sanimex.domain.model.Ubicacion.UbicacionMaps
import com.app.sanimex.domain.model.User
import com.app.sanimex.domain.model.Visitador.VisitadorActivo
import com.app.sanimex.domain.repository.NetworkRepository
import com.app.sanimex.domain.usecase.Cliente.ClienteResponse
import kotlinx.coroutines.flow.Flow
import kotlinx.coroutines.flow.flow
import parseToErrorModel
import retrofit2.HttpException
import java.io.IOException
import javax.inject.Inject

/**
 * Implementación de la interfaz [NetworkRepository] que utiliza la API de [SanimexAPI] para realizar
 * operaciones relacionadas con la red.
 *
 * Esta clase maneja las llamadas a la API, emitiendo un [Flow] de [Resource] para cada operación,
 * indicando el estado de la petición (cargando, éxito o fallo) y los datos o el mensaje de error correspondiente.
 * También incluye manejo de excepciones específicas como [HttpException] e [IOException].
 *
 * @property api La interfaz [SanimexAPI] inyectada para acceder a los endpoints de la API.
 * @author David Duarte
 * @version 1.0
 */

class NetworkRepositoryImpl @Inject constructor(
    private val api: SanimexAPI
) : NetworkRepository {
    /**
     * Realiza la llamada a la API para el inicio de sesión.
     *
     * Emite un [Flow] de [Resource]<[LoginResponse]> que representa el estado de la petición de inicio de sesión
     * y la respuesta del servidor en caso de éxito. Maneja [HttpException] para errores HTTP,
     * [IOException] para problemas de conexión y otras [Exception] para errores inesperados.
     *
     * @param email El correo electrónico del usuario.
     * @param password La contraseña del usuario.
     * @return Un [Flow] de [Resource]<[LoginResponse]> que indica el estado y el resultado de la petición.
     */
    override suspend fun login(email: String, password: String): Flow<Resource<LoginResponse>> {
        return flow {
            emit(Resource.Loading())
            try {
                val response = api.login(email = email, password = password)
                emit(Resource.Success(data = response.toLoginResponse()))
            } catch (e: HttpException) {
                val error = e.parseToErrorModel()
                emit(Resource.Failure(message = error.message))
            } catch (e: IOException) {
                emit(Resource.Failure(message = "No se puede acceder al servidor, compruebe su conexión a Internet"))
            } catch (e: Exception) {
                emit(Resource.Failure(message = "Se ha producido un error desconocido, inténtelo de nuevo más tarde"))
            }
        }
    }
    /**
     * Realiza la llamada a la API para obtener la información de un cliente.
     *
     * Emite un [Flow] de [Resource]<[ClienteResponse]> que representa el estado de la petición
     * y la respuesta del servidor con la información del cliente. Maneja [HttpException], [IOException]
     * y otras [Exception] para el manejo de errores.
     *
     * @param claveCliente El ID del cliente.
     * @param corredor El código del corredor.
     * @return Un [Flow] de [Resource]<[ClienteResponse]> con el estado y la información del cliente.
     */
    override suspend fun cliente(claveCliente: String, corredor: String): Flow<Resource<ClienteResponse>> {
        return flow {
            emit(Resource.Loading())
            try {
                val response = api.cliente(claveCliente = claveCliente, corredor = corredor)
                emit(Resource.Success(data = response.toClienteResponse()))
            } catch (e: HttpException) {
                val error = e.parseToErrorModel()
                emit(Resource.Failure(message = error.message))
            } catch (e: IOException) {
                emit(Resource.Failure(message = "No se puede acceder al servidor, compruebe su conexión a Internet"))
            } catch (e: Exception) {
                emit(Resource.Failure(message = "Se ha producido un error desconocido, inténtelo de nuevo más tarde"))
            }
        }
    }

    /**
     * Realiza la llamada a la API para buscar productos.
     *
     * Emite un [Flow] de [Resource]<[List]<[BusquedaProducto]>> que contiene el estado de la búsqueda
     * y la lista de productos encontrados. Mapea los resultados de [SearchResponseDto] a una lista
     * de [BusquedaProducto] utilizando la función de extensión `toCommonProduct()`. También maneja las excepciones.
     *
     * @param query La cadena de búsqueda.
     * @return Un [Flow] de [Resource]<[List]<[BusquedaProducto]>> con el estado y los resultados de la búsqueda.
     */
    override suspend fun search(query: String): Flow<Resource<List<BusquedaProducto>>> {
        return flow {
            emit(Resource.Loading())
            try {
                val response = api.search(query = query)
                emit(Resource.Success(data = response.data.products.map { it.toCommonProduct() }))
            } catch (e: HttpException) {
                val error = e.parseToErrorModel()
                emit(Resource.Failure(message = error.message))
            } catch (e: IOException) {
                emit(Resource.Failure(message = "No se puede acceder al servidor, compruebe su conexión a Internet"))
            } catch (e: Exception) {
                emit(Resource.Failure(message = "Se ha producido un error desconocido, inténtelo de nuevo más tarde"))
            }
        }
    }
    /**
     * Realiza la llamada a la API para obtener la información de un solo producto utilizando su ID.
     *
     * Emite un [Flow] de [Resource]<[ProductData]> con el estado de la petición y la información del producto.
     * Utiliza la función de extensión `toProductData()` para mapear la respuesta. Maneja las excepciones comunes.
     *
     * @param id El ID del producto.
     * @param corredor El código del corredor.
     * @return Un [Flow] de [Resource]<[ProductData]> con el estado y la información del producto.
     */
    override suspend fun getSingleProduct(
        id: String,
        corredor: String
    ): Flow<Resource<ProductData>> {
        return flow {
            emit(Resource.Loading())
            try {
                val response = api.getSingleProduct(id = id, corredor = corredor)
                emit(Resource.Success(data = response.toProductData()))
            } catch (e: HttpException) {
                val error = e.parseToErrorModel()
                emit(Resource.Failure(message = error.message))
            } catch (e: IOException) {
                emit(Resource.Failure(message = "No se puede acceder al servidor, compruebe su conexión a Internet"))
            } catch (e: Exception) {
                emit(Resource.Failure(message = "Se ha producido un error desconocido, inténtelo de nuevo más tarde"))
            }
        }
    }
    /**
     * Realiza la llamada a la API para obtener la información de un solo producto para un cliente específico.
     *
     * Emite un [Flow] de [Resource]<[ClientData]> con el estado de la petición y la información del producto para el cliente.
     * Utiliza la función de extensión `toClientData()` para mapear la respuesta. Incluye parámetros adicionales
     * para la clave del cliente, tipo de entrega, tipo de pago y tipo de consulta. Maneja las excepciones.
     *
     * @param id El ID del producto.
     * @param corredor El código del corredor.
     * @param claveCliente La clave del cliente.
     * @param tipoEntrega Indica el tipo de entrega.
     * @param tipoPago Indica el tipo de pago.
     * @param TipoConsulta Indica el tipo de consulta.
     * @return Un [Flow] de [Resource]<[ClientData]> con el estado y la información del producto para el cliente.
     */
    override suspend fun getSingleClient(
        id: String,
        corredor: String,
        claveCliente: String,
        tipoEntrega: Boolean,
        tipoPago: Boolean,
        TipoConsulta: Boolean,
        idDireccion: Int
    ): Flow<Resource<ClientData>> {
        return flow {
            emit(Resource.Loading())
            try {
                val response = api.getSingleProductoClient(
                    id = id,
                    corredor = corredor,
                    ClaveCliente = claveCliente,
                    tipoEntrega = tipoEntrega,
                    tipoPago = tipoPago,
                    TipoConsulta = TipoConsulta,
                    idDireccion = idDireccion
                )
                emit(Resource.Success(data = response.toClientData()))
            } catch (e: HttpException) {
                val error = e.parseToErrorModel()
                emit(Resource.Failure(message = error.message))
            } catch (e: IOException) {
                emit(Resource.Failure(message = "No se puede acceder al servidor, compruebe su conexión a Internet"))
            } catch (e: Exception) {
                emit(Resource.Failure(message = "Se ha producido un error desconocido, inténtelo de nuevo más tarde"))
            }
        }
    }
    /**
     * Realiza la llamada a la API para agregar un producto al carrito de compras.
     *
     * Emite un [Flow] de [Resource]<[Unit]> que indica el estado de la petición. No espera un cuerpo de respuesta
     * significativo en caso de éxito. Incluye todos los parámetros necesarios para agregar un producto al carrito.
     * Maneja las excepciones comunes.
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
     * @return Un [Flow] de [Resource]<[Unit]> con el estado de la petición.
     */
    override suspend fun addToCart(
        productID: String,
        descripsion: String,
        corredor: String,
        precioFinal: Float,
        cantidad: Int,
        claveCliente: String,
        recoge: Boolean,
        contado: Boolean,
        tipoConsulta: Boolean
    ): Flow<Resource<Unit>> {
        return flow {
            emit(Resource.Loading())
            try {
                api.addProductToCart(
                    productID = productID,
                    descripsion = descripsion,
                    corredor = corredor,
                    precioFinal = precioFinal,
                    cantidad = cantidad,
                    claveCliente = claveCliente,
                    recoge = recoge,
                    contado = contado,
                    tipoConsulta = tipoConsulta
                )
                emit(Resource.Success(data = Unit))
            } catch (e: HttpException) {
                val error = e.parseToErrorModel()
                emit(Resource.Failure(message = error.message))
            } catch (e: IOException) {
                emit(Resource.Failure(message = "No se puede acceder al servidor, compruebe su conexión a Internet"))
            } catch (e: Exception) {
                emit(Resource.Failure(message = "Se ha producido un error desconocido, inténtelo de nuevo más tarde"))
            }
        }
    }
    /**
     * Realiza la llamada a la API para eliminar un producto del carrito de compras.
     *
     * Emite un [Flow] de [Resource]<[Unit]> que indica el estado de la petición. Requiere el ID del producto a eliminar.
     * Maneja las excepciones comunes.
     *
     * @param productID El ID del producto a eliminar del carrito.
     * @return Un [Flow] de [Resource]<[Unit]> con el estado de la petición.
     */
    override suspend fun removeFromCart(productID: String): Flow<Resource<Unit>> {
        return flow {
            emit(Resource.Loading())
            try {
                api.deleteFromCart(productID = productID)
                emit(Resource.Success(data = Unit))
            } catch (e: HttpException) {
                val error = e.parseToErrorModel()
                emit(Resource.Failure(message = error.message))
            } catch (e: IOException) {
                emit(Resource.Failure(message = "No se puede acceder al servidor, compruebe su conexión a Internet"))
            } catch (e: Exception) {
                emit(Resource.Failure(message = "Se ha producido un error desconocido, inténtelo de nuevo más tarde"))
            }
        }
    }
    /**
     * Realiza la llamada a la API para incrementar en uno la cantidad de un producto en el carrito.
     *
     * Emite un [Flow] de [Resource]<[Unit]> con el estado de la petición. Requiere el ID del producto y el código del corredor.
     * Maneja las excepciones comunes.
     *
     * @param productID El ID del producto a incrementar.
     * @param corredor El código del corredor.
     * @return Un [Flow] de [Resource]<[Unit]> con el estado de la petición.
     */
    override suspend fun sumaUnoFromCart(productID: String, corredor: String): Flow<Resource<Unit>> {
        return flow {
            emit(Resource.Loading())
            try {
                api.sumaUnoFromCart(productID = productID, corredor = corredor)
                emit(Resource.Success(data = Unit))
            } catch (e: HttpException) {
                val error = e.parseToErrorModel()
                emit(Resource.Failure(message = error.message))
            } catch (e: IOException) {
                emit(Resource.Failure(message = "No se puede acceder al servidor, compruebe su conexión a Internet"))
            } catch (e: Exception) {
                emit(Resource.Failure(message = "Se ha producido un error desconocido, inténtelo de nuevo más tarde"))
            }
        }
    }

    override suspend fun sumaDiezFromCart(productID: String, corredor: String): Flow<Resource<Unit>> {
        return flow {
            emit(Resource.Loading())
            try {
                api.sumaDiezFromCart(productID = productID, corredor = corredor)
                emit(Resource.Success(data = Unit))
            } catch (e: HttpException) {
                val error = e.parseToErrorModel()
                emit(Resource.Failure(message = error.message))
            } catch (e: IOException) {
                emit(Resource.Failure(message = "No se puede acceder al servidor, compruebe su conexión a Internet"))
            } catch (e: Exception) {
                emit(Resource.Failure(message = "Se ha producido un error desconocido, inténtelo de nuevo más tarde"))
            }
        }
    }
    override suspend fun removeUnoFromCart(productID: String, corredor: String): Flow<Resource<Unit>> {
        return flow {
            emit(Resource.Loading())
            try {
                api.removeUnoFromCart(productID = productID, corredor = corredor)
                emit(Resource.Success(data = Unit))
            } catch (e: HttpException) {
                val error = e.parseToErrorModel()
                emit(Resource.Failure(message = error.message))
            } catch (e: IOException) {
                emit(Resource.Failure(message = "No se puede acceder al servidor, compruebe su conexión a Internet"))
            } catch (e: Exception) {
                emit(Resource.Failure(message = "Se ha producido un error desconocido, inténtelo de nuevo más tarde"))
            }
        }
    }
    override suspend fun removeDiezFromCart(productID: String, corredor: String): Flow<Resource<Unit>> {
        return flow {
            emit(Resource.Loading())
            try {
                api.removeDiezFromCart(productID = productID, corredor = corredor)
                emit(Resource.Success(data = Unit))
            } catch (e: HttpException) {
                val error = e.parseToErrorModel()
                emit(Resource.Failure(message = error.message))
            } catch (e: IOException) {
                emit(Resource.Failure(message = "No se puede acceder al servidor, compruebe su conexión a Internet"))
            } catch (e: Exception) {
                emit(Resource.Failure(message = "Se ha producido un error desconocido, inténtelo de nuevo más tarde"))
            }
        }
    }

    override suspend fun saveCart(): Flow<Resource<Unit>> {
        return flow {
            emit(Resource.Loading())
            try {
                api.saveCart()
                emit(Resource.Success(data = Unit))
            } catch (e: HttpException) {
                val error = e.parseToErrorModel()
                emit(Resource.Failure(message = error.message))
            } catch (e: IOException) {
                emit(Resource.Failure(message = "No se puede acceder al servidor, compruebe su conexión a Internet"))
            } catch (e: Exception) {
                emit(Resource.Failure(message = "Se ha producido un error desconocido, inténtelo de nuevo más tarde"))
            }
        }
    }


    override suspend fun getMyCart(): Flow<Resource<List<CarritoFinal>>> {
        return flow {
            emit(Resource.Loading())
            try {
                val response = api.getMyCart()
                emit(Resource.Success(data = response.data.cart.items.map { it.toCartItem() }))
            } catch (e: HttpException) {
                val error = e.parseToErrorModel()
                emit(Resource.Failure(message = error.message))
            } catch (e: IOException) {
                emit(Resource.Failure(message = "No se puede acceder al servidor, compruebe su conexión a Internet"))
            } catch (e: Exception) {
                emit(Resource.Failure(message = "Se ha producido un error desconocido, inténtelo de nuevo más tarde"))
            }
        }
    }
    override suspend fun addAddress(
        direccion: String,
        latitu: Double,
        longitu: Double,
        claveSucursal: String,
        tipoIngreso: Boolean,
    ): Flow<Resource<DireccionResponse>> {
        return flow {
            emit(Resource.Loading())
            try {
                val response =  api.addAddress(
                    direccion = direccion,
                    latitu = latitu,
                    longitu = longitu,
                    claveSucursal = claveSucursal,
                    tipoIngreso = tipoIngreso,
                )
                emit(Resource.Success(data = response.toDireccionResponse()))
            } catch (e: HttpException) {
                val error = e.parseToErrorModel()
                emit(Resource.Failure(message = error.message))
            } catch (e: IOException) {
                emit(Resource.Failure(message = "No se puede acceder al servidor, compruebe su conexión a Internet"))
            } catch (e: Exception) {
                emit(Resource.Failure(message = "Se ha producido un error desconocido, inténtelo de nuevo más tarde"))
            }
        }
    }
    override suspend fun getUserSucursal(fecha: String): Flow<Resource<List<MapsUserSucursal>>> =
        flow {
            try {
                emit(Resource.Loading())
                val response = api.getUserSucursal(fecha)
                emit(Resource.Success(data = response.address!!.map { it.toMapsSucursal() }))

            } catch (e: HttpException) {
                Log.e("SucursalRepo", "HTTP Exception: ${e.message}", e)
                val error = e.parseToErrorModel()
                emit(Resource.Failure(message = error.message))
            } catch (e: IOException) {
                Log.e("SucursalRepo", "IO Exception: ${e.message}", e)
                emit(Resource.Failure(message = "No se puede acceder al servidor, compruebe su conexión a Internet"))
            } catch (e: Exception) {
                Log.e("SucursalRepo", "Unknown Exception: ${e.message}", e)
                emit(Resource.Failure(message = "Error: ${e.message}"))
            }
        }

    override suspend fun getWishlist(): Flow<Resource<List<Sucursal>>> {
        return flow {
            emit(Resource.Loading())
            try {
                val response = api.getWishlist()
                emit(Resource.Success(data = response.data.wishlists.map { it.toCommonProduct() }))
            } catch (e: HttpException) {
                val error = e.parseToErrorModel()
                emit(Resource.Failure(message = error.message))
            } catch (e: IOException) {
                emit(Resource.Failure(message = "No se puede acceder al servidor, compruebe su conexión a Internet"))
            } catch (e: Exception) {
                emit(Resource.Failure(message = "Se ha producido un error desconocido, inténtelo de nuevo más tarde"))
            }
        }
    }

    override suspend fun getMyInfo(): Flow<Resource<User>> {
        return flow {
            emit(Resource.Loading())
            try {
                val response = api.getMyInfo()
                emit(Resource.Success(data = response.toUser()))
            } catch (e: HttpException) {
                val error = e.parseToErrorModel()
                emit(Resource.Failure(message = error.message))
            } catch (e: IOException) {
                emit(Resource.Failure(message = "No se puede acceder al servidor, compruebe su conexión a Internet"))
            } catch (e: Exception) {
                emit(Resource.Failure(message = "Se ha producido un error desconocido, inténtelo de nuevo más tarde"))
            }
        }
    }

    // suscursales por id Usuario

    override suspend fun getMySucursal(): Flow<Resource<List<Sucursal>>> {
        return flow {
            emit(Resource.Loading())
            try {
                val response = api.getMySucursal()
                emit(Resource.Success(data = response.data.sucursal.map { it.toSucursal() }))
            } catch (e: HttpException) {
                val error = e.parseToErrorModel()
                emit(Resource.Failure(message = error.message))
            } catch (e: IOException) {
                emit(Resource.Failure(message = "Can't reach server, check your internet connection"))
            } catch (e: Exception) {
                emit(Resource.Failure(message = "Unknown error happened, try again later"))
            }
        }
    }

    override suspend fun getOfferProducts(): Flow<Resource<List<HistoricoVtaMayItem>>> {
        return flow {
            emit(Resource.Loading())
            try {
                val response = api.getOfferProducts(ClienteSap = Constants.cliente)
                emit(Resource.Success(data = response.data.product.map { it.toCommonProduct() }))
            } catch (e: HttpException) {
                val error = e.parseToErrorModel()
                emit(Resource.Failure(message = error.message))
            } catch (e: IOException) {
                emit(Resource.Failure(message = "No se puede acceder al servidor, compruebe su conexión a Internet"))
            } catch (e: Exception) {
                emit(Resource.Failure(message = "Se ha producido un error desconocido, inténtelo de nuevo más tarde"))
            }
        }
    }

    override suspend fun getVisitadoresActivos(
        claveSucursal: String,
        fecha: String
    ): Flow<Resource<List<VisitadorActivo>>> = flow {
        try {
            emit(Resource.Loading())
            val response = api.getVisitadoresActivos(claveSucursal, fecha)
            emit(Resource.Success(data = response.visitadores.map { it.toVisitadorActivo() }))
        } catch (e: HttpException) {
            val error = e.parseToErrorModel()
            emit(Resource.Failure(message = error.message))
        } catch (e: IOException) {
            emit(Resource.Failure(message = "No se puede acceder al servidor, compruebe su conexión a Internet"))
        } catch (e: Exception) {
            emit(Resource.Failure(message = "Error: ${e.message}"))
        }
    }

    override suspend fun getUbicacionesMaps(
        claveSucursal: String,
        numeroEmpleado: String,
        fecha: String
    ): Flow<Resource<List<UbicacionMaps>>> = flow {
        try {
            emit(Resource.Loading())
            val response = api.getUbicacionesMaps(claveSucursal, numeroEmpleado, fecha)
            emit(Resource.Success(data = response.ubicaciones.map { it.toUbicacionMaps() }))
        } catch (e: HttpException) {
            val error = e.parseToErrorModel()
            emit(Resource.Failure(message = error.message))
        } catch (e: IOException) {
            emit(Resource.Failure(message = "No se puede acceder al servidor, compruebe su conexión a Internet"))
        } catch (e: Exception) {
            emit(Resource.Failure(message = "Error: ${e.message}"))
        }
    }
    // suscursales por id Usuario

    override suspend fun getHisCotizacion(
        fecha: String
    ): Flow<Resource<List<HisCotizacionM>>> {
        return flow {
            emit(Resource.Loading())
            try {
                val response = api.getHisCotizacion(fecha)
                emit(Resource.Success(data = response.historicoVtaMay.map { it.toHistoricoVtaMayItem() }))
            } catch (e: HttpException) {
                val error = e.parseToErrorModel()
                emit(Resource.Failure(message = error.message))
            } catch (e: IOException) {
                emit(Resource.Failure(message = "No se puede acceder al servidor, compruebe su conexión a Internet"))
            } catch (e: Exception) {
                emit(Resource.Failure(message = "Se ha producido un error desconocido, inténtelo de nuevo más tarde"))
            }
        }
    }
    override suspend fun getHisCtoGdetalle(
        fecha: String,
        idVistador: String
    ): Flow<Resource<List<HisCotizacionM>>> {
        return flow {
            emit(Resource.Loading())
            try {
                val response = api.getHisCtoGdetalle(fecha, idVistador)
                emit(Resource.Success(data = response.historicoVtaMay.map { it.toHistoricoVtaMayItem() }))
            } catch (e: HttpException) {
                val error = e.parseToErrorModel()
                emit(Resource.Failure(message = error.message))
            } catch (e: IOException) {
                emit(Resource.Failure(message = "No se puede acceder al servidor, compruebe su conexión a Internet"))
            } catch (e: Exception) {
                emit(Resource.Failure(message = "Se ha producido un error desconocido, inténtelo de nuevo más tarde"))
            }
        }
    }

    override suspend fun getHisCotizacionGerente(
        fecha: String
    ): Flow<Resource<List<HisCotizacionGerente>>> {
        return flow {
            emit(Resource.Loading())
            try {
                val response = api.getHisCotizacionGerente(fecha)
                emit(Resource.Success(data = response.historicoVtaMay.map { it.toHistoricoVtaGerenteItem() }))
            } catch (e: HttpException) {
                val error = e.parseToErrorModel()
                emit(Resource.Failure(message = error.message))
            } catch (e: IOException) {
                emit(Resource.Failure(message = "No se puede acceder al servidor, compruebe su conexión a Internet"))
            } catch (e: Exception) {
                emit(Resource.Failure(message = "Se ha producido un error desconocido, inténtelo de nuevo más tarde"))
            }
        }
    }

    override suspend fun getHisCotizacionDetalle(
        idCotizacion: String
    ): Flow<Resource<List<HisCotizacionDetalle>>> {
        return flow {
            emit(Resource.Loading())
            try {
                val response = api.getHisCotizacionDetalle(idCotizacion)
                emit(Resource.Success(data = response.cotizacionDetalle.map { it.toCotizacionDetalleItem() }))
            } catch (e: HttpException) {
                val error = e.parseToErrorModel()
                emit(Resource.Failure(message = error.message))
            } catch (e: IOException) {
                emit(Resource.Failure(message = "No se puede acceder al servidor, compruebe su conexión a Internet"))
            } catch (e: Exception) {
                emit(Resource.Failure(message = "Se ha producido un error desconocido, inténtelo de nuevo más tarde"))
            }
        }
    }

}