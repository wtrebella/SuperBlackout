using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SBDrinkCounter : FContainer {
	private List<FSprite> drinks_;
	private int drinkCount_ = 0;
	private float drinkPadding_ = 10f;
	private float drinkWidth_;
	
	public SBDrinkCounter(int maxDrinks) {
		drinks_ = new List<FSprite>();
		
		for (int i = 0; i < maxDrinks; i++) {
			FSprite drinkGreyed = new FSprite("uiDrinkBW.psd");
			drinkGreyed.alpha = 0.5f;
			drinkGreyed.anchorX = 0;
			drinkGreyed.x = i * (drinkGreyed.width + drinkPadding_);
			AddChild(drinkGreyed);
			
			FSprite drink = new FSprite("uiDrink.psd");
			drink.anchorX = 0;
			drink.x = i * (drink.width + drinkPadding_);
			drink.isVisible = false;
			drinks_.Add(drink);
			AddChild(drink);
		}
		
		drinkWidth_ = drinks_[0].width;
	}
	
	public float GetWidth() {
		return SBConfig.DRINKS_TO_WIN * (drinkPadding_ + drinkWidth_) - drinkPadding_;
	}
	
	public int drinkCount {
		get {return drinkCount_;}
		set {
			drinkCount_ = value;
			if (drinkCount_ > SBConfig.DRINKS_TO_WIN) {
				drinkCount_ = SBConfig.DRINKS_TO_WIN;
				Debug.Log("trying to drink more than you're allowed, you fuck!");
			}
			
			if (drinkCount_ < 0) {
				drinkCount_ = 0;
				Debug.Log("trying to drink into the negatives... are you mormon?");
			}
			
			for (int i = 0; i < SBConfig.DRINKS_TO_WIN; i++) {
				FSprite drink = drinks_[i];
				if (i < drinkCount_) drink.isVisible = true;
				else drink.isVisible = false;
			}
		}
	}
}
