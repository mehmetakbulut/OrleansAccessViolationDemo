using Orleans.Runtime;

namespace Demo;

public class MyStartupTask : IStartupTask
{
    private readonly IClusterClient _client;

    public MyStartupTask(IClusterClient client)
    {
        _client = client;
    }
    
    public async Task Execute(CancellationToken cancellationToken)
    {
        await _client.GetGrain<IMyGrain>(Guid.NewGuid()).Noop();
    }
}