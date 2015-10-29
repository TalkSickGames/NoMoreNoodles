using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Foo;

public class Avatar_Behavior : MonoBehaviour {

	public float moveSpeed;
	private float tempMoveSpeed;
	public float jumpForce;
	public float gravity;
	private float tempGravity;
	public float dashSpeed;
	public float dashIdle;
	public float rampSpeed;
	public float airControlSpeed;
	private float moveMultiplier;
	public float dashDistance;
	public LayerMask groundLayer;
	public GameObject myPart;
	
	private int hp;
	private bool isInvincible;
	private bool isLedge;
	private bool isOnLeftLedge;
	private bool isJumping;
	private bool isFalling;
	private bool isMoving;
	private bool isGrounded;
	private bool isCharging;
	private bool isInWater;
	private bool isDashing;
	private bool justTouchGround;
	private bool justWentAir;
    //private bool facingRight;
    private bool isInDecision;

	private bool applied;
	private float chargeCooldown;
	private float rechargeCooldown;
	private int ammo;

	private Vector2 pInputL;
	private Vector2 pInputR;
	//private bool pInput1;
	private float inputAngleR;
	private float inputAngleL;
	private Quaternion angleClamp;
	private DashDir dashDir;
	private DashDir moveDir;
	private Vector2 velocity;
	private float movement;
	private Vector2 effectVelocity;
	private Vector2 parVelocity;
	private Vector3 parPos;
	private GameObject parObj;
	private float timeCharge;
	private bool collCheckNF;
	private float lastFallV;
	private Vector2 dashTo;
	private bool canLedge;
    private float triggerChargeTime;
    //private float lTriggerChargeTime;


    public List<Anchor> myAnchors = new List<Anchor>();
    private Rigidbody2D myRigid;
	private BoxCollider2D myBox;
	private CircleCollider2D myCircle;
	private Animator myAnimator;
	private GameObject mySprite;
	private GameObject myBurst;
//	public PhysicsMaterial2D slip;
//	public PhysicsMaterial2D noSlip;
	public GameObject spriteDash;
	public GameObject dashEffect;
	private Quaternion tempV30 = Quaternion.Euler( new Vector3 (0f, 0f, 0f));
	private Quaternion tempV38 = Quaternion.Euler( new Vector3 (0f, 180f, 0f));
	private Vector3 checkAngle;


	void Start(){
		myRigid = GetComponent<Rigidbody2D> ();
		myBox = GetComponent<BoxCollider2D> ();
		myCircle = GetComponent<CircleCollider2D> ();
		myAnimator = GetComponentInChildren<Animator> ();
		mySprite = GetComponentInChildren<SpriteRenderer>().gameObject;
		myBurst = GetComponentInChildren<PolygonCollider2D>().gameObject;
		tempGravity = gravity;
		tempMoveSpeed = moveSpeed;
		canLedge = true;
	}

