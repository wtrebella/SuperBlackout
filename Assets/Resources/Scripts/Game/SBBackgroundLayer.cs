using UnityEngine;
using System.Collections;

public class SBBackgroundLayer : FContainer {
	public FContainer player1DrinkHelpContainer = new FContainer();
	public FContainer player2DrinkHelpContainer = new FContainer();
	
	public FContainer player1GetDrinkHelpContainer = new FContainer();
	public FContainer player2GetDrinkHelpContainer = new FContainer();
	
	public SBBackgroundLayer() {
		MakeFloor();
		
		FSprite bathroom1 = new FSprite("bathroomTop.psd");
		bathroom1.x = Futile.screen.halfWidth;
		bathroom1.anchorY = 1;
		bathroom1.y = Futile.screen.height - SBConfig.TOP_UI_HEIGHT;
		AddChild(bathroom1);
		
		FSprite bathroom2 = new FSprite("bathroomBottom.psd");
		bathroom2.x = Futile.screen.halfWidth;
		bathroom2.anchorY = 0;
		bathroom2.y = 0;
		AddChild(bathroom2);	
		
		if (SBConfig.HELP_LABELS_ON) {
			MakeHelpLabels();
		}
	}
	
	public void ShowDrinkHelp(int playerNum) {
		FContainer hContainer = null;
		
		if (playerNum == 1) hContainer = player1DrinkHelpContainer;
		else if (playerNum == 2) hContainer = player2DrinkHelpContainer;
		
		Go.to(hContainer, 0.5f, new TweenConfig().floatProp("alpha", 1));
	}
	
	public void ShowGetDrinkHelp(int playerNum) {
		FContainer hContainer = null;
		
		if (playerNum == 1) hContainer = player1GetDrinkHelpContainer;
		else if (playerNum == 2) hContainer = player2GetDrinkHelpContainer;
		
		Go.to(hContainer, 0.5f, new TweenConfig().floatProp("alpha", 1));
	}
	
	public void HideDrinkHelp(int playerNum) {
		FContainer hContainer = null;
		
		if (playerNum == 1) hContainer = player1DrinkHelpContainer;
		else if (playerNum == 2) hContainer = player2DrinkHelpContainer;
		
		Go.to(hContainer, 0.5f, new TweenConfig().floatProp("alpha", 0));
	}
	
	public void HideGetDrinkHelp(int playerNum) {
		FContainer hContainer = null;
		
		if (playerNum == 1) hContainer = player1GetDrinkHelpContainer;
		else if (playerNum == 2) hContainer = player2GetDrinkHelpContainer;
		
		Go.to(hContainer, 0.5f, new TweenConfig().floatProp("alpha", 0));
	}
	
	private void MakeHelpLabels() {
		AddChild(player1DrinkHelpContainer);
		AddChild(player2DrinkHelpContainer);
		
		player1DrinkHelpContainer.alpha = player2DrinkHelpContainer.alpha = 0;
		
		AddChild(player1GetDrinkHelpContainer);
		AddChild(player2GetDrinkHelpContainer);
		
		float xAmt = 190f;
		float yAmt = 220f;
		
		FSprite drinkHere1 = new FSprite("drinkHere1.psd");
		drinkHere1.alpha = 0.5f;
		drinkHere1.color = new Color(0.3f, 0.5f, 1.0f, 1.0f);
		drinkHere1.anchorX = 0;
		drinkHere1.x = xAmt;
		drinkHere1.y = Futile.screen.height - SBConfig.TOP_UI_HEIGHT - yAmt;
		player1DrinkHelpContainer.AddChild(drinkHere1);
		
		FSprite arrow1 = new FSprite("arrow.psd");
		arrow1.alpha = drinkHere1.alpha;
		arrow1.color = drinkHere1.color;
		arrow1.anchorX = 1;
		arrow1.anchorY = 0;
		arrow1.x = drinkHere1.x - 10f;
		arrow1.y = drinkHere1.y - 5f;
		player1DrinkHelpContainer.AddChild(arrow1);
		
		FSprite drinkHere2 = new FSprite("drinkHere2.psd");
		drinkHere2.alpha = 0.5f;
		drinkHere2.color = new Color(1.0f, 0.3f, 0.5f, 1.0f);
		drinkHere2.anchorX = 1;
		drinkHere2.x = Futile.screen.width - xAmt;
		drinkHere2.y = Futile.screen.height - SBConfig.TOP_UI_HEIGHT - yAmt;
		player2DrinkHelpContainer.AddChild(drinkHere2);
		
		FSprite arrow2 = new FSprite("arrow.psd");
		arrow2.scaleX = -1;
		arrow2.alpha = drinkHere2.alpha;
		arrow2.color = drinkHere2.color;
		arrow2.anchorX = 1;
		arrow2.anchorY = 0;
		arrow2.x = drinkHere2.x + 10f;
		arrow2.y = drinkHere2.y - 5f;
		player2DrinkHelpContainer.AddChild(arrow2);
		
		xAmt = 223f;
		yAmt = 285f;
		
		FSprite getDrinkHere1 = new FSprite("getDrinkHere1.psd");
		getDrinkHere1.alpha = 0.5f;
		getDrinkHere1.color = new Color(0.3f, 0.5f, 1.0f, 1.0f);
		getDrinkHere1.anchorX = 1;
		getDrinkHere1.x = Futile.screen.width - xAmt;
		getDrinkHere1.y = yAmt;
		player1GetDrinkHelpContainer.AddChild(getDrinkHere1);
		
		FSprite getArrow1 = new FSprite("arrow.psd");
		getArrow1.scale = -1;
		getArrow1.alpha = getDrinkHere1.alpha;
		getArrow1.color = getDrinkHere1.color;
		getArrow1.anchorX = 1;
		getArrow1.anchorY = 0;
		getArrow1.x = getDrinkHere1.x + 10f;
		getArrow1.y = getDrinkHere1.y - 5f;
		player1GetDrinkHelpContainer.AddChild(getArrow1);
		
		FSprite getDrinkHere2 = new FSprite("getDrinkHere2.psd");
		getDrinkHere2.alpha = 0.5f;
		getDrinkHere2.color = new Color(1.0f, 0.3f, 0.5f, 1.0f);
		getDrinkHere2.anchorX = 0;
		getDrinkHere2.x = xAmt;
		getDrinkHere2.y = yAmt;
		player2GetDrinkHelpContainer.AddChild(getDrinkHere2);
		
		FSprite getArrow2 = new FSprite("arrow.psd");
		getArrow2.scaleY = -1;
		getArrow2.alpha = getDrinkHere2.alpha;
		getArrow2.color = getDrinkHere2.color;
		getArrow2.anchorX = 1;
		getArrow2.anchorY = 0;
		getArrow2.x = getDrinkHere2.x - 10f;
		getArrow2.y = getDrinkHere2.y - 5f;
		player2GetDrinkHelpContainer.AddChild(getArrow2);
	}
	
	private void MakeFloor() {
		float boardWidth = 200f;
		float boardHeight = 20f;
		float padding = 2f;
		int boardRowCount = (int)((Futile.screen.height - SBConfig.TOP_UI_HEIGHT) / boardHeight);
		
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
	}
}
