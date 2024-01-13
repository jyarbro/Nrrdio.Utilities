namespace Nrrdio.Utilities.WinUI.FrameRate;

public class FrameRateHandler : IFrameRateHandler {
    // Too low will calculate framerate too often.
    // Too high and it becomes hard to pinpoint issues.
    const int FRAMERATE_DELAY = 200;

    public event EventHandler<FrameRateEventArgs>? FrameRateUpdated;

    double FrameCount;
    double FrameDuration;
    double TotalSeconds;
    DateTime FrameRunTimer = DateTime.Now;
    DateTime FrameTimer = DateTime.Now.AddMilliseconds(FRAMERATE_DELAY);

    public void Increment(long elapsedMilliseconds) {
        FrameDuration += elapsedMilliseconds;
        FrameCount++;

        if (FrameTimer < DateTime.Now) {
            FrameTimer = DateTime.Now.AddMilliseconds(FRAMERATE_DELAY);
            UpdateFrameRate();
        }
    }

    void UpdateFrameRate() {
        TotalSeconds = (DateTime.Now - FrameRunTimer).TotalSeconds;

        FrameRateUpdated?.Invoke(this, new FrameRateEventArgs {
            FramesPerSecond = Math.Round(FrameCount / TotalSeconds),
            FrameLag = Math.Round(FrameDuration / FrameCount, 2)
        });

        if (TotalSeconds > 5) {
            FrameCount = 0;
            FrameDuration = 0;
            FrameRunTimer = DateTime.Now;
        }
    }
}
