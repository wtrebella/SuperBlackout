using UnityEngine;
using System.Collections;
using System;

public class SBDrinker : SBEntity, AnimationInterface {	
	public SBSittableComponent currentSittableComponent;
	public bool isActuallySitting = false;
	public bool isInSitStandTransition = false;
	public bool isDrinking = false;
	public float drinkAmountInBladder = 0;
	public float drinkAmountInBodyButNotBladder = 0;
	public SBDrink currentDrink;
	public event Action<SBDrinker> SignalFinishedDrink;
	public bool isReceivingDrink = false;
	
	private int drinkCount_ = 0;
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
	
	public bool HasDrink() {
		return currentDrink != null;	
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
		if (HasDrink()) {
			Debug.Log("already have drink, can't take another until you finish the first! what are you, a drunk?");
			return;
		}
		
		isReceivingDrink = true;
		currentDrink = drink;
		Vector2 newPos = rotatingContainer.LocalToLocal(currentDrink.container, new Vector2(currentDrink.x, currentDrink.y));
		currentDrink.RemoveFromContainer();
		currentDrink.x = newPos.x;
		currentDrink.y = newPos.y;
		rotatingContainer.AddChild(currentDrink);
		Go.to(currentDrink, 0.5f, new TweenConfig()
			.floatProp("x", 20f)
			.floatProp("y", -40f)
			.onComplete(HandleActuallyReceivedDrink));
	}
	
	public void HandleActuallyReceivedDrink(AbstractTween tween) {
		currentSittableComponent.EjectDrinker();
		isReceivingDrink = false;
	}
	
	public void StartDrinkingDrink() {
		if (!HasDrink()) {
			Debug.Log("doesn't have a drink to drink!");
			return;
		}
		
		ProgressBarComponent().progressBar.percent = 1;
		isDrinking = true;
		TimerComponent().Restart();
		Go.to(currentDrink, 0.8f, new TweenConfig()
			.floatProp("x", 23f)
			.floatProp("y", -10f));
	}
	
	public void HandleFinishedDrinkingDrink() {
		drinkCount_++;
		
		Vector2 newPos = rotatingContainer.LocalToGlobal(new Vector2(currentDrink.x, currentDrink.y));
		rotatingContainer.RemoveChild(currentDrink);
		currentDrink.x = newPos.x;
		currentDrink.y = newPos.y;
		WTMain.currentScene.AddChild(currentDrink);
		Vector2 goalPos = SBConfig.EmptyGlassPosition(drinkCount_, tag);
		Go.to(currentDrink, 0.5f, new TweenConfig()
			.floatProp("x", goalPos.x)
			.floatProp("y", goalPos.y));
		
		ProgressBarComponent().progressBar.isVisible = false;
		currentDrink = null;
		isDrinking = false;
		currentSittableComponent.EjectDrinker();
		drinkAmountInBodyButNotBladder += 1;
		TimerComponent().Stop();
		if (SignalFinishedDrink != null) SignalFinishedDrink(this);
	}
	
	override public void HandleUpdate() {
		base.HandleUpdate();

		if (isInSitStandTransition) return;
		
		if (isDrinking) { // is sitting in a special chair
			currentDrink.percentLeft = 1.0f - (TimerComponent().timer / SBConfig.DRINK_DRINK_TIME);
			ProgressBarComponent().progressBar.percent = currentDrink.percentLeft;
			if (currentDrink.percentLeft <= 0) {
				currentDrink.percentLeft = 0;
				HandleFinishedDrinkingDrink();
			}
		}
		
		else if (!isActuallySitting) { // is walking around
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
			
			if (drinkAmountInBodyButNotBladder > 0) {
				float drinkTransferQuantity = SBConfig.BLADDER_FILL_CONSTANT * Time.fixedDeltaTime;
				drinkTransferQuantity = Math.Min(drinkTransferQuantity, drinkAmountInBodyButNotBladder);
				drinkAmountInBodyButNotBladder -= drinkTransferQuantity;
				drinkAmountInBladder += drinkTransferQuantity;
			}
		}
	}
	
	public int drinkCount {
		get {return drinkCount_;}
		/*set {
			drinkCount_ = value;
			if (SignalFinishedDrink != null) SignalFinishedDrink(this);
		}*/
	}
}