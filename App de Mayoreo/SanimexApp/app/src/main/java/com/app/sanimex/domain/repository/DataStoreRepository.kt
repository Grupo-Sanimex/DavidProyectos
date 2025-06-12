package com.app.sanimex.domain.repository

import androidx.datastore.preferences.core.Preferences
import kotlinx.coroutines.flow.Flow

/**
 * Interfaz que define las operaciones básicas para interactuar con el DataStore.
 *
 * Esta interfaz proporciona métodos abstractos para guardar, obtener y eliminar datos
 * persistentes utilizando claves de tipo [Preferences.Key]. Las implementaciones de esta
 * interfaz se encargarán de la lógica específica para interactuar con el DataStore.
 *
 * @author David Duarte
 * @version 1.0
 */
interface DataStoreRepository {
    /**
     * Guarda un valor en el DataStore asociado a una clave específica.
     *
     * Esta función suspendida debe implementar la lógica para guardar el valor proporcionado
     * bajo la clave especificada en el DataStore.
     *
     * @param T El tipo del valor a guardar.
     * @param key La clave de tipo [Preferences.Key]<[T]> bajo la cual se guardará el valor.
     * @param value El valor de tipo [T] a guardar.
     */
    suspend fun <T> saveInDataStore(key: Preferences.Key<T>, value: T)
    /**
     * Obtiene un valor del DataStore asociado a una clave específica.
     *
     * Esta función suspendida debe implementar la lógica para recuperar el valor asociado
     * a la clave especificada desde el DataStore. Si la clave no existe o no hay valor asociado,
     * debe devolver nulo.
     *
     * @param T El tipo del valor a obtener.
     * @param key La clave de tipo [Preferences.Key]<[T]> del valor a obtener.
     * @return El valor de tipo [T] asociado a la clave, o nulo si no se encuentra.
     */
    suspend fun <T> getFromDataStore(key: Preferences.Key<T>): T?
    /**
     * Elimina un valor del DataStore asociado a una clave específica.
     *
     * Esta función suspendida debe implementar la lógica para eliminar la entrada
     * asociada a la clave especificada del DataStore.
     *
     * @param T El tipo del valor a eliminar (solo se usa para la definición de la clave).
     * @param key La clave de tipo [Preferences.Key]<[T]> del valor a eliminar.
     */
    suspend fun <T> deleteFromDataStore(key: Preferences.Key<T>)
}