using UnityEngine;
using System.Collections;

public class Avatar_BehaviorV2 : MonoBehaviour {
	
	//movement
	public float moveSpeed;
	public float jumpForce;
	public float gravity;
	private float moveSpeedRamp;
	
	//time 
	private float slowTimeFactor;
	//private float slowTimeLerp;
	private bool isSlowed;
	private bool isSlowedTime;
	private bool isSlowedMind;
	private float slowTimeTime;
	
	//GUI
	public GameObject myArrow;
	public GameObject myCircle;
	public GameObject myPart;
	
	//Ammo and charge
	private bool chargeIsTrick;
	private bool applied;
	private float chargeCooldown;
	private float rechargeCooldown;
	private int trickAmmo;
	private int powerAmmo;
	private int timeAmmo;
	private bool canCostTimeAmmo;
	public float trickVelocity = 10f;
	public float powerVelocity = 21f;
	
	//life stuff
	public int hp;
	
	
	//State! :D
	private bool isOnLedge;
	private bool isJumping;
	private bool isFalling;
	private bool isTakingDamage;
	private bool isMoving;
	private bool isGrounded;
	private bool isCharging;
	//private bool isChargingFromAir;
	private bool isInWater;
//	private bool isAffected;
	private bool isGravityed;

	//Utility
	public LayerMask groundLayer;
	private Vector2 movement;
	private Vector2 steamVelocity;
	private float timeCharge;

	
	//reference
	private Rigidbody2D myRigid;
	private BoxCollider2D myBox;
	private Animator myAnimator;
	public GameObject mySprite;
	
	void Start(){
		myRigid = GetComponent<Rigidbody2D> ();
		myBox = GetComponent<BoxCollider2D> ();
		myAnimator = GetComponentInChildren<Animator> ();
		
		
	}
	
