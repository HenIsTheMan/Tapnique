#if UNITY_EDITOR

using UnityEditor;

namespace Genesis.Wisdom {
    internal static partial class CreateCustom {
		[MenuItem("Assets/CreateCustom/StaticClass", false, 17)]
		private static void CreateStaticClass() {
			CreateCustomScript("Assets/Wisdom/Internal/Core/Utility/InEditor/CreateCustom/SampleAssets/ScriptTemplates/", "StaticClass", "txt");
		}
	}
}

#endif