package com.app.sanimex.presentation.screens.single_product

import androidx.compose.animation.AnimatedVisibility
import androidx.compose.animation.core.tween
import androidx.compose.animation.fadeIn
import androidx.compose.animation.fadeOut
import androidx.compose.foundation.background
import androidx.compose.foundation.border
import androidx.compose.foundation.clickable
import androidx.compose.foundation.interaction.MutableInteractionSource
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
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.foundation.text.ClickableText
import androidx.compose.material.Checkbox
import androidx.compose.material3.AlertDialog
import androidx.compose.material3.HorizontalDivider
import androidx.compose.material3.Icon
import androidx.compose.material3.IconButton
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Text
import androidx.compose.material3.TextButton
import androidx.compose.runtime.Composable
import androidx.compose.runtime.DisposableEffect
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.layout.ContentScale
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.text.SpanStyle
import androidx.compose.ui.text.buildAnnotatedString
import androidx.compose.ui.text.input.ImeAction
import androidx.compose.ui.text.style.TextOverflow
import androidx.compose.ui.text.withStyle
import androidx.compose.ui.unit.dp
import androidx.hilt.navigation.compose.hiltViewModel
import androidx.lifecycle.Lifecycle
import androidx.lifecycle.LifecycleEventObserver
import androidx.lifecycle.LifecycleOwner
import com.app.sanimex.R
import com.app.sanimex.core.util.Constants
import com.app.sanimex.domain.model.CorredorItem
import com.app.sanimex.domain.model.SingleProductClient
import com.app.sanimex.domain.model.SingleProductInfo
import com.app.sanimex.presentation.components.MainButton
import com.app.sanimex.presentation.components.MainTextFieldNumber
import com.app.sanimex.presentation.components.MainTextFieldSearch
import com.app.sanimex.presentation.components.NetworkImage
import com.app.sanimex.presentation.components.shimmerBrush

@Composable
fun SingleProductScreen(
    viewModel: SingleProductViewModel = hiltViewModel(),
    owner: LifecycleOwner = androidx.lifecycle.compose.LocalLifecycleOwner.current,
    productID: String = "",
    onBackClicked: () -> Unit = {},
    onNavigateToFav: () -> Unit = {},
    onNavigateToReporte: () -> Unit = {},
    onNavigateToSearchResults: (String) -> Unit = {},
    onNavigateToCart: () -> Unit
) {

    val uiState by viewModel.uiState.collectAsState()
    // Estado para controlar la visibilidad del diálogo
    SingleProductScreenContent(
        uiState = uiState,
        onSearchQueryChanged = viewModel::onSearchQueryChanged,
        onFavoriteClicked = onNavigateToFav,
        onSearchResultClicked = onNavigateToSearchResults,
        onProductClicked = viewModel::getProductData,
        onAddToCartClicked = viewModel::onAddToCartClicked,
        onEmailChanged = viewModel::onEmailChanged,
        onTipoEntregaChanged = viewModel::onTipoEntregaChanged,
        ontipoPagoChanged = viewModel::ontipoPagoChanged,
        onReporteInClicked = onNavigateToReporte,
        onBackClicked = onBackClicked,
        onNavigateToCart = onNavigateToCart,
        onResetErrorCliente = viewModel::onResetErrorCliente,
        onResetErrorCart = viewModel::onResetErrorCart,

    )
    DisposableEffect(key1 = owner) {
        val observer = LifecycleEventObserver { _, event ->
            if (event == Lifecycle.Event.ON_CREATE) {
                viewModel.getProductData(productID)
            }
        }
        owner.lifecycle.addObserver(observer)
        onDispose {
            owner.lifecycle.removeObserver(observer)
        }
    }
}

