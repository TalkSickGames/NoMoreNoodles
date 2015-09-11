using UnityEngine;
using System.Collections;

public class SliderDoor : MonoBehaviour {
	public GameObject myWindMill;
	public GameObject myMoveableSwitch;
	public bool isStomper;
	public bool isDynamo;
	public bool isLevered;

//	public float toBeFullyOpen;
//	public float toBeFullyClose;
	public Vector3 myOpenPosition;
	public float weight;
	public float need;
	private Vector3 myClosePosition;
	private float currentPos;
	private float gravity;
	// Use this for initialization
	void Start () {
		myClosePosition = this.transform.position;
		myOpenPosition += this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if(myWindMill != null){
			if(isStomper){ UpdateStomper();}
			if(isDynamo){ UpdateDynamo();}
			if(isLevered){ UpdateLevel();}
		}

		if(myMoveableSwitch != null){
			UpdateMovable();
		}

	}
	void UpdateMovable() {
		//currentPos = Mathf.Lerp(currentPos, myWindMill.GetComponent<WindMill>().Force,1f*Time.deltaTime);
		this.transform.position = Vector3.Lerp(myClosePosition,myOpenPosition,myMoveableSwitch.GetComponent<movableSwitch>().posInSlide/100f);
	}


	void UpdateStomper() {

			if(myWindMill.GetComponent<WindMill>().Force >= weight){
				currentPos += myWindMill.GetComponent<WindMill>().Force * Time.deltaTime;
				gravity = 0f;
			}
			gravity += weight*Time.deltaTime;
			currentPos -= gravity;
			currentPos = Mathf.Clamp(currentPos,0f,need);
			//currentPos = Mathf.Lerp(currentPos, myWindMill.GetComponent<WindMill>().Force,1f*Time.deltaTime);
			this.transform.position = Vector3.Lerp(myClosePosition,myOpenPosition,currentPos/need/*myWindMill.GetComponent<WindMill>().max*/);

	}

	void UpdateDynamo() {

			//currentPos = Mathf.Clamp(currentPos,0f,need);
			currentPos = Mathf.Lerp(currentPos, myWindMill.GetComponent<WindMill>().Force,1f*Time.deltaTime);
			this.transform.position = Vector3.Lerp(myClosePosition,myOpenPosition,currentPos/myWindMill.GetComponent<WindMill>().max);

	}

	void UpdateLevel(){
		currentPos += myWindMill.GetComponent<WindMill>().Force*Time.deltaTime;
		currentPos = Mathf.Clamp(currentPos,need*-1f,need);
		//currentPos = Mathf.Lerp(currentPos, myWindMill.GetComponent<WindMill>().Force,1f*Time.deltaTime);
		this.transform.position = Vector3.Lerp(myClosePosition,myOpenPosition,(currentPos+need)/(need*2));

	}
}
