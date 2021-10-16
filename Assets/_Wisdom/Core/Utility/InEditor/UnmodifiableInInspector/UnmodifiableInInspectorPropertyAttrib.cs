using System;
using UnityEngine;

namespace Genesis.Wisdom {
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
	internal sealed class UnmodifiableInInspectorAttribute: PropertyAttribute {
		internal UnmodifiableInInspectorAttribute(): this(AttribWorkingPeriod.Always) {
		}

		internal UnmodifiableInInspectorAttribute(AttribWorkingPeriod attribWorkingPeriod): base() {
			this.attribWorkingPeriod = attribWorkingPeriod == AttribWorkingPeriod.Amt
				? AttribWorkingPeriod.Always
				: attribWorkingPeriod;
		}

		internal readonly AttribWorkingPeriod attribWorkingPeriod;
	}
}