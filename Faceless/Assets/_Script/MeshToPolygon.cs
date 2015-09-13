using UnityEngine;
using System.Collections;

public class MeshToPolygon : MonoBehaviour {

		private PolygonCollider2D polygonCollider;
		private Mesh myMesh;
 
		void Start () {
		myMesh = this.GetComponent<MeshFilter> ().mesh;
			GameObject temp = Instantiate (new GameObject(), this.transform.position, this.transform.rotation) as GameObject;
		temp.AddComponent <MeshFilter>();
		temp.AddComponent <MeshRenderer>();
		temp.GetComponent<MeshFilter> ().mesh = myMesh;
			this.transform.rotation = Quaternion.Euler (Vector3.zero);
			this.transform.localScale = new Vector3 (-1f, 1f, 1f);


			polygonCollider = this.GetComponent<PolygonCollider2D> ();
			polygonCollider.pathCount = myMesh.triangles.Length;
			//temp.transform.SetParent (this.transform);
				for (int i = 0; i<polygonCollider.pathCount; i++) {

					polygonCollider.SetPath (i, new []{new Vector2 (myMesh.vertices[myMesh.triangles[i*3]].x,myMesh.vertices[myMesh.triangles[i*3]].z), 
						new Vector2 ( myMesh.vertices[myMesh.triangles[i*3+1]].x,myMesh.vertices[myMesh.triangles[i*3+1]].z),
						new Vector2 ( myMesh.vertices[myMesh.triangles[i*3+2]].x,myMesh.vertices[myMesh.triangles[i*3+2]].z)
					});
				}
				
				
		}
		
}
