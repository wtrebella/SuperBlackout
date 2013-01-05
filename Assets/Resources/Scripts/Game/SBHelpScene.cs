using UnityEngine;
using System.Collections;

public class SBHelpScene : FStage {
	private int pageNum_ = 0;
	private FContainer mainContainer = new FContainer();
	private bool currentPageIsFullySetup = false;
	private SBArcadeButtons continueButtons;
	private SBDrinker drinker1;
	private SBDrinker drinker2;
	
	float punchTimer1 = 0;
	float punchTimer2 = 0;
	
	public SBHelpScene() : base("") {
		/*FSprite background = new FSprite("splashBackground.png");
		background.x = Futile.screen.halfWidth;
		background.y = Futile.screen.halfHeight;
		AddChild(background);*/
		
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
		
		continueButtons = new SBArcadeButtons(true);
		continueButtons.x = Futile.screen.halfWidth - 65f;
		continueButtons.y = 145f;
		continueButtons.scale = 0.3f;
		continueButtons.currentFlashingButton = 5;
		AddChild(continueButtons);
		
		AddChild(mainContainer);
		
		drinker1 = NewPrefabbedDrinker(Direction.Left, new Color(0.3f, 0.5f, 1.0f, 1.0f), 1);
		drinker2 = NewPrefabbedDrinker(Direction.Right, new Color(1.0f, 0.3f, 0.5f, 1.0f), 2);
		
		Go.to(drinker1, 3.0f, new TweenConfig().setIterations(-1).setEaseType(EaseType.Linear).floatProp("rotation", 360, true));
		Go.to(drinker2, 2.5f, new TweenConfig().setIterations(-1).setEaseType(EaseType.Linear).floatProp("rotation", 360, true));
		
		SetupPage(0);
	}
	
	override public void HandleAddedToStage() {
		base.HandleAddedToStage();
		Futile.instance.SignalUpdate += HandleUpdate;
	}
	
	override public void HandleRemovedFromStage() {
		base.HandleRemovedFromStage();
		Futile.instance.SignalUpdate -= HandleUpdate;
		for (int i = 0; i < mainContainer.GetChildCount(); i++) {
			Go.killAllTweensWithTarget(mainContainer.GetChildAt(i));	
		}
	}
	
