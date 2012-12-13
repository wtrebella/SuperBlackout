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
	
	public void FaceDirection(Direction toDirection, bool instantly = false) {		
		Direction fromDirection = direction_;

		if (fromDirection == toDirection) return;
		
		direction_ = toDirection;
				
		owner.isBeingControlledByDirectionComponent = true;
		
		if (directionState == DirectionState.Moving) {
			List<AbstractTween> tweens = Go.tweensWithId(currentTweenID);
			if (tweens.Count > 0) {
				foreach (AbstractTween tween in tweens) tween.destroy();
			}
		}
		
		directionState = DirectionState.Moving;
				
		switch (toDirection) {
		case Direction.Right:
			if (fromDirection == Direction.Down) currentAbsoluteRotation_ -= 90;
			else if (fromDirection == Direction.Left) currentAbsoluteRotation_ += 180;
			else if (fromDirection == Direction.Up) currentAbsoluteRotation_ += 90;
			else if (fromDirection == Direction.None) currentAbsoluteRotation_ = 0;
			break;
		case Direction.Down:
			if (fromDirection == Direction.Up) currentAbsoluteRotation_ += 180;
			else if (fromDirection == Direction.Left) currentAbsoluteRotation_ -= 90;
			else if (fromDirection == Direction.Right) currentAbsoluteRotation_ += 90;
			else if (fromDirection == Direction.None) currentAbsoluteRotation_ = 90;
			break;
		case Direction.Left:
			if (fromDirection == Direction.Down) currentAbsoluteRotation_ += 90;
			else if (fromDirection == Direction.Right) currentAbsoluteRotation_ += 180;
			else if (fromDirection == Direction.Up) currentAbsoluteRotation_ -= 90;
			else if (fromDirection == Direction.None) currentAbsoluteRotation_ = 180;
			break;
		case Direction.Up:
			if (fromDirection == Direction.Down) currentAbsoluteRotation_ += 180;
			else if (fromDirection == Direction.Left) currentAbsoluteRotation_ += 90;
			else if (fromDirection == Direction.Right) currentAbsoluteRotation_ -= 90;
			else if (fromDirection == Direction.None) currentAbsoluteRotation_ = -90;
			break;
		case Direction.None:
			Debug.Log("uhhh this guy has no direction");
			break;
		}
		
		if (instantly) {
			owner.rotation = currentAbsoluteRotation_;
			HandleDoneMovingToDirection(null);
		}
		else {
			currentTweenID = ++directionTweenID;
			
			Go.to(owner, 0.2f, new TweenConfig()
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
