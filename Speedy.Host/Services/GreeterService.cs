using Grpc.Core;

namespace Speedy.Host.Services;

public class GreeterService : Greeter.GreeterBase
{
    private readonly IRevertService _revertService;

    public GreeterService(IRevertService revertService)
    {
        _revertService = revertService;
    }

    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        return Task.FromResult(new HelloReply
        {
            Message = _revertService.Revert(request.Name)
        });
    }

    public override Task<HelloReply> SayHelloAgain(HelloRequest request, ServerCallContext context)
    {
        return Task.FromResult(new HelloReply
        {
            Message = _revertService.Revert(request.Name)
        });
    }
}
