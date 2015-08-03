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

	// Use this for initialization
	void Start () {
		myFSM = new FSM();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Idle(){

	}
}
