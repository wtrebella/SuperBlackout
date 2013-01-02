using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SBGameScene : FStage, FSingleTouchableInterface {
	public SBDrinker drinker1;
	public SBDrinker drinker2;
	public SBBarStool specialBarStool1;
	public SBBarStool specialBarStool2;
	public SBBar bar;
	public List<SBEntity> drinkers;
	public List<SBBarStool> specialBarStools;
	public bool isGameOver = false;
	public SBHudLayer hudLayer;
	public KeyCode lastKeyPressed1 = KeyCode.None;
	public KeyCode lastKeyPressed2 = KeyCode.None;
	
	//FLabel tempLogLabel;
	
	private static int frameCount_ = 0;
	
	public SBGameScene(bool addToFutileOnInit) : base("") {
		if (addToFutileOnInit) Futile.AddStage(this);
		
		SBBackgroundLayer backgroundLayer = new SBBackgroundLayer();
		AddChild(backgroundLayer);
			
		bar = new SBBar();
		bar.x = Futile.screen.halfWidth;
		bar.y = Futile.screen.halfHeight;
		AddChild(bar);
				
		drinkers = new List<SBEntity>();
		specialBarStools = new List<SBBarStool>();
		
		specialBarStool1 = new SBBarStool("special bar stool 1", new Color(0.3f, 0.5f, 1.0f, 1.0f));
		specialBarStool1.x = 100f;
		specialBarStool1.y = 100f;
		specialBarStool1.tag = 1;
		specialBarStool1.ProgressBarComponent().progressBar.isVisible = false;
		specialBarStool1.SittableComponent().isSpecial = true;
		specialBarStools.Add(specialBarStool1);
		AddChild(specialBarStool1);
		
		specialBarStool2 = new SBBarStool("special bar stool 2", new Color(1.0f, 0.3f, 0.5f, 1.0f));
		specialBarStool2.x = Futile.screen.width - 100f;
		specialBarStool2.y = Futile.screen.height - SBConfig.TOP_UI_HEIGHT - 100f;
		specialBarStool2.tag = 2;
		specialBarStool2.ProgressBarComponent().progressBar.isVisible = false;
		specialBarStool2.SittableComponent().isSpecial = true;
		specialBarStools.Add(specialBarStool2);
		AddChild(specialBarStool2);
		
		drinker1 = new SBDrinker("drinker 1");
		drinker1.tag = 1;
		drinker1.x = 100f;
		drinker1.y = Futile.screen.height - SBConfig.TOP_UI_HEIGHT - 100f;
		drinker1.ProgressBarComponent().progressBar.isVisible = false;
		drinker1.ProgressBarComponent().progressBar.y += 60f;
		drinker1.DirectionComponent().FaceDirection(Direction.Right, true);
		//drinker1.SpriteComponent().sprite.color = new Color(0.3f, 0.5f, 1.0f, 1.0f);
		drinkers.Add(drinker1);
		AddChild(drinker1);

		drinker2 = new SBDrinker("drinker 2");
		drinker2.tag = 2;
		drinker2.x = Futile.screen.width - 100f;
		drinker2.y = 100f;
		drinker2.ProgressBarComponent().progressBar.isVisible = false;
		drinker2.ProgressBarComponent().progressBar.y -= 60f;
		drinker2.DirectionComponent().FaceDirection(Direction.Left, true);
		//drinker2.SpriteComponent().sprite.color = new Color(1.0f, 0.3f, 0.5f, 1.0f);
		drinkers.Add(drinker2);
		AddChild(drinker2);
		
		hudLayer = new SBHudLayer();
		drinker1.SignalFinishedDrink += hudLayer.HandleDrinkerFinishedDrink;
		drinker2.SignalFinishedDrink += hudLayer.HandleDrinkerFinishedDrink;
		drinker1.SignalFinishedDrink += HandleDrinkerFinishedDrink;
		drinker2.SignalFinishedDrink += HandleDrinkerFinishedDrink;
		drinker1.SignalBladderChanged += hudLayer.HandleBladderChanged;
		drinker2.SignalBladderChanged += hudLayer.HandleBladderChanged;
		drinker1.SignalPissedHimself += HandleDrinkerPissedHimself;
		drinker2.SignalPissedHimself += HandleDrinkerPissedHimself;
		AddChild(hudLayer);
		
		/*tempLogLabel = new FLabel("Silkscreen", "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
		tempLogLabel.color = Color.black;
		tempLogLabel.x = 400f;
		tempLogLabel.y = Futile.screen.halfHeight;
		tempLogLabel.scale = 0.35f;
		AddChild(tempLogLabel);*/
	}
	
	public override void HandleAddedToStage() {
		base.HandleAddedToStage();
		Futile.instance.SignalUpdate += HandleUpdate;
		Futile.touchManager.AddSingleTouchTarget(this);
	}
	
	public override void HandleRemovedFromStage() {
		base.HandleRemovedFromStage();
		Futile.instance.SignalUpdate -= HandleUpdate;
		Futile.touchManager.AddSingleTouchTarget(this);
	}
	
	public bool KeyIsOnlyJoystickKeyBeingHeld(KeyCode keyCode, int player) {
		if (!Input.GetKey(keyCode)) return false;
		
		if (player == 1) {
			if (keyCode == SBConfig.JOYSTICK_1_DOWN) {
				return !Input.GetKey(SBConfig.JOYSTICK_1_UP) &&
					!Input.GetKey(SBConfig.JOYSTICK_1_RIGHT) &&	
					!Input.GetKey(SBConfig.JOYSTICK_1_LEFT);
			}
			
			else if (keyCode == SBConfig.JOYSTICK_1_UP) {
				return !Input.GetKey(SBConfig.JOYSTICK_1_RIGHT) &&	
					!Input.GetKey(SBConfig.JOYSTICK_1_DOWN) &&
					!Input.GetKey(SBConfig.JOYSTICK_1_LEFT);
			}
			
			else if (keyCode == SBConfig.JOYSTICK_1_RIGHT) {
				return !Input.GetKey(SBConfig.JOYSTICK_1_UP) &&
					!Input.GetKey(SBConfig.JOYSTICK_1_DOWN) &&
					!Input.GetKey(SBConfig.JOYSTICK_1_LEFT);
			}
			
			else if (keyCode == SBConfig.JOYSTICK_1_LEFT) {
				return !Input.GetKey(SBConfig.JOYSTICK_1_UP) &&
					!Input.GetKey(SBConfig.JOYSTICK_1_RIGHT) &&	
					!Input.GetKey(SBConfig.JOYSTICK_1_DOWN);
			}
			
			else return false;
		}
		
		else if (player == 2) {
			if (keyCode == SBConfig.JOYSTICK_2_DOWN) {
				return !Input.GetKey(SBConfig.JOYSTICK_2_UP) &&
					!Input.GetKey(SBConfig.JOYSTICK_2_RIGHT) &&	
					!Input.GetKey(SBConfig.JOYSTICK_2_LEFT);
			}
			
			else if (keyCode == SBConfig.JOYSTICK_2_UP) {
				return !Input.GetKey(SBConfig.JOYSTICK_2_RIGHT) &&	
					!Input.GetKey(SBConfig.JOYSTICK_2_DOWN) &&
					!Input.GetKey(SBConfig.JOYSTICK_2_LEFT);
			}
			
			else if (keyCode == SBConfig.JOYSTICK_2_RIGHT) {
				return !Input.GetKey(SBConfig.JOYSTICK_2_UP) &&
					!Input.GetKey(SBConfig.JOYSTICK_2_DOWN) &&
					!Input.GetKey(SBConfig.JOYSTICK_2_LEFT);
			}
			
			else if (keyCode == SBConfig.JOYSTICK_2_LEFT) {
				return !Input.GetKey(SBConfig.JOYSTICK_2_UP) &&
					!Input.GetKey(SBConfig.JOYSTICK_2_RIGHT) &&	
					!Input.GetKey(SBConfig.JOYSTICK_2_DOWN);
			}
			
			else return false;
		}
		
		return false;
	}
	
	public void HandleKeyInput() {
		if (Input.GetKeyDown(SBConfig.JOYSTICK_1_DOWN) || KeyIsOnlyJoystickKeyBeingHeld(SBConfig.JOYSTICK_1_DOWN, 1)) lastKeyPressed1 = SBConfig.JOYSTICK_1_DOWN;
		else if (Input.GetKeyDown(SBConfig.JOYSTICK_1_UP) || KeyIsOnlyJoystickKeyBeingHeld(SBConfig.JOYSTICK_1_UP, 1)) lastKeyPressed1 = SBConfig.JOYSTICK_1_UP;
		else if (Input.GetKeyDown(SBConfig.JOYSTICK_1_RIGHT) || KeyIsOnlyJoystickKeyBeingHeld(SBConfig.JOYSTICK_1_RIGHT, 1)) lastKeyPressed1 = SBConfig.JOYSTICK_1_RIGHT;
		else if (Input.GetKeyDown(SBConfig.JOYSTICK_1_LEFT) || KeyIsOnlyJoystickKeyBeingHeld(SBConfig.JOYSTICK_1_LEFT, 1)) lastKeyPressed1 = SBConfig.JOYSTICK_1_LEFT;
				
		if (Input.GetKeyDown(SBConfig.JOYSTICK_2_DOWN) || KeyIsOnlyJoystickKeyBeingHeld(SBConfig.JOYSTICK_2_DOWN, 2)) lastKeyPressed2 = SBConfig.JOYSTICK_2_DOWN;
		else if (Input.GetKeyDown(SBConfig.JOYSTICK_2_UP) || KeyIsOnlyJoystickKeyBeingHeld(SBConfig.JOYSTICK_2_UP, 2)) lastKeyPressed2 = SBConfig.JOYSTICK_2_UP;
		else if (Input.GetKeyDown(SBConfig.JOYSTICK_2_RIGHT) || KeyIsOnlyJoystickKeyBeingHeld(SBConfig.JOYSTICK_2_RIGHT, 2)) lastKeyPressed2 = SBConfig.JOYSTICK_2_RIGHT;
		else if (Input.GetKeyDown(SBConfig.JOYSTICK_2_LEFT) || KeyIsOnlyJoystickKeyBeingHeld(SBConfig.JOYSTICK_2_LEFT, 2)) lastKeyPressed2 = SBConfig.JOYSTICK_2_LEFT;
		
		if (Input.GetKey(SBConfig.JOYSTICK_1_DOWN) && lastKeyPressed1 == SBConfig.JOYSTICK_1_DOWN) {
			if (!drinker1.isActuallySitting) {
				drinker1.VelocityComponent().accelerationDirection = Direction.Down;
				drinker1.DirectionComponent().FaceDirection(Direction.Down);
			}
		}
		
		else if (Input.GetKey(SBConfig.JOYSTICK_1_UP) && lastKeyPressed1 == SBConfig.JOYSTICK_1_UP) {
			if (!drinker1.isActuallySitting) {
				drinker1.VelocityComponent().accelerationDirection = Direction.Up;
				drinker1.DirectionComponent().FaceDirection(Direction.Up);
			}
		}
		
		else if (Input.GetKey(SBConfig.JOYSTICK_1_RIGHT) && lastKeyPressed1 == SBConfig.JOYSTICK_1_RIGHT) {
			if (drinker1.isActuallySitting) {
				if (!drinker1.isReceivingDrink) {
					drinker1.RotateInChair(650 * Time.fixedDeltaTime);
					drinker1.currentSittableComponent.owner.rotatingContainer.rotation += 650 * Time.fixedDeltaTime;
				}
			}
			else {
				drinker1.VelocityComponent().accelerationDirection = Direction.Right;
				drinker1.DirectionComponent().FaceDirection(Direction.Right);
			}
		}
		
		else if (Input.GetKey(SBConfig.JOYSTICK_1_LEFT) && lastKeyPressed1 == SBConfig.JOYSTICK_1_LEFT) {
			if (drinker1.isActuallySitting) {
				if (!drinker1.isReceivingDrink) {
					drinker1.RotateInChair(-650 * Time.fixedDeltaTime);
					drinker1.currentSittableComponent.owner.rotatingContainer.rotation -= 650 * Time.fixedDeltaTime;
				}
			}
			else {
				drinker1.VelocityComponent().accelerationDirection = Direction.Left;
				drinker1.DirectionComponent().FaceDirection(Direction.Left);
			}
		}
		else drinker1.VelocityComponent().accelerationDirection = Direction.None;
		
		if (Input.GetKey(SBConfig.JOYSTICK_2_DOWN) && lastKeyPressed2 == SBConfig.JOYSTICK_2_DOWN) {
			if (!drinker2.isActuallySitting) {
				drinker2.VelocityComponent().accelerationDirection = Direction.Down;
				drinker2.DirectionComponent().FaceDirection(Direction.Down);
			}
		}
		else if (Input.GetKey(SBConfig.JOYSTICK_2_UP) && lastKeyPressed2 == SBConfig.JOYSTICK_2_UP) {
			if (!drinker2.isActuallySitting) {
				drinker2.VelocityComponent().accelerationDirection = Direction.Up;
				drinker2.DirectionComponent().FaceDirection(Direction.Up);
			}
		}
		else if (Input.GetKey(SBConfig.JOYSTICK_2_RIGHT) && lastKeyPressed2 == SBConfig.JOYSTICK_2_RIGHT) {
			if (drinker2.isActuallySitting) {
				if (!drinker2.isReceivingDrink) {
					drinker2.RotateInChair(650 * Time.fixedDeltaTime);
					drinker2.currentSittableComponent.owner.rotatingContainer.rotation += 650 * Time.fixedDeltaTime;
				}
			}
			else {
				drinker2.VelocityComponent().accelerationDirection = Direction.Right;
				drinker2.DirectionComponent().FaceDirection(Direction.Right);
			}
		}
		else if (Input.GetKey(SBConfig.JOYSTICK_2_LEFT) && lastKeyPressed2 == SBConfig.JOYSTICK_2_LEFT) {
			if (drinker2.isActuallySitting) {
				if (!drinker2.isReceivingDrink) {
					drinker2.RotateInChair(-650 * Time.fixedDeltaTime);
					drinker2.currentSittableComponent.owner.rotatingContainer.rotation -= 650 * Time.fixedDeltaTime;
				}
			}
			else {
				drinker2.VelocityComponent().accelerationDirection = Direction.Left;
				drinker2.DirectionComponent().FaceDirection(Direction.Left);
			}
		}
		else drinker2.VelocityComponent().accelerationDirection = Direction.None;
	}
	
	public void UpdateDrinkerPositions() {
		foreach (SBDrinker drinker in drinkers) {
			if (drinker.isBeingControlledBySittableComponent || drinker.isInBathroom || drinker.isLeavingBathroom) continue;
			float newX = drinker.x + Time.fixedDeltaTime * drinker.VelocityComponent().xVelocity;
			float newY = drinker.y + Time.fixedDeltaTime * drinker.VelocityComponent().yVelocity;
			float bathroomPadding = 30f;
			float borderPadding = -30f;
			
			Rect leftWallRect = new Rect(0, 0, SBConfig.BORDER_WIDTH + borderPadding, Futile.screen.height);
			Rect rightWallRect = new Rect(Futile.screen.width - SBConfig.BORDER_WIDTH - borderPadding, 0, SBConfig.BORDER_WIDTH + borderPadding, Futile.screen.height);
			Rect bottomLeftWallRect = new Rect(0, -300, Futile.screen.halfWidth - SBConfig.BATHROOM_WIDTH / 2f - bathroomPadding, SBConfig.BORDER_WIDTH + borderPadding + 300);
			Rect bottomRightWallRect = new Rect(Futile.screen.halfWidth + SBConfig.BATHROOM_WIDTH / 2f + bathroomPadding, -300, Futile.screen.halfWidth - SBConfig.BATHROOM_WIDTH / 2f - bathroomPadding, SBConfig.BORDER_WIDTH + borderPadding + 300);
			Rect topLeftWallRect = new Rect(0, Futile.screen.height - SBConfig.TOP_UI_HEIGHT - borderPadding, Futile.screen.halfWidth - SBConfig.BATHROOM_WIDTH / 2f - bathroomPadding, SBConfig.BORDER_WIDTH + borderPadding + 300);
			Rect topRightWallRect = new Rect(Futile.screen.halfWidth + SBConfig.BATHROOM_WIDTH / 2f + bathroomPadding, Futile.screen.height - SBConfig.TOP_UI_HEIGHT - borderPadding, Futile.screen.halfWidth - SBConfig.BATHROOM_WIDTH / 2f - bathroomPadding, SBConfig.BORDER_WIDTH + borderPadding + 300);
			
			Vector2 updatedPoint;
			updatedPoint = bar.CollideComponent().PointToAvoidCollidingFixedRectWithMovingEntity(leftWallRect, drinker, newX, newY);
			updatedPoint = bar.CollideComponent().PointToAvoidCollidingFixedRectWithMovingEntity(rightWallRect, drinker, updatedPoint.x, updatedPoint.y);
			updatedPoint = bar.CollideComponent().PointToAvoidCollidingFixedRectWithMovingEntity(bottomLeftWallRect, drinker, updatedPoint.x, updatedPoint.y);
			updatedPoint = bar.CollideComponent().PointToAvoidCollidingFixedRectWithMovingEntity(bottomRightWallRect, drinker, updatedPoint.x, updatedPoint.y);
			updatedPoint = bar.CollideComponent().PointToAvoidCollidingFixedRectWithMovingEntity(topLeftWallRect, drinker, updatedPoint.x, updatedPoint.y);
			updatedPoint = bar.CollideComponent().PointToAvoidCollidingFixedRectWithMovingEntity(topRightWallRect, drinker, updatedPoint.x, updatedPoint.y);
			updatedPoint = bar.CollideComponent().PointToAvoidCollidingFixedRectWithMovingEntity(bar.SpriteComponent(0).GetGlobalRect().CloneWithExpansion(-10f), drinker, updatedPoint.x, updatedPoint.y);
			
			drinker.x = updatedPoint.x;
			drinker.y = updatedPoint.y;
		}
		
		if (drinker1.SpriteComponent(1).GetGlobalRectWithExpansion(-25).CheckIntersect(drinker2.SpriteComponent(1).GetGlobalRectWithExpansion(-25))) {
			if (Input.GetKeyDown(SBConfig.ATTACK_BUTTON_1)) {
				if (drinker2.HasDrink()) {
					drinker2.currentDrink.RemoveFromContainer();
					drinker2.currentDrink = null;
				}
			}
			
			if (Input.GetKeyDown(SBConfig.ATTACK_BUTTON_2)) {
				if (drinker1.HasDrink()) {
					drinker1.currentDrink.RemoveFromContainer();
					drinker1.currentDrink = null;
				}
			}
			
			/*drinker1.SpriteComponent(1).sprite.color = Color.red;
			drinker2.SpriteComponent(1).sprite.color = Color.blue;*/
		}
	}
	
	public void UpdateDrinkerBarstoolRelations() {
		foreach (SBDrinker drinker in drinkers) {			
			if (drinker.HasDrink()) {
				foreach (SBBarStool specialBarStool in specialBarStools) {
					if (!specialBarStool.GetGlobalSitTriggerRect().CheckIntersect(drinker.SpriteComponent(1).GetGlobalRect()) ||
						!specialBarStool.SittableComponent().CanSeatDrinker(drinker)) continue;
						
					specialBarStool.SittableComponent().SeatDrinker(drinker);
				}	
			}
			else {
				SBBarStool barStool = bar.BarStoolThatIntersectsWithGlobalRect(drinker.SpriteComponent(1).GetGlobalRect());
				if (barStool == null || !barStool.SittableComponent().CanSeatDrinker(drinker)) continue;
				barStool.SittableComponent().SeatDrinker(drinker);
			}
		}
	}
	
	public void UpdateDrinkerBathroomRelations() {
		foreach (SBDrinker drinker in drinkers) {
			if (drinker.isInBathroom) {
				if (drinker.drinkAmountInBladder <= 0) {
					drinker.isInBathroom = false;
					drinker.isLeavingBathroom = true;
					if (drinker.y < Futile.screen.halfHeight) {
						Go.to(drinker, 0.5f, new TweenConfig()
							.floatProp("x", Futile.screen.halfWidth)
							.floatProp("y", 100f)
							.onComplete(SBDrinker.HandleDoneLeavingBathroom));
					}
					else {
						Go.to(drinker, 0.5f, new TweenConfig()
							.floatProp("x", Futile.screen.halfWidth)
							.floatProp("y", Futile.screen.height - SBConfig.TOP_UI_HEIGHT - 100f)
							.onComplete(SBDrinker.HandleDoneLeavingBathroom));
					}
				}
			}
			else if (!drinker.isLeavingBathroom) {
				if (drinker.y < -drinker.SpriteComponent(1).sprite.height / 2f) drinker.isInBathroom = true;
				if (drinker.y > Futile.screen.height - SBConfig.TOP_UI_HEIGHT + drinker.SpriteComponent(1).sprite.height / 2f) drinker.isInBathroom = true;
			}
		}
	}
	
	public void HandleDrinkerPissedHimself(SBDrinker drinker) {
		isGameOver = true;
		FLabel label = new FLabel("Silkscreen", string.Format(drinker.name + " pissed himself!"));
		label.color = Color.black;
		label.x = Futile.screen.halfWidth;
		label.y = Futile.screen.height * 0.75f;
		AddChild(label);;
	}
	
	public void HandleDrinkerFinishedDrink(SBDrinker drinker) {
		if (drinker.drinkCount >= SBConfig.DRINKS_TO_WIN) {
			isGameOver = true;
			FLabel label = new FLabel("Silkscreen", string.Format(drinker.name + " blacked out!"));
			label.color = Color.black;
			label.x = Futile.screen.halfWidth;
			label.y = Futile.screen.height * 0.75f;
			AddChild(label);
		}
	}
	
	public void HandleUpdate() {
		/*bool debugPressed = false;
		if (Input.GetKeyDown(KeyCode.Alpha0)) {
			drinker2.VelocityComponent().debugDrinkCount = 0;
			debugPressed = true;
		}
		if (Input.GetKeyDown(KeyCode.Alpha1)) {
			drinker2.VelocityComponent().debugDrinkCount = 1;
			debugPressed = true;
		}
		if (Input.GetKeyDown(KeyCode.Alpha2)) {
			drinker2.VelocityComponent().debugDrinkCount = 2;
			debugPressed = true;
		}
		if (Input.GetKeyDown(KeyCode.Alpha3)) {
			drinker2.VelocityComponent().debugDrinkCount = 3;
			debugPressed = true;
		}
		if (Input.GetKeyDown(KeyCode.Alpha4)) {
			drinker2.VelocityComponent().debugDrinkCount = 4;
			debugPressed = true;
		}
		if (Input.GetKeyDown(KeyCode.Alpha5)) {
			drinker2.VelocityComponent().debugDrinkCount = 5;
			debugPressed = true;
		}
		if (Input.GetKeyDown(KeyCode.Alpha6)) {
			drinker2.VelocityComponent().debugDrinkCount = 6;
			debugPressed = true;
		}
		
		if (debugPressed) {
			drinker2.VelocityComponent().RefreshDrunkLeanValues();
			tempLogLabel.text = string.Format("maxDrunkLeanVelocity: " + drinker2.VelocityComponent().maxDrunkLeanVelocity + "\n" +
				"drunkLeanVelocityAdder: " + drinker2.VelocityComponent().drunkLeanVelocityAdder + "\n" +
				"likelihoodOfDrunkLeanVariation: " + drinker2.VelocityComponent().likelihoodOfDrunkLeanVariation + "\n" +
				"likelihoodOfDrunkStall: " + drinker2.VelocityComponent().likelihoodOfDrunkStall + "\n" +
				"strengthOfDrunkStall: " + drinker2.VelocityComponent().strengthOfDrunkStall + "\n" +
				"slowDownEffect: " + drinker2.VelocityComponent().slowDownEffect + "\n");
		}*/
		
		if (frameCount_++ < 5) return;
		
		if (drinker2.SpriteComponent(0).isAnimating) Debug.Log(drinker2.SpriteComponent(0).currentAnimation.animationTimer);
		
		if (!isGameOver) {
			HandleKeyInput();
			UpdateDrinkerPositions();
			UpdateDrinkerBathroomRelations();
			UpdateDrinkerBarstoolRelations();
			bar.HandleUpdate();
		}
		drinker1.HandleUpdate();
		drinker2.HandleUpdate();
				
		foreach (SBDrinker drinker in drinkers) {
			if (drinker.HasDrink() && !drinker.isDrinking && drinker.isActuallySitting && drinker.currentSittableComponent.isSpecial) {
				drinker.StartDrinkingDrink();
			}
		}
	}
	
	public bool HandleSingleTouchBegan(FTouch touch) {
		return true;
	}
	
	public void HandleSingleTouchMoved(FTouch touch) {
		
	}
	
	public void HandleSingleTouchEnded(FTouch touch) {
		
	}
	
	public void HandleSingleTouchCanceled(FTouch touch) {
		
	}
}
