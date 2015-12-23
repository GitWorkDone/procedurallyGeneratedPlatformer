using UnityEngine;
using System.Collections;

public class controller : MonoBehaviour {
	
	public static bool portalIsSpawned = false;

	public GameObject square;
	public GameObject squareTwo;
	public GameObject upDownSquare;
	public GameObject leftRightSquare;
	public GameObject squareLayer;
	public GameObject upDownSquareLayer;
	public GameObject leftRightSquareLayer;
	public GameObject squareNonCollidable;
	public GameObject portalGameObject;
	public GameObject deathBlock;
	public GameObject bonusBlock;
	public GameObject bonusBlockDetector;
	public GameObject dungeonBlock;
	public GameObject dungeonBlockTwo;
	public float runningXPos;
	public float runningYPos;
	public float runningPortalXPos;
	public float runningPortalYPos;
	public float runningPortPlatXPos;
	public float runningPortPlatYPos;
	public float squareXSize;
	public float squareYSize;
	public float squareLayerXSize;
	public float squareLayerYSize;
	public float maxGapWidth;
	public float minGapWidth;
	public float actualGapWidth;
	public float breakableSquareGapHeight;
	public float playerX;
	public float dungeonHeight;
	public float dungeonWidth;
	public bool justSpawnedGap = true;
	public bool justSpawnedGround = true;
	public bool decider = true;
	public bool shallISpawn = true;
	public bool topLayer = true;
	public int trueInARow = 0;
	public int currentTrueInARow;
	public int portalNumber;
	public int levelSeed;
	public const float NORMALREALMGROUNDHEIGHT = -0.5f;
	public const float NORMALREALMDETECTIONHEIGHT = -0.4f;
	public const float NORMALREALMBRICKDETECTIONHEIGHT = -0.6f;
	public const float NORMALREALMDEATHHEIGHT = -1.5f;
	public const float PORTALREALMGROUNDHEIGHT = -11.0f;
	public const float PORTALREALMDETECTIONHEIGHT = -10.9f;



	void Awake () {
	}

	// Use this for initialization
	void Start () {
		//gets the dimensions of the squares
		squareXSize = square.GetComponent<SpriteRenderer> ().bounds.size.x;
		squareYSize = square.GetComponent<SpriteRenderer> ().bounds.size.y;
		squareLayerXSize = squareLayer.GetComponent<SpriteRenderer> ().bounds.size.x;
		squareLayerYSize = squareLayer.GetComponent<SpriteRenderer> ().bounds.size.y;
		//sets the max and min distances between platforms
		maxGapWidth = 20 * squareXSize;
		minGapWidth = 3 * squareLayerXSize;
		//set how high the breakable squares spawn relative to the normal ground
		breakableSquareGapHeight = 3 * square.GetComponent<SpriteRenderer> ().bounds.size.y;
		cameraController.UpdateCameraStop(runningXPos - squareXSize * 1);
		portalNumber = 0;
		portalIsSpawned = false;
		//sets the initial spawning point of the ground squares
		runningXPos -= (4 * squareXSize);
		//spawns the initial ground that the player starts on
		InstantiateSpawningArea ();
		runningXPos += (9 * squareXSize);
		//spawns other groups of platforms
		InvokeRepeating ("SpawnGround",0, 0.1f);
		//checks the player's location to determine what blocks to destroy and how far ahead blocks should spawn
		InvokeRepeating ("FindPlayer", 5, 5);
	}
	
	// Update is called once per frame
	void Update () {
		if (characterController.spawnPortalRealm) {
			//spawns a dungeon area when the player enters a portal (dungeons are not fully functioning)
			InstantiatePortalRealm();
			characterController.spawnPortalRealm = false;
		}
	}

	// Spawn Tiles
	void SpawnGround() {
		if (playerX + 10 > runningXPos) {
			//decides whether or not to spawn tiles
			if (decider) {
				runningXPos += squareXSize;
				justSpawnedGap = false;
				justSpawnedGround = true;
				//spawns tiles
				InstantiateGroundBlocks ();
			} 
			else if (!decider) {
				actualGapWidth = Random.Range (minGapWidth, maxGapWidth);
				justSpawnedGround = false;
				justSpawnedGap = true;
				InstantiateGroundBlocks ();
				//creates a gap between platforms
				runningXPos += actualGapWidth;
				if(actualGapWidth >= 9 * squareXSize) {
					//creates floating platforms if the gap is big
					InstantiateFloatingPlatforms();
				}
				//spwans the blacks blocks below the ground
				InstantiateDeathBlocks(runningXPos, actualGapWidth, NORMALREALMDEATHHEIGHT);
				InstantiateGroundBlocks ();
			}

			//randomly chooses to spawn blocks or a gap
			int tmp = Random.Range (0, 20);
			if (tmp < 15) {
				decider = true;
				trueInARow++;
			} else {
				decider = false;
				trueInARow = 0;

			}
		}
	}

