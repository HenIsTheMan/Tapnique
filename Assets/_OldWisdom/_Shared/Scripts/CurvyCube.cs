using UnityEngine;
using static IWP.General.InitIDs;

namespace IWP.General {
	internal sealed class CurvyCube: MonoBehaviour {
		#region Fields

		[SerializeField]
		private InitControl initControl;

		private Mesh mesh;
		private Vector3[] vertices;
		private Color32[] colors;
		private Vector3[] normals;

		[SerializeField]
		private bool shldCreateColliders;

		[SerializeField]
		private bool shldDrawGizmos;

		[SerializeField]
		private int xSize;

		[SerializeField]
		private int ySize;

		[SerializeField]
		private int zSize;

		[SerializeField]
		private int curveFactor;

		[SerializeField]
		private Material mtl;

		#endregion

		#region Properties
		#endregion

		#region Ctors and Dtor

		internal CurvyCube(): base() {
			initControl = null;

			mesh = null;
			vertices = System.Array.Empty<Vector3>();
			colors = System.Array.Empty<Color32>();
			normals = System.Array.Empty<Vector3>();

			shldCreateColliders = false;
			shldDrawGizmos = false;

			xSize = 0;
			ySize = 0;
			zSize = 0;

			curveFactor = 0;

			mtl = null;
		}

        static CurvyCube() {
        }

		#endregion

		#region Unity User Callback Event Funcs

		private void OnEnable() {
			if(initControl != null) {
				initControl.AddMethod((uint)InitID.CurvyCube, Init);
			} else {
				Init(); //Workaround
			}
		}

		private void OnDisable() {
			if(initControl != null) {
				initControl.RemoveMethod((uint)InitID.CurvyCube, Init);
			}
		}

		#endregion

		private void Init() {
			gameObject.AddComponent<MeshFilter>().mesh = mesh = new Mesh();
			mesh.name = "Procedural Cube";

			gameObject.AddComponent<MeshRenderer>().materials = new Material[]{
				mtl,
				mtl,
				mtl
			};

			CreateVertices();
			CreateTriangles();

			if(shldCreateColliders) {
				CreateColliders();
			}
		}

		private void CreateVertices() {
			int cornerVertices = 8;
			int edgeVertices = (xSize + ySize + zSize - 3) * 4;
			int faceVertices = 
				((xSize - 1) * (ySize - 1) +
				(xSize - 1) * (zSize - 1) +
				(ySize - 1) * (zSize - 1))
				* 2;

			vertices = new Vector3[cornerVertices + edgeVertices + faceVertices];
			colors = new Color32[vertices.Length];
			normals = new Vector3[vertices.Length];

			int v = 0;
			for(int y = 0; y <= ySize; y++) {
				for(int x = 0; x <= xSize; x++) {
					SetVertex(v++, x, y, 0);
				}
				for(int z = 1; z <= zSize; z++) {
					SetVertex(v++, xSize, y, z);
				}
				for(int x = xSize - 1; x >= 0; x--) {
					SetVertex(v++, x, y, zSize);
				}
				for(int z = zSize - 1; z > 0; z--) {
					SetVertex(v++, 0, y, z);
				}
			}
			for(int z = 1; z < zSize; z++) {
				for(int x = 1; x < xSize; x++) {
					SetVertex(v++, x, ySize, z);
				}
			}
			for(int z = 1; z < zSize; z++) {
				for(int x = 1; x < xSize; x++) {
					SetVertex(v++, x, 0, z);
				}
			}

			mesh.vertices = vertices;
			mesh.colors32 = colors;
			mesh.normals = normals;
		}

