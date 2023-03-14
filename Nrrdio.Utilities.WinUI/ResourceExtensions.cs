using Microsoft.Windows.ApplicationModel.Resources;

namespace Nrrdio.Utilities.WinUI;

/// <summary>
/// Source: WinUI Template Studio
/// </summary>
public static class ResourceExtensions {
	static ResourceLoader ResourceLoader => new();
	public static string GetLocalized(this string resourceKey) => ResourceLoader.GetString(resourceKey);
}

