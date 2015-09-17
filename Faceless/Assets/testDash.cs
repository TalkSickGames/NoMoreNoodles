using UnityEngine;
using System.Collections;

public class testDash : MonoBehaviour {
	private bool goingLeft;
	//private Vector2 goingTo;
	private float tTime;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		tTime += Time.deltaTime;
		if(tTime >= 1f){
			tTime = 0f;
			goingLeft = !goingLeft;
		}
		if(goingLeft){
			this.transform.Translate(-2f*Time.deltaTime,0f,0f);
		}else{
			this.transform.Translate(2f*Time.deltaTime,0f,0f);
		}
	}
}
