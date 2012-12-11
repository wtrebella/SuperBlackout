using UnityEngine;
using System.Collections;

public enum Direction {
	Right,
	Left,
	Down,
	Up,
	None
}

public static class SBConfig {
	public const float BATHROOM_WIDTH = 248f;
	public const float TOP_UI_HEIGHT = 60f;
	public const float BORDER_WIDTH = 30f;
	
	public const KeyCode JOYSTICK_1_RIGHT = KeyCode.RightArrow;
	public const KeyCode JOYSTICK_1_LEFT = KeyCode.LeftArrow;
	public const KeyCode JOYSTICK_1_DOWN = KeyCode.DownArrow;
	public const KeyCode JOYSTICK_1_UP = KeyCode.UpArrow;
	
	public const KeyCode JOYSTICK_2_RIGHT = KeyCode.D;
	public const KeyCode JOYSTICK_2_LEFT = KeyCode.A;
	public const KeyCode JOYSTICK_2_DOWN = KeyCode.W;
	public const KeyCode JOYSTICK_2_UP = KeyCode.S;
}
