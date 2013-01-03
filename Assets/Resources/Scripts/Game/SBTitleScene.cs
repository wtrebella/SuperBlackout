using UnityEngine;
using System.Collections;

public class SBTitleScene : FStage {
	FSprite super;
	FSprite blackout;
	
	bool introIsDone = false;
	bool isSwitchingScenes = false;
	
	TweenChain tweenChain1;
	TweenChain tweenChain2;
	
	bool player1Ready = false;
	bool player2Ready = false;
	
	FLabel inGameHelp;
	FLabel inGameHelpToggle;
	
	FLabel player1;
	FLabel player2;
	
	FLabel ready1;
	FLabel ready2;
	
	FLabel helpLabel;
	
	public SBTitleScene() : base("") {
		float boardWidth = 200f;
		float boardHeight = 20f;
		float padding = 2f;
		int boardRowCount = (int)(Futile.screen.height / boardHeight) + 1;
		
		for (int i = 0; i < boardRowCount; i++) {
			float rowOffset = Random.Range(-boardWidth, 0);
			int boardColumnCount = (int)((Futile.screen.width - rowOffset) / boardWidth) + 1;
			for (int j = 0; j < boardColumnCount; j++) {
				FSprite board = WTSquareMaker.Square(boardWidth + padding * 2, boardHeight + padding * 2);
				float rand = Random.Range(-0.05f, 0.05f) + 0.25f;
				board.color = new Color(0.66f + rand, 0.55f + rand, 0.4f + rand);
				board.anchorX = 0;
				board.anchorY = 0;
				board.x = j * boardWidth + rowOffset - padding;
				board.y = i * boardHeight - padding;
				AddChild(board);
			}
		}
		
		helpLabel = new FLabel("Silkscreen", "Press SPACE or Q for tutorial");
		helpLabel.scale = 0.35f;
		helpLabel.alpha = 0;
		helpLabel.anchorY = 0;
		helpLabel.anchorX = 1;
		helpLabel.x = Futile.screen.width - 30f;
		helpLabel.y = 15f;
		helpLabel.color = Color.black;
		
		Tween fadeOut = new Tween(helpLabel, 0.15f, new TweenConfig().floatProp("alpha", 0));
		Tween fadeIn = new Tween(helpLabel, 0.15f, new TweenConfig().floatProp("alpha", 1));
		
		TweenChain chain = new TweenChain();
		chain.setIterations(-1);
		chain.appendDelay(0.3f);
		chain.append(fadeOut);
		chain.append(fadeIn);
		Go.addTween(chain);
		chain.play();
		
		AddChild(helpLabel);
		
		player1 = new FLabel("Silkscreen", "Player 1:");
		player1.alpha = 0;
		player1.color = Color.black;
		player1.scale = 0.4f;
		player1.anchorX = 0;
		player1.anchorY = 1;
		player1.x = 30f;
		player1.y = Futile.screen.height - 20f;
		AddChild(player1);
		
		player2 = new FLabel("Silkscreen", "Player 2:");
		player2.alpha = 0;
		player2.color = Color.black;
		player2.scale = 0.4f;
		player2.anchorX = 0;
		player2.anchorY = 1;
		player2.x = 30f;
		player2.y = Futile.screen.height - 20f - 50f;
		AddChild(player2);
		
		ready1 = new FLabel("Silkscreen", "Press 1 to join");
		ready1.alpha = 0;
		ready1.color = Color.red;
		ready1.scale = 0.4f;
		ready1.anchorX = 0;
		ready1.anchorY = 1;
		ready1.x = 250f;
		ready1.y = Futile.screen.height - 20f;
		AddChild(ready1);
		
		ready2 = new FLabel("Silkscreen", "Press 2 to join");
		ready2.alpha = 0;
		ready2.color = Color.red;
		ready2.scale = 0.4f;
		ready2.anchorX = 0;
		ready2.anchorY = 1;
		ready2.x = 250f;
		ready2.y = Futile.screen.height - 20f - 50f;
		AddChild(ready2);
		
		inGameHelp = new FLabel("Silkscreen", "In-Game Help (press X or V):");
		inGameHelp.alpha = 0;
		inGameHelp.color = Color.black;
		inGameHelp.scale = 0.4f;
		inGameHelp.anchorX = 1;
		inGameHelp.anchorY = 1;
		inGameHelp.x = Futile.screen.width - 100f;
		inGameHelp.y = Futile.screen.height - 20f;
		AddChild(inGameHelp);
		
		inGameHelpToggle = new FLabel("Silkscreen", "On");
		inGameHelpToggle.alpha = 0;
		inGameHelpToggle.color = new Color(0, 0.8f, 0, 1.0f);
		inGameHelpToggle.scale = 0.4f;
		inGameHelpToggle.anchorX = 1;
		inGameHelpToggle.anchorY = 1;
		inGameHelpToggle.x = Futile.screen.width - 30f;
		inGameHelpToggle.y = Futile.screen.height - 25f;
		AddChild(inGameHelpToggle);
		RefreshInGameHelpToggle();
		
		float amt = 700f;
		
		super = new FSprite("super.png");
		super.data = 0;
		super.x = Futile.screen.halfWidth - amt;
		super.y = Futile.screen.halfHeight - amt;
		
		blackout = new FSprite("blackout.png");
		blackout.data = 1;
		blackout.x = Futile.screen.halfHeight - amt;
		blackout.y = Futile.screen.halfHeight - amt;
		
		AddChild(blackout);
		AddChild(super);
		
		EaseType easeType = EaseType.BounceOut;
		float duration = 0.8f;
		
		Go.to(super, duration, new TweenConfig()
			.floatProp("x", Futile.screen.halfWidth - 5f)
			.floatProp("y", Futile.screen.halfHeight - 5f)
			.setEaseType(easeType)
			.onComplete(HandleWordFinishedComingIn));
		
		Go.to(blackout, duration, new TweenConfig()
			.setDelay(duration / 2f)
			.floatProp("x", Futile.screen.halfWidth - 5f)
			.floatProp("y", Futile.screen.halfHeight - 5f)
			.setEaseType(easeType)
			.onComplete(HandleWordFinishedComingIn));
	}
	
