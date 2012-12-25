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
				break;
			case Direction.Right:
				xVelocity += accelAmt;
				break;
			case Direction.Down:
				yVelocity -= accelAmt;
				break;
			case Direction.Up:
				yVelocity += accelAmt;
				break;
			case Direction.None:
				break;
			}
		}
		
		else {
			switch (accelerationDirection) {
			case Direction.Left:
				xVelocity -= accelAmt;
				yVelocity -= accelAmt / 2f;
				break;
			case Direction.Right:
				xVelocity += accelAmt;
				yVelocity += accelAmt / 2f;
				break;
			case Direction.Down:
				yVelocity -= accelAmt;
				xVelocity -= accelAmt / 2f;
				break;
			case Direction.Up:
				yVelocity += accelAmt;
				xVelocity += accelAmt / 2f;
				break;
			case Direction.None:
				break;
			}	
		}
		
		if (xVelocity < -SBConfig.DRINKER_MAX_VELOCITY) xVelocity = -SBConfig.DRINKER_MAX_VELOCITY;
		if (xVelocity > SBConfig.DRINKER_MAX_VELOCITY) xVelocity = SBConfig.DRINKER_MAX_VELOCITY;
		if (yVelocity < -SBConfig.DRINKER_MAX_VELOCITY) yVelocity = -SBConfig.DRINKER_MAX_VELOCITY;
		if (yVelocity > SBConfig.DRINKER_MAX_VELOCITY) yVelocity = SBConfig.DRINKER_MAX_VELOCITY;

	}
	
	static int frameCount = 0;
	
	public void UpdateDeceleration() {
		float decelAmt = Time.fixedDeltaTime * SBConfig.DRINKER_ACCELERATION_CONSTANT;
		
		float xVelPrev = xVelocity;
		float yVelPrev = yVelocity;
		
		if (xVelPrev != 0) xVelocity += decelAmt * Mathf.Sign(-xVelocity);
		if (yVelPrev != 0) yVelocity += decelAmt * Mathf.Sign(-yVelocity);
		
		if (Mathf.Sign(xVelPrev) != Mathf.Sign(xVelocity)) xVelocity = 0;
		if (Mathf.Sign(yVelPrev) != Mathf.Sign(yVelocity)) yVelocity = 0;
		
		if (accelerationDirection == Direction.Left || accelerationDirection == Direction.Right) xVelocity = xVelPrev;
		if (accelerationDirection == Direction.Down || accelerationDirection == Direction.Up) yVelocity = yVelPrev;
		
		frameCount++;
	}
	
	private int OwnerDrinkCount() {
		return (owner as SBDrinker).drinkCount;	
	}
}
