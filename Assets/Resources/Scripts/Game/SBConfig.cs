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
	public const float DRINKER_MAX_VELOCITY = 800f;
	public const float DRINKER_ACCELERATION_CONSTANT = 800f;
	public const float DRINKER_MIN_FRAME_DURATION = 0.1f;
	public const float DRINKER_MAX_FRAME_DURATION = 0.4f;
	public const float TOP_UI_HEIGHT = 60f;
	public const float BORDER_WIDTH = 30f;
	public const float DRINK_WAIT_TIME = 4.0f;
	public const float DRINK_DRINK_TIME = 4.0f;
	public const float MAX_BLADDER_CAPACITY = 3.0f;	
	public const float BLADDER_FILL_CONSTANT = 1.0f / 4.0f; // takes denominator seconds to fill one drink in bladder
	
	public const KeyCode JOYSTICK_1_RIGHT = KeyCode.RightArrow;
	public const KeyCode JOYSTICK_1_LEFT = KeyCode.LeftArrow;
	public const KeyCode JOYSTICK_1_DOWN = KeyCode.DownArrow;
	public const KeyCode JOYSTICK_1_UP = KeyCode.UpArrow;
	
	public const KeyCode JOYSTICK_2_RIGHT = KeyCode.G;
	public const KeyCode JOYSTICK_2_LEFT = KeyCode.D;
	public const KeyCode JOYSTICK_2_UP = KeyCode.R;
	public const KeyCode JOYSTICK_2_DOWN = KeyCode.F;
}
