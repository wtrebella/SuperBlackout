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
		
		FutileParams fp = new FutileParams(true, true, false, false);
		fp.AddResolutionLevel(1920f, 1.0f, 1.0f, "-sixPack");
		fp.backgroundColor = Color.white;
		fp.origin = Vector2.zero;
		
		Futile.instance.Init(fp);
		
		Futile.atlasManager.LoadAtlas("Atlases/MainSheet");
		Futile.atlasManager.LoadFont("Silkscreen", "Silkscreen.png", "Atlases/Silkscreen");
		
		Go.defaultEaseType = EaseType.SineInOut;
		
		animationManager = new WTAnimationManager();
				
		animationManager.AddAnimation("drinkerWalk", new string[] {
			"drinkerIdle.png",
			"drinkerLeftFront.png",
			"drinkerIdle.png",
			"drinkerRightFront.png"}, 0.05f, 0.4f, true);
		
		animationManager.AddAnimation("drinkerSitTransition", new string[] {
			"drinkerIdle.png",
			"drinkerSittingTrans0.png",
			"drinkerSittingTrans1.png",
			"drinkerSitting.png"}, 0.05f, false);
		
		animationManager.AddAnimation("drinkerStandTransition", new string[] {
			"drinkerSitting.png",
			"drinkerSittingTrans1.png",
			"drinkerSittingTrans0.png",
			"drinkerIdle.png"}, 0.05f, false);
		
		animationManager.AddAnimation("pee", new string[] {
			"pee0.png",
			"pee1.png",
			"pee2.png",
			"pee3.png",
			"pee4.png",
			"pee5.png"}, 0.15f, false);
		
		animationManager.AddAnimation("drinkSpill", new string[] {
			"glassSpill0.png",
			"glassSpill1.png",
			"glassSpill2.png",
			"glassSpill3.png"}, 0.08f, false);
		
		animationManager.AddAnimation("punch", new string[] {
			"drinkerPunching.png"}, 0.05f, false);
		
		SwitchToScene(SceneType.GameScene);
	}
	
	public void SwitchToScene(SceneType sceneType) {
		if (currentScene != null) Futile.RemoveStage(currentScene);
		
		if (sceneType == SceneType.GameScene) currentScene = new SBGameScene(true);
		if (sceneType == SceneType.None) currentScene = null;
		
		//if (currentScene != null) Futile.AddStage(currentScene);
	}
}
