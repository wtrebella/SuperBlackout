using UnityEngine;
using System.Collections;

public class SBDrinker : SBEntity {	
	public SBSittableComponent currentSittableComponent;
	public bool hasDrink = false;
	
	public SBDrinker(string name) : base(name) {
		SBSpriteComponent sc = new SBSpriteComponent("drinkerIdle.png", true, new string[] {"drinkerIdle.png", "drinkerLeftFront.png", "drinkerIdle.png", "drinkerRightFront.png"}, 0.2f);
		sc.name = string.Format("{0} sprite", this.name);
		sc.StartAnimation();
		AddComponent(sc);
		AddComponent(new SBProgressBarComponent(0, 45f, 65f, 10f, Color.green, ProgressBarType.FillLeftToRight));
		AddComponent(new SBTimerComponent());
		AddComponent(new SBDirectionComponent());
		AddComponent(new SBVelocityComponent());
	}
}