using UnityEngine;
using System.Collections;
using System;

public class SBDrinker : SBEntity {	
	public SBSittableComponent currentSittableComponent;
	public bool isActuallySitting = false;
	public bool isInSitStandTransition = false;
	public bool isInBathroom = false;
	public bool isLeavingBathroom = false;
	public bool isDrinking = false;
	private float drinkAmountInBladder_ = 0;
	public float drinkAmountInBodyButNotBladder = 0;
	public SBDrink currentDrink;
	public event Action<SBDrinker> SignalFinishedDrink;
	public event Action<SBDrinker> SignalBladderChanged;
	public event Action<SBDrinker> SignalPissedHimself;
	public bool isReceivingDrink = false;
	public bool isPunching = false;

	private int drinkCount_ = 0;
	private float totalRotationSinceSatDown = 0;
	
	public SBDrinker(string name) : base(name) {
		SBSpriteComponent sc = new SBSpriteComponent("drinkerIdle.png", true);
		sc.name = string.Format("{0} sprite", this.name);
		
		SBSpriteComponent peeSc = new SBSpriteComponent("pee0.png", true);
		peeSc.name = string.Format("pee sprite component");
		peeSc.sprite.color = Color.yellow;
		peeSc.sprite.isVisible = false;
		peeSc.sprite.alpha = 0.75f;
		AddComponent(peeSc);
		
		AddComponent(sc);
		sc.StartAnimation(WTMain.animationManager.AnimationForName("drinkerWalk"));

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
		SpriteComponent(1).StopAnimation();
		WTAnimation sitAnim = WTMain.animationManager.AnimationForName("drinkerSitTransition");
		SpriteComponent(1).StartAnimation(sitAnim);
		isInSitStandTransition = true;
		if (currentSittableComponent.isSpecial) {
			ProgressBarComponent().progressBar.isVisible = true;
			string soundName = string.Format("drink{0}", tag);
			FSoundManager.PlaySound(soundName);
		}
		else {
			string pourName = string.Format("drinkPour{0}", tag);
			FSoundManager.PlaySound(pourName, 0.2f);
		}
	}
	
	public void Stand() {
		int total360sSinceSat = (int)totalRotationSinceSatDown / 360;
		VelocityComponent().xVelocity = 0;
		VelocityComponent().yVelocity = 0;
		totalRotationSinceSatDown = 0;
		rotatingContainer.rotation -= total360sSinceSat * 360;
		isActuallySitting = false;
		WTAnimation standAnim = WTMain.animationManager.AnimationForName("drinkerStandTransition");
		SpriteComponent(1).StartAnimation(standAnim);
		isInSitStandTransition = true;
		ProgressBarComponent().progressBar.isVisible = false;
	}
	
	public void RotateInChair(float deltaRotation) {
		if (!isActuallySitting) return;
		totalRotationSinceSatDown += deltaRotation;
		rotatingContainer.rotation += deltaRotation;
	}
		
