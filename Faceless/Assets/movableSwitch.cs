using UnityEngine;
using System.Collections;

public class movableSwitch : MonoBehaviour {
	public bool affectedByShot;
	public Vector2 myActivePosition;
	public float resistance;
	public bool upDown;
	public Vector2 effect;
	private Vector3 myClosePosition;
	//private float currentPos;
	public bool avatarIsOn;
	public float posInSlide;
	private Rigidbody2D myRigid;
	// Use this for initialization
	void Start () {
		myActivePosition += (Vector2)this.transform.position;
		myClosePosition = (Vector2)this.transform.position;
		myRigid = this.GetComponent<Rigidbody2D>();
		if(upDown){myActivePosition.x = this.transform.position.x;}
		if(!upDown){myActivePosition.y = this.transform.position.y;}
	}
	
	// Update is called once per frame
	void Update () {
		myRigid.velocity = Vector2.Lerp(myRigid.velocity,Vector2.zero,(1f*resistance)*Time.deltaTime);
		posInSlide = 100f*Vector2.Distance(myActivePosition,this.transform.position)/Vector2.Distance(myActivePosition,myClosePosition);
		Push(effect*Time.deltaTime);
//
//		if(avatarIsOn){
//			avatarIsOn = false;
//		}
		if(!upDown){
			if(this.transform.position.x < Mathfx.IsDouxClamp(this.transform.position.x,myActivePosition.x,myClosePosition.x) || this.transform.position.x > Mathfx.IsDouxClamp(this.transform.position.x,myActivePosition.x,myClosePosition.x)){
				myRigid.velocity = Vector2.zero;
			}
			this.transform.position = new Vector3(Mathfx.IsDouxClamp(this.transform.position.x,myActivePosition.x,myClosePosition.x), myClosePosition.y, 0f);
//			myRigid.velocity = new Vector2(0f,Mathf.Clamp(myRigid.velocity.y,(Mathf.Abs(myActivePosition.y - this.transform.position.y)/2f)*-1f,Mathf.Abs(myActivePosition.y - this.transform.position.y)/2f));
//			if(Mathf.Abs(myActivePosition.y - this.transform.position.y)<0.1f){
//				myRigid.velocity = new Vector2(0f,0f);
//				this.transform.position = myActivePosition;
//			}
		}else{
			if(avatarIsOn){
				myRigid.velocity += Vector2.down*2f * Time.deltaTime;
			}
			if(this.transform.position.y < Mathfx.IsDouxClamp(this.transform.position.y,myActivePosition.y,myClosePosition.y) || this.transform.position.y > Mathfx.IsDouxClamp(this.transform.position.y,myActivePosition.y,myClosePosition.y)){
				myRigid.velocity = Vector2.zero;
			}
			this.transform.position = new Vector3(myClosePosition.x,Mathfx.IsDouxClamp(this.transform.position.y,myActivePosition.y,myClosePosition.y), 0f);
//			myRigid.velocity = new Vector2(    Mathf.Clamp(     myRigid.velocity.x,   (Mathf.Abs(myActivePosition.x - this.transform.position.x)/2f)*-1f,     Mathf.Abs(myActivePosition.x - this.transform.position.x)/2f)  ,0f  );
//			if(Mathf.Abs(myActivePosition.x - this.transform.position.x)<0.1f){
//				myRigid.velocity = new Vector2(0f,0f);
//				this.transform.position = myActivePosition;
//			}
		}
		avatarIsOn = false;
		//Debug.Log(myRigid.velocity);

	}

	void OnTriggerEnter2D(Collider2D other) {
		if(other.GetComponent<burst>() != null && affectedByShot){
			float tempF;
			if(other.GetComponent<burst>().isTrick){
				tempF = 1f/Vector2.Distance(this.transform.position,GameManager.Instance.Avatar.transform.position);
			}else{
				tempF = 5f/Vector2.Distance(this.transform.position,GameManager.Instance.Avatar.transform.position);
			}
		
			Push((this.transform.position-GameManager.Instance.Avatar.transform.position).normalized*tempF);
		}

	}

	void OnCollisionStay2D(Collision2D other) {
		if(other.gameObject.GetComponent<Avatar_BehaviorV2>() != null && !avatarIsOn && !upDown){
			myRigid.velocity = Vector2.Lerp(myRigid.velocity,new Vector2(other.gameObject.GetComponent<Avatar_BehaviorV2>().totalMovement.x,0f),1f*Time.deltaTime);
			//Push (Vector2.right*(this.transform.position.x-other.gameObject.transform.position.x));
			//Push (Vector2.right*(other.gameObject.GetComponent<Avatar_BehaviorV2>().totalMovement.x*Time.deltaTime));
			
		}
	}

	void Push(Vector2 vel){
		myRigid.velocity += vel;
	}
}
