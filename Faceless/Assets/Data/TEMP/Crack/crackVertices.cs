using UnityEngine;
using System.Collections;

public class crackVertices : MonoBehaviour {
	private Vector3[] myVerticesNew;
	private Vector3[] myVerticesStart;
	public bool isDisp;
	// Use this for initialization
	void Start () {
		myVerticesStart = this.GetComponent<MeshFilter>().mesh.vertices;
		myVerticesNew = this.GetComponent<MeshFilter>().mesh.vertices;
	}
	
	// Update is called once per frame
	void Update () {
		for(int i = 0; i<this.GetComponent<MeshFilter>().mesh.vertexCount;i++){
			myVerticesNew[i] = myVerticesStart[i]+(Random.insideUnitSphere*0.1f);
			myVerticesNew[i].z = 0f;
			//this.GetComponent<MeshFilter>().mesh.vertices[i] = new Vector3( this.GetComponent<MeshFilter>().mesh.vertices[i].x,this.GetComponent<MeshFilter>().mesh.vertices[i].y,0f);
		}
		this.GetComponent<MeshFilter>().mesh.vertices = myVerticesNew;
		if(isDisp){
			this.GetComponent<MeshRenderer>().material.SetTextureOffset("_BumpMap",this.GetComponent<MeshRenderer>().material.GetTextureOffset("_BumpMap")+Vector2.one*0.1f);
		}
	}
}
