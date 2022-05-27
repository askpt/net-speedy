using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using Grpc.Net.Client;
using Speedy.Host;

AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

const string Name = "Speedy";

await DoGrpcCall(Name);
await DoGrpcHttpCall(Name);

Console.WriteLine("Shutting down");
Console.WriteLine("Press any key to exit...");
Console.ReadKey();

async Task DoGrpcCall(string name)
{
    var stopWatch = new Stopwatch();
    stopWatch.Start();

    var httpHandler = new HttpClientHandler();
    // Return `true` to allow certificates that are untrusted/invalid
    httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

    var channel = GrpcChannel.ForAddress("http://localhost:5000", new GrpcChannelOptions { HttpHandler = httpHandler });
    var client = new Greeter.GreeterClient(channel);

    var reply = await client.SayHelloAsync(new HelloRequest { Name = name });
    Console.WriteLine("Greeting: " + reply.Message);

    stopWatch.Stop();
    var time = stopWatch.ElapsedMilliseconds;
    Console.WriteLine($"Operation DoGrpcCall took {time}ms");
}

async Task DoGrpcHttpCall(string name)
{
    var stopWatch = new Stopwatch();
    stopWatch.Start();

    // Do a Http Call
    var httpClient = new HttpClient();
    var response = await httpClient.GetAsync($"http://localhost:5001/v1/greeter/{name}");
    var content = await response.Content.ReadAsStreamAsync();
    var reply = await JsonSerializer.DeserializeAsync<HelloReply>(content);
    Console.WriteLine("Greeting: " + reply!.Message);

    stopWatch.Stop();
    var time = stopWatch.ElapsedMilliseconds;
    Console.WriteLine($"Operation DoGrpcHttpCall took {time}ms");
}