	void InstantiateSpawningArea() {
		//spawns the first chunk of ground 10x10
		for (int g = 0; g < 10; g++) {
			topLayer = true;
			for (int h = 0; h < 10; h++) {
				if (topLayer) {
					Instantiate (square, new Vector3 (runningXPos - squareXSize * g, NORMALREALMGROUNDHEIGHT, 0), Quaternion.identity);
					Instantiate (squareLayer, new Vector3 (runningXPos - squareXSize * g, NORMALREALMDETECTIONHEIGHT, 0), Quaternion.identity);
					topLayer = false;
				}
				else {
					Instantiate (squareNonCollidable, new Vector3 (runningXPos - squareXSize * g, NORMALREALMGROUNDHEIGHT - h * squareYSize, 0), Quaternion.identity);
				}
			}
		}
		//spawns the top layer of blocks with squareLayer (grounded detector) on top (so the player knows when he can jump)
		for (int i = 0; i < 10; i++) {
			topLayer = true;
			for (int j = 0; j < 10; j++) {
				if (topLayer) {
					Instantiate (square, new Vector3 (runningXPos + squareXSize * i, NORMALREALMGROUNDHEIGHT, 0), Quaternion.identity);
					Instantiate (squareLayer, new Vector3 (runningXPos + squareXSize * i, NORMALREALMDETECTIONHEIGHT, 0), Quaternion.identity);
					topLayer = false;
				}
				else {
					Instantiate (squareNonCollidable, new Vector3 (runningXPos + squareXSize * i, NORMALREALMGROUNDHEIGHT - j * squareYSize, 0), Quaternion.identity);
				}
			}
		}
	}

	void InstantiateGroundBlocks() {
		//spawns a set of ground blocks, 1 wide and 10 high
		for (int i = 0; i < 1; i++) {
			topLayer = true;
			for (int j = 0; j < 10; j++) {
				if (topLayer) {
					Instantiate (square, new Vector3 (runningXPos, NORMALREALMGROUNDHEIGHT, 0), Quaternion.identity);
					Instantiate (squareLayer, new Vector3 (runningXPos, NORMALREALMDETECTIONHEIGHT, 0), Quaternion.identity);
					topLayer = false;
				}
				else {
					if (justSpawnedGap) {
						//spawns collidable squares on the ends of the chunks of ground
						Instantiate (squareTwo, new Vector3 (runningXPos, NORMALREALMGROUNDHEIGHT - j * squareYSize, 0), Quaternion.identity);
					   
					}
					else {
						//spawns noncollidable platforms in the middle of the chunks of ground
						Instantiate (squareNonCollidable, new Vector3 (runningXPos, NORMALREALMGROUNDHEIGHT - j * squareYSize, 0), Quaternion.identity);
					}
				}
			}
		}
		InstantiateBreakableBlocks ();
	}

