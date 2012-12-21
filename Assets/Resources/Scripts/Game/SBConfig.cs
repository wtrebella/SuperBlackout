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
	public const float BLADDER_FILL_TIME = 4.0f; // per drink
	public const float PEE_TIME = 1.0f; // per drink
	public const int DRINKS_TO_WIN = 7;
		
	public const KeyCode JOYSTICK_1_RIGHT = KeyCode.G;
	public const KeyCode JOYSTICK_1_LEFT = KeyCode.D;
	public const KeyCode JOYSTICK_1_UP = KeyCode.R;
	public const KeyCode JOYSTICK_1_DOWN = KeyCode.F;
	
	public const KeyCode JOYSTICK_2_RIGHT = KeyCode.RightArrow;
	public const KeyCode JOYSTICK_2_LEFT = KeyCode.LeftArrow;
	public const KeyCode JOYSTICK_2_DOWN = KeyCode.DownArrow;
	public const KeyCode JOYSTICK_2_UP = KeyCode.UpArrow;
	
	public static Vector2 EmptyGlassPosition(int drinkNum, int playerNum) {
		float x1 = 28f;
		float y1 = 32f;
		
		float x2 = 73f;
		float y2 = 20f;
		
		float x3 = 12f;
		float y3 = 81f;
		
		float x4 = 12f;
		float y4 = 137f;
		
		float x5 = 120f;
		float y5 = 17f;
		
		float x6 = 15f;
		float y6 = 188f;
		
		float x7 = 180f;
		float y7 = 16f;
		
		if (playerNum == 1) {
			switch (drinkNum) {
			case 1:
				return new Vector2(x1, y1);	
			case 2:
				return new Vector2(x2, y2);
			case 3:
				return new Vector2(x3, y3);
			case 4:
				return new Vector2(x4, y4);
			case 5:
				return new Vector2(x5, y5);
			case 6:
				return new Vector2(x6, y6);
			case 7:
				return new Vector2(x7, y7);
			default:
				return Vector2.zero;
			}
		}
		
		else if (playerNum == 2) {
			switch (drinkNum) {
			case 1:
				return new Vector2(Futile.screen.width - x1, Futile.screen.height - TOP_UI_HEIGHT - y1);	
			case 2:
				return new Vector2(Futile.screen.width - x2, Futile.screen.height - TOP_UI_HEIGHT - y2);
			case 3:
				return new Vector2(Futile.screen.width - x3, Futile.screen.height - TOP_UI_HEIGHT - y3);
			case 4:
				return new Vector2(Futile.screen.width - x4, Futile.screen.height - TOP_UI_HEIGHT - y4);
			case 5:
				return new Vector2(Futile.screen.width - x5, Futile.screen.height - TOP_UI_HEIGHT - y5);
			case 6:
				return new Vector2(Futile.screen.width - x6, Futile.screen.height - TOP_UI_HEIGHT - y6);
			case 7:
				return new Vector2(Futile.screen.width - x7, Futile.screen.height - TOP_UI_HEIGHT - y7);
			default:
				return Vector2.zero;
			}
		}
		
		return new Vector2(0, 0);
	}
}
