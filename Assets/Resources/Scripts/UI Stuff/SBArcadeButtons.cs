using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SBArcadeButtons : FContainer {
	List<FSprite> buttons;
	public int currentFlashingButton = -1;
	public bool isFlashing = true;
	bool currentButtonIsColored = false;
	public bool joystickIsRotating = false;
	float flashingButtonTimer = 0;
	float joystickTimer = 0;
	FSprite joystick;
	
	public SBArcadeButtons(bool withJoystick) {		
		buttons = new List<FSprite>();
		
		for (int i = 0; i < 7; i++) {
			FSprite button = new FSprite("button.psd");
			button.anchorX = 0;
			button.anchorY = 1;
			buttons.Add(button);
			AddChild(button);
		}
		
		float buttonWidth = buttons[0].width;
		float buttonMargin = 10f;
		float buttonShiftRight = 50f;
		float xJoystickOffset = 0;
		float yJoystickOffset = 0;
		
		if (withJoystick) {
			xJoystickOffset = 175f;
			yJoystickOffset = -100f;
			
			joystick = new FSprite("joystick.psd");
			joystick.x = joystick.width / 2f - 12f;
			joystick.y = -joystick.height / 2f;
			
			AddChild(joystick);
		}
		
		buttons[0].x = buttonShiftRight + xJoystickOffset;
		buttons[0].y = buttons[1].y = buttons[2].y = yJoystickOffset;
		buttons[1].x = buttonWidth + buttonMargin + buttonShiftRight + xJoystickOffset;
		buttons[2].x = (buttonWidth + buttonMargin) * 2f + buttonShiftRight + xJoystickOffset;
		buttons[3].x = buttonShiftRight + xJoystickOffset;
		buttons[3].y = -(buttonWidth + buttonMargin) + yJoystickOffset;
		buttons[4].y = -(buttonWidth + buttonMargin) + yJoystickOffset;
		buttons[5].y = -(buttonWidth + buttonMargin) + yJoystickOffset;
		buttons[4].x = buttonWidth + buttonMargin + buttonShiftRight + xJoystickOffset;
		buttons[5].x = (buttonWidth + buttonMargin) * 2f + buttonShiftRight + xJoystickOffset;
		buttons[6].x = xJoystickOffset;
		buttons[6].y = -(buttonWidth + buttonMargin) * 2f + 10f + yJoystickOffset;
	}
	
	override public void HandleAddedToStage() {
		base.HandleAddedToStage();
		Futile.instance.SignalUpdate += HandleUpdate;
	}
	
	override public void HandleRemovedFromStage() {
		base.HandleRemovedFromStage();
		Futile.instance.SignalUpdate -= HandleUpdate;
	}
	
	public void HandleUpdate() {
		if (!isFlashing) return;
		
		if (currentFlashingButton != -1 && currentFlashingButton < 7) {
			flashingButtonTimer += Time.fixedDeltaTime;
			if (flashingButtonTimer >= 0.25f) {
				flashingButtonTimer = 0;
				if (currentButtonIsColored) buttons[currentFlashingButton].color = Color.red;
				else buttons[currentFlashingButton].color = Color.white;
				currentButtonIsColored = !currentButtonIsColored;
			}
		}
		
		if (joystickIsRotating) {
			joystick.color = Color.red;
			joystickTimer += Time.fixedDeltaTime;
			if (joystickTimer >= 0.25f) {
				joystickTimer = 0;
				joystick.rotation += 90f;
			}
		}
		else joystick.color = Color.white;
	}
}
