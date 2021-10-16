using System;
using System.Linq;
using UnityEngine;

namespace Genesis.Wisdom {
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
	internal sealed class ShowHideInInspectorAttribute: PropertyAttribute {
		internal ShowHideInInspectorAttribute(bool isShow, string fieldName, params object[] valArr): base() {
			this.isShow = isShow;
			this.fieldName = fieldName;

			valStrArr = valArr.Select((val) => {
				return val.ToString();
			}).ToArray();
		}

		internal readonly bool isShow;

		internal readonly string fieldName;

		internal readonly string[] valStrArr;
	}
}