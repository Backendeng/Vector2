using UnityEngine;
using System.Collections;

public class moveObject : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
	if(transform.position.x > -30)
	{
	//move object if visible
	//transform.Translate(Vector3.right * Time.deltaTime * -10);	
	}
	else{
	//if object is not visible anymore, destroy it
	Destroy(gameObject);	
	}
	}
}
