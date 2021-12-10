using System;
using System.Runtime.InteropServices;

// https://github.com/microsoft/WinUI-3-Demos/blob/420c48fe1613cb20b38000252369a0c556543eac/src/Build2020Demo/DemoBuildCs/DemoBuildCs/DemoBuildCs/App.xaml.cs#L60
[ComImport]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Guid("EECDBF0E-BAE9-4CB6-A68E-9598E1CB57BB")]
internal interface IWindowNative {
    IntPtr WindowHandle { get; }
}