	//spawns the question mark blocks above the normal ground
	void InstantiateBreakableBlocks() {
		//randomly chooses whether or not to spawn question mark blocks
		int tmp = Random.Range (1, 3);
		//runs if a gap was not spawned
		if (currentTrueInARow < trueInARow) {
			currentTrueInARow++;
		} 
		//runs if a gap was just spawned - but the chunk of blocks before the gap was longer than 3 blocks
		else if (trueInARow == 0 && currentTrueInARow >= 3 && tmp == 2) {
			currentTrueInARow++;

			//determines how many question mark blocks will spawn
			//and where they will be spawned relative to the ground below them
			int numberSpawned = Random.Range (2, currentTrueInARow - 1);
			int maxLeftShift = currentTrueInARow - numberSpawned - 2;
			int actualLeftShift = Random.Range (0, maxLeftShift + 1);

			if (numberSpawned < 6) {
				//spawns a smaller number of question mark blocks
				for (int k = 1; k <= numberSpawned; k++) {
					Instantiate (bonusBlock, new Vector3 (runningXPos - (k + actualLeftShift) * squareXSize, NORMALREALMGROUNDHEIGHT + breakableSquareGapHeight, 0), Quaternion.identity);
					Instantiate (squareLayer, new Vector3 (runningXPos - (k + actualLeftShift) * squareXSize, NORMALREALMDETECTIONHEIGHT + breakableSquareGapHeight, 0), Quaternion.identity);
					Instantiate (bonusBlockDetector, new Vector3 (runningXPos - (k + actualLeftShift) * squareXSize, NORMALREALMBRICKDETECTIONHEIGHT + breakableSquareGapHeight, 0), Quaternion.identity);
				}
			}
			if (numberSpawned >= 6) {
				//if a larger number of question mark blocks was to be spawned, they may have a gap between them
				int gapSize = Random.Range (1, numberSpawned - 3);

				//determines the size of the two sections of question mark blocks
				int firstGroupSize = Random.Range (2, numberSpawned - gapSize - 1);
				int secondGroupSize = numberSpawned - gapSize - firstGroupSize;
				//spawns the first group
				for (int k = 1; k <= firstGroupSize; k++) {
					Instantiate (square, new Vector3 (runningXPos - (k + actualLeftShift) * squareXSize, NORMALREALMGROUNDHEIGHT + breakableSquareGapHeight, 0), Quaternion.identity);
					Instantiate (squareLayer, new Vector3 (runningXPos - (k + actualLeftShift) * squareXSize, NORMALREALMDETECTIONHEIGHT + breakableSquareGapHeight, 0), Quaternion.identity);
				}
				//spawns the second group
				for (int k = 1; k <= secondGroupSize; k++) {
					Instantiate (square, new Vector3 (runningXPos - (k + actualLeftShift + gapSize + firstGroupSize) * squareXSize, NORMALREALMGROUNDHEIGHT + breakableSquareGapHeight, 0), Quaternion.identity);
					Instantiate (squareLayer, new Vector3 (runningXPos - (k + actualLeftShift + gapSize + firstGroupSize) * squareXSize, NORMALREALMDETECTIONHEIGHT + breakableSquareGapHeight, 0), Quaternion.identity);
				}
				
			}
			currentTrueInARow = 0;
		} 
		//does not spawn question mark blocks
		else if (trueInARow == 0 && currentTrueInARow >= 3 && tmp == 1) {
			currentTrueInARow = 0;
		} 
		else {	
			//spawns a portal
			InstantiatePortal ();
			currentTrueInARow = 0;
		}

			
	}

	void InstantiateFloatingPlatforms() {
		//determines whether to spawn static, horiz moving, or vert moving platforms
		int platformType = Random.Range (1, 4);
		//determines the midpoint of the gap
		float midpoint = Mathf.Round (actualGapWidth / squareXSize / 2) * squareXSize;
		//determines width and height of floating platforms
		int platformsWideToSpawn = Random.Range (2, 5);
		int platformHeight = Random.Range (-3, 5);
		int startingK = Random.Range (-2, 0);
		if (platformType == 1) {
		//spawns static platforms
			for (int k = startingK; k < startingK + platformsWideToSpawn; k++) {
				Instantiate (square, new Vector3 (runningXPos - midpoint + k * squareXSize, NORMALREALMGROUNDHEIGHT + platformHeight * squareYSize, 0), Quaternion.identity);
				Instantiate (squareLayer, new Vector3 (runningXPos - midpoint + k * squareXSize, NORMALREALMDETECTIONHEIGHT + platformHeight * squareYSize, 0), Quaternion.identity);
			}
		}
		//spawns vertically moving platforms
		else if (platformType == 2) {
			for (int k = startingK; k < startingK + platformsWideToSpawn; k++) {
				Instantiate (leftRightSquare, new Vector3 (runningXPos - midpoint + k * squareXSize, NORMALREALMGROUNDHEIGHT + platformHeight * squareYSize, 0), Quaternion.identity);
				Instantiate (leftRightSquareLayer, new Vector3 (runningXPos - midpoint + k * squareXSize, NORMALREALMDETECTIONHEIGHT + platformHeight * squareYSize, 0), Quaternion.identity);
			}
		}
		//spawns horizontally moving platforms
		else if (platformType == 3) {
			for (int k = startingK; k < startingK + platformsWideToSpawn; k++) {
				Instantiate (upDownSquare, new Vector3 (runningXPos - midpoint + k * squareXSize, NORMALREALMGROUNDHEIGHT + platformHeight * squareYSize, 0), Quaternion.identity);
				Instantiate (upDownSquareLayer, new Vector3 (runningXPos - midpoint + k * squareXSize, NORMALREALMDETECTIONHEIGHT + platformHeight * squareYSize, 0), Quaternion.identity);
			}
		}
	}

