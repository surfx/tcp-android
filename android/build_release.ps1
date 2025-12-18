$ErrorActionPreference = "Stop"

# ===== CONFIGURAÇÃO =====
$projectDir = "D:\projetos\tcp-android\android"
$apkPath    = "$projectDir\app\build\outputs\apk\release\app-release.apk"
$adbDir     = "D:\programas\executaveis\androidsdk\platform-tools"
$gradleCmd  = ".\gradlew"

try {
    # ===== BUILD =====
    Push-Location $projectDir

    Write-Host "🧹 Parando Daemons do Gradle para liberar arquivos..."
    & $gradleCmd --stop

    Write-Host "🚧 Gerando APK Release..."
    & $gradleCmd clean assembleRelease

    if ($LASTEXITCODE -ne 0) {
        throw "Build falhou"
    }
    Pop-Location

    # ===== VERIFICA APK =====
    Write-Host "📦 Verificando APK..."
    if (!(Test-Path $apkPath)) {
        throw "APK não encontrado: $apkPath"
    }

    Write-Host "APK gerado em:"
    Write-Host $apkPath

    # ===== CONFIRMAÇÃO =====
    $confirmation = Read-Host "Instalar no dispositivo? (y/yes/s/sim)"
    $confirmation = $confirmation.ToLower().Trim()

    if ($confirmation -notin @('y','yes','s','sim')) {
        Write-Host "❌ Execução cancelada pelo usuário."
        Exit 0
    }

    # ===== ADB =====
    Push-Location $adbDir

    if (!(Test-Path ".\adb.exe")) {
        throw "adb.exe não encontrado em $adbDir"
    }

    Write-Host "📱 Dispositivos conectados:"
    .\adb devices

    Write-Host "⬇ Instalando APK..."
    .\adb install -r $apkPath

    Pop-Location

    Write-Host "✅ Instalação concluída com sucesso!"

}
catch {
    Write-Error "Erro: $_"
    Exit 1
}
