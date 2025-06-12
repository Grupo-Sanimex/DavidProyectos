package com.app.sanimex.presentation

import android.os.Bundle
import android.view.WindowManager
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.core.splashscreen.SplashScreen.Companion.installSplashScreen
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.lifecycleScope
import androidx.navigation.compose.rememberNavController
import com.app.sanimex.core.navigation.AppNavigation
import com.app.sanimex.core.navigation.AppScreens
import com.app.sanimex.core.util.Constants.isLoggedIn
import com.app.sanimex.presentation.ui.theme.SanimexTheme
import dagger.hilt.android.AndroidEntryPoint
import kotlinx.coroutines.delay
import kotlinx.coroutines.launch

/**
 * Actividad principal de la aplicación, punto de entrada para la interfaz de usuario.
 *
 * Anotada con [AndroidEntryPoint] para habilitar la inyección de dependencias con Hilt.
 * Configura la pantalla de presentación (SplashScreen), la navegación principal y, opcionalmente,
 * desactiva la captura de pantalla.
 *
 * @property isKeepSplashScreen [MutableLiveData] que controla si la pantalla de presentación debe seguir mostrándose.
 * @author David Duarte
 * @version 1.0
 */
@AndroidEntryPoint
class MainActivity : ComponentActivity() {
    private val isKeepSplashScreen = MutableLiveData(true)
    /**
     * Se llama cuando se crea la actividad por primera vez.
     *
     * Configura la pantalla de presentación, la navegación principal de la aplicación
     * y, opcionalmente, desactiva la captura de pantalla.
     *
     * @param savedInstanceState Si la actividad se vuelve a inicializar después de un cierre previo,
     * este Bundle contiene los datos que guardó más recientemente en [onSaveInstanceState].
     * Nota: este valor puede ser nulo.
     */
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        delaySplashScreen() // Inicia la corrutina para ocultar la pantalla de presentación después de un retraso.
        installSplashScreen().setKeepOnScreenCondition {
            isKeepSplashScreen.value ?: true // Mantiene la pantalla de presentación visible mientras isKeepSplashScreen sea true.
        }
        setContent {
            val navController = rememberNavController() // Crea y recuerda un NavController para gestionar la navegación.
            val startDestination = if (isLoggedIn) AppScreens.Explore.route else AppScreens.Login.route // Determina la pantalla de inicio según el estado de inicio de sesión.
            SanimexTheme {
                AppNavigation( // Composable que contiene la configuración de la navegación de la aplicación.
                    navController = navController,
                    startDestination = startDestination
                )
            }
        }
        // Código comentado para evitar capturas de pantalla o grabación.
        // Se puede descomentar si se requiere esta funcionalidad.
       window.setFlags(
            WindowManager.LayoutParams.FLAG_SECURE,
            WindowManager.LayoutParams.FLAG_SECURE
        )
    }
    /**
     * Función privada para retrasar la ocultación de la pantalla de presentación.
     *
     * Inicia una corrutina en [lifecycleScope] que espera 2 segundos y luego establece
     * el valor de [isKeepSplashScreen] en false, lo que permite que la pantalla de presentación se oculte.
     */
    private fun delaySplashScreen() {
        lifecycleScope.launch {
            delay(2000)
            isKeepSplashScreen.value = false
        }
    }

}


