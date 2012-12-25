using UnityEngine;
using System.Collections;

public class SBVelocityComponent : SBAbstractComponent {
	public float xVelocity = 0;
	public float yVelocity = 0;
	
	public Direction accelerationDirection = Direction.None;
	
	public Direction xCurrentDrunkLean = Direction.None;
	public Direction yCurrentDrunkLean = Direction.Up;
	
	public SBVelocityComponent() {
		componentType = ComponentType.Velocity;
		name = "velocity component";
	}
	
	override public void HandleUpdate() {
		base.HandleUpdate();
		UpdateAcceleration();
		UpdateDeceleration();
		UpdateDrunkAdjustments();
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
		
		switch (accelerationDirection) {
		case Direction.Left:
			xVelocity -= accelAmt;
			xCurrentDrunkLean = Direction.Left;
			break;
		case Direction.Right:
			xVelocity += accelAmt;
			xCurrentDrunkLean = Direction.Right;
			break;
		case Direction.Down:
			yVelocity -= accelAmt;
			yCurrentDrunkLean = Direction.Down;
			break;
		case Direction.Up:
			yVelocity += accelAmt;
			yCurrentDrunkLean = Direction.Up;
			break;
		case Direction.None:
			//xCurrentDrunkLean = yCurrentDrunkLean = Direction.None;
			break;
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
	}
	
	private void UpdateDrunkAdjustments() {
		if (OwnerDrinkCount() == 0) {
			Debug.Log(xCurrentDrunkLean);
			
			float amt = 75f;
						
			if (accelerationDirection == Direction.Up || accelerationDirection == Direction.Down) {
				if (xCurrentDrunkLean == Direction.Right) xVelocity += amt;
				else if (xCurrentDrunkLean == Direction.Left) xVelocity -= amt;
			}
			
			if (accelerationDirection == Direction.Right || accelerationDirection == Direction.Left) {
				if (yCurrentDrunkLean == Direction.Up) yVelocity += amt;
				else if (yCurrentDrunkLean == Direction.Down) yVelocity -= amt;
			}
		}
		
		frameCount++;
	}
	
	private int OwnerDrinkCount() {
		return (owner as SBDrinker).drinkCount;	
	}
}
