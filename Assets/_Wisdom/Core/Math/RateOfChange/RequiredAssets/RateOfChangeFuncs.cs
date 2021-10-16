namespace Genesis.Wisdom {
    internal static partial class RateOfChange {
		internal delegate float RateOfChangeDelegate(float x);

		internal static RateOfChangeDelegate[] RateOfChangeFuncs {
			get;
			private set;
		}

		static RateOfChange() {
			int len = (int)RateOfChangeType.Amt;
			RateOfChangeFuncs = new RateOfChangeDelegate[len];

			RateOfChangeFuncs[(int)RateOfChangeType.ConstantRateOfChange] = null;

			for(int i = (int)RateOfChangeType.ConstantRateOfChange + 1; i < len; ++i) {
				RateOfChangeFuncs[i] = (RateOfChangeDelegate)Type.GetMethod(((RateOfChangeType)i).ToString()).CreateDelegate(typeof(RateOfChangeDelegate));
			}
		}
    }
}