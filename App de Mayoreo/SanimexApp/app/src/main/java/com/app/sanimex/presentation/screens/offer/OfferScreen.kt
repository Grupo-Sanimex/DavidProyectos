package com.app.sanimex.presentation.screens.offer

import android.annotation.SuppressLint
import androidx.compose.animation.AnimatedVisibility
import androidx.compose.animation.core.tween
import androidx.compose.animation.fadeIn
import androidx.compose.animation.fadeOut
import androidx.compose.foundation.background
import androidx.compose.foundation.border
import androidx.compose.foundation.clickable
import androidx.compose.foundation.horizontalScroll
import androidx.compose.foundation.interaction.MutableInteractionSource
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.RowScope
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxHeight
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.layout.width
import androidx.compose.foundation.layout.widthIn
import androidx.compose.foundation.layout.wrapContentWidth
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.grid.GridCells
import androidx.compose.foundation.lazy.grid.LazyVerticalGrid
import androidx.compose.foundation.lazy.grid.items
import androidx.compose.foundation.lazy.itemsIndexed
import androidx.compose.foundation.rememberScrollState
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.foundation.verticalScroll
import androidx.compose.material.Surface
import androidx.compose.material3.Divider
import androidx.compose.material3.ExperimentalMaterial3Api
import androidx.compose.material3.HorizontalDivider
import androidx.compose.material3.Icon
import androidx.compose.material3.IconButton
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Scaffold
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
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
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.text.input.ImeAction
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.text.style.TextDecoration
import androidx.compose.ui.text.style.TextOverflow
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import androidx.hilt.navigation.compose.hiltViewModel
import com.app.sanimex.R
import com.app.sanimex.domain.model.HistoricoVtaMayItem
import com.app.sanimex.presentation.components.BottomNavigationBar
import com.app.sanimex.presentation.components.MainTextFieldSearch
import com.app.sanimex.presentation.components.NetworkImage
import com.app.sanimex.presentation.components.shimmerBrush


@SuppressLint("UnusedMaterial3ScaffoldPaddingParameter")
@Composable
fun OfferScreen(
    viewModel: OfferViewModel = hiltViewModel(),
    onNavigationBack: () -> Unit = {}
) {



    val uiState by viewModel.uiState.collectAsState()
    Scaffold(
    ) {
        OfferScreenContent(
            uiState = uiState,
            onBackClicked = onNavigationBack
        )
    }
}

@Composable
private fun OfferScreenContent(
    uiState: OfferScreenUIState,
    onBackClicked: () -> Unit,
) {
    Surface {
        Column(
            modifier = Modifier
                .fillMaxSize()
                .padding(top = 26.dp, bottom = 5.dp)
        ) {
            ScreenHeader(
                onBackClicked = onBackClicked
                ,uiState = uiState
            )
            AnimatedVisibility(
                visible = uiState.isLoading,
                enter = fadeIn(),
                exit = fadeOut(animationSpec = tween(durationMillis = 500))
            ) {
                //LoadingState()
            }
            AnimatedVisibility(
                visible = !uiState.isLoading,
                enter = fadeIn(animationSpec = tween(delayMillis = 500)),
                exit = fadeOut()
            ) {

            }
        }
    }
}

@Composable
private fun ScreenHeader(
    onBackClicked: () -> Unit,
    uiState: OfferScreenUIState
) {
    var searchText by remember { mutableStateOf("") } // Estado para el texto de búsqueda
    Column {
        Row(
            modifier = Modifier
                .fillMaxWidth()
                .padding(horizontal = 4.dp),
            horizontalArrangement = Arrangement.SpaceBetween,
            verticalAlignment = Alignment.CenterVertically
        ) {
            Row(verticalAlignment = Alignment.CenterVertically) {
                IconButton(onClick = onBackClicked) {
                    Icon(
                        painter = painterResource(id = R.drawable.back_icon),
                        contentDescription = null,
                        tint = Color.DarkGray
                    )
                }
            }
            Row(
                modifier = Modifier.padding(bottom = 10.dp, start = 7.dp, end = 7.dp),
                verticalAlignment = Alignment.CenterVertically
            ) {
                MainTextFieldSearch(
                    modifier = Modifier
                        .height(56.dp)
                        .weight(3f),
                    value = searchText, // Estado conectado
                    onValueChanged = { newText -> searchText = newText },
                    placeHolder = stringResource(R.string.buscar_producto),
                    imeAction = ImeAction.Search,
                    leadingIcon = R.drawable.search_icon
                )
            }
        }
        HorizontalDivider(
            color = MaterialTheme.colorScheme.onSurfaceVariant.copy(alpha = 0.2f)
        )
        // Pasamos el estado de búsqueda a ProductsSection
        ProductsSection(
            invoiceList = uiState.invoiceList,
            searchQuery = searchText
        )
    }
}

