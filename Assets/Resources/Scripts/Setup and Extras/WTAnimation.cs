using UnityEngine;
using System.Collections;

public class WTAnimation {
	public float animationTimer = 0;
	public int frameIndex = 0;
	public FAtlasElement[] spriteFrames;
	public float frameDuration;
	public float minFrameDuration;
	public float maxFrameDuration;
	
	public WTAnimation(string[] spriteFrameNames, float minFrameDuration, float maxFrameDuration) {
		spriteFrames = new FAtlasElement[spriteFrameNames.Length];
		this.minFrameDuration = minFrameDuration;
		this.maxFrameDuration = maxFrameDuration;
		this.frameDuration = this.minFrameDuration;
		
		for (int i = 0; i < spriteFrameNames.Length; i++) {
			string spriteFrameName = spriteFrameNames[i];
			spriteFrames[i] = Futile.atlasManager.GetElementWithName(spriteFrameName);
		}
	}
	
	public WTAnimation(FAtlasElement[] spriteFrames, float minFrameDuration, float maxFrameDuration) {
		this.spriteFrames = spriteFrames;
		this.minFrameDuration = minFrameDuration;
		this.maxFrameDuration = maxFrameDuration;
		this.frameDuration = this.minFrameDuration;
	}
	
	public WTAnimation(string[] spriteFrameNames, float frameDuration) : this(spriteFrameNames, frameDuration, frameDuration) {
		
	}
	
	public WTAnimation Copy() {
		return new WTAnimation(spriteFrames, minFrameDuration, maxFrameDuration);
	}
	
	public void ResetSpriteToFirstFrame(FSprite sprite) {
		sprite.element = spriteFrames[0];	
	}
	
	public void HandleUpdateWithSprite(FSprite sprite) {
		animationTimer += Time.fixedDeltaTime;
		
		if (animationTimer >= frameDuration) {
			animationTimer = 0;
			frameIndex = (frameIndex + 1) % spriteFrames.Length;
			sprite.element = spriteFrames[frameIndex];		
		}
	}
}
