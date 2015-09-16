using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Canvas_UI : MonoBehaviour {
	public GameObject[] hearts;
	public GameObject[] trickAmmo;
	public GameObject[] powerAmmo;
	public GameObject[] timeAmmo;

	public GameObject trickCursor;
	public GameObject powerCursor;
	public GameObject[] masks;
	public GameObject[] sMasks;
	// Use this for initialization
	void Start () {

	

	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetAxis("DpadH") < -0.2f || Input.GetAxis("DpadH") > 0.2f || Input.GetAxis("DpadV") < -0.2f || Input.GetAxis("DpadV") > 0.2f){
		//if(Input.GetButton("DpadU") || Input.GetButton("DpadR") || Input.GetButton("DpadD") || Input.GetButton("DpadL")){
			masks[0].SetActive(true);
			masks[1].SetActive(true);
			masks[2].SetActive(true);
			masks[3].SetActive(true);
		}else{
			masks[0].SetActive(false);
			masks[1].SetActive(false);
			masks[2].SetActive(false);
			masks[3].SetActive(false);
		}

//		sMasks[0].SetActive(((int)GameManager.Instance.AvatarB.MyMask == 0)?true:false);
//		sMasks[1].SetActive(((int)GameManager.Instance.AvatarB.MyMask == 1)?true:false);
//		sMasks[2].SetActive(((int)GameManager.Instance.AvatarB.MyMask == 2)?true:false);
//		sMasks[3].SetActive(((int)GameManager.Instance.AvatarB.MyMask == 3)?true:false);
//
//		trickCursor.SetActive(GameManager.Instance.AvatarB.ChargeIsTrick);
		trickAmmo[0].SetActive((GameManager.Instance.AvatarB.Ammo >= 1)?true:false);
		trickAmmo[1].SetActive((GameManager.Instance.AvatarB.Ammo >= 2)?true:false);
		trickAmmo[2].SetActive((GameManager.Instance.AvatarB.Ammo >= 3)?true:false);
		trickAmmo[3].SetActive((GameManager.Instance.AvatarB.Ammo >= 4)?true:false);
//		powerAmmo[0].SetActive((GameManager.Instance.AvatarB.PowerAmmo >= 1)?true:false);
//		powerAmmo[1].SetActive((GameManager.Instance.AvatarB.PowerAmmo >= 2)?true:false);
//		powerCursor.SetActive(!GameManager.Instance.AvatarB.ChargeIsTrick);
		////////////////

		hearts[0].SetActive((GameManager.Instance.AvatarB.HP >= 1)?true:false);
		hearts[1].SetActive((GameManager.Instance.AvatarB.HP >= 2)?true:false);
		hearts[2].SetActive((GameManager.Instance.AvatarB.HP >= 3)?true:false);
		hearts[3].SetActive((GameManager.Instance.AvatarB.HP >= 4)?true:false);
		hearts[4].SetActive((GameManager.Instance.AvatarB.HP >= 5)?true:false);
		hearts[5].SetActive((GameManager.Instance.AvatarB.HP >= 6)?true:false);
		/////////////

//		timeAmmo[0].SetActive((GameManager.Instance.AvatarB.TimeAmmo >= 1)?true:false);
//		timeAmmo[1].SetActive((GameManager.Instance.AvatarB.TimeAmmo >= 2)?true:false);
//		timeAmmo[2].SetActive((GameManager.Instance.AvatarB.TimeAmmo >= 3)?true:false);
//		timeAmmo[3].SetActive((GameManager.Instance.AvatarB.TimeAmmo >= 4)?true:false);
	}
}
