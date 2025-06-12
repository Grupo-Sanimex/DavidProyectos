package com.app.sanimex.presentation.screens.ClinteMayoreo

import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.material3.AlertDialog
import androidx.compose.material3.Icon
import androidx.compose.material3.IconButton
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Text
import androidx.compose.material3.TextButton
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.unit.dp
import androidx.hilt.navigation.compose.hiltViewModel
import com.app.sanimex.R
import com.app.sanimex.presentation.components.MainButton
@Composable
fun ClienteScreen(
    viewModel: ClienteViewModel = hiltViewModel(),
    onNavigateToHome: () -> Unit = {},
){
    val uiState by viewModel.uiState.collectAsState()
    ClienteScreenContent(
        uiState = uiState,
        onConsultaInClicked = viewModel::onConsultaButtonClicked,
        onClienteInClicked = viewModel::onVisitaButtonClicked,
    )
    LaunchedEffect(key1 = uiState.isLoginSuccessful) {
        if (uiState.isLoginSuccessful) {
            onNavigateToHome()
        }
    }
}
@Composable
private fun ClienteScreenContent(
     uiState: ClienteUiState,
     onConsultaInClicked: () -> Unit,
     onClienteInClicked: () -> Unit,
) {
    LazyColumn(
        modifier = Modifier
            .fillMaxSize()
            .padding(16.dp)
    ) {
        item {
            ScreenHeader(
                //onBackClicked = onBackClicked
            )
        }
        item {
            ClienteDataSection(
                isButtonLoading = uiState.isLoading,
                onConsultaInClicked = onConsultaInClicked,
                onClienteInClicked = onClienteInClicked,
            )
        }
    }
}

@Composable
private fun ScreenHeader(
    //onBackClicked: () -> Unit
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
                onClick = {} //onBackClicked
            ) {
                Icon(
                    painter = painterResource(id = R.drawable.back_icon),
                    contentDescription = "",
                    tint = Color.Unspecified
                )
            }
            Text(
                text = stringResource(R.string.tipoConsulta),
                color = MaterialTheme.colorScheme.onBackground,
                style = MaterialTheme.typography.titleMedium,
            )
        }
    }
}

@Composable
private fun ClienteDataSection(
    isButtonLoading: Boolean,
    onConsultaInClicked: () -> Unit,
    onClienteInClicked: () -> Unit,
) {
    Column(
        modifier = Modifier
            .fillMaxWidth()
            .padding(top = 28.dp)
    ) {
Text(
    modifier = Modifier.padding(bottom = 8.dp),
    text = "Es importante seleccionar el botón de Consultar o Visita Cliente. Considera seleccionar el botón de Visita " +
            "Cliente cuando llegues con algún cliente frecuente o nuevo.",
    style = MaterialTheme.typography.titleMedium,
    color = MaterialTheme.colorScheme.onSurfaceVariant
)
        Spacer(
            modifier = Modifier.height(15.dp)
        )
            MainButton(
                modifier = Modifier
                    .fillMaxWidth()
                    .height(48.dp),
                text = stringResource(R.string.tipoa),
                isLoading = isButtonLoading,
                isEnabled = true,
                onClick = onConsultaInClicked
            )
            Spacer(
                modifier = Modifier.height(15.dp)
            )
            MainButton(
                modifier = Modifier
                    .fillMaxWidth()
                    .height(48.dp),
                text = stringResource(R.string.tipob),
                isLoading = isButtonLoading,
                isEnabled = true,
                onClick = onClienteInClicked
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
            title = { Text(text = "Error De Cliente") },
            text = { Text(text = "Verifique que la clave del cliente sea correcta") },
        )
    }
}