package com.app.sanimex.presentation.screens.cart

import android.annotation.SuppressLint
import androidx.compose.animation.AnimatedVisibility
import androidx.compose.animation.core.tween
import androidx.compose.animation.fadeIn
import androidx.compose.animation.fadeOut
import androidx.compose.foundation.background
import androidx.compose.foundation.border
import androidx.compose.foundation.clickable
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
import androidx.compose.foundation.layout.width
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material3.Card
import androidx.compose.material3.CardDefaults
import androidx.compose.material3.Icon
import androidx.compose.material3.IconButton
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Scaffold
import androidx.compose.material3.Surface
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.layout.ContentScale
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.text.style.TextOverflow
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import androidx.hilt.navigation.compose.hiltViewModel
import com.airbnb.lottie.compose.LottieAnimation
import com.airbnb.lottie.compose.LottieCompositionSpec
import com.airbnb.lottie.compose.LottieConstants
import com.airbnb.lottie.compose.rememberLottieComposition
import com.app.sanimex.R
import com.app.sanimex.core.util.Constants
import com.app.sanimex.core.util.calcItemsPriceF
import com.app.sanimex.domain.model.CarritoFinal
import com.app.sanimex.presentation.components.BottomNavigationBar
import com.app.sanimex.presentation.components.DottedShape
import com.app.sanimex.presentation.components.MainButton
import com.app.sanimex.presentation.components.NetworkImage
import com.app.sanimex.presentation.components.shimmerBrush

@SuppressLint("UnusedMaterial3ScaffoldPaddingParameter")
@Composable
fun CartScreen(
    viewModel: CartViewModel = hiltViewModel(),
    onNavigateToExplore: () -> Unit = {},
    onNavigateToAccount: () -> Unit = {},
    onNavigateToUbicacion: () -> Unit = {},
    onNavigationBack: () -> Unit = {},
    onNavigateToTicket: () -> Unit = {},
    onNavigateToTicketGerente: () -> Unit = {},

) {
    val uiState by viewModel.uiState.collectAsState()
    Scaffold(
        bottomBar = {
            BottomNavigationBar(
                selectedItem = 1,
                onNavigateToExplore = onNavigateToExplore,
                onNavigateToAccount = onNavigateToAccount,
                onNavigateToUbicacion = onNavigateToUbicacion,
                onNavigateToTicket = onNavigateToTicket,
                onNavigateToTicketGerente = onNavigateToTicketGerente
            )
        }
    ) {
        CartScreenContent(
            uiState = uiState,
            onDeleteClicked = viewModel::deleteFromCart,
            onSaveClicked = viewModel::saveCart,
            onIncreaseQuantity = { id ->
                viewModel.onChangeQuantity(
                    productID = id,
                    isIncrease = true
                )
            },
            onDecreaseQuantity = { id ->
                viewModel.onChangeQuantity(
                    productID = id,
                    isIncrease = false
                )
            },
            onBackToHomeClicked = onNavigateToExplore,
            onBackClicked = onNavigationBack,
            onIncreaseQuantityTen = { id ->
                viewModel.onChangeQuantityTen(
                    productID = id,
                    isIncrease = true
                )
            },
            onDecreaseQuantityTen = { id ->
                viewModel.onChangeQuantityTen(
                    productID = id,
                    isIncrease = false
                )
            },
        )
    }
    LaunchedEffect(key1 = uiState.guardado) {
        if (Constants.idRol == 10 || Constants.idRol == 8 || Constants.idRol == 1) {
            if (uiState.guardado) {
                // Call your function here when 'guardado' becomes true
                onNavigateToTicketGerente()
            }
        }else{
            if (uiState.guardado) {
                // Call your function here when 'guardado' becomes true
                onNavigateToTicket()
            }
        }
    }
}

