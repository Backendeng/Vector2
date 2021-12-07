using UnityEngine;
using System.Collections;

public class player : MonoBehaviour {
	
	//variable visible in the inspector
    public float jumpingForce;
	//public Transform upperBody;
	//public GameObject bullet;
	public ParticleSystem deadEffect;
	public ParticleSystem coinParticles;
	public string saveCoinsAs = "coins";


	//OGC SwipeControls
	private SwipeControls swipeLogic;
	public Animator anim;
	public Animation animation;
	private CapsuleCollider[] colliders;
	//variables not visible in the inspector
	public static bool dead;

	bool holding = false;
	bool falling = false;
	bool onTheGround;
    bool jump;
	bool grab = true;
	AnimatorClipInfo[] m_CurrentClipInfo;
	string m_ClipName;
	float m_CurrentClipLength;
	float holdingtime = 0.0f;

	//GameObject newBullet;
	GameObject gameManager;

	public float playerSpeed;
	void Start(){
		colliders = GetComponents<CapsuleCollider> ();

		anim = gameObject.GetComponent<Animator>();
		anim.speed = 0.85f;

		anim.SetBool ("isRunning", true);
		anim.SetBool ("isHolding", false);
		anim.SetBool ("isLanding", false);
		anim.SetBool ("isGrabing", false); 
		anim.SetBool ("isEnd", false); 

		onTheGround = true;
		jump = false;
		dead = false;
	
		//find game manager
		gameManager = GameObject.Find("Game manager");
		
		//OGC_Get SwipeLogic
		swipeLogic = transform.GetComponent <SwipeControls>();
	}
  	
    void Update(){
		transform.Translate (Vector3.forward * playerSpeed * Time.deltaTime);
		transform.position = new Vector3 (transform.position.x, transform.position.y, 0);

		//OGC get direction
		SwipeControls.SwipeDirection direction = swipeLogic.getSwipeDirection();
		
		RaycastHit hit;
		//Debug.Log (transform.position);

		if (Physics.Raycast (transform.position, transform.forward, out hit, 0.5f)) {
			if (hit.transform.gameObject.tag == "hill") {
				GetComponent<Rigidbody> ().useGravity = false;
				GetComponent<Rigidbody> ().isKinematic = true;
				colliders [0].enabled = false;
				colliders [1].enabled = false;
//				getcornerposition (hit.transform.gameObject);

				StartCoroutine (Grab (hit.transform.gameObject, Time.time));
			}
		} else {
			grab = true;
		}


		if (Physics.Raycast (transform.position, -transform.up, out hit, 1.0f)) {
			if (hit.transform.gameObject.tag == "fall") {
				playerSpeed = 0;
				colliders [0].enabled = false;
				colliders [1].enabled = false;
			}
			if (hit.transform.gameObject.tag == "end") {
				playerSpeed = 0;
				anim.SetBool ("isEnd", true); 
			}
		}

//		if (Physics.Raycast (transform.position, transform.forward, out hit, 10.0f)) {
//			if (hit.transform.gameObject.tag == "box") {
//				GetComponent<Rigidbody> ().useGravity = false;
//				GetComponent<Rigidbody> ().isKinematic = true;
//				colliders [0].enabled = false;
//				colliders [1].enabled = false;
//				Debug.Log ("ok");
//				StartCoroutine (Jumping2 ());
//			}
//		}

		if (onTheGround == true && direction == SwipeControls.SwipeDirection.Jump) {
			/*jumptime = 0;*/

//			if (Physics.Raycast (transform.position, transform.forward, out hit, 5.0f)) {
//				if (hit.transform.gameObject.tag == "box") {
//					colliders [0].enabled = false;
//					colliders [1].enabled = false;
//					Debug.Log('o');
//					StartCoroutine (Jumping2 ());
//				}
//			} else {
				StartCoroutine (Jumping1 ());
//			}
		} else if (onTheGround == true && direction == SwipeControls.SwipeDirection.SLIDE) {
			colliders [0].enabled = false;
			Invoke ("StandUp", 1.0f);
			//playerSpeed *= 2.0f;
			anim.SetTrigger ("SlideTrigger");
		} else if (onTheGround == true && direction == SwipeControls.SwipeDirection.Right) {
			StartCoroutine (fast_run ());
		}  else if (onTheGround == true && direction == SwipeControls.SwipeDirection.Left) {
			GetComponent<Rigidbody> ().useGravity = false;
			GetComponent<Rigidbody> ().isKinematic = true;
			colliders [0].enabled = false;
			colliders [1].enabled = false;
			Debug.Log ("ok");

			StartCoroutine (Jumping2 ());
		}

		if (Physics.Raycast (transform.position, -transform.up, out hit, 2.0f)) {
			
		}

//		if (Physics.Raycast (transform.position, transform.forward, out hit, 5.0f)) {
//			Debug.Log (hit.transform.gameObject.tag);
//			if (hit.transform.gameObject.tag == "box") {
//				GetComponent<Rigidbody> ().useGravity = false;
//				GetComponent<Rigidbody> ().isKinematic = true;
//				colliders [0].enabled = false;
//				colliders [1].enabled = false;
//				StartCoroutine (Jumping2 ());
//			}
//		}
		//		if (Physics.Raycast (transform.position, transform.forward, out hit, 5.0f)) {
		//			Debug.Log (hit.transform.gameObject.tag);
		//			if (hit.transform.gameObject.tag == "box") {
		//				GetComponent<Rigidbody> ().useGravity = false;
		//				GetComponent<Rigidbody> ().isKinematic = true;
		//				colliders [0].enabled = false;
		//				colliders [1].enabled = false;
		//				StartCoroutine (Jumping2 ());
		//			}
		//		}
		//check holding
		Vector3 pos = new Vector3(transform.position.x + 0.4f, transform.position.y+1, transform.position.z);
		if (Physics.Raycast (pos, -transform.up, out hit)) {
			if (hit.distance > 10.0f) {
				onTheGround = false;
				holding = true;	
				playerSpeed = 4.5f;
			}
		} else {
			onTheGround = false;
			holding = true;
		}
			
		if (holding) {
			anim.SetBool ("isHolding", true);
		}

		//if character falls, check y position to trigger the game over screen
		if(transform.position.y < -50f){
			StartCoroutine(die());	
		}
	}

