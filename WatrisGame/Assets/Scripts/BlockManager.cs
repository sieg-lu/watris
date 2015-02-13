using UnityEngine;
using System.Collections;

public class BlockManager : MonoBehaviour {
	public GameObject[] blockTypes;

	void Start () {
		Random.seed = (int)System.DateTime.Now.Ticks;
	}

	void Update () {
	
	}

	public GameObject GetNextBlock(Vector3 position) {
		GameObject block = (GameObject)Instantiate(blockTypes[Random.Range(0, blockTypes.Length)], position, Quaternion.identity);
		return block;
	}
}
