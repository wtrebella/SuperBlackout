using UnityEngine;
using System.Collections;

public class SBSittableComponent : SBAbstractComponent {
	public SBDrinker currentDrinker;
	public bool isSpecial;

	public SBSittableComponent(bool isSpecial = false) {
		this.isSpecial = isSpecial;
		name = "sittable component";
		componentType = ComponentType.Sittable;
	}
	
	public void SeatDrinker(SBDrinker drinker) {
		if (currentDrinker != null) Debug.Log("you tried to sit on me you fucking piece of shit fucking cunthole!");

		currentDrinker = drinker;
		currentDrinker.VelocityComponent().Reset();
		currentDrinker.currentSittableComponent = this;
		currentDrinker.isBeingControlledBySittableComponent = true;
		
		Go.to(currentDrinker, 0.3f, new TweenConfig()
			.floatProp("x", this.owner.SpriteComponent(0).GetGlobalPosition().x)
			.floatProp("y", this.owner.SpriteComponent(0).GetGlobalPosition().y)
			.onComplete(HandleDrinkerFinishedSittingDown));
	}
	
	public bool CanSeatDrinker(SBDrinker drinker) {
		bool canSit = currentDrinker == null && !drinker.isBeingControlledBySittableComponent;
		if (isSpecial) canSit = canSit && owner.tag == drinker.tag;
		
		return canSit;
	}
	
	public void HandleDrinkerFinishedSittingDown(AbstractTween tween) {
		if (!isSpecial) { // only use the timer when they're waiting at the bar, not their own chair
			owner.ProgressBarComponent().progressBar.isVisible = true;
			owner.ProgressBarComponent().progressBar.percent = 1;
			owner.TimerComponent().Restart();
		}
		
		currentDrinker.Sit();
	}
	
	public void EjectDrinker() {
		owner.ProgressBarComponent().progressBar.isVisible = false;
		owner.TimerComponent().Stop();
		currentDrinker.currentSittableComponent = null;
		currentDrinker.isBeingControlledBySittableComponent = false;
		currentDrinker.Stand();
		currentDrinker = null;
	}
}
