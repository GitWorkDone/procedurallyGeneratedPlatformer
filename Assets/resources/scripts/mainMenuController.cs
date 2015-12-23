using UnityEngine;
using System.Collections;

public class mainMenuController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void LoadGameScene()
    {
        Application.LoadLevel(1);
    }
}
