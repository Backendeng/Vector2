using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class manager : MonoBehaviour {
	
	//variables visible in the inspector
	[Space(10)]
	public bool pcControls;
	public bool deletePlayerPrefs;
	
	[Header("Background color effect")]
	public Color color1;
    public Color color2;
    public float duration;
	
	[Space(10)]
	[TextArea(1, 10)]
	public string mobileHelpText = "";
	
	[TextArea(1, 10)]
	public string pcHelpText = "";
    
	//variables not visible in the inspector
    //new Camera camera;
	
	float camStartTime;
    float camJourneyLength;
	float distance;
	
	Vector3 startingCamPos;
	Vector3 secondCamPos = new Vector3(0f, 2.7f, -12f);
	
	public static GameObject startPanel;
	public static GameObject startPanelTitle;
	public static GameObject gameOverPanel;
	public static GameObject pauseButton;
	public static GameObject pausePanel;
	
	GameObject startButton;
	GameObject howToPlayButton;
	GameObject pressKeyToStartText;
	GameObject targetDistanceText;
	GameObject howToPlayPanel;
	
	bool fading;
	bool showDistance;
	public Transform character;
    void Start(){
		//reset some variables
		distance = 0;
		Time.timeScale = 0;	
		showDistance = false;
		
		//if you want to delete the playerprefs, just delete them all
		if(deletePlayerPrefs){
			PlayerPrefs.DeleteAll();	
		}
		
		//find camera and set it's position and size
		/*camera = Camera.main.GetComponent<Camera>();
		camera.orthographicSize = 0.1f;
		startingCamPos = Camera.main.transform.position;
		camJourneyLength = Vector3.Distance(startingCamPos, secondCamPos);
		camStartTime = Time.time;*/
		
		//find some objects
		startPanel = GameObject.Find("start screen");
		startPanelTitle = GameObject.Find("title");
		
		pauseButton = GameObject.Find("pause button");
		pausePanel = GameObject.Find("pause screen");
		pauseButton.SetActive(false);
		pausePanel.SetActive(false);
		
		howToPlayPanel = GameObject.Find("how to play screen");
		GameObject helpText = howToPlayPanel.transform.Find("how to play text").gameObject;
		//change help text (how to play) according to the pc/mobile controls
		if(pcControls){
			helpText.GetComponent<Text>().text = pcHelpText;
		}
		else{
			helpText.GetComponent<Text>().text = mobileHelpText;	
		}
		
		//find some more objects
		howToPlayButton = GameObject.Find("how to play button");
		howToPlayPanel.SetActive(false);
		
		targetDistanceText = GameObject.Find("target distance");
		targetDistanceText.GetComponent<Text>().text = "Target: " + PlayerPrefs.GetInt("bestDistance") + "m";
		targetDistanceText.SetActive(false);	
		
		gameOverPanel = GameObject.Find("game over screen");
		gameOverPanel.SetActive(false);
		
		startButton = GameObject.Find("start button");
		pressKeyToStartText = GameObject.Find("press to start text");
		
		//turn buttons on only when you want mobile controls. Else, add text and use keyboard keys
		if(pcControls){
			startButton.SetActive(false);	
			howToPlayButton.SetActive(false);
			pressKeyToStartText.SetActive(true);
		}
		else{
			pressKeyToStartText.SetActive(false);
			startButton.SetActive(true);
			howToPlayButton.SetActive(true);
		}
    }
    
	void Update(){	
		//for pc, use space to start and h for help panel
		/*if(pcControls && startPanel.activeSelf && Input.GetKeyDown("space")){
			play();
		}	
		if(pcControls && startPanel.activeSelf && Input.GetKeyDown("h")){
			help();	
		}*/
		
		//add distance when not game over
		if(!player.dead){
			distance += Time.deltaTime * 5;
		}
		
		/*if(showDistance){
			//set distance text to current distance
			targetDistanceText.GetComponent<Text>().text = (int)distance + "m";
		}*/
		
		//do some calculations to lerp the camera position from starting position to second position
		/*float distCovered = (Time.time - camStartTime) * 10;
	    float fracJourney = distCovered / camJourneyLength;
	    Camera.main.transform.position = Vector3.Lerp(startingCamPos, secondCamPos, fracJourney);
		
		//zoom out using orthographic size
		if(camera.orthographicSize < 7){
			camera.orthographicSize += Time.deltaTime;
		}	
		
		//lerp camera background color all the time
		float i = Mathf.PingPong(Time.time, duration) / duration;
	    camera.backgroundColor = Color.Lerp(color2, color1, i);*/
		
		//increase timescale to make the game harder
		if(!player.dead && !startPanel.activeSelf){
			Time.timeScale += Time.deltaTime * 0.005f;
		}
		
		//fade effect for the main title when starting the game
		if(fading && startPanelTitle.GetComponent<Image>().color.a > 0){
			
			var color = new Color(
			startPanelTitle.GetComponent<Image>().color.r,
			startPanelTitle.GetComponent<Image>().color.g,
			startPanelTitle.GetComponent<Image>().color.b,
			startPanelTitle.GetComponent<Image>().color.a);
			color.a -= Time.deltaTime * 2;
			startPanelTitle.GetComponent<Image>().color = color;
		}
		else if(fading){
			//stop fading effect and show the target distance after some time
			fading = false;	
			startPanel.SetActive(false);
			StartCoroutine(targetDistance());
		}
	
		//if you press escape or back button on mobile devices, completely restart the scene
		if(Input.GetKeyDown(KeyCode.Escape)){
			Application.LoadLevel(Application.loadedLevel);
		}
	}
	
	//fade and start running
    public void play(){
		fading = true;	
		Time.timeScale = 1;	
    }
	
	//open help panel and set some text/buttons false
	public void help(){
		if(pcControls){	
			pressKeyToStartText.SetActive(false);
			howToPlayPanel.SetActive(true);
		}
		else{
			startButton.SetActive(false);
			howToPlayButton.SetActive(false);
			howToPlayPanel.SetActive(true);
		}	
	}
	
	//close the help panel and go back to the start screen again
	public void back(){
		if(pcControls){	
			howToPlayPanel.SetActive(false);
			pressKeyToStartText.SetActive(true);
		}
		else{
			howToPlayPanel.SetActive(false);
			startButton.SetActive(true);
			howToPlayButton.SetActive(true);
		}	
	}
	
	//save distance and show game over panel
	public void gameOver(){
		if((int)distance > PlayerPrefs.GetInt("bestDistance")){
			PlayerPrefs.SetInt("bestDistance", (int)distance);
		}	
		pauseButton.SetActive(false);
		gameOverPanel.SetActive(true);
	}
	
	//pause when pressing the pause button and save the current timescale
	float currentTimeScale;
	public void pause(){
		currentTimeScale = Time.timeScale;
		Time.timeScale = 0;
		pauseButton.SetActive(false);
		pausePanel.SetActive(true);	
	}
	
	//use the saved timescale to resume the game as fast
	public void resume(){
		Time.timeScale = currentTimeScale;
		pausePanel.SetActive(false);		
		pauseButton.SetActive(true);
	}
	
	IEnumerator targetDistance(){
		//create the flicker effect using for loop in coroutine
		bool onOff = true;
		for(int i = 0; i < 10; i++){
			if(onOff){
				targetDistanceText.SetActive(true);	
				onOff = false;
			}	
			else{
				targetDistanceText.SetActive(false);	
				onOff = true;	
			}
			yield return new WaitForSeconds(0.3f);
		}
		//set target distance not active, wait some time and show current distance. Also show a pause button
		targetDistanceText.SetActive(false);
		yield return new WaitForSeconds(1.5f);
		targetDistanceText.SetActive(true);
		targetDistanceText.GetComponent<Text>().text = "";
		showDistance = true;
		pauseButton.SetActive(true);
	}
}
