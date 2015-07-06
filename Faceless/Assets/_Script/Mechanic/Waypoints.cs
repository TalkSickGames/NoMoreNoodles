using UnityEngine;
using System.Collections;

public class Waypoints : MonoBehaviour {
	public enum wayType{Plateforme,Saws,Bat};
	public wayType myWayType;

	// Use this for initialization
	void OnDrawGizmos() {
		if(myWayType == wayType.Plateforme){
			Gizmos.color =new Color(0.2f,1f,0.2f,0.5f);
			Gizmos.DrawCube(this.transform.position,new Vector3(6f,2f,2f));
		}
		if(myWayType == wayType.Saws){
			Gizmos.color =new Color(1f,0.2f,0.2f,0.5f);
			Gizmos.DrawSphere(this.transform.position,2f);
		}
		if(myWayType == wayType.Bat){
			Gizmos.color =new Color(0.3f,0.3f,0.3f,0.5f);
			Gizmos.DrawSphere(this.transform.position,0.5f);
		}
	}
}
