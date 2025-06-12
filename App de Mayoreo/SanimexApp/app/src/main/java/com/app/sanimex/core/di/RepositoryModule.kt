package com.app.sanimex.core.di

import androidx.datastore.core.DataStore
import androidx.datastore.preferences.core.Preferences
import com.app.sanimex.data.remote.SanimexAPI
import com.app.sanimex.data.repository.DataStoreRepositoryImpl
import com.app.sanimex.data.repository.NetworkRepositoryImpl
import com.app.sanimex.domain.repository.DataStoreRepository
import com.app.sanimex.domain.repository.NetworkRepository
import dagger.Module
import dagger.Provides
import dagger.hilt.InstallIn
import dagger.hilt.components.SingletonComponent
import javax.inject.Singleton

/**
 * Módulo de Dagger Hilt que proporciona las implementaciones de los repositorios.
 *
 * Este módulo es responsable de crear y proveer instancias singleton de los repositorios
 * que se utilizan para acceder a diferentes fuentes de datos (red y DataStore).
 *
 * @author David Duarte
 * @version 1.0
 */
@Module
@InstallIn(SingletonComponent::class)
object RepositoryModule {

    /**
     * Proporciona una instancia singleton de [NetworkRepository].
     *
     * Esta implementación utiliza la interfaz [SanimexAPI] para interactuar con la API remota.
     *
     * @param api La interfaz [SanimexAPI] para realizar llamadas a la red.
     * @return Una instancia singleton de [NetworkRepository].
     */
    @Provides
    @Singleton
    fun provideNetworkRepository(api: SanimexAPI): NetworkRepository {
        return NetworkRepositoryImpl(api)
    }
    /**
     * Proporciona una instancia singleton de [DataStoreRepository].
     *
     * Esta implementación utiliza [DataStore]<[Preferences]> para acceder y gestionar datos persistentes localmente.
     *
     * @param dataStore La instancia de [DataStore]<[Preferences]> para acceder a los datos locales.
     * @return Una instancia singleton de [DataStoreRepository].
     */
    @Provides
    @Singleton
    fun provideDataStoreRepository(dataStore: DataStore<Preferences>): DataStoreRepository {
        return DataStoreRepositoryImpl(dataStore)
    }

}