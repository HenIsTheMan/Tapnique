using System;
using UnityEngine;

namespace Genesis.Wisdom {
	[Serializable]
	internal sealed class TranslateAnimSupport {
		[Serializable]
		internal struct ShldAnimateVec3 {
			[SerializeField]
			internal bool shldAnimateX;

			[SerializeField]
			internal bool shldAnimateY;

			[SerializeField]
			internal bool shldAnimateZ;
		}

		[Serializable]
		internal struct Vec3<T> where T:
			struct,
			IComparable,
			IComparable<T>,
			IConvertible,
			IEquatable<T>,
			IFormattable
		{
			[SerializeField]
			[ShowHideInInspector(true, nameof(shldShowX), true)]
			internal T x;

			[SerializeField]
			[ShowHideInInspector(true, nameof(shldShowY), true)]
			internal T y;

			[SerializeField]
			[ShowHideInInspector(true, nameof(shldShowZ), true)]
			internal T z;
			
			[HideInInspector]
			[SerializeField]
			internal bool shldShowX;

			[HideInInspector]
			[SerializeField]
			internal bool shldShowY;

			[HideInInspector]
			[SerializeField]
			internal bool shldShowZ;
		}

		[HideInInspector]
		[SerializeField]
		private bool shldShowPos;

		[SerializeField]
		private ShldAnimateVec3 shldAnimateVec3;

		[SerializeField]
		[ShowHideInInspector(true, nameof(shldShowPos), true)]
		private Vec3<float> startPos;

		[SerializeField]
		[ShowHideInInspector(true, nameof(shldShowPos), true)]
		private Vec3<float> endPos;

		internal delegate void CalcComponentValDelegate(ref Vector3 vec, float lerpFactor);

		internal CalcComponentValDelegate calcComponentValDelegate;

		internal void MyOnValidate() {
			startPos.shldShowX = endPos.shldShowX = shldAnimateVec3.shldAnimateX;
			startPos.shldShowY = endPos.shldShowY = shldAnimateVec3.shldAnimateY;
			startPos.shldShowZ = endPos.shldShowZ = shldAnimateVec3.shldAnimateZ;

			shldShowPos = shldAnimateVec3.shldAnimateX || shldAnimateVec3.shldAnimateY || shldAnimateVec3.shldAnimateZ;

			MyAwake();
		}

		internal void MyAwake() {
			if(Application.isEditor && Application.isPlaying && !shldShowPos) {
				UnityEngine.Assertions.Assert.IsTrue(
					false,
					"Application.isEditor && Application.isPlaying && !shldShowPos"
				);
			}

			calcComponentValDelegate = null;

			if(shldAnimateVec3.shldAnimateX) {
				calcComponentValDelegate += CalcX;
			}

			if(shldAnimateVec3.shldAnimateY) {
				calcComponentValDelegate += CalcY;
			}

			if(shldAnimateVec3.shldAnimateZ) {
				calcComponentValDelegate += CalcZ;
			}

			void CalcX(ref Vector3 vec, float lerpFactor) {
				vec.x = Mathf.LerpUnclamped(startPos.x, endPos.x, lerpFactor);
			}

			void CalcY(ref Vector3 vec, float lerpFactor) {
				vec.y = Mathf.LerpUnclamped(startPos.y, endPos.y, lerpFactor);
			}

			void CalcZ(ref Vector3 vec, float lerpFactor) {
				vec.z = Mathf.LerpUnclamped(startPos.z, endPos.z, lerpFactor);
			}
		}
	}
}