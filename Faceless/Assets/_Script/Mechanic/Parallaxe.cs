using UnityEngine;
using System.Collections;

public class Parallaxe : MonoBehaviour {
	private Camera myCam;
	public float speed;
//	private Vector3 camStartPos;
	private Vector3 myStartPos;
	// Use this for initialization
	void Start () {
		myCam = GameManager.Instance.MainCamera.GetComponent<Camera>();
//		camStartPos = myCam.transform.position;
		myStartPos = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.position = myStartPos + (Vector3.Lerp(Vector3.zero,myCam.transform.position,speed));
		this.transform.position = new Vector3(this.transform.position.x,this.transform.position.y,myStartPos.z);
	}
}
