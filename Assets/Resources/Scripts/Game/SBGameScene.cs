using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SBGameScene : FStage {
	public SBDrinker drinker1;
	public SBDrinker drinker2;
	public SBBar bar;
	public List<SBEntity> drinkers;
	
	public SBGameScene(bool addToFutileOnInit) : base("") {
		if (addToFutileOnInit) Futile.AddStage(this);
		
		SBBackgroundLayer backgroundLayer = new SBBackgroundLayer();
		AddChild(backgroundLayer);
			
		bar = new SBBar();
		bar.x = Futile.screen.halfWidth;
		bar.y = Futile.screen.halfHeight;
		bar.UpdateMatrix();
		AddChild(bar);
		
		drinkers = new List<SBEntity>();
		
		drinker1 = new SBDrinker("drinker1");
		drinker1.tag = 1;
		drinker1.x = 100f;
		drinker1.y = 100f;
		drinker1.ProgressBarComponent().progressBar.isVisible = false;
		drinker1.DirectionComponent().FaceDirection(Direction.Right, true);
		//drinker1.SpriteComponent().sprite.color = new Color(0.3f, 0.5f, 1.0f, 1.0f);
		drinkers.Add(drinker1);
		AddChild(drinker1);

		drinker2 = new SBDrinker("drinker2");
		drinker2.tag = 2;
		drinker2.x = Futile.screen.width - 100f;
		drinker2.y = Futile.screen.height - SBConfig.TOP_UI_HEIGHT - 100f;
		drinker2.ProgressBarComponent().progressBar.isVisible = false;
		drinker2.DirectionComponent().FaceDirection(Direction.Left, true);
		//drinker2.SpriteComponent().sprite.color = new Color(1.0f, 0.3f, 0.5f, 1.0f);
		drinkers.Add(drinker2);
		AddChild(drinker2);
	}
	
	public override void HandleAddedToStage() {
		base.HandleAddedToStage();
		Futile.instance.SignalUpdate += HandleUpdate;
	}
	
	public override void HandleRemovedFromStage() {
		base.HandleRemovedFromStage();
		Futile.instance.SignalUpdate -= HandleUpdate;
	}
	
	public void HandleKeyInput() {
		if (Input.GetKey(SBConfig.JOYSTICK_1_DOWN)) {
			drinker1.VelocityComponent().accelerationDirection = Direction.Down;
			drinker1.DirectionComponent().FaceDirection(Direction.Down);
		}
		else if (Input.GetKey(SBConfig.JOYSTICK_1_UP)) {
			drinker1.VelocityComponent().accelerationDirection = Direction.Up;
			drinker1.DirectionComponent().FaceDirection(Direction.Up);
		}
		else if (Input.GetKey(SBConfig.JOYSTICK_1_RIGHT)) {
			drinker1.VelocityComponent().accelerationDirection = Direction.Right;
			drinker1.DirectionComponent().FaceDirection(Direction.Right);
		}
		else if (Input.GetKey(SBConfig.JOYSTICK_1_LEFT)) {
			drinker1.VelocityComponent().accelerationDirection = Direction.Left;
			drinker1.DirectionComponent().FaceDirection(Direction.Left);
		}
		else drinker1.VelocityComponent().accelerationDirection = Direction.None;
		
		if (Input.GetKey(SBConfig.JOYSTICK_2_DOWN)) {
			drinker2.VelocityComponent().accelerationDirection = Direction.Down;
			drinker2.DirectionComponent().FaceDirection(Direction.Down);
		}
		else if (Input.GetKey(SBConfig.JOYSTICK_2_UP)) {
			drinker2.VelocityComponent().accelerationDirection = Direction.Up;
			drinker2.DirectionComponent().FaceDirection(Direction.Up);
		}
		else if (Input.GetKey(SBConfig.JOYSTICK_2_RIGHT)) {
			drinker2.VelocityComponent().accelerationDirection = Direction.Right;
			drinker2.DirectionComponent().FaceDirection(Direction.Right);
		}
		else if (Input.GetKey(SBConfig.JOYSTICK_2_LEFT)) {
			drinker2.VelocityComponent().accelerationDirection = Direction.Left;
			drinker2.DirectionComponent().FaceDirection(Direction.Left);
		}
		else drinker2.VelocityComponent().accelerationDirection = Direction.None;
	}
	
	public void UpdateDrinkerPositions() {
		foreach (SBDrinker drinker in drinkers) {
			if (drinker.isBeingControlledBySittableComponent) continue;
			
			float newX = drinker.x + Time.fixedDeltaTime * drinker.VelocityComponent().xVelocity;
			float newY = drinker.y + Time.fixedDeltaTime * drinker.VelocityComponent().yVelocity;
						
			Rect leftWallRect = new Rect(0, 0, SBConfig.BORDER_WIDTH, Futile.screen.height);
			Rect rightWallRect = new Rect(Futile.screen.width - SBConfig.BORDER_WIDTH, 0, SBConfig.BORDER_WIDTH, Futile.screen.height);
			Rect bottomWallRect = new Rect(0, 0, Futile.screen.width, SBConfig.BORDER_WIDTH);
			Rect topWallRect = new Rect(0, Futile.screen.height - SBConfig.TOP_UI_HEIGHT - SBConfig.BORDER_WIDTH, Futile.screen.width, SBConfig.BORDER_WIDTH);
			
			Vector2 updatedPoint;
			updatedPoint = bar.CollideComponent().PointToAvoidCollidingFixedRectWithMovingEntity(leftWallRect, drinker, newX, newY);
			updatedPoint = bar.CollideComponent().PointToAvoidCollidingFixedRectWithMovingEntity(rightWallRect, drinker, updatedPoint.x, updatedPoint.y);
			updatedPoint = bar.CollideComponent().PointToAvoidCollidingFixedRectWithMovingEntity(bottomWallRect, drinker, updatedPoint.x, updatedPoint.y);
			updatedPoint = bar.CollideComponent().PointToAvoidCollidingFixedRectWithMovingEntity(topWallRect, drinker, updatedPoint.x, updatedPoint.y);
			updatedPoint = bar.CollideComponent().PointToAvoidCollidingFixedRectWithMovingEntity(bar.SpriteComponent().GetGlobalRect().CloneWithExpansion(-10f), drinker, updatedPoint.x, updatedPoint.y);
			
			drinker.x = updatedPoint.x;
			drinker.y = updatedPoint.y;
		}
	}
	
	public void UpdateDrinkerBarstoolRelations() {
		foreach (SBDrinker drinker in drinkers) {
			if (drinker.hasDrink) continue;
			
			SBBarStool barStool = bar.BarStoolThatIntersectsWithGlobalRect(drinker.SpriteComponent().GetGlobalRect());
			if (barStool != null && barStool.SittableComponent().currentDrinker == null && !drinker.isBeingControlledBySittableComponent) {
				barStool.SittableComponent().SeatDrinker(drinker);
				return;
			}
		}
	}
	
	public void HandleUpdate() {
		HandleKeyInput();
		UpdateDrinkerPositions();
		UpdateDrinkerBarstoolRelations();
		drinker1.HandleUpdate();
		drinker2.HandleUpdate();
		bar.HandleUpdate();
		
		// === temp ===
		
		foreach (SBDrinker drinker in drinkers) {
			if (drinker.hasDrink) {
				if (Input.GetKeyDown(KeyCode.Space)) {
					drinker.hasDrink = false;	
				}
			}
		}
		// === temp ===
	}	
}
