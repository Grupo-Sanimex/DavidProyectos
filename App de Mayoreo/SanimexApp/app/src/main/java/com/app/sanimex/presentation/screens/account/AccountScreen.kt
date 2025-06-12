package com.app.sanimex.presentation.screens.account

import android.annotation.SuppressLint
import androidx.compose.animation.AnimatedVisibility
import androidx.compose.animation.core.tween
import androidx.compose.animation.fadeIn
import androidx.compose.animation.fadeOut
import androidx.compose.foundation.Image
import androidx.compose.foundation.background
import androidx.compose.foundation.border
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.shape.CircleShape
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material3.Card
import androidx.compose.material3.CardDefaults
import androidx.compose.material3.Divider
import androidx.compose.material3.ExperimentalMaterial3Api
import androidx.compose.material3.HorizontalDivider
import androidx.compose.material3.Icon
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Scaffold
import androidx.compose.material3.Text
import androidx.compose.material3.TextButton
import androidx.compose.runtime.Composable
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.rememberCoroutineScope
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.layout.ContentScale
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import androidx.compose.ui.window.Dialog
import androidx.compose.ui.window.DialogProperties
import androidx.hilt.navigation.compose.hiltViewModel
import com.app.sanimex.R
import com.app.sanimex.core.util.Constants
import com.app.sanimex.domain.model.User
import com.app.sanimex.presentation.components.BottomNavigationBar
import com.app.sanimex.presentation.components.MainButton
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.delay
import kotlinx.coroutines.launch

@OptIn(ExperimentalMaterial3Api::class)
@SuppressLint("UnusedMaterial3ScaffoldPaddingParameter")
@Composable
fun AccountScreen(
    onNavigateToExplore: () -> Unit = {},
    onNavigateToCart: () -> Unit = {},
    onNavigateToUbicacion: () -> Unit = {},
    onNavigateToTicket: () -> Unit = {},
    onNavigateToTicketGerente: () -> Unit = {},
    viewModel: AccountViewModel = hiltViewModel(),
    onNavigateToLogin: () -> Unit = {}
) {

    val uiState by viewModel.uiState.collectAsState()

    val scope: CoroutineScope = rememberCoroutineScope()
    val selectedItem = if (Constants.idRol == 10 || Constants.idRol == 8 || Constants.idRol == 1) {
        3
    } else {
        3
    }
    Scaffold(
        bottomBar = {
            BottomNavigationBar(
                selectedItem = selectedItem,
                onNavigateToExplore = onNavigateToExplore,
                onNavigateToCart = onNavigateToCart,
                onNavigateToUbicacion = onNavigateToUbicacion,
                onNavigateToTicket = onNavigateToTicket,
                onNavigateToTicketGerente = onNavigateToTicketGerente
            )
        }
    ) {
        it.calculateBottomPadding()
        AccountScreenContent(
            uiState = uiState,
            onLogoutClicked = {
                viewModel.logout()
                scope.launch {
                    delay(500)
                    onNavigateToLogin()
                }
            }
        )
    }
}
@Composable
private fun AccountScreenContent(
    uiState: AccountUiState,
    onLogoutClicked: () -> Unit
) {
    var isShowLogoutDialog by remember { mutableStateOf(false) }

    androidx.compose.material.Surface(
        modifier = Modifier.fillMaxSize()
    ) {
        Column(
            Modifier
                .fillMaxWidth()
                .padding(top = 24.dp),
        ) {
           ScreenHeader()
            AnimatedVisibility(
                visible = !uiState.isLoading,
                enter = fadeIn(animationSpec = tween(delayMillis = 500)),
                exit = fadeOut()
            ) {
                Box(
                    modifier = Modifier
                        .fillMaxSize()
                        .padding(vertical = 16.dp)
                ) {
                    Column(
                        Modifier
                            .padding(horizontal = 16.dp)
                            .align(Alignment.TopCenter)
                    ) {
                        uiState.me?.let { info ->
                            MainInfoSection(
                                name = info.nombre + " " + info.aPaterno,
                                id = info.id
                            )
                            SecondaryInfoSection(
                                info = info
                            )
                        }
                        // Espacio opcional para separar el contenido del botón
                        Spacer(modifier = Modifier.height(16.dp))

                        MainButton(
                            modifier = Modifier
                                .fillMaxWidth()
                                .height(56.dp)
                                .padding(horizontal = 16.dp),
                            text = stringResource(R.string.logout),
                            activeColor = MaterialTheme.colorScheme.secondary,
                            inactiveColor = MaterialTheme.colorScheme.secondary.copy(alpha = 0.5f),
                            onClick = {
                                isShowLogoutDialog = true
                            }
                        )
                    }


                }
            }
        }


        AnimatedVisibility(
            visible = isShowLogoutDialog,
            enter = fadeIn(),
            exit = fadeOut()
        ) {
            LogoutDialog(
                onDismiss = {
                    isShowLogoutDialog = false
                },
                onLogoutClicked = onLogoutClicked
            )
        }
    }
}


