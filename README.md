# Net Speedy
This is a repository for testing speed improvements for the dotnet 7.

## Features
- gRPC Calls
- WASI Environment

## Requirements
- dotnet 7
- wasmtime (https://wasmtime.dev/)

## Running

```wasmtime /Speedy.Host/bin/Debug/net7.0/Speedy.Host.wasm --tcplisten localhost:5000 --tcplisten localhost:5001```

## Testing
This was tested without WASM runtime.
- [net7.0-preview6](https://askpt.github.io/net-speedy/summary_net7.0.0_pre6.html)
- [net7.0-preview7](https://askpt.github.io/net-speedy/summary_net7.0.0_pre7.html)
- [net7.0-rc1](https://askpt.github.io/net-speedy/summary_net7.0.0_rc1.html)
- [net7.0](https://askpt.github.io/net-speedy/summary_net7.0.0.html)
