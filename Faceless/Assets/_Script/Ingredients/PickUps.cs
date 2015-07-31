using UnityEngine;
using System.Collections;
using Foo;

public class PickUps : MonoBehaviour {

	public PickupType myType;
	public bool isRespawnable;
	public float respawnTime;
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
			case PickupType.HalfHeart:
				GameManager.Instance.AvatarB.HP += 1;
				break;
			case PickupType.FullHeart:
				GameManager.Instance.AvatarB.HP += 2;
				break;
			}
			if(isRespawnable){
				GameManager.Instance.gameObject.GetComponent<RespawnManager>().MyRespawn.Add( new Respawnable(myType, this.gameObject.transform.position, respawnTime));
			}
			Destroy(this.gameObject);
		}

	}
}