@Composable
private fun ScreenHeader() {
    Column(
        Modifier.fillMaxWidth()
    ) {
        Text(
            modifier = Modifier.padding(horizontal = 16.dp),
            text = stringResource(R.string.profile),
            color = MaterialTheme.colorScheme.onBackground,
            style = MaterialTheme.typography.titleMedium
        )
        HorizontalDivider(
            modifier = Modifier.padding(top = 28.dp),
            color = MaterialTheme.colorScheme.onSurfaceVariant.copy(alpha = 0.3f)
        )
    }
}

@Composable
private fun MainInfoSection(
    name: String,
    id: String
) {

    Row(
        Modifier.fillMaxWidth(),
        verticalAlignment = Alignment.CenterVertically
    ) {

        Image(
            modifier = Modifier
                .size(72.dp)
                .clip(CircleShape),
            painter = painterResource(id = R.drawable.user_placeholder),
            contentDescription = "",
            contentScale = ContentScale.Crop
        )

        Column(
            Modifier.padding(start = 16.dp)
        ) {

            Text(
                text = name,
                color = MaterialTheme.colorScheme.onBackground,
                style = MaterialTheme.typography.labelMedium
            )

            Text(
                modifier = Modifier.padding(top = 4.dp),
                text = id,
                color = MaterialTheme.colorScheme.onSurfaceVariant,
                style = MaterialTheme.typography.bodySmall
            )
        }
    }
}


@Composable
private fun SecondaryInfoSection(
    info: User
) {
    LazyColumn(
        modifier = Modifier.padding(horizontal = 4.dp, vertical = 32.dp)
    ) {

        items(
            count = 4
        ) { pos ->
            InfoItem(
                pos = pos,
                info = info
            )
        }
    }
}


@Composable
private fun InfoItem(
    pos: Int,
    info: User
) {

    Row(
        Modifier
            .fillMaxWidth()
            .padding(vertical = 16.dp),
        verticalAlignment = Alignment.CenterVertically,
        horizontalArrangement = Arrangement.SpaceBetween
    ) {

        Row(
            verticalAlignment = Alignment.CenterVertically // Alinea los elementos en el centro verticalmente
        ) {
            // Icono correspondiente a la posición actual
            Icon(
                painter = painterResource(
                    id = when (pos) { // Selecciona el icono según la posición
                        0 -> R.drawable.email_icon // Ícono de correo
                        1 -> R.drawable.phone_icon // Ícono de teléfono
                        2 -> R.drawable.user_role_icon // Ícono de rol
                        3 -> R.drawable.active_icon // Ícono de estado
                        else -> R.drawable.check_icon // Ícono por defecto
                    }
                ),
                contentDescription = "", // Descripción del contenido para accesibilidad, puede ser mejorado
                tint = MaterialTheme.colorScheme.primary // Color del ícono según el tema
            )
            // Texto que describe la información del ítem
            Text(
                modifier = Modifier.padding(start = 16.dp), // Padding a la izquierda de 16 dp
                text = when (pos) { // Selecciona el texto según la posición
                    0 -> stringResource(R.string.email) // Texto para correo
                    1 -> stringResource(id = R.string.phone_number) // Texto para número de teléfono
                    2 -> stringResource(R.string.idSucursal) // Texto para idsucursal
                    3 -> stringResource(R.string.status) // Texto para estado
                    else -> "" // Texto vacío por defecto
                },
                color = MaterialTheme.colorScheme.onBackground, // Color del texto según el tema
                style = MaterialTheme.typography.labelSmall // Estilo de texto para la descripción
            )
        }
        // Texto que muestra el valor correspondiente a la descripción
        Text(
            modifier = Modifier.padding(start = 16.dp), // Padding a la izquierda de 16 dp
            text = when (pos) { // Selecciona el valor a mostrar según la posición
                0 -> info.correo // Muestra el correo del usuario
                1 -> info.telefono // Muestra el número de teléfono del usuario
                2 -> info.idSucursal // Muestra el rol del usuario
                3 -> if (info.status) "Active" else "Not Active" // Muestra el estado (activo o no)
                else -> "" // Texto vacío por defecto
            },
            color = MaterialTheme.colorScheme.onSurfaceVariant, // Color del texto según el tema
            style = MaterialTheme.typography.bodySmall // Estilo de texto para el valor
        )
    }
}


