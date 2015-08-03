using UnityEngine;
using System.Collections;

public class particle_killTime : MonoBehaviour {
	private ParticleSystem mySystem;
	// Use this for initialization
	void Start () {
		mySystem = this.GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
		if(mySystem.isStopped){
			Destroy(this.gameObject);
		}
	}
}
