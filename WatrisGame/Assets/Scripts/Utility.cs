using System;
using UnityEngine;

public class Utility {
	public static void Assert(bool aCondition, string aMessage) {
		if (!aCondition) {
			Debug.LogError(aMessage);
		}
	}

	public static Boolean FltEqual(float x, float y) {
		return Math.Abs(x - y) < 1e-6;
	}

	public static Boolean FltEqual(float x, float y, float eps) {
		return Math.Abs(x - y) < eps;
	}
}

