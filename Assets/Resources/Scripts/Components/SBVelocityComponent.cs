using UnityEngine;
using System.Collections;

public class SBVelocityComponent : SBAbstractComponent {
	public float xVelocity = 0;
	public float yVelocity = 0;
	
	public Direction accelerationDirection = Direction.None;
	
	public SBVelocityComponent() {
		componentType = ComponentType.Velocity;
		name = "velocity component";
	}
	
	override public void HandleUpdate() {
		base.HandleUpdate();
		UpdateAcceleration();
		UpdateDeceleration();		
	}
	
	public void Reset() {
		ResetX();
		ResetY();
	}
	
	public void ResetX() {
		xVelocity = 0;
		if (accelerationDirection == Direction.Left || accelerationDirection == Direction.Right) accelerationDirection = Direction.None;
	}
	
	public void ResetY() {
		yVelocity = 0;
		if (accelerationDirection == Direction.Up || accelerationDirection == Direction.Down) accelerationDirection = Direction.None;
	}
		
	public void UpdateAcceleration() {	
		float accelAmt = Time.fixedDeltaTime * SBConfig.DRINKER_ACCELERATION_CONSTANT;
		
		if (OwnerDrinkCount() == 0) {
			switch (accelerationDirection) {
			case Direction.Left:
				xVelocity -= accelAmt;
				if (xVelocity < -SBConfig.DRINKER_MAX_VELOCITY) xVelocity = -SBConfig.DRINKER_MAX_VELOCITY;
				break;
			case Direction.Right:
				xVelocity += accelAmt;
				if (xVelocity > SBConfig.DRINKER_MAX_VELOCITY) xVelocity = SBConfig.DRINKER_MAX_VELOCITY;
				break;
			case Direction.Down:
				yVelocity -= accelAmt;
				if (yVelocity < -SBConfig.DRINKER_MAX_VELOCITY) yVelocity = -SBConfig.DRINKER_MAX_VELOCITY;
				break;
			case Direction.Up:
				yVelocity += accelAmt;
				if (yVelocity > SBConfig.DRINKER_MAX_VELOCITY) yVelocity = SBConfig.DRINKER_MAX_VELOCITY;
				break;
			case Direction.None:
				break;
			}
		}
		
		else {
			switch (accelerationDirection) {
			case Direction.Left:
				xVelocity -= accelAmt;
				if (xVelocity < -SBConfig.DRINKER_MAX_VELOCITY) xVelocity = -SBConfig.DRINKER_MAX_VELOCITY;
				break;
			case Direction.Right:
				xVelocity += accelAmt;
				if (xVelocity > SBConfig.DRINKER_MAX_VELOCITY) xVelocity = SBConfig.DRINKER_MAX_VELOCITY;
				break;
			case Direction.Down:
				yVelocity -= accelAmt;
				if (yVelocity < -SBConfig.DRINKER_MAX_VELOCITY) yVelocity = -SBConfig.DRINKER_MAX_VELOCITY;
				break;
			case Direction.Up:
				yVelocity += accelAmt;
				if (yVelocity > SBConfig.DRINKER_MAX_VELOCITY) yVelocity = SBConfig.DRINKER_MAX_VELOCITY;
				break;
			case Direction.None:
				break;
			}	
		}
	}
	
	static int frameCount = 0;
	
	public void UpdateDeceleration() {
		float decelAmt = Time.fixedDeltaTime * SBConfig.DRINKER_ACCELERATION_CONSTANT;
		
		if (xVelocity > 0 && accelerationDirection != Direction.Right) {
			xVelocity -= decelAmt;
			if (xVelocity < 0) xVelocity = 0;
		}
		else if (xVelocity < 0 && accelerationDirection != Direction.Left) {
			xVelocity += decelAmt;
			if (xVelocity > 0) xVelocity = 0;
		}
		
		if (yVelocity > 0 && accelerationDirection != Direction.Up) {
			yVelocity -= decelAmt;
			if (yVelocity < 0) yVelocity = 0;
		}
		else if (yVelocity < 0 && accelerationDirection != Direction.Down) {
			yVelocity += decelAmt;
			if (yVelocity > 0) yVelocity = 0;
		}
		
		frameCount++;
		
		if (frameCount % 15 == 0 && OwnerDrinkCount() > 0) {
			xVelocity += Random.Range(-20000f, 20000f) * Time.fixedDeltaTime;
			yVelocity += Random.Range(-20000f, 20000f) * Time.fixedDeltaTime;
			
			if (xVelocity > SBConfig.DRINKER_MAX_VELOCITY) xVelocity = SBConfig.DRINKER_MAX_VELOCITY;
			if (yVelocity > SBConfig.DRINKER_MAX_VELOCITY) yVelocity = SBConfig.DRINKER_MAX_VELOCITY;
			if (xVelocity < -SBConfig.DRINKER_MAX_VELOCITY) xVelocity = -SBConfig.DRINKER_MAX_VELOCITY;
			if (yVelocity < -SBConfig.DRINKER_MAX_VELOCITY) yVelocity = -SBConfig.DRINKER_MAX_VELOCITY;
		}
	}
	
	private int OwnerDrinkCount() {
		return (owner as SBDrinker).drinkCount;	
	}
}