	void Update(){

        //ammo = 90;
        pInputL = new Vector2 (Input.GetAxisRaw ("Horizontal"),Input.GetAxisRaw ("Vertical"));
		pInputR = new Vector2 (Input.GetAxisRaw ("Horizontal2"),Input.GetAxisRaw ("Vertical2"));
		inputAngleL = 180f+ Mathf.Atan2(pInputL.x*-1f,pInputL.y)* Mathf.Rad2Deg;
		GetMoveDir();

//		if(IsRJoystickAct()){
//			inputAngleR = 180f+ Mathf.Atan2(pInputR.x*-1f,pInputR.y)* Mathf.Rad2Deg;
//			angleClamp = Quaternion.Euler(0f, 0f, ( Mathf.Round(inputAngleR/45f)*45f));
//			GetDashDir();
//		}


		velocity = myRigid.velocity;
		justWentAir = false;
		justTouchGround = false;

		if(!isGrounded && CheckIsGround()){
			justTouchGround = true;
			DeslipCollider();
			tempGravity = gravity*2f;
			tempMoveSpeed = moveSpeed;
			if(ammo == 0){
				ammo = 1;
			}
			if((lastFallV  <= -19f || velocity.y <= -19f)){
				TakeDamage(1);
				
			}
		}
		if(isGrounded && !CheckIsGround()){
			justWentAir = true;
			SlipCollider();
			tempGravity = gravity;
			tempMoveSpeed = moveSpeed-1.5f;
		}
					

		isGrounded = CheckIsGround();

		if(isGrounded || isLedge){

		}
 
        if (IsTriggerAct()) {
            triggerChargeTime += Time.deltaTime*(1f/Time.timeScale);
            if (triggerChargeTime > 0.25f && triggerChargeTime < 20f) {
                Time.timeScale = 0.1f;
                isInDecision = true;

                Anchor tempAn = null;
                float tempAngle = 100f;
                Vector3 tempDes = (Quaternion.Euler(0f, 0f, Mathf.Atan2(pInputL.x * -1f, pInputL.y) * Mathf.Rad2Deg) * Vector3.up);

                foreach (Anchor an in myAnchors) {
                    an.isSelected = false;
                    if (Vector3.Angle((this.transform.position - an.transform.position).normalized, tempDes * -1f) < 45f && Vector3.Angle((this.transform.position - an.transform.position).normalized, tempDes * -1f) < tempAngle) {
                        tempAngle = Vector3.Angle((this.transform.position - an.transform.position).normalized, tempDes * -1f);
                        tempAn = an;
                    }

                }
                if (tempAn != null) {
                    myAnchors[myAnchors.IndexOf(tempAn)].isSelected = true;
                }

            } else {
                Time.timeScale = 1f;
                isInDecision = false;
            }
        } else {
            if (triggerChargeTime < 0.25f && triggerChargeTime != 0f) {
                Anchor tempAn = null;
                float tempDis = 100f;
                foreach (Anchor an in myAnchors) {
                    if (Vector3.Distance(this.transform.position, an.transform.position)< tempDis) {
                        tempDis = Vector3.Distance(this.transform.position, an.transform.position);
                        tempAn = an;
                    }

                }
                if (tempAn != null) {
                    myAnchors[myAnchors.IndexOf(tempAn)].GetDash();
                    //DoDash(myAnchors[myAnchors.IndexOf(tempAn)].force, myAnchors[myAnchors.IndexOf(tempAn)].transform.position);
                }
            }
            if (triggerChargeTime > 0.25f && IsJoystickAct()) {

                Anchor tempAn = null;
                float tempAngle = 100f;
                Vector3 tempDes = (Quaternion.Euler(0f, 0f, Mathf.Atan2(pInputL.x * -1f, pInputL.y) * Mathf.Rad2Deg) * Vector3.up);
                
                foreach (Anchor an in myAnchors) {
                    
                    if (Vector3.Angle((this.transform.position-an.transform.position).normalized, tempDes * -1f) <30f && Vector3.Angle((this.transform.position - an.transform.position).normalized, tempDes * -1f) < tempAngle) {
                        tempAngle = Vector3.Angle((this.transform.position - an.transform.position).normalized, tempDes * -1f);
                        tempAn = an;
                    }

                }


                if (tempAn != null) {
                    myAnchors[myAnchors.IndexOf(tempAn)].GetDash();
                    //DoDash(myAnchors[myAnchors.IndexOf(tempAn)].force, myAnchors[myAnchors.IndexOf(tempAn)].transform.position);
                }
            }
        
            Time.timeScale = 1f;
            isInDecision = false;
            triggerChargeTime = 0f;
        }


		//if (IsRJoystickAct()){
		//	if(!isCharging){
		//		timeCharge = 0f;
		//	}


		//	inputAngleR = 180f+ Mathf.Atan2(pInputR.x*-1f,pInputR.y)* Mathf.Rad2Deg;
		//	angleClamp = Quaternion.Euler(0f, 0f, ( Mathf.Round(inputAngleR/45f)*45f));
		//	GetDashDir();


		//	isCharging = true;
		//	timeCharge += 20f * Time.deltaTime * (1f / Time.timeScale);
			
		
		//}else{
		//	isCharging = false;

		//	//isChargingFromAir = false;

		//	if(timeCharge > 0.25f &&( isDashing == Mathfx.Approx (this.transform.position, dashTo, 0.05f) )&& ammo >=1) {

		//		//applied = true;
		//		chargeCooldown = 0f;

		//		StopJumpIdle();

		//		if(!Physics2D.BoxCast(this.transform.position,new Vector2(myBox.size.x,myBox.size.y+0.3f),0f, (angleClamp*Vector3.up)*-1f,dashDistance,groundLayer)){	
		//			dashTo = this.transform.position +( (angleClamp*Vector3.up)*-1f*dashDistance);
		//			checkAngle = (angleClamp*Vector3.up)*-1f;
		//		}else{
		//			dashTo = Physics2D.BoxCast(this.transform.position,new Vector2(myBox.size.x,myBox.size.y+0.3f),0f,(angleClamp*Vector3.up)*-1f,dashDistance,groundLayer).centroid;
		//			checkAngle = (angleClamp*Vector3.up)*-1f;
		//		}
		//		Vector3 tempdashTo;
		//		GameObject tempDash = this.gameObject;
		//		tempdashTo = new Vector3(dashTo.x,dashTo.y,0f);
		//		tempDash = Instantiate(dashEffect[0], this.transform.position +((angleClamp*Vector3.up)*-1f*(Vector3.Distance(this.transform.position,tempdashTo)/2f)),angleClamp) as GameObject;
		//		tempDash.transform.localScale = new Vector3( 3f,Vector3.Distance(this.transform.position,tempdashTo)-0.5f,1f);
		//		for(int i = 0; i< (int)Vector3.Distance(this.transform.position,tempdashTo);i++){
		//			GameObject tempSprite = Instantiate(spriteDash,this.transform.position +(angleClamp*Vector3.up)*-1f*i,mySprite.transform.rotation) as GameObject;
		//			tempSprite.GetComponent<SpriteRenderer>().material.color = new Color(tempSprite.GetComponent<SpriteRenderer>().material.color.r,tempSprite.GetComponent<SpriteRenderer>().material.color.g,tempSprite.GetComponent<SpriteRenderer>().material.color.b,0.2f+(i*0.2f));
		//		}

		//		StartParticle();
		//		CancelInvoke("StopParticle");
		//		Invoke("StopParticle",0.15f);

		//		//trickAmmo -=1;
		//	//	myBurst.GetComponent<burst>().isTrick = true;
		//		isLedge = false;
		//		canLedge = false;
		//		CancelInvoke("CanLedgeBack");
		//		Invoke("CanLedgeBack",0.2f);
		//		isDashing = true;
		//		velocity = Vector2.zero;
		//		myBurst.GetComponent<PolygonCollider2D>().enabled = true;

		//	}
		//	timeCharge = 0f;
		//}


		//isLedge = false;
		if(velocity.y <=2f && !isGrounded && canLedge && !Physics2D.Raycast(this.transform.position,Vector2.down,3f,groundLayer)){

			if(/*(pInputL.x<-0.2f || isLedge) && */Physics2D.Raycast(this.transform.position+(Vector3.left*0.25f)+(Vector3.up*1.5f),Vector2.down,2f,groundLayer)){
				Vector2 tempPoint = Physics2D.Raycast(this.transform.position+(Vector3.left*0.25f)+(Vector3.up*1.5f),Vector2.down,2f,groundLayer).point;
				if(!Physics2D.Raycast(tempPoint+(Vector2.up*0.1f),Vector2.left,0.25f,groundLayer)){
					this.transform.position = tempPoint-(Vector2.up*0.65f)+(Vector2.right*0.25f);

					isLedge = true;
					isOnLeftLedge = true;
					StopJumpIdle();

				}			
			}
			
			if(/*(pInputL.x>0.2f || isLedge)&& */Physics2D.Raycast(this.transform.position+(Vector3.right*0.25f)+(Vector3.up*1.5f),Vector2.down,2f,groundLayer)){
				Vector2 tempPoint = Physics2D.Raycast(this.transform.position+(Vector3.right*0.25f)+(Vector3.up*1.5f),Vector2.down,2f,groundLayer).point;
				if(!Physics2D.Raycast(tempPoint+(Vector2.up*0.1f),Vector2.right,0.25f,groundLayer)){
					this.transform.position = tempPoint-(Vector2.up*0.65f)+(Vector2.left*0.25f);

					isLedge = true;
					isOnLeftLedge = false;
					StopJumpIdle();

				}		
			}


		}	

		if(isGrounded || Physics2D.Raycast(this.transform.position,Vector2.down,2.15f,groundLayer)){
			isLedge = false;
			canLedge = false;
			CancelInvoke("CanLedgeBack");
			Invoke("CanLedgeBack",0.1f);
		}
		if(isDashing){
			if(!Physics2D.BoxCast(this.transform.position,new Vector2(myBox.size.x,myBox.size.y+0.3f),0f,checkAngle,Vector2.Distance(this.transform.position,dashTo),groundLayer)){	
				dashTo = this.transform.position +( checkAngle*Vector2.Distance(this.transform.position,dashTo));
			}else{
				dashTo = Physics2D.BoxCast(this.transform.position,new Vector2(myBox.size.x,myBox.size.y+0.3f),0f,checkAngle,Vector2.Distance(this.transform.position,dashTo),groundLayer).centroid;
			}
		}
		
		
		
		if(isGrounded || isLedge){
			if(isGrounded && Physics2D.Raycast(this.transform.position,Vector2.down,1.5f,groundLayer)){
				if(Physics2D.Raycast(this.transform.position,Vector2.down,1.5f,groundLayer).collider.gameObject.GetComponent<movableSwitch>()!=null){
					Physics2D.Raycast(this.transform.position,Vector2.down,1.5f,groundLayer).collider.gameObject.GetComponent<movableSwitch>().avatarIsOn = true;
				}
				if(parObj != Physics2D.Raycast(this.transform.position,Vector2.down,1.5f,groundLayer).collider.gameObject){
					parPos = Physics2D.Raycast(this.transform.position,Vector2.down,1.5f,groundLayer).collider.transform.position;	
					parObj = Physics2D.Raycast(this.transform.position,Vector2.down,1.5f,groundLayer).collider.gameObject;
					parVelocity = Vector2.zero;
				}
				if(parPos != Physics2D.Raycast(this.transform.position,Vector2.down,1.5f,groundLayer).collider.transform.position){
					parVelocity = Physics2D.Raycast(this.transform.position,Vector2.down,1.5f,groundLayer).collider.transform.position-parPos;
					parPos = Physics2D.Raycast(this.transform.position,Vector2.down,1.5f,groundLayer).collider.transform.position;
				}else{
					parVelocity = Vector2.zero;
				}
			}
			if(isLedge){
				if(isOnLeftLedge){
					if(Physics2D.Raycast(this.transform.position,Vector2.left,1f,groundLayer).collider.gameObject.GetComponent<movableSwitch>()!=null){
						Physics2D.Raycast(this.transform.position,Vector2.left,1f,groundLayer).collider.gameObject.GetComponent<movableSwitch>().avatarIsOn = true;
					}
					if(parObj != Physics2D.Raycast(this.transform.position,Vector2.left,1f,groundLayer).collider.gameObject){
						parPos = Physics2D.Raycast(this.transform.position,Vector2.left,1f,groundLayer).collider.transform.position;	
						parObj = Physics2D.Raycast(this.transform.position,Vector2.left,1f,groundLayer).collider.gameObject;
						parVelocity = Vector2.zero;
					}
					if(parPos != Physics2D.Raycast(this.transform.position,Vector2.left,1f,groundLayer).collider.transform.position){
						parVelocity = Physics2D.Raycast(this.transform.position,Vector2.left,1f,groundLayer).collider.transform.position-parPos;
						if(parVelocity.x>0f){
							parVelocity.x = 0f;
						}
						parPos = Physics2D.Raycast(this.transform.position,Vector2.left,1f,groundLayer).collider.transform.position;
					}else{
						parVelocity = Vector2.zero;
					}
				}else{
					if(Physics2D.Raycast(this.transform.position,Vector2.right,1f,groundLayer).collider.gameObject.GetComponent<movableSwitch>()!= null){
						Physics2D.Raycast(this.transform.position,Vector2.right,1f,groundLayer).collider.gameObject.GetComponent<movableSwitch>().avatarIsOn = true;
					}
					if(parObj != Physics2D.Raycast(this.transform.position,Vector2.right,1f,groundLayer).collider.gameObject){
						parPos = Physics2D.Raycast(this.transform.position,Vector2.right,1f,groundLayer).collider.transform.position;	
						parObj = Physics2D.Raycast(this.transform.position,Vector2.right,1f,groundLayer).collider.gameObject;
						parVelocity = Vector2.zero;
					}
					if(parPos != Physics2D.Raycast(this.transform.position,Vector2.right,1f,groundLayer).collider.transform.position){
						parVelocity = Physics2D.Raycast(this.transform.position,Vector2.right,1f,groundLayer).collider.transform.position-parPos;
						if(parVelocity.x<0f){
							parVelocity.x = 0f;
						}
						parPos = Physics2D.Raycast(this.transform.position,Vector2.right,1f,groundLayer).collider.transform.position;
					}else{
						parVelocity = Vector2.zero;
					}
				}
			}
		}else{
			parVelocity = Vector2.zero;
			parObj = null;
		}
		parVelocity.y = 0f;


		if(Input.GetButtonDown("Fire1")){
			if(isGrounded && !isLedge){
				Jump();
			}
			if(isLedge){
				JumpFromLedge(moveDir);
				canLedge = false;
				CancelInvoke("CanLedgeBack");
				Invoke("CanLedgeBack",0.2f);
				isLedge = false;
			}
		}
		if(isDashing){
			if (!Mathfx.Approx (this.transform.position, dashTo, 0.05f)/* && dashTo != default(Vector2)*/) {
				this.transform.position = Vector3.Lerp (this.transform.position, dashTo, dashSpeed * Time.deltaTime);
				CancelInvoke("StopJumpIdle");
			} else {
				if(!IsInvoking("StopJumpIdle") && !isGrounded){
					Invoke ("StopJumpIdle",dashIdle);
				}
				if(!IsInvoking("StopJumpIdle") && isGrounded){
					StopJumpIdle();
				}
			}
		}

		if(IsJoystickAct() && !isDashing){
			if(isGrounded){
				movement = Mathf.Lerp(movement,tempMoveSpeed*pInputL.x,rampSpeed*Time.deltaTime);
			}else{
				movement = Mathf.Lerp(movement,tempMoveSpeed*pInputL.x,airControlSpeed*Time.deltaTime);
			}
		}else{
			movement = Mathf.Lerp(movement,0f,10f*Time.deltaTime);
		}

		if(!isLedge && !isDashing){
			velocity = new Vector2(movement ,velocity.y-(tempGravity*Time.deltaTime));
			velocity.y = Mathf.Clamp(velocity.y,-20f,20f);
		}else{
			velocity = Vector2.zero;
		}

		myRigid.velocity =  velocity + (parVelocity*120f)+effectVelocity;
	//	Debug.Log(myRigid.velocity);

		///-////-////-////-////-////-////-////-////-////-////-////-////-////-////-////-////-////-////-////-////-////-////-////-////-////-////-////-////-////-////-////-////-////-////-////-////-////-////-//


		if(lastFallV > myRigid.velocity.y){lastFallV = myRigid.velocity.y;}
		if(myRigid.velocity.y>=0f){lastFallV = 0f;}

		if (!Mathfx.Approx(new Vector3(effectVelocity.x,effectVelocity.y,0f),Vector3.zero,0.1f)) {
			effectVelocity = Vector2.Lerp(effectVelocity,Vector2.zero,4f*Time.deltaTime);
		}else{effectVelocity = Vector2.zero;}
		if(!isLedge){
			if (movement < -0.1f) {mySprite.transform.rotation = tempV38;} 
			if (movement > 0.1f) {mySprite.transform.rotation = tempV30;}
		}else{
			if (isOnLeftLedge) {mySprite.transform.rotation = tempV38;} 
			if (!isOnLeftLedge) {mySprite.transform.rotation = tempV30;}
		}

		myAnimator.SetBool("isMoving",(Mathf.Abs(movement)>0f));
		myAnimator.SetBool("isRunning",(Mathf.Abs(movement)>=5f));
		myAnimator.SetBool("isWalking",(Mathf.Abs(movement)<5f && Mathf.Abs(movement)>0.2f));
		myAnimator.SetBool ("isGrounded", isGrounded);
		myAnimator.SetBool ("isLedge", isLedge);
	
	}