@Composable
private fun CartScreenContent(
    uiState: CartScreenUIState,
    onDeleteClicked: (String) -> Unit,
    onSaveClicked: () -> Unit,
    onIncreaseQuantity: (String) -> Unit,
    onDecreaseQuantity: (String) -> Unit,
    onIncreaseQuantityTen: (String) -> Unit,
    onDecreaseQuantityTen: (String) -> Unit,
    onBackToHomeClicked: () -> Unit,
    onBackClicked: () -> Unit
) {
    Surface(
        modifier = Modifier.fillMaxSize(),
        color = MaterialTheme.colorScheme.background
    ) {
        Column(
            modifier = Modifier
                .fillMaxSize()
                .padding(top = 16.dp, bottom = 80.dp)
        ) {
            ScreenHeader(onBackClicked = onBackClicked)
            
            AnimatedVisibility(
                visible = uiState.isLoading,
                enter = fadeIn(),
                exit = fadeOut(animationSpec = tween(durationMillis = 500))
            ) {
                LoadingState()
            }

            AnimatedVisibility(
                visible = uiState.cartItems.isEmpty() && !uiState.isLoading,
                enter = fadeIn(animationSpec = tween(delayMillis = 500)),
                exit = fadeOut()
            ) {
                EmptyState(
                    onBackToExploreClicked = onBackToHomeClicked
                )
            }

            AnimatedVisibility(
                visible = !uiState.isLoading && uiState.cartItems.isNotEmpty(),
                enter = fadeIn(animationSpec = tween(delayMillis = 500)),
                exit = fadeOut()
            ) {
                Box(
                    modifier = Modifier.fillMaxSize()
                ) {
                    LazyColumn(
                        modifier = Modifier
                            .fillMaxSize()
                            .padding(horizontal = 16.dp)
                    ) {
                        item { 
                            Spacer(modifier = Modifier.height(16.dp))
                            InfoEntregaSection(cartItems = uiState.cartItems)
                            Spacer(modifier = Modifier.height(16.dp))
                        }
                        
                        item {
                            Text(
                                text = "Productos (${uiState.cartItems.size})",
                                style = MaterialTheme.typography.titleMedium,
                                color = MaterialTheme.colorScheme.onBackground,
                                modifier = Modifier.padding(vertical = 8.dp)
                            )
                        }

                        item {
                            ScreenBody(
                                cartItems = uiState.cartItems,
                                onDeleteClicked = onDeleteClicked,
                                onIncreaseQuantity = onIncreaseQuantity,
                                onDecreaseQuantity = onDecreaseQuantity,
                                onIncreaseQuantityTen = onIncreaseQuantityTen,
                                onDecreaseQuantityTen = onDecreaseQuantityTen
                            )
                        }

                        item { 
                            Spacer(modifier = Modifier.height(16.dp))
                            TotalPriceSection(cartItems = uiState.cartItems)
                            Spacer(modifier = Modifier.height(80.dp))
                        }
                    }

                    // Botón flotante de cotizar
                    Box(
                        modifier = Modifier
                            .align(Alignment.BottomCenter)
                            .fillMaxWidth()
                            .background(
                                MaterialTheme.colorScheme.background.copy(alpha = 0.95f)
                            )
                            .padding(16.dp)
                    ) {
                        MainButton(
                            modifier = Modifier
                                .fillMaxWidth()
                                .height(56.dp),
                            text = stringResource(R.string.cotizar),
                            onClick = onSaveClicked
                        )
                    }
                }
            }
        }
    }
}

@Composable
private fun ScreenHeader(
    onBackClicked: () -> Unit
) {
    Row(
        modifier = Modifier
            .fillMaxWidth()
            .padding(horizontal = 4.dp),
        horizontalArrangement = Arrangement.SpaceBetween,
        verticalAlignment = Alignment.CenterVertically
    ) {
        Row(
            verticalAlignment = Alignment.CenterVertically
        ) {
            IconButton(
                onClick = onBackClicked
            ) {
                Icon(
                    painter = painterResource(id = R.drawable.back_icon),
                    contentDescription = "",
                    tint = Color.Unspecified
                )
            }
            Text(
                modifier = Modifier.padding(horizontal = 16.dp),
                text = stringResource(R.string.your_venta),
                color = MaterialTheme.colorScheme.onBackground,
                style = MaterialTheme.typography.titleMedium
            )
        }
    }
}

