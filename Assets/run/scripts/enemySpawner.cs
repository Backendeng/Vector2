using UnityEngine;
using System.Collections;

public class enemySpawner : MonoBehaviour {
	
	//variables visible in the inspector
	public GameObject enemy;
	public GameObject coin;
	public int amountOfCoins;
	
	//not visible in the inspector
	Vector3 pos;

	void Start () {
	//randomly check if an enemy should be instantiated, get position and instantiate enemy
	/*int doInstantiate = Random.Range(0, 2);
	if(doInstantiate == 0){
	pos = new Vector3(transform.position.x + Random.Range(-2f, 2f), transform.position.y, transform.position.z);
	Instantiate(enemy, pos, transform.rotation);*/	
	}
		//else{
		//if there's no enemy, add as much coins as the amount of coins
		/*	for(int i = 0; i < amountOfCoins; i++){
			pos = new Vector3(transform.position.x + Random.Range(-10, 1f), transform.position.y - 0.5f, transform.position.z);
			Instantiate(coin, pos, transform.rotation);	
		}
		}
		}*/
}
