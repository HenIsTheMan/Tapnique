using System.Collections.Generic;
using UnityEngine;

namespace IWP.General {
	[System.Serializable]
    internal struct TutorialData {
		[SerializeField]
		internal string str;

		[SerializeField]
		internal List<Rect> rects;
    }
}