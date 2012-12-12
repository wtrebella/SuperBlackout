using UnityEngine;
using System.Collections;

public class SBBarStool : SBEntity {	
	public SBBarStool(string name, Color color) : base(name) {
		this.name = name;
		SBSpriteComponent sc = new SBSpriteComponent("barStool.psd");
		sc.name = string.Format("{0} sprite", this.name);
		sc.sprite.color = color;
		AddComponent(sc);
		AddComponent(new SBSittableComponent());
	}
	
	public Rect GetGlobalSitTriggerRect() {
		return SpriteComponent().GetGlobalRect().CloneWithExpansion(15f);
	}
}