	public void RefreshInGameHelpToggle() {
		if (SBConfig.HELP_LABELS_ON) {
			inGameHelpToggle.text = "ON";
			inGameHelpToggle.color = new Color(0, 0.8f, 0, 1.0f);
		}
		else {
			inGameHelpToggle.text = "OFF";
			inGameHelpToggle.color = Color.red;
		}
	}
	
	public void HandleWordFinishedComingIn(AbstractTween tween) {
		FSprite word = (tween as Tween).target as FSprite;
		ShakeWord(word);
		if ((int)word.data == 0) {
		}
		else if ((int)word.data == 1) {
			introIsDone = true;
			Go.to(helpLabel, 0.5f, new TweenConfig().floatProp("alpha", 1));
			Go.to(player1, 0.5f, new TweenConfig().floatProp("alpha", 1));
			Go.to(player2, 0.5f, new TweenConfig().floatProp("alpha", 1));
			Go.to(ready1, 0.5f, new TweenConfig().floatProp("alpha", 1));
			Go.to(ready2, 0.5f, new TweenConfig().floatProp("alpha", 1));
			Go.to(inGameHelp, 0.5f, new TweenConfig().floatProp("alpha", 1));
			Go.to(inGameHelpToggle, 0.5f, new TweenConfig().floatProp("alpha", 1));
		}
	}
	
	public void HandleWordFinishedShake(AbstractTween tween) {
		FSprite word = (tween as Tween).target as FSprite;
		ShakeWord(word);
	}
	
	public void HandleWordDoneDismissing(AbstractTween tween) {
		if (player1Ready && player2Ready) WTMain.SwitchToScene(SceneType.GameScene);
	}
	
	public void DismissWord(FSprite word) {
		Go.killAllTweensWithTarget(word);
		
		if ((int)word.data == 0) {
			tweenChain1.destroy();
		}
		else if ((int)word.data == 1) {
			tweenChain2.destroy();
		}
		
		float duration = 1.0f;
		float amt = 800f;
		EaseType easeType = EaseType.ExpoOut;
		
		Go.to(word, duration, new TweenConfig()
			.floatProp("x", Futile.screen.halfWidth - amt)
			.floatProp("y", Futile.screen.halfHeight - amt)
			.setEaseType(easeType)
			.onComplete(HandleWordDoneDismissing));
	}
	
	public void ShakeWord(FSprite word) {
		/*float dur = Random.Range(0.05f, 0.15f);
		float xDelta = Random.Range(-5f, 5f);
		float yDelta = Random.Range(-5f, 5f);*/
		
		float dur = 0.3f;
		float xDelta = -40f;
		float yDelta = -40f;
		
		Tween tweenIn = new Tween(word, dur, new TweenConfig().floatProp("x", xDelta, true).floatProp("y", yDelta, true));
		Tween tweenOut = new Tween(word, dur, new TweenConfig().floatProp("x", -xDelta, true).floatProp("y", -yDelta, true).onComplete(HandleWordFinishedShake));
		
		TweenChain tweenChain = new TweenChain();
		tweenChain.append(tweenIn);
		tweenChain.append(tweenOut);
		
		if ((int)word.data == 0) {
			tweenChain1 = tweenChain;
		}
		else if ((int)word.data == 1) {
			tweenChain2 = tweenChain;
		}
		
		Go.addTween(tweenChain);
		
		tweenChain.play();
	}
	
	override public void HandleAddedToStage() {
		base.HandleAddedToStage();
		Futile.instance.SignalUpdate += HandleUpdate;
	}
	
	override public void HandleRemovedFromStage() {
		base.HandleRemovedFromStage();
		Futile.instance.SignalUpdate -= HandleUpdate;
	}
	
	public void ToggleInGameHelp() {
		SBConfig.HELP_LABELS_ON = !SBConfig.HELP_LABELS_ON;
		RefreshInGameHelpToggle();
	}
	
	public void HandleUpdate() {
		if (!introIsDone || isSwitchingScenes) return;
				
		if (Input.GetKeyDown(KeyCode.Alpha1)) {
			ready1.color = new Color(0, 0.8f, 0, 1.0f);
			ready1.y -= 6f;
			ready1.text = "Ready";
			player1Ready = true;
			DismissWord(super);
		}
		if (Input.GetKeyDown(KeyCode.Alpha2)) {
			ready2.color = new Color(0, 0.8f, 0, 1.0f);
			ready2.y -= 6f;
			ready2.text = "Ready";
			player2Ready = true;
			DismissWord(blackout);
		}
		
		if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.V)) ToggleInGameHelp();
		
		if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Q)) {
			isSwitchingScenes = true;
			WTMain.SwitchToScene(SceneType.HelpScene);
		}
	}
}
