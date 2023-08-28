namespace Nrrdio.Utilities;

public class Wait {
    public bool Continue { get; set; }

    public async Task ForContinue() {
        Continue = false;

        await Task.Run(() => {
            while (!Continue) {
                // 80ms after a click feels pretty snappy but doesn't bog down the machine with a loop.
                Thread.Sleep(80);
            }
        });
    }

    public async Task For(int ms) => await Task.Delay(ms);
}
