using System.Runtime.InteropServices;
using System.Text;

namespace Nrrdio.Utilities.WinUI;

/// <summary>
/// Source: WinUI Template Studio
/// </summary>
public class RuntimeHelper {
	[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
	static extern int GetCurrentPackageFullName(ref int packageFullNameLength, StringBuilder? packageFullName);

	public static bool IsMSIX {
		get {
			var length = 0;

			return GetCurrentPackageFullName(ref length, null) != 15700L;
		}
	}
}