@Composable
fun ProductsSection(
    invoiceList: List<HistoricoVtaMayItem>,
    searchQuery: String
) {
    // Filtramos la lista según el texto de búsqueda
    val filteredInvoiceList = remember(searchQuery, invoiceList) {
        invoiceList.filter { item ->
            item.descripcion.contains(searchQuery, ignoreCase = true) ||
                    item.codigo.contains(searchQuery, ignoreCase = true)
        }
    }

    val scrollState = rememberScrollState()
    var selectedRow by remember { mutableStateOf(-1) }

    Column(Modifier.fillMaxWidth()) {
        // Encabezado
        Row(Modifier.fillMaxWidth()) {
            TableCell(
                text = "Descripción",
                title = true,
                modifier = Modifier
                    .background(MaterialTheme.colorScheme.outlineVariant)
                    .width(100.dp)
            )

            Row(
                modifier = Modifier
                    .weight(1f)
                    .horizontalScroll(scrollState)
            ) {
                TableCell(
                    text = "Código", title = true,
                    modifier = Modifier.background(MaterialTheme.colorScheme.primary).width(120.dp)
                )
                TableCell(
                    text = "Actual Importe", alignment = TextAlign.Right, title = true,
                    modifier = Modifier.background(MaterialTheme.colorScheme.primary).width(120.dp)
                )
                TableCell(
                    text = "Actual Cantidad", alignment = TextAlign.Right, title = true,
                    modifier = Modifier.background(MaterialTheme.colorScheme.primary).width(120.dp)
                )
            }
        }

        // Filas del contenido
        LazyColumn(Modifier.fillMaxWidth()) {
            itemsIndexed(filteredInvoiceList) { index, invoice ->
                val isSelected = index == selectedRow
                val backgroundColor = if (isSelected) Color.LightGray else Color.Transparent

                Row(
                    Modifier
                        .fillMaxWidth()
                        .background(backgroundColor)
                        .clickable { selectedRow = index }
                ) {
                    TableCell(
                        text = invoice.descripcion,
                        alignment = TextAlign.Left,
                        modifier = Modifier
                            .padding(8.dp)
                            .width(100.dp)
                    )
                    Row(
                        modifier = Modifier
                            .weight(1f)
                            .padding(8.dp)
                            .horizontalScroll(scrollState)
                    ) {
                        TableCell(text = invoice.codigo, modifier = Modifier.width(120.dp))
                        TableCell(text = invoice.IMPORTE_ACTUAL.toString(), alignment = TextAlign.Right, modifier = Modifier.width(120.dp))
                        TableCell(text = invoice.CANTIDAD_ACTUAL.toString(), alignment = TextAlign.Right, modifier = Modifier.width(120.dp))
                    }
                }
            }
        }
    }
}


@Composable
fun TableCell(
    text: String,
    alignment: TextAlign = TextAlign.Center,
    title: Boolean = false,
    modifier: Modifier = Modifier
) {
    Text(
        text = text,
        textAlign = alignment,
        modifier = modifier
            .padding(8.dp),
        fontWeight = if (title) FontWeight.Bold else FontWeight.Normal,
        color = Color.Black
    )
}

@Composable
fun StatusCell(
    text: String,
    alignment: TextAlign = TextAlign.Center,
    modifier: Modifier = Modifier
) {
    val color = when (text) {
        "Pending" -> Color(0xfff8deb5)
        "Paid" -> Color(0xffadf7a4)
        else -> Color(0xffffcccf)
    }
    val textColor = when (text) {
        "Pending" -> Color(0xffde7a1d)
        "Paid" -> Color(0xff00ad0e)
        else -> Color(0xffca1e17)
    }

    Text(
        text = text,
        modifier = modifier
            .padding(12.dp)
            .background(color, shape = RoundedCornerShape(50.dp)),
        textAlign = alignment,
        color = textColor
    )
}


@Preview
@Composable
fun OfferScreenPreview() {
    OfferScreen()
}