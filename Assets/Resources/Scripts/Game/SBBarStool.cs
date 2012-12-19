using UnityEngine;
using System.Collections;

public class SBBarStool : SBEntity {		
	public SBBarStool(string name, Color color) : base(name) {
		this.name = name;
		SBSpriteComponent sc = new SBSpriteComponent("barStool.psd", true);
		sc.name = string.Format("{0} sprite", this.name);
		sc.sprite.color = color;
		AddComponent(sc);
		rotatingContainer.rotation = Random.Range(0, 360);
		AddComponent(new SBSittableComponent());
		AddComponent(new SBTimerComponent());
		AddComponent(new SBProgressBarComponent(0, 0, 65f, 10f, Color.green, ProgressBarType.FillLeftToRight));
	}
	
	public Rect GetGlobalSitTriggerRect() {
		return SpriteComponent().GetGlobalRect().CloneWithExpansion(15f);
	}
	
	override public void HandleUpdate() {
		base.HandleUpdate();
		
		if (SittableComponent().currentDrinker == null) return;
		
		if (SittableComponent().currentDrinker.isActuallySitting) {
			ProgressBarComponent().progressBar.percent = (SBConfig.DRINK_WAIT_TIME - TimerComponent().timer) / SBConfig.DRINK_WAIT_TIME;
		}
	}
}