@Composable
private fun SingleProductScreenContent(
    onSearchQueryChanged: (String) -> Unit,
    onFavoriteClicked: () -> Unit,
    onSearchResultClicked: (String) -> Unit,
    uiState: SingleProductScreenUIState,
    onBackClicked: () -> Unit,
    onProductClicked: (String) -> Unit,
    onAddToCartClicked: () -> Unit,
    onEmailChanged: (String) -> Unit,
    onTipoEntregaChanged: (Boolean) -> Unit,
    ontipoPagoChanged : (Boolean) -> Unit,
    onReporteInClicked: () -> Unit,
    onNavigateToCart: () -> Unit,
    onResetErrorCliente: ()-> Unit,
    onResetErrorCart: ()-> Unit
) {

    Column(
        Modifier
            .fillMaxSize() // Ocupará todo el tamaño disponible
            .padding(vertical = 16.dp) // Espaciado vertical de 16 dp
    ){
        ScreenHeader(
            searchQuery = uiState.searchQuery, // Pasar la consulta de búsqueda actual
            onSearchQueryChanged = onSearchQueryChanged, // Manejar cambios en la búsqueda
            onFavoriteClicked = onFavoriteClicked, // Manejar clic en favoritos
            onNavigateToCart = onNavigateToCart,
            onBackClicked = onBackClicked
        )
        // Se muestra la lista de resultados de búsqueda si hay una consulta activa
        AnimatedVisibility(
            visible = uiState.searchQuery.isNotEmpty(), // Visible solo si hay una consulta
            enter = fadeIn(), // Animación de entrada
            exit = fadeOut() // Animación de salida
        ) {
            // Lista vertical de resultados de búsqueda
            LazyColumn(
                modifier = Modifier.padding(horizontal = 8.dp, vertical = 8.dp) // Espaciado horizontal y vertical
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
        AnimatedVisibility(
            visible = uiState.isLoading,
            enter = fadeIn(),
            exit = fadeOut(tween(durationMillis = 20))
        ) {
            LoadingSection()
        }
        AnimatedVisibility(
            visible = !uiState.isLoading,
            enter = fadeIn(tween(delayMillis = 20)),
            exit = fadeOut()
        ) {
            LazyColumn(
                modifier = Modifier.fillMaxWidth()
            ) {
                item {
                    Row (
                        modifier = Modifier
                            .weight(1f)
                            .padding(start = 10.dp, end = 10.dp, top = 8.dp) // Aplica margen vertical y horizontal
                    ) {
                        Box(modifier = Modifier.weight(1f)) {

                            ProductSpecSection(
                                productInfo = uiState.productData.productInfo,
                                onProductClicked = onProductClicked,
                            )
                        }
                        Box(modifier = Modifier.weight(1f)) {
                            ClienteSpecSection(
                                clientInfo = uiState.clientData.clientInfo,
                                isButtonLoading = uiState.isLoading,
                                onReporteInClicked = onReporteInClicked
                            )
                        }
                    }
                }
                item {
                    Row(
                        modifier = Modifier
                            .weight(1f)
                            .padding(
                                start = 20.dp,
                                end = 20.dp,
                                top = 16.dp
                            ) // Aplica margen vertical y horizontal
                    ) {
                        Box(modifier = Modifier.weight(1f)) {
                            ClienteDataSection(
                                email = uiState.cliente,
                                tipoEntrega = uiState.tipoEntrega,
                                tipoPago = uiState.tipoPago,
                                isButtonLoading = uiState.isLoading,
                                onEmailChanged = onEmailChanged,
                                onTipoEntregaChanged = onTipoEntregaChanged,
                                ontipoPagoChanged =  ontipoPagoChanged,
                                onAddToCartClicked = onAddToCartClicked,
                                isAddButton = uiState.isAddButton,
                                isBtnLoading = uiState.isButtonLoading,
                                cartItems = uiState.productData.disponibles
                            )
                        }
                    }
                }
                item {
                    Spacer(
                        modifier = Modifier.height(4.dp)
                    )
                    TotalDisponibleSection(
                        cartItems = uiState.productData.disponibles
                    )
                }
            }
        }
        var showDialog by remember { mutableStateOf(false) }

        LaunchedEffect(uiState.isErrorCart) {
            if (uiState.isErrorCart) {
                showDialog = true
            }
        }

        ErrorCart(
            show = showDialog,
            onDismiss = {
                showDialog = false
                onResetErrorCart()
            },
            onConfirm = {
                showDialog = false
                onResetErrorCart()
            }
        )
        var showClientNull by remember { mutableStateOf(false) }

        LaunchedEffect(uiState.isErrorCliente) {
            if (uiState.isErrorCliente) {
                showClientNull = true
            }
        }

        ErrorClient(
            show = showClientNull,
            onDismiss = {
                showClientNull = false
                onResetErrorCliente()
            },
            onConfirm = {
                showClientNull = false
                onResetErrorCliente()
            }
        )
    }
}

@Composable
private fun ScreenHeader(
    // Consulta de búsqueda actual
    searchQuery: String,
    // Callback para manejar cambios en la consulta de búsqueda
    onSearchQueryChanged: (String) -> Unit,
    // Callback para manejar el clic en el botón de favoritos
    onFavoriteClicked: () -> Unit,
    onNavigateToCart: () -> Unit,
    onBackClicked: () -> Unit
) {
    // Columna que organiza los elementos verticalmente
    Column(
        Modifier.fillMaxWidth() // Ocupará todo el ancho disponible
    ) {
        // Fila que contiene la barra de búsqueda y el botón de favoritos
        Row(
            modifier = Modifier.padding(bottom = 10.dp, start = 7.dp, end = 7.dp), // Espaciado alrededor de la fila
            verticalAlignment = Alignment.CenterVertically // Alineación vertical al centro
        ) {
            IconButton(onClick = onBackClicked) {
                Icon(
                    painter = painterResource(id = R.drawable.back_icon),
                    contentDescription = null,
                    tint = Color.DarkGray
                )
            }
            // Campo de texto principal para la búsqueda
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
            // Botón para acceder a los favoritos
            IconButton(
                onClick = onFavoriteClicked // Callback al hacer clic
            ) {
                // Ícono para el botón de favoritos
                Icon(
                    painter = painterResource(id = R.drawable.home_icon), // Ícono de favoritos
                    contentDescription = "", // Descripción para accesibilidad (vacío aquí)
                    tint = Color.Unspecified // Color del ícono (sin tintado específico)
                )
            }
        }
        // Divisor para separar el encabezado del resto del contenido
        HorizontalDivider(
            color = MaterialTheme.colorScheme.onSurfaceVariant.copy(alpha = 0.2f) // Color del divisor con transparencia
        )
        Row (
            modifier = Modifier.padding(top = 5.dp,bottom = 5.dp, start = 15.dp, end = 7.dp), // Espaciado alrededor de la fila
            verticalAlignment = Alignment.CenterVertically
        ){
            Text(
                text = "Sucursal:",
                color = MaterialTheme.colorScheme.primary,
                style = MaterialTheme.typography.labelSmall,
                overflow = TextOverflow.Ellipsis,
            )
            Text(
                text =  " " + Constants.corredor,
                color = MaterialTheme.colorScheme.onBackground,
                style = MaterialTheme.typography.labelSmall,
                overflow = TextOverflow.Ellipsis,
                maxLines = 1
            )
        }
    }
}

@Composable
private fun ProductSpecSection(
    productInfo: SingleProductInfo,
    onProductClicked: (String) -> Unit,
) {
    Column(
        modifier = Modifier
            .size(width = 184.dp, height = 260.dp)
            .clip(RoundedCornerShape(5.dp))
            .border(
                width = 1.dp,
                color = MaterialTheme.colorScheme.outline,
                shape = RoundedCornerShape(5.dp)
            )
            .padding(start = 6.dp, top = 10.dp) // margen izquierdo de 8 dp y superior de 16 dp
            .clickable(
                interactionSource = remember { MutableInteractionSource() },
                indication = null
            ) {
                onProductClicked(productInfo.codigo)
            }
    ) {
        Box(
            modifier = Modifier.fillMaxWidth(),
            contentAlignment = Alignment.Center
        ) {
            NetworkImage(
                modifier = Modifier
                    .size(100.dp)
                    .clip(RoundedCornerShape(5.dp)),
                model = productInfo.codigo + ".jpg",
                contentScale = ContentScale.FillBounds
            )
        }

        Text(
            modifier = Modifier.padding(top = 4.dp, start = 4.dp), // margen izquierdo en el texto
            text = productInfo.codigo,
            color = MaterialTheme.colorScheme.onBackground,
            style = MaterialTheme.typography.labelSmall,
            overflow = TextOverflow.Ellipsis,
            maxLines = 2
        )

        Text(
            modifier = Modifier.padding(top = 4.dp, start = 4.dp), // margen izquierdo en el texto
            text = productInfo.description,
            color = MaterialTheme.colorScheme.onBackground,
            style = MaterialTheme.typography.labelSmall,
            overflow = TextOverflow.Ellipsis,
            maxLines = 3
        )
        Text(
            modifier = Modifier.padding(top = 4.dp, start = 4.dp), // margen izquierdo en el texto
            text = productInfo.proveedor,
            color = MaterialTheme.colorScheme.onBackground,
            style = MaterialTheme.typography.labelSmall,
            overflow = TextOverflow.Ellipsis,
            maxLines = 2
        )
        Row(
            modifier = Modifier.padding(top = 4.dp, start = 4.dp), // margen izquierdo en descuento
            verticalAlignment = Alignment.CenterVertically
        ) {
            Text(
                text = "Precio Caja:",
                color = MaterialTheme.colorScheme.primary,
                style = MaterialTheme.typography.labelSmall,
                overflow = TextOverflow.Ellipsis,
            )
            Text(
                text = "  " + "$"+productInfo.precioProducto.toString(),
                color = MaterialTheme.colorScheme.onBackground,
                style = MaterialTheme.typography.labelSmall,
                overflow = TextOverflow.Ellipsis
            )
        }
        Row(
            modifier = Modifier.padding(top = 4.dp, start = 4.dp), // margen izquierdo en descuento
            verticalAlignment = Alignment.CenterVertically
        ) {
            Text(
                text = "Precio M²:",
                color = MaterialTheme.colorScheme.primary,
                style = MaterialTheme.typography.labelSmall,
                overflow = TextOverflow.Ellipsis,
            )
            Text(
                text = "  " + "$"+productInfo.precioMetroProducto.toString(),
                color = MaterialTheme.colorScheme.onBackground,
                style = MaterialTheme.typography.labelSmall,
                overflow = TextOverflow.Ellipsis
            )
        }
    }
}

@Composable
private fun ClienteSpecSection(
    clientInfo: SingleProductClient,
    isButtonLoading: Boolean,
    onReporteInClicked: () -> Unit
) {
    Column(
        modifier = Modifier
            .size(width = 164.dp, height = 260.dp)
            .clip(RoundedCornerShape(5.dp))
            .border(
                width = 1.dp,
                color = MaterialTheme.colorScheme.outline,
                shape = RoundedCornerShape(5.dp)
            )
            .padding(start = 6.dp, top = 10.dp) // margen izquierdo de 8 dp y superior de 16 dp
    ) {
        Row(
            modifier = Modifier.padding(top = 4.dp, start = 4.dp , end = 2.dp)
        ) {
            Text(
                text = "Nombre:",
                color = MaterialTheme.colorScheme.primary,
                style = MaterialTheme.typography.labelSmall,
                overflow = TextOverflow.Ellipsis,
            )
            Text(
                text =  " " + clientInfo.nombreCliente,
                color = MaterialTheme.colorScheme.onBackground,
                style = MaterialTheme.typography.labelSmall,
                overflow = TextOverflow.Ellipsis,
                maxLines = 6
            )
        }
        Row(
            modifier = Modifier.padding(top = 4.dp, start = 4.dp)
        ) {
            Text(
                text = "Tipo de Cliente:",
                color = MaterialTheme.colorScheme.primary,
                style = MaterialTheme.typography.labelSmall,
                overflow = TextOverflow.Ellipsis,
            )
            Text(
                text = " "+clientInfo.clasificacion,
                color = MaterialTheme.colorScheme.onBackground,
                style = MaterialTheme.typography.labelSmall,
                overflow = TextOverflow.Ellipsis,
                maxLines = 1
            )
        }
        Box(
            modifier = Modifier.fillMaxSize(),
            contentAlignment = Alignment.TopStart
        ) {
            Column {
               Row(
                   modifier = Modifier.padding(top = 4.dp, start = 4.dp), // margen izquierdo en descuento
                   verticalAlignment = Alignment.CenterVertically
               ) {
                   Text(
                       text = "Recoge:",
                       color = MaterialTheme.colorScheme.primary,
                       style = MaterialTheme.typography.labelSmall,
                       overflow = TextOverflow.Ellipsis,
                   )
                   Text(
                       text = "  " + clientInfo.descuentoRecoje + "%",
                       color = MaterialTheme.colorScheme.secondary,
                       style = MaterialTheme.typography.labelSmall,
                       overflow = TextOverflow.Ellipsis
                   )
               }
                Row(
                    modifier = Modifier.padding(top = 4.dp, start = 4.dp), // margen izquierdo en descuento
                    verticalAlignment = Alignment.CenterVertically
                ) {
                    Text(
                        text = "Contado:",
                        color = MaterialTheme.colorScheme.primary,
                        style = MaterialTheme.typography.labelSmall,
                        overflow = TextOverflow.Ellipsis,
                    )
                    Text(
                        text = "  " + clientInfo.descuentoContado + "%",
                        color = MaterialTheme.colorScheme.secondary,
                        style = MaterialTheme.typography.labelSmall,
                        overflow = TextOverflow.Ellipsis
                    )
                }
                Row(
                    modifier = Modifier.padding(top = 4.dp, start = 4.dp), // margen izquierdo en descuento
                    verticalAlignment = Alignment.CenterVertically
                ) {
                    Text(
                        text = "Precio Caja:",
                        color = MaterialTheme.colorScheme.primary,
                        style = MaterialTheme.typography.labelSmall,
                        overflow = TextOverflow.Ellipsis,
                    )
                    Text(
                        text = "  " + "$ "+clientInfo.precioFinal.toString(),
                        color = MaterialTheme.colorScheme.onBackground,
                        style = MaterialTheme.typography.labelSmall,
                        overflow = TextOverflow.Ellipsis
                    )
                }
                Row(
                    modifier = Modifier.padding(top = 4.dp, start = 4.dp), // margen izquierdo en descuento
                    verticalAlignment = Alignment.CenterVertically
                ) {
                    Text(
                        text = "Precio M²:",
                        color = MaterialTheme.colorScheme.primary,
                        style = MaterialTheme.typography.labelSmall,
                        overflow = TextOverflow.Ellipsis,
                    )
                    Text(
                        text = "  " + "$ "+clientInfo.precioMetroFinal.toString(),
                        color = MaterialTheme.colorScheme.onBackground,
                        style = MaterialTheme.typography.labelSmall,
                        overflow = TextOverflow.Ellipsis
                    )
                }
                Row(
                    modifier = Modifier.padding(top = 4.dp, start = 4.dp), // margen izquierdo en descuento
                    verticalAlignment = Alignment.CenterVertically
                ) {
                    Text(
                        text = "Actual Imp: ",
                        color = MaterialTheme.colorScheme.primary,
                        style = MaterialTheme.typography.labelSmall,
                        overflow = TextOverflow.Ellipsis,
                    )
                    Text(
                        text = "  " + "%.1f".format(clientInfo.actualimporte),
                        color = MaterialTheme.colorScheme.onBackground,
                        style = MaterialTheme.typography.labelSmall,
                        overflow = TextOverflow.Ellipsis
                    )
                }
                Row(
                    modifier = Modifier.padding(top = 4.dp, start = 4.dp), // margen izquierdo en descuento
                    verticalAlignment = Alignment.CenterVertically
                ) {
                    Text(
                        text = "Actuales Mt: ",
                        color = MaterialTheme.colorScheme.primary,
                        style = MaterialTheme.typography.labelSmall,
                        overflow = TextOverflow.Ellipsis,
                    )
                    Text(
                        text = "  " + "%.1f".format(clientInfo.actualmetros),
                        color = MaterialTheme.colorScheme.onBackground,
                        style = MaterialTheme.typography.labelSmall,
                        overflow = TextOverflow.Ellipsis
                    )
                }
                Box(
                    modifier = Modifier.fillMaxSize(),
                    contentAlignment = Alignment.TopStart
                ) {
                    Column {
                        MainButton(
                            modifier = Modifier
                                .fillMaxWidth()
                                .height(38.dp),
                            text = stringResource(R.string.reporteCliente),
                            isLoading = isButtonLoading,
                            isEnabled = !isButtonLoading && Constants.cliente.isNotEmpty(),
                            onClick = onReporteInClicked
                        )
                    }
                }
            }
        }
    }
}

@Composable
private fun ClienteDataSection(
    email: String,
    tipoEntrega : Boolean,
    tipoPago : Boolean,
    onEmailChanged: (String) -> Unit,
    onTipoEntregaChanged : (Boolean) -> Unit,
    ontipoPagoChanged : (Boolean) -> Unit,
    isButtonLoading: Boolean,
    isAddButton: Boolean,
    isBtnLoading: Boolean,
    onAddToCartClicked: () -> Unit,
    cartItems: List<CorredorItem>
) {
    Column(
        modifier = Modifier
            .fillMaxWidth()
            .padding(top = 4.dp)
    ) {
        MainTextFieldNumber(
            modifier = Modifier
                .fillMaxWidth()
                .height(58.dp),
            value = email,
            onValueChanged = onEmailChanged,
            placeHolder = stringResource(R.string.your_cliente),
            leadingIcon = R.drawable.person_icon,
            enabled = true
        )
        Spacer(
            modifier = Modifier.height(4.dp)
        )
        Row(
            modifier = Modifier
                .fillMaxWidth()
                .height(58.dp)
                .padding(horizontal = 8.dp),
            verticalAlignment = Alignment.CenterVertically
        ) {
            Icon(
                painter = painterResource(id = R.drawable.location_icon),
                contentDescription = null,
                modifier = Modifier.padding(end = 8.dp) // Espaciado entre ícono y texto
            )
            Text(
                text = " " + stringResource(R.string.your_entrega),
                modifier = Modifier.weight(1f), // Ocupa 50% del ancho disponible
                color = MaterialTheme.colorScheme.onSurfaceVariant
            )
            Checkbox(
                checked = tipoEntrega,
                onCheckedChange = { checked ->
                    if (email.isNotEmpty()) {
                        onTipoEntregaChanged(checked)
                    }
                },
                modifier = Modifier.weight(1f), // Ocupa 50% del ancho disponible
                enabled = email.isNotEmpty()
            )
            Spacer(
                modifier = Modifier.height(4.dp)
            )
            Icon(
                painter = painterResource(id = R.drawable.bottom_bar_cart_ic),
                contentDescription = null,
                modifier = Modifier.padding(end = 8.dp) // Espaciado entre ícono y texto
            )
            Text(
                text = " " + stringResource(R.string.your_pago),
                modifier = Modifier.weight(1f), // Ocupa 50% del ancho disponible
                color = MaterialTheme.colorScheme.onSurfaceVariant
            )
            Checkbox(
                checked = tipoPago,
                onCheckedChange = { checked ->
                    if (email.isNotEmpty()) {
                        ontipoPagoChanged(checked)
                    }
                },
                modifier = Modifier.weight(1f), // Ocupa 50% del ancho disponible
                enabled = email.isNotEmpty()
            )
        }
        Spacer(
            modifier = Modifier.height(4.dp)
        )
            AnimatedVisibility(
                visible = isAddButton,
                enter = fadeIn(tween(durationMillis = 500)),
                exit = fadeOut(tween(durationMillis = 250))
            ) {
                MainButton(
                    modifier = Modifier
                        .fillMaxWidth()
                        .height(58.dp),
                    text = stringResource(R.string.add_to_cart),
                    onClick = onAddToCartClicked,
                    //isEnabled = !isBtnLoading,
                    isEnabled = !isButtonLoading && Constants.cliente.isNotEmpty() && cartItems.isNotEmpty(),
                    isLoading = isBtnLoading
                )
            }
    }
}

@Composable
private fun TotalDisponibleSection(
    cartItems: List<CorredorItem>
) {
    Column(
        Modifier
            .clip(RoundedCornerShape(5.dp))
            .border(
                width = 1.dp,
                color = MaterialTheme.colorScheme.onSurfaceVariant.copy(alpha = 0.2f),
                shape = RoundedCornerShape(5.dp)
            )
            .padding(16.dp)
    ) {
        // Header row for columns
        // Iterate through each item and display its data
        if (cartItems.isEmpty()) {
            Text(
                text = "No contamos con artículos disponibles en esta sucursal",
                style = MaterialTheme.typography.labelSmall,
                color = MaterialTheme.colorScheme.secondary,
                modifier = Modifier
                    .fillMaxWidth()
            )
        } else {
            Row(
                modifier = Modifier
                    .fillMaxWidth()
                    .padding(bottom = 12.dp)
                    .border(
                        width = 1.dp,
                        color = MaterialTheme.colorScheme.onSurfaceVariant.copy(alpha = 0.2f)
                    ),
                horizontalArrangement = Arrangement.SpaceBetween,
                verticalAlignment = Alignment.CenterVertically
            ) {
                Text(
                    text = "Clave SAP",
                    style = MaterialTheme.typography.labelSmall,
                    modifier = Modifier
                        .weight(1f)
                        .padding(8.dp),
                    color = MaterialTheme.colorScheme.primary
                )
                Text(
                    text = "Nombre Sucursal",
                    style = MaterialTheme.typography.labelSmall,
                    modifier = Modifier
                        .weight(1f)
                        .padding(8.dp),
                    color = MaterialTheme.colorScheme.primary
                )
                Text(
                    text = "Stock Disponible",
                    style = MaterialTheme.typography.labelSmall,
                    modifier = Modifier
                        .weight(1f)
                        .padding(8.dp),
                    color = MaterialTheme.colorScheme.primary
                )
            }

            cartItems.forEach { item ->
                Row(
                    modifier = Modifier
                        .fillMaxWidth()
                        .border(
                            width = 1.dp,
                            color = MaterialTheme.colorScheme.onSurfaceVariant.copy(alpha = 0.2f)
                        ),
                    horizontalArrangement = Arrangement.SpaceBetween,
                    verticalAlignment = Alignment.CenterVertically
                ) {
                    Text(
                        text = item.claveSAP, // Assuming `CorredorItem` has `claveSAP` property
                        style = MaterialTheme.typography.bodySmall,
                        modifier = Modifier
                            .weight(1f)
                            .padding(8.dp),
                        color = MaterialTheme.colorScheme.onSurfaceVariant
                    )
                    Text(
                        text = item.nombreSucursal, // Assuming `CorredorItem` has `nombreSucursal` property
                        style = MaterialTheme.typography.bodySmall,
                        modifier = Modifier
                            .weight(1f)
                            .padding(8.dp),
                        color = MaterialTheme.colorScheme.onSurfaceVariant
                    )
                    Text(
                        text = item.stockDispo.toString(), // Assuming `CorredorItem` has `stockDispo` property
                        style = MaterialTheme.typography.bodySmall,
                        modifier = Modifier
                            .weight(1f)
                            .padding(8.dp),
                        color = MaterialTheme.colorScheme.onSurfaceVariant
                    )
                }
            }
        }
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
fun ErrorCart(
    show: Boolean, // Estado que controla la visibilidad del diálogo
    onDismiss: () -> Unit, // Función para cerrar el diálogo
    onConfirm: () -> Unit // Función para confirmar
) {
    if (show) {
        AlertDialog(
            onDismissRequest = { onDismiss() }, // Se llama cuando se hace clic fuera del diálogo
            confirmButton = {
                TextButton(onClick = { onConfirm() }) {
                    Text(text = "Confirmar")
                }
            },
            title = { Text(text = "Error en Carro") },
            text = { Text(text = "Posiblemente, Modificaste el cliente, solo puedes crear un carrito para un mismo cliente " +
                    " O Verifica el tipo de Entrega y Tipo de Pago Deben de ser los mismos con que iniciaste el carrito") },
        )
    }
}
@Composable
fun ErrorClient(
    show: Boolean,
    onDismiss: () -> Unit,
    onConfirm: () -> Unit
) {
    if (show) {
        AlertDialog(
            onDismissRequest = { onDismiss() },
            confirmButton = {
                TextButton(onClick = { onConfirm() }) {
                    Text(text = "Confirmar")
                }
            },
            title = { Text(text = "Cliente no Existe") },
            text = { Text(text = "Verifica la clave del Cliente o posiblemente superaste el límite de búsqueda por día de clientes " +
                    "Solo puedes buscar 8 Clientes por día") },
        )
    }
}


@Composable
private fun LoadingSection() {
    Column(
        Modifier.padding(horizontal = 16.dp, vertical = 26.dp)
    ) {
        //image
        Spacer(
            modifier = Modifier
                .fillMaxWidth()
                .height(238.dp)
                .clip(RoundedCornerShape(5.dp))
                .background(brush = shimmerBrush())
        )
        Spacer(modifier = Modifier.height(16.dp))

        //title
        Spacer(
            modifier = Modifier
                .size(
                    width = 240.dp,
                    height = 15.dp
                )
                .clip(RoundedCornerShape(5.dp))
                .background(brush = shimmerBrush())
        )
        Spacer(modifier = Modifier.height(8.dp))

        //brand
        Spacer(
            modifier = Modifier
                .size(
                    width = 80.dp,
                    height = 15.dp
                )
                .clip(RoundedCornerShape(5.dp))
                .background(brush = shimmerBrush())
        )
        Spacer(modifier = Modifier.height(24.dp))

        //spec
        Spacer(
            modifier = Modifier
                .size(
                    width = 150.dp,
                    height = 15.dp
                )
                .clip(RoundedCornerShape(5.dp))
                .background(brush = shimmerBrush())
        )
        Spacer(modifier = Modifier.height(8.dp))

        //spec details
        for (i in 0..5) {
            Spacer(
                modifier = Modifier
                    .size(
                        width = 300.dp,
                        height = 15.dp
                    )
                    .clip(RoundedCornerShape(5.dp))
                    .background(brush = shimmerBrush())
            )
            Spacer(modifier = Modifier.height(8.dp))
        }

        //review
        Spacer(modifier = Modifier.height(24.dp))
        Spacer(
            modifier = Modifier
                .size(
                    width = 150.dp,
                    height = 15.dp
                )
                .clip(RoundedCornerShape(5.dp))
                .background(brush = shimmerBrush())
        )
        Spacer(modifier = Modifier.height(8.dp))
        //review details
        Spacer(
            modifier = Modifier
                .size(
                    width = 2500.dp,
                    height = 15.dp
                )
                .clip(RoundedCornerShape(5.dp))
                .background(brush = shimmerBrush())
        )
        Spacer(modifier = Modifier.height(8.dp))
        Spacer(
            modifier = Modifier
                .size(
                    width = 200.dp,
                    height = 15.dp
                )
                .clip(RoundedCornerShape(5.dp))
                .background(brush = shimmerBrush())
        )
        Spacer(modifier = Modifier.height(8.dp))
        Spacer(
            modifier = Modifier
                .size(
                    width = 300.dp,
                    height = 15.dp
                )
                .clip(RoundedCornerShape(5.dp))
                .background(brush = shimmerBrush())
        )
        Spacer(modifier = Modifier.height(8.dp))
        Spacer(
            modifier = Modifier
                .size(
                    width = 120.dp,
                    height = 15.dp
                )
                .clip(RoundedCornerShape(5.dp))
                .background(brush = shimmerBrush())
        )
    }
}

