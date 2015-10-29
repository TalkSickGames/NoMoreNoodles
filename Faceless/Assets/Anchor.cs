using UnityEngine;
using System.Collections;

public class Anchor : MonoBehaviour {
//	public GameObject middle;
	public GameObject end;
    public float visibleDistance;
    public float activeDistance;
    private float force;
    public bool isDirected;
	public bool limitedOpening;
   	private float direction;
    public float angle;
    private float opening;
    private Vector3 myCheckAngle;
    private SpriteRenderer mySprite;
//	private Vector3 myDest;
	private bool wasActivated;
//    public bool debug;
    public bool isSelected;
    //private bool isOnRight;
    // Use this for initialization
    void Start () {
		force = Vector3.Distance(this.transform.position, end.transform.position);

		if(limitedOpening){
			opening = 90f;
		}else{
			opening = 360f;
		}
        myCheckAngle = (Quaternion.Euler(0f, 0f, angle * -1f) * Vector3.up);
        mySprite = this.GetComponent<SpriteRenderer>();
        mySprite.enabled = false;
        mySprite.color = Color.red;
        
		end.transform.position = end.transform.position;
    }
	
	// Update is called once per frame
	void Update () {
		end.transform.position = end.transform.position;
        if (/*isDirected && */wasActivated && GameManager.Instance.AvatarB.IsDashing && Vector3.Distance(GameManager.Instance.Avatar.transform.position, this.transform.position) < 0.5f) {
            float distance = Vector3.Distance(GameManager.Instance.Avatar.transform.position, end.transform.position);
            Vector3 angle = (end.transform.position - GameManager.Instance.Avatar.transform.position).normalized;
			if(force != 0f){
				GameManager.Instance.AvatarB.DoProp(end.transform.position);
				//GameManager.Instance.AvatarB.DoProp(this.transform.position,middle.transform.position,end.transform.position);
			}
			wasActivated = false;
           // GameManager.Instance.AvatarB.DoDash(distance, angle);
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
        //isSelected = false;
    }

    void OnDrawGizmos() {
//		if(end != null){
			
//		}
        if (isDirected) {
			Gizmos.color = Color.green;
			for(float i = -1f; i < 2f; i+=0.1f){
				Gizmos.DrawLine(Mathfx.TestCurve(this.transform.position,end.transform.position,i),Mathfx.TestCurve(this.transform.position,end.transform.position,i+0.1f));
			}

//            Gizmos.color = Color.blue;
//            Gizmos.DrawSphere(end.transform.position, 0.1f);
//            Gizmos.DrawLine(this.transform.position, end.transform.position);
        }
        if (limitedOpening) {
			opening = 90f;
            Gizmos.color = Color.red;
            Gizmos.DrawLine(this.transform.position, this.transform.position + ((Quaternion.Euler(0f, 0f, angle*-1f) * Vector3.up) * activeDistance));
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(this.transform.position, this.transform.position + ((Quaternion.Euler(0f, 0f, angle * -1f + (opening / 2f)) * Vector3.up) * activeDistance));
            Gizmos.DrawLine(this.transform.position, this.transform.position + ((Quaternion.Euler(0f, 0f, angle * -1f - (opening / 2f)) * Vector3.up) * activeDistance));
        }
		if(isSelected){
			Gizmos.color = Color.blue;
			Gizmos.DrawLine(this.transform.position, GameManager.Instance.Avatar.transform.position);
		}
		Gizmos.color = Color.blue;
		Gizmos.DrawSphere(end.transform.position, 0.1f);
		Gizmos.DrawLine(this.transform.position, end.transform.position);
    }

    public Vector2 MyDest{
        get{return end.transform.position;}
    }

    public void GetDash() {
        if (!isDirected) {
//			float distance = Vector3.Distance(GameManager.Instance.Avatar.transform.position, this.transform.position) + force;
//			Vector3 angle = (this.transform.position - GameManager.Instance.Avatar.transform.position).normalized;
//			GameManager.Instance.AvatarB.DoDash(distance, angle);


			Vector3 temp = (this.transform.position - GameManager.Instance.Avatar.transform.position).normalized;
			end.transform.position = this.transform.position + (temp * force);
			float distance = Vector3.Distance(GameManager.Instance.Avatar.transform.position, this.transform.position);
			Vector3 angle = (this.transform.position - GameManager.Instance.Avatar.transform.position).normalized;
			GameManager.Instance.AvatarB.DoDash(distance, angle);

        } else {
            float distance = Vector3.Distance(GameManager.Instance.Avatar.transform.position, this.transform.position);
            Vector3 angle = (this.transform.position - GameManager.Instance.Avatar.transform.position).normalized;
            GameManager.Instance.AvatarB.DoDash(distance, angle);
        }
		wasActivated = true;
    }

    //public bool IsOnRight {
    //    get {return isOnRight;}
    //}
}
