package com.app.sanimex.presentation.screens.explore

import android.annotation.SuppressLint
import androidx.compose.animation.AnimatedVisibility
import androidx.compose.animation.core.tween
import androidx.compose.animation.fadeIn
import androidx.compose.animation.fadeOut
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.text.ClickableText
import androidx.compose.material.Surface
import androidx.compose.material3.HorizontalDivider
import androidx.compose.material3.Icon
import androidx.compose.material3.IconButton
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Scaffold
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.text.SpanStyle
import androidx.compose.ui.text.buildAnnotatedString
import androidx.compose.ui.text.input.ImeAction
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.text.withStyle
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import androidx.hilt.navigation.compose.hiltViewModel
import com.airbnb.lottie.compose.LottieAnimation
import com.airbnb.lottie.compose.LottieCompositionSpec
import com.airbnb.lottie.compose.LottieConstants
import com.airbnb.lottie.compose.rememberLottieComposition
import com.app.sanimex.R
import com.app.sanimex.core.util.Constants
import com.app.sanimex.presentation.components.BottomNavigationBar
import com.app.sanimex.presentation.components.MainTextFieldSearch

@SuppressLint("UnusedMaterial3ScaffoldPaddingParameter")

@Composable
fun ExploreScreen(
    viewModel: ExploreViewModel = hiltViewModel(),
    onNavigateToFav: () -> Unit = {},
    onNavigateToHome: () -> Unit = {},
    onNavigateToSearchResults: (String) -> Unit = {},
    onNavigateToCart: () -> Unit = {},
    onNavigateToAccount: () -> Unit = {},
    onNavigateToUbicacion: () -> Unit = {},
    onNavigateToTicket: () -> Unit = {},
    onNavigateToTicketGerente: () -> Unit = {}
) {
    val uiState by viewModel.uiState.collectAsState()

    Scaffold(
        bottomBar = {

            BottomNavigationBar(
                selectedItem = 0,
                onNavigateToCart = onNavigateToCart,
                onNavigateToAccount = onNavigateToAccount,
                onNavigateToUbicacion = onNavigateToUbicacion,
                onNavigateToTicket = onNavigateToTicket,
                onNavigateToTicketGerente = onNavigateToTicketGerente
            )
        }
    ) {
        ExploreScreenContent(
            uiState = uiState,
            onSearchQueryChanged = viewModel::onSearchQueryChanged,
            onFavoriteClicked = onNavigateToFav,
            onSearchResultClicked = onNavigateToSearchResults,
            onNavigateToCart = onNavigateToCart,
            onNavigateToHome = onNavigateToHome,
        )
    }
}

@Composable
private fun ExploreScreenContent(
    uiState: ExploreScreenUIState,
    onSearchQueryChanged: (String) -> Unit,
    onFavoriteClicked: () -> Unit,
    onNavigateToCart: () -> Unit,
    onSearchResultClicked: (String) -> Unit,
    onNavigateToHome: () -> Unit
) {
    Surface(
        modifier = Modifier
            .fillMaxSize()
    ) {
        Column(
            Modifier
                .fillMaxSize()
                .padding(vertical = 16.dp) // Espaciado vertical de 16 dp
        ) {
            // Encabezado de la pantalla con la barra de búsqueda y el botón de favoritos
            ScreenHeader(
                searchQuery = uiState.searchQuery, // Pasar la consulta de búsqueda actual
                onSearchQueryChanged = onSearchQueryChanged, // Manejar cambios en la búsqueda
                onFavoriteClicked = onFavoriteClicked, // Manejar clic en favoritos
                onNavigateToCart = onNavigateToCart,
                onNavigateToHome = onNavigateToHome
            )
            AnimatedVisibility(
                visible = !uiState.isLoading && uiState.isError,
                enter = fadeIn(animationSpec = tween(delayMillis = 500)),
                exit = fadeOut()
            ) {
                EmptyState(
                )
            }
            // Se muestra la lista de resultados de búsqueda si hay una consulta activa
            AnimatedVisibility(
                visible = uiState.searchQuery.isNotEmpty(), // Visible solo si hay una consulta
                enter = fadeIn(), // Animación de entrada
                exit = fadeOut() // Animación de salida
            ) {
                // Lista vertical de resultados de búsqueda
                LazyColumn(
                    modifier = Modifier.padding(horizontal = 16.dp, vertical = 16.dp) // Espaciado horizontal y vertical
                ) {
                    // Se itera sobre la lista de resultados de búsqueda
                    items(count = uiState.searchResultsList.size) { pos ->
                        // Cada ítem de la lista es un resultado de búsqueda
                        SearchItem(
                            result = uiState.searchResultsList[pos], // Pasar el resultado actual
                            onSearchResultClicked = onSearchResultClicked // Manejar clic en el resultado
                        )
                    }
                }
            }
        }
    }
}


