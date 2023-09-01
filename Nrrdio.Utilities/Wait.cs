namespace Nrrdio.Utilities;

public class Wait {
    public bool Continue { get; set; }
    public bool Pause { get; set; }
    public int Delay { get; set; }

    public async Task ForContinue() {
        Continue = false;

        await Task.Run(() => {
            while (!Continue) {
                // 80ms after a click feels pretty snappy but doesn't bog down the machine with a loop.
                Thread.Sleep(80);
            }
        });
    }

    public async Task For(int ms) {
        if (Pause) {
            await ForContinue();
        }
        else {
            await Task.Delay(ms);
        }
    }

    public async Task ForDelay() => await For(Delay);
}
