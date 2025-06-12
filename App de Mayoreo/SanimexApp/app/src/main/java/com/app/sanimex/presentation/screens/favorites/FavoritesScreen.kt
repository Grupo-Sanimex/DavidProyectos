package com.app.sanimex.presentation.screens.favorites

import android.Manifest
import android.annotation.SuppressLint
import android.app.Activity
import android.content.Context
import android.content.pm.PackageManager
import android.location.Geocoder
import android.location.LocationManager
import androidx.activity.compose.rememberLauncherForActivityResult
import androidx.activity.result.contract.ActivityResultContracts
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
import androidx.compose.foundation.layout.heightIn
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.lazy.grid.GridCells
import androidx.compose.foundation.lazy.grid.LazyVerticalGrid
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material3.Divider
import androidx.compose.material3.DropdownMenu
import androidx.compose.material3.DropdownMenuItem
import androidx.compose.material3.Icon
import androidx.compose.material3.IconButton
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Surface
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.MutableState
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.rememberCoroutineScope
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.text.style.TextOverflow
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import androidx.core.content.ContextCompat
import androidx.hilt.navigation.compose.hiltViewModel
import com.airbnb.lottie.compose.LottieAnimation
import com.airbnb.lottie.compose.LottieCompositionSpec
import com.airbnb.lottie.compose.LottieConstants
import com.airbnb.lottie.compose.rememberLottieComposition
import com.app.sanimex.R
import com.app.sanimex.core.util.Constants
import com.app.sanimex.domain.model.Sucursal
import com.app.sanimex.presentation.components.MainButton
import com.app.sanimex.presentation.components.shimmerBrush
import com.google.android.gms.location.FusedLocationProviderClient
import com.google.android.gms.location.LocationServices
import com.google.android.gms.maps.model.LatLng
import kotlinx.coroutines.launch
import kotlinx.coroutines.tasks.await
import java.util.Locale
import android.content.Intent
import android.os.Looper
import android.provider.Settings
import android.widget.Toast
import androidx.compose.material3.HorizontalDivider
import androidx.compose.runtime.snapshotFlow
import com.google.android.gms.location.LocationCallback
import com.google.android.gms.location.LocationRequest
import com.google.android.gms.location.LocationResult
import com.google.android.gms.location.Priority
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.withContext

@Composable
fun FavoritesScreen(
    viewModel: FavoritesViewModel = hiltViewModel(),
    onNavigationBack: () -> Unit = {},
    onNavigationLogin: () -> Unit = {},
    onNavigationExplore: (String) -> Unit = {},
    onNavigateToCliente: (String) -> Unit = {},
) {
    val uiState by viewModel.uiState.collectAsState()

    // Siempre mostrar LocationDisplayAndStore
    LocationDisplayAndStore(
        getWishlist = viewModel::getWishlist
    )
    FavoritesScreenContent(
        uiState = uiState,
        onBackClicked = onNavigationBack,
        onLoginClicked = onNavigationLogin,
        onExploreClicked = onNavigationExplore,
        onClienteClicked = onNavigateToCliente,
    )
}

@Composable
private fun FavoritesScreenContent(
    uiState: FavoritesScreenUiState,
    onBackClicked: () -> Unit,
    onLoginClicked: () -> Unit,
    onExploreClicked: (String) -> Unit,
    onClienteClicked: (String) -> Unit
) {
    Surface(
        modifier = Modifier.fillMaxSize()
    ) {
        Column(
            modifier = Modifier
                .fillMaxSize()
                .padding(vertical = 26.dp)
        ) {
            ScreenHeader(
                onBackClicked = onBackClicked,
            )
            HorizontalDivider(
                modifier = Modifier.padding(top = 8.dp),
                color = MaterialTheme.colorScheme.onSurfaceVariant.copy(alpha = 0.3f)
            )
            AnimatedVisibility(
                visible = uiState.isLoading,
                enter = fadeIn(),
                exit = fadeOut(animationSpec = tween(durationMillis = 500))
            ) {
                LoadingState()
            }
            AnimatedVisibility(
                visible = !uiState.isLoading && uiState.wishlist.isEmpty(),
                enter = fadeIn(animationSpec = tween(delayMillis = 500)),
                exit = fadeOut()
            ) {
                EmptyState(
                    onBackToHomeClicked = onLoginClicked
                )
            }
            AnimatedVisibility(
                visible = !uiState.isLoading && uiState.wishlist.isNotEmpty(),
                enter = fadeIn(animationSpec = tween(delayMillis = 100)),
                exit = fadeOut()
            ) {
                FavoriteItemsSection(
                    wishlist = uiState.wishlist,
                    onClienteClicked = onClienteClicked,
                    onExploreClicked = onExploreClicked,
                )
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
                text = stringResource(R.string.sucursal),
                color = MaterialTheme.colorScheme.onBackground,
                style = MaterialTheme.typography.titleMedium,
            )
        }
    }
}

