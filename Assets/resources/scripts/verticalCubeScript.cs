using UnityEngine;
using System.Collections;

public class verticalCubeScript : MonoBehaviour {

	public GameObject cubeObject;
	float speed;
	public Rigidbody2D rB;
	
	
	
	// Use this for initialization
	void Start () {
		speed = 0.4f;

		//checks the block's position relative to the player every 5 seconds
		InvokeRepeating ("KillBlock", 5, 5);
		rB = GetComponent<Rigidbody2D> ();

		//changes the direction the blocks are moving
		InvokeRepeating ("ChangeDirection", 0.8f, 1.6f);
	}
	
	// Update is called once per frame
	void Update () {
		rB.velocity = new Vector2 (0, speed);
	}
	void KillBlock() 
	{
		if (characterController.isAtNormalRealm) {
			float tmp = characterController.GetPlayerPositionX (); 
			//destroys the block if it is a certain distance from the player (prevents framerate drops)
			if (transform.position.x + 7.6f < tmp) {
				Destroy (cubeObject);
			}
		}
	}
	//changes blocks direction
	void ChangeDirection() {
		speed = -speed;
	}
}