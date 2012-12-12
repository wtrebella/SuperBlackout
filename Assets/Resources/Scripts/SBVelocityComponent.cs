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
		xVelocity = 0;
		yVelocity = 0;
		accelerationDirection = Direction.None;	
	}
		
	public void UpdateAcceleration() {	
		float accelAmt = Time.fixedDeltaTime * SBConfig.DRINKER_ACCELERATION_CONSTANT;
		
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
	}
}
