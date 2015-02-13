using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour {
	public Camera mainCamera;
	public GameObject unitBasePrefab;
	public GameObject unitBlockPrefab;
	// public GameObject testBlockPrefab;
	public BlockManager blockManager;
	public int lengthOfSideInUnit = 5;
	public int maxHeightInUnit = 10;
	public float fallingTimeIntervalMilli = 1000.0f;

	private GameObject currentBlock = null;
	private Vector3 blockSpawnPosition;
	private Vector3 xStepVector;
	private Vector3 yStepVector;
	private Vector3 zStepVector;

	private bool isKeyDown;

	void Start() {
		blockSpawnPosition = new Vector3(
			((float)(lengthOfSideInUnit >> 1) + 0.5f) * unitBlockPrefab.transform.localScale.x,
			((float)maxHeightInUnit + 0.5f) * unitBlockPrefab.transform.localScale.y,
			((float)(lengthOfSideInUnit >> 1) + 0.5f) * unitBlockPrefab.transform.localScale.z);
		xStepVector = new Vector3(unitBlockPrefab.transform.localScale.x, 0, 0);
		yStepVector = new Vector3(0, unitBlockPrefab.transform.localScale.y, 0);
		zStepVector = new Vector3(0, 0, unitBlockPrefab.transform.localScale.z);
		isKeyDown = false;
		GameLogic.Instance.Initialize(lengthOfSideInUnit, maxHeightInUnit, unitBlockPrefab.transform.localScale);

		Utility.Assert(Utility.FltEqual(unitBasePrefab.transform.localScale.x, unitBasePrefab.transform.localScale.z), "Base is not a SQUARE");
		InitCameraPosition();
		SetBaseFloor();
		if (SpawnFallingBlock()) {
			StartCoroutine("TimerTickEvent");
		} else {
			Debug.LogError("Game Over");
		}
	}

	void Update() {
		if (currentBlock != null) {
			if (Input.GetKeyUp(KeyCode.E)) {
				currentBlock.transform.Rotate(new Vector3(0, 90, 0));
				if (!GameLogic.Instance.IsValidPosition(currentBlock.transform)) {
					currentBlock.transform.Rotate(new Vector3(0, -90, 0));
				}
			}
			if (Input.GetKeyUp(KeyCode.W)) {
				TryMove(zStepVector);
			}
			if (Input.GetKeyUp(KeyCode.S)) {
				TryMove(-zStepVector);
			}
			if (Input.GetKeyUp(KeyCode.A)) {
				TryMove(-xStepVector);
			}
			if (Input.GetKeyUp(KeyCode.D)) {
				TryMove(xStepVector);
			}
			if (Input.GetKey(KeyCode.Space)) {
				currentBlock.transform.position -= yStepVector;
				if (!GameLogic.Instance.IsFallable(currentBlock.transform)) {
					currentBlock.transform.position += yStepVector;
				}
			}
		}
	}

	protected bool TryMove(Vector3 step) {
		currentBlock.transform.position += step;
		if (!GameLogic.Instance.IsValidPosition(currentBlock.transform)) {
			currentBlock.transform.position -= step;
			return false;
		}
		return true;
	}

	protected IEnumerator TimerTickEvent() {
		while (true) {
			yield return new WaitForSeconds(fallingTimeIntervalMilli / 1000.0f);
			// Debug.Log("Tick");
			// do things
			if (currentBlock == null) {
				if (!SpawnFallingBlock()) {
					Debug.LogError("Game Over");
					StopCoroutine("TimerTickEvent");
				}
			}
			currentBlock.transform.position -= yStepVector;
			if (!GameLogic.Instance.IsFallable(currentBlock.transform)) {
				GameLogic.Instance.MarkBlock(currentBlock.transform);
				currentBlock = null;
			}
		}
	}

	protected bool SpawnFallingBlock() {
		// currentBlock = (GameObject)Instantiate(testBlockPrefab, blockSpawnPosition, Quaternion.identity);
		currentBlock = blockManager.GetNextBlock(blockSpawnPosition);
		return GameLogic.Instance.IsFallable(currentBlock.transform);
	}

	protected void InitCameraPosition() {
		float unitBasePrefabLen = unitBasePrefab.transform.localScale.x;
		float halfUnitBasePrefabLen = unitBasePrefabLen / 2.0f;
		Vector3 tmp = new Vector3(
			(float)lengthOfSideInUnit * halfUnitBasePrefabLen,
			(float)maxHeightInUnit * 1.5f,
			-(float)lengthOfSideInUnit * halfUnitBasePrefabLen * 2.0f);
		mainCamera.gameObject.transform.position = tmp;
		mainCamera.gameObject.transform.LookAt(
			new Vector3(mainCamera.gameObject.transform.position.x, 
		    (float)maxHeightInUnit / 2.5f, 
		    -mainCamera.gameObject.transform.position.z));
	}

	protected void SetBaseFloor() {
		GameObject floor = new GameObject("Floor");
		float unitBasePrefabLen = unitBasePrefab.transform.localScale.x;
		float xPos = unitBasePrefabLen / 2.0f;
		float yPos = -unitBasePrefab.transform.localScale.y / 2.0f - 0.02f;
		for (int i = 0; i < lengthOfSideInUnit; i++, xPos += unitBasePrefabLen) {
			float zPos = unitBasePrefabLen / 2.0f;
			for (int j = 0; j < lengthOfSideInUnit; j++, zPos += unitBasePrefabLen) {
				GameObject clone = (GameObject)Instantiate(unitBasePrefab, new Vector3(xPos, yPos, zPos), Quaternion.identity);
				clone.transform.parent = floor.transform;
			}
		}
	}
}
