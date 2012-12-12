using UnityEngine;
using System.Collections;

public class SBSittableComponent : SBAbstractComponent {
	public SBDrinker currentDrinker;

	public SBSittableComponent() {
		name = "sittable component";
		componentType = ComponentType.Sittable;
	}
	
	public void SeatDrinker(SBDrinker drinker) {
		if (currentDrinker != null) Debug.Log("you tried to sit on me you fucking piece of shit fucking cunthole!");
		
		currentDrinker = drinker;
		currentDrinker.currentSittableComponent = this;
		currentDrinker.isBeingControlledBySittableComponent = true;
		
		Go.to(currentDrinker, 0.3f, new TweenConfig()
			.floatProp("x", this.owner.SpriteComponent().GetGlobalPosition().x)
			.floatProp("y", this.owner.SpriteComponent().GetGlobalPosition().y)
			.onComplete(HandleDrinkerFinishedSittingDown));
	}
	
	public void HandleDrinkerFinishedSittingDown(AbstractTween tween) {
	
	}
	
	public void EjectDrinker() {
		currentDrinker.currentSittableComponent = null;
		currentDrinker.isBeingControlledBySittableComponent = false;
		currentDrinker = null;
	}
}
