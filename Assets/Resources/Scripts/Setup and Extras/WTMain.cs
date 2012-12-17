using UnityEngine;
using System.Collections;

public class WTMain : MonoBehaviour {

	public static FStage currentScene;
	public static WTMain instance;
	public static WTAnimationManager animationManager;
	
	public enum SceneType {
		None,
		GameScene
	}
	
	void Start () {
		if (instance == null) instance = this;
		
		WTMain.animationManager = new WTAnimationManager();
		WTMain.animationManager.AddAnimation("drinkerWalk", new string[] {"drinkerIdle.png", "drinkerLeftFront.png", "drinkerIdle.png", "drinkerRightFront.png"}, 0.2f, 0.5f);
		
		FutileParams fp = new FutileParams(true, true, false, false);
		fp.AddResolutionLevel(1920f, 1.0f, 1.0f, "-sixPack");
		fp.backgroundColor = Color.white;
		fp.origin = Vector2.zero;
		
		Futile.instance.Init(fp);
		
		Futile.atlasManager.LoadAtlas("Atlases/MainSheet");
		Futile.atlasManager.LoadFont("Silkscreen", "SilkscreenSmall.png", "Atlases/SilkscreenSmall");
		
		Go.defaultEaseType = EaseType.SineInOut;
		
		SwitchToScene(SceneType.GameScene);
	}
	
	public void SwitchToScene(SceneType sceneType) {
		if (currentScene != null) Futile.RemoveStage(currentScene);
		
		if (sceneType == SceneType.GameScene) currentScene = new SBGameScene(true);
		if (sceneType == SceneType.None) currentScene = null;
		
		//if (currentScene != null) Futile.AddStage(currentScene);
	}
}
