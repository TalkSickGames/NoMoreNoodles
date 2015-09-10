using UnityEngine;
using System.Collections;

public class WindMill : MonoBehaviour {
	public float idleSpeed;
	public float resistance;

	public GameObject myPalm;
	public float forceToRot;

	public float max = 20f;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if(!Mathfx.Approx(forceToRot,idleSpeed,1f)){
			if(forceToRot > idleSpeed){
				forceToRot -= resistance*Time.deltaTime;
			}else{
				forceToRot += resistance*Time.deltaTime;
			}
		}else{
			forceToRot = Mathf.Lerp(forceToRot,idleSpeed,1f*Time.deltaTime);
		}


		forceToRot = Mathf.Clamp(forceToRot,max*-1f,max);
		myPalm.transform.Rotate(0f,0f,forceToRot*Time.deltaTime*20f);
	}
	void OnTriggerEnter2D(Collider2D other) {
		if(other.GetComponent<burst>() != null){

			Activate(other.GetComponent<burst>().isTrick,GameManager.Instance.Avatar.transform.position,other.gameObject.transform.rotation.eulerAngles.z);
		}
	}

	void Activate(bool isTrick, Vector2 pos, float dir){
		float baseForce = isTrick?5f:10f;
		float direction = 1f;

		int tempPos = 0;
		Debug.Log(pos);
		if(pos.x > this.transform.position.x && pos.y > this.transform.position.y){
			tempPos = 1;
		}
		if(pos.x > this.transform.position.x && pos.y < this.transform.position.y){
			tempPos = 2;
		}
		if(pos.x < this.transform.position.x && pos.y < this.transform.position.y){
			tempPos = 3;
		}
		if(pos.x < this.transform.position.x && pos.y > this.transform.position.y){
			tempPos = 4;
		}
		Debug.Log(tempPos);
		Debug.Log(dir);
		if(Mathfx.Approx(dir,0f,1f)){
			if(tempPos == 1){direction = 1f; }
			if(tempPos == 2){direction = 1f; }
			if(tempPos == 3){direction = -1f; }
			if(tempPos == 4){direction = -1f; }
		}
		if(Mathfx.Approx(dir,315f,1f)){
			if(tempPos == 1){direction = -1f; }
			if(tempPos == 2){direction = 1f; }
			if(tempPos == 3){direction = 1f; }
			if(tempPos == 4){direction = -1f; }
		}
		if(Mathfx.Approx(dir,270f,1f)){
			if(tempPos == 1){direction = -1f; }
			if(tempPos == 2){direction = 1f; }
			if(tempPos == 3){direction = 1f; }
			if(tempPos == 4){direction = -1f; }
		}
		if(Mathfx.Approx(dir,225f,1f)){
			if(tempPos == 1){direction = -1f; }
			if(tempPos == 2){direction = -1f; }
			if(tempPos == 3){direction = 1f; }
			if(tempPos == 4){direction = 1f; }
		}
		if(Mathfx.Approx(dir,180f,1f)){
			if(tempPos == 1){direction = -1f; }
			if(tempPos == 2){direction = -1f; }
			if(tempPos == 3){direction = 1f; }
			if(tempPos == 4){direction = 1f; }
		}
		if(Mathfx.Approx(dir,135f,1f)){
			if(tempPos == 1){direction = 1f; }
			if(tempPos == 2){direction = -1f; }
			if(tempPos == 3){direction = -1f; }
			if(tempPos == 4){direction = 1f; }
		}
		if(Mathfx.Approx(dir,90f,1f)){
			if(tempPos == 1){direction = 1f; }
			if(tempPos == 2){direction = -1f; }
			if(tempPos == 3){direction = -1f; }
			if(tempPos == 4){direction = 1f; }
		}
		if(Mathfx.Approx(dir,45f,1f)){
			if(tempPos == 1){direction = 1f; }
			if(tempPos == 2){direction = 1f; }
			if(tempPos == 3){direction = -1f; }
			if(tempPos == 4){direction = -1f; }
		}
		Debug.Log(direction);
		forceToRot += baseForce * direction;
		

	}
}
