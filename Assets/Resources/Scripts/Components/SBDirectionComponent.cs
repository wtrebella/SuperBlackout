using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// must be used in conjunction with SBSpriteComponent, obviously, so there's something to see facing a direction

enum DirectionState {
	Static,
	Moving
}

public class SBDirectionComponent : SBAbstractComponent {
	static private int directionTweenID = -1;
	int currentTweenID = -1;
	private Direction direction_ = Direction.None;
	private float currentAbsoluteRotation_ = 0;
	private DirectionState directionState = DirectionState.Static;
	
	public SBDirectionComponent() {
		name = "direction component";
		componentType = ComponentType.Direction;
	}
	
	public Direction GetCurrentDirection() {
		return direction_;	
	}
	
	public void FaceDirection(Direction toDirection, bool instantly = false) {		
		Direction fromDirection = direction_;
						
		float newAbsoluteRotation = currentAbsoluteRotation_;
		
		switch (toDirection) {
		case Direction.Right:
			if (fromDirection == Direction.Down) newAbsoluteRotation -= 90;
			else if (fromDirection == Direction.Left) newAbsoluteRotation += 180;
			else if (fromDirection == Direction.Up) newAbsoluteRotation += 90;
			else if (fromDirection == Direction.None) newAbsoluteRotation = 0;
			break;
		case Direction.Down:
			if (fromDirection == Direction.Up) newAbsoluteRotation += 180;
			else if (fromDirection == Direction.Left) newAbsoluteRotation -= 90;
			else if (fromDirection == Direction.Right) newAbsoluteRotation += 90;
			else if (fromDirection == Direction.None) newAbsoluteRotation = 90;
			break;
		case Direction.Left:
			if (fromDirection == Direction.Down) newAbsoluteRotation += 90;
			else if (fromDirection == Direction.Right) newAbsoluteRotation += 180;
			else if (fromDirection == Direction.Up) newAbsoluteRotation -= 90;
			else if (fromDirection == Direction.None) newAbsoluteRotation = 180;
			break;
		case Direction.Up:
			if (fromDirection == Direction.Down) newAbsoluteRotation += 180;
			else if (fromDirection == Direction.Left) newAbsoluteRotation += 90;
			else if (fromDirection == Direction.Right) newAbsoluteRotation -= 90;
			else if (fromDirection == Direction.None) newAbsoluteRotation = -90;
			break;
		case Direction.None:
			Debug.Log("uhhh this guy has no direction");
			break;
		}
		
		if (direction_ == toDirection) {
			if (owner.rotatingContainer.rotation != newAbsoluteRotation) {
				if (directionState == DirectionState.Moving) return;	
			}
		}
		
		if (directionState == DirectionState.Moving) {
			List<AbstractTween> tweens = Go.tweensWithId(currentTweenID);
			if (tweens != null && tweens.Count > 0) {
				foreach (AbstractTween tween in tweens) tween.destroy();
			}
		}
		
		currentAbsoluteRotation_ = newAbsoluteRotation;
		directionState = DirectionState.Moving;
		owner.isBeingControlledByDirectionComponent = true;
		direction_ = toDirection;

		if (instantly) {
			owner.rotatingContainer.rotation = currentAbsoluteRotation_;
			HandleDoneMovingToDirection(null);
		}
		else {
			currentTweenID = ++directionTweenID;
			
			Go.to(owner.rotatingContainer, 0.2f, new TweenConfig()
				.floatProp("rotation", currentAbsoluteRotation_)
				.setId(currentTweenID)
				.onComplete(HandleDoneMovingToDirection));
		}
	}
	
	public void HandleDoneMovingToDirection(AbstractTween tween) {
		directionState = DirectionState.Static;
		owner.isBeingControlledByDirectionComponent = false;
	}
}
