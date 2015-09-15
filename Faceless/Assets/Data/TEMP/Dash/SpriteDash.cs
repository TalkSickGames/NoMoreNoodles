﻿using UnityEngine;
using System.Collections;

public class SpriteDash : MonoBehaviour {
	//public Sprite[] mySprites;
	private SpriteRenderer myRender;
	private SpriteRenderer avatar;
	// Use this for initialization
	void Start () {
		myRender = this.GetComponent<SpriteRenderer>();
		myRender.sprite = GameManager.Instance.Avatar.GetComponentInChildren<SpriteRenderer>().sprite;

	
	}
	
	// Update is called once per frame
	void Update () {
		myRender.material.color = new Color(myRender.material.color.r,myRender.material.color.g,myRender.material.color.b,Mathf.Lerp(myRender.material.color.a,0f,5f*Time.deltaTime));

	}
}
