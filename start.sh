#!/bin/bash

# Navigate to the RealData project folder
cd "BDZPrototype(RealData)"

# Restore NuGet packages
dotnet restore

# Build the project
dotnet build

# Run the project on the port Railway provides
dotnet run --urls "http://0.0.0.0:$PORT"
