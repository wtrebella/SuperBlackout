using UnityEngine;
using System.Collections;

public class SBBarStool : SBEntity {	
	public SBBarStool(string name, Color color) : base(name) {
		this.name = name;
		SBSpriteComponent sc = new SBSpriteComponent("barStool.psd", false);
		sc.name = string.Format("{0} sprite", this.name);
		sc.sprite.color = color;
		AddComponent(sc);
		AddComponent(new SBSittableComponent());
		AddComponent(new SBTimerComponent());
		AddComponent(new SBProgressBarComponent(0, 0, 65f, 10f, Color.green, ProgressBarType.FillLeftToRight));
	}
	
	public Rect GetGlobalSitTriggerRect() {
		return SpriteComponent().GetGlobalRect().CloneWithExpansion(15f);
	}
	
	override public void HandleUpdate() {
		if (ProgressBarComponent().progressBar.isVisible) {
			ProgressBarComponent().progressBar.percent = (2.0f - TimerComponent().timer) / 2.0f;
		}
	}
}