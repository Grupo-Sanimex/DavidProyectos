package com.app.sanimex.core.util

import com.app.sanimex.domain.model.CarritoFinal

/**
 * Extensión de función para el tipo nullable [Number] que devuelve el número original si no es nulo,
 * o un valor alternativo proporcionado si es nulo.
 *
 * Esta función facilita el manejo de valores numéricos que pueden ser nulos, permitiendo
 * proporcionar un valor de respaldo de forma concisa.
 *
 * @param alt El valor [Number] que se devolverá si el número receptor es nulo.
 * @return El número receptor si no es nulo, o el valor de [alt] si el receptor es nulo.
 * @author David Duarte
 * @version 1.0
 */
fun Number?.ifNull(alt: Number): Number {
    return this ?: alt
}

/**
 * Extensión de función para la lista de [CarritoFinal] que calcula el precio total de los items en el carrito.
 *
 * Itera a través de cada elemento [CarritoFinal] en la lista, calcula el precio efectivo
 * (considerando el descuento si está presente y no es cero), y suma el resultado al precio total.
 * El precio efectivo se calcula utilizando el precio original si no hay descuento o si el descuento es cero,
 * de lo contrario, se utiliza el valor del descuento. Finalmente, devuelve el precio total como un [Float].
 *
 * @return El precio total de todos los items en la lista de [CarritoFinal] como un [Float].
 */
fun List<CarritoFinal>.calcItemsPriceF(): Float {
    var itemsPrice = 0.0f
    this.forEach {
        val effectivePrice = it.discount?.takeIf { discount -> discount != 0 } ?: it.price
        itemsPrice += effectivePrice.toFloat() * it.quantity
    }
    return itemsPrice
}