		private void SetVertex(int i, float x, float y, float z) {
			Vector3 inner = vertices[i] = new Vector3(x, y, z);

			if(x < curveFactor) {
				inner.x = curveFactor;
			} else if(x > xSize - curveFactor) {
				inner.x = xSize - curveFactor;
			}
			if(y < curveFactor) {
				inner.y = curveFactor;
			} else if(y > ySize - curveFactor) {
				inner.y = ySize - curveFactor;
			}
			if(z < curveFactor) {
				inner.z = curveFactor;
			} else if(z > zSize - curveFactor) {
				inner.z = zSize - curveFactor;
			}

			normals[i] = (vertices[i] - inner).normalized;
			vertices[i] = inner + normals[i] * curveFactor - new Vector3(xSize * 0.5f, ySize * 0.5f, zSize * 0.5f);
			colors[i] = new Color32((byte)x, (byte)y, (byte)z, 0);
		}

		private void CreateTriangles() {
			int[] trianglesZ = new int[(xSize * ySize) * 12];
			int[] trianglesX = new int[(ySize * zSize) * 12];
			int[] trianglesY = new int[(xSize * zSize) * 12];
			int ring = (xSize + zSize) * 2;
			int tZ = 0, tX = 0, tY = 0, v = 0;

			for(int y = 0; y < ySize; y++, v++) {
				for(int q = 0; q < xSize; q++, v++) {
					tZ = SetQuad(trianglesZ, tZ, v, v + 1, v + ring, v + ring + 1);
				}
				for(int q = 0; q < zSize; q++, v++) {
					tX = SetQuad(trianglesX, tX, v, v + 1, v + ring, v + ring + 1);
				}
				for(int q = 0; q < xSize; q++, v++) {
					tZ = SetQuad(trianglesZ, tZ, v, v + 1, v + ring, v + ring + 1);
				}
				for(int q = 0; q < zSize - 1; q++, v++) {
					tX = SetQuad(trianglesX, tX, v, v + 1, v + ring, v + ring + 1);
				}
				tX = SetQuad(trianglesX, tX, v, v - ring + 1, v + ring, v + 1);
			}

			tY = CreateTopFace(trianglesY, tY, ring);
			_ = CreateBottomFace(trianglesY, tY, ring);

			mesh.subMeshCount = 3;
			mesh.SetTriangles(trianglesZ, 0);
			mesh.SetTriangles(trianglesX, 1);
			mesh.SetTriangles(trianglesY, 2);
		}

		private int CreateTopFace(int[] triangles, int t, int ring) {
			int v = ring * ySize;
			for(int x = 0; x < xSize - 1; x++, v++) {
				t = SetQuad(triangles, t, v, v + 1, v + ring - 1, v + ring);
			}
			t = SetQuad(triangles, t, v, v + 1, v + ring - 1, v + 2);

			int vMin = ring * (ySize + 1) - 1;
			int vMid = vMin + 1;
			int vMax = v + 2;

			for(int z = 1; z < zSize - 1; z++, vMin--, vMid++, vMax++) {
				t = SetQuad(triangles, t, vMin, vMid, vMin - 1, vMid + xSize - 1);
				for(int x = 1; x < xSize - 1; x++, vMid++) {
					t = SetQuad(
						triangles, t,
						vMid, vMid + 1, vMid + xSize - 1, vMid + xSize);
				}
				t = SetQuad(triangles, t, vMid, vMax, vMid + xSize - 1, vMax + 1);
			}

			int vTop = vMin - 2;
			t = SetQuad(triangles, t, vMin, vMid, vTop + 1, vTop);
			for(int x = 1; x < xSize - 1; x++, vTop--, vMid++) {
				t = SetQuad(triangles, t, vMid, vMid + 1, vTop, vTop - 1);
			}
			t = SetQuad(triangles, t, vMid, vTop - 2, vTop, vTop - 1);

			return t;
		}

