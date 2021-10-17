using System;
using UnityEngine;

namespace Genesis.Wisdom {
	[Serializable]
	internal sealed class ScaleAnimSupport {
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
		private bool shldShowScale;

		[SerializeField]
		private ShldAnimateVec3 shldAnimateVec3;

		[SerializeField]
		[ShowHideInInspector(true, nameof(shldShowScale), true)]
		private Vec3<float> startScale;

		[SerializeField]
		[ShowHideInInspector(true, nameof(shldShowScale), true)]
		private Vec3<float> endScale;

		internal delegate void CalcComponentValDelegate(ref Vector3 vec, float lerpFactor);

		internal CalcComponentValDelegate calcComponentValDelegate;

		internal void MyOnValidate() {
			startScale.shldShowX = endScale.shldShowX = shldAnimateVec3.shldAnimateX;
			startScale.shldShowY = endScale.shldShowY = shldAnimateVec3.shldAnimateY;
			startScale.shldShowZ = endScale.shldShowZ = shldAnimateVec3.shldAnimateZ;

			shldShowScale = shldAnimateVec3.shldAnimateX || shldAnimateVec3.shldAnimateY || shldAnimateVec3.shldAnimateZ;

			MyAwake();
		}

		internal void MyAwake() {
			if(Application.isEditor && Application.isPlaying && !shldShowScale) {
				UnityEngine.Assertions.Assert.IsTrue(
					false,
					"Application.isEditor && Application.isPlaying && !shldShowScale"
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
				vec.x = Mathf.LerpUnclamped(startScale.x, endScale.x, lerpFactor);
			}

			void CalcY(ref Vector3 vec, float lerpFactor) {
				vec.y = Mathf.LerpUnclamped(startScale.y, endScale.y, lerpFactor);
			}

			void CalcZ(ref Vector3 vec, float lerpFactor) {
				vec.z = Mathf.LerpUnclamped(startScale.z, endScale.z, lerpFactor);
			}
		}
	}
}