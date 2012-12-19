using UnityEngine;
using System.Collections;

public class SBDrinker : SBEntity, AnimationInterface {	
	public SBSittableComponent currentSittableComponent;
	public bool isActuallySitting = false;
	public bool isInSitStandTransition = false;
	public bool hasDrink = false;
	public SBDrink currentDrink;

	private float totalRotationSinceSatDown = 0;
	
	public SBDrinker(string name) : base(name) {		
		SBSpriteComponent sc = new SBSpriteComponent("drinkerIdle.png", true);
		sc.name = string.Format("{0} sprite", this.name);
		
		WTMain.animationManager.AddAnimation("drinkerWalk", new string[] {
			"drinkerIdle.png",
			"drinkerLeftFront.png",
			"drinkerIdle.png",
			"drinkerRightFront.png"}, 0.05f, 0.4f, true);
		
		WTMain.animationManager.AddAnimation("drinkerSitTransition", new string[] {
			"drinkerIdle.png",
			"drinkerSittingTrans0.png",
			"drinkerSittingTrans1.png",
			"drinkerSitting.png"}, 0.05f, false);
		
		WTMain.animationManager.AddAnimation("drinkerStandTransition", new string[] {
			"drinkerSitting.png",
			"drinkerSittingTrans1.png",
			"drinkerSittingTrans0.png",
			"drinkerIdle.png"}, 0.05f, false);
		
		sc.StartAnimation(WTMain.animationManager.AnimationForName("drinkerWalk"));
		AddComponent(sc);
		AddComponent(new SBProgressBarComponent(0, 0, 65f, 10f, Color.green, ProgressBarType.FillLeftToRight));
		AddComponent(new SBTimerComponent());
		AddComponent(new SBDirectionComponent());
		AddComponent(new SBVelocityComponent());
	}
	
	public void Sit() {
		isActuallySitting = true;
		SpriteComponent().StopAnimation();
		WTAnimation sitAnim = WTMain.animationManager.AnimationForName("drinkerSitTransition");
		sitAnim.animationDelegate = this;
		SpriteComponent().StartAnimation(sitAnim);
		isInSitStandTransition = true;
		if (currentSittableComponent.isSpecial) ProgressBarComponent().progressBar.isVisible = true;
	}
	
	public void Stand() {
		int total360sSinceSat = (int)totalRotationSinceSatDown / 360;
		VelocityComponent().xVelocity = 0;
		VelocityComponent().yVelocity = 0;
		totalRotationSinceSatDown = 0;
		rotatingContainer.rotation -= total360sSinceSat * 360;
		isActuallySitting = false;
		WTAnimation standAnim = WTMain.animationManager.AnimationForName("drinkerStandTransition");
		standAnim.animationDelegate = this;
		SpriteComponent().StartAnimation(standAnim);
		isInSitStandTransition = true;
		ProgressBarComponent().progressBar.isVisible = false;
	}
	
	public void RotateInChair(float deltaRotation) {
		if (!isActuallySitting) return;
		totalRotationSinceSatDown += deltaRotation;
		rotatingContainer.rotation += deltaRotation;
	}
		
	public void AnimationDone(WTAnimation animation) {
		SpriteComponent().PauseAnimation();	
		if (animation.name == "drinkerStandTransition") {
			SpriteComponent().StartAnimation(WTMain.animationManager.AnimationForName("drinkerWalk"));
			isInSitStandTransition = false;
		}
		else if (animation.name == "drinkerSitTransition") {
			isInSitStandTransition = false;
		}
	}
	
	public void TakeDrink(SBDrink drink) {
		hasDrink = true;
		currentDrink = drink;
		currentDrink.x = 20f;
		currentDrink.y = -40f;
		rotatingContainer.AddChild(currentDrink);
	}
	
	public void DrinkDrink() {
		hasDrink = false;
		rotatingContainer.RemoveChild(currentDrink);
	}
	
	override public void HandleUpdate() {
		if (!isActuallySitting && !isInSitStandTransition) {
			float curVel = Mathf.Max(Mathf.Abs(VelocityComponent().xVelocity), Mathf.Abs(VelocityComponent().yVelocity));
	
			if (curVel == 0) {
				SpriteComponent().currentAnimation.frameDuration = 1000;
				SpriteComponent().ResetAnimation();
			}
			else {
				SpriteComponent().currentAnimation.frameDuration = (1 - curVel / SBConfig.DRINKER_MAX_VELOCITY) * SpriteComponent().currentAnimation.maxFrameDuration;
				if (SpriteComponent().currentAnimation.frameDuration < SpriteComponent().currentAnimation.minFrameDuration) {
					SpriteComponent().currentAnimation.frameDuration = SpriteComponent().currentAnimation.minFrameDuration;	
				}
			}
		}
		
		base.HandleUpdate();
	}
}