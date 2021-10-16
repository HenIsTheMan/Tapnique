using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.Wisdom {
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
	internal sealed class EnumRangeAttribute: PropertyAttribute {
		internal EnumRangeAttribute(Type type, int minIndexInclusive, int maxIndexExclusive): base() {
			string[] nameArr = Enum.GetNames(type);

			if((minIndexInclusive >= maxIndexExclusive)
				|| (minIndexInclusive < 0)
				|| (maxIndexExclusive > nameArr.Length)
			) {
				UnityEngine.Assertions.Assert.IsTrue(false);
				return;
			}

			enumValIndexList = new List<int>(nameArr.Length);
			nameList = new List<string>(nameArr.Length);

			for(int i = minIndexInclusive; i < maxIndexExclusive; ++i) {
				enumValIndexList.Add(i);
				nameList.Add(nameArr[i]);
			}
		}

		internal EnumRangeAttribute(Type type, string minNameInclusive, string maxNameInclusive): base() {
			string[] nameArr = Enum.GetNames(type);

			int minIndexInclusive = Array.IndexOf(nameArr, minNameInclusive);
			int maxIndexInclusive = Array.IndexOf(nameArr, maxNameInclusive);

			if((minIndexInclusive > maxIndexInclusive)
				|| (minIndexInclusive < 0)
				|| (maxIndexInclusive > nameArr.Length - 1)
			) {
				UnityEngine.Assertions.Assert.IsTrue(false);
				return;
			}

			enumValIndexList = new List<int>(nameArr.Length);
			nameList = new List<string>(nameArr.Length);

			for(int i = minIndexInclusive; i <= maxIndexInclusive; ++i) {
				enumValIndexList.Add(i);
				nameList.Add(nameArr[i]);
			}
		}

		internal readonly List<int> enumValIndexList;

		internal readonly List<string> nameList;
	}
}