	void Update () {
		
		
		chargeCooldown += 1f * Time.deltaTime * (1f / Time.timeScale);
		rechargeCooldown += 1f * Time.deltaTime * (1f / Time.timeScale);
		
		if (chargeCooldown >= 0.5f) {
			chargeCooldown =0f;
			applied = false;
		}
		if (rechargeCooldown >= 1f) {
			rechargeCooldown =0f;
			trickAmmo += 1;
			trickAmmo = Mathf.Clamp(trickAmmo,0,4);
		}
		
		if(powerAmmo == 0){
			chargeIsTrick = true;
		}
		
		myArrow.GetComponent<SpriteRenderer> ().enabled = false;
		

		
		if(!isGrounded && CheckIsGround() && !isInWater){
			if(movement.y <= -24f){
				hp -= 1;
				//Debug.Log(movement.y.ToString());
			}
		}
		movement = myRigid.velocity;
		isGrounded = CheckIsGround();
		if(Mathfx.Approx(steamVelocity.x,0f,3f)/* && Mathfx.Approx(steamVelocity.y,0f,3f)*/){

			isGravityed = true;
		}


		if(isGrounded){
			moveSpeed = 6f;
		}else{
			moveSpeed = 5f;
		}

		///Lerp movement and Gravity
		if(!isInWater){
			if(movement.y > -25f && isGravityed){
				movement = new Vector2 (movement.x, movement.y - gravity * Time.deltaTime);
			}
		}else{
			if(movement.y > -2f || CheckIsUsingRightJoystick()){
				movement = new Vector2 (movement.x, movement.y - gravity/2f * Time.deltaTime);
			}
			else{
				movement = new Vector2 (movement.x, -2f);
			}
		}
		///Action
		
		if(Input.GetAxis("TriggerR")>0.5f){
			if(slowTimeTime>0f){
				if(timeAmmo >=1 || !canCostTimeAmmo){
					if(canCostTimeAmmo){
						timeAmmo--;
						canCostTimeAmmo = false;
					}
					isSlowed = true;
					slowTimeTime -= 1f * Time.deltaTime * (1f / Time.timeScale);
					slowTimeFactor = Mathf.Lerp (slowTimeFactor, 10f, 4f * Time.deltaTime);
				}
				
			}else{
				slowTimeFactor = 1f;
			}
			
		}
		
		if(Input.GetAxis("TriggerR")==0f){
			slowTimeFactor = 1f;
			slowTimeTime = 2f;
			isSlowed = false;
			canCostTimeAmmo = true;
			
		}
		
		if(Input.GetButtonDown("BumperL")){
			if(powerAmmo >=1 && chargeIsTrick){
				chargeIsTrick = false;
			}else{
				chargeIsTrick = true;
			}
		}
		
		Time.timeScale = 1f/slowTimeFactor;


		if(isInWater){
			if (CheckIsUsingRightJoystick()){

				SetArrow();
				isSlowed = false;
				applied = true;
				steamVelocity = Vector2.Lerp(steamVelocity,( myArrow.transform.up * trickVelocity* -2f),10f*Time.deltaTime);
				StartParticle();
				CancelInvoke("StopParticle");
				Invoke("StopParticle",0.15f);
				movement.y = steamVelocity.y*3f;
				//movement.y = Mathf.Lerp(movement.y, steamVelocity.y,10f*Time.deltaTime);
				steamVelocity.y = 0f;
				chargeCooldown = 0f;

			}
		}else{
			if (CheckIsUsingRightJoystick()){
				if(!isCharging){
					timeCharge = 0f;
				}
				
				isCharging = true;
				SetArrow();
				timeCharge += 20f * Time.deltaTime * (1f / Time.timeScale);
				
//				if(!isGrounded){
//					isChargingFromAir = true;
//				}
				
			}else{
				isCharging = false;
				//isChargingFromAir = false;
	
				if(timeCharge > 0.25f && !applied) {
					
					isSlowed = false;
					applied = true;
					chargeCooldown = 0f;
					if(chargeIsTrick && trickAmmo >=1){
						steamVelocity = myArrow.transform.up * trickVelocity* -1f;
						StartParticle();
						CancelInvoke("StopParticle");
						Invoke("StopParticle",0.15f);
						if(trickAmmo == 4){
							rechargeCooldown = 0f;
						}
						trickAmmo -=1;
					}
					if(!chargeIsTrick && powerAmmo >=1){
						steamVelocity = myArrow.transform.up * powerVelocity* -1f;
						StartParticle();
						CancelInvoke("StopParticle");
						Invoke("StopParticle",0.5f);
						powerAmmo -=1;
					}
					
					movement.y = steamVelocity.y;

					if(Mathfx.Approx(steamVelocity.y,0f,1f)){
						isGravityed = false;
					}

					steamVelocity.y = 0f;
					timeCharge = 0f;
				}
			}
		}

		
		
		
		
		
		
		
		
		
		if (slowTimeTime == 2) {
			myCircle.SetActive (false);
			myCircle.transform.localScale = new Vector3 (1f, 1f, 1f);
			//myCircle.transform.localScale = Vector3.Lerp (myCircle.transform.localScale, new Vector3 (slowTimeTime * 2f, slowTimeTime * 2f, 1f), Time.deltaTime * (1f / Time.timeScale));
		} else {
			myCircle.SetActive (true);
			myCircle.transform.localScale = Vector3.Lerp (myCircle.transform.localScale, new Vector3 (slowTimeTime * 1f, slowTimeTime * 1f, 1f),2f* Time.deltaTime * (1f / Time.timeScale));
			
		}
		
		
		if (!Mathfx.Approx(new Vector3(steamVelocity.x,steamVelocity.y,0f),Vector3.zero,0.1f)) {
			steamVelocity = Vector2.Lerp(steamVelocity,Vector2.zero,4f*Time.deltaTime);
		}else{
			steamVelocity = Vector2.zero;
		}
		//Debug.Log (steamVelocity);
		if ((Input.GetButtonDown("Fire1") && isGrounded) || (isInWater && Input.GetButtonDown("Fire1"))) {
			isSlowed = false;
			Jump ();
		}
		if(!Mathfx.Approx(Input.GetAxisRaw ("Horizontal"),0f,0.1f)){
			
			
			moveSpeedRamp = Mathf.Lerp(moveSpeedRamp, Input.GetAxisRaw ("Horizontal"), 7.5f * Time.deltaTime);
		}else{
			moveSpeedRamp = Mathf.Lerp(moveSpeedRamp, Input.GetAxisRaw ("Horizontal"), 5f * Time.deltaTime);
			
		}
		movement = new Vector2 (Input.GetAxisRaw ("Horizontal") * ((isInWater)? moveSpeed/2f:moveSpeed) * Mathf.Abs( moveSpeedRamp), movement.y);

		Quaternion tempV30 = Quaternion.Euler( new Vector3 (0f, 0f, 0f));
		Quaternion tempV38 = Quaternion.Euler( new Vector3 (0f, 180f, 0f));
		

		if (movement.x < -0.1f) {
			mySprite.transform.rotation = tempV38;
		
		} 
		if (movement.x > 0.1f) {
			mySprite.transform.rotation = tempV30;
		}
		myAnimator.SetBool("isMoving",(Mathf.Abs(movement.x)>0f));
		myAnimator.SetBool("isRunning",(Mathf.Abs(movement.x)>=5f));
		myAnimator.SetBool("isWalking",(Mathf.Abs(movement.x)<5f && Mathf.Abs(movement.x)>0.1f));
		myAnimator.SetBool ("isGrounded", isGrounded);
		
		//movement = new Vector2 (Mathf.Clamp (movement.x, -15f, 15f), Mathf.Clamp (movement.y, -20f,20f));
		
		
		myRigid.velocity = movement + steamVelocity;
		
		
		
	}
	
