using UnityEngine;
using System.Collections;

public class cameraController : MonoBehaviour {

	public static float cameraStop;
	public GameObject myCamera;
	public static float playerMostRight;
	// Use this for initialization
	void Start () {
		cameraStop = 0;
	}
	
	// Update is called once per frame
	void Update () {
		//keeps track of the farthest the player has travelled to the right
		if (playerMostRight < characterController.myX) {
			playerMostRight = characterController.myX;

			//prevents the camera from moving to the left if the player backtracks a certain amount
			//this prevents the player from seeing deleted blocks
			if(playerMostRight > 5) {
				cameraStop = playerMostRight - 5;
			}
			else {
			//the initial position of the camera (no going left at the start)
				cameraStop = 0;
			}
		}
		if (characterController.myX < cameraStop) {
			//restrains the camera's position if the character is going too far left
			myCamera.transform.position = new Vector3 (cameraStop, transform.position.y, -1);
		}
	}

	public static void UpdateCameraStop(float cameraX) {
		cameraStop = cameraX;
	}

}