	public void HandleUpdate() {		
		for (int i = 0; i < mainContainer.GetChildCount(); i++) {
			FNode node = mainContainer.GetChildAt(i);
			if (node.GetType().FullName == "SBDrinker") {
				SBDrinker drinker = (node as SBDrinker);
				if (pageNum_ != 3) drinker.VelocityComponent().xVelocity = drinker.tag * 100f + 150f;
				drinker.HandleUpdate();
			}
		}
		
		if (pageNum_ == 3) {
			punchTimer1 += Time.fixedDeltaTime;
			punchTimer2 += Time.fixedDeltaTime;
			
			if (punchTimer1 >= 0.4f) {
				punchTimer1 = 0;
				drinker1.PunchDrinker(null);
			}
			
			if (punchTimer2 >= 0.3f) {
				punchTimer2 = 0;
				drinker2.PunchDrinker(null);
			}
		}
		
		if (!currentPageIsFullySetup) return;
		
		if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Q)) DismissAllNodes(0.2f, 0.1f);
	}
	
	public void GoToNextPage() {
		for (int i = mainContainer.GetChildCount() - 1; i >= 0; i--) {
			mainContainer.GetChildAt(i).RemoveFromContainer();	
		}
				
		SetupPage(++pageNum_);
	}
	
	public SBDrinker NewPrefabbedDrinker(Direction faceDirection, Color color, int tag) {
		SBDrinker drinker = new SBDrinker("drinker");
		drinker.tag = tag;
		drinker.DirectionComponent().FaceDirection(faceDirection);
		drinker.ProgressBarComponent().progressBar.isVisible = false;
		drinker.SpriteComponent(1).sprite.color = color;
		return drinker;
	}
	
	public void SetupPage(int pageNum) {
		currentPageIsFullySetup = false;
		
		drinker1.scale = 1;
		drinker2.scale = 1;
		
		if (pageNum == 0) {
			drinker1.x = Futile.screen.width * 1f/3f;
			drinker1.y = Futile.screen.halfHeight + 50f;
			
			drinker2.x = Futile.screen.width * 2f/3f;
			drinker2.y = Futile.screen.halfHeight + 50f;
			
			FLabel label1 = new FLabel("Silkscreen", "This is\nPlayer 1");
			label1.x = drinker1.x;
			label1.y = drinker1.y - 100f;
			label1.color = drinker1.SpriteComponent(1).sprite.color;
			label1.scale = 0.5f;
			
			FLabel label2 = new FLabel("Silkscreen", "This is\nPlayer 2");
			label2.x = drinker2.x;
			label2.y = drinker2.y - 100f;
			label2.color = drinker2.SpriteComponent(1).sprite.color;
			label2.scale = 0.5f;
			
			ShowNodes(new FNode[] {drinker1, label1, drinker2, label2}, 0.6f, 0.3f);
		}
		
		if (pageNum == 1) {
			drinker1.y = Futile.screen.halfHeight + 200f;
			drinker2.y = Futile.screen.halfHeight + 200f;
			
			drinker1.SpriteComponent(1).StartAnimation(WTMain.animationManager.AnimationForName("drinkerWalk"));
			drinker2.SpriteComponent(1).StartAnimation(WTMain.animationManager.AnimationForName("drinkerWalk"));
			
			SBArcadeButtons ab1 = new SBArcadeButtons(true);
			ab1.joystickIsRotating = true;
			ab1.x = drinker1.x - 75f;
			ab1.y = drinker1.y - 70f;
			ab1.scale = 0.3f;
			
			SBArcadeButtons ab2 = new SBArcadeButtons(true);
			ab2.joystickIsRotating = true;
			ab2.x = drinker2.x - 75f;
			ab2.y = drinker2.y - 70f;
			ab2.scale = 0.3f;
			
			FLabel text = new FLabel("Silkscreen", "Walk around the bar with the joystick");
			text.scale = 0.8f;
			text.color = Color.black;
			text.anchorY = 1;
			text.x = Futile.screen.halfWidth;
			text.y = Futile.screen.halfHeight - 50f;			
			
			ShowNodes(new FNode[] {drinker1, ab1, drinker2, ab2, text}, 0.5f, 0.25f);
		}
		
		if (pageNum == 2) {
			SBBar bar = new SBBar();
			
			FLabel text = new FLabel("Silkscreen", "Get a drink at your bar,\nthen drink it at your barstool");
			text.scale = 0.8f;
			text.color = Color.black;
			text.anchorY = 1;
			text.x = Futile.screen.halfWidth;
			text.y = Futile.screen.halfHeight + 100f;		
			
			SBBarStool specialBarStool1 = new SBBarStool("special bar stool 1", new Color(0.3f, 0.5f, 1.0f, 1.0f));
			specialBarStool1.x = 100f;
			specialBarStool1.y = Futile.screen.height - SBConfig.TOP_UI_HEIGHT - 100f;
			specialBarStool1.ProgressBarComponent().progressBar.isVisible = false;
			
			SBBarStool specialBarStool2 = new SBBarStool("special bar stool 2", new Color(1.0f, 0.3f, 0.5f, 1.0f));
			specialBarStool2.x = Futile.screen.width - 100f;
			specialBarStool2.y = Futile.screen.height - SBConfig.TOP_UI_HEIGHT - 100f;
			specialBarStool2.ProgressBarComponent().progressBar.isVisible = false;
			
			float xAmt = 190f;
			float yAmt = 220f;
			
			FSprite drinkHere1 = new FSprite("drinkHere1.psd");
			drinkHere1.alpha = 0.5f;
			drinkHere1.color = new Color(0.3f, 0.5f, 1.0f, 1.0f);
			drinkHere1.anchorX = 0;
			drinkHere1.x = xAmt;
			drinkHere1.y = Futile.screen.height - SBConfig.TOP_UI_HEIGHT - yAmt;
			
			FSprite arrow1 = new FSprite("arrow.psd");
			arrow1.alpha = drinkHere1.alpha;
			arrow1.color = drinkHere1.color;
			arrow1.anchorX = 1;
			arrow1.anchorY = 0;
			arrow1.x = drinkHere1.x - 10f;
			arrow1.y = drinkHere1.y - 5f;
			
			FSprite drinkHere2 = new FSprite("drinkHere2.psd");
			drinkHere2.alpha = 0.5f;
			drinkHere2.color = new Color(1.0f, 0.3f, 0.5f, 1.0f);
			drinkHere2.anchorX = 1;
			drinkHere2.x = Futile.screen.width - xAmt;
			drinkHere2.y = Futile.screen.height - SBConfig.TOP_UI_HEIGHT - yAmt;
			
			FSprite arrow2 = new FSprite("arrow.psd");
			arrow2.scaleX = -1;
			arrow2.alpha = drinkHere2.alpha;
			arrow2.color = drinkHere2.color;
			arrow2.anchorX = 1;
			arrow2.anchorY = 0;
			arrow2.x = drinkHere2.x + 10f;
			arrow2.y = drinkHere2.y - 5f;
			
			xAmt = 223f;
			yAmt = 285f;
			
			FSprite getDrinkHere1 = new FSprite("getDrinkHere1.psd");
			getDrinkHere1.alpha = 0.5f;
			getDrinkHere1.color = new Color(0.3f, 0.5f, 1.0f, 1.0f);
			getDrinkHere1.anchorX = 1;
			getDrinkHere1.x = Futile.screen.width - xAmt;
			getDrinkHere1.y = yAmt;
			
			FSprite getArrow1 = new FSprite("arrow.psd");
			getArrow1.scale = -1;
			getArrow1.alpha = getDrinkHere1.alpha;
			getArrow1.color = getDrinkHere1.color;
			getArrow1.anchorX = 1;
			getArrow1.anchorY = 0;
			getArrow1.x = getDrinkHere1.x + 10f;
			getArrow1.y = getDrinkHere1.y - 5f;
			
			FSprite getDrinkHere2 = new FSprite("getDrinkHere2.psd");
			getDrinkHere2.alpha = 0.5f;
			getDrinkHere2.color = new Color(1.0f, 0.3f, 0.5f, 1.0f);
			getDrinkHere2.anchorX = 0;
			getDrinkHere2.x = xAmt;
			getDrinkHere2.y = yAmt;
			
			FSprite getArrow2 = new FSprite("arrow.psd");
			getArrow2.scaleY = -1;
			getArrow2.alpha = getDrinkHere2.alpha;
			getArrow2.color = getDrinkHere2.color;
			getArrow2.anchorX = 1;
			getArrow2.anchorY = 0;
			getArrow2.x = getDrinkHere2.x - 10f;
			getArrow2.y = getDrinkHere2.y - 5f;
			
			ShowNodes(new FNode[] {drinker1, drinker2, text, specialBarStool1, specialBarStool2, drinkHere1, drinkHere2, getDrinkHere1, getDrinkHere2, arrow1, arrow2, getArrow1, getArrow2, bar}, 0.25f, 0.125f);
		}
		
		if (pageNum == 3) {
			SBDrink drink = new SBDrink("drink");
			drink.x = Futile.screen.halfWidth;
			drink.y = Futile.screen.halfHeight + 200f;
			drink.Spill();
			
			drinker1.y = Futile.screen.halfHeight + 200f;
			drinker2.y = Futile.screen.halfHeight + 200f;
			
			drinker1.SpriteComponent(1).StartAnimation(WTMain.animationManager.AnimationForName("drinkerWalk"));
			drinker2.SpriteComponent(1).StartAnimation(WTMain.animationManager.AnimationForName("drinkerWalk"));
						
			SBArcadeButtons ab1 = new SBArcadeButtons(true);
			ab1.currentFlashingButton = 6;
			ab1.x = drinker1.x - 75f;
			ab1.y = drinker1.y - 70f;
			ab1.scale = 0.3f;
			
			SBArcadeButtons ab2 = new SBArcadeButtons(true);
			ab2.currentFlashingButton = 6;
			ab2.x = drinker2.x - 75f;
			ab2.y = drinker2.y - 70f;
			ab2.scale = 0.3f;
			
			FLabel punch = new FLabel("Silkscreen", "Punch each other\nto spill drinks!");
			punch.anchorY = 0;
			punch.color = Color.black;
			punch.scale = 0.8f;
			punch.x = Futile.screen.halfWidth;
			punch.y = Futile.screen.halfHeight - 150f;
			
			ShowNodes(new FNode[] {drinker1, ab1, drinker2, ab2, punch, drink}, 0.5f, 0.25f);
		}
		
		if (pageNum == 4) {
			drinker1.y = Futile.screen.halfHeight + 150f;
			drinker2.y = Futile.screen.halfHeight + 150f;
			
			drinker1.SpriteComponent(1).StartAnimation(WTMain.animationManager.AnimationForName("drinkerPassOut"));
			drinker2.SpriteComponent(1).StartAnimation(WTMain.animationManager.AnimationForName("drinkerPassOut"));
			
			FLabel text = new FLabel("Silkscreen", "First one to pass out\nfrom drinking wins!");
			text.scale = 0.8f;
			text.color = Color.black;
			text.anchorY = 1;
			text.x = Futile.screen.halfWidth;
			text.y = Futile.screen.halfHeight - 50f;			
			
			ShowNodes(new FNode[] {drinker1, drinker2, text}, 0.5f, 0.25f);
		}
		
		if (pageNum == 5) {			
			drinker1.SpriteComponent(0).sprite.isVisible = true;
			drinker2.SpriteComponent(0).sprite.isVisible = true;
			
			drinker1.SpriteComponent(0).StartAnimation(WTMain.animationManager.AnimationForName("pee"));
			drinker2.SpriteComponent(0).StartAnimation(WTMain.animationManager.AnimationForName("pee"));
			
			FLabel pee = new FLabel("Silkscreen", "But make sure you\ndon't piss yourself!");
			pee.anchorY = 0;
			pee.color = Color.black;
			pee.scale = 0.8f;
			pee.x = Futile.screen.halfWidth;
			pee.y = Futile.screen.halfHeight - 150f;
			
			ShowNodes(new FNode[] {drinker1, drinker2, pee}, 0.5f, 0.25f);
		}
		
		if (pageNum == 6) {
			WTMain.SwitchToScene(SceneType.TitleScene);	
		}
	}
	
	public void ShowNodes(FNode[] nodes, float showDuration, float betweenDuration) {
		for (int i = 0; i < nodes.Length; i++) {
			FNode node = nodes[i];
			
			mainContainer.AddChild(node);
			
			float xEndScale = node.scaleX;
			float yEndScale = node.scaleY;
			
			node.scaleX = 0;
			node.scaleY = 0;
			
			if (i < nodes.Length - 1) Go.to(node, showDuration, new TweenConfig().setDelay(i * betweenDuration).floatProp("scaleX", xEndScale).floatProp("scaleY", yEndScale));
			else Go.to(node, showDuration, new TweenConfig().setDelay(i * betweenDuration).floatProp("scaleX", xEndScale).floatProp("scaleY", yEndScale).onComplete(HandleDoneShowingNodes));
		}
	}
	
	public void DismissNodes(FNode[] nodes, float dismissDuration, float betweenDuration) {
		for (int i = 0; i < nodes.Length; i++) {
			FNode node = nodes[i];
						
			if (i < nodes.Length - 1) Go.to(node, dismissDuration, new TweenConfig().setDelay(i * betweenDuration).floatProp("scale", 0));
			else Go.to(node, dismissDuration, new TweenConfig().setDelay(i * betweenDuration).floatProp("scale", 0).onComplete(HandleDoneDismissingNodes));
		}
	}
	
	public void DismissAllNodes(float dismissDuration, float betweenDuration) {
		FNode[] nodes = new FNode[mainContainer.GetChildCount()];
		
		for (int i = 0; i < mainContainer.GetChildCount(); i++) {
			nodes[i] = mainContainer.GetChildAt(i);	
		}
		
		DismissNodes(nodes, dismissDuration, betweenDuration);
	}
	
	public void HandleDoneShowingNodes(AbstractTween tween) {
		currentPageIsFullySetup = true;
	}
	
	public void HandleDoneDismissingNodes(AbstractTween tween) {
		GoToNextPage();
	}
}
