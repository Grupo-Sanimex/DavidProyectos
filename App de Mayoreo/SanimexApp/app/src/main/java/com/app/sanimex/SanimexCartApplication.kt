package com.app.sanimex

import android.app.Application
import com.app.sanimex.core.util.Constants
import com.app.sanimex.core.util.Constants.IS_LOGGED_IN_KEY
import com.app.sanimex.core.util.Constants.USER_TOKEN_KEY
import com.app.sanimex.domain.usecase.GetFromDataStoreUseCase
import dagger.hilt.android.HiltAndroidApp
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.Job
import kotlinx.coroutines.launch
import javax.inject.Inject

/**
 * Clase de aplicación principal para la aplicación Sanimex Cart, anotada con [HiltAndroidApp]
 * para habilitar la inyección de dependencias con Hilt.
 *
 * Esta clase se inicializa al arrancar la aplicación y se utiliza para configurar componentes
 * globales, como inicializar el estado de inicio de sesión y el token del usuario desde el DataStore.
 *
 * @property getFromDataStoreUseCase Caso de uso inyectado para obtener datos del DataStore.
 * @property job Coroutine Job utilizado para realizar operaciones asíncronas en [Dispatchers.IO].
 * @author David Duarte
 * @version 1.0
 */
@HiltAndroidApp
class SanimexCartApplication : Application() {

    @Inject
    lateinit var getFromDataStoreUseCase: GetFromDataStoreUseCase

    private var job: Job? = null
    /**
     * Se llama cuando se crea la aplicación por primera vez.
     *
     * Inicia una corrutina en [Dispatchers.IO] para leer el estado de inicio de sesión y el token del usuario
     * desde el DataStore utilizando el [getFromDataStoreUseCase]. Los valores recuperados se asignan
     * a las variables estáticas correspondientes en la clase [Constants].
     */

    override fun onCreate() {
        super.onCreate()
        job = CoroutineScope(Dispatchers.IO).launch {
            val isLoggedIn = getFromDataStoreUseCase(key = IS_LOGGED_IN_KEY) ?: false
            val userToken = getFromDataStoreUseCase(key = USER_TOKEN_KEY) ?: ""
            Constants.isLoggedIn = isLoggedIn
            Constants.userToken = userToken
        }
    }

    /**
     * Se llama por el sistema cuando el dispositivo tiene poca memoria.
     *
     * Cancela cualquier corrutina en ejecución para liberar recursos.
     */
    override fun onLowMemory() {
        super.onLowMemory()
        job?.cancel()
    }
    /**
     * Se llama cuando el proceso de la aplicación está a punto de terminar.
     *
     * Cancela cualquier corrutina en ejecución para asegurar una limpieza adecuada.
     */
    override fun onTerminate() {
        super.onTerminate()
        job?.cancel()
    }
}