using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Canvas_UI : MonoBehaviour {

	// Use this for initialization
	void Start () {

	

	}
	
	// Update is called once per frame
	void Update () {
		this.transform.GetChild(0).gameObject.SetActive(GameManager.Instance.AvatarB.ChargeIsTrick);
		this.transform.GetChild(1).gameObject.SetActive((GameManager.Instance.AvatarB.TrickAmmo >= 1)?true:false);
		this.transform.GetChild(2).gameObject.SetActive((GameManager.Instance.AvatarB.TrickAmmo >= 2)?true:false);
		this.transform.GetChild(3).gameObject.SetActive((GameManager.Instance.AvatarB.TrickAmmo >= 3)?true:false);
		this.transform.GetChild(4).gameObject.SetActive((GameManager.Instance.AvatarB.TrickAmmo >= 4)?true:false);
		this.transform.GetChild(5).gameObject.SetActive((GameManager.Instance.AvatarB.PowerAmmo >= 1)?true:false);
		this.transform.GetChild(6).gameObject.SetActive(!GameManager.Instance.AvatarB.ChargeIsTrick);
		
	}
}
