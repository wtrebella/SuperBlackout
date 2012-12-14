using UnityEngine;
using System.Collections;

public class SBDrinker : SBEntity {	
	public SBSittableComponent currentSittableComponent;
	public bool isActuallySitting = false;
	public bool hasDrink = false;
	public bool isWalking = true;
	
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
	
	override public void HandleUpdate() {
		base.HandleUpdate();
		
		if (!isWalking) return;
		
		float curVel = Mathf.Max(Mathf.Abs(VelocityComponent().xVelocity), Mathf.Abs(VelocityComponent().yVelocity));
		if (curVel == 0 && SpriteComponent().isAnimating) SpriteComponent().StopAnimation();
		else {
			if (!SpriteComponent().isAnimating) SpriteComponent().StartAnimation();
			SpriteComponent().frameDuration = (1 - curVel / SBConfig.DRINKER_MAX_VELOCITY) * SBConfig.DRINKER_MAX_FRAME_DURATION;
		}
		if (SpriteComponent().frameDuration < SBConfig.DRINKER_MIN_FRAME_DURATION) {
			SpriteComponent().frameDuration = SBConfig.DRINKER_MIN_FRAME_DURATION;	
		}
	}
	
	public void Sit() {
		isActuallySitting = true;
		SpriteComponent().StopAnimation();
		SpriteComponent().sprite.element = Futile.atlasManager.GetElementWithName("drinkerSitting.png");
	}
	
	public void Stand() {
		isActuallySitting = false;
		currentSittableComponent = null;
		isBeingControlledBySittableComponent = false;
		SpriteComponent().RestartAnimation();
	}
}