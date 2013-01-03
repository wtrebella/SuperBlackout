using UnityEngine;
using System.Collections;

public class SBTitleScene : FStage {
	FSprite super;
	FSprite blackout;
	
	bool introIsDone = false;
	
	TweenChain tweenChain1;
	TweenChain tweenChain2;
	
	public SBTitleScene() : base("") {
		FSprite background = new FSprite("splashBackground.png");
		background.x = Futile.screen.halfWidth;
		background.y = Futile.screen.halfHeight;
		AddChild(background);
		
		float amt = 700f;
		
		super = new FSprite("super.png");
		super.data = 0;
		super.x = Futile.screen.halfWidth - amt;
		super.y = Futile.screen.halfHeight - amt;
		
		blackout = new FSprite("blackout.png");
		blackout.data = 1;
		blackout.x = Futile.screen.halfHeight - amt;
		blackout.y = Futile.screen.halfHeight - amt;
		
		AddChild(blackout);
		AddChild(super);
		
		EaseType easeType = EaseType.BounceOut;
		float duration = 0.8f;
		
		Go.to(super, duration, new TweenConfig()
			.floatProp("x", Futile.screen.halfWidth - 5f)
			.floatProp("y", Futile.screen.halfHeight - 5f)
			.setEaseType(easeType)
			.onComplete(HandleWordFinishedComingIn));
		
		Go.to(blackout, duration, new TweenConfig()
			.setDelay(duration)
			.floatProp("x", Futile.screen.halfWidth - 5f)
			.floatProp("y", Futile.screen.halfHeight - 5f)
			.setEaseType(easeType)
			.onComplete(HandleWordFinishedComingIn));
	}
	
	public void HandleWordFinishedComingIn(AbstractTween tween) {
		FSprite word = (tween as Tween).target as FSprite;
		ShakeWord(word);
		if ((int)word.data == 0) {
		}
		else if ((int)word.data == 1) {
			introIsDone = true;
		}
	}
	
	public void HandleWordFinishedShake(AbstractTween tween) {
		FSprite word = (tween as Tween).target as FSprite;
		ShakeWord(word);
	}
	
	public void DismissWord(FSprite word) {
		Go.killAllTweensWithTarget(word);
		
		if ((int)word.data == 0) {
			tweenChain1.destroy();
		}
		else if ((int)word.data == 1) {
			tweenChain2.destroy();
		}
		
		float duration = 0.7f;
		float amt = 800f;
		EaseType easeType = EaseType.ExpoOut;
		
		Go.to(word, duration, new TweenConfig()
			.floatProp("x", Futile.screen.halfWidth - amt)
			.floatProp("y", Futile.screen.halfHeight - amt)
			.setEaseType(easeType));
	}
	
	public void ShakeWord(FSprite word) {
		/*float dur = Random.Range(0.05f, 0.15f);
		float xDelta = Random.Range(-5f, 5f);
		float yDelta = Random.Range(-5f, 5f);*/
		
		float dur = 0.3f;
		float xDelta = -40f;
		float yDelta = -40f;
		
		Tween tweenIn = new Tween(word, dur, new TweenConfig().floatProp("x", xDelta, true).floatProp("y", yDelta, true));
		Tween tweenOut = new Tween(word, dur, new TweenConfig().floatProp("x", -xDelta, true).floatProp("y", -yDelta, true).onComplete(HandleWordFinishedShake));
		
		TweenChain tweenChain = new TweenChain();
		tweenChain.append(tweenIn);
		tweenChain.append(tweenOut);
		
		if ((int)word.data == 0) {
			tweenChain1 = tweenChain;
		}
		else if ((int)word.data == 1) {
			tweenChain2 = tweenChain;
		}
		
		Go.addTween(tweenChain);
		
		tweenChain.play();
	}
	
	override public void HandleAddedToStage() {
		base.HandleAddedToStage();
		Futile.instance.SignalUpdate += HandleUpdate;
	}
	
	override public void HandleRemovedFromStage() {
		base.HandleRemovedFromStage();
		Futile.instance.SignalUpdate -= HandleUpdate;
	}
	
	public void HandleUpdate() {
		if (!introIsDone) return;
				
		if (Input.GetKeyDown(KeyCode.Alpha1)) DismissWord(super);
		if (Input.GetKeyDown(KeyCode.Alpha2)) DismissWord(blackout);
	}
}