@Composable
private fun ScreenBody(
    cartItems: List<CarritoFinal>,
    onDeleteClicked: (String) -> Unit,
    onIncreaseQuantity: (String) -> Unit,
    onDecreaseQuantity: (String) -> Unit,
    onIncreaseQuantityTen: (String) -> Unit,
    onDecreaseQuantityTen: (String) -> Unit,
) {
    Column {
        for (item in cartItems) {
            CartItemDesign(
                cartItem = item,
                onDeleteClicked = onDeleteClicked,
                onIncreaseQuantity = onIncreaseQuantity,
                onDecreaseQuantity = onDecreaseQuantity,
                onIncreaseQuantityTen = onIncreaseQuantityTen,
                onDecreaseQuantityTen = onDecreaseQuantityTen
            )
            Spacer(modifier = Modifier.height(16.dp))
        }
    }
}

@Composable
private fun CartItemDesign(
    cartItem: CarritoFinal,
    onDeleteClicked: (String) -> Unit,
    onIncreaseQuantity: (String) -> Unit,
    onDecreaseQuantity: (String) -> Unit,
    onIncreaseQuantityTen: (String) -> Unit,
    onDecreaseQuantityTen: (String) -> Unit
) {
    Card(
        modifier = Modifier
            .fillMaxWidth()
            .padding(vertical = 8.dp),
        shape = RoundedCornerShape(12.dp),
        colors = CardDefaults.cardColors(
            containerColor = MaterialTheme.colorScheme.primaryContainer
        )
    ) {
        Row(
            modifier = Modifier
                .fillMaxWidth()
                .padding(8.dp),
            verticalAlignment = Alignment.Top
        ) {
            NetworkImage(
                model = cartItem.image,
                modifier = Modifier
                    .size(70.dp)
                    .clip(RoundedCornerShape(8.dp)),
                contentScale = ContentScale.Crop
            )
            
            Column(
                modifier = Modifier
                    .weight(1f)
                    .padding(start = 8.dp),
                verticalArrangement = Arrangement.spacedBy(8.dp)
            ) {
                Row(
                    modifier = Modifier.fillMaxWidth(),
                    horizontalArrangement = Arrangement.SpaceBetween,
                    verticalAlignment = Alignment.Top
                ) {
                    Text(
                        text = cartItem.product,
                        style = MaterialTheme.typography.labelMedium,
                        color = MaterialTheme.colorScheme.onSurfaceVariant,
                        maxLines = 3,
                        overflow = TextOverflow.Ellipsis,
                        modifier = Modifier.weight(1f)
                    )
                    
                    IconButton(
                        onClick = { onDeleteClicked(cartItem.id) },
                        modifier = Modifier.size(24.dp)
                    ) {
                        Icon(
                            painter = painterResource(id = R.drawable.delete_icon),
                            contentDescription = "Eliminar",
                            tint = MaterialTheme.colorScheme.error
                        )
                    }
                }

                Text(
                    text = "$${"%.2f".format(cartItem.discount?.takeIf { it != 0 } ?: cartItem.price)}",
                    style = MaterialTheme.typography.labelMedium,
                    color = MaterialTheme.colorScheme.primary
                )

                QuantitySelector(
                    quantity = cartItem.quantity,
                    onIncrease = { onIncreaseQuantity(cartItem.id) },
                    onDecrease = { onDecreaseQuantity(cartItem.id) },
                    onIncreaseTen = { onIncreaseQuantityTen(cartItem.id) },
                    onDecreaseTen = { onDecreaseQuantityTen(cartItem.id) }
                )
            }
        }
    }
}

