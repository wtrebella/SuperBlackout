using UnityEngine;
using System.Collections;

public class WTAnimation {
	public float animationTimer = 0;
	public int frameIndex = 0;
	public FAtlasElement[] spriteFrames;
	public float frameDuration;
	public float minFrameDuration;
	public float maxFrameDuration;
	public bool isLooping;
	public SBSpriteComponent animationDoneDelegate;
	
	public WTAnimation(string[] spriteFrameNames, float minFrameDuration, float maxFrameDuration, bool isLooping) {
		spriteFrames = new FAtlasElement[spriteFrameNames.Length];
		this.isLooping = isLooping;
		this.minFrameDuration = minFrameDuration;
		this.maxFrameDuration = maxFrameDuration;
		this.frameDuration = this.minFrameDuration;
		
		for (int i = 0; i < spriteFrameNames.Length; i++) {
			string spriteFrameName = spriteFrameNames[i];
			spriteFrames[i] = Futile.atlasManager.GetElementWithName(spriteFrameName);
		}
	}
	
	public WTAnimation(FAtlasElement[] spriteFrames, float minFrameDuration, float maxFrameDuration, bool isLooping) {
		this.spriteFrames = spriteFrames;
		this.isLooping = isLooping;
		this.minFrameDuration = minFrameDuration;
		this.maxFrameDuration = maxFrameDuration;
		this.frameDuration = this.minFrameDuration;
	}
	
	public WTAnimation(string[] spriteFrameNames, float frameDuration, bool isLooping) : this(spriteFrameNames, frameDuration, frameDuration, isLooping) {
		
	}
	
	public WTAnimation Copy() {
		return new WTAnimation(spriteFrames, minFrameDuration, maxFrameDuration, isLooping);
	}
	
	public void ResetSpriteToFirstFrame(FSprite sprite) {
		sprite.element = spriteFrames[0];	
	}
	
	public void HandleUpdateWithSprite(FSprite sprite) {
		animationTimer += Time.fixedDeltaTime;
		
		if (animationTimer >= frameDuration) {
			animationTimer = 0;
			frameIndex++;
			if (isLooping) {
				frameIndex = frameIndex % spriteFrames.Length;
				sprite.element = spriteFrames[frameIndex];
			}
			else {
				if (animationDoneDelegate != null) animationDoneDelegate.AnimationDone(this);
			}
		}
	}
}
