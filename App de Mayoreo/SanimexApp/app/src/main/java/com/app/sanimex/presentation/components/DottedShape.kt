package com.app.sanimex.presentation.components

import androidx.compose.ui.geometry.Offset
import androidx.compose.ui.geometry.Rect
import androidx.compose.ui.geometry.Size
import androidx.compose.ui.graphics.Outline
import androidx.compose.ui.graphics.Path
import androidx.compose.ui.graphics.Shape
import androidx.compose.ui.unit.Density
import androidx.compose.ui.unit.Dp
import androidx.compose.ui.unit.LayoutDirection
import kotlin.math.roundToInt

/**
 * Implementación de [Shape] que dibuja una serie de rectángulos (puntos) espaciados uniformemente
 * a lo largo del ancho de un componente. La altura de los puntos coincide con la altura del componente.
 *
 * Esta forma se puede utilizar para crear efectos visuales de líneas punteadas o discontinuas.
 *
 * @property step La distancia entre el inicio de cada punto (rectángulo) en [Dp].
 * @author David Duarte
 * @version 1.0
 */
data class DottedShape(
    val step: Dp,
) : Shape {
    /**
     * Crea el contorno ([Outline]) de la forma para un tamaño, dirección de diseño y densidad específicos.
     *
     * Calcula el número de puntos que caben en el ancho dado, ajusta el espaciado real entre ellos
     * para que los puntos se distribuyan uniformemente y luego agrega un rectángulo por cada punto al [Path].
     * La altura de cada punto es igual a la altura del tamaño proporcionado.
     *
     * @param size El tamaño del componente para el cual se creará el contorno.
     * @param layoutDirection La dirección del diseño (izquierda a derecha o derecha a izquierda). No se utiliza en esta implementación.
     * @param density La densidad de píxeles de la pantalla, utilizada para convertir [Dp] a píxeles.
     * @return Un [Outline.Generic] que contiene el [Path] con los rectángulos (puntos) dibujados.
     */
    override fun createOutline(
        size: Size,
        layoutDirection: LayoutDirection,
        density: Density
    ) = Outline.Generic(Path().apply {
        // Convierte la distancia del paso de Dp a píxeles utilizando la densidad actual.
        val stepPx = with(density) { step.toPx() }
        // Calcula el número de pasos (y por lo tanto, puntos) que caben en el ancho disponible.
        val stepsCount = (size.width / stepPx).roundToInt()
        // Calcula el espaciado real entre el inicio de cada punto para distribuirlos uniformemente.
        val actualStep = size.width / stepsCount
        // Define el tamaño de cada punto (rectángulo). El ancho es la mitad del espaciado real para crear un espacio entre puntos, y la altura es la altura del componente.
        val dotSize = Size(width = actualStep / 2, height = size.height)
        // Itera sobre el número de pasos para agregar cada punto al Path.
        for (i in 0 until stepsCount) {
            // Agrega un rectángulo al Path en la posición calculada.
            addRect(
                Rect(
                    offset = Offset(x = i * actualStep, y = 0f), // El desplazamiento horizontal se basa en el índice del paso y el espaciado real. El desplazamiento vertical es 0.
                    size = dotSize // Establece el tamaño del punto.
                )
            )
        }
        // Cierra el Path (aunque no es estrictamente necesario para rectángulos, es una buena práctica).
        close()
    })
}