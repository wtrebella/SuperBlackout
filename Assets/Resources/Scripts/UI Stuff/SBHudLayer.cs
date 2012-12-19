using UnityEngine;
using System.Collections;

public class SBHudLayer : FContainer {
	FLabel drinkCountLabel1;
	FLabel drinkCountLabel2;
	
	public SBHudLayer() {
		drinkCountLabel1 = new FLabel("Silkscreen", "Drinks: 0");
		drinkCountLabel2 = new FLabel("Silkscreen", "Drinks: 0");
		
		drinkCountLabel1.scale = drinkCountLabel2.scale = 0.75f;
		
		drinkCountLabel1.anchorX = 0;
		drinkCountLabel2.anchorX = 1;
		
		float xPadding = 12f;
		float yPadding = 4f;
		drinkCountLabel1.x = xPadding;
		drinkCountLabel2.x = Futile.screen.width - xPadding - 10f;
		
		drinkCountLabel1.y = drinkCountLabel2.y = Futile.screen.height - SBConfig.TOP_UI_HEIGHT / 2f + yPadding;
		
		drinkCountLabel1.color = new Color(0.3f, 0.5f, 1.0f, 1.0f);
		drinkCountLabel2.color = new Color(1.0f, 0.3f, 0.5f, 1.0f);
		
		AddChild(drinkCountLabel1);
		AddChild(drinkCountLabel2);
	}
}
