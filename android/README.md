# Build

```powershell
cd D:\projetos\tcp-android\android
.\gradlew.bat assembleDebug
```

# Emulador

```powershell
& "D:\programas\executaveis\androidsdk\emulator\emulator.exe" -avd Pixel_36 -wipe-data
```

# Instalar a aplicação

```powershell
& "D:\programas\executaveis\androidsdk\platform-tools\adb.exe" install -r "d:\projetos\tcp-android\android\app\build\outputs\apk\debug\app-debug.apk"
```

## Iniciar a aplicação

```powershell
& "D:\programas\executaveis\androidsdk\platform-tools\adb.exe" shell am start -n com.example.tcpandroid/br.com.controltcpandroid.MainActivity
```

# Build final - sem assinatura

```powershell
.\gradlew assembleRelease
```

Vai criar o `app-release-unsigned.apk`:

`"D:\projetos\tcp-android\android\app\build\outputs\apk\release\app-release-unsigned.apk"`

# Build final - com assinatura

## Gerar a chave:

Obs: a senha é `root123`

```powershell
cd D:\projetos\tcp-android\android\chave_apk
keytool -genkeypair -v -keystore my-release-key.jks -alias mykey -keyalg RSA -keysize 2048 -validity 10000 -storepass root123
```

Vai criar o arquivo `D:\projetos\tcp-android\android\chave_apk\my-release-key.jks`

Altere o arquivo: `D:\projetos\tcp-android\android\app\build.gradle`

```gradle
...
android {
    ...

    signingConfigs {
        release {
            storeFile file("../chave_apk/my-release-key.jks")
            storePassword "root123"
            keyAlias "mykey"
            keyPassword "root123"
        }
    }

    buildTypes {
        release {
            minifyEnabled false
            shrinkResources false
            proguardFiles getDefaultProguardFile('proguard-android-optimize.txt'), 'proguard-rules.pro'
            signingConfig signingConfigs.release
        }
    }
    compileOptions {
        sourceCompatibility JavaVersion.VERSION_1_8
        targetCompatibility JavaVersion.VERSION_1_8
    }
}
...
```

```powershell
.\gradlew assembleRelease
```

Vai criar o `app-release.apk`:

`"D:\projetos\tcp-android\android\app\build\outputs\apk\release\app-release.apk"`

# AAB (Play Store)

```powershell
.\gradlew bundleRelease
```

Vai criar o `pp-release.aab`:

`""D:\projetos\tcp-android\android\app\build\outputs\bundle\release\app-release.aab""`

# Resumo builds

| Objetivo         | Comando           |
| ---------------- | ----------------- |
| APK debug        | `assembleDebug`   |
| APK release      | `assembleRelease` |
| AAB (Play Store) | `bundleRelease`   |


# Install apk celular

No celular

1. Ativar Opções do desenvolvedor
2. Ativar Depuração USB
3. Conectar o celular no USB e autorizar o PC quando aparecer o popup

```powershell
cd "D:\programas\executaveis\androidsdk\platform-tools"
.\adb devices
```

```
List of devices attached
ABC123456	device
```

Se aparecer `unauthorized`, olhe a tela do celular e aceite.

```powershell
.\adb install -r "D:\projetos\tcp-android\android\app\build\outputs\apk\release\app-release.apk"
```

