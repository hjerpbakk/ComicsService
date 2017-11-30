#!/bin/bash
rm -r ./publish
set -e
dotnet publish Hjerpbakk.ComicsService/Hjerpbakk.ComicsService.csproj -o ../publish -c Release
docker build -t comics .
# Run container locally
# docker run -p 5000:80 comics