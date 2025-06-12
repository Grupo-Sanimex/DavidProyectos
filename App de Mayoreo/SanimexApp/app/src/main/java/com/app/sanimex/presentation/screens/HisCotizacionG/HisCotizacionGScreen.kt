package com.app.sanimex.presentation.screens.HisCotizacionG

import android.util.Log
import androidx.compose.foundation.background
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
import androidx.compose.foundation.layout.width
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.items
import androidx.compose.foundation.shape.CircleShape
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material.CircularProgressIndicator
import androidx.compose.material.Icon
import androidx.compose.material.IconButton
import androidx.compose.material.TextButton
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.automirrored.filled.KeyboardArrowRight
import androidx.compose.material.icons.filled.DateRange
import androidx.compose.material.icons.filled.Warning
import androidx.compose.material3.Button
import androidx.compose.material3.ButtonDefaults
import androidx.compose.material3.Card
import androidx.compose.material3.CardDefaults
import androidx.compose.material3.DatePicker
import androidx.compose.material3.DatePickerDialog
import androidx.compose.material3.ExperimentalMaterial3Api
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Scaffold
import androidx.compose.material3.Text
import androidx.compose.material3.TopAppBar
import androidx.compose.material3.rememberDatePickerState
import androidx.compose.runtime.Composable
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.unit.dp
import androidx.hilt.navigation.compose.hiltViewModel
import com.app.sanimex.R
import com.app.sanimex.core.util.Constants
import com.app.sanimex.domain.model.HisCotizacionModel.HisCotizacionGerente
import com.app.sanimex.presentation.components.BottomNavigationBar
import java.text.SimpleDateFormat
import java.util.Calendar
import java.util.Locale

