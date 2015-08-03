using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class NPC_Behavior : MonoBehaviour {

	public GameObject myHead;
	public GameObject myChest;
	public GameObject myLegs;
	//Action
	private FSM myFSM;
	private Action moveToPOI;
	private Action idle;
	private Action thinking;
	//Useful stuff (::
	public Nodes[] myNodes;
	private Vector3 POI;

	private Vector3 jumpTo;

	float tempTimeJump = 0f;
	Vector3 tempPos;
	Vector3 tempPosForCoroutine;
	// Use this for initialization
	void Start () {

		myFSM = new FSM();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Idle(){

	}

	void Jump(){
		tempPosForCoroutine = this.transform.position;
		StartCoroutine("JumpTo");
	}

	IEnumerator JumpTo(){
		while(true){
		//if(!Mathfx.Approx(this.transform.position,jumpTo,0.1f)){
			tempTimeJump += 1.5f*Time.deltaTime;
			tempPos.x = Mathf.Lerp(tempPosForCoroutine.x, jumpTo.x,tempTimeJump);
			tempPos.y = Mathfx.Curve(tempPosForCoroutine.y, jumpTo.y,tempTimeJump);
			tempPos.z = 0f;
			this.transform.position = tempPos;
			yield return null;
		}

	}
}
