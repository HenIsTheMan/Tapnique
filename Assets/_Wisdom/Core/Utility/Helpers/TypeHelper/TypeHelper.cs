using System;
using System.Linq;

namespace Genesis.Wisdom {
	internal static class TypeHelper {
		internal static Type[] GetDerivedTypes(Type baseType) {
			return (from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()
				from assemblyType in domainAssembly.GetTypes()
				where assemblyType.IsSubclassOf(baseType) //where baseType.IsAssignableFrom(assemblyType) && assemblyType != baseType
				select assemblyType).ToArray();
		}
	}
}