package com.app.sanimex.presentation.components


import androidx.compose.foundation.text.KeyboardOptions
import androidx.compose.material3.ExperimentalMaterial3Api
import androidx.compose.material3.Icon

import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.OutlinedTextField
import androidx.compose.material3.OutlinedTextFieldDefaults
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.compose.ui.Modifier
import androidx.compose.ui.focus.onFocusChanged
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.text.input.ImeAction
import androidx.compose.ui.text.input.KeyboardType
import androidx.compose.ui.tooling.preview.Preview

@Preview
@Composable
fun MainTextFieldSearch(
    modifier: Modifier = Modifier,
    value: String = "",
    onValueChanged: (String) -> Unit = {},
    placeHolder: String = "",
    leadingIcon: Int? = null,
    trailingIcon: Int? = null,
    error: String? = null,
    singleLine: Boolean = true,
    imeAction: ImeAction = ImeAction.Next,
    isPassword: Boolean = false,
    enabled: Boolean = true,
    keyboardType:KeyboardType = KeyboardType.Text
) {
    var isFocused: Boolean by remember { mutableStateOf(false) }

    OutlinedTextField(
        modifier = modifier.onFocusChanged { isFocused = it.isFocused },
        value = value,
        enabled = enabled,
        onValueChange = { newValue ->
            // Filtrar caracteres no permitidos '&' y '\''
            // Usar una expresión regular para eliminar acentos, caracteres de operaciones aritméticas y caracteres no permitidos
            val filteredValue = newValue.replace(Regex("[&'\\$+\\*@=^%<>,.;:()_!?|\\[\\]{}]"), "")
                .replace(Regex("[\\p{InCombiningDiacriticalMarks}]"), "") // Eliminar los diacríticos (acentos)
            onValueChanged(filteredValue)
        },
        isError = error != null,
        singleLine = singleLine,
        leadingIcon = if (leadingIcon != null) {
            {
                Icon(
                    painter = painterResource(id = leadingIcon),
                    contentDescription = "",
                    tint = if (isFocused) MaterialTheme.colorScheme.primary else Color.Unspecified
                )
            }
        } else null,
        placeholder = {
            Text(
                text = placeHolder,
                color = MaterialTheme.colorScheme.onSurfaceVariant,
                style = MaterialTheme.typography.bodySmall
            )
        },
        colors = OutlinedTextFieldDefaults.colors(
            disabledTextColor = MaterialTheme.colorScheme.onSurfaceVariant,
            focusedBorderColor = MaterialTheme.colorScheme.primary,
            unfocusedBorderColor = MaterialTheme.colorScheme.outline,
            disabledBorderColor = MaterialTheme.colorScheme.outline,
            errorBorderColor = MaterialTheme.colorScheme.secondary,
            focusedTextColor = MaterialTheme.colorScheme.onBackground,
            unfocusedTextColor = MaterialTheme.colorScheme.onBackground
        ),
        keyboardOptions = KeyboardOptions(imeAction = imeAction, keyboardType = keyboardType),
    )
}