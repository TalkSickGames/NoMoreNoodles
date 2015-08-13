using UnityEngine;
using System.Collections;
using Foo;

public class PickUps : MonoBehaviour {
	
	public PickupType myType;
	public bool isRespawnable;
	public float respawnTime;
	private Vector3 respawnTo;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//Gizmos.DrawCube(respawnTo,new Vector3(this.gameObject.GetComponent<BoxCollider2D>().size.x,this.gameObject.GetComponent<BoxCollider2D>().size.x,this.gameObject.GetComponent<BoxCollider2D>().size.x));
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		if(other.gameObject.tag == "Player"){
			switch(myType){
			case PickupType.Focus :
				GameManager.Instance.AvatarB.TimeAmmo += 1;
				break;	
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
				respawnTo = this.transform.position;
				Invoke("Respawn",respawnTime);
				this.transform.position = Vector3.up*99999f;
			}else{
				Destroy(this.gameObject);
			}
			
		}
		
	}
	
	//	void OnDrawGizmos(){
	//		Gizmos.DrawCube(respawnTo,new Vector3(this.gameObject.GetComponent<BoxCollider2D>().size.x,this.gameObject.GetComponent<BoxCollider2D>().size.x,this.gameObject.GetComponent<BoxCollider2D>().size.x));
	//
	//	}
	
	void Respawn(){
		//if(!Physics2D.BoxCast(new Vector2(respawnTo.x,respawnTo.y),new Vector2(0.5f,0.5f),0f,Vector2.up)){
		this.transform.position = respawnTo;
		//		}else{
		//		
		//			Debug.Log("sad");
		//			Invoke("Respawn",1f);
		//		}
		
	}
}
