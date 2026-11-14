plugins {
    id("com.android.application")
    id("org.jetbrains.kotlin.android")
    id("org.jetbrains.kotlin.plugin.compose")
}

android {
    namespace = "com.osaka.tv"
    compileSdk = 35

    defaultConfig {
        applicationId = "com.osaka.tv"
        minSdk = 26
        targetSdk = 35
        versionCode = 1000 + (System.getenv("GITHUB_RUN_NUMBER")?.toIntOrNull() ?: 0)
        versionName = "1.0.${System.getenv("GITHUB_RUN_NUMBER") ?: "0"}"
    }

    compileOptions {
        sourceCompatibility = JavaVersion.VERSION_17
        targetCompatibility = JavaVersion.VERSION_17
    }

    signingConfigs {
        create("release") {
            val storePath = System.getenv("OSAKATV_KEYSTORE_PATH")
            if (!storePath.isNullOrBlank()) {
                storeFile = file(storePath)
                storePassword = System.getenv("OSAKATV_KEYSTORE_PASSWORD")
                keyAlias = System.getenv("OSAKATV_KEY_ALIAS")
                keyPassword = System.getenv("OSAKATV_KEY_PASSWORD")
            }
        }
    }

    buildTypes {
        getByName("release") {
            signingConfig = signingConfigs.getByName("release")
            isMinifyEnabled = false
        }
    }

    buildFeatures { compose = true }
}

kotlin { jvmToolchain(17) }

dependencies {
    implementation(platform("androidx.compose:compose-bom:2024.10.00"))
    implementation("androidx.compose.ui:ui")
    implementation("androidx.compose.material3:material3")
    implementation("androidx.compose.ui:ui-tooling-preview")
    implementation("androidx.activity:activity-compose:1.9.3")
    implementation("androidx.core:core-ktx:1.15.0")
    implementation("androidx.lifecycle:lifecycle-runtime-ktx:2.8.7")
    implementation("androidx.media3:media3-exoplayer:1.4.1")
    implementation("androidx.media3:media3-exoplayer-hls:1.4.1")
    implementation("androidx.media3:media3-ui:1.4.1")
}
