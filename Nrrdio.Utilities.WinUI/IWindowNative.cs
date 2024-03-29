﻿using System.Runtime.InteropServices;

namespace Nrrdio.Utilities.WinUI;

/// <summary>
/// Source: https://github.com/microsoft/WinUI-3-Demos/blob/420c48fe1613cb20b38000252369a0c556543eac/src/Build2020Demo/DemoBuildCs/DemoBuildCs/DemoBuildCs/App.xaml.cs#L60
/// </summary>
[ComImport]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Guid("EECDBF0E-BAE9-4CB6-A68E-9598E1CB57BB")]
public interface IWindowNative {
	IntPtr WindowHandle { get; }
}
