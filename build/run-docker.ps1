param(
    [string]$ImageName = "lar-testedotnet:latest",
    [string]$ContainerName = "lar-testedotnet-api",
    [int]$HostPort = 5215,
    [string]$AspNetCoreEnvironment = "Development",
    [string]$DbVolume = "lar_testedotnet_data"
)

$ErrorActionPreference = "Stop"
$RootDir = Split-Path -Parent $PSScriptRoot

Write-Host "[1/3] Buildando imagem: $ImageName"
docker build -f "$RootDir/build/Dockerfile" -t $ImageName $RootDir

$existingContainer = docker ps -a --format "{{.Names}}" | Where-Object { $_ -eq $ContainerName }
if ($existingContainer) {
    Write-Host "[2/3] Removendo Container Existente: $ContainerName"
    docker rm -f $ContainerName | Out-Null
}
else {
    Write-Host "[2/3] Nenhum Container encontrado"
}

Write-Host "[3/3] Iniciando Container: $ContainerName"
$containerId = docker run -d --rm `
  -p "${HostPort}:8080" `
  -e "ASPNETCORE_ENVIRONMENT=$AspNetCoreEnvironment" `
  -e "ConnectionStrings__DefaultConnection=Data Source=/app/data/lar.testedotnet.db" `
  -v "${DbVolume}:/app/data" `
  --name $ContainerName `
  $ImageName

Write-Host "Container iniciado em modo detach."
Write-Host "Container ID: $containerId"
Write-Host "Swagger: http://localhost:$HostPort/swagger"
Write-Host "Logs: docker logs -f $ContainerName"
Write-Host "Stop: docker stop $ContainerName"
