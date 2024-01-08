namespace Nrrdio.Utilities.WinUI.FrameRate;

public interface IFrameRateHandler
{
    event EventHandler<FrameRateEventArgs> FrameRateUpdated;

    void Increment(long elapsedMilliseconds);
}