@Composable
private fun LogoutDialog(
    onDismiss: () -> Unit, // Función que se llama al cerrar el diálogo
    onLogoutClicked: () -> Unit // Función que se llama al confirmar el cierre de sesión
) {
    // Crea un diálogo que se puede cerrar al hacer clic fuera de él
    Dialog(
        onDismissRequest = onDismiss, // Maneja la solicitud de cerrar el diálogo
        DialogProperties(usePlatformDefaultWidth = false) // Desactiva el ancho predeterminado de la plataforma
    ) {
        // Crea una tarjeta que contiene el contenido del diálogo
        Card(
            modifier = Modifier
                .padding(16.dp) // Padding de 16 dp alrededor de la tarjeta
                .fillMaxWidth(), // La tarjeta ocupatodo el ancho disponible
            shape = RoundedCornerShape(16.dp), // Bordes redondeados para la tarjeta
            colors = CardDefaults.cardColors(containerColor = MaterialTheme.colorScheme.surface) // Color de fondo de la tarjeta
        ) {
            // Columna para organizar el contenido del diálogo
            Column(
                modifier = Modifier
                    .padding(top = 26.dp, start = 16.dp, end = 16.dp, bottom = 32.dp), // Padding interno
            ) {
                // Título del diálogo
                Text(
                    modifier = Modifier
                        .fillMaxWidth() // El texto ocupatodo el ancho
                        .padding(bottom = 16.dp), // Padding inferior de 16 dp
                    text = stringResource(R.string.logout), // Texto de cierre de sesión
                    color = MaterialTheme.colorScheme.onBackground, // Color del texto
                    style = MaterialTheme.typography.titleMedium, // Estilo de texto para el título
                    textAlign = TextAlign.Center // Centra el texto
                )
                // Mensaje de confirmación
                Text(
                    modifier = Modifier.padding(bottom = 24.dp), // Padding inferior de 24 dp
                    text = stringResource(R.string.are_you_sure_you_want_to_logout), // Mensaje de confirmación
                    color = MaterialTheme.colorScheme.onBackground, // Color del texto
                    style = MaterialTheme.typography.labelSmall, // Estilo de texto para el mensaje
                )
                // Fila para los botones
                Row(
                    modifier = Modifier.fillMaxWidth(), // La fila ocupatodo el ancho disponible
                    verticalAlignment = Alignment.CenterVertically, // Alinea los elementos verticalmente en el centro
                ) {
                    // Botón principal para confirmar cierre de sesión
                    MainButton(
                        modifier = Modifier
                            .padding(end = 8.dp) // Padding a la derecha de 8 dp
                            .weight(1f) // El botón ocupa espacio proporcional
                            .height(48.dp), // Altura del botón
                        text = stringResource(R.string.logout), // Texto del botón
                        activeColor = MaterialTheme.colorScheme.secondary, // Color activo del botón
                        inactiveColor = MaterialTheme.colorScheme.secondary.copy(alpha = 0.5f), // Color inactivo del botón
                        onClick = { // Acción al hacer clic
                            onLogoutClicked() // Llama a la función de cierre de sesión
                            onDismiss() // Cierra el diálogo
                        }
                    )
                    // Botón de cancelar
                    TextButton(
                        modifier = Modifier
                            .padding(start = 8.dp) // Padding a la izquierda de 8 dp
                            .weight(1f) // El botón ocupa espacio proporcional
                            .height(48.dp) // Altura del botón
                            .clip(RoundedCornerShape(5.dp)) // Bordes redondeados
                            .border(
                                width = 1.dp, // Ancho del borde
                                color = MaterialTheme.colorScheme.secondary, // Color del borde
                                shape = RoundedCornerShape(5.dp) // Forma del borde
                            )
                            .background(MaterialTheme.colorScheme.surface), // Color de fondo del botón
                        onClick = onDismiss // Acción al hacer clic (cerrar diálogo)
                    ) {
                        Text(
                            text = stringResource(R.string.cancel), // Texto del botón de cancelar
                            color = MaterialTheme.colorScheme.secondary, // Color del texto
                            style = MaterialTheme.typography.bodySmall // Estilo de texto para el botón
                        )
                    }
                }
            }
        }
    }
}

@Preview
@Composable
private fun AccountScreenPreview() {
    AccountScreen()
}