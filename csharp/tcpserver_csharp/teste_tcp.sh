#!/bin/bash

cd "$(dirname "$0")" || exit 1

if [ $# -eq 0 ]; then
    echo "Testes offline do protocolo TCP (52 testes)"
    echo ""
    echo "Exemplos:"
    echo "  ./teste_tcp.sh                              -> testes offline"
    echo "  ./teste_tcp.sh --server 127.0.0.1 9876      -> testes integracao"
    echo "  ./teste_tcp.sh --client 127.0.0.1 9876 sync -> modo cliente"
    echo ""
fi

rm -rf obj testes/obj 2>/dev/null
dotnet run --project testes/teste_tcp.csproj -- "$@"
