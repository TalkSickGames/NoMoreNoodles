using UnityEngine;
using System.Collections;

public class Avatar_Behavior : MonoBehaviour {
	
	public float moveSpeed;
	public float jumpForce;
	public float gravity;
	//public float steamChargeMax;
	public float steamChargeLittle;
	public float steamChargeBig;
	private float slowTimeFactor;
	private float slowTimeLerp;
	private bool slowTimeBool;
	private float slowTimeTime;
	public GameObject myArrow;
	public GameObject myCircle;

	private bool applied;
	private float cooldown;

	//State! :D
	public bool isJumping;
	public bool isFalling;
	public bool isTakingDamage;
	public bool isMoving;
	public bool isGrounded;
	public bool isCharging;
	private bool isChargingFromAir;

	public LayerMask groundLayer;
	
	private Vector2 movement;
	private Vector2 effectVelocity;
	private Vector2 steamVelocity;
	private float steamDirection;
	private float steamCharge;

	private Vector3 tempMouse;
	private Rigidbody2D myRigid;
	private BoxCollider2D myBox;
	private Animator myAnimator;

	void Start(){
		myRigid = GetComponent<Rigidbody2D> ();
		myBox = GetComponent<BoxCollider2D> ();
		myAnimator = GetComponentInChildren<Animator> ();


	}
	
	void Update () {
		cooldown += 1f * Time.deltaTime * (1f / Time.timeScale);

		if (cooldown >= 0.5f) {
			cooldown =0f;
			applied = false;
		}

		tempMouse = Input.mousePosition;
		tempMouse.z = 10f;
		tempMouse = GameManager.Instance.MainCamera.GetComponent<Camera>().ScreenToWorldPoint(tempMouse);

		movement = myRigid.velocity;
		isGrounded = CheckIsGround();

		///Lerp movement and Gravity

		movement = new Vector2 (movement.x, movement.y - gravity * Time.deltaTime);
		//movement = new Vector2 (Mathf.Lerp (movement.x, Input.GetAxisRaw ("Horizontal") * moveSpeed, 20f * Time.deltaTime), movement.y);


		///Action

		if ((Input.GetAxis("Horizontal2")>=0.5f || Input.GetAxis("Vertical2")>=0.5f )|| (Input.GetAxis("Horizontal2")<=-0.5f || Input.GetAxis("Vertical2")<=-0.5f ) ){
			if(!isCharging){
				steamCharge = 0f;
			}

			isCharging = true;
			if(!isGrounded){
				isChargingFromAir = true;
			}
	
			/*steamVelocity = Vector2.zero;*/
		}else{
			isCharging = false;
			isChargingFromAir = false;
		}

//		if ((/*(Input.GetAxis("Horizontal2")<=0.1f && */Input.GetAxis("Vertical2")<=0.1f ) && (Input.GetAxis("Horizontal2")>=-0.1f && Input.GetAxis("Vertical2")>=-0.1f ) ) {
//			isCharging = false;
//			isChargingFromAir = false;
//		}

//		if (isChargingFromAir) {
//			if(steamCharge > 3f){
//				Time.timeScale = 1f/slowTimeFactor;
//			}
//
//		} else {
//			Time.timeScale = 1f;
//		}

		myArrow.GetComponent<SpriteRenderer> ().enabled = false;

		if(Input.GetAxis("TriggerR")>0.5f){
			if(slowTimeTime>0f){
				slowTimeBool = true;
				slowTimeTime -= 1f * Time.deltaTime * (1f / Time.timeScale);
				slowTimeFactor = Mathf.Lerp (slowTimeFactor, 10f, 4f * Time.deltaTime);
				//slowTimeFactor = 1f;
			}else{
				slowTimeFactor = 1f;
			}

		}

		if(Input.GetAxis("TriggerR")==0f){
			slowTimeFactor = 1f;
			slowTimeTime = 2f;
		}



		Time.timeScale = 1f/slowTimeFactor;

		if (isCharging) {

			myArrow.GetComponent<SpriteRenderer>().enabled = true;
			float angle = Mathf.Atan2(Input.GetAxis("Horizontal2")*-1f,Input.GetAxis("Vertical2")) * Mathf.Rad2Deg;
//			Vector3 diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
//			diff.Normalize();
//			float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
//			myArrow.transform.rotation = Quaternion.Euler(0f, 0f, rot_z-90f*-1f);	
//			myArrow.transform.rotation = Quaternion.Euler(0f, 0f, ( Mathf.Round(myArrow.transform.rotation.eulerAngles.z/45)*45f));
//			myArrow.transform.position = this.transform.position + myArrow.transform.up*1.5f;
			myArrow.transform.rotation = Quaternion.Euler(0f,0f,angle);
			myArrow.transform.rotation = Quaternion.Euler(0f, 0f, ( Mathf.Round(myArrow.transform.rotation.eulerAngles.z/45)*45f));
			myArrow.transform.position = this.transform.position + myArrow.transform.up*1.5f;
			steamCharge += 20f * Time.deltaTime * (1f / Time.timeScale);

		} else if(steamCharge > 1f && !applied) {
			if(steamCharge < 11f){
				steamCharge = steamChargeLittle;
			}
			if(steamCharge >= 11f){
				steamCharge = steamChargeBig;
			}
			slowTimeBool = false;
			applied = true;
			cooldown = 0f;
			//steamVelocity = (transform.position - tempMouse).normalized * steamCharge;
			steamVelocity = myArrow.transform.up * steamCharge*-1f;
			movement.y = steamVelocity.y;
			steamVelocity.y = 0f;
			steamCharge = 0f;
		}


		if (slowTimeTime == 2) {
			myCircle.SetActive (false);
			myCircle.transform.localScale = new Vector3 (1f, 1f, 1f);
			//myCircle.transform.localScale = Vector3.Lerp (myCircle.transform.localScale, new Vector3 (slowTimeTime * 2f, slowTimeTime * 2f, 1f), Time.deltaTime * (1f / Time.timeScale));
		} else {
			myCircle.SetActive (true);
			myCircle.transform.localScale = Vector3.Lerp (myCircle.transform.localScale, new Vector3 (slowTimeTime * 1f, slowTimeTime * 1f, 1f),2f* Time.deltaTime * (1f / Time.timeScale));

		}


		if (!Approx(new Vector3(steamVelocity.x,steamVelocity.y,0f),Vector3.zero,0.1f)) {
			steamVelocity = Vector2.Lerp(steamVelocity,Vector2.zero,4f*Time.deltaTime);
		}else{
			steamVelocity = Vector2.zero;
		}
		//Debug.Log (steamVelocity);
		if ((Input.GetKeyDown (KeyCode.Space) || Input.GetButtonDown("Fire1")) && isGrounded) {
			slowTimeBool = false;
			Jump ();
		}



		movement = new Vector2 (Input.GetAxisRaw ("Horizontal") * moveSpeed, movement.y);

		myAnimator.SetBool("isMoving",(!Approx(movement.x,0f,1f))?true:false);
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


	public void Jump(){
		movement = new Vector2 (movement.x, jumpForce);
	}

	public static bool Approx(Vector3 val, Vector3 about, float range) {
		return ( (val - about).sqrMagnitude < range*range);
	}
	public static bool Approx(float val, float about, float range) {
		return(Mathf.Abs(val - about)<range);
	}

	

}