	override public void AnimationDone(WTAnimation animation) {		
		if (animation.name == "pee") {
			SpriteComponent(0).PauseAnimation();
		}
		else if (animation.name == "punch") {
			isPunching = false;
			SpriteComponent(1).StartAnimation(WTMain.animationManager.AnimationForName("drinkerWalk"));
		}
		else {
			SpriteComponent(1).PauseAnimation();	
			if (animation.name == "drinkerStandTransition") {
				SpriteComponent(1).StartAnimation(WTMain.animationManager.AnimationForName("drinkerWalk"));
				isInSitStandTransition = false;
			}
			else if (animation.name == "drinkerSitTransition") {
				isInSitStandTransition = false;
			}
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
		Go.to(currentDrink, 0.25f, new TweenConfig()
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
		//TimerComponent().Restart();
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
		VelocityComponent().RefreshDrunkLeanValues();
		if (SignalFinishedDrink != null) SignalFinishedDrink(this);
	}
	
	override public void HandleUpdate() {
		base.HandleUpdate();

		if (isInSitStandTransition) return;
		
		if (isDrinking) { // is sitting in a special chair
			if (!TimerComponent().isRunning) TimerComponent().Restart();
			currentDrink.percentLeft = 1.0f - (TimerComponent().timer / SBConfig.DRINK_DRINK_TIME);
			ProgressBarComponent().progressBar.percent = currentDrink.percentLeft;
			if (currentDrink.percentLeft <= 0) {
				currentDrink.percentLeft = 0;
				HandleFinishedDrinkingDrink();
			}
		}
		
		else if (!isActuallySitting && !isPunching) { // is walking around
			float curVel = Mathf.Max(Mathf.Abs(VelocityComponent().xVelocity), Mathf.Abs(VelocityComponent().yVelocity));
						
			if (curVel == 0) {
				SpriteComponent(1).currentAnimation.frameDuration = 1000;
				SpriteComponent(1).ResetAnimation();
			}
			else {
				SpriteComponent(1).currentAnimation.frameDuration = (1 - curVel / SBConfig.DRINKER_MAX_VELOCITY) * SpriteComponent(1).currentAnimation.maxFrameDuration;
				if (SpriteComponent(1).currentAnimation.frameDuration < SpriteComponent(1).currentAnimation.minFrameDuration) {
					SpriteComponent(1).currentAnimation.frameDuration = SpriteComponent(1).currentAnimation.minFrameDuration;	
				}
			}
		}
		
		if (!isLeavingBathroom) {
			if (isInBathroom) {
				float drinkTransferQuantity = 1.0f / SBConfig.PEE_TIME * Time.fixedDeltaTime;
				drinkTransferQuantity = Math.Min(drinkTransferQuantity, this.drinkAmountInBladder);
				this.drinkAmountInBladder -= drinkTransferQuantity;
			}
			else if (drinkAmountInBodyButNotBladder > 0 && this.drinkAmountInBladder < SBConfig.MAX_BLADDER_CAPACITY) {
				float drinkTransferQuantity = 1.0f / SBConfig.BLADDER_FILL_TIME * Time.fixedDeltaTime;
				drinkTransferQuantity = Math.Min(drinkTransferQuantity, drinkAmountInBodyButNotBladder);
				drinkAmountInBodyButNotBladder -= drinkTransferQuantity;
				this.drinkAmountInBladder += drinkTransferQuantity;
			}			
		}
	}
	
	public void PunchDrinker(SBDrinker drinker) {
		if (isPunching || isInBathroom || isActuallySitting || isBeingControlledBySittableComponent || isDrinking || isReceivingDrink) return;

		isPunching = true;
		
		//SpriteComponent(1).sprite.element = Futile.atlasManager.GetElementWithName("drinkerPunching.png");
		SpriteComponent(1).StartAnimation(WTMain.animationManager.AnimationForName("punch"));
		
		if (drinker != null) {
			Direction curDir = this.DirectionComponent().GetCurrentDirection();
				
			float amt = 1500;
			
			if (curDir == Direction.Left) {
				drinker.VelocityComponent().xVelocity = -amt;
				this.VelocityComponent().xVelocity = amt;
			}
			
			else if (curDir == Direction.Right) {
				drinker.VelocityComponent().xVelocity = amt;
				this.VelocityComponent().xVelocity = -amt;
			}
			
			else if (curDir == Direction.Down) {
				drinker.VelocityComponent().yVelocity = -amt;
				this.VelocityComponent().yVelocity = amt;
			}
			
			else if (curDir == Direction.Up) {
				drinker.VelocityComponent().yVelocity = amt;
				this.VelocityComponent().yVelocity = -amt;
			}
			
			if (drinker.HasDrink()) {
				FContainer mainContainer = this.container;
				
				SBDrink otherDrinkersDrink = drinker.currentDrink;
				drinker.currentDrink = null;
				Vector2 globalPos = drinker.rotatingContainer.LocalToGlobal(new Vector2(otherDrinkersDrink.x, otherDrinkersDrink.y));
				otherDrinkersDrink.RemoveFromContainer();
				otherDrinkersDrink.x = globalPos.x;
				otherDrinkersDrink.y = globalPos.y;
				otherDrinkersDrink.Spill(true);
				mainContainer.AddChild(otherDrinkersDrink);
				(mainContainer as SBGameScene).RefreshZOrders();
			}
		}
	}
	
	public static void HandleDoneLeavingBathroom(AbstractTween tween) {
		SBDrinker drinker = (tween as Tween).target as SBDrinker;
		drinker.isLeavingBathroom = false;
		drinker.VelocityComponent().xVelocity = 0;
		drinker.VelocityComponent().yVelocity = 0;
	}
		
	public float drinkAmountInBladder {
		get {return drinkAmountInBladder_;}
		set {
			drinkAmountInBladder_ = value;
		
			if (SignalBladderChanged != null) SignalBladderChanged(this);
			if (drinkAmountInBladder_ >= SBConfig.MAX_BLADDER_CAPACITY && SignalPissedHimself != null) {
				SpriteComponent(1).StopAnimation();
				SpriteComponent(0).sprite.isVisible = true;
				SpriteComponent(0).StartAnimation(WTMain.animationManager.AnimationForName("pee"));
				SignalPissedHimself(this);
			}
		}
	}
	
	public int drinkCount {
		get {return drinkCount_;}
	}
}