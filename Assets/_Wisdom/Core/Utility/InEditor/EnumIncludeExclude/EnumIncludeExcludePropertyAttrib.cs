using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Genesis.Wisdom {
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
	internal sealed class EnumIncludeExcludeAttribute: PropertyAttribute {
		internal EnumIncludeExcludeAttribute(Type type, bool isInclusion, params string[] nameArr): base() {
			if(nameArr.Length == 0) {
				UnityEngine.Assertions.Assert.IsTrue(false);
				return;
			}

			string[] myNameArr = Enum.GetNames(type);

			enumValIndexList = new List<int>(myNameArr.Length);
			nameList = new List<string>(myNameArr.Length);

			if(isInclusion) {
				InclusionFunc(nameArr, myNameArr);
			} else {
				ExclusionFunc(nameArr, myNameArr);
			}
		}

		private void InclusionFunc(string[] nameArr, string[] myNameArr) {
			int myNameArrLen = myNameArr.Length;

			for(int i = 0; i < myNameArrLen; ++i) {
				if(nameArr.Any((name) => {
					return name.Equals(myNameArr[i]);
				})) {
					enumValIndexList.Add(i);
					nameList.Add(myNameArr[i]);
				}
			}
		}

		private void ExclusionFunc(string[] nameArr, string[] myNameArr) {
			int myNameArrLen = myNameArr.Length;

			for(int i = 0; i < myNameArrLen; ++i) {
				if(nameArr.All((name) => {
					return !name.Equals(myNameArr[i]);
				})) {
					enumValIndexList.Add(i);
					nameList.Add(myNameArr[i]);
				}
			}
		}

		internal readonly List<int> enumValIndexList;

		internal readonly List<string> nameList;
	}
}