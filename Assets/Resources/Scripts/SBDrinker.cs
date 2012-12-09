using UnityEngine;
using System.Collections;

public class SBDrinker : FContainer {
	public FSprite sprite;
	
	public SBDrinker() {
		sprite = new FSprite("drinker.psd");
		AddChild(sprite);
	}
}