	//spawns a portal if there are currently no other portals spawned
	void InstantiatePortal() {
		if (!(portalIsSpawned)) {
			int tmp = Random.Range (0, 2);
			if (tmp == 0) {
				//ensures that portals are only spawned on groups of blocks that are two blocks wide
				if (currentTrueInARow == 1 && trueInARow == 0) {
					Instantiate (portalGameObject, new Vector3 (runningXPos - squareXSize * 0.5f, NORMALREALMGROUNDHEIGHT + squareYSize * 1.5f , 0), Quaternion.identity);
					portalNumber++;
					portalIsSpawned = true;
					portal.SetPositionX (runningXPos - squareXSize / 2);
					portal.SetPositionY (-NORMALREALMGROUNDHEIGHT + squareYSize * 1.5f);
				}
			}
		}
	}

	void InstantiatePortalRealm() {
		///sets the location the portal will take the player
		runningPortalXPos = portal.m_positionX;
		runningPortalYPos = portal.m_positionY;
		runningPortPlatXPos = portal.m_positionX;
		runningPortPlatYPos = portal.m_positionY;
		InstantiatePortalRealmSpawningArea ();
		InstantiatePortalRealmPlatforms ();
	}

	//spawns the initial area for the dungeon
	void InstantiatePortalRealmSpawningArea() {
		for (int e = 0; e < 15; e++) {
			Instantiate (dungeonBlock, new Vector3 (runningPortalXPos - squareXSize * 9, PORTALREALMGROUNDHEIGHT + squareYSize * (e + 1), 0), Quaternion.identity);
		}

		for (int x = 0; x < 10; x++) {
			for (int y = 0; y < 30; y++) {
				Instantiate (dungeonBlockTwo, new Vector3 (runningPortalXPos - squareXSize * 10 - squareXSize * x, PORTALREALMGROUNDHEIGHT - squareYSize * y, 0), Quaternion.identity);
			}
		}

		for (int g = 0; g < 10; g++) {
			topLayer = true;
			for (int h = 0; h < 10; h++) {
				if (topLayer) {
					Instantiate (dungeonBlock, new Vector3 (runningPortalXPos - squareXSize * g, PORTALREALMGROUNDHEIGHT, 0), Quaternion.identity);
					Instantiate (squareLayer, new Vector3 (runningPortalXPos - squareXSize * g, PORTALREALMDETECTIONHEIGHT, 0), Quaternion.identity);
					topLayer = false;
				}
				else {
					Instantiate (dungeonBlockTwo, new Vector3 (runningPortalXPos - squareXSize * g, PORTALREALMGROUNDHEIGHT - h * squareYSize, 0), Quaternion.identity);
				}
			}
		}
		for (int i = 0; i < 10; i++) {
			topLayer = true;
			for (int j = 0; j < 10; j++) {
				if (topLayer) {
					Instantiate (dungeonBlock, new Vector3 (runningPortalXPos + squareXSize * i, PORTALREALMGROUNDHEIGHT, 0), Quaternion.identity);
					Instantiate (squareLayer, new Vector3 (runningPortalXPos + squareXSize * i, PORTALREALMDETECTIONHEIGHT, 0), Quaternion.identity);
					topLayer = false;
				}
				else {
					Instantiate (dungeonBlockTwo, new Vector3 (runningPortalXPos + squareXSize * i, PORTALREALMGROUNDHEIGHT - j * squareYSize, 0), Quaternion.identity);
				}
			}
		}
	}
	//spawns the dungeons platforms (well, not yet - but it should eventually)
	void InstantiatePortalRealmPlatforms() {
		//determines the size of the dungeon
		dungeonWidth = squareXSize * Random.Range (150, 251);
		dungeonHeight = 75 * squareXSize;
		//Instantiate (portalGameObject, new Vector3 (runningPortalXPos + dungeonWidth - 5 * squareXSize, -10.9f + dungeonHeight - 5 * squareYSize, 0), Quaternion.identity);

		//for (int g = 0, g < 1; g++) {
			
		//}
	
	}

	//spawns the row of black blocks that kill the player
	void InstantiateDeathBlocks(float startingX, float gapWidth, float gapHeight){
		for (int i = 0; i < gapWidth / squareLayerXSize; i++) {
			Instantiate(deathBlock, new Vector3(startingX - gapWidth + i * squareYSize, gapHeight, 0.0f), Quaternion.identity);
		}

	}
	void FindPlayer() 
	{
		//gets the player's location every 5 seconds
		playerX = characterController.GetPlayerPositionX(); 
	}
}
