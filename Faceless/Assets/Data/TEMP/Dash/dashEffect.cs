using UnityEngine;
using System.Collections;

public class dashEffect : MonoBehaviour {
	private float lifeTime;
	public bool isH;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		lifeTime += Time.deltaTime;
		if(lifeTime>= 0.5f){
			Destroy(this.gameObject);
		}
		if(!isH){
			this.transform.localScale = new Vector3( Mathf.Lerp(this.transform.localScale.x,0f,7.5f*Time.deltaTime),this.transform.localScale.y,1f);
		}else{
			this.transform.localScale = new Vector3( this.transform.localScale.x, Mathf.Lerp(this.transform.localScale.y,0f,7.5f*Time.deltaTime),1f);
		}
		//this.transform.localScale = Vector3.Lerp(this.transform.localScale, Vector3.zero,5f*Time.deltaTime);
		this.GetComponent<MeshRenderer>().material.SetFloat("_BumpAmt",Mathf.Lerp( this.GetComponent<MeshRenderer>().material.GetFloat("_BumpAmt"),5000f, 5f*Time.deltaTime));
	}
}
