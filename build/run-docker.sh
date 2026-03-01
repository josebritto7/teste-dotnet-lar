#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"

IMAGE_NAME="${IMAGE_NAME:-lar-testedotnet:latest}"
CONTAINER_NAME="${CONTAINER_NAME:-lar-testedotnet-api}"
HOST_PORT="${HOST_PORT:-5215}"
ASPNETCORE_ENVIRONMENT="${ASPNETCORE_ENVIRONMENT:-Development}"
DB_VOLUME="${DB_VOLUME:-lar_testedotnet_data}"

printf '[1/3] Buildando imagem: %s\n' "${IMAGE_NAME}"
docker build -f "${ROOT_DIR}/build/Dockerfile" -t "${IMAGE_NAME}" "${ROOT_DIR}"

if docker ps -a --format '{{.Names}}' | grep -Fxq "${CONTAINER_NAME}"; then
  printf '[2/3] Removendo Container Existente: %s\n' "${CONTAINER_NAME}"
  docker rm -f "${CONTAINER_NAME}" >/dev/null
else
  printf '[2/3] Nenhum Container encontrado\n'
fi

printf '[3/3] Iniciando Container: %s\n' "${CONTAINER_NAME}"
container_id=$(docker run -d --rm \
  -p "${HOST_PORT}:8080" \
  -e "ASPNETCORE_ENVIRONMENT=${ASPNETCORE_ENVIRONMENT}" \
  -e "ConnectionStrings__DefaultConnection=Data Source=/app/data/lar.testedotnet.db" \
  -v "${DB_VOLUME}:/app/data" \
  --name "${CONTAINER_NAME}" \
  "${IMAGE_NAME}")

printf 'Container iniciado em modo detach.\n'
printf 'Container ID: %s\n' "${container_id}"
printf 'Swagger: http://localhost:%s/swagger\n' "${HOST_PORT}"
printf 'Logs: docker logs -f %s\n' "${CONTAINER_NAME}"
printf 'Stop: docker stop %s\n' "${CONTAINER_NAME}"