	public void OnCollisionEnter(Collision col){
//		if (col.gameObject.tag == "box") {
//			Debug.Log ("Box");
//			GetComponent<Rigidbody> ().useGravity = false;
//			GetComponent<Rigidbody> ().isKinematic = true;
//			colliders [0].enabled = false;
//			colliders [1].enabled = false;
//			StartCoroutine (Jumping2 ());
//		}

//		if (col.gameObject.tag == "hill") {
//			Debug.Log ("hill");
//			GetComponent<Rigidbody> ().useGravity = false;
//			GetComponent<Rigidbody> ().isKinematic = true;
//			colliders [0].enabled = false;
//			colliders [1].enabled = false;
//			StartCoroutine (Grab ());
//		}
			
		if (col.gameObject.tag == "ground") {
			if (holding) {
				holding = false;

				StartCoroutine ( rolling ());
			}

			onTheGround = true;
			jump = false;
			holding = false;
			//jumptime = 0;

			//anim.SetBool ("isHolding", true);
		}
	}

	IEnumerator Jumping1(){
		onTheGround = false;
		jump = true;
		GetComponent<Rigidbody> ().velocity = Vector3.zero;
		GetComponent<Rigidbody> ().AddForce (transform.up * jumpingForce);
		anim.SetTrigger("JumpTrigger");
		yield return new WaitForSeconds (0.5f);
//		GetComponent<Rigidbody> ().mass += Mathf.Sqrt();
//		yield return new WaitForSeconds (0.5f);
//		playerSpeed -= Time.deltaTime;
		//transform.Translate (Vector3.forward);
	}

	IEnumerator Jumping2(){

		anim.SetBool ("isJumping2", true);
		playerSpeed = 3;
		yield return new WaitForSeconds (0.5f);
		playerSpeed = 6;
		yield return new WaitForSeconds (0.5f);
		colliders [0].enabled = true;
		colliders [1].enabled = true;
		GetComponent<Rigidbody> ().useGravity = true;
		anim.SetBool ("isJumping2", false);
		GetComponent<Rigidbody> ().isKinematic = false;
	}

