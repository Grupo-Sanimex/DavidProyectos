package com.app.sanimex.core.util

import android.annotation.SuppressLint
import android.util.Log
import com.app.sanimex.core.util.Constants.TAG
import java.text.ParseException
import java.text.SimpleDateFormat
import java.util.Calendar
import java.util.Locale
import java.util.TimeZone

/**
 * Objeto utilitario para analizar y formatear cadenas de fecha en un formato legible.
 *
 * Utiliza un [SimpleDateFormat] interno para realizar el análisis desde el formato ISO 8601
 * y luego formatea la fecha resultante en un formato más amigable para el usuario
 * (ejemplo: "May 5, 2025").
 *
 * @author David Duarte
 * @version 1.0
 */
object DateParser {
    /**
     * Formateador de fecha interno configurado para analizar cadenas en formato "yyyy-MM-dd'T'HH:mm:ss.SSS'Z'" (ISO 8601).
     *
     * Se ignora la advertencia [SuppressLint] ya que se controla el uso de este formateador dentro de este objeto.
     */
    @SuppressLint("SimpleDateFormat")
    private val dateFormat = SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss.SSS'Z'")
    /**
     * Analiza una cadena de fecha en formato ISO 8601 y la formatea en un formato legible.
     *
     * Intenta analizar la cadena de fecha utilizando el [dateFormat] configurado en UTC.
     * Si el análisis es exitoso, extrae el año, el nombre del mes y el día, y los formatea
     * en una cadena como "NombreDelMes Día, Año" (ejemplo: "May 5, 2025").
     * Si ocurre una [ParseException] durante el análisis, se registra un error en el log
     * y se devuelve una cadena vacía.
     *
     * @param dateString La cadena de fecha en formato "yyyy-MM-dd'T'HH:mm:ss.SSS'Z'" a analizar.
     * @return Una cadena de fecha formateada en formato "NombreDelMes Día, Año", o una cadena vacía si ocurre un error.
     */
    fun parseDate(dateString: String): String {
        var result = ""
        try {
            dateFormat.timeZone = TimeZone.getTimeZone("UTC")
            val date = dateFormat.parse(dateString)
            val calendar = Calendar.getInstance()
            date?.let {
                calendar.time = it
            }
            val year = calendar.get(Calendar.YEAR)
            val monthName = date?.let { SimpleDateFormat("MMMM", Locale.getDefault()).format(it) }
            val day = calendar.get(Calendar.DAY_OF_MONTH)
            monthName?.let { result = "$monthName $day, $year" }
        } catch (e: ParseException) {
            Log.d(TAG, "parseDate: ${e.cause}")
        }
        return result
    }
}