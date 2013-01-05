using UnityEngine;
using System.Collections;

public class SBVelocityComponent : SBAbstractComponent {
	public float xVelocity = 0;
	public float yVelocity = 0;
	
	public Direction accelerationDirection = Direction.None;
	
	public Direction xDrunkLean = Direction.Left;
	public Direction yDrunkLean = Direction.Down;
	
	public bool shouldDecelerate = true;
	
	public float maxDrunkLeanVelocity;
	public float drunkLeanVelocityAdder;
	public float likelihoodOfDrunkLeanVariation;
	public float likelihoodOfDrunkStall;
	public float strengthOfDrunkStall;
	public float slowDownEffect;
	
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
		
		float curMaxVelocity = SBConfig.DRINKER_MAX_VELOCITY * (slowDownEffect + (1.0f - slowDownEffect) * ((6.0f - OwnerDrinkCount()) / 6.0f));
		
		if (xVelocity < -curMaxVelocity) xVelocity = -curMaxVelocity;
		if (xVelocity > curMaxVelocity) xVelocity = curMaxVelocity;
		if (yVelocity < -curMaxVelocity) yVelocity = -curMaxVelocity;
		if (yVelocity > curMaxVelocity) yVelocity = curMaxVelocity;
	}
		
	public void UpdateDeceleration() {
		if (!shouldDecelerate) return;
		
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
				
		float rand = RXRandom.Float();
		
		float randomizedMaxDrunkLeanVelocity = maxDrunkLeanVelocity;
		
		if (rand < likelihoodOfDrunkLeanVariation) randomizedMaxDrunkLeanVelocity *= Random.Range(0.2f, 0.7f);
				
		if (accelerationDirection == Direction.Up || accelerationDirection == Direction.Down) {
			if (xDrunkLean == Direction.Right) {
				xVelocity += drunkLeanVelocityAdder;
				if (xVelocity >= randomizedMaxDrunkLeanVelocity) {
					xVelocity = randomizedMaxDrunkLeanVelocity;
					xDrunkLean = Direction.Left;
				}
			}
			else if (xDrunkLean == Direction.Left) {
				xVelocity -= drunkLeanVelocityAdder;
				if (xVelocity <= -randomizedMaxDrunkLeanVelocity) {
					xVelocity = -randomizedMaxDrunkLeanVelocity;
					xDrunkLean = Direction.Right;
				}
			}
			
			if (RXRandom.Float() < likelihoodOfDrunkStall) {
				yVelocity -= yVelocity * strengthOfDrunkStall * 2 * Random.Range(0.8f, 1.2f);
			}
		}
		
		if (accelerationDirection == Direction.Right || accelerationDirection == Direction.Left) {
			if (yDrunkLean == Direction.Up) {
				yVelocity += drunkLeanVelocityAdder;
				if (yVelocity >= randomizedMaxDrunkLeanVelocity) {
					yVelocity = randomizedMaxDrunkLeanVelocity;
					yDrunkLean = Direction.Down;
				}
			}
			else if (yDrunkLean == Direction.Down) {
				yVelocity -= drunkLeanVelocityAdder;
				if (yVelocity <= -randomizedMaxDrunkLeanVelocity) {
					yVelocity = -randomizedMaxDrunkLeanVelocity;
					yDrunkLean = Direction.Up;
				}
			}
			
			if (RXRandom.Float() < likelihoodOfDrunkStall) {
				xVelocity -= xVelocity * strengthOfDrunkStall * 2 * Random.Range(0.8f, 1.2f);
			}
		}
	}
	
	public void RefreshDrunkLeanValues() {
		switch (OwnerDrinkCount()) {
		case 1:
			maxDrunkLeanVelocity = 200f;
			drunkLeanVelocityAdder = 15f;
			likelihoodOfDrunkLeanVariation = 0.15f;
			likelihoodOfDrunkStall = 0.005f;
			strengthOfDrunkStall = 0.2f;
			slowDownEffect = 0.95f;
			break;
		case 2:
			maxDrunkLeanVelocity = 300f;
			drunkLeanVelocityAdder = 20f;
			likelihoodOfDrunkLeanVariation = 0.2f;
			likelihoodOfDrunkStall = 0.006f;
			strengthOfDrunkStall = 0.3f;
			slowDownEffect = 0.9f;
			break;
		case 3:
			maxDrunkLeanVelocity = 400f;
			drunkLeanVelocityAdder = 25f;
			likelihoodOfDrunkLeanVariation = 0.25f;
			likelihoodOfDrunkStall = 0.007f;
			strengthOfDrunkStall = 0.4f;
			slowDownEffect = 0.85f;
			break;
		case 4:
			maxDrunkLeanVelocity = 700f;
			drunkLeanVelocityAdder = 30f;
			likelihoodOfDrunkLeanVariation = 0.3f;
			likelihoodOfDrunkStall = 0.008f;
			strengthOfDrunkStall = 0.5f;
			slowDownEffect = 0.8f;
			break;
		case 5:
			maxDrunkLeanVelocity = 900f;
			drunkLeanVelocityAdder = 40f;
			likelihoodOfDrunkLeanVariation = 0.37f;
			likelihoodOfDrunkStall = 0.009f;
			strengthOfDrunkStall = 0.6f;
			slowDownEffect = 0.75f;
			break;
		case 6:
			maxDrunkLeanVelocity = 1000f;
			drunkLeanVelocityAdder = 50f;
			likelihoodOfDrunkLeanVariation = 0.5f;
			likelihoodOfDrunkStall = 0.01f;
			strengthOfDrunkStall = 1.0f;
			slowDownEffect = 0.7f;
			break;
		default:
			break;
		}
		
	}
		
	private int OwnerDrinkCount() {
		if (owner.GetType().FullName == "SBDrinker") {
			//return debugDrinkCount;
			return (owner as SBDrinker).drinkCount;
		}
		return 0;
	}
}