	IEnumerator Grab(GameObject hill, float startTime){

		if(grab == true){
			Vector3 box_corner = new Vector3(hill.transform.position.x - hill.GetComponent<Collider>().bounds.size.x/2 -0.5f , hill.transform.position.y + hill.GetComponent<Collider>().bounds.size.y/2-1.0f , 0);
			playerSpeed *= 0;
			anim.SetBool ("isGrabing", true);
			transform.position = Vector3.Lerp(transform.position, box_corner, 0.05f);
			yield return new WaitForSeconds (0.5f);
			playerSpeed = 2 ;
			box_corner = new Vector3(hill.transform.position.x - hill.GetComponent<Collider>().bounds.size.x/2 -0.5f , hill.transform.position.y + hill.GetComponent<Collider>().bounds.size.y/2+0.8f , 0);
			transform.position = Vector3.Lerp(transform.position, box_corner, 0.025f);
			yield return new WaitForSeconds (0.4f);
			playerSpeed = 4 ;

			playerSpeed += Time.deltaTime ;
			anim.SetBool ("isGrabing", false);
		}
		yield return new WaitForSeconds (0.1f);

		grab = false;
		Debug.Log ("end: " +  grab);
		colliders [0].enabled = true;
		colliders [1].enabled = true;
		GetComponent<Rigidbody> ().useGravity = true;
		GetComponent<Rigidbody> ().isKinematic = false;
		float delay = 2.0f;
		if (playerSpeed  > 6) {
			playerSpeed += Time.deltaTime * 0.05f;
		}

		playerSpeed = 6;
		holding = false;
		anim.SetBool ("isHolding" ,false);
	}

	IEnumerator rolling(){
		playerSpeed = 3;
		anim.SetTrigger("LandingTrigger");
		anim.SetBool ("isHolding" ,false);
		yield return new WaitForSeconds (1.8f);
		playerSpeed = 6;
	}


//	public void getcornerposition(GameObject hill) {
//		Transform box = hill.transform; // This is the box
//		BoxCollider boxCollider = hill.GetComponent<BoxCollider>(); // This is the box's BoxCollider
//
//		// Box Collider center in world coordinates
//		Vector3 centerWorld = box.position + Vector3.Scale(box.localScale, boxCollider.center);
//
//		// Box Collider world size
//			Vector3 sizeWorld = Vector3.Dot (box.localScale, boxCollider.size);
//
//		for (int i = -1; i <= 1; i+=2){
//			for (int j = -1; j <= 1; j+=2){
//				for (int k = -1; k <= 1; k+=2){
//					float xCoord = centerWorld.x + i*(sizeWorld.x/2);
//					float yCoord = centerWorld.y + j*(sizeWorld.y/2);
//					float zCoord = centerWorld.z + k*(sizeWorld.z/2);  
//					Vector3 anotherCorner = new Vector3(xCoord, yCoord, zCoord);
//					//Store it somewhere          
//				}              
//			}
//		}
//	}


	public void StandUp(){
		colliders [0].enabled = true;
		//anim.SetBool ("isSliding", false);
		//playerSpeed /= 2.0f;
		//transform.Translate (Vector3.forward * 1.5f);
	}

    void FixedUpdate(){
		
    }
		
	IEnumerator die(){
		//set bool dead true, add effect and add the game over screen using manager.gameOver
		dead = true;
		GetComponent<Rigidbody>().constraints &= ~RigidbodyConstraints.FreezeRotationX;
		ParticleSystem effect = Instantiate(deadEffect);
		effect.transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
		gameManager.GetComponent<manager>().gameOver();
		yield return new WaitForSeconds(0.1f);
		this.gameObject.SetActive(false);
	}

	IEnumerator fast_run(){
		playerSpeed *= 1.5f;
		anim.speed *= 1.5f;
		yield return new WaitForSeconds(2.0f);
		playerSpeed /= 1.5f;
		anim.speed /= 1.5f;
	}
}
