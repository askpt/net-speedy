using System.Diagnostics;
using System.Text.Json;
using Grpc.Net.Client;
using Speedy.Host;

AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

const string Name = "Speedy";

await DoGrpcJsonTranscodingCall(Name);
await DoGrpcJsonTranscodingHttpCall(Name);
await DoHttpCall(Name);
await DoNativeGrpcCall(Name);

Console.WriteLine("Press any key to exit...");
Console.ReadKey();

async Task DoGrpcJsonTranscodingCall(string name)
{
    var httpHandler = new HttpClientHandler
    {
        // Return `true` to allow certificates that are untrusted/invalid
        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    };

    var stopWatch = new Stopwatch();
    stopWatch.Start();

    var channel = GrpcChannel.ForAddress("http://localhost:5000", new GrpcChannelOptions { HttpHandler = httpHandler });
    var client = new Greeter.GreeterClient(channel);

    var reply = await client.SayHelloAsync(new HelloRequest { Name = name });

    stopWatch.Stop();
    var time = stopWatch.ElapsedMilliseconds;
    Console.WriteLine($"Operation {nameof(DoGrpcJsonTranscodingCall)} took {time}ms. Response: {reply.Message}");
}

async Task DoNativeGrpcCall(string name)
{
    var httpHandler = new HttpClientHandler
    {
        // Return `true` to allow certificates that are untrusted/invalid
        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    };

    var stopWatch = new Stopwatch();
    stopWatch.Start();

    var channel = GrpcChannel.ForAddress("http://localhost:5000", new GrpcChannelOptions { HttpHandler = httpHandler });
    var client = new Greeter.GreeterClient(channel);

    var reply = await client.SayHelloAgainAsync(new HelloRequest { Name = name });

    stopWatch.Stop();
    var time = stopWatch.ElapsedMilliseconds;
    Console.WriteLine($"Operation {nameof(DoNativeGrpcCall)} took {time}ms. Response: {reply.Message}");
}

async Task DoGrpcJsonTranscodingHttpCall(string name)
{
    var stopWatch = new Stopwatch();
    stopWatch.Start();

    // Do a Http Call
    var httpClient = new HttpClient
    {
        DefaultRequestVersion = Version.Parse("2.0"),
        DefaultVersionPolicy = HttpVersionPolicy.RequestVersionExact
    };
    var response = await httpClient.GetAsync($"http://localhost:5001/v1/greeter/{name}");
    var content = await response.Content.ReadAsStringAsync();
    var reply = JsonSerializer.Deserialize<HelloReply>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

    stopWatch.Stop();
    var time = stopWatch.ElapsedMilliseconds;
    Console.WriteLine($"Operation {nameof(DoGrpcJsonTranscodingHttpCall)} took {time}ms. Response: {reply!.Message}");
}

async Task DoHttpCall(string name)
{
    var stopWatch = new Stopwatch();
    stopWatch.Start();

    // Do a Http Call
    var httpClient = new HttpClient
    {
        DefaultRequestVersion = Version.Parse("2.0"),
        DefaultVersionPolicy = HttpVersionPolicy.RequestVersionExact
    };
    var response = await httpClient.GetAsync($"http://localhost:5001/v2/greeter/{name}");
    var content = await response.Content.ReadAsStringAsync();
    var reply = JsonSerializer.Deserialize<HelloReply>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

    stopWatch.Stop();
    var time = stopWatch.ElapsedMilliseconds;
    Console.WriteLine($"Operation {nameof(DoHttpCall)} took {time}ms. Response: {reply!.Message}");
}