@Composable
private fun FavoriteItemsSection(
    wishlist: List<Sucursal>,  // Lista de sucursales
    onClienteClicked: (String) -> Unit,  // Callback cuando un producto es clicado
    onExploreClicked: (String) -> Unit,// Callback para eliminar un producto
) {
    // Solo se crea un FavoriteProduct para mostrar el selector
    FavoriteProduct(
        wishlist = wishlist,
        onClienteClicked = onClienteClicked,
        onExploreClicked = onExploreClicked
    )
}

@Composable
private fun FavoriteProduct(
    wishlist: List<Sucursal>,            // Lista de sucursales para el menú desplegable
    onClienteClicked: (String) -> Unit,  // Callback cuando un producto es clicado
    onExploreClicked: (String) -> Unit,         // Callback para redirigir si no hay elementos
) {
    if (wishlist.size == 1) {
        val firstSucursalIdSAP = wishlist.first().idSAP
        if (Constants.idRol == 10 || Constants.idRol == 8 || Constants.idRol == 1) {
            LaunchedEffect(Unit) {
                onExploreClicked(firstSucursalIdSAP)
            }
        }else{
            LaunchedEffect(Unit) {
                onClienteClicked(firstSucursalIdSAP)
            }
        }
    } else {
        var expanded by remember { mutableStateOf(false) }
        var selectedText by remember { mutableStateOf("Selecciona sucursal") }
        var selectedSucursalId by remember { mutableStateOf<String?>(null) }

        Column(
            modifier = Modifier
                .fillMaxWidth()
                .padding(vertical = 24.dp, horizontal = 24.dp)
                .clip(RoundedCornerShape(5.dp))
                .border(
                    width = 1.dp,
                    color = MaterialTheme.colorScheme.primary,
                    shape = RoundedCornerShape(5.dp)
                )
                .clickable { expanded = !expanded }
        ) {
            Text(
                modifier = Modifier
                    .padding(vertical = 10.dp)
                    .fillMaxWidth(),
                text = selectedText,
                color = MaterialTheme.colorScheme.onBackground,
                style = MaterialTheme.typography.labelMedium,
                overflow = TextOverflow.Ellipsis,
                maxLines = 1,
                textAlign = TextAlign.Center
            )
            DropdownMenu(
                expanded = expanded,
                onDismissRequest = { expanded = false },
                modifier = Modifier
                    .fillMaxWidth()
                    .heightIn(max = 300.dp)
                    .background(MaterialTheme.colorScheme.background)
            ) {
                wishlist.forEach { sucursal ->
                        DropdownMenuItem(
                            text = { Text(sucursal.nombreSucursal) },
                            onClick = {
                                selectedText = sucursal.nombreSucursal
                                selectedSucursalId = sucursal.idSAP
                                Constants.corredor = sucursal.idSAP
                                onClienteClicked(sucursal.idSAP)
                                expanded = false
                            }
                        )
                }
            }
        }
    }
}