@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun HisCotizacionGScreen(
    onNavigationBack: () -> Unit,
    onNavigateToDetail: (String, String) -> Unit,
    viewModel: HisCotizacionGViewModel = hiltViewModel(),
    onNavigateToExplore: () -> Unit = {},
    onNavigateToUbicacion: () -> Unit = {},
    onNavigateToAccount: () -> Unit = {},
    onNavigateToTicketGerente: () -> Unit = {},
)
{
    val uiState by viewModel.uiState.collectAsState()
    var showDatePicker by remember { mutableStateOf(false) }

    Scaffold(
        bottomBar = {
            BottomNavigationBar(
                selectedItem = 1,
                onNavigateToExplore = onNavigateToExplore,
                onNavigateToAccount = onNavigateToAccount,
                onNavigateToUbicacion = onNavigateToUbicacion,
                onNavigateToTicket = onNavigateToTicketGerente
            )
        },
        topBar = {
            TopAppBar(
                title = { Text(text = stringResource(R.string.visitadores),
                    color = MaterialTheme.colorScheme.onBackground,
                    style = MaterialTheme.typography.titleMedium,) },
                navigationIcon = {
                    IconButton(onClick = onNavigationBack) {
                        Icon(
                            painter = painterResource(id = R.drawable.back_icon),
                            contentDescription = "Regresar"
                        )
                    }
                }
            )
        }
    ) { paddingValues ->
        Column(
            modifier = Modifier
                .fillMaxSize()
                .padding(paddingValues)
                .padding(horizontal = 16.dp)
        ) {
            Spacer(modifier = Modifier.height(6.dp))

            // Botón de fecha con mejor diseño
            Button(
                onClick = { showDatePicker = true },
                modifier = Modifier
                    .fillMaxWidth()
                    .height(56.dp),
                shape = RoundedCornerShape(12.dp),
                colors = ButtonDefaults.buttonColors(
                    containerColor = MaterialTheme.colorScheme.primary
                )
            ) {
                Icon(
                    imageVector = Icons.Default.DateRange,
                    contentDescription = "Calendario",
                    modifier = Modifier.size(24.dp)
                )
                Spacer(modifier = Modifier.width(8.dp))
                Text(
                    text = if (uiState.selectedDate.isEmpty())
                        "Seleccionar Fecha"
                    else
                        "Cambiar Fecha"
                )
            }

            if (uiState.selectedDate.isNotEmpty()) {
                Spacer(modifier = Modifier.height(16.dp))
                Card(
                    modifier = Modifier.fillMaxWidth(),
                    colors = CardDefaults.cardColors(
                        containerColor = MaterialTheme.colorScheme.primaryContainer
                    ),
                    shape = RoundedCornerShape(8.dp)
                ) {
                    Text(
                        text = "Fecha seleccionada: ${uiState.selectedDate}",
                        style = MaterialTheme.typography.labelMedium,
                        modifier = Modifier.padding(16.dp),
                        color = MaterialTheme.colorScheme.onPrimaryContainer
                    )
                }
            }
            //Spacer(modifier = Modifier.height(16.dp))
            /*
            HistCotizacionGerente(
                fecha = uiState.selectedDate,
                idUsuario = Constants.idUsuario,
                onSucursalClick = { fecha,idVistador ->
                    try {
                        onNavigateToDetail(uiState.selectedDate, idVistador)
                    } catch (e: Exception) {
                        Log.e("MapsSucursalScreen", "Error navigating: ${e.message}")
                    }
                }
            )
             */

            Spacer(modifier = Modifier.height(7.dp))
            when {
                uiState.isLoading -> {
                    Box(
                        modifier = Modifier.fillMaxSize(),
                        contentAlignment = Alignment.Center
                    ) {
                        CircularProgressIndicator(
                            color = MaterialTheme.colorScheme.primary,
                            modifier = Modifier.size(48.dp)
                        )
                    }
                }
                uiState.error != null -> {
                    Column(
                        modifier = Modifier.fillMaxWidth(),
                        horizontalAlignment = Alignment.CenterHorizontally
                    ) {
                        Icon(
                            imageVector = Icons.Default.Warning,
                            contentDescription = "Error",
                            tint = MaterialTheme.colorScheme.error,
                            modifier = Modifier.size(48.dp)
                        )
                        Spacer(modifier = Modifier.height(8.dp))
                        Text(
                            text = uiState.error ?: "Error desconocido",
                            color = MaterialTheme.colorScheme.error,
                            textAlign = TextAlign.Center,
                            modifier = Modifier.padding(vertical = 8.dp)
                        )
                        Button(
                            onClick = { viewModel.getHisCotizacion(uiState.selectedDate) },
                            colors = ButtonDefaults.buttonColors(
                                containerColor = MaterialTheme.colorScheme.error
                            )
                        ) {
                            Text("Reintentar")
                        }
                    }
                }

                uiState.hisCotizaciones.isEmpty() -> {
                    Box(
                        modifier = Modifier.fillMaxSize(),
                        contentAlignment = Alignment.Center
                    ) {
                        Column(
                            horizontalAlignment = Alignment.CenterHorizontally
                        ) {
                            Icon(
                                imageVector = Icons.Default.DateRange,
                                contentDescription = "No hay Cotizaciones",
                                tint = MaterialTheme.colorScheme.onSurfaceVariant,
                                modifier = Modifier.size(48.dp)
                            )
                            Spacer(modifier = Modifier.height(8.dp))
                            Text(
                                text = "No hay Visitadores disponibles para esta fecha",
                                style = MaterialTheme.typography.bodyLarge,
                                textAlign = TextAlign.Center,
                                color = MaterialTheme.colorScheme.onSurfaceVariant
                            )
                        }
                    }
                }
                else -> {
                    LazyColumn(
                        modifier = Modifier.fillMaxWidth(),
                        verticalArrangement = Arrangement.spacedBy(12.dp)
                    ) {
                        items(uiState.hisCotizaciones) { hisCotizacion ->
                            HistCotizacionCard(
                                fecha = uiState.selectedDate,
                                sucursal = hisCotizacion,
                                onSucursalClick = { fecha,idVistador ->
                                    try {
                                        onNavigateToDetail(uiState.selectedDate, hisCotizacion.idDispositivo)
                                    } catch (e: Exception) {
                                        Log.e("MapsSucursalScreen", "Error navigating: ${e.message}")
                                    }
                                }
                            )
                        }
                        item {
                            Spacer(modifier = Modifier.height(8.dp))
                        }
                    }
                }
            }
        }

        if (showDatePicker) {
            val datePickerState = rememberDatePickerState()
            DatePickerDialog(
                onDismissRequest = { showDatePicker = false },
                confirmButton = {
                    TextButton(onClick = {
                        datePickerState.selectedDateMillis?.let { millis ->
                            val calendar = Calendar.getInstance().apply {
                                timeInMillis = millis
                                add(Calendar.DAY_OF_MONTH, 1) // Sumar un día
                            }
                            val date = SimpleDateFormat("yyyy-MM-dd", Locale.getDefault()).format(calendar.time)
                            viewModel.updateSelectedDate(date)
                        }
                        showDatePicker = false
                    }) {
                        Text("Confirmar")
                    }
                },
                dismissButton = {
                    TextButton(onClick = { showDatePicker = false }) {
                        Text("Cancelar")
                    }
                }
            ) {
                DatePicker(state = datePickerState)
            }
        }
    }
}

