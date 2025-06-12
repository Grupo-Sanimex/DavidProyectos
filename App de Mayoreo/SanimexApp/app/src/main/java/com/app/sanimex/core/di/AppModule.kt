package com.app.sanimex.core.di
import android.content.Context
import androidx.datastore.core.DataStore
import androidx.datastore.preferences.core.Preferences
import androidx.datastore.preferences.preferencesDataStore
import com.app.sanimex.core.util.Constants
import com.app.sanimex.core.util.Constants.APP_DATA_STORE_NAME
import com.app.sanimex.data.remote.SanimexAPI
import dagger.Module
import dagger.Provides
import dagger.hilt.InstallIn
import dagger.hilt.android.qualifiers.ApplicationContext
import dagger.hilt.components.SingletonComponent
import okhttp3.OkHttpClient
import okhttp3.logging.HttpLoggingInterceptor
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory
import java.util.concurrent.TimeUnit
import javax.inject.Singleton

/**
 * Módulo de Dagger Hilt que proporciona dependencias a nivel de aplicación.
 *
 * @author David Duarte
 * @version 1.7
 */
private val Context.appDataStore by preferencesDataStore(name = APP_DATA_STORE_NAME)
@Module
@InstallIn(SingletonComponent::class)
object AppModule {

    /**
     * Proporciona una instancia singleton de [OkHttpClient] configurada para la comunicación HTTP.
     *
     * Esta configuración incluye:
     * - Un interceptor de logging para registrar el cuerpo de las peticiones y respuestas (actualmente deshabilitado).
     * - Un interceptor para agregar encabezados de `Accept: application/json` y `Authorization: Bearer <token>`.
     * - Configuración de timeouts de lectura, escritura y llamada a 60 segundos.
     *
     * @return Una instancia singleton de [OkHttpClient].
     */
    @Provides
    @Singleton
    fun provideHTTPClient(): OkHttpClient {
        val httpClientLoggingInterceptor = HttpLoggingInterceptor {
        }
        httpClientLoggingInterceptor.level = HttpLoggingInterceptor.Level.BODY

        return OkHttpClient().newBuilder().apply {
            addInterceptor { chain ->
                val newRequest = chain.request().newBuilder()
                newRequest.addHeader("Accept", "application/json")
                newRequest.addHeader("Authorization", "Bearer ${Constants.userToken}")
                chain.proceed(newRequest.build())
            }
            addInterceptor(httpClientLoggingInterceptor)
            readTimeout(60, TimeUnit.SECONDS)
            writeTimeout(60, TimeUnit.SECONDS)
            callTimeout(60, TimeUnit.SECONDS)
        }.build()
    }
    /**
     * Proporciona una instancia singleton de la interfaz [SanimexAPI] para interactuar con la API de Sanimex.
     *
     * Utiliza [Retrofit] para construir la instancia, configurando la URL base (actualmente la de producción),
     * un convertidor de Gson para la serialización/deserialización de datos y el cliente [OkHttpClient] proporcionado.
     *
     * @param client La instancia de [OkHttpClient] utilizada para las peticiones HTTP.
     * @return Una instancia singleton de [SanimexAPI].
     */
    @Provides
    @Singleton
    fun provideAPI(client: OkHttpClient): SanimexAPI {
        return Retrofit.Builder()
            // productivo
            .baseUrl("http://200.57.183.111:60801/api/")
            // desarrollo
            //.baseUrl("http://192.168.5.1:8080/api/")
            .addConverterFactory(GsonConverterFactory.create())
            .client(client)
            .build()
            .create(SanimexAPI::class.java)
    }

    @Provides
    @Singleton
    fun provideDataStore(@ApplicationContext context: Context): DataStore<Preferences> {
        return context.appDataStore
    }

}

