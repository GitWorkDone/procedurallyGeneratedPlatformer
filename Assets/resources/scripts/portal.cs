using UnityEngine;
using System.Collections;

public class portal : MonoBehaviour {

	public static float m_positionX;
	public static float m_positionY;
	public static float m_teleToX;
	public static float m_teleToY;

	public GameObject portalGameObject;


	// Use this for initialization
	void Start () {
		//checks the block's position relative to the player every 5 seconds
		InvokeRepeating ("KillPortal", 5, 5);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void KillPortal() 
	{
		//makes sure the player isn't in a dungeon
		if (characterController.isAtNormalRealm) {
			float tmp = characterController.GetPlayerPositionX (); 
			//destroys the portal if it is a certain distance from the player (prevents framerate drops)
			if (transform.position.x + 7.6f < tmp) {
				controller.portalIsSpawned = false;
				Destroy (portalGameObject);
			}
		}
	}
	//sets where the player will be teleported to when entering the portal
	public static void SetPositionX(float positionX) {
		m_positionX = positionX;
	}

	public static void SetPositionY(float positionY) {
		m_positionY = positionY;
	}

	public static void SetTeleToX(float teleToX){
		m_teleToX = teleToX;

	}
	public static void SetTeleToY(float teleToY){
		m_teleToY = teleToY;
		
	}

}
