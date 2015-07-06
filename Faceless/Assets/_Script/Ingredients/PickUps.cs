using UnityEngine;
using System.Collections;

public class PickUps : MonoBehaviour {
	public enum PickupType { Ammo, Life, Focus};
	public PickupType myType;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other) {
		if(other.gameObject.tag == "Player"){
			switch(myType){

			case PickupType.Ammo :
				GameManager.Instance.AvatarB.PowerAmmo += 1;
				break;
			}
			Destroy(this.gameObject);
		}

	}
}
