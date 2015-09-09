using UnityEngine;
using System.Collections;

public class SliderDoor : MonoBehaviour {
	public GameObject myTrigger;
//	public float toBeFullyOpen;
//	public float toBeFullyClose;
	public Vector3 myOpenPosition;
	private Vector3 myClosePosition;
	private float currentPos;
	// Use this for initialization
	void Start () {
		myClosePosition = this.transform.position;
		myOpenPosition += this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		currentPos = Mathf.Lerp(currentPos, myTrigger.GetComponent<WindMill>().forceToRot,1f*Time.deltaTime);
		this.transform.position = Vector3.Lerp(myClosePosition,myOpenPosition,currentPos/myTrigger.GetComponent<WindMill>().max);
	}
}
