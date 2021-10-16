using UnityEngine;

namespace Genesis.Wisdom {
    internal sealed partial class ExistenceEnforcer: Singleton<ExistenceEnforcer> {
		[SerializeField]
		private ExistenceInfo[] existenceInfoContainer;

		private void Awake() {
			if(existenceInfoContainer == null) {
				return;
			}

			foreach(ExistenceInfo existenceInfo in existenceInfoContainer) {
				if(!GameObject.Find(existenceInfo.myName)) {
					Instantiate(
						existenceInfo.prefabGameObj,
						existenceInfo.pos,
						existenceInfo.rotation,
						existenceInfo.parentTransform
					).name = existenceInfo.myName;
				}
			}

			Destroy(gameObject);
		}
	}
}