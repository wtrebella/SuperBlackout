using UnityEngine;
using System.Collections;

public class SBCollideComponent : SBAbstractComponent {

	public SBCollideComponent() {
		componentType = ComponentType.Collide;
		name = "collide component";
	}
	
	public Vector2 PointToAvoidCollidingFixedRectWithMovingEntity(Rect fixedRect, SBEntity entity, float newX, float newY) {				
		float xDelta = newX - entity.x;
		float yDelta = newY - entity.y;
		
		Rect preMovedSpriteRect = entity.SpriteComponent().GetGlobalRect();
		Rect postMovedSpriteRect = entity.SpriteComponent().GetGlobalRectWithOffset(xDelta, yDelta);
		
		if (!postMovedSpriteRect.CheckIntersect(fixedRect)) {
			return new Vector2(newX, newY);
		}
	
		float xMin = fixedRect.xMin;
		float xMax = fixedRect.xMax;
		float yMin = fixedRect.yMin;
		float yMax = fixedRect.yMax;
				
		if (preMovedSpriteRect.xMin >= xMax && postMovedSpriteRect.xMin < xMax) {
			newX = xMax + preMovedSpriteRect.width / 2f;
			entity.VelocityComponent().ResetX();
		}
		else if (preMovedSpriteRect.xMax <= xMin && postMovedSpriteRect.xMax > xMin) {
			newX = xMin - preMovedSpriteRect.width / 2f;
			entity.VelocityComponent().ResetX();
		}
		
		if (preMovedSpriteRect.yMin >= yMax && postMovedSpriteRect.yMin < yMax) {
			newY = yMax + preMovedSpriteRect.height / 2f;
			entity.VelocityComponent().ResetY();
		}
		else if (preMovedSpriteRect.yMax <= yMin && postMovedSpriteRect.yMax > yMin) {
			newY = yMin - preMovedSpriteRect.height / 2f;
			entity.VelocityComponent().ResetY();
		}
		
		return new Vector2(newX, newY);
	}
}