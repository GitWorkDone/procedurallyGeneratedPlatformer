using UnityEngine;
using System.Collections;

public class characterController : MonoBehaviour {
    public float xMovement = 0f;
	public float yMovement = 0f;
	public bool grounded = false;
	public float speed = 1;
	public float otherXVelocity = 0;
	public Rigidbody2D rB;
	public static float myX;
	public static float myY;
	public static float playerFurthestRight;
	public static bool spawnPortalRealm = false;
	public static bool isAtNormalRealm = true;
	public static float cameraPositionDifference = 0;
	public GameObject horizontalLayer;



	// Use this for initialization
	void Start () {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0.0f);
		rB = GetComponent<Rigidbody2D> ();

    }
	
	// Update is called once per frame
	void Update () {
		myX = transform.position.x;
		myY = transform.position.y;
		//controls the character's movement
		//otherXVelocity adds speed from moving platforms when the player stands on them
		rB.velocity = new Vector2(xMovement * speed + otherXVelocity, yMovement); 
		if (cameraController.cameraStop < transform.position.x) {
			//tracks how far the player is from the farthest to the left (camera stop position) they are allowed to go
			cameraPositionDifference = transform.position.x - cameraController.cameraStop;
		}
			if (Input.anyKey) {    
			//moves player to the right
			if (Input.GetKey (KeyCode.RightArrow) || (Input.GetKey (KeyCode.D))) {
				xMovement = 1f;


			} 
			//moves player to the left
			else if (Input.GetKey (KeyCode.LeftArrow) || (Input.GetKey (KeyCode.A))) {
				xMovement = -1f;
			}
			//currently does nothing
			else if (Input.GetKey ("s")) {

			} 
			//doesn't do anything either
			else if (Input.GetKey ("w")) {

			} 
		}
		//makes character jump if touching the ground
		if (Input.GetKeyDown ("space")) {
			if (grounded) {
				yMovement = 3.3f;
			}
		}
		//makes the character sprint
		if (Input.GetKey (KeyCode.LeftShift)) {
			speed = 2;
			Debug.Log ("Speed boost!");
		}
		else {
			speed = 1;
		}

		//stops movement if there is no input
		if ((!Input.GetKey (KeyCode.RightArrow)) && (!Input.GetKey (KeyCode.D)) && (!Input.GetKey (KeyCode.LeftArrow)) && (!Input.GetKey (KeyCode.A))) {
			xMovement = 0f;
		}
		//makes character fall if he isn't
		if (yMovement > -2f) {
			yMovement -= .1f;
		}
	}

	void OnTriggerEnter2D(Collider2D trigger) {
		//player dies if overlapping with black bar (not implemented yet)
		if (trigger.gameObject.tag == "death") {
			Debug.Log("You dead.");
			// Load gameOver Scene.
		}

		if (trigger.gameObject.tag == "bonusBlock") {
			Debug.Log("You hit your head.");
			// Pop Block up like in mario.
			//not fully implemented yet
		}
	}

	void OnTriggerStay2D(Collider2D trigger) {
		//determines when the player is touching the ground (those gray rectangles)
		if (trigger.gameObject.tag == "ground" || trigger.gameObject.tag == "horizontal" || trigger.gameObject.tag == "vertical") {
			grounded = true;
			Debug.Log ("You are grounded.");
		}

		//if the player is on a horizontally moving platform, finds the platforms x velocity
		//this velocity is added to the player's current x velocity
		if (trigger.gameObject.tag == "horizontal") {
			otherXVelocity = trigger.attachedRigidbody.velocity.x;
		}

		//sends player to dungeon if they go through a portal (the purple squares)
		if (trigger.gameObject.tag == "portal" ) {
			if (Input.GetKeyDown(KeyCode.B)){
				if(isAtNormalRealm) {
					portal.SetTeleToX(portal.m_positionX);
					portal.SetTeleToY(portal.m_positionY - 9.0f);
					spawnPortalRealm = true;
					transform.position =  new Vector3(portal.m_teleToX,portal.m_teleToY, 0);

				}
				else {
					//TeleportToNormalRealm();
				}
			}
		}
	}

	void OnTriggerExit2D(Collider2D trigger) {
		//determines when you stop touching the ground
		if (trigger.gameObject.tag == "ground" || trigger.gameObject.tag == "horizontal" || trigger.gameObject.tag == "vertical"){
			grounded = false;
			Debug.Log ("You are not grounded.");
		}

		//stops adding platform velocity when you stop touching the platform
		if (trigger.gameObject.tag == "horizontal") {
			otherXVelocity = 0;
		}
	}

	public static float GetPlayerPositionX() {
		return myX;
	}

	public static float GetPlayerPositionY() {
		return myY;
	}

}
