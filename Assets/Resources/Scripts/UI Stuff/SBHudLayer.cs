using UnityEngine;
using System.Collections;

public class SBHudLayer : FContainer {
	FLabel drinkCountLabel1;
	FLabel drinkCountLabel2;
	
	SBBladderBar bladderBar1;
	SBBladderBar bladderBar2;
	
	public SBHudLayer() {
		drinkCountLabel1 = new FLabel("Silkscreen", "Drinks: 0");
		drinkCountLabel2 = new FLabel("Silkscreen", "Drinks: 0");
		
		bladderBar1 = new SBBladderBar(1);
		bladderBar2 = new SBBladderBar(2);
		
		drinkCountLabel1.scale = drinkCountLabel2.scale = 0.75f;
		
		drinkCountLabel1.anchorX = 0;
		drinkCountLabel2.anchorX = 1;
		
		float xPadding = 12f;
		float extraRightSideXPadding = 10f;
		float yPadding = 4f;
		drinkCountLabel1.x = xPadding;
		drinkCountLabel2.x = Futile.screen.width - xPadding - extraRightSideXPadding;
		
		drinkCountLabel1.y = drinkCountLabel2.y = Futile.screen.height - SBConfig.TOP_UI_HEIGHT / 2f + yPadding;
		
		float bladderBarEdgeThickness = 8f;
		bladderBar1.x = Futile.screen.halfWidth - 450f;// 2 * xPadding + drinkCountLabel1.textRect.width + bladderBarEdgeThickness + bladderBar1.maxWidth / 2f;
		bladderBar2.x = Futile.screen.halfWidth + 450f;//Futile.screen.width - 2 * xPadding - drinkCountLabel2.textRect.width - 2 * bladderBarEdgeThickness - bladderBar2.maxWidth - extraRightSideXPadding + bladderBar2.maxWidth / 2f;
		
		bladderBar1.y = bladderBar2.y = Futile.screen.height - SBConfig.TOP_UI_HEIGHT / 2f;
		
		drinkCountLabel1.color = new Color(0.3f, 0.5f, 1.0f, 1.0f);
		drinkCountLabel2.color = new Color(1.0f, 0.3f, 0.5f, 1.0f);
		
		AddChild(drinkCountLabel1);
		AddChild(drinkCountLabel2);
		
		AddChild(bladderBar1);
		AddChild(bladderBar2);
		
		for (int i = 0; i < 7; i++) {
			string name;
			float alpha;
			if (i < 4) {
				name = "uiDrink.psd";
				alpha = 1;
			}
			else {
				name = "uiDrinkBW.psd";
				alpha = 0.5f;
			}
			FSprite drinky = new FSprite(name);
			drinky.x = Futile.screen.halfWidth - 150f + i * 8 * 7;
			drinky.y = Futile.screen.height - SBConfig.TOP_UI_HEIGHT / 2f;
			drinky.alpha = alpha;
			AddChild(drinky);
		}
	}
	
	public void HandleDrinkerFinishedDrink(SBDrinker drinker) {
		if (drinker.tag == 1) {
			drinkCountLabel1.text = string.Format("Drinks: {0}", drinker.drinkCount);
		}
		
		else if (drinker.tag == 2) {
			drinkCountLabel2.text = string.Format("Drinks: {0}", drinker.drinkCount);
		}
	}
	
	public void HandleBladderChanged(SBDrinker drinker) {
		if (drinker.tag == 1) {
			bladderBar1.percent = drinker.drinkAmountInBladder / SBConfig.MAX_BLADDER_CAPACITY;
		}
		
		else if (drinker.tag == 2) {
			bladderBar2.percent = drinker.drinkAmountInBladder / SBConfig.MAX_BLADDER_CAPACITY;	
		}
	}
}
