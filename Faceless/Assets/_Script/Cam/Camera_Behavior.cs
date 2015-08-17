using UnityEngine;
using System.Collections;

public class Camera_Behavior : MonoBehaviour {
	

	public Vector3 offset;
	private Transform target;
	
	public float xDamping = 1;
	public float yDamping = 1;

	private float shakeRemove = 0.01f;
	private float shakeForce;
	private Vector2 posOrigin;
	
	private Vector3 currentVelocity;

	
	

	void Start () {
		target = GameManager.Instance.Avatar.gameObject.transform;
	}
	

	void Update () {
		
		Vector3 aheadTargetPos = target.position + offset;
		Vector3 newPos;
		newPos.x = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref currentVelocity, xDamping).x;
		newPos.y = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref currentVelocity, yDamping).y;
		//newPos.y = Vector3.MoveTowards(transform.position, aheadTargetPos, yDamping * 100 * Time.deltaTime).y;
		newPos.z = offset.z;
		
		transform.position = newPos;
		

		if (shakeForce > 0){
			posOrigin = this.transform.position;
			transform.position = posOrigin + Random.insideUnitCircle * shakeForce;
			transform.position = new Vector3(this.transform.position.x,this.transform.position.y,offset.z);
			shakeForce -= shakeRemove;
		}
	}

	public void CameraShake(){
		posOrigin = this.transform.position;
		shakeForce = 0.3f;
	}

}
