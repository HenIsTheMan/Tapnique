using System;

namespace Genesis.Wisdom {
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
	internal class EnumIndicesForColorAttribute: EnumIndicesAttribute {
		internal EnumIndicesForColorAttribute(bool shldShowEyedropper, bool shldShowAlpha, bool isHDR, params Type[] typeContainer):
			this(shldShowEyedropper, shldShowAlpha, isHDR, ", ", typeContainer)
		{
		}

		internal EnumIndicesForColorAttribute(bool shldShowEyedropper, bool shldShowAlpha, bool isHDR, string delimiter, params Type[] typeContainer):
			base(delimiter, typeContainer)
		{
			this.shldShowEyedropper = shldShowEyedropper;
			this.shldShowAlpha = shldShowAlpha;
			this.isHDR = isHDR;
		}

		internal readonly bool shldShowEyedropper;

		internal readonly bool shldShowAlpha;

		internal readonly bool isHDR;
	}
}