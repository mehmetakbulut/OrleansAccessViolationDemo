namespace Demo;

public interface IMyGrain : IGrainWithGuidKey
{
    public Task Noop();
}