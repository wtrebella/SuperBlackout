using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SBBar : SBEntity {
	public List<SBBarStool> barStools;
	
	public SBBar() : base("bar") {
		SBSpriteComponent sc = new SBSpriteComponent("bar.psd", false);
		sc.name = "bar sprite component 1";
		sc.sprite.color = new Color(0.3f, 0.5f, 1.0f, 1.0f);
		sc.sprite.x = Futile.screen.width - SBConfig.BORDER_WIDTH;
		sc.sprite.y = SBConfig.BORDER_WIDTH;
		AddComponent(sc);
		
		sc = new SBSpriteComponent("bar.psd", false);
		sc.name = "bar sprite component 2";
		sc.sprite.color = new Color(1.0f, 0.3f, 0.5f, 1.0f);
		sc.sprite.x = SBConfig.BORDER_WIDTH;
		sc.sprite.y = SBConfig.BORDER_WIDTH;
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
		
		/*for (int i = 0; i < 8; i++) {
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
		}*/
		
		SBBarStool barStool = new SBBarStool(string.Format("barStool1"), new Color(0.3f, 0.5f, 1.0f, 1.0f));
		barStool.ProgressBarComponent().progressBar.y += 45f;
		barStool.ProgressBarComponent().progressBar.isVisible = false;
		barStool.x = Futile.screen.width - 135f;
		barStool.y = 135f;
		barStool.tag = 1;
		barStools.Add(barStool);
		AddChild(barStool);
		
		barStool = new SBBarStool(string.Format("barStool2"), new Color(1.0f, 0.3f, 0.5f, 1.0f));
		barStool.ProgressBarComponent().progressBar.y += 45f;
		barStool.ProgressBarComponent().progressBar.isVisible = false;
		barStool.x = 135f;
		barStool.y = 135f;
		barStool.tag = 2;
		barStools.Add(barStool);
		AddChild(barStool);
	}
	
	override public void HandleUpdate() {
		foreach (SBBarStool barStool in barStools) {
			barStool.HandleUpdate();
			if (barStool.SittableComponent().currentDrinker != null && !barStool.SittableComponent().currentDrinker.HasDrink() && barStool.TimerComponent().timer > SBConfig.DRINK_WAIT_TIME) {
				SBDrink drink = new SBDrink("drink");
				if (barStool.tag == 1) {
					drink.x = Futile.screen.width - SBConfig.BORDER_WIDTH;
					drink.y = SBConfig.BORDER_WIDTH;
				}
				else if (barStool.tag == 2) {
					drink.x = SBConfig.BORDER_WIDTH;
					drink.y = SBConfig.BORDER_WIDTH;
				}
				AddChild(drink);
				barStool.SittableComponent().currentDrinker.TakeDrink(drink);
			}
		}
	}
}
