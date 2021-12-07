using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeControls : MonoBehaviour {

	public enum SwipeDirection
	{
		Null = 0,
		SLIDE = 1,
		Jump = 2,
		Right = 3,
		Left = 4
	}

	private float fsensitivity = 100f;

	private SwipeDirection sSwipeDirection;
	private float fInitialX;
	private float fInitialY;
	private float fFinalX;
	private float fFinalY;
	private float iTouchStateFlag;

	private float inputX;
	private float inputY;
	private float slope;
	private float fDistance;

	// Use this for initialization
	void Start () {
		fInitialX = 0.0f;
		fInitialY = 0.0f;
		fFinalX = 0.0f;
		fFinalY = 0.0f;

		inputX = 0.0f;
		inputY = 0.0f;

		iTouchStateFlag = 0;
		sSwipeDirection = SwipeDirection.Null;
	}
	
	// Update is called once per frame
	void Update () {
		if (iTouchStateFlag == 0 && Input.GetMouseButtonDown (0)) {
			fInitialX = Input.mousePosition.x;
			fInitialY = Input.mousePosition.y;

			sSwipeDirection = SwipeDirection.Null;
			iTouchStateFlag = 1;
		}
		if (iTouchStateFlag == 1) {
			fFinalX = Input.mousePosition.x;
			fFinalY = Input.mousePosition.y;

			sSwipeDirection = swipeDirection ();
			if (sSwipeDirection != SwipeDirection.Null) {
				iTouchStateFlag = 2;
			}
			if (iTouchStateFlag == 2 || Input.GetMouseButtonUp (0)) {
				iTouchStateFlag = 0;
			}
		}
	}
	//get mouse direction
	private SwipeDirection swipeDirection() {
		inputX = fFinalX - fInitialX;
		inputY = fFinalY - fInitialY;
		slope = inputY / inputX;

		fDistance = Mathf.Sqrt (Mathf.Pow ((fFinalY - fInitialY), 2) + Mathf.Pow ((fFinalX - fInitialX), 2));

		if (fDistance <= (Screen.width / fsensitivity))
			return SwipeDirection.Null;

		if (inputX >= 0 && inputY > 0 && slope > 1) {
			return SwipeDirection.Jump;
		} else if (inputX <= 0 && inputY > 0 && slope < -1) {
			return SwipeDirection.Jump;
		} else if (inputX > 0 && inputY <= 0 && slope < 1 && slope >= 0) {
			return SwipeDirection.Right; 
		} else if (inputX > 0 && inputY <= 0 && slope > -1 && slope <= 0) {
			return SwipeDirection.Right;
		} else if (inputX < 0 && inputY >= 0 && slope > -1 && slope < 1) {
			return SwipeDirection.Left;
		} else if (inputX < 0 && inputY <= 0 && slope >= 0 && slope < 1) {
			return SwipeDirection.Left;
		} else if (inputX >= 0 && inputY < 0 && slope < -1) {
			return SwipeDirection.SLIDE;
		} else if (inputX <= 0 && inputY < 0 && slope > 1) {
			return SwipeDirection.SLIDE;
		}
		return SwipeDirection.Null;
	}

	public SwipeDirection getSwipeDirection() {
		if (sSwipeDirection != SwipeDirection.Null) {
			var etempSwipeDirection = sSwipeDirection;
			sSwipeDirection = SwipeDirection.Null;
			return etempSwipeDirection;
		} else
			return SwipeDirection.Null;
	}
}
