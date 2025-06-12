package com.app.sanimex.presentation.components

import androidx.compose.foundation.text.KeyboardOptions
import androidx.compose.material3.ExperimentalMaterial3Api
import androidx.compose.material3.Icon
import androidx.compose.material3.IconButton
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
import androidx.compose.ui.text.input.PasswordVisualTransformation
import androidx.compose.ui.text.input.VisualTransformation
import androidx.compose.ui.tooling.preview.Preview
import com.app.sanimex.R

/**
 * Composable que representa un campo de texto personalizado con varias opciones de configuración.
 *
 * Este campo de texto basado en [OutlinedTextField] de Material Design permite configurar
 * el valor, la acción al cambiar el valor, el texto de marcador de posición, íconos (leading y trailing),
 * mensajes de error, modo de una sola línea, acción del teclado IME, si es un campo de contraseña
 * y si está habilitado. También incluye lógica para mostrar/ocultar la contraseña.
 *
 * @param modifier Modificador para personalizar el layout del campo de texto (por defecto, un [Modifier] vacío).
 * @param value El valor actual del texto en el campo.
 * @param onValueChanged Función lambda que se llama cuando el valor del texto cambia, recibiendo el nuevo valor.
 * @param placeHolder Texto a mostrar cuando el campo está vacío y no tiene el foco.
 * @param leadingIcon Recurso de ícono (Int) a mostrar al inicio del campo de texto (opcional).
 * @param trailingIcon Recurso de ícono (Int) a mostrar al final del campo de texto (opcional). Si `isPassword` es true, se mostrará un ícono de mostrar/ocultar contraseña en lugar de este.
 * @param error Mensaje de error a mostrar debajo del campo de texto si no es nulo. Esto también cambia el estilo del borde.
 * @param singleLine Booleano que indica si el campo de texto debe limitarse a una sola línea (por defecto, true).
 * @param imeAction La acción del teclado IME a configurar (por defecto, [ImeAction.Next]).
 * @param isPassword Booleano que indica si este campo debe ocultar el texto como una contraseña (por defecto, false). Muestra un ícono para mostrar/ocultar.
 * @param enabled Booleano que indica si el campo de texto está habilitado para la interacción del usuario (por defecto, true).
 * @param keyboardType El tipo de teclado a mostrar (por defecto, [KeyboardType.Text]).
 * @author Tu Nombre
 * @version 1.0
 */
@Preview
@Composable
fun MainTextField(
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
    var isShowPassword: Boolean by remember { mutableStateOf(false) }
    var visualTransformation: VisualTransformation by remember { mutableStateOf(VisualTransformation.None) }
    visualTransformation =
        if (isPassword && !isShowPassword) PasswordVisualTransformation() else VisualTransformation.None

    OutlinedTextField(
        modifier = modifier.onFocusChanged { isFocused = it.isFocused },
        value = value,
        enabled = enabled,
        onValueChange = { newValue ->
            // Usar una expresión regular para eliminar caracteres no permitidos y espacios vacíos
            val filteredValue = newValue.replace(Regex("[&'\\$\\s]"), "")
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
        trailingIcon = if (trailingIcon != null || isPassword) {
            {
                if (trailingIcon != null) {
                    Icon(
                        painter = painterResource(id = trailingIcon),
                        contentDescription = "",
                    )
                } else {
                    if (isShowPassword) {
                        visualTransformation = PasswordVisualTransformation()
                        IconButton(
                            onClick = {
                                isShowPassword = !isShowPassword

                            }
                        ) {
                            Icon(
                                painter = painterResource(id = R.drawable.eye_open_icon),
                                contentDescription = "",
                                tint = MaterialTheme.colorScheme.onSurfaceVariant,
                            )
                        }
                    } else {
                        visualTransformation = VisualTransformation.None
                        IconButton(
                            onClick = {
                                isShowPassword = !isShowPassword

                            }
                        ) {
                            Icon(
                                painter = painterResource(id = R.drawable.eye_closed_icon),
                                contentDescription = "",
                                tint = MaterialTheme.colorScheme.onSurfaceVariant,
                            )
                        }
                    }
                }
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
        visualTransformation = visualTransformation
    )
}