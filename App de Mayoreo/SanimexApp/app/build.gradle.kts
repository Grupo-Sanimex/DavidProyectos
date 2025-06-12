plugins {
    id("com.android.application")
    id("org.jetbrains.kotlin.android")
    id("kotlin-kapt")
    id("dagger.hilt.android.plugin")
   // id("org.jetbrains.dokka") version "2.0.0"
}

android {
    namespace = "com.app.sanimex"
    compileSdk = 34

    defaultConfig {
        applicationId = "com.app.sanimex"
        minSdk = 26
        targetSdk = 34
        versionCode = 9
        versionName = "1.8"

        testInstrumentationRunner = "androidx.test.runner.AndroidJUnitRunner"
        vectorDrawables {
            useSupportLibrary = true
        }
    }

    buildTypes {
        release {
            isMinifyEnabled = false
            proguardFiles(
                getDefaultProguardFile("proguard-android-optimize.txt"),
                "proguard-rules.pro"
            )
        }
    }
    compileOptions {
        sourceCompatibility = JavaVersion.VERSION_17
        targetCompatibility = JavaVersion.VERSION_17
    }
    kotlinOptions {
        jvmTarget = "17"
    }
    buildFeatures {
        compose = true
    }
    composeOptions {
        kotlinCompilerExtensionVersion = "1.4.3"
    }
    packaging {
        resources {
            excludes += "/META-INF/{AL2.0,LGPL2.1}"
        }
    }
    kapt {
        correctErrorTypes = true
    }
}





dependencies {

    implementation("androidx.core:core-ktx:1.13.1")
    implementation("androidx.lifecycle:lifecycle-runtime-ktx:2.8.6")
    implementation("androidx.activity:activity-compose:1.9.2")
    implementation(platform("androidx.compose:compose-bom:2024.09.03"))
    implementation ("androidx.compose.runtime:runtime")
    implementation("androidx.compose.ui:ui")
    implementation("androidx.compose.ui:ui-graphics")
    implementation("androidx.compose.ui:ui-tooling-preview")
    implementation("androidx.compose.material3:material3")
    implementation("com.google.android.gms:play-services-location:21.3.0")
    testImplementation("junit:junit:4.13.2")
    androidTestImplementation("androidx.test.ext:junit:1.2.1")
    androidTestImplementation("androidx.test.espresso:espresso-core:3.6.1")
    androidTestImplementation(platform("androidx.compose:compose-bom:2024.09.03"))
    androidTestImplementation("androidx.compose.ui:ui-test-junit4")
    debugImplementation("androidx.compose.ui:ui-tooling")
    debugImplementation("androidx.compose.ui:ui-test-manifest")

    // solicita permisos de ubicación
    implementation("com.google.accompanist:accompanist-permissions:0.31.1-alpha")

    // Coroutines
    implementation("org.jetbrains.kotlinx:kotlinx-coroutines-core:1.7.3")
    implementation("org.jetbrains.kotlinx:kotlinx-coroutines-android:1.7.3")

    //Dagger - Hilt
    implementation("com.google.dagger:hilt-android:2.50")

    kapt("com.google.dagger:hilt-android-compiler:2.45")
    kapt("androidx.hilt:hilt-compiler:1.2.0")

    implementation("androidx.hilt:hilt-navigation-compose:1.2.0")

    // Retrofit
    implementation("com.squareup.retrofit2:retrofit:2.9.0")
    implementation("com.squareup.retrofit2:converter-gson:2.9.0")
    implementation("com.squareup.okhttp3:okhttp:5.0.0-alpha.2")
    implementation("com.squareup.okhttp3:logging-interceptor:5.0.0-alpha.2")

    //Lottie
    implementation("com.airbnb.android:lottie-compose:6.0.1")

    //Splash Screen API
    implementation("androidx.core:core-splashscreen:1.0.1")

    //DataStore
    implementation("androidx.datastore:datastore-preferences:1.1.1")

    //Easy Permissions
    implementation("pub.devrel:easypermissions:3.0.0")

    //coil
    implementation("io.coil-kt:coil-compose:2.4.0")

    // Pager and Indicators - Accompanist
    implementation("com.google.accompanist:accompanist-pager:0.33.0-alpha")
    implementation("com.google.accompanist:accompanist-pager-indicators:0.33.0-alpha")

    //Constraint Layout
    implementation ("androidx.constraintlayout:constraintlayout-compose:1.0.1")

    //Compose Navigation
    implementation ("androidx.navigation:navigation-compose:2.8.2")

    implementation ("androidx.compose.animation:animation:1.7.3")

    // maps
    implementation ("com.google.maps.android:maps-compose:2.11.4")
    implementation ("com.google.android.gms:play-services-maps:18.1.0")
    implementation ("org.jetbrains.kotlinx:kotlinx-coroutines-play-services:1.6.4")

    // dokka
    //implementation("org.jetbrains.dokka:dokka-gradle-plugin:2.0.0")

}