using UnityEngine;
using System.Collections;

public class horizontalCubeScript : MonoBehaviour {

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
		InvokeRepeating ("ChangeDirection", 0.6f, 1.2f);
	}
	
	// Update is called once per frame
	void Update () {
			rB.velocity = new Vector2 (speed, 0);
	}
	void KillBlock() 
	{
		//makes sure the player isn't in a dungeon
		if (characterController.isAtNormalRealm) {
			float tmp = characterController.GetPlayerPositionX (); 
			//destroys the block if it is a certain distance from the player (prevents framerate drops)
			if (transform.position.x + 7.6f < tmp) {
				Destroy (cubeObject);
			}
		}
	}
	//changes the direction the blocks are moving
	void ChangeDirection() {
		speed = -speed;
	}
}