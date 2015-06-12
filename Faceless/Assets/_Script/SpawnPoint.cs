using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class SpawnPoint : MonoBehaviour {

	public int id;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		this.name = "SpawnPoint_id_" + id;
	}
}
