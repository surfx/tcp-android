#!/bin/bash
# Script para executar o projeto de testes do TCP Server
# Modos de uso:
#   ./teste_tcp.sh                        - Testes de protocolo (offline)
#   ./teste_tcp.sh --server [ip] [port]    - Testes de integração (requer servidor)
#   ./teste_tcp.sh --client [ip] [port] [cmd] [args...] - Modo cliente
#
# Comandos: sync, volume <valor>, shutdown, mouse <wc hc x y>, click, lock, up, down, left, right

cd "$(dirname "$0")" || exit 1
dotnet run -- "$@"
