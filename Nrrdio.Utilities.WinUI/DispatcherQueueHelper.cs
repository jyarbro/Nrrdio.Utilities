using System.Runtime.InteropServices;

namespace Nrrdio.Utilities.WinUI;

/// <summary>
/// Source: WinUI 3 Gallery
/// A Windows.System.DispatcherQueue must exist on the thread to use MicaController or DesktopAcrylicController. This helper class exposes and uses the underlying create function.
/// </summary>
public class DispatcherQueueHelper {
	[DllImport("CoreMessaging.dll")]
	static extern int CreateDispatcherQueueController(
		[In] DispatcherQueueOptions options,
		[In, Out, MarshalAs(UnmanagedType.IUnknown)] ref object dispatcherQueueController
	);

	object? DispatcherQueueController = null;

	public void EnsureWindowsSystemDispatcherQueueController() {
		// If one already exists, use it.
		if (Windows.System.DispatcherQueue.GetForCurrentThread() is not null) {
			return;
		}

		if (DispatcherQueueController is null) {
			DispatcherQueueOptions options;

			options.dwSize = Marshal.SizeOf(typeof(DispatcherQueueOptions));
			options.threadType = 2;    // DQTYPE_THREAD_CURRENT
			options.apartmentType = 2; // DQTAT_COM_STA

			CreateDispatcherQueueController(options, ref DispatcherQueueController!);
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	struct DispatcherQueueOptions {
		internal int dwSize;
		internal int threadType;
		internal int apartmentType;
	}
}