@Composable
private fun ScreenHeader(
    searchQuery: String,
    onSearchQueryChanged: (String) -> Unit,
    onFavoriteClicked: () -> Unit,
    onNavigateToCart : ()-> Unit,
    onNavigateToHome: () -> Unit
) {
    Column(
        Modifier.fillMaxWidth() // Ocupará todo el ancho disponible
    ) {
        Row(
            modifier = Modifier.padding(bottom = 24.dp, start = 14.dp, end = 14.dp),
            verticalAlignment = Alignment.CenterVertically
        ) {
            MainTextFieldSearch(
                modifier = Modifier
                    .height(56.dp) // Altura fija del campo de texto
                    .weight(3f), // Toma el 3/4 del ancho disponible en la fila
                value = searchQuery, // Valor actual de la búsqueda
                onValueChanged = onSearchQueryChanged, // Callback para manejar cambios en el texto
                placeHolder = stringResource(R.string.buscar_producto), // Texto de marcador de posición
                imeAction = ImeAction.Search, // Acción del teclado (buscar)
                leadingIcon = R.drawable.search_icon // Ícono que aparece al inicio del campo de texto
            )
            if (Constants.idRol == 10 || Constants.idRol == 8 || Constants.idRol == 1) {
                IconButton(
                    onClick = onNavigateToHome
                ) {
                    Icon(
                        painter = painterResource(id = R.drawable.home_icon),
                        contentDescription = "",
                        tint = Color.Unspecified
                    )
                }
                // Botón para acceder a los Carro
                IconButton(
                    onClick = onNavigateToCart // Callback al hacer clic
                ) {
                    // Ícono para el botón de favoritos
                    Icon(
                        painter = painterResource(id = R.drawable.m_car_ic), // Ícono de favoritos
                        contentDescription = "", // Descripción para accesibilidad (vacío aquí)
                        tint = Color.Unspecified // Color del ícono (sin tintado específico)
                    )
                }
            }else {
                IconButton(
                    onClick = onFavoriteClicked
                ) {
                    Icon(
                        painter = painterResource(id = R.drawable.user_role_icon),
                        contentDescription = "",
                        tint = Color.Unspecified
                    )
                }
                IconButton(
                    onClick = onNavigateToHome
                ) {
                    Icon(
                        painter = painterResource(id = R.drawable.home_icon),
                        contentDescription = "",
                        tint = Color.Unspecified
                    )
                }
            }
        }
        // Divisor para separar el encabezado del resto del contenido
        HorizontalDivider(
            color = MaterialTheme.colorScheme.onSurfaceVariant.copy(alpha = 0.2f) // Color del divisor con transparencia
        )
    }
}


@Composable
private fun SearchItem(
    // Resultado de búsqueda que se mostrará
    result: String,
    // Callback que se llama al hacer clic en el resultado
    onSearchResultClicked: (String) -> Unit
) {
    // Fila que organiza el ícono y el texto del resultado de búsqueda
    Row(
        verticalAlignment = Alignment.CenterVertically // Alineación vertical al centro
    ) {
        // Ícono que representa la búsqueda
        Icon(
            painter = painterResource(id = R.drawable.search_icon), // Cargar el ícono de búsqueda
            contentDescription = "", // Descripción para accesibilidad (vacío aquí)
            tint = Color.Unspecified // Color del ícono (sin tintado específico)
        )
        // Texto clicable que muestra el resultado de búsqueda
        ClickableText(
            modifier = Modifier
                .fillMaxWidth() // Ocupará todo el ancho disponible
                .padding(top = 8.dp, bottom = 8.dp, start = 8.dp), // Espaciado alrededor del texto
            text = buildAnnotatedString {
                withStyle(SpanStyle(color = MaterialTheme.colorScheme.onSurfaceVariant)) {
                    // Estilo del texto, usando el color del tema para texto en superficie variante
                    append(result) // Añadir el resultado al texto
                }
            },
            style = MaterialTheme.typography.bodySmall, // Estilo del texto
            onClick = { onSearchResultClicked(result.split(" ")[0]) } // Llamada al callback al hacer clic
        )
    }
}

@Composable
private fun EmptyState(
) {
    Box(
        modifier = Modifier
            .fillMaxSize()
            .padding(horizontal = 16.dp),
        contentAlignment = Alignment.Center
    ) {
        Column(
            horizontalAlignment = Alignment.CenterHorizontally
        ) {
            val composition by rememberLottieComposition(
                LottieCompositionSpec.RawRes(R.raw.search_empty_anim)
            )
            LottieAnimation(
                modifier = Modifier.size(290.dp),
                composition = composition,
                iterations = LottieConstants.IterateForever
            )
            Text(
                modifier = Modifier.padding(top = 16.dp),
                text = stringResource(R.string.you_don_t_have_items_in_your_Explore),
                color = MaterialTheme.colorScheme.onBackground,
                style = MaterialTheme.typography.headlineSmall,
                textAlign = TextAlign.Center
            )
            Text(
                modifier = Modifier.padding(top = 8.dp , start = 8.dp, end = 8.dp),
                text = stringResource(R.string.items_marked_as_Explore_will_be_shown_here),
                color = MaterialTheme.colorScheme.onSurfaceVariant,
                style = MaterialTheme.typography.bodyMedium
            )
        }
    }
}

@Preview
@Composable
private fun ExploreScreenPreview() {
    // Vista previa de la pantalla de exploración
    ExploreScreen() // Renderiza la pantalla de exploración en el editor
}
