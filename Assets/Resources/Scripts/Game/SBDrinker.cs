using UnityEngine;
using System.Collections;

public class SBDrinker : SBEntity {	
	public SBSittableComponent currentSittableComponent;
	public bool isBeingControlledBySittableComponent = false;
	public bool isBeingControlledByFaceADirectionComponent = false;
	public bool hasDrink = false;
	
	public SBDrinker(string name) : base(name) {
		SBSpriteComponent sc = new SBSpriteComponent("drinker.psd");
		sc.name = string.Format("{0} sprite", this.name);
		AddComponent(sc);
		AddComponent(new SBVelocityComponent());
	}
}