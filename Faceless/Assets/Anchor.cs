using UnityEngine;
using System.Collections;

public class Anchor : MonoBehaviour {
    public float visibleDistance;
    public float activeDistance;
    public float force;
    public bool isDirected;
    public float direction;
    public float angle;
    public float opening;
    private Vector3 myCheckAngle;
    private SpriteRenderer mySprite;
    private Vector3 myDest;
    public bool debug;
    public bool isSelected;
    //private bool isOnRight;
    // Use this for initialization
    void Start () {
        myCheckAngle = (Quaternion.Euler(0f, 0f, angle * -1f) * Vector3.up);
        mySprite = this.GetComponent<SpriteRenderer>();
        mySprite.enabled = false;
        mySprite.color = Color.red;
        
        myDest = this.transform.position + ((Quaternion.Euler(0f, 0f, direction * -1f) * Vector3.up) * force);
    }
	
	// Update is called once per frame
	void Update () {

        if (isDirected && GameManager.Instance.AvatarB.IsDashing && Vector3.Distance(GameManager.Instance.Avatar.transform.position, this.transform.position) < 0.5f) {
            float distance = Vector3.Distance(GameManager.Instance.Avatar.transform.position, myDest);
            Vector3 angle = (myDest - GameManager.Instance.Avatar.transform.position).normalized;
            GameManager.Instance.AvatarB.DoDash(distance, angle);
        }

        if (Vector3.Distance(GameManager.Instance.Avatar.transform.position, this.transform.position) < visibleDistance) {
            if (!mySprite.enabled) {
                mySprite.enabled = true;
            }
            if (Vector3.Distance(GameManager.Instance.Avatar.transform.position, this.transform.position) < activeDistance) {
                if ((opening > 360f) ? true : (Mathf.Abs( Vector3.Angle((GameManager.Instance.Avatar.transform.position - this.transform.position).normalized, myCheckAngle)) < (opening / 2f))) {
                    if (!GameManager.Instance.AvatarB.myAnchors.Contains(this)) {
                        mySprite.color = Color.green;
                        GameManager.Instance.AvatarB.myAnchors.Add(this);
                    }
                } else {
                    if (GameManager.Instance.AvatarB.myAnchors.Contains(this)) {
                        mySprite.color = Color.red;
                        GameManager.Instance.AvatarB.myAnchors.Remove(this);
                    }
                }


            }else {
                if (GameManager.Instance.AvatarB.myAnchors.Contains(this)) {
                    mySprite.color = Color.red;
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
        //if (isSelected) {
        //    mySprite.color = Color.white;
        //} else {
        //    mySprite.color = Color.black;
        //}
        isSelected = false;
    }

    void OnDrawGizmos() {

        if (isDirected) {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(this.transform.position + ((Quaternion.Euler(0f, 0f, direction * -1f) * Vector3.up) * force), 0.1f);
            Gizmos.DrawLine(this.transform.position, this.transform.position + ((Quaternion.Euler(0f, 0f, direction * -1f) * Vector3.up) * force));
        }
        if (opening < 360f && opening > 0f) {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(this.transform.position, this.transform.position + ((Quaternion.Euler(0f, 0f, angle*-1f) * Vector3.up) * activeDistance));
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(this.transform.position, this.transform.position + ((Quaternion.Euler(0f, 0f, angle * -1f + (opening / 2f)) * Vector3.up) * activeDistance));
            Gizmos.DrawLine(this.transform.position, this.transform.position + ((Quaternion.Euler(0f, 0f, angle * -1f - (opening / 2f)) * Vector3.up) * activeDistance));
        }
    }

    public Vector2 MyDest{
        get{return myDest;}
    }

    public void GetDash() {
        if (!isDirected) {
            float distance = Vector3.Distance(GameManager.Instance.Avatar.transform.position, this.transform.position) + force;
            Vector3 angle = (this.transform.position - GameManager.Instance.Avatar.transform.position).normalized;
            GameManager.Instance.AvatarB.DoDash(distance, angle);
        } else {
            float distance = Vector3.Distance(GameManager.Instance.Avatar.transform.position, this.transform.position);
            Vector3 angle = (this.transform.position - GameManager.Instance.Avatar.transform.position).normalized;
            GameManager.Instance.AvatarB.DoDash(distance, angle);
        }
    }

    //public bool IsOnRight {
    //    get {return isOnRight;}
    //}
}