	///-////-////-////-////-////-////-////-////-////-////-////-////-////-////-////-////-////-////-////-////-////-////-////-////-////-////-////-////-////-////-////-////-////-////-////-////-////-////-//
	//\\\\\\-\\\\-\\\\-\\\\-\\\\-\\\\-\\\\-\\\\-\\\\-\\\\-\\\\-\\\\-\\\\-\\\\-\\\\-\\\\-\\\\-\\\\-\\\\-\\\\-\\\\-\\\\-\\\\-\\\\-\\\\-\\\\-\\\\-\\\\-\\\\-\\\\-\\\\-\\\\-\\\\-\\\\-\\\\-\\\\-\\\\-\\\\-\\
	void StartParticle(){
		myPart.GetComponent<ParticleSystem>().enableEmission = true;
	}
	void StopParticle(){
		myPart.GetComponent<ParticleSystem>().enableEmission = false;
	}

//    void OnDrawGizmos() {
//
//        if (isInDecision) {
//            Gizmos.color = Color.blue;
//            Gizmos.DrawLine(this.transform.position, this.transform.position + (Quaternion.Euler(0f, 0f, Mathf.Atan2(pInputL.x * -1f, pInputL.y) * Mathf.Rad2Deg) * Vector3.up) * 10f);
//        }
//    }

