using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMove : MonoBehaviour {

	public GameObject player;
	public float cameraSpeed = 5.0f;
	public float xPos = 9.0f;
	public float yPos = 7.0f;
	public float zPos = 20.0f;
	private Camera camera;
	// Use this for initialization
	void Start () {
		camera = Camera.main.GetComponent<Camera>();
	}

	void FixedUpdate(){
		//set raycast starting position
		/*if(camera.orthographicSize < 7){
			camera.orthographicSize = 7;
		}	*/
		Vector3 camPos = transform.position;
		camPos.x = player.transform.position.x + xPos;
		transform.position = Vector3.Lerp (transform.position, camPos, 3 * Time.fixedDeltaTime);

		camPos.y = player.transform.position.y + yPos;
		transform.position = Vector3.Lerp (transform.position, camPos, 3 * Time.fixedDeltaTime);

		camPos.z = player.transform.position.z - zPos;
		transform.position = Vector3.Lerp (transform.position, camPos, 3 * Time.fixedDeltaTime);
	}
	// Update is called once per frame
	void Update () {
		
	}
}
