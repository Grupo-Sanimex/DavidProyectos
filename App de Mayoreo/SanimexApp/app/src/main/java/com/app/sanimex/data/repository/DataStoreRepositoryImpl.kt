package com.app.sanimex.data.repository

import android.util.Log
import androidx.datastore.core.DataStore
import androidx.datastore.preferences.core.Preferences
import androidx.datastore.preferences.core.edit
import com.app.sanimex.core.util.Constants.TAG
import com.app.sanimex.domain.repository.DataStoreRepository
import kotlinx.coroutines.flow.catch
import kotlinx.coroutines.flow.firstOrNull
import kotlinx.coroutines.flow.map
import javax.inject.Inject

/**
 * Implementación de la interfaz [DataStoreRepository] utilizando [DataStore]<[Preferences]>.
 *
 * Esta clase proporciona métodos para interactuar con el DataStore, permitiendo guardar,
 * obtener y eliminar datos persistentes utilizando claves de tipo [Preferences.Key].
 *
 * @property dataStore La instancia de [DataStore]<[Preferences]> inyectada para acceder al almacenamiento de datos.
 * @author David Duarte
 * @version 1.0
 */

class DataStoreRepositoryImpl @Inject constructor(
    private val dataStore: DataStore<Preferences>
) : DataStoreRepository {

    /**
     * Guarda un valor en el DataStore asociado a una clave específica.
     *
     * Esta función suspendida utiliza la función [DataStore.edit] para realizar una transacción atómica
     * que modifica las preferencias. El valor proporcionado se asocia con la clave especificada.
     *
     * @param T El tipo del valor a guardar.
     * @param key La clave de tipo [Preferences.Key]<[T]> bajo la cual se guardará el valor.
     * @param value El valor de tipo [T] a guardar.
     */
    override suspend fun <T> saveInDataStore(key: Preferences.Key<T>, value: T) {
        dataStore.edit {
            it[key] = value
        }
    }

    /**
     * Obtiene un valor del DataStore asociado a una clave específica.
     *
     * Esta función suspendida accede al flujo de datos del DataStore, maneja posibles excepciones
     * de lectura logueándolas, mapea el resultado para obtener el valor asociado a la clave
     * y devuelve el primer valor emitido por el flujo (o nulo si el flujo está vacío o la clave no existe).
     *
     * @param T El tipo del valor a obtener.
     * @param key La clave de tipo [Preferences.Key]<[T]> del valor a obtener.
     * @return El valor de tipo [T] asociado a la clave, o nulo si la clave no existe o ocurre un error.
     */
    override suspend fun <T> getFromDataStore(key: Preferences.Key<T>): T? {
        return dataStore.data
            .catch { throwable -> Log.d(TAG, "getFromDataStore: $throwable") }
            .map { it[key] }
            .firstOrNull()
    }

    /**
     * Elimina un valor del DataStore asociado a una clave específica.
     *
     * Esta función suspendida utiliza la función [DataStore.edit] para realizar una transacción atómica
     * que modifica las preferencias, eliminando la entrada asociada a la clave especificada.
     *
     * @param T El tipo del valor a eliminar (solo se usa para la definición de la clave).
     * @param key La clave de tipo [Preferences.Key]<[T]> del valor a eliminar.
     */
    override suspend fun <T> deleteFromDataStore(key: Preferences.Key<T>) {
        dataStore.edit {
            it.remove(key)
        }
    }
}