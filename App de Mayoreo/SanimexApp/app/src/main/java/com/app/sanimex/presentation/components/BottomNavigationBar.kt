package com.app.sanimex.presentation.components

import androidx.compose.foundation.background
import androidx.compose.foundation.clickable
import androidx.compose.foundation.interaction.MutableInteractionSource
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.material3.Icon
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.remember
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.shadow
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.unit.dp
import com.app.sanimex.R
import com.app.sanimex.core.util.Constants


/**
 * Composable que representa la barra de navegación inferior de la aplicación.
 *
 * Muestra un conjunto de íconos y texto para navegar entre las diferentes secciones
 * de la aplicación. El número de ítems y los destinos de navegación varían
 * dependiendo del rol del usuario (definido en [Constants.idRol]).
 *
 * @param selectedItem Índice del ítem actualmente seleccionado en la barra de navegación (por defecto es 0).
 * @param onNavigateToHome Función lambda llamada al hacer clic en el ícono de "Inicio".
 * @param onNavigateToAccount Función lambda llamada al hacer clic en el ícono de "Cuenta".
 * @param onNavigateToCart Función lambda llamada al hacer clic en el ícono de "Carrito".
 * @param onNavigateToUbicacion Función lambda llamada al hacer clic en el ícono de "Ubicación".
 * @param onNavigateToTicket Función lambda llamada al hacer clic en el ícono de "Ticket" (para roles específicos).
 * @param onNavigateToTicketGerente Función lambda llamada al hacer clic en el ícono de "Cotización" (para roles específicos).
 * @param onNavigateToExplore Función lambda llamada al hacer clic en el ícono de "Explorar/Buscar".
 * @author David Duarte
 * @version 1.0
 */
