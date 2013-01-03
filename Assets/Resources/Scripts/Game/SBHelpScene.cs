using UnityEngine;
using System.Collections;

public class SBHelpScene : FStage {
	private int pageNum_ = 0;
	private FContainer mainContainer = new FContainer();
	private bool currentPageIsFullySetup = false;
	private FLabel continueLabel;
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
		
		continueLabel = new FLabel("Silkscreen", "Press C or J to continue");
		continueLabel.anchorY = 0;
		continueLabel.x = Futile.screen.halfWidth;
		continueLabel.y = 30f;
		continueLabel.color = Color.black;
		
		Tween fadeOut = new Tween(continueLabel, 0.15f, new TweenConfig().floatProp("alpha", 0));
		Tween fadeIn = new Tween(continueLabel, 0.15f, new TweenConfig().floatProp("alpha", 1));
		
		TweenChain chain = new TweenChain();
		chain.setIterations(-1);
		chain.appendDelay(0.3f);
		chain.append(fadeOut);
		chain.append(fadeIn);
		Go.addTween(chain);
		chain.play();
		
		AddChild(mainContainer);
		
		drinker1 = NewPrefabbedDrinker(Direction.Left, new Color(0.3f, 0.5f, 1.0f, 1.0f), 1);
		drinker2 = NewPrefabbedDrinker(Direction.Right, new Color(1.0f, 0.3f, 0.5f, 1.0f), 2);
		
		Go.to(drinker1, 3.0f, new TweenConfig().setIterations(-1).setEaseType(EaseType.Linear).floatProp("rotation", 360, true));
		Go.to(drinker2, 2.5f, new TweenConfig().setIterations(-1).setEaseType(EaseType.Linear).floatProp("rotation", -360, true));
		
		SetupPage(0);
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
		for (int i = 0; i < mainContainer.GetChildCount(); i++) {
			FNode node = mainContainer.GetChildAt(i);
			if (node.GetType().FullName == "SBDrinker") {
				SBDrinker drinker = (node as SBDrinker);
				if (pageNum_ != 2) drinker.VelocityComponent().xVelocity = drinker.tag * 100f + 150f;
				drinker.HandleUpdate();
			}
		}
		
		if (pageNum_ == 2) {
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
		
		if (Input.GetKeyDown(SBConfig.ATTACK_BUTTON_1) || Input.GetKeyDown(SBConfig.ATTACK_BUTTON_2)) DismissAllNodes(0.2f, 0.1f);
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
		continueLabel.scale = 0.4f;
		
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
			
			ShowNodes(new FNode[] {drinker1, label1, drinker2, label2, continueLabel}, 0.6f, 0.3f);
		}
		
		if (pageNum == 1) {
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
			
			ShowNodes(new FNode[] {drinker1, drinker2, text, continueLabel}, 0.5f, 0.25f);
		}
		
		if (pageNum == 2) {
			drinker1.SpriteComponent(1).StartAnimation(WTMain.animationManager.AnimationForName("drinkerWalk"));
			drinker2.SpriteComponent(1).StartAnimation(WTMain.animationManager.AnimationForName("drinkerWalk"));
			
			drinker1.y += 50f;
			drinker2.y += 50f;
			
			FLabel label1 = new FLabel("Silkscreen", "Punch: C");
			label1.x = drinker1.x;
			label1.y = drinker1.y - 100f;
			label1.color = drinker1.SpriteComponent(1).sprite.color;
			label1.scale = 0.5f;
			
			FLabel label2 = new FLabel("Silkscreen", "Punch: J");
			label2.x = drinker2.x;
			label2.y = drinker2.y - 100f;
			label2.color = drinker2.SpriteComponent(1).sprite.color;
			label2.scale = 0.5f;
			
			FLabel punch = new FLabel("Silkscreen", "Punch each other\nto spill drinks!");
			punch.anchorY = 0;
			punch.color = Color.black;
			punch.scale = 0.8f;
			punch.x = Futile.screen.halfWidth;
			punch.y = Futile.screen.halfHeight - 150f;
			
			ShowNodes(new FNode[] {drinker1, label1, drinker2, label2, punch, continueLabel}, 0.5f, 0.25f);
		}
		
		if (pageNum == 3) {			
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
			
			ShowNodes(new FNode[] {drinker1, drinker2, pee, continueLabel}, 0.5f, 0.25f);
		}
		
		if (pageNum == 4) {
			WTMain.SwitchToScene(SceneType.TitleScene);	
		}
	}
	
	public void ShowNodes(FNode[] nodes, float showDuration, float betweenDuration) {
		for (int i = 0; i < nodes.Length; i++) {
			FNode node = nodes[i];
			
			mainContainer.AddChild(node);
			
			float endScale = node.scale;
			
			node.scale = 0;
			
			if (i < nodes.Length - 1) Go.to(node, showDuration, new TweenConfig().setDelay(i * betweenDuration).floatProp("scale", endScale));
			else Go.to(node, showDuration, new TweenConfig().setDelay(i * betweenDuration).floatProp("scale", endScale).onComplete(HandleDoneShowingNodes));
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
