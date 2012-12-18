using UnityEngine;
using System.Collections;

public class SBSpriteComponent : SBAbstractComponent {
	public FSprite sprite;
	public bool shouldBeInRotatingContainer;
	public bool isAnimating = false;
	public WTAnimation currentAnimation;
	
	public SBSpriteComponent(string imageName, bool ableToRotate) {
		this.shouldBeInRotatingContainer = ableToRotate;
		sprite = new FSprite(imageName);
		componentType = ComponentType.Sprite;
		name = "sprite component";
	}

	public void StopAnimation() {
		PauseAnimation();
		ResetAnimation();
	}
	
	public void ResetAnimation() {
		currentAnimation.ResetSpriteToFirstFrame(sprite);
	}
	
	public void PauseAnimation() {
		isAnimating = false;
		currentAnimation.animationDelegate = null;
		currentAnimation.animationTimer = currentAnimation.frameDuration;
	}
	
	public void StartAnimation(WTAnimation animation) {
		currentAnimation = animation;
		isAnimating = true;
	}
	
	public void RestartAnimation() {
		ResetAnimation();
		StartAnimation(currentAnimation);
	}
	
	public Rect GetGlobalRect() {
		Vector2 globalPos = GetGlobalPosition();
		float adjWidth = sprite.width * sprite.container.scaleX;
		float adjHeight = sprite.height * sprite.container.scaleY;
		float xOrigin = globalPos.x - adjWidth / 2f;
		float yOrigin = globalPos.y - adjHeight / 2f;
		return new Rect(xOrigin, yOrigin, adjWidth, adjHeight);
	}
	
	public Rect GetGlobalRectWithOffset(float xOffset, float yOffset) {
		Rect globalRect = GetGlobalRect();
		return globalRect.CloneAndOffset(xOffset, yOffset);
	}
		
	public Vector2 GetGlobalPosition() {
		return sprite.container.LocalToGlobal(new Vector2(sprite.x, sprite.y));
	}
	
	override public void HandleUpdate() {
		if (!isAnimating || currentAnimation == null) return;
		
		currentAnimation.HandleUpdateWithSprite(sprite);	
	}
}
