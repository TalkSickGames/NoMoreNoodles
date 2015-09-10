using UnityEngine;
using System.Collections;

public class DamageBlock : MonoBehaviour {
	public int damage;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.GetComponent<Avatar_BehaviorV2>() != null){
			coll.gameObject.GetComponent<Avatar_BehaviorV2>().TakeDamage(damage,(float)damage*10f,this.transform.position);
		}
		
	}

	void OnCollisionStay2D(Collision2D coll) {
		if (coll.gameObject.GetComponent<Avatar_BehaviorV2>() != null){
			coll.gameObject.GetComponent<Avatar_BehaviorV2>().TakeDamage(damage,(float)damage*10f,this.transform.position);
		}
		
	}
}
