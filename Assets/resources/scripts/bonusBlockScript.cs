using UnityEngine;
using System.Collections;

public class bonusBlockScript : MonoBehaviour {

	public GameObject bonusBlockObject;

	// Use this for initialization
	void Start () {
		//checks the block's position relative to the player every 5 seconds
		InvokeRepeating ("KillBlock", 5, 5);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void KillBlock() 
	{
		//makes sure the player isn't in a dungeon
		if (characterController.isAtNormalRealm) {
			float tmp = characterController.GetPlayerPositionX (); 
			//destroys the block if it is a certain distance from the player (prevents framerate drops)
			if (transform.position.x + 5 < tmp) {
				Destroy (bonusBlockObject);
			}
		}
	}
}
