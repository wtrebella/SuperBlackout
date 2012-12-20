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
		
		bladderBar1 = new SBBladderBar();
		bladderBar2 = new SBBladderBar();
		
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
	}
	
	public void HandleDrinkerFinishedDrink(SBDrinker drinker) {
		if (drinker.tag == 1) {
			drinkCountLabel1.text = string.Format("Drinks: {0}", drinker.drinkCount);
		}
		
		else if (drinker.tag == 2) {
			drinkCountLabel2.text = string.Format("Drinks: {0}", drinker.drinkCount);
		}
	}
}
