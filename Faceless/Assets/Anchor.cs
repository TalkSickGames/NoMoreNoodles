using UnityEngine;
using System.Collections;

public class Anchor : MonoBehaviour {
    public float visibleDistance;
    public float activeDistance;
    public float force;
    private SpriteRenderer mySprite;
    //private bool isOnRight;
    // Use this for initialization
    void Start () {
        mySprite = this.GetComponent<SpriteRenderer>();
        mySprite.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (Vector3.Distance(GameManager.Instance.Avatar.transform.position, this.transform.position) < visibleDistance) {
            if (!mySprite.enabled) {
                mySprite.enabled = true;
            }
            if (Vector3.Distance(GameManager.Instance.Avatar.transform.position, this.transform.position) < activeDistance) {
                //if (this.transform.position.x > GameManager.Instance.Avatar.transform.position.x) {
                //    isOnRight = true;
                //} else {
                //    isOnRight = false;
                //}
                if (!GameManager.Instance.AvatarB.myAnchors.Contains(this)) {
                    GameManager.Instance.AvatarB.myAnchors.Add(this);
                }

            }else {
                if (GameManager.Instance.AvatarB.myAnchors.Contains(this)) {
                    GameManager.Instance.AvatarB.myAnchors.Remove(this);
                }

            }


        } else {
            if (GameManager.Instance.AvatarB.myAnchors.Contains(this)) {
                GameManager.Instance.AvatarB.myAnchors.Remove(this);
            }
            if (mySprite.enabled) {
                mySprite.enabled = false;
            }


        }
    }

    //public bool IsOnRight {
    //    get {return isOnRight;}
    //}
}
