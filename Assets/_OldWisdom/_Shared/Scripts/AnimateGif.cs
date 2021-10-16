using UnityEngine; 
using UnityEngine.UI; 
 
namespace IWP.General { 
    internal sealed class AnimateGif: MonoBehaviour { 
		#region Fields 
 
		[SerializeField] 
		private bool shldAnimate; 
 
		[SerializeField] 
		private RawImage rawImg; 
 
		[SerializeField] 
		private float FPS; 
 
		[SerializeField]
		private Texture[] frames; 
 
		#endregion 
 
		#region Properties 
		#endregion 
 
		#region Ctors and Dtor 
 
		internal AnimateGif(): base() { 
			shldAnimate = false; 
 
			rawImg = null; 
 
			FPS = 0.0f; 
			frames = System.Array.Empty<Texture>(); 
        } 
 
        static AnimateGif() { 
        } 
 
		#endregion 
 
		#region Unity User Callback Event Funcs 
 
		private void Update() { 
			if(shldAnimate) { 
				int index = (int)(Time.time * FPS); 
				index %= frames.Length; 
				rawImg.texture = frames[index]; 
			} 
		} 
 
		#endregion 
	} 
}