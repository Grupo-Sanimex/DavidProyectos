package com.app.sanimex.presentation.screens.tipoIngreso

import android.Manifest
import android.content.Context
import android.content.Intent
import android.location.Geocoder
import android.location.LocationManager
import android.provider.Settings
import android.widget.Toast
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.material3.*
import androidx.compose.runtime.*
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.unit.dp
import androidx.hilt.navigation.compose.hiltViewModel
import com.app.sanimex.R
import com.app.sanimex.core.util.Constants
import com.google.accompanist.permissions.ExperimentalPermissionsApi
import com.google.accompanist.permissions.isGranted
import com.google.accompanist.permissions.rememberPermissionState
import com.google.android.gms.location.FusedLocationProviderClient
import com.google.android.gms.location.LocationServices
import kotlinx.coroutines.launch
import kotlinx.coroutines.suspendCancellableCoroutine
import java.util.*
import kotlin.coroutines.resume
import kotlin.coroutines.resumeWithException

@OptIn(ExperimentalPermissionsApi::class)
@Composable
fun TipoIngresoScreen(
    viewModel: TipoIngresoViewModel = hiltViewModel(),
    onNavigateToHome: () -> Unit = {},
    onNavigateBack: () -> Unit = {}
) {
    val uiState by viewModel.uiState.collectAsState()
    val context = LocalContext.current
    val coroutineScope = rememberCoroutineScope()
    val fusedLocationClient = remember { LocationServices.getFusedLocationProviderClient(context) }
    val locationState = remember { mutableStateOf(LatLng(0.0, 0.0)) }
    val addressState = remember { mutableStateOf("Obteniendo dirección...") }
    val alreadySaved = remember { mutableStateOf(false) }

    // Handle location permission
    val locationPermissionState = rememberPermissionState(Manifest.permission.ACCESS_FINE_LOCATION)

    LaunchedEffect(Unit) {
        snapshotFlow {isLocationEnabled(context) }
            .collect { isEnabled ->
                if (!isEnabled) {
                    Toast.makeText(
                        context,
                        "La ubicación está desactivada. Actívala para continuar usando la app.",
                        Toast.LENGTH_LONG
                    ).show()
                    promptEnableLocation(context)
                }
            }
    }

    // Fetch location and address
    LaunchedEffect(locationPermissionState.status) {
        if (!isLocationEnabled(context)) {
            addressState.value = "Ubicación desactivada. Por favor actívala."
            promptEnableLocation(context)
            return@LaunchedEffect
        }

        if (locationPermissionState.status.isGranted && !alreadySaved.value) {
            coroutineScope.launch {
                try {
                    val location = fusedLocationClient.getLastLocationSuspend()
                    location?.let {
                        locationState.value = LatLng(it.latitude, it.longitude)
                        updateAddress(context, locationState.value, addressState)
                        if (locationState.value.latitude != 0.0 && locationState.value.longitude != 0.0) {
                            Constants.latitud = locationState.value.latitude
                            Constants.longitu = locationState.value.longitude
                            Constants.direccion = addressState.value
                            viewModel.addAddress()
                            alreadySaved.value = true // ← Evita re-ejecuciones
                        }
                    } ?: run { addressState.value = "No se pudo obtener la ubicación" }
                } catch (e: Exception) {
                    addressState.value = "Error al obtener la ubicación"
                }
            }
        } else {
            locationPermissionState.launchPermissionRequest()
        }
    }
    // Navigate based on uiState
    LaunchedEffect(uiState.errorDireccion) {
        if (!uiState.errorDireccion) {
            onNavigateToHome()
        }
    }

    // UI Content
    LazyColumn(
        modifier = Modifier
            .fillMaxSize()
            .padding(16.dp)
    ) {
        item {
            Row(
                modifier = Modifier
                    .fillMaxWidth()
                    .padding(horizontal = 4.dp),
                horizontalArrangement = Arrangement.SpaceBetween,
                verticalAlignment = Alignment.CenterVertically
            ) {
                Row(verticalAlignment = Alignment.CenterVertically) {
                    IconButton(onClick = onNavigateBack) {
                        Icon(
                            painter = painterResource(id = R.drawable.back_icon),
                            contentDescription = "Back",
                            tint = Color.Unspecified
                        )
                    }
                    Text(
                        text = stringResource(R.string.ubicacion),
                        color = MaterialTheme.colorScheme.onBackground,
                        style = MaterialTheme.typography.titleMedium
                    )
                }
            }
        }
        item {
            Column(
                modifier = Modifier
                    .fillMaxWidth()
                    .padding(top = 28.dp),
                horizontalAlignment = Alignment.CenterHorizontally
            ) {
                Spacer(modifier = Modifier.height(50.dp))
                Icon(
                    painter = painterResource(id = R.drawable.location_icon),
                    contentDescription = null,
                    modifier = Modifier
                        .fillMaxWidth()
                        .height(58.dp)
                )
                Spacer(modifier = Modifier.height(20.dp))
                Text(text = addressState.value)
                Spacer(modifier = Modifier.height(20.dp))
            }
        }
    }
}

// Utility data class
data class LatLng(val latitude: Double, val longitude: Double)

// Utility functions
fun isLocationEnabled(context: Context): Boolean {
    val locationManager = context.getSystemService(Context.LOCATION_SERVICE) as LocationManager
    return locationManager.isProviderEnabled(LocationManager.GPS_PROVIDER) ||
            locationManager.isProviderEnabled(LocationManager.NETWORK_PROVIDER)
}

fun promptEnableLocation(context: Context) {
    val intent = Intent(Settings.ACTION_LOCATION_SOURCE_SETTINGS)
    intent.flags = Intent.FLAG_ACTIVITY_NEW_TASK
    context.startActivity(intent)
}

private suspend fun updateAddress(
    context: Context,
    location: LatLng,
    addressState: MutableState<String>
) {
    val geocoder = Geocoder(context, Locale.getDefault())
    try {
        val addresses = geocoder.getFromLocation(location.latitude, location.longitude, 1)
        addressState.value = if (!addresses.isNullOrEmpty()) {
            addresses[0].getAddressLine(0)
        } else {
            "Dirección no disponible"
        }
    } catch (e: Exception) {
        addressState.value = "Error al obtener la dirección"
    }
}

private suspend fun FusedLocationProviderClient.getLastLocationSuspend(): android.location.Location? =
    suspendCancellableCoroutine { continuation ->
        getLastLocation()
            .addOnSuccessListener { location ->
                continuation.resume(location)
            }
            .addOnFailureListener { exception ->
                continuation.resumeWithException(exception)
            }
    }