	bool CheckIsGround(){
		if (Physics2D.OverlapArea (new Vector2 (this.transform.position.x - myBox.size.x / 2.5f, this.transform.position.y - myBox.size.y / 2f), new Vector2 (this.transform.position.x + myBox.size.x / 2.5f, this.transform.position.y - 0.2f - myBox.size.y / 2f), groundLayer)) {
			return true;
		} else {
			return false;
		}
	}

	bool CheckIsUsingRightJoystick(){
		if ((Input.GetAxis("Horizontal2")>=0.5f || Input.GetAxis("Vertical2")>=0.5f )|| (Input.GetAxis("Horizontal2")<=-0.5f || Input.GetAxis("Vertical2")<=-0.5f ) ){
			return true;
		}else{
			return false;
		}
	}

	void SetArrow(){
		myArrow.GetComponent<SpriteRenderer>().enabled = true;
		float angle = Mathf.Atan2(Input.GetAxis("Horizontal2")*-1f,Input.GetAxis("Vertical2")) * Mathf.Rad2Deg;
		myArrow.transform.rotation = Quaternion.Euler(0f, 0f, ( Mathf.Round(angle/45f)*45f));
		myArrow.transform.position = this.transform.position + myArrow.transform.up*1.5f;
	}

	void StopParticle(){
		myPart.GetComponent<ParticleSystem>().enableEmission = false;
	}

	void StartParticle(){
		myPart.GetComponent<ParticleSystem>().enableEmission = true;
	}
	
	public void Jump(){
		movement = new Vector2 (movement.x, ((isInWater)? jumpForce/2f:jumpForce));
	}
	

	public bool IsInWater{
		get {return isInWater;}
		set{isInWater = value;}
	}

	public bool Applied{
		get {return applied;}
		set{applied = value;}
	}

	public int PowerAmmo{
		get{return powerAmmo;}
		set{

			powerAmmo = value;
			if (powerAmmo > 1){
				powerAmmo = 1;
			}
		
		}
	}
	
	public int TrickAmmo{
		get{return trickAmmo;}
		set{trickAmmo = value;}
	}

	public int TimeAmmo{
		get{return timeAmmo;}
		set{	
			timeAmmo = value;
			if (timeAmmo > 3){
				timeAmmo = 3;
			}
		}
	}
	
	public int HP{
		get{return hp;}
		set{hp = value;}
	}
	
	public bool ChargeIsTrick{
		get{return chargeIsTrick;}
		set{chargeIsTrick = value;}
	}
	
	
}