@Composable
fun LocationDisplayAndStore(
    getWishlist: () -> Unit
) {
    val context = LocalContext.current
    val fusedLocationClient = remember { LocationServices.getFusedLocationProviderClient(context) }
    val currentLocation = remember { mutableStateOf(LatLng(0.0, 0.0)) }
    val addressState = remember { mutableStateOf("Obteniendo dirección...") }
    val coroutineScope = rememberCoroutineScope()


    // ✅ Aquí defines el launcher, dentro del @Composable
    val locationPermissionLauncher = rememberLauncherForActivityResult(
        contract = ActivityResultContracts.RequestPermission()
    ) { isGranted ->
        if (!isGranted) {
            Toast.makeText(
                context,
                "Permiso de ubicación denegado. La app no podrá funcionar correctamente.",
                Toast.LENGTH_LONG
            ).show()
        } else {
            // Si quieres hacer algo cuando se conceda el permiso, hazlo aquí
            coroutineScope.launch {
                    getWishlist()
            }
        }
    }
    // ✅ Este efecto ahora sí puede usar locationPermissionLauncher
    LaunchedEffect(Unit) {
        snapshotFlow {
            isLocationEnabled(context) &&
                    ContextCompat.checkSelfPermission(
                        context,
                        Manifest.permission.ACCESS_FINE_LOCATION
                    ) == PackageManager.PERMISSION_GRANTED
        }.collect { isGrantedAndEnabled ->
            if (!isGrantedAndEnabled) {
                Toast.makeText(
                    context,
                    "Ubicación exacta requerida. Activa el GPS y concede permisos.",
                    Toast.LENGTH_LONG
                ).show()

                if (!isLocationEnabled(context)) {
                    promptEnableLocation(context)
                }

                if (ContextCompat.checkSelfPermission(
                        context,
                        Manifest.permission.ACCESS_FINE_LOCATION
                    ) != PackageManager.PERMISSION_GRANTED
                ) {
                    locationPermissionLauncher.launch(Manifest.permission.ACCESS_FINE_LOCATION)
                    coroutineScope.launch {
                        getWishlist()
                    }
                }
            }else{
                coroutineScope.launch {
                    getWishlist()
                }
            }
        }
    }

    // Recuperar la última ubicación guardada al iniciar
    LaunchedEffect(Unit) {
        val savedLat = Constants.latitud
        val savedLong = Constants.longitu
        if (savedLat != 0.0 && savedLong != 0.0) {
            currentLocation.value = LatLng(savedLat, savedLong)
            updateAddress(context, currentLocation.value, addressState)
        }
    }

    // Verificar y solicitar permisos al iniciar
    LaunchedEffect(Unit) {
        if (!isLocationEnabled(context)) {
            addressState.value = "Ubicación desactivada. Por favor actívala."
            promptEnableLocation(context)
            restartApp(context)
            return@LaunchedEffect
        }

        when {
            ContextCompat.checkSelfPermission(
                context,
                Manifest.permission.ACCESS_FINE_LOCATION
            ) == PackageManager.PERMISSION_GRANTED -> {
                fetchLocation(fusedLocationClient, currentLocation, context)
                coroutineScope.launch {
                    getWishlist()
                }
            }
            else -> {
                locationPermissionLauncher.launch(Manifest.permission.ACCESS_FINE_LOCATION)
                return@LaunchedEffect
            }
        }
    }

    // Obtener y guardar dirección si la ubicación es válida
    LaunchedEffect(currentLocation.value) {
        if (currentLocation.value.latitude != 0.0 && currentLocation.value.longitude != 0.0) {
            updateAddress(context, currentLocation.value, addressState)
            // Guardar la ubicación
            Constants.latitud = currentLocation.value.latitude
            Constants.longitu = currentLocation.value.longitude
        } else {
            addressState.value = "Ubicación no disponible"
        }
    }
}

fun restartApp(context: Context) {
    val intent = context.packageManager.getLaunchIntentForPackage(context.packageName)
    intent?.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP or Intent.FLAG_ACTIVITY_NEW_TASK)
    context.startActivity(intent)
    if (context is Activity) {
        context.finish()
    }
    Runtime.getRuntime().exit(0) // Forzar cierre del proceso (opcional)
}


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

@SuppressLint("MissingPermission")
private suspend fun fetchLocation(
    fusedLocationClient: FusedLocationProviderClient,
    currentLocation: MutableState<LatLng>,
    context: Context
) {
    if (ContextCompat.checkSelfPermission(
            context,
            Manifest.permission.ACCESS_FINE_LOCATION
        ) == PackageManager.PERMISSION_GRANTED
    ) {
        try {
            val location = fusedLocationClient.lastLocation.await()
            if (location != null) {
                currentLocation.value = LatLng(location.latitude, location.longitude)
            } else {
                // Intentar obtener ubicación en tiempo real
                val locationRequest = LocationRequest.Builder(
                    Priority.PRIORITY_HIGH_ACCURACY,
                    1000L // intervalo de 1 segundo
                ).apply {
                    setWaitForAccurateLocation(true)
                    setMinUpdateIntervalMillis(500L)
                    setMaxUpdates(1)
                }.build()

                val locationCallback = object : LocationCallback() {
                    override fun onLocationResult(locationResult: LocationResult) {
                        val loc = locationResult.lastLocation
                        if (loc != null) {
                            currentLocation.value = LatLng(loc.latitude, loc.longitude)
                        } else {
                            Toast.makeText(context, "No se pudo obtener ubicación en tiempo real", Toast.LENGTH_SHORT).show()
                        }
                        fusedLocationClient.removeLocationUpdates(this) // Limpia el callback
                    }
                }

                // Lanzar la solicitud de ubicación en tiempo real
                withContext(Dispatchers.Main) {
                    fusedLocationClient.requestLocationUpdates(locationRequest, locationCallback, Looper.getMainLooper())
                }
            }
        } catch (e: Exception) {
            e.printStackTrace()
            Toast.makeText(context, "Error al obtener la ubicación", Toast.LENGTH_SHORT).show()
        }
    }
}

