#if UNITY_EDITOR

using UnityEditor;

namespace Genesis.Wisdom {
    internal static partial class CreateCustom {
		[MenuItem("Assets/CreateCustom/AbstractClass", false, 15)]
		private static void CreateAbstractClass() {
			CreateCustomScript("Assets/Wisdom/Internal/Core/Utility/InEditor/CreateCustom/SampleAssets/ScriptTemplates/", "AbstractClass", "txt");
		}
	}
}

#endif