@Composable
fun BottomNavigationBar(
    // Parámetro que indica el ítem seleccionado, por defecto es 0 (Home)
    selectedItem: Int = 0,
    // Funciones lambda para manejar la navegación a cada sección
    onNavigateToAccount: () -> Unit = {},
    onNavigateToCart: () -> Unit = {},
    onNavigateToUbicacion: () -> Unit = {},
    onNavigateToTicket: () -> Unit = {},
    onNavigateToTicketGerente: () -> Unit = {},
    onNavigateToExplore:  () -> Unit = {}
) {
    // Contenedor principal que organiza los ítems en una fila
    Row(
        modifier = Modifier
            .fillMaxWidth() // Ocupa todo el ancho de la pantalla
            .height(60.dp) // Altura fija de 60 dp
            .shadow(elevation = 30.dp) // Sombra con elevación de 30 dp
            .background(color = MaterialTheme.colorScheme.background), // Fondo con el color de fondo del tema
        horizontalArrangement = Arrangement.SpaceEvenly, // Espacio uniforme entre ítems
        verticalAlignment = Alignment.CenterVertically // Alineación vertical al centro
    ) {
        if (Constants.idRol == 10 || Constants.idRol == 8 || Constants.idRol == 1){
            // Iteramos del 0 al 4 para crear los 5 ítems de navegación
            for (i in 0..3) {
                // Contenedor para cada ítem, que responde al clic
                Column(
                    modifier = Modifier.clickable(
                        interactionSource = remember { MutableInteractionSource() }, // Manejo de interacciones
                        indication = null // Sin indicación visual al hacer clic
                    ) {
                        // Navegación según el índice del ítem
                        when (i) {
                            //0 -> onNavigateToHome() // Navegar a Home
                            0 ->  onNavigateToExplore()// Navegar a Explorar
                            1 -> onNavigateToTicketGerente() // Navegar al Carrito
                            2 -> onNavigateToUbicacion() // Navegar a Ofertas
                            3 -> onNavigateToAccount() // Navegar a Cuenta
                        }
                    },
                    horizontalAlignment = Alignment.CenterHorizontally // Alineación horizontal al centro
                ) {
                    // Icono para el ítem
                    Icon(
                        painter = painterResource(
                            id = when (i) {
                                //0 -> R.drawable.home_icon // Ícono de Home
                                0 -> R.drawable.bottom_bar_explore_ic // Ícono de Explorar
                                1 -> R.drawable.bottom_bar_tickets_ic // Ícono de Carrito
                                2 -> R.drawable.bottom_bar_location_ic // Ícono de ubicaciones
                                3 -> R.drawable.bottom_bar_profile_ic // Ícono de Cuenta
                                else -> R.drawable.bottom_bar_profile_ic // Valor por defecto
                            }
                        ),
                        contentDescription = "", // Descripción para accesibilidad (vacío aquí)
                        tint = if (i == selectedItem) MaterialTheme.colorScheme.primary else MaterialTheme.colorScheme.onSurfaceVariant // Color del ícono según si está seleccionado
                    )
                    // Espacio entre el ícono y el texto
                    Spacer(modifier = Modifier.height(4.dp))
                    // Texto para el ítem
                    Text(
                        text = when (i) {
                            //0 -> stringResource(R.string.home) // Texto para Home
                            0 -> stringResource(R.string.buscar)   // Texto para Explorar
                            1 -> stringResource(R.string.cotizacion) // Texto para Carrito
                            2 -> stringResource(R.string.ubicacion) // Texto para Ofertas
                            3 -> stringResource(R.string.Perfil) // Texto para Cuenta
                            else -> "" // Valor por defecto
                        },
                        color = if (i == selectedItem) MaterialTheme.colorScheme.primary else Color.Unspecified, // Color del texto según si está seleccionado
                        style = MaterialTheme.typography.titleSmall // Estilo del texto
                    )
                }
            }
        }else {
            // Iteramos del 0 al 4 para crear los 5 ítems de navegación
            for (i in 0..3) {
                // Contenedor para cada ítem, que responde al clic
                Column(
                    modifier = Modifier.clickable(
                        interactionSource = remember { MutableInteractionSource() }, // Manejo de interacciones
                        indication = null // Sin indicación visual al hacer clic
                    ) {
                        // Navegación según el índice del ítem
                        when (i) {
                            //0 -> onNavigateToHome() // Navegar a Home
                            0 -> onNavigateToExplore()// Navegar a Explorar
                            1 -> onNavigateToCart() // Navegar al Carrito
                            2 -> onNavigateToTicket()// Navegar a Ofertas
                            3 -> onNavigateToAccount() // Navegar a Cuenta
                        }
                    },
                    horizontalAlignment = Alignment.CenterHorizontally // Alineación horizontal al centro
                ) {
                    // Icono para el ítem
                    Icon(
                        painter = painterResource(
                            id = when (i) {
                                //0 -> R.drawable.home_icon // Ícono de Home
                                0 -> R.drawable.bottom_bar_explore_ic // Ícono de Explorar
                                1 -> R.drawable.bottom_bar_cart_ic // Ícono de Carrito
                                2 -> R.drawable.bottom_bar_tickets_ic // Ícono de Ofertas
                                3 -> R.drawable.bottom_bar_profile_ic // Ícono de Cuenta
                                else -> R.drawable.bottom_bar_profile_ic // Valor por defecto
                            }
                        ),
                        contentDescription = "", // Descripción para accesibilidad (vacío aquí)
                        tint = if (i == selectedItem) MaterialTheme.colorScheme.primary else MaterialTheme.colorScheme.onSurfaceVariant // Color del ícono según si está seleccionado
                    )
                    // Espacio entre el ícono y el texto
                    Spacer(modifier = Modifier.height(4.dp))
                    // Texto para el ítem
                    Text(
                        text = when (i) {
                            //0 -> stringResource(R.string.home) // Texto para Home
                            0 -> stringResource(R.string.buscar)   // Texto para Explorar
                            1 -> stringResource(R.string.carro) // Texto para Carrito
                            2 -> stringResource(R.string.cotizacion) // Texto para Ofertas
                            3 -> stringResource(R.string.Perfil) // Texto para Cuenta
                            else -> "" // Valor por defecto
                        },
                        color = if (i == selectedItem) MaterialTheme.colorScheme.primary else Color.Unspecified, // Color del texto según si está seleccionado
                        style = MaterialTheme.typography.titleSmall // Estilo del texto
                    )
                }
            }
        }
    }
}
