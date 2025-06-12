package com.app.sanimex.presentation.components

import androidx.compose.runtime.Composable
import androidx.compose.ui.Modifier
import androidx.compose.ui.layout.ContentScale
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.res.painterResource
import coil.ImageLoader
import coil.compose.AsyncImage
import coil.request.CachePolicy
import com.app.sanimex.R
import com.app.sanimex.core.util.Constants
import kotlinx.coroutines.Dispatchers

/**
 * Composable para cargar y mostrar imágenes desde una URL de red utilizando la librería Coil.
 *
 * Este composable simplifica la carga de imágenes remotas, proporcionando soporte para un
 * marcador de posición mientras se carga la imagen, una imagen de error en caso de fallo
 * y opciones para escalar el contenido de la imagen. Utiliza una configuración personalizada
 * de [ImageLoader] para habilitar el caching en memoria y disco.
 *
 * @param modifier Modificador para personalizar el layout del composable de la imagen (por defecto, un [Modifier] vacío).
 * @param model La parte de la URL de la imagen a cargar. Se concatena con [Constants.IMAGE_URL] para formar la URL completa.
 * @param placeHolder Recurso de imagen (Int) a mostrar mientras se carga la imagen de red (por defecto, [R.drawable.product_image_placeholder]).
 * @param error Recurso de imagen (Int) a mostrar si la carga de la imagen de red falla (por defecto, [R.drawable.image_error]).
 * @param contentScale Estrategia para escalar la imagen dentro de sus límites (por defecto, [ContentScale.None]).
 * @author David Duarte
 * @version 1.0
 */
@Composable
fun NetworkImage(
    modifier: Modifier = Modifier,
    model: String,
    placeHolder: Int = R.drawable.product_image_placeholder,
    error: Int = R.drawable.image_error,
    contentScale: ContentScale = ContentScale.None
) {
    // Composable AsyncImage de la librería Coil para cargar imágenes de forma asíncrona.
    AsyncImage(
        modifier = modifier, // Aplica el modificador recibido.
        model = Constants.IMAGE_URL + model, // Construye la URL completa de la imagen.
        contentDescription = "", // Descripción para accesibilidad (se deja vacía aquí).
        placeholder = painterResource(id = placeHolder), // Muestra esta imagen mientras se carga la imagen de red.
        error = painterResource(id = error), // Muestra esta imagen si la carga de la imagen de red falla.
        contentScale = contentScale, // Define cómo se debe escalar la imagen dentro de sus límites.
        imageLoader = ImageLoader // Configuración personalizada del ImageLoader de Coil.
            .Builder(LocalContext.current) // Utiliza el contexto local para construir el ImageLoader.
            .memoryCachePolicy(CachePolicy.ENABLED) // Habilita el caching en memoria.
            .respectCacheHeaders(false) // No respeta las cabeceras de caché HTTP para la política de caché.
            .networkCachePolicy(CachePolicy.ENABLED) // Habilita el caching de red (descarga de la imagen).
            .diskCachePolicy(CachePolicy.ENABLED) // Habilita el caching en disco.
            .crossfade(true) // Aplica un efecto de fundido cruzado al cargar la nueva imagen.
            .dispatcher(Dispatchers.IO) // Utiliza el dispatcher de IO para las operaciones de red.
            .build() // Construye la instancia del ImageLoader.
    )
}