@Composable
private fun QuantitySelector(
    quantity: Int,
    onIncrease: () -> Unit,
    onDecrease: () -> Unit,
    onIncreaseTen: () -> Unit,
    onDecreaseTen: () -> Unit,
) {
    Row(
        modifier = Modifier
            .clip(RoundedCornerShape(8.dp))
            .border(
                width = 1.dp,
                color = MaterialTheme.colorScheme.outline.copy(alpha = 0.5f),
                shape = RoundedCornerShape(8.dp)
            ),
        verticalAlignment = Alignment.CenterVertically
    ) {
        QuantityButtonTen(
            text = "10",
            onClick = onDecreaseTen
        )
        QuantityButton(
            text = "-",
            onClick = onDecrease
        )
        Text(
            text = quantity.toString(),
            modifier = Modifier
                .padding(horizontal = 16.dp),
            style = MaterialTheme.typography.titleMedium
        )
        QuantityButton(
            text = "+",
            onClick = onIncrease
        )
        QuantityButtonTen(
            text = "10",
            onClick = onIncreaseTen
        )
    }
}

@Composable
private fun QuantityButton(
    text: String,
    onClick: () -> Unit,
    isSecondary: Boolean = false
) {
    Box(
        modifier = Modifier
            .clickable(onClick = onClick)
            .background(
                if (isSecondary)
                    MaterialTheme.colorScheme.surfaceVariant
                else
                    MaterialTheme.colorScheme.primary
            )
            .padding(horizontal = 12.dp, vertical = 8.dp),
        contentAlignment = Alignment.Center
    ) {
        Text(
            text = text,
            color = if (isSecondary)
                MaterialTheme.colorScheme.onSurfaceVariant
            else
                MaterialTheme.colorScheme.onPrimary,
            style = MaterialTheme.typography.labelMedium
        )
    }
}

@Composable
private fun QuantityButtonTen(
    text: String,
    onClick: () -> Unit,
    isSecondary: Boolean = false
) {
    Box(
        modifier = Modifier
            .clickable(onClick = onClick)
            .background(
                if (isSecondary)
                    MaterialTheme.colorScheme.surfaceVariant
                else
                    MaterialTheme.colorScheme.onSurfaceVariant
            )
            .padding(horizontal = 12.dp, vertical = 8.dp),
        contentAlignment = Alignment.Center
    ) {
        Text(
            text = text,
            color = if (isSecondary)
                MaterialTheme.colorScheme.onSurfaceVariant
            else
                MaterialTheme.colorScheme.onPrimary,
            style = MaterialTheme.typography.labelMedium
        )
    }
}

@Composable
private fun InfoEntregaSection(
    cartItems: List<CarritoFinal>
) {
    Column(
        Modifier
            .clip(RoundedCornerShape(5.dp))
            .border(
                width = 1.dp,
                color = MaterialTheme.colorScheme.onSurfaceVariant.copy(alpha = 0.2f),
                shape = RoundedCornerShape(5.dp)
            )
            .padding(16.dp),
    ) {
        Row(
            modifier = Modifier
                .fillMaxWidth()
                .padding(bottom = 12.dp),
            horizontalArrangement = Arrangement.SpaceBetween,
            verticalAlignment = Alignment.CenterVertically
        ) {
            val cliente = cartItems.getOrNull(0)?.cliente ?: "Desconocido"
            Text(
                text = "Cliente ($cliente)",
                color = MaterialTheme.colorScheme.onSurfaceVariant,
                style = MaterialTheme.typography.labelSmall
            )
        }

        Box(
            Modifier
                .height(1.dp)
                .fillMaxWidth()
                .background(Color.Gray, shape = DottedShape(step = 10.dp))
        )

        Row(
            modifier = Modifier
                .fillMaxWidth()
                .padding(top = 12.dp),
            horizontalArrangement = Arrangement.SpaceBetween,
            verticalAlignment = Alignment.CenterVertically
        ) {
            Text(
                text = "Recoge: ",
                color = MaterialTheme.colorScheme.onBackground,
                style = MaterialTheme.typography.labelSmall
            )
            Text(
                text = cartItems.firstOrNull()?.recoge?.let { if (it) "Sí" else "No" } ?: "No disponible",
                color = MaterialTheme.colorScheme.primary,
                style = MaterialTheme.typography.labelSmall
            )
            Text(
                text = "Contado: ",
                color = MaterialTheme.colorScheme.onBackground,
                style = MaterialTheme.typography.labelSmall
            )
            Text(
                text = cartItems.firstOrNull()?.contado?.let { if (it) "Sí" else "No" } ?: "No disponible",
                color = MaterialTheme.colorScheme.primary,
                style = MaterialTheme.typography.labelSmall
            )
        }
    }
}

