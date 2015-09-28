using UnityEngine;
using System.Collections;

public class SpriteDash : MonoBehaviour {
	//public Sprite[] mySprites;
	private SpriteRenderer myRender;
	private SpriteRenderer avatar;
	private float tempF;
	// Use this for initialization
	void Start () {
		myRender = this.GetComponent<SpriteRenderer>();
		myRender.sprite = GameManager.Instance.Avatar.GetComponentInChildren<SpriteRenderer>().sprite;

	
	}
	
	// Update is called once per frame
	void Update () {
		tempF += 0.25f*Time.deltaTime;
		myRender.material.color = new Color(myRender.material.color.r,myRender.material.color.g,myRender.material.color.b,myRender.material.color.a-tempF);
		if(myRender.material.color.a<=0.1f){
			Destroy(this.gameObject);
		}
	}
}