/*@Composable
fun HistCotizacionGerente(
    fecha: String,
    idUsuario: String,
    onSucursalClick: (String, String) -> Unit
) {
    Card(
        modifier = Modifier
            .fillMaxWidth()
            .padding(vertical = 4.dp)
            .height(100.dp)
            .clickable(
                interactionSource = remember { MutableInteractionSource() },
                indication = null
            ) {
                //Log.d("SucursalCard", "Clicked on sucursal: ${sucursal.nombre}")
                onSucursalClick(fecha, idUsuario)
            },
        elevation = CardDefaults.cardElevation(
            defaultElevation = 2.dp
        ),
        colors = CardDefaults.cardColors(
            containerColor = MaterialTheme.colorScheme.primary
        ),
        shape = RoundedCornerShape(12.dp)
    ) {
        Row(
            modifier = Modifier
                .fillMaxSize()
                .padding(16.dp),
            verticalAlignment = Alignment.CenterVertically
        ) {
            // Icono de la sucursal
            Icon(
                painter = painterResource(id = R.drawable.meta_ic), // Asegúrate de tener este icono
                contentDescription = "Icono Sucursal",
                modifier = Modifier
                    .size(40.dp)
                    .background(
                        color = MaterialTheme.colorScheme.onPrimary.copy(alpha = 0.1f),
                        shape = CircleShape
                    )
                    .padding(8.dp),
                tint = MaterialTheme.colorScheme.onSurfaceVariant
            )

            Spacer(modifier = Modifier.width(16.dp))

            // Información de la sucursal
            Column(
                modifier = Modifier.weight(1f)
            ) {
                Text(
                    text = "Ver Mis Cotizaciones",
                    style = MaterialTheme.typography.titleMedium,
                    color = MaterialTheme.colorScheme.onPrimary
                )
            }

            // Flecha o indicador
            Icon(
                imageVector = Icons.AutoMirrored.Filled.KeyboardArrowRight,
                contentDescription = "Ver detalles",
                tint = MaterialTheme.colorScheme.onPrimary,
                modifier = Modifier.size(24.dp)
            )
        }
    }
}

 */

@Composable
fun HistCotizacionCard(
    fecha: String,
    sucursal: HisCotizacionGerente,
    onSucursalClick: (String, String) -> Unit
) {
    Card(
        modifier = Modifier
            .fillMaxWidth()
            .padding(vertical = 4.dp)
            .height(100.dp)
            .clickable(
                interactionSource = remember { MutableInteractionSource() },
                indication = null
            ) {
                Log.d("SucursalCard", "Clicked on sucursal: ${sucursal.nombre}")
                onSucursalClick(fecha, sucursal.idDispositivo)
            },
        elevation = CardDefaults.cardElevation(
            defaultElevation = 2.dp
        ),
        colors = CardDefaults.cardColors(
            containerColor = MaterialTheme.colorScheme.surface
        ),
        shape = RoundedCornerShape(12.dp)
    ) {
        Row(
            modifier = Modifier
                .fillMaxSize()
                .padding(16.dp),
            verticalAlignment = Alignment.CenterVertically
        ) {
            // Icono de la sucursal
            Icon(
                painter = painterResource(id = R.drawable.fashion_icon), // Asegúrate de tener este icono
                contentDescription = "Icono Sucursal",
                modifier = Modifier
                    .size(40.dp)
                    .background(
                        color = MaterialTheme.colorScheme.primary.copy(alpha = 0.1f),
                        shape = CircleShape
                    )
                    .padding(8.dp),
                tint = MaterialTheme.colorScheme.primary
            )

            Spacer(modifier = Modifier.width(16.dp))

            // Información de la sucursal
            Column(
                modifier = Modifier.weight(1f)
            ) {
                Text(
                    text = "${sucursal.nombre} ${sucursal.aPaterno} ${sucursal.aMaterno}",
                    style = MaterialTheme.typography.titleMedium,
                    color = MaterialTheme.colorScheme.onSurfaceVariant
                )

                Spacer(modifier = Modifier.height(4.dp))

                Text(
                    text = "Numero Empleado: ${sucursal.numEmpleado}",
                    style = MaterialTheme.typography.bodyMedium,
                    color = MaterialTheme.colorScheme.onSurfaceVariant
                )
            }

            // Flecha o indicador
            Icon(
                imageVector = Icons.AutoMirrored.Filled.KeyboardArrowRight,
                contentDescription = "Ver detalles",
                tint = MaterialTheme.colorScheme.primary,
                modifier = Modifier.size(24.dp)
            )
        }
    }
}

