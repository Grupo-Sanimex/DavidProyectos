package com.app.sanimex.presentation.screens.RutaVisitador

import androidx.compose.foundation.layout.*
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.items
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material.CircularProgressIndicator
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.LocationOn
import androidx.compose.material.icons.filled.Warning
import androidx.compose.material3.*
import androidx.compose.runtime.*
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.unit.dp
import androidx.hilt.navigation.compose.hiltViewModel
import com.app.sanimex.R
import com.app.sanimex.domain.model.Ubicacion.UbicacionMaps
import com.google.android.gms.maps.model.CameraPosition
import com.google.android.gms.maps.model.LatLng
import com.google.maps.android.compose.*

@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun RutaVisitadorScreen(
    onNavigationBack: () -> Unit,
    claveSucursal: String,
    numeroEmpleado: String,
    fecha: String,
    viewModel: RutaVisitadorViewModel = hiltViewModel()
) {
    val uiState by viewModel.uiState.collectAsState()

    LaunchedEffect(key1 = true) {
        viewModel.getUbicaciones(claveSucursal, numeroEmpleado, fecha)
    }

    Scaffold(
        topBar = {
            TopAppBar(
                title = {
                    Text(
                        text = "Ruta del Visitador",
                        color = MaterialTheme.colorScheme.onBackground,
                        style = MaterialTheme.typography.titleMedium
                    )
                },
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
        ) {
            when {
                uiState.isLoading -> {
                    Box(
                        modifier = Modifier.fillMaxSize(),
                        contentAlignment = Alignment.Center
                    ) {
                        CircularProgressIndicator()
                    }
                }
                uiState.error != null -> {
                    ErrorMessage(
                        message = uiState.error!!,
                        onRetry = {
                            viewModel.getUbicaciones(
                                uiState.claveSucursal,
                                uiState.numeroEmpleado,
                                uiState.fecha
                            )
                        }
                    )
                }
                uiState.ubicaciones.isEmpty() -> {
                    EmptyState()
                }
                else -> {
                    MapContent(
                        ubicaciones = uiState.ubicaciones,
                        routePoints = uiState.routePoints,
                        onLocationSelected = { viewModel.updateSelectedLocation(it) }
                    )
                }
            }
        }
    }
}

@Composable
fun MapContent(
    ubicaciones: List<UbicacionMaps>,
    routePoints: List<LatLng>,
    onLocationSelected: (LatLng) -> Unit
) {
    val firstLocation = ubicaciones.firstOrNull()?.let {
        LatLng(it.latitud, it.longitud)
    } ?: LatLng(0.0, 0.0)

    val cameraPositionState = rememberCameraPositionState {
        position = CameraPosition.fromLatLngZoom(firstLocation, 15f)
    }

    Box(modifier = Modifier.fillMaxSize()) {
        GoogleMap(
            modifier = Modifier.fillMaxSize(),
            cameraPositionState = cameraPositionState,
            properties = MapProperties(isMyLocationEnabled = true)
        ) {
            // Dibujar marcadores
            ubicaciones.forEach { ubicacion ->
                Marker(
                    state = MarkerState(
                        position = LatLng(ubicacion.latitud, ubicacion.longitud)
                    ),
                    title = "Visita: ${ubicacion.horaUnitaria}",
                    snippet = ubicacion.direccion,
                    onClick = {
                        onLocationSelected(LatLng(ubicacion.latitud, ubicacion.longitud))
                        true
                    }
                )
            }

            // Dibujar la ruta
            if (routePoints.size > 1) {
                Polyline(
                    points = routePoints,
                    color = MaterialTheme.colorScheme.primary
                )
            }
        }

        // Lista de ubicaciones con horarios
        Card(
            modifier = Modifier
                .align(Alignment.BottomCenter)
                .fillMaxWidth()
                .padding(16.dp),
            shape = RoundedCornerShape(12.dp)
        ) {
            LazyColumn(
                modifier = Modifier.heightIn(max = 200.dp)
            ) {
                items(ubicaciones.sortedBy { it.horaUnitaria }) { ubicacion ->
                    ListItem(
                        headlineContent = {
                            Text(text = ubicacion.horaUnitaria,
                                color = MaterialTheme.colorScheme.primary)
                        },
                        supportingContent = {
                            Text(text = ubicacion.direccion)
                        },
                        leadingContent = {
                            Icon(
                                imageVector = Icons.Default.LocationOn,
                                contentDescription = null,
                                tint = MaterialTheme.colorScheme.primary
                            )
                        }
                    )
                    HorizontalDivider()
                }
            }
        }
    }
}

@Composable
private fun EmptyState() {
    Box(
        modifier = Modifier.fillMaxSize(),
        contentAlignment = Alignment.Center
    ) {
        Column(
            horizontalAlignment = Alignment.CenterHorizontally
        ) {
            Icon(
                imageVector = Icons.Default.LocationOn,
                contentDescription = null,
                modifier = Modifier.size(64.dp),
                tint = MaterialTheme.colorScheme.onSurfaceVariant
            )
            Spacer(modifier = Modifier.height(16.dp))
            Text(
                text = "No hay ubicaciones disponibles",
                style = MaterialTheme.typography.titleMedium,
                color = MaterialTheme.colorScheme.onSurfaceVariant
            )
        }
    }
}

@Composable
private fun ErrorMessage(
    message: String,
    onRetry: () -> Unit
) {
    Column(
        modifier = Modifier.fillMaxSize(),
        horizontalAlignment = Alignment.CenterHorizontally,
        verticalArrangement = Arrangement.Center
    ) {
        Icon(
            imageVector = Icons.Default.Warning,
            contentDescription = null,
            modifier = Modifier.size(64.dp),
            tint = MaterialTheme.colorScheme.error
        )
        Spacer(modifier = Modifier.height(16.dp))
        Text(
            text = message,
            style = MaterialTheme.typography.bodyLarge,
            color = MaterialTheme.colorScheme.error,
            textAlign = TextAlign.Center
        )
        Spacer(modifier = Modifier.height(16.dp))
        Button(
            onClick = onRetry,
            colors = ButtonDefaults.buttonColors(
                containerColor = MaterialTheme.colorScheme.error
            )
        ) {
            Text("Reintentar")
        }
    }
} 