    void StopJumpIdle(){
		isDashing = false;
		dashTo = default(Vector2);
	}

	void CanLedgeBack(){
		canLedge = true;
	}

	public void Jump(){
		SlipCollider();
		velocity = new Vector2 (velocity.x,jumpForce);
		tempGravity = gravity;
		tempMoveSpeed = moveSpeed-1.5f;

	}

    public void DoDash(float distance, Vector3 angle) {

        StopJumpIdle();
        //float distanceToPoint = Vector3.Distance(this.transform.position, anchor);
        //Vector3 angle = (anchor - this.transform.position).normalized;

        if (!Physics2D.BoxCast(this.transform.position, new Vector2(myBox.size.x, myBox.size.y + 0.3f), 0f, angle , distance , groundLayer)) {
            dashTo = this.transform.position + ((angle) * (distance));
            checkAngle = (angle);
        } else {
            dashTo = Physics2D.BoxCast(this.transform.position, new Vector2(myBox.size.x, myBox.size.y + 0.3f), 0f, angle , distance, groundLayer).centroid;
            checkAngle = (angle);
        }
        Vector3 tempdashTo;
        GameObject tempDash = this.gameObject;

        tempdashTo = new Vector3(dashTo.x, dashTo.y, 0f);
        tempDash = Instantiate(dashEffect, this.transform.position + (angle * (Vector3.Distance(this.transform.position, tempdashTo) / 2f)), Quaternion.LookRotation(Vector3.forward,angle)) as GameObject;
        tempDash.transform.localScale = new Vector3(3f, Vector3.Distance(this.transform.position, tempdashTo) - 0.5f, 1f);

        for (int i = 0; i < (int)Vector3.Distance(this.transform.position, tempdashTo); i++) {
            GameObject tempSprite = Instantiate(spriteDash, this.transform.position + (angle) * i, mySprite.transform.rotation) as GameObject;
            tempSprite.GetComponent<SpriteRenderer>().material.color = new Color(tempSprite.GetComponent<SpriteRenderer>().material.color.r, tempSprite.GetComponent<SpriteRenderer>().material.color.g, tempSprite.GetComponent<SpriteRenderer>().material.color.b, 0.2f + (i * 0.2f));
        }

        StartParticle();
        CancelInvoke("StopParticle");
        Invoke("StopParticle", 0.15f);
        isLedge = false;
        canLedge = false;
        CancelInvoke("CanLedgeBack");
        Invoke("CanLedgeBack", 0.2f);
        isDashing = true;
        velocity = Vector2.zero;
    }

