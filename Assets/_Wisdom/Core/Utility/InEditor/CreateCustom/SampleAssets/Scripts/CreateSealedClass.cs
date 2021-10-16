#if UNITY_EDITOR

using UnityEditor;

namespace Genesis.Wisdom {
    internal static partial class CreateCustom {
		[MenuItem("Assets/CreateCustom/SealedClass", false, 16)]
		private static void CreateSealedClass() {
			CreateCustomScript("Assets/Wisdom/Internal/Core/Utility/InEditor/CreateCustom/SampleAssets/ScriptTemplates/", "SealedClass", "txt");
		}
	}
}

#endif