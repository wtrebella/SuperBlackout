using UnityEngine;
using System.Collections;

public class SBGameScene : FStage {
	public SBGameScene() : base("") {
		SBBackgroundLayer backgroundLayer = new SBBackgroundLayer();
		AddChild(backgroundLayer);
	}
}