    public void JumpFromLedge(DashDir dir){
		SlipCollider();

		switch(dir){
		case DashDir.Up:
		case DashDir.UpRight:
		case DashDir.UpLeft:
			velocity = new Vector2 (velocity.x,jumpForce*1.2f);
			movement = 0f;
			break;
		case DashDir.Right:
			if(isOnLeftLedge){
				velocity = new Vector2 (velocity.x,jumpForce);
				movement = 6f;
			}else{
				this.transform.position = this.transform.position+(Vector3.up*2f)+(Vector3.right*0.25f);
				movement = 0f;
			}
			break;
	
		case DashDir.Left:
			if(!isOnLeftLedge){
				velocity = new Vector2 (velocity.x,jumpForce);
				movement = -6f;
			}else{
				this.transform.position = this.transform.position+(Vector3.up*2f)+(Vector3.left*0.25f);
				movement = 0f;
			}
			break;

		case DashDir.DownLeft:
		case DashDir.Down:
			velocity = new Vector2 (velocity.x,-2f);
			movement = 0f;
			break;
		}

		tempGravity = gravity;
		tempMoveSpeed = moveSpeed-1.5f;
		
	}

	void SlipCollider(){
		myBox.sharedMaterial.friction = 0f;
		myCircle.sharedMaterial.friction = 0f;
		myBox.enabled = false;
		myBox.enabled = true;
		myCircle.enabled = false;
		myCircle.enabled = true;
	}
	void DeslipCollider(){
		myBox.sharedMaterial.friction = 10f;
		myCircle.sharedMaterial.friction = 1f;
		myBox.enabled = false;
		myBox.enabled = true;
		myCircle.enabled = false;
		myCircle.enabled = true;
	}

