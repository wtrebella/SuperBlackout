using UnityEngine;
using System.Collections;

public class SBDrinker : SBEntity {	
	public SBSittableComponent currentSittableComponent;
	public bool isActuallySitting = false;
	public bool hasDrink = false;
	
	public SBDrinker(string name) : base(name) {		
		SBSpriteComponent sc = new SBSpriteComponent("drinkerIdle.png", true);
		sc.name = string.Format("{0} sprite", this.name);
		WTMain.animationManager.AddAnimation("drinkerWalk", new string[] {"drinkerIdle.png", "drinkerLeftFront.png", "drinkerIdle.png", "drinkerRightFront.png"}, 0.05f, 0.4f);
		sc.StartAnimation(WTMain.animationManager.AnimationForName("drinkerWalk"));
		AddComponent(sc);
		AddComponent(new SBProgressBarComponent(0, 45f, 65f, 10f, Color.green, ProgressBarType.FillLeftToRight));
		AddComponent(new SBTimerComponent());
		AddComponent(new SBDirectionComponent());
		AddComponent(new SBVelocityComponent());
		
		SBDrink drink = new SBDrink("drink");
		drink.x = 20f;
		drink.y = -40f;
		rotatingContainer.AddChild(drink);
	}
	
	public void Sit() {
		isActuallySitting = true;
		SpriteComponent().StopAnimation();
		SpriteComponent().sprite.element = Futile.atlasManager.GetElementWithName("drinkerSitting.png");
	}
	
	public void Stand() {
		isActuallySitting = false;
		SpriteComponent().RestartAnimation();
	}
}