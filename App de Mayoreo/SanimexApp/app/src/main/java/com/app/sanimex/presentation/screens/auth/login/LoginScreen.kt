package com.app.sanimex.presentation.screens.auth.login

import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.material3.AlertDialog
import androidx.compose.material3.Icon
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Text
import androidx.compose.material3.TextButton
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import androidx.hilt.navigation.compose.hiltViewModel
import com.app.sanimex.R
import com.app.sanimex.presentation.components.MainButton
import com.app.sanimex.presentation.components.MainTextField

@Composable
fun LoginScreen(
    viewModel: LoginViewModel = hiltViewModel(),
    onNavigateToHome: () -> Unit = {},
) {
    val uiState by viewModel.uiState.collectAsState()
    LoginScreenContent(
        uiState = uiState,
        onEmailChanged = viewModel::onEmailChanged,
        onPasswordChanged = viewModel::onPasswordChanged,
        onSignInClicked = viewModel::onLoginButtonClicked,
        onResetErrorVpn = viewModel::onResetErrorVpn
    )
    LaunchedEffect(key1 = uiState.isLoginSuccessful) {
        if (uiState.isLoginSuccessful) {
            onNavigateToHome()
        }
    }
}

@Composable
private fun LoginScreenContent(
    uiState: LoginScreenUIState,
    onEmailChanged: (String) -> Unit,
    onPasswordChanged: (String) -> Unit,
    onSignInClicked: () -> Unit,
    onResetErrorVpn: () -> Unit = {}
) {
    var showClientNull by remember { mutableStateOf(false) }
    LazyColumn(
        modifier = Modifier
            .fillMaxSize()
            .padding(16.dp)
    ) {
        item {
            LoginScreenHeader()
        }
        item {
            LoginDataSection(
                email = uiState.email,
                password = uiState.password,
                isButtonLoading = uiState.isLoading,
                onEmailChanged = onEmailChanged,
                onPasswordChanged = onPasswordChanged,
                onSignInClicked = onSignInClicked
            )
        }
        if(uiState.isError){
            item {
                Text(
                    modifier = Modifier.padding(start = 25.dp, top = 12.dp),
                    text = uiState.errorMessage,
                    color = MaterialTheme.colorScheme.secondary,
                    style = MaterialTheme.typography.labelMedium,
                    textAlign = TextAlign.Center
                )
            }
        }
        if(uiState.isErrorFailed){
            showClientNull = true
            item {
                ErrorClient(
                    show = showClientNull,
                    onDismiss = {
                        showClientNull = false
                        onResetErrorVpn()
                    },
                    onConfirm = {
                        showClientNull = false
                        onResetErrorVpn()
                    }
                )
            }
        }

    }

}

@Composable
private fun LoginScreenHeader() {
    Column(
        modifier = Modifier.fillMaxWidth(),
        horizontalAlignment = Alignment.CenterHorizontally
    ) {
        Icon(
            modifier = Modifier.padding(bottom = 16.dp, top = 68.dp),
            painter = painterResource(id = R.drawable.ic_logo_gsa),
            contentDescription = stringResource(R.string.app_icon),
            tint = Color.Unspecified
        )
        Text(
            modifier = Modifier.padding(bottom = 8.dp),
            text = stringResource(R.string.welcome_to_omnicart),
            style = MaterialTheme.typography.titleMedium,
            color = MaterialTheme.colorScheme.onBackground
        )
        Text(
            text = stringResource(R.string.sign_in_to_continue),
            style = MaterialTheme.typography.bodyMedium,
            color = MaterialTheme.colorScheme.onSurfaceVariant
        )
    }
}


@Composable
private fun LoginDataSection(
    email: String,
    password: String,
    onEmailChanged: (String) -> Unit,
    onPasswordChanged: (String) -> Unit,
    isButtonLoading: Boolean,
    onSignInClicked: () -> Unit
) {
    Column(
        modifier = Modifier
            .fillMaxWidth()
            .padding(top = 28.dp)
    ) {
        MainTextField(
            modifier = Modifier
                .fillMaxWidth()
                .height(58.dp),
            value = email,
            onValueChanged = onEmailChanged,
            placeHolder = stringResource(R.string.your_email),
            leadingIcon = R.drawable.email_icon
        )
        Spacer(
            modifier = Modifier.height(8.dp)
        )
        MainTextField(
            modifier = Modifier
                .fillMaxWidth()
                .height(58.dp),
            value = password,
            onValueChanged = onPasswordChanged,
            isPassword = true,
            placeHolder = stringResource(R.string.password),
            leadingIcon = R.drawable.password_icon
        )
        Spacer(
            modifier = Modifier.height(16.dp)
        )
        MainButton(
            modifier = Modifier
                .fillMaxWidth()
                .height(48.dp),
            text = stringResource(R.string.sign_in),
            isLoading = isButtonLoading,
            isEnabled = !isButtonLoading && email.isNotEmpty() && password.isNotEmpty(),
            onClick = onSignInClicked
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
            title = { Text(text = "Error de Internet") },
            text = { Text(text = "Accesa a internet antes de abrir la app GAM para continuar con el proceso") },
        )
    }
}

@Preview(showSystemUi = true)
@Composable
private fun LoginScreenPreview() {
    LoginScreen()
}