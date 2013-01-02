using UnityEngine;
using System.Collections;

public class SBDrink : SBEntity {
	private float percentLeft_ = 1;
	
	public SBDrink(string name) : base(name) {
		SBSpriteComponent sc = new SBSpriteComponent("glass.psd", false);
		sc.name = string.Format("glass sprite", this.name);
		AddComponent(sc);
		
		sc = new SBSpriteComponent("drink.psd", false);
		sc.name = string.Format("liquid sprite", this.name);
		AddComponent(sc);
	}
	
	public float percentLeft {
		get {return percentLeft_;}
		set {
			percentLeft_ = value;
			if (percentLeft_ < 0) percentLeft_ = 0;
			if (percentLeft_ > 1) percentLeft_ = 1;
			SpriteComponent(1).sprite.alpha = percentLeft_;
		}
	}
	
	public void Spill() {
		RemoveComponent(SpriteComponent(0));
		RemoveComponent(SpriteComponent(1));
		
		SBSpriteComponent sc = new SBSpriteComponent("glassSpill0.png", false);
		sc.name = string.Format("spill sprite", this.name);
		sc.sprite.anchorX = 0;
		sc.sprite.rotation = Random.Range(0, 359);
		AddComponent(sc);
		
		sc.StartAnimation(WTMain.animationManager.AnimationForName("drinkSpill"));
		
		Futile.instance.SignalUpdate += sc.HandleUpdate;
	}
	
	override public void AnimationDone(WTAnimation animation) {
		Futile.instance.SignalUpdate -= SpriteComponent(0).HandleUpdate;	
	}
}
