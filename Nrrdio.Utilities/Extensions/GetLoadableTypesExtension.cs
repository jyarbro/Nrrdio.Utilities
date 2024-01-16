using System.Reflection;

namespace Nrrdio.Utilities.Extensions;

public static class GetLoadableTypesExtension {
	// Original: https://haacked.com/archive/2012/07/23/get-all-types-in-an-assembly.aspx/
	public static IEnumerable<Type> GetLoadableTypes(this Assembly assembly) {
		ArgumentNullException.ThrowIfNull(assembly);

		try {
			return assembly.GetTypes();
		}
		catch (ReflectionTypeLoadException e) {
			return e.Types.Where(t => t is not null).Cast<Type>();
		}
	}
}
