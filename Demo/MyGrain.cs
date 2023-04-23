using Orleans.Runtime;

namespace Demo;

public class MyGrain : Grain, IMyGrain
{
    private readonly IPersistentState<MyState> _state;
    private IDisposable _timer;

    public MyGrain([PersistentState("my")] IPersistentState<MyState> state)
    {
        _state = state;
    }
    
    public override Task OnActivateAsync(CancellationToken cancellationToken)
    {
        _timer = this.RegisterTimer(OnTimerAsync, null, TimeSpan.FromMilliseconds(1), TimeSpan.FromMilliseconds(1));
        return base.OnActivateAsync(cancellationToken);
    }

    public override Task OnDeactivateAsync(DeactivationReason reason, CancellationToken cancellationToken)
    {
        _state.State = new MyState();
        _state.WriteStateAsync();
        return base.OnDeactivateAsync(reason, cancellationToken);
    }

    public Task Noop()
    {
        return Task.CompletedTask;
    }

    public async Task OnTimerAsync(object state = null)
    {
        _timer?.Dispose();
        _timer = null;
        await Task.Run(() => this.AsReference<IMyGrain>().Noop());                              // will throw on deactivate
        // await Task.Run(() => this.AsReference<IMyGrain>().Noop()).ConfigureAwait(false);     // wont throw on deactivate
        DeactivateOnIdle();
    }
}