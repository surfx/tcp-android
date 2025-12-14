Set-Location "D:\projetos\tcp-android\android"

# build completa
#.\gradlew --stop
#.\gradlew clean
#.\gradlew assembleDebug --no-build-cache
#.\gradlew clean assembleDebug --no-daemon

.\gradlew assembleDebug


# Emulador
$confirmation = Read-Host "Iniciar emulador? (y/yes/s/sim)"
$confirmation = $confirmation.ToLower().Trim()
if ($confirmation -in @('y','yes','s','sim')) {
    # Pixel_36
    Start-Process `
        -FilePath "D:\programas\executaveis\androidsdk\emulator\emulator.exe" `
        -ArgumentList "-avd Pixel_34 -wipe-data"
}

# Instalar a aplicação
& "D:\programas\executaveis\androidsdk\platform-tools\adb.exe" install -r "d:\projetos\tcp-android\android\app\build\outputs\apk\debug\app-debug.apk"

# Iniciar a aplicação
& "D:\programas\executaveis\androidsdk\platform-tools\adb.exe" shell am start -n com.example.tcpandroid/br.com.controltcpandroid.MainActivity

