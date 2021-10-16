using System;
using System.Collections.Generic;
using UnityEngine;

namespace IWP.Math {
	internal static class Val {
		internal static T RoundToNearest<T>(T multiple, T val) where T:
			struct,
			IComparable,
			IComparable<T>,
			IConvertible,
			IEquatable<T>,
			IFormattable
		{
			return default(T);
		}

		internal static float RoundToNearest(float multiple, float val) {
			return Mathf.Round(val / multiple) * multiple;
		}

		internal static float Lerp(float start, float end, float lerpFactor) {
			return start * (1.0f - lerpFactor) + end * lerpFactor;
		}

		internal static double Lerp(double start, double end, double lerpFactor) {
			return start * (1.0 - lerpFactor) + end * lerpFactor;
		}

		internal static double Lerp(in double start, in double end, in double lerpFactor) {
			return start * (1.0 - lerpFactor) + end * lerpFactor;
		}

		internal static Color Lerp(in Color start, in Color end, float lerpFactor) {
			return start * (1.0f - lerpFactor) + end * lerpFactor;
		}

		internal static Vector3 Lerp(Vector3 start, Vector3 end, float lerpFactor) {
			return start * (1.0f - lerpFactor) + end * lerpFactor;
		}

		internal static Vector3 Lerp(in Vector3 start, in Vector3 end, float lerpFactor) {
			return start * (1.0f - lerpFactor) + end * lerpFactor;
		}

		internal static Quaternion Slerp(in Quaternion start, in Quaternion end, float slerpFactor, bool shldTakeShortestPath = true) {
			Quaternion tempQuat = end;
			float cosTheta = Quaternion.Dot(start, end);

			if(shldTakeShortestPath && cosTheta < 0.0f) { //Prevent interpolation from taking long way arnd the sphere
				tempQuat.w = -tempQuat.w;
				tempQuat.x = -tempQuat.x;
				tempQuat.y = -tempQuat.y;
				tempQuat.z = -tempQuat.z;

				cosTheta = -cosTheta;
			}

			if(cosTheta > 1.0f - Mathf.Epsilon) { //Do lerp when cosTheta is close to 1 to avoid side effect of sin(angle) becoming a 0 denominator
				return Quaternion.LerpUnclamped(start, tempQuat, slerpFactor);
			} else {
				float angle = Mathf.Acos(cosTheta);
				float val0 = Mathf.Sin(slerpFactor * angle) / Mathf.Sin(angle);
				float val1 = Mathf.Sin((1.0f - slerpFactor) * angle) / Mathf.Sin(angle);
				Quaternion result;

				result.w = val1 * start.w + val0 * tempQuat.w;
				result.x = val1 * start.x + val0 * tempQuat.x;
				result.y = val1 * start.y + val0 * tempQuat.y;
				result.z = val1 * start.z + val0 * tempQuat.z;

				return result;
			}
		}

		internal static float InverseLerp(in Vector3 start, in Vector3 end, in Vector3 interpolant) {
			Vector3 endPrime = end - start;
			Vector3 interpolantPrime = interpolant - start;
			return Vector3.Dot(interpolantPrime, endPrime) / Vector3.SqrMagnitude(endPrime);
		}

		internal static void Swap<T>(ref T[] arr, int index0, int index1) {
			T temp = arr[index0];
			arr[index0] = arr[index1];
			arr[index1] = temp;
		}

		internal static void Swap<T>(ref T[] arr, uint index0, uint index1) {
			T temp = arr[index0];
			arr[index0] = arr[index1];
			arr[index1] = temp;
		}

		internal static void Swap<T>(ref IList<T> list, int index0, int index1) {
			T temp = list[index0];
			list[index0] = list[index1];
			list[index1] = temp;
		}

		internal static void Swap<T>(ref T val0, ref T val1) {
			T temp = val0;
			val0 = val1;
			val1 = temp;
		}

		internal static void Swap(ref short val0, ref short val1) {
			val0 ^= val1 ^= val0 ^= val1;
		}

		internal static void Swap(ref int val0, ref int val1) {
			val0 ^= val1;
			val1 = val0 ^ val1;
			val0 ^= val1;
		}

		internal static void Swap(ref long val0, ref long val1) {
			val0 *= val1;
			val1 = val0 / val1;
			val0 /= val1;
		}
	}
}