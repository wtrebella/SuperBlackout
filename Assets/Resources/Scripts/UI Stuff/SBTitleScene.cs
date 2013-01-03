using UnityEngine;
using System.Collections;

public class SBTitleScene : FStage {
	public SBTitleScene() : base("") {
		FSprite background = new FSprite("Atlases/splash");
		background.x = Futile.screen.halfWidth;
		background.y = Futile.screen.halfHeight;
		AddChild(background);
	}
	
	override public void HandleAddedToStage() {
		base.HandleAddedToStage();
		Futile.instance.SignalUpdate += HandleUpdate;
	}
	
	override public void HandleRemovedFromStage() {
		base.HandleRemovedFromStage();
		Futile.instance.SignalUpdate -= HandleUpdate;
	}
	
	public void HandleUpdate() {
		if (Input.anyKeyDown) WTMain.SwitchToScene(WTMain.SceneType.GameScene);
	}
}
