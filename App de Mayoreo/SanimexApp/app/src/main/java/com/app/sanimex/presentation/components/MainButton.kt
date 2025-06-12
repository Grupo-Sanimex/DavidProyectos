package com.app.sanimex.presentation.components

import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material3.Button
import androidx.compose.material3.ButtonDefaults
import androidx.compose.material3.Icon
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.Dp
import androidx.compose.ui.unit.dp
import com.airbnb.lottie.compose.LottieAnimation
import com.airbnb.lottie.compose.LottieCompositionSpec
import com.airbnb.lottie.compose.LottieConstants
import com.airbnb.lottie.compose.rememberLottieComposition
import com.app.sanimex.R

/**
 * Composable que representa un botón principal personalizado con soporte para texto, ícono y estado de carga.
 *
 * Este botón utiliza los estilos del tema de Material Design y permite configurar su apariencia
 * (color, forma, habilitado/deshabilitado) y comportamiento (acción al hacer clic, indicador de carga).
 *
 * @param modifier Modificador para personalizar el layout del botón (por defecto, establece una altura de 58 dp).
 * @param text Texto a mostrar en el botón (opcional).
 * @param icon Recurso de ícono (Int) a mostrar en el botón (opcional).
 * @param cornerRadius Radio de las esquinas redondeadas del botón (por defecto, 5 dp).
 * @param padding Espacio interno alrededor del contenido del botón (por defecto, 16 dp).
 * @param onClick Función lambda que se ejecuta al hacer clic en el botón (por defecto, una función vacía).
 * @param isLoading Booleano que indica si el botón se encuentra en estado de carga (por defecto, false). Si es true, se muestra una animación de carga de Lottie.
 * @param isEnabled Booleano que indica si el botón está habilitado para recibir clics (por defecto, true).
 * @param activeColor Color de fondo del botón cuando está habilitado (por defecto, el color primario del tema).
 * @param inactiveColor Color de fondo del botón cuando está deshabilitado (por defecto, el color terciario del tema).
 * @author David Duarte
 * @version 1.0
 */
@Preview
@Composable
fun MainButton(
    modifier: Modifier = Modifier.height(58.dp),
    text: String? = null,
    icon: Int? = null,
    cornerRadius: Dp = 5.dp,
    padding: Dp = 16.dp,
    onClick: () -> Unit = {},
    isLoading: Boolean = false,
    isEnabled: Boolean = true,
    activeColor: Color = MaterialTheme.colorScheme.primary,
    inactiveColor: Color = MaterialTheme.colorScheme.tertiary
) {
    // Composable Button de Material Design.
    Button(
        modifier = modifier, // Aplica el modificador recibido.
        onClick = onClick, // Asigna la función lambda para el evento de clic.
        colors = ButtonDefaults.buttonColors(
            containerColor = activeColor, // Establece el color de fondo cuando está habilitado.
            disabledContainerColor = inactiveColor // Establece el color de fondo cuando está deshabilitado.
        ),
        shape = RoundedCornerShape(cornerRadius), // Aplica la forma con las esquinas redondeadas especificadas.
        enabled = isEnabled // Controla si el botón está habilitado o deshabilitado.
    ) {
        // Muestra el texto si está presente y el botón no está en estado de carga.
        if (text != null && !isLoading) {
            Text(
                text = text, // Texto a mostrar.
                color = MaterialTheme.colorScheme.onPrimary, // Color del texto sobre el color primario.
                style = MaterialTheme.typography.labelMedium // Aplica un estilo de texto predefinido.
            )
        }
        // Muestra el ícono si está presente y el botón no está en estado de carga.
        if (icon != null && !isLoading) {
            Icon(
                painter = painterResource(id = icon), // Carga el ícono desde el recurso.
                contentDescription = "" // Descripción para accesibilidad (se deja vacía aquí).
            )
        }
        // Muestra la animación de carga si isLoading es true.
        if (isLoading) {
            // Carga la composición de la animación de Lottie desde un recurso raw.
            val composition by rememberLottieComposition(
                LottieCompositionSpec.RawRes(R.raw.button_loading_anim)
            )
            // Composable para mostrar la animación de Lottie.
            LottieAnimation(
                modifier = Modifier.size(72.dp), // Establece el tamaño de la animación.
                composition = composition, // Asigna la composición cargada.
                iterations = LottieConstants.IterateForever // Hace que la animación se repita indefinidamente.
            )
        }
    }
}