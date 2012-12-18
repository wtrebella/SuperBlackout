using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WTAnimationManager {
	Dictionary<string, WTAnimation> animationDictionary;
	
	public WTAnimationManager() {
		animationDictionary = new Dictionary<string, WTAnimation>();
	}
	
	public void AddAnimation(string animationName, string[] spriteFrameNames, float minFrameDuration, float maxFrameDuration, bool isLooping) {
		if (animationDictionary.ContainsKey(animationName)) return;
		animationDictionary.Add(animationName, new WTAnimation(animationName, spriteFrameNames, minFrameDuration, maxFrameDuration, isLooping));
	}
	
	public void AddAnimation(string animationName, string[] spriteFrameNames, float frameDuration, bool isLooping) {
		if (animationDictionary.ContainsKey(animationName)) return;
		animationDictionary.Add(animationName, new WTAnimation(animationName, spriteFrameNames, frameDuration, isLooping));	
	}
	
	public void AddAnimation(WTAnimation animation, bool isLooping) {
		if (animationDictionary.ContainsKey(animation.name)) return;
		animationDictionary.Add(animation.name, animation);
	}
	
	public WTAnimation AnimationForName(string animationName) {
		if (!animationDictionary.ContainsKey(animationName)) {
			Debug.Log(string.Format("no animation with name {0}", animationName));
			return null;
		}
		
		return animationDictionary[animationName].Copy();
	}
}
