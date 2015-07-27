using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Canvas_UI : MonoBehaviour {
	public GameObject[] hearts;
	public GameObject[] trickAmmo;
	public GameObject powerAmmo;
	public GameObject trickCursor;
	public GameObject powerCursor;
	// Use this for initialization
	void Start () {

	

	}
	
	// Update is called once per frame
	void Update () {
		trickCursor.SetActive(GameManager.Instance.AvatarB.ChargeIsTrick);
		trickAmmo[0].SetActive((GameManager.Instance.AvatarB.TrickAmmo >= 1)?true:false);
		trickAmmo[1].SetActive((GameManager.Instance.AvatarB.TrickAmmo >= 2)?true:false);
		trickAmmo[2].SetActive((GameManager.Instance.AvatarB.TrickAmmo >= 3)?true:false);
		trickAmmo[3].SetActive((GameManager.Instance.AvatarB.TrickAmmo >= 4)?true:false);
		powerAmmo.SetActive((GameManager.Instance.AvatarB.PowerAmmo >= 1)?true:false);
		powerCursor.SetActive(!GameManager.Instance.AvatarB.ChargeIsTrick);
		////////////////

		hearts[0].SetActive((GameManager.Instance.AvatarB.HP >= 1)?true:false);
		hearts[1].SetActive((GameManager.Instance.AvatarB.HP >= 2)?true:false);
		hearts[2].SetActive((GameManager.Instance.AvatarB.HP >= 3)?true:false);
		hearts[3].SetActive((GameManager.Instance.AvatarB.HP >= 4)?true:false);
		hearts[4].SetActive((GameManager.Instance.AvatarB.HP >= 5)?true:false);
		hearts[5].SetActive((GameManager.Instance.AvatarB.HP >= 6)?true:false);
	}
}
