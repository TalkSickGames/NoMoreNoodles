using UnityEngine;
using System.Collections;
[ExecuteInEditMode]
public class testCurve : MonoBehaviour {
	public GameObject start;
	public GameObject end;
	public float value;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.position = Mathfx.TestCurve(start.transform.position,end.transform.position,value);
	}


}
