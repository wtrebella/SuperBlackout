using UnityEngine;
using System.Collections;

public class SBHudLayer : FContainer {
	//FLabel drinkCountLabel1;
	//FLabel drinkCountLabel2;
	
	SBBladderBar bladderBar1;
	SBBladderBar bladderBar2;
	
	SBDrinkCounter drinkCounter1;
	SBDrinkCounter drinkCounter2;
	
	public SBHudLayer() {
		/*drinkCountLabel1.scale = drinkCountLabel2.scale = 0.75f;
		drinkCountLabel1 = new FLabel("Silkscreen", "Drinks: 0");
		drinkCountLabel2 = new FLabel("Silkscreen", "Drinks: 0");
		drinkCountLabel1.anchorX = 0;
		drinkCountLabel2.anchorX = 1;
		drinkCountLabel1.x = xPadding;
		drinkCountLabel2.x = Futile.screen.width - xPadding - extraRightSideXPadding;
		drinkCountLabel1.y = drinkCountLabel2.y = Futile.screen.height - SBConfig.TOP_UI_HEIGHT / 2f + yPadding;
		drinkCountLabel1.color = new Color(0.3f, 0.5f, 1.0f, 1.0f);
		drinkCountLabel2.color = new Color(1.0f, 0.3f, 0.5f, 1.0f);
		AddChild(drinkCountLabel1);
		AddChild(drinkCountLabel2);*/
		
		float xPadding = 12f;
		float extraRightSideXPadding = 10f;
		float yPadding = 4f;
		float bladderBarEdgeThickness = 8f;
		
		drinkCounter1 = new SBDrinkCounter(SBConfig.DRINKS_TO_WIN);
		drinkCounter2 = new SBDrinkCounter(SBConfig.DRINKS_TO_WIN);
		drinkCounter1.x = xPadding;
		drinkCounter2.x = Futile.screen.width - xPadding - drinkCounter2.GetWidth();
		drinkCounter1.y = drinkCounter2.y = Futile.screen.height - SBConfig.TOP_UI_HEIGHT / 2f;
		
		bladderBar1 = new SBBladderBar(1);
		bladderBar2 = new SBBladderBar(2);
		bladderBar1.x = Futile.screen.halfWidth - 450f;
		bladderBar2.x = Futile.screen.halfWidth + 450f;
		bladderBar1.y = bladderBar2.y = Futile.screen.height - SBConfig.TOP_UI_HEIGHT / 2f;
		
		AddChild(bladderBar1);
		AddChild(bladderBar2);
		AddChild(drinkCounter1);
		AddChild(drinkCounter2);
	}
	
	public void HandleDrinkerFinishedDrink(SBDrinker drinker) {
		if (drinker.tag == 1) {
			drinkCounter1.drinkCount = drinker.drinkCount;
		}
		
		else if (drinker.tag == 2) {
			drinkCounter2.drinkCount = drinker.drinkCount;
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