	bool CheckIsGround(){
		if (Physics2D.OverlapArea (new Vector2 (this.transform.position.x - myBox.size.x / 2.5f, this.transform.position.y - 0.95f), new Vector2 (this.transform.position.x + myBox.size.x / 2.5f, this.transform.position.y - 1.1f), groundLayer)) {
			return true;
		} else {
			return false;
		}
	}

	void GetDashDir(){

		if(Mathfx.Approx(inputAngleR,45f,35f)){dashDir = DashDir.DownRight;}
		if(Mathfx.Approx(inputAngleR,135f,35f)){dashDir = DashDir.UpRight;}
		if(Mathfx.Approx(inputAngleR,225f,35f)){dashDir = DashDir.UpLeft;}
		if(Mathfx.Approx(inputAngleR,315f,35f)){dashDir = DashDir.DownLeft;}
		if(Mathfx.Approx(inputAngleR,90f,10f)){dashDir = DashDir.Right;}
		if(Mathfx.Approx(inputAngleR,180f,10f)){dashDir = DashDir.Up;}
		if(Mathfx.Approx(inputAngleR,270f,10f)){dashDir = DashDir.Left;}
		if(inputAngleR <= 10f || inputAngleR >=350f){dashDir = DashDir.Down;}
	}

	void GetMoveDir(){
		
		if(Mathfx.Approx(inputAngleL,45f,22.5f)){moveDir = DashDir.DownRight;}
		if(Mathfx.Approx(inputAngleL,135f,22.5f)){moveDir = DashDir.UpRight;}
		if(Mathfx.Approx(inputAngleL,225f,22.5f)){moveDir = DashDir.UpLeft;}
		if(Mathfx.Approx(inputAngleL,315f,22.5f)){moveDir = DashDir.DownLeft;}
		if(Mathfx.Approx(inputAngleL,90f,22.5f)){moveDir = DashDir.Right;}
		if(Mathfx.Approx(inputAngleL,180f,22.5f)){moveDir = DashDir.Up;}
		if(Mathfx.Approx(inputAngleL,270f,22.5f)){moveDir = DashDir.Left;}
		if(inputAngleL <= 22.5f || inputAngleL >=337.5f){moveDir = DashDir.Down;}
	}

