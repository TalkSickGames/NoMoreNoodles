using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Foo;
public class RespawnManager : MonoBehaviour {
	public List<Respawnable> myRespawn = new List <Respawnable> ();

	public GameObject pickupTemp;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
		foreach(Respawnable myRes in myRespawn){
			myRes.myTime -= 1f*Time.deltaTime;
			if(myRes.myTime <= 0f){
				GameObject temp	= Instantiate(pickupTemp,myRes.myPos,Quaternion.identity) as GameObject;
				temp.gameObject.GetComponent<PickUps>().myType = myRes.myTy;
				temp.gameObject.GetComponent<PickUps>().isRespawnable = true;
				temp.gameObject.GetComponent<PickUps>().respawnTime = myRes.myTimeToRespawn;
			}

		}
		myRespawn.RemoveAll(Respawnable => Respawnable.myTime <= 0f);
	}

	public List<Respawnable> MyRespawn{
		get{return myRespawn;}
		set{myRespawn = value;}
	}


}
public class Respawnable{
	
	public PickupType myTy;
	public Vector3 myPos;
	public float myTime;
	public float myTimeToRespawn;
	
	public Respawnable(PickupType tMyTy,Vector3 tMyPos, float tMyTime){
		myTy = tMyTy;
		myPos = tMyPos;
		myTime = tMyTime;
		myTimeToRespawn = tMyTime;
	}
}