@Composable
private fun TotalPriceSection(
    cartItems: List<CarritoFinal>
) {
    Column(
        Modifier
            .clip(RoundedCornerShape(5.dp))
            .border(
                width = 1.dp,
                color = MaterialTheme.colorScheme.onSurfaceVariant.copy(alpha = 0.2f),
                shape = RoundedCornerShape(5.dp)
            )
            .padding(16.dp),
    ) {
        Row(
            modifier = Modifier
                .fillMaxWidth()
                .padding(bottom = 12.dp),
            horizontalArrangement = Arrangement.SpaceBetween,
            verticalAlignment = Alignment.CenterVertically
        ) {
            Text(
                text = "Productos (${cartItems.size})",
                color = MaterialTheme.colorScheme.onSurfaceVariant,
                style = MaterialTheme.typography.bodySmall
            )
            Text(
                text = "$${"%.1f".format(cartItems.calcItemsPriceF())}",
                color = MaterialTheme.colorScheme.onBackground,
                style = MaterialTheme.typography.bodySmall
            )
        }

        Box(
            Modifier
                .height(1.dp)
                .fillMaxWidth()
                .background(Color.Gray, shape = DottedShape(step = 10.dp))
        )

        Row(
            modifier = Modifier
                .fillMaxWidth()
                .padding(top = 12.dp),
            horizontalArrangement = Arrangement.SpaceBetween,
            verticalAlignment = Alignment.CenterVertically
        ) {
            Text(
                text = "Total + IVA",
                color = MaterialTheme.colorScheme.onBackground,
                style = MaterialTheme.typography.labelSmall
            )
            Text(
                text = "$${"%.1f".format(cartItems.calcItemsPriceF())}",
                color = MaterialTheme.colorScheme.primary,
                style = MaterialTheme.typography.labelSmall
            )
        }
    }
}

