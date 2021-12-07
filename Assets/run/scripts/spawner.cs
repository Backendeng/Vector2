using UnityEngine;
using System.Collections;

public class spawner : MonoBehaviour {

	//variables visible in the inspector
	[Header("Spawn settings")]
	[Space(5)]
	public float maxSpawnWait;
	public float minSpawnWait;
	
	[Header("Add track parts here:")]
	public GameObject[] trackParts;
	
	[Header("Add roof parts here:")]
	public GameObject[] roofParts;
	
	//not visible in the inspector
	int lastParcourPart;
	
	//add roof each 1.8 seconds and keep spawning platforms
	IEnumerator Start(){
		InvokeRepeating("addRoof", 1, 1.8f);
		yield return new WaitForSeconds(5);
        while(true){
            SpawnNew();
			float spawnWait = Random.Range(minSpawnWait, maxSpawnWait);
            yield return new WaitForSeconds(spawnWait);
        }
    }
	
	//handles the actual spawing
	void SpawnNew(){
	//if platforms are added, get a random one and instantiate it as a child of the track object
	if(trackParts.Length > 0){
	int random = Random.Range(0, trackParts.Length);
	if(lastParcourPart != random){
	GameObject newParcour = Instantiate(trackParts[random], transform.position, transform.rotation) as GameObject;
	newParcour.transform.parent = GameObject.Find("Track").transform;
	lastParcourPart = random;
	}
	else{
	//if the random platform is same type as last platform, spawn other one
	SpawnNew();	
	}
	}	
	}
	
	void addRoof(){
	//get random roof and instantiate it as a child of the track object
	int random = Random.Range(0, roofParts.Length);
	GameObject newRoof = Instantiate(roofParts[random], new Vector3(30, 9, 0), transform.rotation) as GameObject;
	newRoof.transform.parent = GameObject.Find("Track").transform;
	}
}
