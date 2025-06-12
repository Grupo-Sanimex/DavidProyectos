package com.app.sanimex.core.util

import androidx.datastore.preferences.core.booleanPreferencesKey
import androidx.datastore.preferences.core.stringPreferencesKey

/**
 * Objeto que contiene las constantes utilizadas en toda la aplicación.
 *
 * Este objeto centraliza la definición de valores constantes, claves para el DataStore
 * y variables de tiempo de ejecución que son de uso general en la aplicación.
 *
 * @author David Duarte
 * @version 1.0
 */
object Constants {

    // App Constants

    /**
     * Etiqueta de registro (Tag) utilizada para identificar los logs de la aplicación.
     */
    const val TAG = "Sanimex"
    /**
     * Nombre utilizado para el DataStore de la aplicación.
     */
    const val APP_DATA_STORE_NAME = "Sanimex"
    /**
     * URL base para acceder a las imágenes de los productos (actualmente en entorno de desarrollo).
     *
     * Comentada está la URL para el entorno productivo.
     */
     const val IMAGE_URL = "http://200.57.183.111:60802/" // productivo
    //const val IMAGE_URL = "http://192.168.5.1:8081/"

    // Data Store Keys

    /**
     * Clave para almacenar y recuperar el estado de inicio de sesión del usuario en el DataStore.
     */
    val IS_LOGGED_IN_KEY = booleanPreferencesKey("IS_LOGGED_IN_KEY")
    /**
     * Clave para almacenar y recuperar el token de autenticación del usuario en el DataStore.
     */
    val USER_TOKEN_KEY = stringPreferencesKey("USER_TOKEN_KEY")

    // Runtime Util Constants

    /**
     * Variable de tiempo de ejecución que indica si el usuario está actualmente logueado.
     */
    var isLoggedIn = false
    /**
     * Variable de tiempo de ejecución que almacena el token de autenticación del usuario actual.
     */
    var userToken = ""
    /**
     * Variable de tiempo de ejecución para almacenar el corredor asociado a la operación actual.
     */
    var corredor: String = ""
    /**
     * Variable de tiempo de ejecución para almacenar el ID del producto actual.
     */
    var productID: String = ""
    /**
     * Variable de tiempo de ejecución para almacenar la clave del cliente actual.
     */
    var ClaveCliente: String = ""
    /**
     * Variable de tiempo de ejecución para almacenar el nombre del cliente actual.
     */
    var cliente = ""
    /**
     * Variable de tiempo de ejecución para indicar el tipo de entrega (true/false).
     */
    var tipoEntrega = true
    /**
     * Variable de tiempo de ejecución para indicar el tipo de pago (true/false).
     */
    var tipoPago = true
    /**
     * Variable de tiempo de ejecución para almacenar la descripción de algún elemento.
     */
    var descripsion : String = ""
    /**
     * Variable de tiempo de ejecución para almacenar el precio final de algún elemento.
     */
    var precioFinal : Float = 0.0f
    /**
     * Variable de tiempo de ejecución para almacenar la cantidad de algún elemento.
     */
    var cantidad : Int = 0
    // Rol de usuario
    var idRol : Int = 0

    // id usuario
    var idUsuario : String = ""

    // guardar direccion del maps
    var direccion :String = ""
    var latitud : Double = 0.0
    var longitu : Double = 0.0

    // variable para maps
    var fechaConsulta :String = ""

    var fechaSeleccionada :String = ""
    var fechaGDetalle :String = ""
    var idVisitadorGD :String = ""
    var fechaUbicaion :String = ""
    var sucursalUbicacion :String = ""

    // para validar el tipo de consulta del visitador
    var tipoConsulta :Boolean = false

    // creamos el id de la direccion
    var idDireccion : Int = 0
}