		private int CreateBottomFace(int[] triangles, int t, int ring) {
			int v = 1;
			int vMid = vertices.Length - (xSize - 1) * (zSize - 1);
			t = SetQuad(triangles, t, ring - 1, vMid, 0, 1);
			for(int x = 1; x < xSize - 1; x++, v++, vMid++) {
				t = SetQuad(triangles, t, vMid, vMid + 1, v, v + 1);
			}
			t = SetQuad(triangles, t, vMid, v + 2, v, v + 1);

			int vMin = ring - 2;
			vMid -= xSize - 2;
			int vMax = v + 2;

			for(int z = 1; z < zSize - 1; z++, vMin--, vMid++, vMax++) {
				t = SetQuad(triangles, t, vMin, vMid + xSize - 1, vMin + 1, vMid);
				for(int x = 1; x < xSize - 1; x++, vMid++) {
					t = SetQuad(
						triangles, t,
						vMid + xSize - 1, vMid + xSize, vMid, vMid + 1);
				}
				t = SetQuad(triangles, t, vMid + xSize - 1, vMax + 1, vMid, vMax);
			}

			int vTop = vMin - 1;
			t = SetQuad(triangles, t, vTop + 1, vTop, vTop + 2, vMid);
			for(int x = 1; x < xSize - 1; x++, vTop--, vMid++) {
				t = SetQuad(triangles, t, vTop, vTop - 1, vMid, vMid + 1);
			}
			t = SetQuad(triangles, t, vTop, vTop - 1, vMid, vTop - 2);

			return t;
		}

		private static int SetQuad(int[] triangles, int i, int v00, int v10, int v01, int v11) {
			triangles[i] = v00;
			triangles[i + 1] = triangles[i + 4] = v01;
			triangles[i + 2] = triangles[i + 3] = v10;
			triangles[i + 5] = v11;
			return i + 6;
		}

		private void CreateColliders() {
			//AddBoxCollider(xSize, ySize - curveFactor * 2, zSize - curveFactor * 2);
			//AddBoxCollider(xSize - curveFactor * 2, ySize, zSize - curveFactor * 2);
			//AddBoxCollider(xSize - curveFactor * 2, ySize - curveFactor * 2, zSize);

			Vector3 min = Vector3.one * curveFactor;
			Vector3 half = new Vector3(xSize, ySize, zSize) * 0.5f;
			Vector3 max = new Vector3(xSize, ySize, zSize) - min;

			AddCapsuleCollider(0, half.x, min.y, min.z);
			AddCapsuleCollider(0, half.x, min.y, max.z);
			AddCapsuleCollider(0, half.x, max.y, min.z);
			AddCapsuleCollider(0, half.x, max.y, max.z);

			AddCapsuleCollider(1, min.x, half.y, min.z);
			AddCapsuleCollider(1, min.x, half.y, max.z);
			AddCapsuleCollider(1, max.x, half.y, min.z);
			AddCapsuleCollider(1, max.x, half.y, max.z);

			AddCapsuleCollider(2, min.x, min.y, half.z);
			AddCapsuleCollider(2, min.x, max.y, half.z);
			AddCapsuleCollider(2, max.x, min.y, half.z);
			AddCapsuleCollider(2, max.x, max.y, half.z);
		}

		private void AddBoxCollider(float x, float y, float z) {
			BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
			//boxCollider.center -= new Vector3(xSize * 0.5f, ySize * 0.5f, zSize * 0.5f);
			boxCollider.size = new Vector3(x, y, z);
		}

		private void AddCapsuleCollider(int direction, float x, float y, float z) {
			CapsuleCollider c = gameObject.AddComponent<CapsuleCollider>();

			c.center = new Vector3(x, y, z);

			c.direction = direction;
			c.radius = curveFactor;

			c.height = c.center[direction] * 2f;

			c.center = new Vector3(x, y, z) - new Vector3(xSize * 0.5f, ySize * 0.5f, zSize * 0.5f);
		}

		private void OnDrawGizmos() {
			if(!shldDrawGizmos || vertices == null) {
				return;
			}

			for(int i = 0; i < vertices.Length; i++) {
				Gizmos.color = Color.black;
				Gizmos.DrawSphere(vertices[i], 0.1f);
				Gizmos.color = Color.yellow;
				Gizmos.DrawRay(vertices[i], normals[i]);
			}
		}
	}
}