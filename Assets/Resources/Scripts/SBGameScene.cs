using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SBGameScene : FStage {
	public SBEntity drinker1;
	public SBEntity drinker2;
	public List<SBEntity> drinkers;
	
	public SBGameScene(bool addToFutileOnInit) : base("") {
		if (addToFutileOnInit) Futile.AddStage(this);
		
		SBBackgroundLayer backgroundLayer = new SBBackgroundLayer();
		AddChild(backgroundLayer);
		
		drinkers = new List<SBEntity>();
		
		drinker1 = new SBEntity();
		drinker1.name = "drinker1";
		drinker1.x = Futile.screen.width * 1.0f/3.0f;
		drinker1.y = Futile.screen.halfHeight;
		SBSpriteComponent sc = new SBSpriteComponent("drinker.psd");
		sc.name = "drinker sprite";
		drinker1.AddComponent(sc);
		drinker1.AddComponent(new SBVelocityComponent());
		drinkers.Add(drinker1);
		AddChild(drinker1);

		drinker2 = new SBEntity();
		drinker2.name = "drinker2";
		drinker2.x = Futile.screen.width * 2.0f/3.0f;
		drinker2.y = Futile.screen.halfHeight;
		sc = new SBSpriteComponent("drinker.psd");
		sc.name = "drinker sprite";
		drinker2.AddComponent(sc);
		drinker2.AddComponent(new SBVelocityComponent());
		drinkers.Add(drinker2);
		AddChild(drinker2);
	}
	
	public override void HandleAddedToStage() {
		base.HandleAddedToStage();
		Futile.instance.SignalUpdate += HandleUpdate;
	}
	
	public override void HandleRemovedFromStage() {
		base.HandleRemovedFromStage();
		Futile.instance.SignalUpdate -= HandleUpdate;
	}
	
	public void HandleUpdate() {			
		foreach (SBEntity drinker in drinkers) {
			if (Input.GetKey(KeyCode.DownArrow)) {
				drinker.VelocityComponent().xVelocity = 0;
				drinker.VelocityComponent().yVelocity = -800;
			}
			
			else if (Input.GetKey(KeyCode.UpArrow)) {
				drinker.VelocityComponent().xVelocity = 0;
				drinker.VelocityComponent().yVelocity = 800;
			}
			
			else if (Input.GetKey(KeyCode.RightArrow)) {
				drinker.VelocityComponent().xVelocity = 800;
				drinker.VelocityComponent().yVelocity = 0;
			}
			
			else if (Input.GetKey(KeyCode.LeftArrow)) {
				drinker.VelocityComponent().xVelocity = -800;
				drinker.VelocityComponent().yVelocity = 0;
			}
			
			else {
				drinker.VelocityComponent().xVelocity = 0;
				drinker.VelocityComponent().yVelocity = 0;
			}
			
			float xDelta = Time.fixedDeltaTime * drinker.VelocityComponent().xVelocity;
			float yDelta = Time.fixedDeltaTime * drinker.VelocityComponent().yVelocity;
			
			float newX = drinker.x + xDelta;
			float newY = drinker.y + yDelta;
			
			//Rect preMovedSpriteRect = drinker.SpriteComponent().sprite.GetGlobalRect();
			Rect postMovedSpriteRect = drinker.SpriteComponent().GetGlobalRectWithOffset(xDelta, yDelta);

			float xMin = SBConfig.BORDER_WIDTH;
			float xMax = Futile.screen.width - SBConfig.BORDER_WIDTH;
			float yMin = SBConfig.BORDER_WIDTH;
			float yMax = Futile.screen.height - SBConfig.TOP_UI_HEIGHT - SBConfig.BORDER_WIDTH;
			
			if (postMovedSpriteRect.xMax > xMax) newX = xMax - drinker.SpriteComponent().sprite.width / 2f;
			else if (postMovedSpriteRect.xMin < xMin) newX = xMin + drinker.SpriteComponent().sprite.width / 2f;
			
			if (postMovedSpriteRect.yMax > yMax) newY = yMax - drinker.SpriteComponent().sprite.height / 2f;
			else if (postMovedSpriteRect.yMin < yMin) newY = yMin + drinker.SpriteComponent().sprite.height / 2f;
			
			drinker.x = newX;
			drinker.y = newY;
		}
	}
}
