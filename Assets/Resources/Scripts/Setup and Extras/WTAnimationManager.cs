using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WTAnimationManager {
	Dictionary<string, WTAnimation> animationDictionary;
	
	public WTAnimationManager() {
		animationDictionary = new Dictionary<string, WTAnimation>();
	}
	
	public void AddAnimation(string animationName, string[] spriteFrameNames, float minFrameDuration, float maxFrameDuration) {
		animationDictionary.Add(animationName, new WTAnimation(spriteFrameNames, minFrameDuration, maxFrameDuration));
	}
	
	public void AddAnimation(string animationName, string[] spriteFrameNames, float frameDuration) {
		animationDictionary.Add(animationName, new WTAnimation(spriteFrameNames, frameDuration));	
	}
	
	public void AddAnimation(string animationName, WTAnimation animation) {
		animationDictionary.Add(animationName, animation);
	}
	
	public WTAnimation AnimationForName(string animationName) {
		if (!animationDictionary.ContainsKey(animationName)) {
			Debug.Log(string.Format("no animation with name {0}", animationName));
			return null;
		}
		
		return animationDictionary[animationName].Copy();
	}
}
