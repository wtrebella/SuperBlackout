using UnityEngine;
using System.Collections;

public class SBDrink : SBEntity {
	public float percentLeft = 1;
	
	public SBDrink(string name) : base(name) {
		SBSpriteComponent sc = new SBSpriteComponent("drink.psd", false);
		sc.name = string.Format("{0} sprite", this.name);
		AddComponent(sc);
	}
}
