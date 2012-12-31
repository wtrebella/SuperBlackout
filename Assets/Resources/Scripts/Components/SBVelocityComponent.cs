using UnityEngine;
using System.Collections;

public class SBVelocityComponent : SBAbstractComponent {
	public float xVelocity = 0;
	public float yVelocity = 0;
	
	public Direction accelerationDirection = Direction.None;
	
	public Direction xDrunkLean = Direction.Left;
	public Direction yDrunkLean = Direction.Down;
	
	public float maxDrunkLeanVelocity = SBConfig.BASE_DRUNK_LEAN_MAX_VELOCITY;
	public float drunkLeanMultiplier = 1f;
	
	//public int debugDrinkCount = 0;
		
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
				
		if (xDrunkLean == Direction.Right) {
			xDrunkLean = Direction.Left;
			xVelocity = -50f;
		}
		else if (xDrunkLean == Direction.Left) {
			xDrunkLean = Direction.Right;
			xVelocity = 50f;
		}
	}
	
	public void ResetY() {
		yVelocity = 0;
		if (accelerationDirection == Direction.Up || accelerationDirection == Direction.Down) accelerationDirection = Direction.None;
		
		if (yDrunkLean == Direction.Up) {
			yDrunkLean = Direction.Down;
			yVelocity = -50f;
		}
		else if (yDrunkLean == Direction.Down) {
			yDrunkLean = Direction.Up;
			yVelocity = 50f;
		}
	}
		
	public void UpdateAcceleration() {	
		float accelAmt = Time.fixedDeltaTime * SBConfig.DRINKER_ACCELERATION_CONSTANT;
		
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
		
		float curMaxVelocity = SBConfig.DRINKER_MAX_VELOCITY * (SBConfig.MIN_SLOW_DOWN_EFFECT + (1.0f - SBConfig.MIN_SLOW_DOWN_EFFECT) * ((6.0f - OwnerDrinkCount()) / 6.0f));
		
		if (xVelocity < -curMaxVelocity) xVelocity = -curMaxVelocity;
		if (xVelocity > curMaxVelocity) xVelocity = curMaxVelocity;
		if (yVelocity < -curMaxVelocity) yVelocity = -curMaxVelocity;
		if (yVelocity > curMaxVelocity) yVelocity = curMaxVelocity;
	}
		
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
		if (OwnerDrinkCount() == 0) return;

		RefreshDrunkLeanValues();
				
		float rand = RXRandom.Float();
		float likelihoodOfVariation = SBConfig.BASE_LIKELIHOOD_OF_VARIATION * OwnerDrinkCount();
		float randomizedMaxDrunkLeanVelocity = maxDrunkLeanVelocity;
		
		if (rand < likelihoodOfVariation) randomizedMaxDrunkLeanVelocity *= Random.Range(0.2f, 0.7f);
				
		if (accelerationDirection == Direction.Up || accelerationDirection == Direction.Down) {
			if (xDrunkLean == Direction.Right && xVelocity < randomizedMaxDrunkLeanVelocity) {
				xVelocity += SBConfig.BASE_DRUNK_LEAN_VELOCITY_ADDER * drunkLeanMultiplier;
				if (xVelocity >= randomizedMaxDrunkLeanVelocity) {
					xVelocity = randomizedMaxDrunkLeanVelocity;
					xDrunkLean = Direction.Left;
				}
			}
			else if (xDrunkLean == Direction.Left && xVelocity > -randomizedMaxDrunkLeanVelocity) {
				xVelocity -= SBConfig.BASE_DRUNK_LEAN_VELOCITY_ADDER * drunkLeanMultiplier;
				if (xVelocity <= -randomizedMaxDrunkLeanVelocity) {
					xVelocity = -randomizedMaxDrunkLeanVelocity;
					xDrunkLean = Direction.Right;
				}
			}
			
			if (RXRandom.Float() < SBConfig.BASE_LIKELIHOOD_OF_TURN_AROUND * OwnerDrinkCount()) {
				yVelocity *= -1;
				yVelocity *= OwnerDrinkCount() * 0.15f + Random.Range(0, 0.2f);
			}
		}
		
		if (accelerationDirection == Direction.Right || accelerationDirection == Direction.Left) {
			if (yDrunkLean == Direction.Up && yVelocity < randomizedMaxDrunkLeanVelocity) {
				yVelocity += SBConfig.BASE_DRUNK_LEAN_VELOCITY_ADDER * drunkLeanMultiplier;
				if (yVelocity >= randomizedMaxDrunkLeanVelocity) {
					yVelocity = randomizedMaxDrunkLeanVelocity;
					yDrunkLean = Direction.Down;
				}
			}
			else if (yDrunkLean == Direction.Down && yVelocity > -randomizedMaxDrunkLeanVelocity) {
				yVelocity -= SBConfig.BASE_DRUNK_LEAN_VELOCITY_ADDER * drunkLeanMultiplier;
				if (yVelocity <= -randomizedMaxDrunkLeanVelocity) {
					yVelocity = -randomizedMaxDrunkLeanVelocity;
					yDrunkLean = Direction.Up;
				}
			}
			
			if (RXRandom.Float() < SBConfig.BASE_LIKELIHOOD_OF_TURN_AROUND * OwnerDrinkCount()) {
				xVelocity *= -1;
				xVelocity *= OwnerDrinkCount() * 0.15f + Random.Range(0, 0.2f);
			}
		}
	}
	
	public void RefreshDrunkLeanValues() {
		maxDrunkLeanVelocity = Mathf.Max(SBConfig.BASE_DRUNK_LEAN_MAX_VELOCITY, SBConfig.BASE_DRUNK_LEAN_MAX_VELOCITY * SBConfig.DRUNK_LEAN_MAX_VELOCITY_MULTIPLIER * OwnerDrinkCount());
		drunkLeanMultiplier = Mathf.Max(1.0f, SBConfig.DRUNK_LEAN_MULTIPLIER_MULTIPLIER * OwnerDrinkCount());
	}
		
	private int OwnerDrinkCount() {
		//return debugDrinkCount;
		return (owner as SBDrinker).drinkCount;	
	}
}
