using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Speedy.Host;
using Speedy.Host.Services;

var builder = WebApplication.CreateBuilder(args);

// This is for the WASI runtime.
// builder.UseWasiConnectionListener();

// Hack for HTTP/2 TLS for macOS
builder.WebHost.ConfigureKestrel(options =>
{
    // Setup a HTTP/2 endpoint without TLS.
    options.ListenLocalhost(5000, o => o.Protocols = HttpProtocols.Http2);

    // Setup a HTTP/2 endpoint without TLS.
    options.ListenLocalhost(5001, o => o.Protocols = HttpProtocols.Http1);
});

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc().AddJsonTranscoding();
builder.Services.AddScoped<IRevertService, RevertService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGet("/v2/greeter/{name}", (string name, [FromServices] IRevertService revertService) => new HelloReply
{
    Message = revertService.Revert(name)
});
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
