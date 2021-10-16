using System;
using UnityEngine;

namespace Genesis.Wisdom {
	internal sealed class TypeHelperTest: MonoBehaviour {
		private void Awake() {
			Type[] derivedTypes = TypeHelper.GetDerivedTypes(typeof(MonoBehaviour));
			Debug.Log(derivedTypes.Length, gameObject);

			Array.ForEach(derivedTypes, (derivedType) => {
				Debug.Log(derivedType, gameObject);
			});
		}
    }
}