private suspend fun updateAddress(
    context: Context,
    location: LatLng,
    addressState: MutableState<String>
) {
    if (location.latitude == 0.0 && location.longitude == 0.0) {
        addressState.value = "Ubicación inválida"
        return
    }

    val geocoder = Geocoder(context, Locale.getDefault())
    try {
        val addresses = geocoder.getFromLocation(location.latitude, location.longitude, 1)
        if (!addresses.isNullOrEmpty()) {
            val address = addresses[0].getAddressLine(0)
            addressState.value = address
            Constants.direccion = address
        } else {
            addressState.value = "Dirección no disponible"
        }
    } catch (e: Exception) {
        e.printStackTrace()
        addressState.value = "Error al obtener la dirección"
    }
}


@Composable
private fun LoadingState() {
    LazyVerticalGrid(
        modifier = Modifier
            .fillMaxWidth()
            .padding(16.dp),
        columns = GridCells.Fixed(2),
        verticalArrangement = Arrangement.spacedBy(16.dp),
        horizontalArrangement = Arrangement.spacedBy(16.dp)
    ) {
        items(count = 1) {
            Column(
                modifier = Modifier
                    .padding(end = 12.dp)
                    .clip(RoundedCornerShape(5.dp))
                    .background(MaterialTheme.colorScheme.surface) // Fondo para cada ítem
                    .padding(16.dp) // Espaciado interno para mejorar la apariencia
            ) {
                // Carga de imagen simulada
                Spacer(
                    modifier = Modifier
                        .fillMaxWidth()
                        .height(200.dp) // Ajusta la altura de la imagen
                        .clip(RoundedCornerShape(5.dp))
                        .background(brush = shimmerBrush())
                )
                Spacer(modifier = Modifier.height(12.dp)) // Espaciado mejorado

                // Carga de texto simulada (nombre del producto)
                Spacer(
                    modifier = Modifier
                        .fillMaxWidth()
                        .height(16.dp) // Altura del texto
                        .clip(RoundedCornerShape(5.dp))
                        .background(brush = shimmerBrush())
                )
                Spacer(modifier = Modifier.height(8.dp)) // Espaciado entre líneas de texto

                // Carga de texto simulada (más detalles)
                Spacer(
                    modifier = Modifier
                        .fillMaxWidth(0.6f) // Ajusta el ancho del segundo texto
                        .height(16.dp)
                        .clip(RoundedCornerShape(5.dp))
                        .background(brush = shimmerBrush())
                )
                Spacer(modifier = Modifier.height(8.dp)) // Espaciado adicional
            }
        }
    }
}


@Composable
private fun EmptyState(
    onBackToHomeClicked: () -> Unit
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
                LottieCompositionSpec.RawRes(R.raw.no_feedback_anim)
            )
            LottieAnimation(
                modifier = Modifier.size(290.dp),
                composition = composition,
                iterations = LottieConstants.IterateForever
            )
            Text(
                modifier = Modifier.padding(top = 16.dp),
                text = stringResource(R.string.you_don_t_have_items_in_your_favorites),
                color = MaterialTheme.colorScheme.onBackground,
                style = MaterialTheme.typography.headlineSmall,
                textAlign = TextAlign.Center
            )
            Text(
                modifier = Modifier.padding(top = 8.dp , start = 8.dp, end = 8.dp),
                text = stringResource(R.string.items_marked_as_favorite_will_be_shown_here),
                color = MaterialTheme.colorScheme.onSurfaceVariant,
                style = MaterialTheme.typography.bodyMedium
            )
            MainButton(
                modifier = Modifier
                    .padding(top = 20.dp)
                    .fillMaxWidth()
                    .height(52.dp),
                text = stringResource(R.string.back_to_Login),
                onClick = onBackToHomeClicked
            )
        }
    }
}

@Preview
@Composable
private fun FavoritesScreenPreview() {
    FavoritesScreen()
}