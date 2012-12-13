using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SBBar : SBEntity {
	public List<SBBarStool> barStools;
	
	public SBBar() : base("bar") {
		SBSpriteComponent sc = new SBSpriteComponent("bar.psd", false);
		sc.name = "bar sprite component";
		AddComponent(sc);
		
		SBCollideComponent cc = new SBCollideComponent();
		cc.name = "bar collide component";
		AddComponent(cc);
				
		InitBarStools();
	}
	
	public SBBarStool BarStoolThatIntersectsWithGlobalRect(Rect otherRect) {
		foreach (SBBarStool barStool in barStools) {
			if (barStool.GetGlobalSitTriggerRect().CheckIntersect(otherRect)) {
				return barStool;
			}
		}
		
		return null;
	}
	
	private void InitBarStools() {
		barStools = new List<SBBarStool>();
		
		float barStoolMargin = 150f;
		
		for (int i = 0; i < 8; i++) {
			SBBarStool barStool = new SBBarStool(string.Format("barStool{0}", i), new Color(165f/255f, 115f/255f, 67f/255f));
			if (i == 5 || i == 6 || i == 7) {
				barStool.ProgressBarComponent().progressBar.y -= 45f;	
			}
			else barStool.ProgressBarComponent().progressBar.y += 45f;
			barStool.ProgressBarComponent().progressBar.isVisible = false;
			barStool.x = barStoolMargin * Mathf.Cos(i * 45 * Mathf.Deg2Rad);
			barStool.y = barStoolMargin * Mathf.Sin(i * 45 * Mathf.Deg2Rad);
			barStools.Add(barStool);
			AddChild(barStool);
		}
	}
	
	override public void HandleUpdate() {
		foreach (SBBarStool barStool in barStools) {
			barStool.HandleUpdate();
		}
	}
}
