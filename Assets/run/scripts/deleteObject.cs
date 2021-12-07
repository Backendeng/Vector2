using UnityEngine;
using System.Collections;

public class deleteObject : MonoBehaviour {

	public float lifetime;
	
	//delete object after lifetime
	void Start () {
	Destroy(gameObject, lifetime);
	}
}