	bool IsJoystickAct(){
		if(!Mathfx.Approx(pInputL,Vector3.zero,0.1f)){
			return true;
		}else{
			return false;
		}
	}
	bool IsRJoystickAct(){
		if(!Mathfx.Approx(pInputR,Vector3.zero,0.7f)){
			return true;
		}else{
			return false;
		}
	}

    bool IsTriggerAct() {
        if (!Mathfx.Approx(Input.GetAxis("TriggerR"), 0f, 0.7f) || !Mathfx.Approx(Input.GetAxis("TriggerL"), 0f, 0.7f)) {
            return true;
        } else {
            return false;
        }
    }

    //bool IsLTriggerAct() {
    //    if (!Mathfx.Approx(Input.GetAxis("TriggerL"), 0f, 0.7f)) {
    //        return true;
    //    } else {
    //        return false;
    //    }
    //}

    public void TakeDamage(int dmg){
		if(!isInvincible){
			hp -= dmg;
			isInvincible = true;
			GameManager.Instance.MainCamera.GetComponent<Camera_Behavior>().CameraShake((float)dmg/10f);
			Invoke("BecomeVulnerable",0.5f);
			if(hp <= 0){
				Application.LoadLevel(0);
			}
		}
	}
	public void TakeDamage(int dmg,float force, Vector3 pos){
		if(!isInvincible){
			hp -= dmg;
			//KnockBack(force,pos);
			isInvincible = true;
			GameManager.Instance.MainCamera.GetComponent<Camera_Behavior>().CameraShake((float)dmg/10f);
			Invoke("BecomeVulnerable",0.5f);
			if(hp <= 0){
				Application.LoadLevel(0);
			}
		}
	}
	void BecomeVulnerable(){
		isInvincible = false;
	}
	public int HP{
		get{return hp;}
		set{hp = value;}
	}

	public int Ammo{
		get{return ammo;}
		set{ammo = value;}
	}

	public bool IsInWater{
		get{return isInWater;}
		set{isInWater = value;}
	}

	public bool IsDashing{
		get{return isDashing;}
		set{isDashing = value;}
	}

}


