package com.app.sanimex.presentation.components

import androidx.compose.animation.core.FastOutSlowInEasing
import androidx.compose.animation.core.RepeatMode
import androidx.compose.animation.core.animateFloat
import androidx.compose.animation.core.infiniteRepeatable
import androidx.compose.animation.core.rememberInfiniteTransition
import androidx.compose.animation.core.tween
import androidx.compose.runtime.Composable
import androidx.compose.ui.geometry.Offset
import androidx.compose.ui.graphics.Brush
import androidx.compose.ui.graphics.Color

/**
 * Composable que crea un [Brush] lineal para un efecto de shimmer (brillo) animado.
 *
 * Este brush se puede aplicar como fondo a elementos de la interfaz de usuario para
 * simular un efecto de carga o resaltado visual. La animación del brillo se logra
 * mediante un gradiente lineal que se desplaza infinitamente.
 *
 * @return Un [Brush] lineal animado que representa el efecto de shimmer.
 * @author David Duarte
 * @version 1.0
 */
@Composable
fun shimmerBrush():Brush {
    // Crea una transición infinita que se repetirá continuamente.
    val transition = rememberInfiniteTransition(label = "")
    // Anima un valor float desde 0f hasta 1000f de forma infinita.
    val transitionAnim = transition.animateFloat(
        initialValue = 0f, // Valor inicial de la animación.
        targetValue = 1000f, // Valor final de la animación.
        label = "", // Etiqueta para la animación (puede ser útil para debugging).
        animationSpec = infiniteRepeatable( // Especificación para la repetición infinita de la animación.
            animation = tween( // Define la animación como un tween (animación suave entre dos puntos).
                durationMillis = 1000, // Duración de cada ciclo de la animación en milisegundos.
                easing = FastOutSlowInEasing // Interpolación para la animación (inicio y fin más lentos, parte central más rápida).
            ),
            repeatMode = RepeatMode.Restart // Modo de repetición: la animación se reinicia desde el valor inicial al finalizar.
        )
    )

    // Crea un pincel de gradiente lineal.
    return Brush.linearGradient(
        colors = listOf(
            Color(0xFFD1D1D1).copy(alpha = 0.8f), // Color gris claro con una opacidad del 80%.
            Color(0xFFD1D1D1).copy(alpha = 0.4f), // Color gris claro con una opacidad del 40% (para el "brillo").
            Color(0xFFD1D1D1).copy(alpha = 0.8f)  // Color gris claro con una opacidad del 80%.
        ),
        start = Offset.Zero, // El gradiente comienza desde la esquina superior izquierda (0, 0).
        end = Offset(x = transitionAnim.value, y = transitionAnim.value) // El punto final del gradiente se desplaza según el valor animado, creando el efecto de "barrido" del brillo.
    )
}