@Composable
private fun LoadingState() {
    LazyColumn(
        modifier = Modifier
            .fillMaxWidth()
            .padding(16.dp),
        verticalArrangement = Arrangement.spacedBy(16.dp)
    ) {
        items(count = 3) {
            Row(
                modifier = Modifier
                    .fillMaxWidth()
                    .height(104.dp)
                    .clip(RoundedCornerShape(5.dp))
                    .border(
                        width = 1.dp,
                        color = MaterialTheme.colorScheme.onSurfaceVariant.copy(alpha = 0.2f),
                        shape = RoundedCornerShape(5.dp)
                    )
                    .padding(16.dp),
                verticalAlignment = Alignment.Top
            ) {
                Spacer(
                    modifier = Modifier
                        .size(84.dp)
                        .background(brush = shimmerBrush(), shape = RoundedCornerShape(5.dp))
                )
                Column(
                    Modifier.padding(start = 12.dp, top = 4.dp)
                ) {
                    Spacer(
                        modifier = Modifier
                            .width(150.dp)
                            .height(10.dp)
                            .background(brush = shimmerBrush(), shape = RoundedCornerShape(5.dp))
                    )
                    Spacer(modifier = Modifier.height(8.dp))
                    Spacer(
                        modifier = Modifier
                            .width(100.dp)
                            .height(10.dp)
                            .background(brush = shimmerBrush(), shape = RoundedCornerShape(5.dp))
                    )
                }
            }
        }
        item {
            Column(
                Modifier
                    .clip(RoundedCornerShape(5.dp))
                    .border(
                        width = 1.dp,
                        color = MaterialTheme.colorScheme.onSurfaceVariant.copy(alpha = 0.2f),
                        shape = RoundedCornerShape(5.dp)
                    )
                    .padding(16.dp),
            ) {
                Row(
                    modifier = Modifier
                        .fillMaxWidth()
                        .padding(bottom = 12.dp),
                    horizontalArrangement = Arrangement.SpaceBetween,
                    verticalAlignment = Alignment.CenterVertically
                ) {
                    Spacer(
                        modifier = Modifier
                            .size(width = 100.dp, height = 10.dp)
                            .background(
                                brush = shimmerBrush(),
                                shape = RoundedCornerShape(5.dp)
                            )
                    )
                    Spacer(
                        modifier = Modifier
                            .size(width = 50.dp, height = 10.dp)
                            .background(
                                brush = shimmerBrush(),
                                shape = RoundedCornerShape(5.dp)
                            )
                    )
                }

                Row(
                    modifier = Modifier
                        .fillMaxWidth()
                        .padding(bottom = 12.dp),
                    horizontalArrangement = Arrangement.SpaceBetween,
                    verticalAlignment = Alignment.CenterVertically
                ) {
                    Spacer(
                        modifier = Modifier
                            .size(width = 100.dp, height = 10.dp)
                            .background(
                                brush = shimmerBrush(),
                                shape = RoundedCornerShape(5.dp)
                            )
                    )
                    Spacer(
                        modifier = Modifier
                            .size(width = 50.dp, height = 10.dp)
                            .background(
                                brush = shimmerBrush(),
                                shape = RoundedCornerShape(5.dp)
                            )
                    )
                }

                Row(
                    modifier = Modifier
                        .fillMaxWidth()
                        .padding(bottom = 12.dp),
                    horizontalArrangement = Arrangement.SpaceBetween,
                    verticalAlignment = Alignment.CenterVertically
                ) {
                    Spacer(
                        modifier = Modifier
                            .size(width = 100.dp, height = 10.dp)
                            .background(
                                brush = shimmerBrush(),
                                shape = RoundedCornerShape(5.dp)
                            )
                    )
                    Spacer(
                        modifier = Modifier
                            .size(width = 50.dp, height = 10.dp)
                            .background(
                                brush = shimmerBrush(),
                                shape = RoundedCornerShape(5.dp)
                            )
                    )
                }

                Box(
                    Modifier
                        .height(1.dp)
                        .fillMaxWidth()
                        .background(Color.Gray, shape = DottedShape(step = 10.dp))
                )

                Row(
                    modifier = Modifier
                        .fillMaxWidth()
                        .padding(top = 12.dp),
                    horizontalArrangement = Arrangement.SpaceBetween,
                    verticalAlignment = Alignment.CenterVertically
                ) {
                    Spacer(
                        modifier = Modifier
                            .size(width = 100.dp, height = 10.dp)
                            .background(
                                brush = shimmerBrush(),
                                shape = RoundedCornerShape(5.dp)
                            )
                    )
                    Spacer(
                        modifier = Modifier
                            .size(width = 50.dp, height = 10.dp)
                            .background(
                                brush = shimmerBrush(),
                                shape = RoundedCornerShape(5.dp)
                            )
                    )
                }

            }
        }
    }
}

@Composable
private fun EmptyState(
    onBackToExploreClicked: () -> Unit
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
                LottieCompositionSpec.RawRes(R.raw.empty_cart_anim)
            )
            LottieAnimation(
                composition = composition,
                iterations = LottieConstants.IterateForever
            )
            Text(
                modifier = Modifier.padding(top = 16.dp),
                text = stringResource(R.string.tu_carro_esta_vacio),
                color = MaterialTheme.colorScheme.onBackground,
                style = MaterialTheme.typography.headlineSmall
            )
            Text(
                modifier = Modifier.padding(top = 8.dp),
                text = stringResource(R.string.articulos_en_tu_carro_se_mostraran_aqui),
                color = MaterialTheme.colorScheme.onSurfaceVariant,
                style = MaterialTheme.typography.bodySmall
            )
            MainButton(
                modifier = Modifier
                    .padding(top = 20.dp)
                    .fillMaxWidth()
                    .height(52.dp),
                text = stringResource(R.string.volver_a_buscar),
                onClick = onBackToExploreClicked
            )
        }
    }
}


@Preview
@Composable
fun CartScreenPreview() {
    CartScreen()
}