using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour {
	public Camera mainCamera;
	public GameObject unitBasePrefab;
	public int lengthOfSideInUnit = 5;
	public int maxHeightInUnit = 7;

	void Start() {
		LogicManager.Instance.SetSideLength(lengthOfSideInUnit);
		LogicManager.Instance.SetMaxHeight(maxHeightInUnit);

		Utility.Assert(Utility.FltEqual(unitBasePrefab.transform.localScale.x, unitBasePrefab.transform.localScale.z), "Base is not a SQUARE");
		InitCameraPosition();
		SetBaseFloor();
	}

	void Update() {
	
	}

	protected void InitCameraPosition() {
		float unitBasePrefabLen = unitBasePrefab.transform.localScale.x;
		float halfUnitBasePrefabLen = unitBasePrefabLen / 2.0f;
		Vector3 tmp = new Vector3(
			(float)lengthOfSideInUnit * halfUnitBasePrefabLen,
			(float)maxHeightInUnit * 1.5f,
			-(float)lengthOfSideInUnit * halfUnitBasePrefabLen);
		mainCamera.gameObject.transform.position = tmp;
		mainCamera.gameObject.transform.LookAt(
			new Vector3(mainCamera.gameObject.transform.position.x, 
		    (float)maxHeightInUnit / 2.0f, 
		    -mainCamera.gameObject.transform.position.z));
	}

	protected void SetBaseFloor() {
		GameObject floor = new GameObject("Floor");
		float unitBasePrefabLen = unitBasePrefab.transform.localScale.x;
		float xPos = unitBasePrefabLen / 2.0f;
		float yPos = -unitBasePrefabLen / 2.0f - 0.02f;
		for (int i = 0; i < lengthOfSideInUnit; i++, xPos += unitBasePrefabLen) {
			float zPos = unitBasePrefabLen / 2.0f;
			for (int j = 0; j < lengthOfSideInUnit; j++, zPos += unitBasePrefabLen) {
				GameObject clone = (GameObject)Instantiate(unitBasePrefab, new Vector3(xPos, yPos, zPos), Quaternion.identity);
				clone.transform.parent = floor.transform;
			}
		}
	}
}
