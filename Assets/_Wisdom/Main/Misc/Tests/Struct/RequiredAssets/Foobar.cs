namespace Genesis.Wisdom {
    internal struct Foobar {
		internal int val;

		/* Cannot
		Foobar() {
		}
		//*/

		internal Foobar(int val) {
			this.val = val;
		}
    }
}