using System;
using UnityEngine;

public class Utility {
	public static void Assert(bool aCondition, string aMessage) {
		if (!aCondition) {
			Debug.LogError(aMessage);
		}
	}

	public static Boolean FltEqual(float x, float y) {
		return FltEqual(x, y, (float)1e-6);
	}

	public static Boolean FltEqual(float x, float y, float eps) {
		return Math.Abs(x - y) < eps;
	}

	public static int EncodeIndex(int i, int j, int k) {
		int res = 0;
		res |= ((i & 0x3ff) << 20);
		res |= ((j & 0x3ff) << 10);
		res |= (k & 0x3ff);
		return res;
	}

	// 0b 0011 1111 1111 0000 0000 0000 0000 0000
	public static int DecodeIndexI(int code) {
		return ((code & 0x3ff00000) >> 20);
	}

	// 0b 0000 0000 0000 1111 1111 1100 0000 0000
	public static int DecodeIndexJ(int code) {
		return ((code & 0xffc00) >> 10);
	}

	// 0b 0000 0000 0000 0000 0000 0011 1111 1111
	public static int DecodeIndexK(int code) {
		return (code & 0x3ff);
	}
}

