using System;
using UnityEngine;

namespace Genesis.Wisdom {
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
	internal class EnumIndicesAttribute: PropertyAttribute {
		internal EnumIndicesAttribute(params Type[] typeContainer): this(", ", typeContainer) {
		}

		internal EnumIndicesAttribute(string delimiter, params Type[] typeContainer): base() {
			int len = typeContainer.Length;
			nameContainer = new string[len][];

			for(int i = 0; i < len; ++i) {
				nameContainer[i] = Enum.GetNames(typeContainer[i]);
			}

			this.delimiter = delimiter;
		}

		internal readonly string[][] nameContainer;

		internal readonly string delimiter;
	}
}