using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Water : MonoBehaviour{
	public ParticleSystem mySplash;
	private List<WaterBlock> myBlocks = new List<WaterBlock>();
	private Transform[] myBlocksTemp;
	private float idleTime = 10f;


	void Start(){
		myBlocksTemp = GetComponentsInChildren<Transform>();
		myBlocksTemp = myBlocksTemp.OrderBy(item => item.transform.position.x).ToArray();
		foreach(Transform blk in myBlocksTemp){
			if(blk.childCount ==0){
				myBlocks.Add(new WaterBlock(blk.gameObject, blk.GetComponent<MeshFilter>().mesh));
			}
		}


	}
	void Update(){
		idleTime += 1f*Time.deltaTime;
		if(idleTime>10f){
			if(idleTime>15f){
				idleTime = 10f;
			}
			IdleWave(new Vector3(Random.Range(myBlocks[0].gameObject.transform.position.x,myBlocks[myBlocks.Count-1].gameObject.transform.position.x),this.transform.position.y,0f),0.1f);
			//IdleWave(new Vector3(Mathf.Lerp(myBlocks[0].gameObject.transform.position.x,myBlocks[myBlocks.Count-1].gameObject.transform.position.x,(idleTime-10f)/5f),this.transform.position.y,0f),0.1f);
		}
		foreach(WaterBlock wtb in myBlocks){
			Vector3[] tempVert;
			tempVert = wtb.mesh.vertices;
			tempVert[1] = new Vector3(tempVert[1].x,Mathf.Lerp(tempVert[1].y,wtb.rightV+0.5f,6f*Time.deltaTime) ,tempVert[1].z);
			tempVert[3] = new Vector3(tempVert[3].x,Mathf.Lerp(tempVert[3].y,wtb.leftV+0.5f,6f*Time.deltaTime),tempVert[3].z);
			if(Approx(tempVert[3].y,wtb.leftV+0.5f,0.1f)){
				wtb.leftV = wtb.leftV*-0.6f;
			}
			if(Approx(tempVert[1].y,wtb.rightV+0.5f,0.1f)){
				wtb.rightV = wtb.rightV*-0.6f;
			}
			wtb.mesh.vertices = tempVert;
		}
	}

	void CreateWave(Vector3 pos,float velocity){
		if(velocity>0f){
			foreach(WaterBlock wtb in myBlocks){
				
				wtb.leftV += Mathf.Clamp((5f / Vector3.Distance(wtb.gameObject.transform.position+Vector3.left*0.25f,pos)-1f)*0.1f,-2f,2f);
				wtb.rightV += Mathf.Clamp((5f / Vector3.Distance(wtb.gameObject.transform.position+Vector3.right*0.25f,pos)-1f)*0.1f,-2f,2f);
			}
			Instantiate(mySplash,new Vector3(pos.x,this.transform.position.y+1.5f,0f), Quaternion.identity);
		}else{
			foreach(WaterBlock wtb in myBlocks){
				
				wtb.leftV += Mathf.Clamp((-8f / Vector3.Distance(wtb.gameObject.transform.position+Vector3.left*0.25f,pos)-1f)*0.1f,-2f,2f);
				wtb.rightV += Mathf.Clamp((-8f / Vector3.Distance(wtb.gameObject.transform.position+Vector3.right*0.25f,pos)-1f)*0.1f,-2f,2f);
			}
			Instantiate(mySplash,new Vector3(pos.x,this.transform.position.y+0.5f,0f), Quaternion.identity);
		}
	
	}

	void IdleWave(Vector3 pos,float velocity){
		foreach(WaterBlock wtb in myBlocks){
			wtb.leftV = Mathf.Clamp((velocity/ Vector3.Distance(wtb.gameObject.transform.position+Vector3.left*0.25f,pos)-1f)*0.1f,-2f,2f);
			wtb.rightV = Mathf.Clamp((velocity / Vector3.Distance(wtb.gameObject.transform.position+Vector3.right*0.25f,pos)-1f)*0.1f,-2f,2f);
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		idleTime = 0f;
		CreateWave(other.transform.position, other.attachedRigidbody.velocity.y);

	}

	public static bool Approx(float val, float about, float range) {
		return ( ( Mathf.Abs(val - about) < range) );
	}
}

public class WaterBlock {
	public GameObject gameObject;
	public Mesh mesh;
	public float leftV;
	public float rightV;

	public WaterBlock(GameObject go, Mesh ms){
		gameObject = go;
		mesh = ms;
	}

}
