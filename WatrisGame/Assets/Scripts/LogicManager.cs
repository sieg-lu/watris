using System;

public class LogicManager : SingletonBase<LogicManager> {

	private int lengthOfSide;
	private int maxHeight;

	public void SetSideLength(int len) {
		this.lengthOfSide = len;
	}

	public void SetMaxHeight(int height) {
		this.maxHeight = height;
	}
}

