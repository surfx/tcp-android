#!/bin/bash

cd "$(dirname "$0")" || exit 1
rm -rf obj 2>/dev/null
dotnet run
