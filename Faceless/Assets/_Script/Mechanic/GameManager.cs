using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public GameObject myAvatar;
	private Avatar_BehaviorV2 avatarB;
	private Dictionary<int, SpawnPoint> mySpawns = new Dictionary<int, SpawnPoint> ();
	private GameObject[] ListMySpawns;
	public int idToSpawnTo;
	private GameObject mainCamera;
	private GameObject avatar;
	private int curCoins;
	private int maxCoins;
	private static GameManager instance;
		
	void Awake(){
	
		if (Instance == null) {
			instance = this;
		} else if(Instance != this) {
			Destroy(this.gameObject);
		}
		DontDestroyOnLoad (this.gameObject);
		ListMySpawns = GameObject.FindGameObjectsWithTag("SpawnPoint");
		foreach (GameObject sp in ListMySpawns) {
			mySpawns.Add (sp.gameObject.GetComponent<SpawnPoint>().id,sp.gameObject.GetComponent<SpawnPoint>());
		}



		mainCamera = Camera.main.gameObject;
		avatar = Instantiate (myAvatar, mySpawns [idToSpawnTo].transform.position, Quaternion.identity) as GameObject;
		avatarB = avatar.GetComponent<Avatar_BehaviorV2>();
		avatarB.HP = 6;
	}

	void Start(){

	}

	void Update(){
		if(Input.GetKeyDown(KeyCode.Space)){
		//	Application.LoadLevel(1);
		}
	}

	public int IdToSpawnTo{
		get{return idToSpawnTo;}
		set{idToSpawnTo = value;}
	}

	public GameObject MainCamera{
		get{return mainCamera;}
		set{mainCamera = value;}
	}

	public GameObject Avatar{
		get{return avatar;}
		set{avatar = value;}
	}
	public Avatar_BehaviorV2 AvatarB{
		get{return avatarB;}
		set{avatarB = value;}
	}


	public static GameManager Instance{
		get{return instance;}
		set{instance = value;}
	}

}
