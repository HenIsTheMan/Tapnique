using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static IWP.General.InitIDs;
using static IWP.General.OutlineSilhouetteTypes;

namespace IWP.General {
	[RequireComponent(typeof(Renderer))]
	internal sealed class OutlineSilhouette: MonoBehaviour {
		#region Fields

		[SerializeField]
		private InitControl initControl;

		private Material mtl;

		[ColorUsage(true, true), SerializeField]
		private Color outlineSilhouetteColor;

		[SerializeField]
		private float outlineThickness;

		[SerializeField]
		private OutlineSilhouetteType type;

		#endregion

		#region Properties

		internal Color OutlineSilhouetteColor {
			get {
				return outlineSilhouetteColor;
			}
			set {
				outlineSilhouetteColor = value;
				mtl.SetColor("_OutlineSilhouetteColor", outlineSilhouetteColor);
			}
		}

		internal float OutlineThickness {
			get {
				return outlineThickness;
			}
			set {
				outlineThickness = value;
				mtl.SetFloat("_OutlineThickness", outlineThickness);
			}
		}

		internal OutlineSilhouetteType Type {
			get {
				return type;
			}
		}

		#endregion

		#region Ctors and Dtor

		internal OutlineSilhouette(): base() {
			initControl = null;

			mtl = null;

			outlineSilhouetteColor = Color.white;

			outlineThickness = 0.0f;

			type = OutlineSilhouetteType.Amt;
		}

        static OutlineSilhouette() {
        }

		#endregion

		#region Unity User Callback Event Funcs

		private void OnValidate() {
			if(!Application.isPlaying) {
				LoadSmoothNormals();
			}

			InitStuff();
		}

		private void OnEnable() {
			if(initControl != null) {
				initControl.AddMethod((uint)InitID.OutlineSilhouette, Init);
			}
		}

		private void OnDisable() {
			if(initControl != null) {
				initControl.RemoveMethod((uint)InitID.OutlineSilhouette, Init);
			}
		}

		#endregion

		private void Init() {
			InitStuff();
		}

		internal void InitMe() {
			InitStuff();
		}

		private void InitStuff() {
			mtl = GetComponent<Renderer>().sharedMaterial;

			switch(type) {
				case OutlineSilhouetteType.OutlineAll:
					mtl.SetFloat("_ZTestFill", (float)UnityEngine.Rendering.CompareFunction.Always);
					mtl.SetFloat("_ZTestMask", (float)UnityEngine.Rendering.CompareFunction.Always);
					break;
				case OutlineSilhouetteType.OutlineVisible:
					mtl.SetFloat("_ZTestFill", (float)UnityEngine.Rendering.CompareFunction.LessEqual);
					mtl.SetFloat("_ZTestMask", (float)UnityEngine.Rendering.CompareFunction.Always);
					break;
				case OutlineSilhouetteType.OutlineHidden:
					mtl.SetFloat("_ZTestFill", (float)UnityEngine.Rendering.CompareFunction.Greater);
					mtl.SetFloat("_ZTestMask", (float)UnityEngine.Rendering.CompareFunction.Always);
					break;
				case OutlineSilhouetteType.OutlineAndSilhouette:
					mtl.SetFloat("_ZTestFill", (float)UnityEngine.Rendering.CompareFunction.Always);
					mtl.SetFloat("_ZTestMask", (float)UnityEngine.Rendering.CompareFunction.LessEqual);
					break;
				case OutlineSilhouetteType.Silhouette:
					mtl.SetFloat("_ZTestFill", (float)UnityEngine.Rendering.CompareFunction.Greater);
					mtl.SetFloat("_ZTestMask", (float)UnityEngine.Rendering.CompareFunction.LessEqual);
					break;
			}

			mtl.SetColor("_OutlineSilhouetteColor", outlineSilhouetteColor);
			mtl.SetFloat("_OutlineThickness", outlineThickness);
		}

		#pragma warning disable IDE0090 //Use 'new(...)'
		private static readonly HashSet<Mesh> registeredMeshes = new HashSet<Mesh>();
		#pragma warning restore IDE0090 //Use 'new(...)'

		[System.Serializable]
		private class ListVector3 {
			public List<Vector3> data;
		}

		[SerializeField, HideInInspector]
		#pragma warning disable IDE0090 //Use 'new(...)'
		private List<Mesh> bakeKeys = new List<Mesh>();
		#pragma warning restore IDE0090 //Use 'new(...)'

		[SerializeField, HideInInspector]
		#pragma warning disable IDE0090 //Use 'new(...)'
		private List<ListVector3> bakeValues = new List<ListVector3>();
		#pragma warning restore IDE0090 //Use 'new(...)'

		private void LoadSmoothNormals() {

			// Retrieve or generate smooth normals
			foreach(var meshFilter in GetComponentsInChildren<MeshFilter>()) {

				// Skip if smooth normals have already been adopted
				if(!registeredMeshes.Add(meshFilter.sharedMesh)) {
					continue;
				}

				// Retrieve or generate smooth normals
				var index = bakeKeys.IndexOf(meshFilter.sharedMesh);
				var smoothNormals = (index >= 0) ? bakeValues[index].data : SmoothNormals(meshFilter.sharedMesh);

				// Store smooth normals in UV3
				meshFilter.sharedMesh.SetUVs(3, smoothNormals);
			}

			// Clear UV3 on skinned mesh renderers
			foreach(var skinnedMeshRenderer in GetComponentsInChildren<SkinnedMeshRenderer>()) {
				if(registeredMeshes.Add(skinnedMeshRenderer.sharedMesh)) {
					skinnedMeshRenderer.sharedMesh.uv4 = new Vector2[skinnedMeshRenderer.sharedMesh.vertexCount];
				}
			}
		}

		List<Vector3> SmoothNormals(Mesh mesh) {

			// Group vertices by location
			var groups = mesh.vertices.Select((vertex, index) => new KeyValuePair<Vector3, int>(vertex, index)).GroupBy(pair => pair.Key);

			// Copy normals to a new list
			var smoothNormals = new List<Vector3>(mesh.normals);

			// Average normals for grouped vertices
			foreach(var group in groups) {

				// Skip single vertices
				if(group.Count() == 1) {
					continue;
				}

				// Calculate the average normal
				var smoothNormal = Vector3.zero;

				foreach(var pair in group) {
					smoothNormal += mesh.normals[pair.Value];
				}

				smoothNormal.Normalize();

				// Assign smooth normal to each vertex
				foreach(var pair in group) {
					smoothNormals[pair.Value] = smoothNormal;
				}
			}

			return smoothNormals;
		}
	}
}