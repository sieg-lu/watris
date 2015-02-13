using System;
using UnityEngine;

public class GameLogic : SingletonBase<GameLogic> {
	private int lengthOfSide;
	private int maxHeight;

	private bool[,,] blockExistedMark;
	private Vector3 stepVector;

	public void Initialize(int len, int height, Vector3 step) {
		this.lengthOfSide = len;
		this.maxHeight = height;
		blockExistedMark = new bool[len, height, len];
		for (int i = 0; i < len; i++) {
			for (int j = 0; j < height; j++) {
				for (int k = 0; k < len; k++) {
					blockExistedMark[i, j, k] = false;
				}
			}
		}
		stepVector = new Vector3(step.x, step.y, step.z);
	}

	private bool IsOneBlockFallable(Vector3 currentPosition) {
		// Debug.Log(currentPosition.x / stepVector.x + ", " + currentPosition.y / stepVector.y + ", " + currentPosition.z / stepVector.z);
		int iIndex = (int)(currentPosition.x / stepVector.x);
		int jIndex = (int)(currentPosition.y / stepVector.y);
		int kIndex = (int)(currentPosition.z / stepVector.z);
		// Debug.Log(iIndex + ", " + jIndex + ", " + kIndex);
		if (jIndex == maxHeight) {
			if (blockExistedMark[iIndex, jIndex - 1, kIndex] == false) {
				return true;
			}
			return false;
		}
		if (jIndex - 1 >= 0 && jIndex - 1 < maxHeight && blockExistedMark[iIndex, jIndex - 1, kIndex] == false) {
			return true;
		}
		// blockExistedMark[iIndex, jIndex, kIndex] = true;
		return false;
	}

	public bool IsFallable(Transform currentBlock) {
		foreach (Transform obj in currentBlock) {
			if (!IsOneBlockFallable(obj.position)) {
				return false;
			}
		}
		return true;
	}

	public void MarkBlock(Transform currentBlock) {
		foreach (Transform obj in currentBlock) {
			int jIndex = (int)(obj.position.y / stepVector.y);
			if (jIndex != maxHeight) {
				int iIndex = (int)(obj.position.x / stepVector.x);
				int kIndex = (int)(obj.position.z / stepVector.z);
				blockExistedMark[iIndex, jIndex, kIndex] = true;
			}
		}
	}

	private bool IsValidPositionOneBlock(Vector3 currentPosition) {
		if (currentPosition.x < 0.0f || currentPosition.z < 0.0f) {
			return false;
		}
		int jIndex = (int)(currentPosition.y / stepVector.y);
		if (jIndex == maxHeight) {
			// Debug.Log(jIndex);
			return true;
		}
		int iIndex = (int)(currentPosition.x / stepVector.x);
		int kIndex = (int)(currentPosition.z / stepVector.z);
		// Debug.Log(iIndex + ", " + jIndex + ", " + kIndex);
		return (iIndex >= 0 && iIndex < lengthOfSide && kIndex >= 0 && kIndex < lengthOfSide && 
		        blockExistedMark[iIndex, jIndex, kIndex] == false);
	}

	public bool IsValidPosition(Transform currentBlock) {
		foreach (Transform obj in currentBlock) {
			// Debug.Log(obj.position);
			if (!IsValidPositionOneBlock(obj.position)) {
				return false;
			}
		}
		return true;
	}
}

