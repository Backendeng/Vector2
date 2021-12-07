using UnityEngine;
using System.Collections;

public class bot : MonoBehaviour {
	
	//variable visible in the inspector
    public float jumpingForce;
	//public Transform upperBody;
	//public GameObject bullet;
	public ParticleSystem deadEffect;
	public ParticleSystem coinParticles;
	public string saveCoinsAs = "coins";


	//OGC SwipeControls
	//private SwipeControls swipeLogic;
	public Animator anim;
	private CapsuleCollider[] colliders;
	//variables not visible in the inspector
	public static bool dead;

	bool holding = false;
	bool onTheGround;
    bool jump;
	//private float jumptime = 0;

	//GameObject newBullet;
	GameObject gameManager;

	public float playerSpeed;
	void Start(){
		colliders = GetComponents<CapsuleCollider> ();

		anim = GetComponent<Animator> ();
		anim.SetBool ("isRunning", true);
		anim.SetBool ("isHolding", false);
		anim.SetBool ("isLanding", false);
		onTheGround = true;
		jump = false;
		dead = false;
	
		//find game manager
		gameManager = GameObject.Find("Game manager");
		
		//OGC_Get SwipeLogic
		//swipeLogic = transform.GetComponent <SwipeControls>();
	}
  	
    void Update(){
		transform.Translate (Vector3.forward * playerSpeed * Time.deltaTime);
		transform.position = new Vector3 (transform.position.x, transform.position.y, 0);

		//OGC get direction
		//SwipeControls.SwipeDirection direction = swipeLogic.getSwipeDirection();

		RaycastHit hit;
		//Debug.Log (transform.position);
		/*if (Physics.Raycast (transform.position, transform.forward, out hit, 5.0f)) {
			Debug.Log ("asdfasd");
			if (hit.transform.gameObject.tag == "box") {
				GetComponent<Rigidbody> ().useGravity = false;
				GetComponent<Rigidbody> ().isKinematic = true;
				colliders [0].enabled = false;
				colliders [1].enabled = false;
				StartCoroutine (Jumping2 ());
			}
		}*/

		if (onTheGround == true && transform.position.x >= 42.0f && transform.position.x <= 43.0f) {
			/*jumptime = 0;*/
			onTheGround = false;
			jump = true;

			/*if (Physics.Raycast (transform.position, transform.forward, out hit, 5.0f)) {
				if (hit.transform.gameObject.tag == "box") {
					colliders [0].enabled = false;
					colliders [1].enabled = false;
					StartCoroutine (Jumping2 ());
				}
			} else {*/
				GetComponent<Rigidbody> ().velocity = Vector3.zero;
				GetComponent<Rigidbody> ().AddForce (transform.up * jumpingForce);
				anim.SetTrigger("JumpTrigger");
			//}
		} /*else if (onTheGround == true && direction == SwipeControls.SwipeDirection.SLIDE) {
			colliders [0].enabled = false;
			Invoke ("StandUp", 1.0f);
			anim.SetTrigger ("SlideTrigger");
		}*/

		//check holding
		Vector3 pos = new Vector3(transform.position.x + 0.4f, transform.position.y+1, transform.position.z);
		if (Physics.Raycast (pos, -transform.up, out hit)) {
			if (hit.distance > 3.0f) {
				onTheGround = false;
				holding = true;
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
		if (col.gameObject.tag == "box") {
			Debug.Log ("Box");
			GetComponent<Rigidbody> ().useGravity = false;
			GetComponent<Rigidbody> ().isKinematic = true;
			colliders [0].enabled = false;
			colliders [1].enabled = false;
			StartCoroutine (Jumping2 ());
		}
			
		if (col.gameObject.tag == "ground") {
			
			if (holding) {
				holding = false;
				anim.SetTrigger("LandingTrigger");
				anim.SetBool ("isHolding" ,false);
			}

			onTheGround = true;
			jump = false;
			holding = false;
			//jumptime = 0;

			//anim.SetBool ("isHolding", true);
		}
	}

	IEnumerator Jumping2(){

		anim.SetBool ("isJumping2", true);
		yield return new WaitForSeconds (1.0f);
		colliders [0].enabled = true;
		colliders [1].enabled = true;
		GetComponent<Rigidbody> ().useGravity = true;
		anim.SetBool ("isJumping2", false);
		GetComponent<Rigidbody> ().isKinematic = false;
		//transform.Translate (Vector3.forward);
	}

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
}
