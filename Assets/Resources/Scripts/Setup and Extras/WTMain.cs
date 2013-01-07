using UnityEngine;
using System.Collections;

public enum SceneType {
	None,
	GameScene,
	TitleScene,
	HelpScene
}

public class WTMain : MonoBehaviour {

	public static FStage currentScene;
	public static WTMain instance;
	public static WTAnimationManager animationManager;
	
	void Start () {	
		Screen.showCursor = false;
		Screen.SetResolution(1920, 1080, true);
		//SetupLetterbox(Futile.instance.camera);

		if (instance == null) instance = this;
		
		FutileParams fp = new FutileParams(true, true, false, false);
		fp.AddResolutionLevel(1920f, 1.0f, 1.0f, "-sixPack");
		fp.backgroundColor = Color.white;
		fp.origin = Vector2.zero;
		//fp.shouldLerpToNearestResolutionLevel = false;
		
		Futile.instance.Init(fp);
		
		FSoundManager.PlayMusic("jazz");
				
		Futile.atlasManager.LoadAtlas("Atlases/MainSheet");
		Futile.atlasManager.LoadAtlas("Atlases/TitlePageSheet");
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
		
		animationManager.AddAnimation("drinkerPassOut", new string[] {
			"drinkerPassOut0.png",
			"drinkerPassOut1.png",
			"drinkerPassOut2.png",
			"drinkerPassOut3.png",
			"drinkerPassOut4.png"}, 0.04f, false);
		
		SwitchToScene(SceneType.TitleScene);
	}
	
	public static void SwitchToScene(SceneType sceneType) {		
		if (currentScene != null) Futile.RemoveStage(currentScene);
		
		if (sceneType == SceneType.GameScene) {
			currentScene = new SBGameScene(true);
			FSoundManager.StopMusic();
		}
		if (sceneType == SceneType.TitleScene) {
			currentScene = new SBTitleScene();
			Futile.AddStage(currentScene);
		}
		if (sceneType == SceneType.HelpScene) {
			currentScene = new SBHelpScene();
			Futile.AddStage(currentScene);
		}
		if (sceneType == SceneType.None) currentScene = null;
	}
	
	/*void Update() {
		if (Input.GetKeyDown(KeyCode.F)) {
			Screen.fullScreen = !Screen.fullScreen;
		}
	}*/
	
 	void SetupLetterbox(Camera camera)
    {
         // set the desired aspect ratio (the values in this example are
        // hard-coded for 16:10, but you could make them into public
        // variables instead so you can set them at design time)
        float targetaspect = 16.0f / 9.0f;
 
        // determine the game window's current aspect ratio
        float windowaspect = (float)Screen.width / (float)Screen.height;
 
        // current viewport height should be scaled by this amount
        float scaleheight = windowaspect / targetaspect;
 
        // obtain camera component so we can modify its viewport
       // Camera camera = Camera.main;
 
        // if scaled height is less than current height, add letterbox
        if (scaleheight < 1.0f)
        {
            Rect rect = camera.rect;
 
            rect.width = 1.0f;
            rect.height = scaleheight;
            rect.x = 0;
            rect.y = (1.0f - scaleheight) / 2.0f;
 
            camera.rect = rect;
        }
        else // add pillarbox
        {
            float scalewidth = 1.0f / scaleheight;
 
            Rect rect = camera.rect;
 
            rect.width = scalewidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scalewidth) / 2.0f;
            rect.y = 0;
 
            camera.rect = rect;
        }
 
 
 
    }
}
