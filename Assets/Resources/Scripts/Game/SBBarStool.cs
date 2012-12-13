using UnityEngine;
using System.Collections;

public class SBBarStool : SBEntity {	
	SBTimerComponent drinkWaitTimer;
	
	public SBBarStool(string name, Color color) : base(name) {
		this.name = name;
		SBSpriteComponent sc = new SBSpriteComponent("barStool.psd", false);
		sc.name = string.Format("{0} sprite", this.name);
		sc.sprite.color = color;
		AddComponent(sc);
		AddComponent(new SBSittableComponent());
		drinkWaitTimer = new SBTimerComponent();
		AddComponent(drinkWaitTimer);
		AddComponent(new SBProgressBarComponent(0, 0, 65f, 10f, Color.green, ProgressBarType.FillLeftToRight));
	}
	
	public Rect GetGlobalSitTriggerRect() {
		return SpriteComponent().GetGlobalRect().CloneWithExpansion(15f);
	}
	
	override public void HandleUpdate() {
		if (SittableComponent().drinkerIsActuallySitting) {
			ProgressBarComponent().progressBar.percent = (SBConfig.DRINK_WAIT_TIME - drinkWaitTimer.timer) / SBConfig.DRINK_WAIT_TIME;
		}
	}
}