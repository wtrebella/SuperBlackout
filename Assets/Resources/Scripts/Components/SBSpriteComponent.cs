using UnityEngine;
using System.Collections;

public class SBSpriteComponent : SBAbstractComponent {
	public FSprite sprite;
	public bool shouldBeInRotatingContainer;
	public bool isAnimating = false;
	public float frameDuration;
	
	private float animationTimer = 0;
	private int frameIndex = 0;
	private FAtlasElement[] spriteFrames;
	
	public SBSpriteComponent(string imageName, bool ableToRotate) : this(imageName, ableToRotate, null, 0) {
		
	}
	
	public SBSpriteComponent(string imageName, bool ableToRotate, string[] spriteFrameNames, float frameDuration) {
		this.shouldBeInRotatingContainer = ableToRotate;
		sprite = new FSprite(imageName);
		componentType = ComponentType.Sprite;
		name = "sprite component";
		
		if (spriteFrameNames == null) return;
		
		this.frameDuration = frameDuration;
		animationTimer = frameDuration;
		spriteFrames = new FAtlasElement[spriteFrameNames.Length];
		
		for (int i = 0; i < spriteFrameNames.Length; i++) {
			string spriteFrameName = spriteFrameNames[i];
			spriteFrames[i] = Futile.atlasManager.GetElementWithName(spriteFrameName);
		}
	}
	
	public void UpdateAnimation() {
		animationTimer += Time.fixedDeltaTime;
		if (animationTimer >= frameDuration) {
			animationTimer -= frameDuration;
			frameIndex = (frameIndex + 1) % spriteFrames.Length;
			sprite.element = spriteFrames[frameIndex];	
		}
	}
	
	public void StopAnimation() {
		PauseAnimation();
		ResetAnimation();
	}
	
	public void ResetAnimation() {
		sprite.element = spriteFrames[0];	
	}
	
	public void PauseAnimation() {
		isAnimating = false;
		animationTimer = frameDuration;
	}
	
	public void StartAnimation() {
		isAnimating = true;
	}
	
	public void RestartAnimation() {
		ResetAnimation();
		StartAnimation();
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
		if (!isAnimating) return;
		
		UpdateAnimation();
	}
}
