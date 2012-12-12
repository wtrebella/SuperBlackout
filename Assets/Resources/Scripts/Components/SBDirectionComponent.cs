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
	private DirectionState directionState = DirectionState.Static;
	
	public SBDirectionComponent() {
		name = "direction component";
		componentType = ComponentType.Direction;
	}
	
	private void FaceDirection(Direction direction) {		
		if (directionState == DirectionState.Moving) {
			List<AbstractTween> tweens = Go.tweensWithId(currentTweenID);
			if (tweens.Count > 0) {
				foreach (AbstractTween tween in tweens) tween.destroy();
			}
		}
		
		directionState = DirectionState.Moving;
		
		float newRotation = 0;
		
		// don't let it turn more than 180
		
		switch (direction) {
		case Direction.Right:
			newRotation = 0;
			break;
		case Direction.Down:
			newRotation = 0;
			break;
		case Direction.Left:
			newRotation = 180;
			break;
		case Direction.Up:
			newRotation = 270;
			break;
		case Direction.None:
			Debug.Log("uhhh this guy has no direction");
			break;
		}
		
		currentTweenID = ++directionTweenID;
		
		Go.to(owner, 0.2f, new TweenConfig()
			.floatProp("rotation", newRotation)
			.setId(currentTweenID)
			.onComplete(HandleDoneMovingToDirection));
	}
	
	public void HandleDoneMovingToDirection(AbstractTween tween) {
		directionState = DirectionState.Static;
	}
	
	public Direction direction {
		get {return direction_;}
		set {
			if (this.direction == value) return;
			direction_ = value;
			FaceDirection(direction_);
		}
	}
}
