using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class NPC_Behavior : MonoBehaviour {

	public GameObject myHead;
	public GameObject myChest;
	public GameObject myLegs;
	private Animator myHeadA;
	private Animator myChestA;
	private Animator myLegsA;
	//Action
	private FSM myFSM;
	private Action moveToPOI;
	private Action idle;
	private Action thinking;
	//Useful stuff (::
	public Nodes[] myNodes;
	public float speed;
	public float jumpHeight;
	private Vector3 POI;

	private Vector3 jumpTo;

	float tempTimeJump = 0f;
	Vector3 tempPos;
	Vector3 tempPosForCoroutine;
	// Use this for initialization

	//state 4 anim
	private bool isIdle;
	private bool isWalking;
	private bool isJumping;
	private bool isFalling;
	void Start () {
		myHeadA = myHead.GetComponent<Animator> ();
		myChestA = myChest.GetComponent<Animator> ();
		myLegsA = myLegs.GetComponent<Animator> ();
		myFSM = new FSM();
		isIdle = true;
	}
	
	// Update is called once per frame
	void Update () {
		UpdateAnim ();
		if (isIdle) {
			StateAnimReset();
			isWalking = true;
		}
		if(isWalking){

			if (Physics2D.Raycast (this.transform.position, Vector2.right*Mathf.Sign(speed),0.5f)) {
				if(CheckForJumpHeight((Vector2)this.transform.position + Vector2.right*Mathf.Sign(speed) * 1f) != default(Vector3)){
					Jump(CheckForJumpHeight((Vector2)this.transform.position + Vector2.right*Mathf.Sign(speed) * 1f));
				}else{
					speed *= -1f;
				}
			}

			if (!Physics2D.Raycast ((Vector2)this.transform.position + (Vector2.right*Mathf.Sign(speed)*0.5f),Vector2.down,0.8f)) {
				Debug.Log("Jump Down");
				if(CheckForJumpHeightDown((Vector2)this.transform.position + Vector2.right*Mathf.Sign(speed) * 1f) != default(Vector3)){
					Jump(CheckForJumpHeight((Vector2)this.transform.position + Vector2.right*Mathf.Sign(speed) * 1f));
				}else{
					speed *= -1f;
				}
			}else{
				this.transform.Translate(speed*Time.deltaTime,0f,0f);
			}
		}

	}


	void Jump(Vector3 to){
		jumpTo = to;
		tempPosForCoroutine = this.transform.position;
		StateAnimReset ();
		isJumping = true;
		StartCoroutine("JumpTo");
	}
	void StateAnimReset(){
		isJumping = false;
		isIdle = false;
		isWalking = false;
		isFalling = false;
	}

	void UpdateAnim(){
		Quaternion tempV30 = Quaternion.Euler( new Vector3 (0f, 0f, 0f));
		Quaternion tempV38 = Quaternion.Euler( new Vector3 (0f, 180f, 0f));

		if (isIdle) {
			myHeadA.Play("Idle");
			myChestA.Play("Idle");
			myLegsA.Play("Idle");
		}
		if (isWalking) {
			myHeadA.Play("Walk");
			myChestA.Play("Walk");
			myLegsA.Play("Walk");
		}
		if (isJumping) {
			myHeadA.Play("Jump");
			myChestA.Play("Jump");
			myLegsA.Play("Jump");
		}
		if (speed > 0f) {
			myHead.transform.rotation = tempV30;
			myChest.transform.rotation = tempV30;
			myLegs.transform.rotation = tempV30;
		} else {
			myHead.transform.rotation = tempV38;
			myChest.transform.rotation = tempV38;
			myLegs.transform.rotation = tempV38;
		}
	}
	Vector3 CheckForJumpHeight(Vector2 pos){

//		while(!Physics2D.Raycast(pos,Vector2.down*0.5f)){
//			pos.y += 1f;
//		}
		while(Physics2D.BoxCast( pos,new Vector2(0.1f,0.1f),0f,Vector2.down*0f)){
			pos.y += 1f;
		}
		if ((Physics2D.Raycast (pos, Vector2.down * 0.5f).point + (Vector2.up * 0.75f)).y - this.transform.position.y <= jumpHeight) {
			return Physics2D.Raycast (pos, Vector2.down * 0.5f).point + (Vector2.up * 0.75f);
		} else {
			return default(Vector3);
		}
	}

	Vector3 CheckForJumpHeightDown(Vector2 pos){
		


		if ((Physics2D.Raycast (pos, Vector2.down , jumpHeight+ 0.75f))) {
			return Physics2D.Raycast (pos, Vector2.down , jumpHeight + 0.75f).point;
		} else {
			return default(Vector3);
		}
	}
	IEnumerator JumpTo(){
		while(true){
		
			tempTimeJump += 1.5f*Time.deltaTime;
			tempPos.x = Mathf.Lerp(tempPosForCoroutine.x, jumpTo.x,tempTimeJump);
			tempPos.y = Mathfx.Curve(tempPosForCoroutine.y, jumpTo.y,tempTimeJump);
			tempPos.z = 0f;
			this.transform.position = tempPos;

			if(Mathfx.Approx(this.transform.position,jumpTo,0.05f)){

				StateAnimReset();
				isIdle = true;
				tempTimeJump = 0f;
				StopCoroutine("JumpTo");


			}
			yield return null;
		}
	
	}
}
