# Net Speedy
This is a repository for testing speed improvements for the dotnet 7.

## Features
- gRPC Calls
- WASI Environment

## Requirements
- dotnet 7
- wasmtime (https://wasmtime.dev/)

## Running

```wasmtime /Users/silvan04/Source/Experiments/net-speedy/Speedy.Host/bin/Debug/net7.0/Speedy.Host.wasm --tcplisten localhost:5000 --tcplisten localhost:5001```
